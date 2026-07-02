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

            ScanRegistryKey(Registry.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\RunOnce", "HKCU\\RunOnce", items);
            ScanRegistryKey(Registry.LocalMachine, @"Software\Microsoft\Windows\CurrentVersion\RunOnce", "HKLM\\RunOnce", items);

            ScanRegistryKey(Registry.LocalMachine, @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run", "HKLM\\WOW6432Node", items);

            ScanStartupFolder(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "Папка Startup (User)", items);
            ScanStartupFolder(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup), "Папка Startup (All)", items);

            ScanSystemServices(items);

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

        private void ScanStartupFolder(string startupPath, string sourceName, List<StartupItem> items)
        {
            try
            {
                if (Directory.Exists(startupPath))
                {
                    foreach (string file in Directory.GetFiles(startupPath))
                    {
                        items.Add(new StartupItem
                        {
                            Name = Path.GetFileNameWithoutExtension(file),
                            Path = file,
                            Source = sourceName,
                            IsActive = true
                        });
                    }
                }
            }
            catch { }
        }

        private void ScanSystemServices(List<StartupItem> items)
        {
            try
            {
                using (RegistryKey servicesKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services"))
                {
                    if (servicesKey != null)
                    {
                        foreach (string serviceName in servicesKey.GetSubKeyNames())
                        {
                            using (RegistryKey serviceKey = servicesKey.OpenSubKey(serviceName))
                            {
                                if (serviceKey != null)
                                {
                                    // Читаем тип запуска службы: 2 = Автоматически, 3 = Вручную, 4 = Отключено
                                    object startValue = serviceKey.GetValue("Start");

                                    if (startValue != null && (int)startValue == 2)
                                    {
                                        string imagePath = serviceKey.GetValue("ImagePath")?.ToString();

                                        items.Add(new StartupItem
                                        {
                                            Name = serviceName,
                                            Path = imagePath,
                                            Source = "Служба (Авто)",
                                            IsActive = true
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }
    }
}