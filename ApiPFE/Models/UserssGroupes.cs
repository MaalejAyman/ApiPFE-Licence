using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPFE.Models
{
    public partial class UserssGroupes
    {
        [Key]
        public long IdGrp { get; set; }
        [Key]
        public long IdUsr { get; set; }

        [ForeignKey(nameof(IdGrp))]
        [InverseProperty(nameof(Groupes.UserssGroupes))]
        public virtual Groupes IdGrpNavigation { get; set; }
        [ForeignKey(nameof(IdUsr))]
        [InverseProperty(nameof(Userss.UserssGroupes))]
        public virtual Userss IdUsrNavigation { get; set; }
    }
}
