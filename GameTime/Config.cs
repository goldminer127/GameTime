using System;
using System.IO;
using System.Text.Json;

namespace GameTime
{
    public class Config
    {
        private static string path = "config.json";
        public ulong HomeGuildId { get; set; }
        public string Token { get; set; }
        public string Prefix { get; set; }
        public Config()
        {

        }
        public Config(string token, string prefix, string id, bool isTest = false)
        {
            Token = token;
            Prefix = prefix;
            HomeGuildId = UInt64.Parse(id);
            if(isTest)
            {
                path = "testconfig.json";
            }
        }
        public static Config LoadConfig()
        {
            try
            {
                var json = File.ReadAllText(path);
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
            File.WriteAllText(path, json);
        }
        public bool HasToken()
        {
            return String.IsNullOrEmpty(Token);
        }
    }
}
