using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

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
        [Range(0, double.MaxValue, ErrorMessage = "Количество должно быть больше, либо равно нулю")]
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
    public class YourEqualityComparer : IEqualityComparer<MachineParts>
    {
        public bool Equals([AllowNull] MachineParts x, [AllowNull] MachineParts y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] MachineParts obj)
        {
            unchecked
            {
                var hash = 17;
                //same here, if you only want to get a hashcode on a, remove the line with b
                hash = hash * 23 + obj.Id.GetHashCode();

                return hash;
            }
        }
    }
}