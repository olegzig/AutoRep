﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Models
{
    public class User
    {
        public User() { }
        public int Id { get; set; }
        public string Name { get; set; }//ФИО работника/гово
        public bool IsOwner { get; set; }//t = владелец, f = работник
    }
}
