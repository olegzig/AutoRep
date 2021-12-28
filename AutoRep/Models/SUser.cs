using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AutoRep.Models
{
    // Add profile data for application users by adding properties to the SUser class
    public class SUser : IdentityUser
    {
        public string Name { get; set; }//ФИО работника/гово
        public bool IsMananger { get; set; }//t = владелец, f = работник

        public override string ToString()
        {
            return Id.ToString() + " " + Name + " " + IsMananger.ToString();
        }

        public enum SortState
        {
            NameAsc,
            NameDesc
        }
    }
}
