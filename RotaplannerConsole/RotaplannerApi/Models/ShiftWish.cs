using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShiftCalculations;

namespace RotaplannerApi.Models
{
    public class ShiftWish
    {
        public long Id { get; set; }
        public int EmpId { get; set; }
        public int Shift { get; set; }
        public int Day { get; set; }
        public string Creator { get; set; }
        public string Set { get; set; }

        public ShiftWish(int empId, int shift, int day, string creator, string set, long id)
        {
            EmpId = empId;
            Shift = shift;
            Day = day;
            Creator = creator;
            Set = set;
            Id = id;
        }

        public ShiftWish()
        {
            var rand = new Random();
            Id = rand.Next();
        }
    }
}
