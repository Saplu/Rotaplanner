using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
