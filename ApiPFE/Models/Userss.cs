using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPFE.Models
{
    public partial class Userss
    {
        public Userss()
        {
            Folders = new HashSet<Folders>();
            Groupes = new HashSet<Groupes>();
            Passwords = new HashSet<Passwords>();
            UserssGroupes = new HashSet<UserssGroupes>();
            WebSites = new HashSet<WebSites>();
        }

        [Key]
        public long Id { get; set; }
        [Column("login")]
        [StringLength(50)]
        public string Login { get; set; }
        [StringLength(50)]
        public string Password { get; set; }

        [InverseProperty("IdUserNavigation")]
        public virtual ICollection<Folders> Folders { get; set; }
        [InverseProperty("IdUserNavigation")]
        public virtual ICollection<Groupes> Groupes { get; set; }
        [InverseProperty("IdUserNavigation")]
        public virtual ICollection<Passwords> Passwords { get; set; }
        [InverseProperty("IdUsrNavigation")]
        public virtual ICollection<UserssGroupes> UserssGroupes { get; set; }
        [InverseProperty("IdUserNavigation")]
        public virtual ICollection<WebSites> WebSites { get; set; }
    }
}
