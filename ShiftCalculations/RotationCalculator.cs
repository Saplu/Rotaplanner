using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ShiftCalculations
{
    public class RotationCalculator
    {
        public List<WorkShift> TeamShiftsOfWeek(Team team, int openingTeam)
        {
            var status = GetWeekStatus(team, openingTeam);
            var shifts = GetWeekShifts(status, team);

            return shifts;
        }

        public void DaycareShiftsOfThreeWeeks(Daycare dc, int openingTeam)
        {
            dc.Teams.ForEach(t => TeamShiftsOfWeek(t, openingTeam));
            dc.RotateTeamsOneWeek();
            openingTeam++;
            if (openingTeam > 3)
                openingTeam = 0;
            dc.Teams.ForEach(t => TeamShiftsOfWeek(t, openingTeam));
            openingTeam++;
            if (openingTeam > 3)
                openingTeam = 0;
            dc.Teams.ForEach(t => TeamShiftsOfWeek(t, openingTeam));
        }

        public void Switch(Daycare dc, Wish wish)
        {
            var team = dc.Teams[wish.Employee.Team];
            AdjustTeamByWish(team, wish);

            //var current = dc.Employees.Find(e => Convert.ToInt32(e.Shifts[wish.Day].Shift) == wish.WantedShift);
            //return current.Id.ToString();
        }

        private int GetWeekStatus(Team team, int openingTeam)
            => (team.TeamNumber, openingTeam) switch
            {
                (0, 1) => 3,
                (0, 2) => 2,
                (0, 3) => 1,
                (1, 0) => 1,
                (1, 2) => 3,
                (1, 3) => 2,
                (2, 0) => 2,
                (2, 1) => 1,
                (2, 3) => 3,
                (3, 0) => 3,
                (3, 1) => 2,
                (3, 2) => 1,
                (_, _) => 0
            };

        private List<WorkShift> GetWeekShifts(int status, Team team)
        {
            var shifts = new List<WorkShift>();
            var teamShifts = GetTeamShifts(status);

            for (int i = 0; i < team.TeamEmp.Count; i++)
            {
                team.TeamEmp[i].Shifts.Add(new WorkShift((ShiftEnum)Enum.Parse(typeof(ShiftEnum), teamShifts[i].ToString())));
                team.TeamEmp[i].Shifts.Add(new WorkShift((ShiftEnum)Enum.Parse(typeof(ShiftEnum), teamShifts[i+1].ToString())));
                team.TeamEmp[i].Shifts.Add(new WorkShift((ShiftEnum)Enum.Parse(typeof(ShiftEnum), teamShifts[i+2].ToString())));
                team.TeamEmp[i].Shifts.Add(new WorkShift((ShiftEnum)Enum.Parse(typeof(ShiftEnum), teamShifts[i+3].ToString())));
                team.TeamEmp[i].Shifts.Add(new WorkShift((ShiftEnum)Enum.Parse(typeof(ShiftEnum), teamShifts[i+4].ToString())));
                shifts.AddRange(team.TeamEmp[i].Shifts);
            }
            return shifts;
        }

        private List<int> GetTeamShifts(int status) =>
            status switch
            {
                0 => new List<int>() { 0, 4, 8, 0, 4, 8, 0 },
                1 => new List<int>() { 1, 5, 9, 1, 5, 9, 1 },
                2 => new List<int>() { 2, 6, 10, 2, 6, 10, 2 },
                3 => new List<int>() { 3, 7, 11, 3, 7, 11, 3 },
                _ => throw new ArgumentException("Something weird has happened. Just close the app.")
            };

        private void AdjustTeamByWish(Team team, Wish wish)
        {
            var teamFirst = team.TeamEmp.FindIndex(e => Convert.ToInt32(e.Shifts[wish.Day].Shift) <= 3);
            var teamLast = team.TeamEmp.FindIndex(e => Convert.ToInt32(e.Shifts[wish.Day].Shift) >= 8);
            var teamMiddle = team.TeamEmp.FindIndex(e => Convert.ToInt32(e.Shifts[wish.Day].Shift) >= 4 &&
            Convert.ToInt32(e.Shifts[wish.Day].Shift) <= 7);
            var wisher = team.TeamEmp.FindIndex(e => e == wish.Employee);
            if (wish.WantedShift <= 3 && team.TeamEmp[teamFirst] != wish.Employee)
            {
                var current = team.TeamEmp[teamFirst].Shifts[wish.Day];
                var newSwitch = wish.Employee.Shifts[wish.Day];
                team.TeamEmp[teamFirst].Shifts[wish.Day] = newSwitch;
                team.TeamEmp[wisher].Shifts[wish.Day] = current;
                
            }
            else if (wish.WantedShift <= 7 && team.TeamEmp[teamMiddle] != wish.Employee)
            {
                var current = team.TeamEmp[teamMiddle].Shifts[wish.Day];
                var newSwitch = wish.Employee.Shifts[wish.Day];
                team.TeamEmp[teamMiddle].Shifts[wish.Day] = newSwitch;
                team.TeamEmp[wisher].Shifts[wish.Day] = current;
            }
            else if (wish.WantedShift >= 8 && team.TeamEmp[teamLast] != wish.Employee)
            {
                var current = team.TeamEmp[teamLast].Shifts[wish.Day];
                var newSwitch = wish.Employee.Shifts[wish.Day];
                team.TeamEmp[teamLast].Shifts[wish.Day] = newSwitch;
                team.TeamEmp[wisher].Shifts[wish.Day] = current;
            }
        }
    }
}
