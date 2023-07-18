using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System.Timers;

namespace BSolutions.SHES.App.ViewModels
{
    public class AppInfoBarViewModel : ObservableRecipient
    {
        private bool _isOpen;
        private InfoBarSeverity _severity = InfoBarSeverity.Informational;
        private string _title;
        private string _message;

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                SetProperty(ref _isOpen, value);
            }
        }

        public InfoBarSeverity Severity
        {
            get { return _severity; }
            set { SetProperty(ref _severity, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
    }
}
