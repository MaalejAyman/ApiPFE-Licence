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
            Notes = new HashSet<Notes>();
            Passwords = new HashSet<Passwords>();
            UserssGroupes = new HashSet<UserssGroupes>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [Column("login")]
        [StringLength(50)]
        public string Login { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
        public int? IsAdmin { get; set; }

        [InverseProperty("IdUserNavigation")]
        public virtual ICollection<Folders> Folders { get; set; }
        [InverseProperty("IdUserNavigation")]
        public virtual ICollection<Notes> Notes { get; set; }
        [InverseProperty("IdUserNavigation")]
        public virtual ICollection<Passwords> Passwords { get; set; }
        [InverseProperty("IdUsrNavigation")]
        public virtual ICollection<UserssGroupes> UserssGroupes { get; set; }
    }
}
