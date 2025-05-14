using System;
using System.Diagnostics;

namespace Misc
{
    public static class Logger
    {
        public enum LogLevel
        {
            Info,
            Warning,
            Error
        }

        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            string levelPrefix = level switch
            {
                LogLevel.Warning => "[WARNING]",
                LogLevel.Error => "[ERROR]",
                _ => "[INFO]"
            };

            Debug.WriteLine($"[{DateTime.Now}] {levelPrefix} {message}");
        }
    }
}