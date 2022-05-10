using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DTSaveManager.Views.Attached_Properties
{
    class ToolTipProperties : DependencyObject
    {
        public static DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached(
                "Text",
                typeof(string),
                typeof(ToolTipProperties));

        public static DependencyProperty ColorProperty =
            DependencyProperty.RegisterAttached(
                "Color",
                typeof(System.Windows.Media.Brush),
                typeof(ToolTipProperties));

        public static DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached(
                "Foreground",
                typeof(System.Windows.Media.Brush),
                typeof(ToolTipProperties));

        public static DependencyProperty PaddingProperty =
            DependencyProperty.RegisterAttached(
                "Padding",
                typeof(Thickness),
                typeof(ToolTipProperties));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        [TypeConverter(typeof(BrushConverter))]
        public System.Windows.Media.Brush Color
        {
            get { return (System.Windows.Media.Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        [TypeConverter(typeof(BrushConverter))]
        public System.Windows.Media.Brush Foreground
        {
            get { return (System.Windows.Media.Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public static string GetText(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(TextProperty, value);
        }

        public static System.Windows.Media.Brush GetColor(DependencyObject dependencyObject)
        {
            return (System.Windows.Media.Brush)dependencyObject.GetValue(ColorProperty);
        }

        public static void SetColor(DependencyObject dependencyObject, System.Windows.Media.Brush value)
        {
            dependencyObject.SetValue(ColorProperty, value);
        }

        public static System.Windows.Media.Brush GetForeground(DependencyObject dependencyObject)
        {
            return (System.Windows.Media.Brush)dependencyObject.GetValue(ForegroundProperty);
        }

        public static void SetForeground(DependencyObject dependencyObject, System.Windows.Media.Brush value)
        {
            dependencyObject.SetValue(ForegroundProperty, value);
        }

        public static Thickness GetPadding(DependencyObject dependencyObject)
        {
            return (Thickness)dependencyObject.GetValue(PaddingProperty);
        }

        public static void SetPadding(DependencyObject dependencyObject, Thickness value)
        {
            dependencyObject.SetValue(PaddingProperty, value);
        }
    }
}
