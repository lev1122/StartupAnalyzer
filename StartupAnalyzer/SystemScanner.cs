using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;

namespace StartupAnalyzer
{
    public class SystemScanner
    {
        public List<StartupItem> GetStartupItems()
        {
            var items = new List<StartupItem>();

            ScanRegistryKey(Registry.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\Run", "HKCU\\Run", items);

            ScanRegistryKey(Registry.LocalMachine, @"Software\Microsoft\Windows\CurrentVersion\Run", "HKLM\\Run", items);

            ScanStartupFolder(items);

            return items;
        }

        private void ScanRegistryKey(RegistryKey rootKey, string path, string sourceName, List<StartupItem> items)
        {
            try
            {
                using (RegistryKey key = rootKey.OpenSubKey(path))
                {
                    if (key != null)
                    {
                        foreach (string valueName in key.GetValueNames())
                        {
                            items.Add(new StartupItem
                            {
                                Name = valueName,
                                Path = key.GetValue(valueName)?.ToString(),
                                Source = sourceName,
                                IsActive = true
                            });
                        }
                    }
                }
            }
            catch { }
        }

        private void ScanStartupFolder(List<StartupItem> items)
        {
            try
            {
                string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                if (Directory.Exists(startupPath))
                {
                    foreach (string file in Directory.GetFiles(startupPath))
                    {
                        items.Add(new StartupItem
                        {
                            Name = Path.GetFileNameWithoutExtension(file),
                            Path = file,
                            Source = "Папка Startup",
                            IsActive = true
                        });
                    }
                }
            }
            catch { }
        }
    }
}