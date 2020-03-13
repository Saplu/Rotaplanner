using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ShiftCalculations
{
    public class Daycare
    {
        public List<Employee> Employees { get; }
        public List<Team> Teams { get; }

        public Daycare()
        {
            Teams = new List<Team>() { new Team(0, 2), new Team(1, 1), new Team(2, 2), new Team(3, 1) };
            Employees = new List<Employee>();
            Teams.ForEach(t => Employees.AddRange(t.TeamEmp));
        }
    }
}
