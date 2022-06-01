using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BSolutions.SHES.Models.Entities
{
    public class ProjectItem : EntityBase
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Comment { get; set; }

        public Guid? ParentId { get; set; }

        public ProjectItem Parent { get; set; }

        public ICollection<ProjectItem> Children { get; set; } = new Collection<ProjectItem>();

        public override string ToString()
        {
            return $"{this.GetType().Name} ({this.Id}): {this.Name}";
        }
    }
}
