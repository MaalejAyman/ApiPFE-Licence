using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPFE.Models
{
    public class User
    {
        public long Id { get; set; }
        public string login { get; set; }
        public string Password { get; set; }
    }
}
