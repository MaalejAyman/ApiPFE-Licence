﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPFE.Model
{
    public partial class Userss
    {
        public Userss()
        {
            Folders = new HashSet<Folders>();
            Groupes = new HashSet<Groupes>();
            Passwords = new HashSet<Passwords>();
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
        [InverseProperty("IdUserNavigation")]
        public virtual ICollection<WebSites> WebSites { get; set; }
    }
}