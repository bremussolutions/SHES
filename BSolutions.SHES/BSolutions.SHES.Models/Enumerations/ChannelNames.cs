using System.ComponentModel.DataAnnotations;

namespace BSolutions.SHES.Models.Enumerations
{
    public enum ChannelNames
    {
        [Display(Name = "Unbekannt")]
        Unknown,

        [Display(Name = "Numerisch")]
        Numerical,

        [Display(Name = "Alphabetisch")]
        Alphabetical
    }
}
