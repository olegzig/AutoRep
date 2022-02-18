using System.ComponentModel.DataAnnotations;

namespace AutoRep.Models
{
    public class UserRequest
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Услуга")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public string WorkType { get; set; }//работа которую нужно проделать

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public string Name { get; set; }

        [Phone(ErrorMessage = "Телефонный номер дожен состоять только из цифр")]
        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public string PhoneNumber { get; set; }

        [EmailAddress]
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