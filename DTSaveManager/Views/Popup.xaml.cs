using DTSaveManager.Services;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DTSaveManager.Views
{
    /// <summary>
    /// Interaction logic for Popup.xaml
    /// </summary>
    public partial class Popup : UserControl
    {
        public Popup()
        {
            InitializeComponent();
        }

        public static DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached(
            "Text",
            typeof(string),
            typeof(Popup));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static DependencyProperty PopupPaddingProperty =
        DependencyProperty.RegisterAttached(
            "PopupPadding",
            typeof(Thickness),
            typeof(Popup));

        public Thickness PopupPadding
        {
            get { return (Thickness)GetValue(PopupPaddingProperty); }
            set { SetValue(PopupPaddingProperty, value); }
        }

        public static DependencyProperty ShowTailProperty =
        DependencyProperty.RegisterAttached(
            "ShowTail",
            typeof(bool),
            typeof(Popup),
            new PropertyMetadata(true));

        public bool ShowTail
        {
            get { return (bool)GetValue(ShowTailProperty); }
            set { SetValue(ShowTailProperty, value); }
        }

        public static DependencyProperty PopupColorProperty =
        DependencyProperty.RegisterAttached(
            "PopupColor",
            typeof(Brush),
            typeof(Popup));

        public Brush PopupColor
        {
            get { return (Brush)GetValue(PopupColorProperty); }
            set { SetValue(PopupColorProperty, value); }
        }

        public static DependencyProperty PopupForegroundProperty =
        DependencyProperty.RegisterAttached(
            "PopupForeground",
            typeof(Brush),
            typeof(Popup));

        public Brush PopupForeground
        {
            get { return (Brush)GetValue(PopupForegroundProperty); }
            set { SetValue(PopupForegroundProperty, value); }
        }
    }
}
