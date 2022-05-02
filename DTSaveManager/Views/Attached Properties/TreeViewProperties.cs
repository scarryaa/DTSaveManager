using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DTSaveManager.Views.Attached_Properties
{
    class TreeViewProperties
    {
        public static DependencyProperty IsFocusedProperty = 
            DependencyProperty.RegisterAttached(
                "IsFocused", typeof(bool), 
                typeof(TreeViewProperties),
                new UIPropertyMetadata(false, OnIsFocusedChanged));

        public static bool GetIsFocused(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsFocusedProperty, value);
        }

        public static void OnIsFocusedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            System.Windows.Controls.TreeView treeView = dependencyObject as System.Windows.Controls.TreeView;
            bool newValue = (bool)dependencyPropertyChangedEventArgs.NewValue;
            bool oldValue = (bool)dependencyPropertyChangedEventArgs.OldValue;
            if (newValue && !oldValue && !treeView.IsFocused) treeView.Focus();
        }
    }
}
