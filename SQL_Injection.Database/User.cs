﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SQL_Injection.Database
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
