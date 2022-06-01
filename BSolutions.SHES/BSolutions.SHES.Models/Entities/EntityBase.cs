using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSolutions.SHES.Models.Entities
{
    public abstract class EntityBase
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public EntityBase()
        {
            this.Id = Guid.NewGuid();
            this.CreationTime = DateTime.Now;
        }
    }
}
