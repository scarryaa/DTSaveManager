using System;
using System.Windows;

namespace DTSaveManager.Services
{
    public static class ThemeService
    {
        public static ResourceDictionary ThemeDictionary
        {
            // You could probably get it via its name with some query logic as well.
            get { return Application.Current.Resources; }
        }

        public static void ChangeTheme()
        {
            if (ThemeDictionary.MergedDictionaries[0].Source.OriginalString == "Styles/Themes/DarkDTTheme.xaml" || 
                ThemeDictionary.MergedDictionaries[0].Source.OriginalString == "pack://application:,,,../../Styles/Themes/DarkDTTheme.xaml")
            {
                ThemeDictionary.MergedDictionaries.RemoveAt(0);
                ThemeDictionary.MergedDictionaries.Insert(0, new ResourceDictionary() { Source = new Uri("pack://application:,,,../../Styles/Themes/LightDTTheme.xaml") });
            }
            else
            {
                ThemeDictionary.MergedDictionaries.RemoveAt(0);
                ThemeDictionary.MergedDictionaries.Insert(0, new ResourceDictionary() { Source = new Uri("pack://application:,,,../../Styles/Themes/DarkDTTheme.xaml") });
            }
        }
    }
}
