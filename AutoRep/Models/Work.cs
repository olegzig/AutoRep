using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Models
{
    public class Work
    {
        public int Id { get; set; }

        [Display(Name = "Оказываемая услуга")]
        public string WorkType { get; set; }//работа которую нужно проделать

        [NotMapped]
        public string[] WorkTypeIds { get; set; }//Id работ которые нужно проделать


        [Display(Name = "Детали")]
        public string MachineParts { get; set; }//Используемые детали

        [NotMapped]
        public string[] MachinePartsIds { get; set; }//Id работ которые нужно проделать

        [Display(Name = "Работник")]
        public string Worker { get; set; }//работник который делает/сделает/записал

        [Display(Name = "ФИО клиента")]
        public string Client { get; set; }//Клиент и его контактные данные

        [Display(Name = "Мобильный телефон")]
        public string PhoneNumber { get; set; }//Клиент и его контактные данные

        [Display(Name = "Номер машины клиента")]
        public string CarNumber { get; set; }

        [Display(Name = "Модель машины клиента")]
        public string CarModel { get; set; }

        [EmailAddress]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [Display(Name = "Дата")]
        public DateTime Date { get; set; }//время на которое записали работника

        [Display(Name = "Работа выполнена")]
        public bool IsCompleted { get; set; }

        [NotMapped]
        [Display(Name = "Основан на id")]
        public int? MadeOnId { get; set; }//для отслеживания на каком Id сделан

        public enum SortState
        {
            ClientAsc,
            ClientDesc,
            DateAsc,
            DateDesc,
        }
    }
    
}
