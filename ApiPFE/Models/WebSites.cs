using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPFE.Models
{
    public partial class WebSites
    {
        public WebSites()
        {
            Passwords = new HashSet<Passwords>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Link { get; set; }

        [InverseProperty("IdWsNavigation")]
        public virtual ICollection<Passwords> Passwords { get; set; }
    }
}
