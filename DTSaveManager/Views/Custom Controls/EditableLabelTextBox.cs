using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DTSaveManager.Views.Custom_Controls
{
    internal class EditableLabelTextBox : TextBox
    {
        private TextBoxBase textBoxPart = null;
        public const string TEXT_BOX_TEMPLATE_PART_NAME = "PART_TextBox";

        public bool IsEditing { get; set; } = false;

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public ICommand LostFocusCommand
        {
            get => (ICommand)GetValue(LostFocusCommandProperty);
            set => SetValue(LostFocusCommandProperty, value);
        }

        public object LostFocusCommandParameter
        {
            get => GetValue(LostFocusCommandParameterProperty);
            set => SetValue(LostFocusCommandParameterProperty, value);
        }

        public bool Focused
        {
            get => (bool)GetValue(FocusedProperty);
            set => SetValue(FocusedProperty, value);
        }

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static DependencyProperty CommandProperty =
        DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(EditableLabelTextBox));

        public static DependencyProperty IsEditingProperty =
        DependencyProperty.RegisterAttached(
            "IsEditing",
            typeof(bool),
            typeof(EditableLabelTextBox));

        public static DependencyProperty LostFocusCommandProperty =
        DependencyProperty.Register(
            "LostFocusCommand",
            typeof(ICommand),
            typeof(EditableLabelTextBox));

        public static DependencyProperty LostFocusCommandParameterProperty =
        DependencyProperty.Register(
            "LostFocusCommandParameter",
            typeof(object),
            typeof(EditableLabelTextBox));

        public static DependencyProperty FocusedProperty =
        DependencyProperty.Register(
            "Focused",
            typeof(bool),
            typeof(EditableLabelTextBox),
            new PropertyMetadata(OnFocusedChanged));

        public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(
            "Value",
            typeof(object),
            typeof(EditableLabelTextBox),
            new FrameworkPropertyMetadata(default(object),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static bool GetIsEditing(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsEditingProperty);
        }

        public static void SetIsEditing(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsEditingProperty, value);
        }

        public override void OnApplyTemplate()
        {
            if (textBoxPart != null)
            {
                textBoxPart.LostFocus -= TextBox_LostFocus;
                textBoxPart.GotFocus -= TextBox_GotFocus;
                textBoxPart.KeyDown -= TextBox_KeyDown;
                textBoxPart = null;
            }

            // Find the template child with the special name. It can be any kind
            // of ButtonBase in this example.
            textBoxPart = GetTemplateChild(TEXT_BOX_TEMPLATE_PART_NAME) as TextBox;

            // Add a handler to its Click event that simply forwards it on to our
            // Click event.
            if (textBoxPart != null)
            {
                textBoxPart.LostFocus += TextBox_LostFocus;
                textBoxPart.GotFocus += TextBox_GotFocus;
                textBoxPart.KeyDown += TextBox_KeyDown;
            }
        }

        public static void OnFocusedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            TextBox textBox = (TextBox)(dependencyObject as EditableLabelTextBox).textBoxPart;
            bool newValue = (bool)dependencyPropertyChangedEventArgs.NewValue;
            bool oldValue = (bool)dependencyPropertyChangedEventArgs.OldValue;
            if (newValue && !oldValue && !textBox.IsFocused) textBox.Focus();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsEditing = false;
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!IsEditing)
            {
                if (!e.Handled)
                {
                    IsEditing = true;
                    (this.Template.FindName(TEXT_BOX_TEMPLATE_PART_NAME, this) as TextBox).Focus();
                    (this.Template.FindName(TEXT_BOX_TEMPLATE_PART_NAME, this) as TextBox).CaretIndex = (this.Template.FindName(TEXT_BOX_TEMPLATE_PART_NAME, this) as TextBox).Text.Length;
                }
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(sender as TextBox), null);
                Keyboard.ClearFocus();
            }
        }
    }
}
