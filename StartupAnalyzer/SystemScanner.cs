using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace StartupAnalyzer
{
    public class SystemScanner
    {
        public List<StartupItem> GetStartupItems()
        {
            var items = new List<StartupItem>();

            string registryPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath))
            {
                if (key != null)
                {
                    foreach (string valueName in key.GetValueNames())
                    {
                        string filePath = key.GetValue(valueName)?.ToString();

                        var item = new StartupItem
                        {
                            Name = valueName,
                            Path = filePath,
                            Source = "HKCU\\Run",
                            IsActive = true
                        };

                        items.Add(item);
                    }
                }
            }

            return items;
        }
    }
}
