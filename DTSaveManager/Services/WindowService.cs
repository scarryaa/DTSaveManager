using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DTSaveManager.Services
{
    public class WindowService
    {
        private bool MainWindowRendered = false;

        static WindowService() 
        { 
            Instance = new WindowService();
        }

        public static WindowService Instance { get; private set; }

        public Point GetMainWindowLocation()
        {
            if (!Instance.MainWindowRendered) return new Point(0, 0);
            else
            {
                var position = Application.Current.MainWindow.PointToScreen(new Point(0, 0));
                return position;
            }
        }

        public Point GetMainWindowDimensions()
        {
            var dimensions = new Point(Application.Current.MainWindow.Width, Application.Current.MainWindow.Height);
            return dimensions;
        }

        public Point GetMainScreenDimensions()
        {
            return new Point(SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
        }

        public void SetMainWindowIsRendered()
        {
            MainWindowRendered = true;
        }
    }
}
