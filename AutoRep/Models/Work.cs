using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoRep.Models
{
    public class Work
    {
        public int Id { get; set; }

        [Display(Name = "Оказываемая услуга")]
        public string WorkType { get; set; }//работа которую нужно проделать

        [Required(ErrorMessage = "Выберите хотя бы один элемент")]
        [NotMapped]
        public string[] WorkTypeIds { get; set; }//Id работ которые нужно проделать

        [Display(Name = "Детали")]
        public string MachineParts { get; set; }//Используемые детали

        [Required(ErrorMessage = "Выберите хотя бы один элемент")]
        [NotMapped]
        public string[] MachinePartsIds { get; set; }//Id работ которые нужно проделать

        [Display(Name = "Работник")]
        public string Worker { get; set; }//работник который делает/сделает/записал

        [Display(Name = "ФИО клиента")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public string Client { get; set; }//Клиент и его контактные данные

        [Display(Name = "Мобильный телефон")]
        [Phone(ErrorMessage = "Телефонный номер дожен состоять только из цифр")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public string PhoneNumber { get; set; }//Клиент и его контактные данные

        [Display(Name = "Номер машины клиента")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public string CarNumber { get; set; }

        [Display(Name = "Модель машины клиента")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public string CarModel { get; set; }

        [Display(Name = "Электронная почта")]
        [Required(ErrorMessage = "Данная информация необходима")]
        [EmailAddress(ErrorMessage = "Это не корректная электронная почта.")]
        public string Email { get; set; }

        [Display(Name = "Дата")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public DateTime Date { get; set; }//время на которое записали работника

        [Display(Name = "Работа выполнена")]
        public bool IsCompleted { get; set; }

        [NotMapped]
        [Display(Name = "Основан на id")]
        public int? MadeOnId { get; set; }//для отслеживания на каком Id сделан

        [Display(Name = "Итоговая стоимость")]
        public double Cost { get; set; }


        public enum SortState
        {
            ClientAsc,
            ClientDesc,
            DateAsc,
            DateDesc,
        }
    }
}