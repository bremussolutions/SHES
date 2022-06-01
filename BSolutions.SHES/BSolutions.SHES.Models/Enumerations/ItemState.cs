using System.ComponentModel.DataAnnotations;

namespace BSolutions.SHES.Models.Enumerations
{
    public enum ItemState
    {
        [Display(Name = "Unbekannt")]
        Unknown,

        [Display(Name = "In Planung")]
        Planning,

        [Display(Name = "In Bearbeitung")]
        Implementation,

        [Display(Name = "Inbetriebnahme")]
        Commissioning,

        [Display(Name = "Test")]
        Testing,

        [Display(Name = "Abgeschlossen")]
        Completed
    }
}
