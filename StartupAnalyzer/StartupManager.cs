using System;
using System.IO;
using Microsoft.Win32;

namespace StartupAnalyzer
{
    public class StartupManager
    {
        public bool RemoveItem(StartupItem item)
        {
            try
            {
                if (item.Source.Contains("Папка Startup"))
                {
                    if (File.Exists(item.Path))
                    {
                        File.Delete(item.Path);
                        return true;
                    }
                    return false;
                }

                if (item.Source == "Служба (Авто)")
                {
                    using (RegistryKey servicesKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services", true))
                    {
                        if (servicesKey != null)
                        {
                            using (RegistryKey serviceKey = servicesKey.OpenSubKey(item.Name, true))
                            {
                                if (serviceKey != null)
                                {
                                    serviceKey.SetValue("Start", 4, RegistryValueKind.DWord);
                                    return true;
                                }
                            }
                        }
                    }
                    return false;
                }

                RegistryKey rootKey = Registry.CurrentUser;
                string registryPath = "";

                if (item.Source.StartsWith("HKCU"))
                {
                    rootKey = Registry.CurrentUser;
                    if (item.Source.Contains("RunOnce"))
                        registryPath = @"Software\Microsoft\Windows\CurrentVersion\RunOnce";
                    else
                        registryPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
                }
                else if (item.Source.StartsWith("HKLM"))
                {
                    rootKey = Registry.LocalMachine;
                    if (item.Source.Contains("WOW6432Node"))
                        registryPath = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run";
                    else if (item.Source.Contains("RunOnce"))
                        registryPath = @"Software\Microsoft\Windows\CurrentVersion\RunOnce";
                    else
                        registryPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
                }

                using (RegistryKey key = rootKey.OpenSubKey(registryPath, true))
                {
                    if (key != null)
                    {
                        key.DeleteValue(item.Name, false);
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}