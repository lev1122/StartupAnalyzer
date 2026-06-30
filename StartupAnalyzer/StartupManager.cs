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
                if (item.Source == "HKCU\\Run")
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                    {
                        key?.DeleteValue(item.Name, false);
                    }
                }
                else if (item.Source == "HKLM\\Run")
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                    {
                        key?.DeleteValue(item.Name, false);
                    }
                }
                else if (item.Source == "Папка Startup")
                {
                    if (File.Exists(item.Path))
                    {
                        File.Delete(item.Path);
                    }
                }

                return true; // Возвращаем true, если удаление прошло успешно
            }
            catch (Exception)
            {
                return false; // Возвращаем false, если не хватило прав администратора
            }
        }
    }
}