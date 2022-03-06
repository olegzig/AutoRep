using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoRep.Models
{
    public class WorkType
    {
        public WorkType()
        { }

        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public string Name { get { return name; } set { name = value; } }//Название работы
        public string name;

        [Display(Name = "Описание")]
        public string Text { get; set; }//Описание работы

        [Display(Name = "Стоимость")]
        [Required(ErrorMessage = "Данная информация необходима")]
        public double Cost { get { return cost; } set { cost = value; } }//Стоимость работы
        public double cost;

        [NotMapped]
        [Display(Name = "Количество использований")]
        public int countusage { get; set; }//Стоимость работы

        public enum SortState
        {
            NameAsc,
            NameDesc,
            CostAsc,
            CostDesc
        }
    }
}