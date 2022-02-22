using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoRep.Models
{
    public class UserRequest
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Услуга")]
        public string WorkType { get; set; }//работа которую нужно проделать

        [NotMapped]
        public string[] WorkTypeIds { get; set; }//Id работ которые нужно проделать

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public string Name { get; set; }

        [Phone(ErrorMessage = "Телефонный номер дожен состоять только из цифр")]
        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Это не корректная электронная почта.")]
        [Display(Name = "Электронная почта")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public string Email { get; set; }

        public enum SortState
        {
            ClientAsc,
            ClientDesc
        }
    }
}