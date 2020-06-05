using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPFE.Model
{
    public partial class Folders
    {
        public Folders()
        {
            InverseIdParentFolderNavigation = new HashSet<Folders>();
            Passwords = new HashSet<Passwords>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public long? IdParentFolder { get; set; }
        public long IdUser { get; set; }

        [ForeignKey(nameof(IdParentFolder))]
        [InverseProperty(nameof(Folders.InverseIdParentFolderNavigation))]
        public virtual Folders IdParentFolderNavigation { get; set; }
        [ForeignKey(nameof(IdUser))]
        [InverseProperty(nameof(Userss.Folders))]
        public virtual Userss IdUserNavigation { get; set; }
        [InverseProperty(nameof(Folders.IdParentFolderNavigation))]
        public virtual ICollection<Folders> InverseIdParentFolderNavigation { get; set; }
        [InverseProperty("IdFldrNavigation")]
        public virtual ICollection<Passwords> Passwords { get; set; }
    }
}
