using Common.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Common
{
    public class Configuration
    {
        public int Port { get; set; }
        public long MaxBufferSize { get; set; }

        public CryptoServiceAlgorithm CryptoServiceAlgorithm { get; set; }
        public LogLevel LogLevel { get; set; }

        public List<string> ProtectedDirectories { get; set; }
        public string Password { get; set; }
        public bool StartWithWindows { get; set; }
            
        public Configuration()
        {
            // Defaults
            Port = 15056;
            MaxBufferSize = 1024 * 1024; // 1 MB
            CryptoServiceAlgorithm = CryptoServiceAlgorithm.AES;

            ProtectedDirectories = new List<string>();
            ProtectedDirectories.AddRange(new string[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.Windows)
            });

            Password = "https://jordanhook.com";

            LogLevel = LogLevel.Info;
            StartWithWindows = false;
        }

        public void Save(string path, bool encrypted = false)
        {
            var json = JsonSerializer.Serialize<Configuration>(this);

            // TODO: encrypt
            File.WriteAllText(path, json);
        }

        public static Configuration FromFile(string path, bool encrypted = false)
        {
            var config = new Configuration();

            // If no config exists, create it
            if (!File.Exists(path))
            {
                config.Save(path);
            }
            else
            {
                // TODO: Decrypt???
                string raw = File.ReadAllText(path);
                config = JsonSerializer.Deserialize<Configuration>(raw);
            }

            return config;
        }
    }
}
