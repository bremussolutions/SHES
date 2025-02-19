using Microsoft.UI.Xaml.Controls;

namespace BSolutions.SHES.Controls
{
    public class FormField
    {
        public string Label { get; }
        public Control Control { get; }

        public FormField(string label, Control control)
        {
            Label = label;
            Control = control;
        }
    }
}
