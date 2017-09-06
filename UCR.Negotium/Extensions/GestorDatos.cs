using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UCR.Negotium.DataAccess;

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

                if (result.Equals(0))
                {
                    if (!Directory.Exists(backupPath))
                        Directory.CreateDirectory(backupPath);

                    //creacion de backup de la base de datos actual
                    File.Copy(Path.Combine(dataPath, dbName), Path.Combine(backupPath, dbName), true);

                    //creacion de la base de datos temporal sacada del export
                    if (!Directory.Exists(tempPath))
                        Directory.CreateDirectory(tempPath);

                    if (File.Exists(Path.Combine(tempPath, dbName)))
                        File.Delete(Path.Combine(tempPath, dbName));

                    using (FileStream fs = File.Create(Path.Combine(tempPath, dbName)))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }

                    if (MergeProyectos(Path.Combine(dataPath, dbName), Path.Combine(tempPath, dbName)))
                    {

                    }

                    return true;
                }

                return false;
            }
            catch { return false; }
        }

        private static string GetCurrentVersion()
        {
            return System.Configuration.ConfigurationManager.AppSettings["versionDb"];
        }

        private static string GetBase64StringDb()
        {
            string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\NegotiumDatabase.db");
            var streamFile = File.Open(databasePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                streamFile.CopyTo(ms);
                bytes = ms.ToArray();
            }

            return Convert.ToBase64String(bytes);
        }

        private static int CompareVersion(string backupVersion)
        {
            Version backup = new Version(backupVersion);
            Version current = new Version(GetCurrentVersion());
            return backup.CompareTo(current);
        }

        private static bool MergeProyectos(string dbName, string tempdbName)
        {
            ExportarData exportarData = new ExportarData();
            Dictionary<Guid, int> indicesdb = exportarData.GetIndicesProyecto(dbName);
            Dictionary<Guid, int> indicesdbTemp = exportarData.GetIndicesProyecto(tempdbName);
            Dictionary<Guid, int> indicesNuevos = new Dictionary<Guid, int>();
            Dictionary<Guid, int> indicesEditar = new Dictionary<Guid, int>();

            foreach (var dicEntry in indicesdbTemp)
            {
                if (indicesdb.ContainsKey(dicEntry.Key))
                    indicesEditar.Add(dicEntry.Key, dicEntry.Value);
                else
                    indicesNuevos.Add(dicEntry.Key, dicEntry.Value);
            }

            return AddProyectos(indicesNuevos) && EditProyectos(indicesEditar);
        }

        private static bool AddProyectos(Dictionary<Guid, int> indicesNuevos)
        {
            foreach (var dicEntry in indicesNuevos)
            {

            }

            return true;
        }

        private static bool EditProyectos(Dictionary<Guid, int> indicesEditar)
        {
            foreach (var dicEntry in indicesEditar)
            {

            }

            return true;
        }
    }
}
