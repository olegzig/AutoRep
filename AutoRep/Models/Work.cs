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
        public string WorkType { get; set; }//работа которую нужно проделать
        public string Worker { get; set; }//работник который делает/сделает/записал
        public string Client { get; set; }//Клиент и его контактные данные
        public DateTime Date { get; set; }//время на которое записали работника

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
