using System;
using System.IO;

namespace StartupAnalyzer
{
    public class Logger
    {
        private readonly string logFilePath = "audit_logs.txt";

        public void LogAction(string actionType, string targetName)
        {
            try
            {
                string logLine = $"[{DateTime.Now}] {actionType} | {targetName}";

                File.AppendAllText(logFilePath, logLine + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка записи лога: " + ex.Message);
            }
        }
    }
}