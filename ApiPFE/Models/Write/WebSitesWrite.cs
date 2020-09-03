using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPFE.Models.Write
{
    public class WebSitesWrite
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public long? IdUser { get; set; }
    }
}
