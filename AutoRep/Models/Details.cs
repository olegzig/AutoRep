using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Models
{
    public class Details
    {
        public int Id { get; set; }

        [Display(Name = "Наименование")]
        public string Name { get; set; }//Название детали
     
        [Display(Name = "Количество на складе")]
        public int Count { get; set; }//работник который делает/сделает/записал

        [Display(Name = "Описание")]
        public string Discription { get; set; }//Клиент и его контактные данные

        [Display(Name = "Стоимость")]
        public double Cost { get; set; }//время на которое записали работника
        

        public enum SortState
        {
            ClientAsc,
            ClientDesc,
            DateAsc,
            DateDesc,
        }
    }
}
