using System;
using System.Collections.Generic;

namespace ApiPFE.Models
{
    public partial class Groupes
    {
        public Groupes()
        {
            Passwords = new HashSet<Passwords>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long? IdUser { get; set; }

        public virtual Userss IdUserNavigation { get; set; }
        public virtual ICollection<Passwords> Passwords { get; set; }
    }
}
