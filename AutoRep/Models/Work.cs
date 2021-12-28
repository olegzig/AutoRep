using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Models
{
    public class Work
    {
        public string Id { get; set; }
        public string WorkType { get; set; }//работа которую нужно проделать
        public string Worker { get; set; }//работник который делает/сделает/записал
        public string Client { get; set; }//Клиент и его контактные данные
        public DateTime Date { get; set; }//время на которое записали работника

        public enum SortState
        {
            ClientAsc,
            ClientDesc,
            DateAsc,
            DateDesc,
        }
    }
    
}
