﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Models
{
    public class WorkType
    {
        public WorkType() { }
        public string Id { get; set; }
        public string Name { get; set; }//Название работы
        public string Text { get; set; }//Описание работы
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
