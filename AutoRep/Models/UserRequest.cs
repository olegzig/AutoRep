using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Models
{
    public class UserRequest
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Услуга")]
        public string WorkType { get; set; }//работа которую нужно проделать

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Phone]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }
        public enum SortState
        {
            ClientAsc,
            ClientDesc
        }
    }
}
