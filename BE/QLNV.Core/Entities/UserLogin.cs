﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Entities
{
    public class UserLogin
    {
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
