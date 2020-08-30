using System;
using System.Collections.Generic;
using ShiftCalculations;

namespace RotaplannerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var dc = new Daycare();

            var calc = new RotationCalculator();

            try
            {
                var wishes = new List<Wish>()
                {
                    new Wish(dc.Employees.Find(e => e.Id == 1), 10, 4),
                    //new Wish(dc.Employees.Find(e => e.Id == 6), 10, 4),
                    new Wish(dc.Employees.Find(e => e.Id == 10), 6, 1)
                };
                calc.DaycareShiftsOfThreeWeeks(dc, 0, wishes, 0);

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
