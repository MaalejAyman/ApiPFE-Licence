using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPFE.Models.Write
{
    public class GroupesWrite
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? IdUser { get; set; }
        public Userss[]? Users { get; set; }
    }
}
