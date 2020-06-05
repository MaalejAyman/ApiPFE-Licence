using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPFE.Model
{
    public partial class Groupes
    {
        public Groupes()
        {
            Passwords = new HashSet<Passwords>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public long? IdUser { get; set; }

        [ForeignKey(nameof(IdUser))]
        [InverseProperty(nameof(Userss.Groupes))]
        public virtual Userss IdUserNavigation { get; set; }
        [InverseProperty("IdGrpNavigation")]
        public virtual ICollection<Passwords> Passwords { get; set; }
    }
}
