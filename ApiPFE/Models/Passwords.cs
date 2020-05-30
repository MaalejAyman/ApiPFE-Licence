using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPFE.Models
{
    public partial class Passwords
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Login { get; set; }
        [Required]
        [StringLength(50)]
        public string Value { get; set; }
        public int? Score { get; set; }
        public long IdGrp { get; set; }
        public long IdFldr { get; set; }
        [Column("IdWS")]
        public long IdWs { get; set; }
        public long IdUser { get; set; }

        [ForeignKey(nameof(IdFldr))]
        [InverseProperty(nameof(Folders.Passwords))]
        public virtual Folders IdFldrNavigation { get; set; }
        [ForeignKey(nameof(IdGrp))]
        [InverseProperty(nameof(Groupes.Passwords))]
        public virtual Groupes IdGrpNavigation { get; set; }
        [ForeignKey(nameof(IdUser))]
        [InverseProperty(nameof(Userss.Passwords))]
        public virtual Userss IdUserNavigation { get; set; }


        [ForeignKey(nameof(IdWs))]
        [InverseProperty(nameof(WebSites.Passwords))]
        public virtual WebSites IdWsNavigation { get; set; }
    }
}
