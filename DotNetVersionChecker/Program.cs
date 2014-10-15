using Microsoft.Win32;
using System;

namespace DotNetVersionChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
      RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
            {
                int releaseKey = (int)ndpKey.GetValue("Release");
                if (releaseKey == 378389)
                {
                    Console.WriteLine(".NET Framework 4.5");
                }
                if (releaseKey == 378675)
                {
                    Console.WriteLine(".NET Framework 4.5.1 installed with Windows 8.1");
                }
                if (releaseKey == 378758)
                {
                    Console.WriteLine(
                        ".NET Framework 4.5.1 installed on Windows 8, Windows 7 SP1, or Windows Vista SP2");
                }
                if (releaseKey == 379893)
                {
                    Console.WriteLine(
                        ".NET Framework 4.5.2");
                }
                if (releaseKey > 379893)
                {
                    Console.WriteLine("A later version than 4.5.2");
                }
            }
            Console.ReadKey();
        }
    }
}
