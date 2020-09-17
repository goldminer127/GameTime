using System;
using System.IO;
using System.Text.Json;

namespace GameTime
{
    public class Config
    {
        private static string configPath = "config.json";
        public string Token { get; set; }
        public string Prefix { get; set; }
        public Config()
        {
            Token = "";
            Prefix = "g/";
        }
        public Config(string token, string prefix)
        {
            Token = token;
            Prefix = prefix;
        }
        public static Config LoadConfig()
        {
            try
            {
                var json = File.ReadAllText(configPath);
                return JsonSerializer.Deserialize<Config>(json);
            }
            catch
            {
                return null;
            }
        }
        public static void SaveConfig(Config config)
        {
            var json = JsonSerializer.Serialize<Config>(config);
            File.WriteAllText(configPath, json);
        }

        public bool IsEmpty()
        {
            return String.IsNullOrEmpty(Token);
        }
    }
}
