using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPFE.Models.Write
{
    public class UsersWrite
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
