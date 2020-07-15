using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPFE.Models.Write

{
    public class PasswordsWrite
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Value { get; set; }
        public long? IdGrp { get; set; }
        public long? IdFldr { get; set; }
        public long IdWs { get; set; }
        public long IdUser { get; set; }

        public long[] Groupes { get; set; }
        public string[] PasswordCrypPubs { get;set;}
    }
}
