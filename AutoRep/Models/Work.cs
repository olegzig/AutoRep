using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Models
{
    public class Work
    {
        public int Id { get; set; }
        public WorkType workType { get; set; }
        public User Worker { get; set; }
        public string Client { get; set; }
        public WorkType[] ToDo { get; set; }//список работ которые нужно проделать
        public DateTime Date { get; set; }//время на которое записали работника

    }
    
}
