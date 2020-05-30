using System;
using System.Collections.Generic;

namespace ApiPFE.Models
{
    public partial class Userss
    {
        public Userss()
        {
            Folders = new HashSet<Folders>();
            Groupes = new HashSet<Groupes>();
            Passwords = new HashSet<Passwords>();
        }

        public long Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Folders> Folders { get; set; }
        public virtual ICollection<Groupes> Groupes { get; set; }
        public virtual ICollection<Passwords> Passwords { get; set; }

        public static implicit operator Userss(Folders v)
        {
            throw new NotImplementedException();
        }
    }
}
