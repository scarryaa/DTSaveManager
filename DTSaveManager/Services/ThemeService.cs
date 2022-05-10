using DTSaveManager.DataTypes.Enums;
using System;
using System.Windows;

namespace DTSaveManager.Services
{
    public static class ThemeService
    {
        private static ThemeType _currentTheme = (ThemeType)Enum.Parse(typeof(ThemeType), ConfigService.GetActiveTheme());
        private static string themePrefix = "pack://application:,,,../../Styles/Themes/";

        public static ResourceDictionary ThemeDictionary
        {
            get { return Application.Current.Resources; }
        }

        public static void ChangeTheme()
        {
            if (_currentTheme == ThemeType.DarkTheme) SetTheme(ThemeType.LightTheme);
            else SetTheme(ThemeType.DarkTheme);
        }

        public static void SetTheme(ThemeType themeType)
        {
            ThemeDictionary.MergedDictionaries.RemoveAt(0);
            ThemeDictionary.MergedDictionaries.Insert(0,
                new ResourceDictionary() { Source = new Uri(themePrefix + (ThemeType)Enum.Parse(typeof(ThemeType), themeType.ToString()) + ".xaml") });
            _currentTheme = themeType;

            ConfigService.SetActiveTheme(themeType);
        }
        public static ThemeType GetCurrentTheme()
        {
            return _currentTheme;
        }

        public static object FindResource(string resourceName)
        {
            return ThemeDictionary.MergedDictionaries[0].Values;
        }
    }
}
