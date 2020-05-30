using System;
using System.Collections.Generic;

namespace ApiPFE.Models
{
    public partial class WebSites
    {
        public WebSites()
        {
            Passwords = new HashSet<Passwords>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public long? UserId { get; set; }

        public virtual ICollection<Passwords> Passwords { get; set; }
    }
}
