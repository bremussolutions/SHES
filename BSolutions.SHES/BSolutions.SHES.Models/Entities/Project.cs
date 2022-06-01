using BSolutions.SHES.Models.Enumerations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BSolutions.SHES.Models.Entities
{
    public class Project : EntityBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Number { get; set; }

        public string ClientFirstName { get; set; }

        public string ClientSurname { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string ClientPhone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string ClientEmail { get; set; }

        public string ConstructionStreet { get; set; }

        [DataType(DataType.PostalCode)]
        public string ConstructionPostalCode { get; set; }

        public string ConstructionCity { get; set; }

        public ItemState State { get; set; }

        public ICollection<Building> Buildings { get; set; } = new Collection<Building>();

        public Project()
        {
            this.State = ItemState.Planning;
        }

        public override string ToString()
        {
            return $"Project ({this.Id}): {this.Number} | {this.Name}";
        }
    }
}
