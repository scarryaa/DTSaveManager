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
                config.LockedFiles = new List<string>();
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

        public static void LockFile(string fileName)
        {
            config.LockedFiles.Add(fileName);
            SaveConfig();
        }

        public static void UnlockFile(string fileName)
        {
            config.LockedFiles.Remove(fileName);
            SaveConfig();
        }

        public static List<string> GetLockedFiles()
        {
            return config.LockedFiles;
        }

        public static void SetDTSaveDirectory(string directory)
        {
            config.DemonTurfDirectory = directory;
            SaveConfig();
        }

        public static string GetDTSaveDirectory()
        {
            return config.DemonTurfDirectory;
        }

        public static void SetNSSaveDirectory(string directory)
        {
            config.NeonSplashDirectory = directory;
            SaveConfig();
        }

        public static bool? GetNeonSplashDisabled()
        {
            return config.NeonSplashDisabled;
        }

        public static void SetNeonSplashDisabled(bool value)
        {
            config.NeonSplashDisabled = value;
        }

        public static string GetNSSaveDirectory()
        {
            return config.NeonSplashDirectory;
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
