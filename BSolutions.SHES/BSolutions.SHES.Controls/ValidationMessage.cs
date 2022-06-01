using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BSolutions.SHES.Controls
{
    public sealed class ValidationMessage : Control
    {
        public Visibility IsVisible
        {
            get => (Visibility)GetValue(IsVisibleProperty);
            private set => SetValue(IsVisibleProperty, value);
        }

        public string Errors
        {
            get => (string)GetValue(ErrorsProperty);
            set => SetValue(ErrorsProperty, value);
        }

        public static readonly DependencyProperty ErrorsProperty =
        DependencyProperty.Register(
            nameof(Errors),
            typeof(string),
            typeof(ValidationMessage),
            new PropertyMetadata(default(string), new PropertyChangedCallback(OnErrorsChanged)));

        public static readonly DependencyProperty IsVisibleProperty =
        DependencyProperty.Register(
            nameof(IsVisible),
            typeof(Visibility),
            typeof(ValidationMessage),
            new PropertyMetadata(null));

        public ValidationMessage()
        {
            this.DefaultStyleKey = typeof(ValidationMessage);
        }

        private static void OnErrorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ValidationMessage control = d as ValidationMessage; //null checks omitted
            String s = e.NewValue as String; //null checks omitted
            if (s == String.Empty)
            {
                control.IsVisible = Visibility.Collapsed;
            }
            else
            {
                control.IsVisible = Visibility.Visible;
            }
        }
    }
}
