using BSolutions.SHES.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BSolutions.SHES.Models.Observables
{
    public abstract class ObservableBase<TEntity> : ObservableValidator where TEntity : EntityBase
    {
        public readonly TEntity entity;

        #region --- Properties ---

        public Guid Id
        {
            get => entity.Id;
            set => SetProperty(entity.Id, value, entity, (u, n) => u.Id = n);
        }

        public DateTime CreationTime
        {
            get => entity.CreationTime;
            set => SetProperty(entity.CreationTime, value, entity, (u, n) => u.CreationTime = n);
        }

        public DateTime? LastModificationTime
        {
            get => entity.LastModificationTime;
            set => SetProperty(entity.LastModificationTime, value, entity, (u, n) => u.LastModificationTime = n);
        }

        public string Errors => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(null) select e.ErrorMessage);

        #endregion

        #region --- Constructor ---

        public ObservableBase(TEntity entity)
        {
            this.entity = entity;

            this.PropertyChanged += StudyGroup_PropertyChanged;
            this.ErrorsChanged += StudyGroup_ErrorsChanged;
        }

        #endregion

        #region --- Events ---

        private void StudyGroup_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(HasErrors))
            {
                OnPropertyChanged(nameof(HasErrors));
            }
        }

        private void StudyGroup_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Errors));
        }

        #endregion
    }
}
