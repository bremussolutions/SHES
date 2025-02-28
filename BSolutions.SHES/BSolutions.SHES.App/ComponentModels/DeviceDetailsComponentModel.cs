using BSolutions.SHES.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace BSolutions.SHES.App.ComponentModels
{
    public class DeviceDetailsComponentModel : ObservableRecipient
    {
        public ObservableCollection<FormField> FormFields { get; } = new()
        {
            new FormField("Name", new TextBox()),
            new FormField("Kennzeichen", new TextBox()),
            new FormField("Gerätetyp", new ComboBox()),
            new FormField("Bustyp", new ComboBox()),
            new FormField("Hersteller", new TextBox()),
            new FormField("Bestellnummer", new TextBox()),
            new FormField("Phys. KNX-Adresse", new TextBox()),
            new FormField("", new CheckBox { Content = "Hutschienenmontage" }),
            new FormField("Teilungseinheiten", new ComboBox())
        };

        #region --- Constructor ---

        /// <summary>Initializes a new instance of the <see cref="DeviceDetailsComponentModel" /> class.</summary>
        public DeviceDetailsComponentModel()
        {
        }

        #endregion
    }
}
