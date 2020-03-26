using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftCalculations
{
    public class Wish
    {
        public Employee Employee {get; set;}
        public int WantedShift { get; set; }
        public int Day { get; set; }

        public Wish(Employee emp, int shift, int day)
        {
            Employee = emp;
            WantedShift = shift;
            Day = day;
        }
    }
}
