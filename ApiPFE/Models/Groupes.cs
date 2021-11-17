using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPFE.Models
{
    public partial class Groupes
    {
        public Groupes()
        {
            GroupesPasswords = new HashSet<GroupesPasswords>();
            Passwords = new HashSet<Passwords>();
            UserssGroupes = new HashSet<UserssGroupes>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [InverseProperty("IdGrpNavigation")]
        public virtual ICollection<GroupesPasswords> GroupesPasswords { get; set; }
        [InverseProperty("IdGrpNavigation")]
        public virtual ICollection<Passwords> Passwords { get; set; }
        [InverseProperty("IdGrpNavigation")]
        public virtual ICollection<UserssGroupes> UserssGroupes { get; set; }
    }
}
