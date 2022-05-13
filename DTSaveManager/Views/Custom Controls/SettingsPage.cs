using DTSaveManager.Services;
using DTSaveManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DTSaveManager.Views.Custom_Controls
{
    public static class SettingsPage
    {
        public static (string, string) Show()
        {
            SettingsWindow window = new SettingsWindow();
            var mainWindowLocation = WindowService.Instance.GetMainWindowLocation();
            var mainWindowDimensions = WindowService.Instance.GetMainWindowDimensions();

            window.Left = mainWindowLocation.X + (mainWindowDimensions.X / 2) - (window.Width / 2);
            window.Top = mainWindowLocation.Y + (mainWindowDimensions.Y / 2) - (window.Height / 2);

            if (mainWindowLocation == new Point(0, 0) || mainWindowDimensions == new Point(0, 0))
            {
                var mainScreenDimensions = WindowService.Instance.GetMainScreenDimensions();
                window.Left = (mainScreenDimensions.X / 2) - (window.Width / 2);
                window.Top = (mainScreenDimensions.Y / 2) - (window.Height / 2);
            }

            window.ShowDialog();

            var result = (window.DataContext as SettingsWindowViewModel);
            return (result.DtSaveDirectory, result.NeonSaveDirectory);
        }
    }
}
