using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Models
{
    public class WorkType
    {
        public WorkType() { }

        [Display( Name ="Id")]
        public int Id { get; set; }

        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public string Name { get; set; }//Название работы

        [Display(Name = "Описание")]
        public string Text { get; set; }//Описание работы

        [Display(Name = "Стоимость")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public double Cost { get; set; }//Стоимость работы

        public enum SortState
        {
            NameAsc,
            NameDesc,
            CostAsc,
            CostDesc
        }
    }
}
