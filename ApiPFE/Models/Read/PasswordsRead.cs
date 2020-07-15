using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPFE.Models.Read

{
    public class PasswordsRead
    {   
        public long Id { get; set; }
        public string Login { get; set; }
        public string Value { get; set; }
        public string Value2 { get; set; }
        public long? IdGrp { get; set; }
        public long? IdFldr { get; set; }
        public long IdWs { get; set; }
        public long IdUser { get; set; }
        public string GrpName { get; set; }
        public List<long> Groupes { get; set; }
    }
}
