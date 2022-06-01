using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace BSolutions.SHES.Models.Observables
{
    public class ObservableProject : ObservableBase<Project>
    {
        #region --- Properties ---

        [Required]
        public string Name
        {
            get => entity.Name;
            set => SetProperty(entity.Name, value, entity, (u, n) => u.Name = n, true);
        }

        [Required]
        public string Number
        {
            get => entity.Number;
            set => SetProperty(entity.Number, value, entity, (u, n) => u.Number = n, true);
        }

        public string ClientFirstName
        {
            get => entity.ClientFirstName;
            set => SetProperty(entity.ClientFirstName, value, entity, (u, n) => u.ClientFirstName = n);
        }

        public string ClientSurname
        {
            get => entity.ClientSurname;
            set => SetProperty(entity.ClientSurname, value, entity, (u, n) => u.ClientSurname = n);
        }

        public string ClientPhone
        {
            get => entity.ClientPhone;
            set => SetProperty(entity.ClientPhone, value, entity, (u, n) => u.ClientPhone = n);
        }

        public string ClientEmail
        {
            get => entity.ClientEmail;
            set => SetProperty(entity.ClientEmail, value, entity, (u, n) => u.ClientEmail = n);
        }

        public string ConstructionStreet
        {
            get => entity.ConstructionStreet;
            set => SetProperty(entity.ConstructionStreet, value, entity, (u, n) => u.ConstructionStreet = n);
        }

        public string ConstructionPostalCode
        {
            get => entity.ConstructionPostalCode;
            set => SetProperty(entity.ConstructionPostalCode, value, entity, (u, n) => u.ConstructionPostalCode = n);
        }

        public string ConstructionCity
        {
            get => entity.ConstructionCity;
            set => SetProperty(entity.ConstructionCity, value, entity, (u, n) => u.ConstructionCity = n);
        }

        public ItemState State
        {
            get => entity.State;
            set => SetProperty(entity.State, value, entity, (u, n) => u.State = n);
        }

        #endregion

        #region --- Constructors ---

        public ObservableProject()
            : this(new Project())
        { }

        public ObservableProject(Project project)
            : base(project)
        { }

        #endregion
    }
}
