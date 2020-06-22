using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ShiftCalculations
{
    public class Daycare
    {
        public List<Employee> Employees { get; set; }
        public List<Team> Teams { get; }

        public Daycare()
        {
            Teams = new List<Team>() { new Team(0, 2), new Team(1, 1), new Team(2, 2), new Team(3, 1) };
            Employees = new List<Employee>();
            Teams.ForEach(t => Employees.AddRange(t.TeamEmp));
        }

        public Daycare(List<Team> teams)
        {
            Teams = HandleTeamNumbers(teams);
            Employees = new List<Employee>();
            Teams.ForEach(t => Employees.AddRange(t.TeamEmp));
        }

        public void RotateTeamsOneWeek()
        {
            foreach (var team in Teams)
            {
                var empList = new List<Employee>();
                empList.Add(team.TeamEmp[1]);
                empList.Add(team.TeamEmp[2]);
                empList.Add(team.TeamEmp[0]);
                team.TeamEmp = empList;
            }
            var dcEmpList = new List<Employee>();
            Teams.ForEach(t => dcEmpList.AddRange(t.TeamEmp));
            Employees = dcEmpList;
        }

        private List<Team> HandleTeamNumbers(List<Team> teams)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                teams[i].TeamNumber = i;
            }
            return teams;
        }
    }
}
