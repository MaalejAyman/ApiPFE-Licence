﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPFE.Models.Read
{
    public class UsersRead
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}