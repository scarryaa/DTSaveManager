using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DTSaveManager.Views
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        public MessageBoxResult Result { get; set; }
        public string Message { get; set; }
        public bool ShowYesNoButtons { get; set; }
        public bool ShowOKButton { get; set; }

        public PopupWindow(string message, bool showYesNoButtons = false, bool showOKButton = true)
        {
            InitializeComponent();
            Message = message;
            ShowYesNoButtons = showYesNoButtons;
            ShowOKButton = showOKButton;
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }

        private void OnYesButtonClick(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Yes;
            Close();
        }

        private void OnNoButtonClick(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            Close();
        }

        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }
    }
}
