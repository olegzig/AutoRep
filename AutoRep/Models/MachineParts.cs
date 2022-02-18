using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Models
{
    public class MachineParts
    {
        public int Id { get; set; }

        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public string Name { get; set; }//Название детали
     
        [Display(Name = "Количество на складе")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public int Count { get; set; }//работник который делает/сделает/записал

        [Display(Name = "Описание")]
        public string Discription { get; set; }//Клиент и его контактные данные

        [Display(Name = "Стоимость")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public double Cost { get; set; }//время на которое записали работника

        public enum SortState
        {
            NameAsc,
            NameDesc,
            CostAsc,
            CostDesc,
            CountAsc,
            CountDesc
        }
    }
}
