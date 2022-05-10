using DTSaveManager.DataTypes.Enums;
using DTSaveManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTSaveManager.Services
{
    public static class ConfigService
    {
        private static string _configPath = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "DTSaveManager");
        private static string _configFileName = "Configuration.dtsm";
        private static Config config;

        public static void Initialize()
        {
            config = new Config();
            if (!Directory.Exists(_configPath) || !File.Exists(_configPath + $@"\{_configFileName}"))
            {
                Directory.CreateDirectory(_configPath);
                SetActiveTheme(ThemeType.DarkTheme);
                SetActiveFiles(("DTSaveData.txt", "DTSaveData.txt"));
                SaveConfig();
            }
            else LoadConfig();
        }

        public static void SetActiveFiles((string, string) files)
        {
            config.DtActiveFile = files.Item1;
            config.NeonSplashActiveFile = files.Item2;
            SaveConfig();
        }

        public static (string, string) GetActiveFiles()
        {
            return (config.DtActiveFile, config.NeonSplashActiveFile);
        }

        public static void SetActiveTheme(Enum activeTheme)
        {
            config.ActiveTheme = activeTheme.ToString();
            SaveConfig();
        }

        public static string GetActiveTheme()
        {
            return config.ActiveTheme;
        }

        private static void LoadConfig()
        {
            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(_configPath + $@"\{_configFileName}"));
        }

        private static void SaveConfig()
        {
            File.WriteAllText(_configPath + $@"\{_configFileName}", JsonConvert.SerializeObject(config));
        }
    }
}
