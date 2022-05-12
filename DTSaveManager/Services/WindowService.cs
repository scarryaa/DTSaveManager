using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DTSaveManager.Services
{
    public static class WindowService
    {
        public static Point GetMainWindowLocation()
        {
            var position = Application.Current.MainWindow.PointToScreen(new Point(0, 0));
            return position;
        }

        public static Point GetMainWindowDimensions()
        {
            var dimensions = new Point(Application.Current.MainWindow.Width, Application.Current.MainWindow.Height);
            return dimensions;
        }
    }
}
