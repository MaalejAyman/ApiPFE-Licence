using System;
using System.Collections.Generic;

namespace ApiPFE.Models
{
    public partial class Passwords
    {
        public Passwords()
        {

        }
        public long Id { get; set; }
        public string Login { get; set; }
        public string Value { get; set; }
        public int? Score { get; set; }
        public long IdGrp { get; set; }
        public long IdFldr { get; set; }
        public long IdWs { get; set; }
        public long IdUser { get; set; }

        public virtual Folders IdFldrNavigation { get; set; }
        public virtual Groupes IdGrpNavigation { get; set; }
        public virtual Userss IdUserNavigation { get; set; }
        public virtual WebSites IdWsNavigation { get; set; }
    }
}
