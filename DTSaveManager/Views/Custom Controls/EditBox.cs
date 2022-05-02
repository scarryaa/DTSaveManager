﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DTSaveManager
{
    public class EditBox : Label
    {
        private TextBoxBase textBoxPart = null;
        public const string TEXT_BOX_TEMPLATE_PART_NAME = "PART_TextBoxPart";

        public bool IsEditing { get; set; } = false;
        public bool CanBeEdited { get; set; } = true;

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(object),
                typeof(EditBox));

        public static readonly DependencyProperty ReadOnlyProperty =
            DependencyProperty.Register(
                "ReadOnly",
                typeof(bool),
                typeof(EditBox),
                new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register(
                "IsEditing",
                typeof(bool),
                typeof(EditBox),
                new FrameworkPropertyMetadata(false));

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public bool IsReadOnly
        {
            get => (bool)GetValue(ReadOnlyProperty);
            set => SetValue(ReadOnlyProperty, value);
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

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (!IsEditing)
            {
                if (!e.Handled && CanBeEdited)
                {
                    IsReadOnly = false;
                    IsEditing = true;
                    (this.Template.FindName(TEXT_BOX_TEMPLATE_PART_NAME, this) as TextBox).Focus();
                    (this.Template.FindName(TEXT_BOX_TEMPLATE_PART_NAME, this) as TextBox).SelectAll();
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsReadOnly = true;
            IsEditing = false;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!IsEditing)
            {
                if (!e.Handled && CanBeEdited)
                {
                    IsReadOnly = false;
                    IsEditing = true;
                    (this.Template.FindName(TEXT_BOX_TEMPLATE_PART_NAME, this) as TextBox).Focus();
                    (this.Template.FindName(TEXT_BOX_TEMPLATE_PART_NAME, this) as TextBox).SelectAll();
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
