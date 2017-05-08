using System;
using System.Configuration;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace UCR.Negotium.Utils
{
    public static class Installation
    {
        private const string dataFolder = "Data";

        public static bool IsInstalled()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return Convert.ToBoolean(config.AppSettings.Settings["Installed"].Value);
        }

        public static bool RunInstallation()
        {
            if (SetGrantAccess())
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["Installed"].Value = "True";
                config.Save(ConfigurationSaveMode.Modified);

                return true;
            }

            return false;
        }

        public static bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static bool SetGrantAccess()
        {
            try
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dataFolder);
                DirectoryInfo dInfo = new DirectoryInfo(fullPath);
                DirectorySecurity dSecurity = dInfo.GetAccessControl();
                dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                dInfo.SetAccessControl(dSecurity);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
