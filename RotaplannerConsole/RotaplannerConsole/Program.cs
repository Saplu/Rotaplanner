using System;
using ShiftCalculations;

namespace RotaplannerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var dc = new Daycare();
            dc.Employees.ForEach(d => Console.WriteLine($"{d.Id} {d.Status}"));
        }
    }
}
