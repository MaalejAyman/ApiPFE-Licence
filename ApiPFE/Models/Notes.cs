using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPFE.Models
{
    public partial class Notes
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Text { get; set; }
        public long IdUser { get; set; }

        [ForeignKey(nameof(IdUser))]
        [InverseProperty(nameof(Userss.Notes))]
        public virtual Userss IdUserNavigation { get; set; }
    }
}
