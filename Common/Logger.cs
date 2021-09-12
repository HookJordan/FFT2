using System;
using System.IO;

namespace Common
{
    public enum LogLevel
    {
        Error = 1,
        Info = 2,
        Debug = 3
    }

    public class Logger : IDisposable
    {
        public string FileName { get; private set; }
        public LogLevel LogLevel { get; private set; }
        public bool ConsoleLogging { get; private set; }

        private readonly object _logLock;
        private readonly StreamWriter _streamWriter;
        private bool _disposed;

        // Singleton
        private static Logger _instance;
        public static Logger Get 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                }

                return _instance;
            }
        }

        private Logger()
        {
            LogLevel = LogLevel.Info;
            ConsoleLogging = false;
            _logLock = new object();
            FileName = $"logs/{DateTime.Now:yyyy-MM-dd}.txt";

            if (!Directory.Exists("logs"))
            {
                Directory.CreateDirectory("logs");
            }

            _streamWriter = new StreamWriter(FileName, true);
            _streamWriter.AutoFlush = true;
        }

        public static void SetLogLevel(LogLevel level, bool logToConsole = false)
        {
            Get.LogLevel = level;
            Get.ConsoleLogging = logToConsole;
        }

        public static void Info(string msg)
        {
            if (Get.LogLevel > LogLevel.Error)
            {
                Get.WriteLine($"[INFO] {msg}");
                Get.InfoLog?.Invoke(Get, $"[INFO] {msg}");
            }
        }

        public static void Error(string msg)
        {
            Get.WriteLine($"[ERROR] {msg}");
            Get.ErrorLog?.Invoke(Get, $"[ERROR] {msg}");
        }

        public static void Debug(string msg)
        {
            if (Get.LogLevel == LogLevel.Debug)
            {
                Get.WriteLine($"[DEBUG] {msg}");
                Get.DebugLog?.Invoke(Get, $"[DEBUG] {msg}");
            }
        }

        private void WriteLine(string msg)
        {
            if (!_disposed)
            {
                lock (_logLock)
                {
                    var log = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {msg}";
                    _streamWriter?.WriteLine(log);

                    if (ConsoleLogging)
                    {
                        Console.WriteLine(msg.Substring(msg.IndexOf(']') + 2));
                    }

                    WriteLineEvent?.Invoke(this, log);
                }
            }
            else
            {
                throw new Exception("Cannot write to a disposed logger instance");
            }
        }

        public string GetLogsFolder()
        {
            FileInfo info = new FileInfo(FileName);

            return info.Directory.FullName;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _streamWriter.Dispose();
            }
        }

        public delegate void ErrorLogHandler(Logger logger, string log);
        public event ErrorLogHandler ErrorLog;
        public delegate void InfoLogHandler(Logger logger, string log);
        public event InfoLogHandler InfoLog;
        public delegate void DebugLogHandler(Logger logger, string log);
        public event DebugLogHandler DebugLog;

        public delegate void WriteLineHandler(Logger logger, string line);
        public event WriteLineHandler WriteLineEvent;
    }
}
