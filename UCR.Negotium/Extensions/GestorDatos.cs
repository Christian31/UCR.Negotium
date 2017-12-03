using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Threading;
using System.Xml;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain.Tracing;

namespace UCR.Negotium.Extensions
{
    public static class GestorDatos
    {
        public static string ImportarDatos()
        {
            XmlDocument xdocBackup = new XmlDocument();
            XmlElement rootElement = xdocBackup.CreateElement("backup");
            XmlAttribute versionAtr = xdocBackup.CreateAttribute("version");
            versionAtr.Value = GetCurrentVersion();
            rootElement.Attributes.Append(versionAtr);
            rootElement.InnerText = GetBase64StringDb();
            xdocBackup.AppendChild(rootElement);

            string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = string.Format("NegotiumBackup_{0}.bak", DateTime.Now.ToString("yyyyMMddHHmmss"));
            string filePath = Path.Combine(desktopFolder, fileName);
            xdocBackup.Save(filePath);

            return fileName;
        }

        public static bool ValidateBackup(string backupContent)
        {
            try
            {
                XmlDocument xdocBackup = new XmlDocument();
                xdocBackup.LoadXml(backupContent);

                string backupVersion = xdocBackup.FirstChild.Attributes["version"].Value;
                return !CompareVersion(backupVersion).Equals(-1) && xdocBackup.FirstChild.FirstChild != null;
            }
            catch { return false; }
        }

        public static bool ExportarDatos(string backupContent)
        {
            try
            {
                XmlDocument xdocBackup = new XmlDocument();
                xdocBackup.LoadXml(backupContent);

                string backupVersion = xdocBackup.FirstChild.Attributes["version"].Value;
                int result = CompareVersion(backupVersion);
                byte[] bytes = Convert.FromBase64String(xdocBackup.FirstChild.InnerText);

                string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data");
                string tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\_temp");
                string backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\_backup");
                string dbName = "NegotiumDatabase.db";
                string tempdbPath = @"Data\_temp\" + dbName;
                string dbPath = @"Data\" + dbName;

                if (result > -1)
                {
                    #region AddToTask

                    const int numberOfRetries = 3;
                    const int delayOnRetry = 5000;

                    #region backupDB
                    if (!Directory.Exists(backupPath))
                        Directory.CreateDirectory(backupPath);
                    
                    File.Copy(Path.Combine(dataPath, dbName), Path.Combine(backupPath, dbName), true);
                    #endregion

                    #region temporalDB
                    if (!Directory.Exists(tempPath))
                        Directory.CreateDirectory(tempPath);
                    else if (File.Exists(Path.Combine(tempPath, dbName)))
                        File.Delete(Path.Combine(tempPath, dbName));

                    using (FileStream fs = File.Create(Path.Combine(tempPath, dbName)))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }
                    #endregion

                    try
                    {
                        MergeProyectos(dbPath, tempdbPath);
                    }
                    catch (Exception ex)
                    {
                        //rollback backupDB
                        for (int i = 1; i <= numberOfRetries; ++i)
                        {
                            try
                            {
                                File.Copy(Path.Combine(backupPath, dbName), Path.Combine(dataPath, dbName), true);
                                break;
                            }
                            catch (IOException e)
                            {
                                if (i == numberOfRetries)
                                    throw new AggregateException(ex, e);

                                Thread.Sleep(delayOnRetry);
                            }
                        }
                        throw ex;
                    }
                    #endregion

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                ex.TraceExceptionAsync();
                return false;
            }
        }

        private static string GetCurrentVersion()
        {
            return System.Configuration.ConfigurationManager.AppSettings["versionDb"];
        }

        private static string GetBase64StringDb()
        {
            string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\NegotiumDatabase.db");
            byte[] bytes;

            using (BinaryReader binReader = new BinaryReader(File.Open(databasePath, FileMode.Open)))
            {
                bytes = binReader.GetAllBytes();
            }

            return Convert.ToBase64String(bytes);
        }

        private static byte[] GetAllBytes(this BinaryReader reader)
        {
            int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);

                return ms.ToArray();
            }
        }

        private static int CompareVersion(string backupVersion)
        {
            Version backup = new Version(backupVersion);
            Version current = new Version(GetCurrentVersion());
            return backup.CompareTo(current);
        }

        private static void MergeProyectos(string dbName, string tempdbName)
        {
            ExportarData exportarData = new ExportarData();
            Dictionary<Guid, int> indicesdb = exportarData.GetIndicesProyecto(dbName);
            Dictionary<Guid, int> indicesdbTemp = exportarData.GetIndicesProyecto(tempdbName);

            foreach (var dicEntry in indicesdbTemp)
            {
                OrderedDictionary tablesData = exportarData.GetProyecto(tempdbName, dicEntry.Value);

                if (indicesdb.ContainsKey(dicEntry.Key))
                    exportarData.EditarProyecto(dbName, dicEntry.Value, tablesData);
                else
                    exportarData.InsertarProyecto(dbName, tablesData, dicEntry.Key);
            }
        }
    }
}
