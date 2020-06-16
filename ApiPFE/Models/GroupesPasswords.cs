using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPFE.Models
{
    public partial class GroupesPasswords
    {
        [Key]
        public long IdGrp { get; set; }
        [Key]
        public long IdPass { get; set; }

        [ForeignKey(nameof(IdGrp))]
        [InverseProperty(nameof(Groupes.GroupesPasswords))]
        public virtual Groupes IdGrpNavigation { get; set; }
        [ForeignKey(nameof(IdPass))]
        [InverseProperty(nameof(Passwords.GroupesPasswords))]
        public virtual Passwords IdPassNavigation { get; set; }
    }
}
