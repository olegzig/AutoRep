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
        public string Name { get; set; }//Клиент и его контактные данные

        [Display(Name = "Контактные данные")]
        public string ContactData { get; set; }//время на которое записали работника

        public enum SortState
        {
            ClientAsc,
            ClientDesc
        }
    }
}
