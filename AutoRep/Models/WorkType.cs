using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Models
{
    public class WorkType
    {
        public WorkType() { }
        public int Id { get; set; }
        public string Name { get; set; }//Название работы
        public string Text { get; set; }//Описание работы
        public int Cost { get; set; }//Стоимость работы

        public enum SortState
        {
            NameAsc,
            NameDesc,
            CostAsc,
            CostDesc
        }
    }
}
