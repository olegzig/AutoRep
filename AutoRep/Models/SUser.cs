﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AutoRep.Models
{
    // Add profile data for application users by adding properties to the SUser class
    public class SUser : IdentityUser
    {
        [Display(Name = "Id")]
        public override string Id { get => base.Id; set => base.Id = value; }

        [Display(Name = "Мененджер")]
        public bool IsMananger { get; set; }//t = владелец, f = работник

        [Display(Name = "Электронная почта")]
        public override string Email { get => base.Email; set => base.Email = value; }

        [Display(Name = "Телефонный номер")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        [Display(Name = "Имя")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }

        [NotMapped]
        [Display(Name = "Пароль")]
        public string  Password { get; set; }

        [NotMapped]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароль не совпадает.")]
        public string ConfirmPassword { get; set; }
        //NotMapped не должен идти в БД

        public override string ToString()
        {
            return Id.ToString() + " " + UserName + " " + IsMananger.ToString();
        }

        public enum SortState
        {
            NameAsc,
            NameDesc
        }
    }
}
