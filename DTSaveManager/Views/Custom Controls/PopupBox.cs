using DTSaveManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DTSaveManager.Views.Custom_Controls
{
    public static class PopupBox
    {
        public static MessageBoxResult Show(string popupText, bool showYesNoButtons = false, bool showOKButton = true)
        {
            PopupWindow window = new PopupWindow(popupText, showYesNoButtons, showOKButton);
            var mainWindowLocation = WindowService.GetMainWindowLocation();
            var mainWindowDimensions = WindowService.GetMainWindowDimensions();

            window.Left = mainWindowLocation.X + (mainWindowDimensions.X / 2) - (window.Width / 2);
            window.Top = mainWindowLocation.Y + (mainWindowDimensions.Y / 2) - (window.Height / 2);
            window.ShowDialog();

            return window.Result;
        }
    }
}
