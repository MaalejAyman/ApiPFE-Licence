
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPFE.Models.Write
{
    public class FoldersWrite
    {
        public string Name { get; set; }
        public long? IdParentFolder { get; set; }
        public long IdUser { get; set; }
        public string? Parent { get; set; }
    }
}
