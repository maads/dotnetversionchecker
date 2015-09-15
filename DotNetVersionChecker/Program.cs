using System.Text;
using Microsoft.Win32;
using System;

namespace DotNetVersionChecker
{
    class Program
    {
        static void Main(string[] args)
        {

            var version = Get4XVersion();

            if (string.IsNullOrEmpty(version))
            {
                version = GetVersionFromRegistry();
            }
            Console.WriteLine(version);
            Console.ReadKey();
        }

        private static string Get4XVersion()
        {
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
            {
                if (ndpKey == null)
                {
                    return string.Empty;
                }
                int releaseKey = (int)ndpKey.GetValue("Release");
                if (releaseKey == 378389)
                {
                    return ".NET Framework 4.5";
                }
                if (releaseKey == 378675)
                {
                    return ".NET Framework 4.5.1 installed with Windows 8.1";
                }
                if (releaseKey == 378758)
                {
                    return ".NET Framework 4.5.1 installed on Windows 8, Windows 7 SP1, or Windows Vista SP2";
                }
                if (releaseKey == 379893)
                {
                    return ".NET Framework 4.5.2";
                }
                if (releaseKey == 381023)
                {
                    return ".NET Framework shipped with Windows 10 Technical Preview. 1 increment from 4.6 Preview";
                }
                if (releaseKey == 381024)
                {
                    return ".NET Framework 4.6 Preview";
                }
                if (releaseKey == 393295)
                {
                    return ".NET Framework 4.6 on Windows 10";
                }
                if (releaseKey == 393297)
                {
                    return ".NET Framework 4.6";
                }
                if (releaseKey > 393297)
                {
                    return string.Format("A later version than 4.6 ({0})", releaseKey);
                }
                return string.Empty;
            }
        }

        private static string GetVersionFromRegistry()
        {
            var sb = new StringBuilder();
            // Opens the registry key for the .NET Framework entry. 
            using (RegistryKey ndpKey =
                RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").
                OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (versionKeyName.StartsWith("v"))
                    {

                        RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                        if (versionKey == null) continue;
                        string name = (string)versionKey.GetValue("Version", "");
                        string sp = versionKey.GetValue("SP", "").ToString();
                        string install = versionKey.GetValue("Install", "").ToString();
                        if (install == "") //no install info, must be later.
                            sb.AppendLine(versionKeyName + "  " + name);
                        else
                        {
                            if (sp != "" && install == "1")
                            {
                                sb.AppendLine(versionKeyName + "  " + name + "  SP" + sp);
                            }

                        }
                        if (name != "")
                        {
                            continue;
                        }
                        foreach (string subKeyName in versionKey.GetSubKeyNames())
                        {
                            RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                            if (subKey == null) continue;
                            name = (string)subKey.GetValue("Version", "");
                            if (name != "")
                                sp = subKey.GetValue("SP", "").ToString();
                            install = subKey.GetValue("Install", "").ToString();
                            if (install == "") //no install info, must be later.
                                sb.AppendLine(versionKeyName + "  " + name);
                            else
                            {
                                if (sp != "" && install == "1")
                                {
                                    sb.AppendLine("  " + subKeyName + "  " + name + "  SP" + sp);
                                }
                                else if (install == "1")
                                {
                                    sb.AppendLine("  " + subKeyName + "  " + name);
                                }

                            }

                        }

                    }
                }
            }
            return sb.ToString();
        }
    }
}
