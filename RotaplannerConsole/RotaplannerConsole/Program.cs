using System;
using ShiftCalculations;

namespace RotaplannerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var dc = new Daycare();

            var calc = new RotationCalculator();

            calc.DaycareShiftsOfThreeWeeks(dc, 1);
            calc.Switch(dc, new Wish(dc.Employees[2], 9, 0));
            calc.Switch(dc, new Wish(dc.Employees[10], 5, 0));
            calc.Switch(dc, new Wish(dc.Employees[1], 10, 1));
            calc.Switch(dc, new Wish(dc.Employees[5], 10, 1));
            calc.Switch(dc, new Wish(dc.Employees[8], 4, 2));
            calc.Switch(dc, new Wish(dc.Employees[6], 0, 2));
            calc.CheckTeacherSwitches(dc);

            foreach (var team in dc.Teams)
            {
                foreach(var emp in team.TeamEmp)
                {
                    var shifts = emp.Id.ToString() + " " + emp.Status.ToString() + ": ";
                    foreach(var shift in emp.Shifts)
                    {
                        var num = Convert.ToInt32(shift.Shift);
                        shifts += num + ((num > 9) ? " " : "  ");
                    }
                    Console.WriteLine(shifts);
                }
                Console.WriteLine("\n");
            }
        }
    }
}
