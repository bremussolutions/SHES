using BSolutions.SHES.Models.Attributes;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Shared.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BSolutions.SHES.Models.Observables
{
    public class ObservableProjectItem : ObservableBase<ProjectItem>
    {
        #region --- Properties ---

        public string Name
        {
            get => entity.Name;
            set => SetProperty(entity.Name, value, entity, (u, n) => u.Name = n);
        }

        private string _icon;
        public string Icon
        {
            get => _icon;
        }

        public string Description
        {
            get => entity.Description;
            set => SetProperty(entity.Description, value, entity, (u, n) => u.Description = n);
        }

        public string Comment
        {
            get => entity.Comment;
            set => SetProperty(entity.Comment, value, entity, (u, n) => u.Comment = n);
        }

        public ObservableCollection<ObservableProjectItem> Children = new ObservableCollection<ObservableProjectItem>();

        public ObservableProjectItem Parent
        {
            get => new ObservableProjectItem(entity.Parent);
            set => SetProperty(entity.Parent, value.entity, entity, (u, n) => u.Parent = n);
        }

        #endregion

        #region --- Constructors ---

        public ObservableProjectItem()
            : this(new ProjectItem())
        { }

        public ObservableProjectItem(ProjectItem projectItem)
            : base(projectItem)
        {
            var attribute = projectItem.GetType().GetCustomAttributes(typeof(ProjectItemInfoAttribute), false).FirstOrDefault() as ProjectItemInfoAttribute;
            
            if(attribute != null)
            {
                this._icon = attribute.Icon;
            }

            this.Children.AddRange(projectItem.Children.Select(pi => new ObservableProjectItem(pi)));
        }

        #endregion
    }
}
