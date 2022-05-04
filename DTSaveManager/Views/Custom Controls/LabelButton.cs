using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DTSaveManager.Views.Custom_Controls
{
    public class LabelButton : Button
    {

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(
                "LabelText",
                typeof(object),
                typeof(LabelButton));

        public static readonly DependencyProperty ButtonTextProperty =
          DependencyProperty.Register(
                "ButtonText",
                typeof(object),
                typeof(LabelButton));

        public static readonly DependencyProperty ButtonColorProperty =
          DependencyProperty.Register(
                "ButtonColor",
                typeof(object),
                typeof(LabelButton));

        public new static DependencyProperty CommandProperty =
            DependencyProperty.Register(
                "Command",
                typeof(ICommand),
                typeof(LabelButton));

        public new static DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                "CommandParameter",
                typeof(object),
                typeof(LabelButton));


        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        public Color ButtonColor
        {
            get => (Color)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        public new ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public new object CommandParameter
        {
            get => GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
    }
}
