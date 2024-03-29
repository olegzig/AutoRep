﻿using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoRep.Models
{
    // Add profile data for application users by adding properties to the SUser class
    public class SUser : IdentityUser
    {

        [Display(Name = "Id")]
        public override string Id { get => base.Id; set => base.Id = value; }

        [Display(Name = "Электронная почта")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public override string Email { get => base.Email; set => base.Email = value; }

        [Display(Name = "Телефонный номер")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        [Display(Name = "Имя")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }

        [NotMapped]
        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Данная информация необходима")]
        [StringLength(100, ErrorMessage = "Пароль должен состоять из {2} символов как минимум, и быть не длиннее {1} символов.", MinimumLength = 6)]
        public string Password { get; set; }

        [NotMapped]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароль не совпадает.")]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        [Display(Name = "Роль")]
        [Required(ErrorMessage = "Не выбрана роль")]
        public string Role { get; set; }

        public override string ToString()
        {
            return Id.ToString() + " " + UserName;
        }

        [NotMapped]
        public int count { get; set; }
        [NotMapped]
        public string name{ get; set; }

        public enum SortState
        {
            NameAsc,
            NameDesc
        }
    }
}