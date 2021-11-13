using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Models
{
    public class Work
    {
        public int Id { get; set; }
        public int WorkType { get; set; }//работа которую нужно проделать
        public int Worker { get; set; }//работник который делает/сделает/записал
        public string Client { get; set; }//Клиент и его контактные данные
        public DateTime Date { get; set; }//время на которое записали работника
    }
    
}
