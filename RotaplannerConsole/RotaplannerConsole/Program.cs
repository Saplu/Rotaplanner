using System;
using ShiftCalculations;

namespace RotaplannerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var dc = new Daycare();
            //dc.Employees.ForEach(d => Console.WriteLine($"{d.Id} {d.Status}"));
            //Console.WriteLine("Then after rotation:");
            //dc.RotateTeamsOneWeek();
            //dc.Employees.ForEach(d => Console.WriteLine($"{d.Id} {d.Status}"));
            //Console.WriteLine("And one more rotation:");
            //dc.RotateTeamsOneWeek();
            //dc.Employees.ForEach(d => Console.WriteLine($"{d.Id} {d.Status}"));


            var calc = new RotationCalculator();
            //dc.Teams.ForEach(t => calc.TeamShiftsOfWeek(t, 0));
            //dc.RotateTeamsOneWeek();
            //dc.Teams.ForEach(t => calc.TeamShiftsOfWeek(t, 1));
            //dc.RotateTeamsOneWeek();
            //dc.Teams.ForEach(t => calc.TeamShiftsOfWeek(t, 2));

            calc.DaycareShiftsOfThreeWeeks(dc, 2);

            //dc.Teams[0].TeamEmp.ForEach(e => e.Shifts.ForEach(s => Console.WriteLine(s.Shift.ToString())));

            foreach(var team in dc.Teams)
            {
                foreach(var emp in team.TeamEmp)
                {
                    var shifts = "";
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
