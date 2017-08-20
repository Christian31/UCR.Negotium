using System;
using System.IO;
using System.Xml;

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

                string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\NegotiumDatabase.db");
                string tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\_temp");
                string databaseTempPath = Path.Combine(tempPath, "NegotiumDatabase.db");

                if (result.Equals(0))
                {
                    if (!Directory.Exists(tempPath))
                        Directory.CreateDirectory(tempPath);

                    File.Copy(databasePath, databaseTempPath, true);

                    using (FileStream output = new FileStream(databasePath, 
                        FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                    {
                        output.Write(bytes, 0, bytes.Length);
                        output.Flush();
                        output.Close();
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex) { return false; }
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


    }
}
