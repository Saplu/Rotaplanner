using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ShiftCalculations
{
    public class RotationCalculator
    {

        public void DaycareShiftsOfThreeWeeks(Daycare dc, int openingTeam, List<Wish> wishes)
        {
            dc.Teams.ForEach(t => TeamShiftsOfWeek(t, openingTeam));
            openingTeam++;
            if (openingTeam > 3)
                openingTeam = 0;
            dc.RotateTeamsOneWeek();
            dc.Teams.ForEach(t => TeamShiftsOfWeek(t, openingTeam));
            openingTeam++;
            if (openingTeam > 3)
                openingTeam = 0;
            dc.RotateTeamsOneWeek();
            dc.Teams.ForEach(t => TeamShiftsOfWeek(t, openingTeam));

            foreach(var wish in wishes)
                Switch(dc, wish);
            CheckTeacherSwitches(dc);
        }
        public List<WorkShift> TeamShiftsOfWeek(Team team, int openingTeam)
        {
            var status = GetWeekStatus(team, openingTeam);
            var shifts = GetWeekShifts(status, team);

            return shifts;
        }

        private void Switch(Daycare dc, Wish wish)
        {
            var team = dc.Teams[wish.Employee.Team];
            AdjustTeamByWish(team, wish);
            var current = Convert.ToInt32(wish.Employee.Shifts[wish.Day].Shift);
            var wanted = wish.WantedShift;
            if (current != wanted)
                AdjustDaycareByWish(dc, wish, current);
        }

        private void CheckTeacherSwitches(Daycare dc)
        {
            foreach(var emp in dc.Employees)
            {
                if (emp.Status == StatusEnum.Teacher)
                {
                    var counts = GetOpensAndCloses(emp.Shifts, 0, 5);
                    CheckTeacherWeek(emp, counts, dc);
                    counts = GetOpensAndCloses(emp.Shifts, 5, 10);
                    CheckTeacherWeek(emp, counts, dc);
                    counts = GetOpensAndCloses(emp.Shifts, 10, 15);
                    CheckTeacherWeek(emp, counts, dc);
                    CheckPlantimeAvailable(emp, dc);
                }
            }
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
                if (team.TeamEmp[teamFirst].Shifts[wish.Day].Locked)
                    throw new Exception($"Two wishes in same team for same time on day {wish.Day}. Remove one.");
                var current = team.TeamEmp[teamFirst].Shifts[wish.Day];
                var newSwitch = wish.Employee.Shifts[wish.Day];
                team.TeamEmp[teamFirst].Shifts[wish.Day] = newSwitch;
                team.TeamEmp[wisher].Shifts[wish.Day] = current;
                
            }
            else if (wish.WantedShift <= 7 && wish.WantedShift >= 4 && team.TeamEmp[teamMiddle] != wish.Employee)
            {
                if (team.TeamEmp[teamMiddle].Shifts[wish.Day].Locked)
                    throw new Exception($"Two wishes in same team for same time on day {wish.Day}. Remove one.");
                var current = team.TeamEmp[teamMiddle].Shifts[wish.Day];
                var newSwitch = wish.Employee.Shifts[wish.Day];
                team.TeamEmp[teamMiddle].Shifts[wish.Day] = newSwitch;
                team.TeamEmp[wisher].Shifts[wish.Day] = current;
            }
            else if (wish.WantedShift >= 8 && team.TeamEmp[teamLast] != wish.Employee)
            {
                if (team.TeamEmp[teamLast].Shifts[wish.Day].Locked)
                    throw new Exception($"Two wishes in same team for same time on day {wish.Day}. Remove one.");
                var current = team.TeamEmp[teamLast].Shifts[wish.Day];
                var newSwitch = wish.Employee.Shifts[wish.Day];
                team.TeamEmp[teamLast].Shifts[wish.Day] = newSwitch;
                team.TeamEmp[wisher].Shifts[wish.Day] = current;
            }
        }

        private void AdjustDaycareByWish(Daycare dc, Wish wish, int current)
        {
            if (current >= 4 && current <= 7) return;
            var shiftList = PossibleShifts(wish.WantedShift);
            shiftList.Remove(current);
            foreach(var item in shiftList)
            {
                var emp = dc.Employees.Find(e => Convert.ToInt32(e.Shifts[wish.Day].Shift) == item);
                if (!emp.Shifts[wish.Day].Locked)
                {
                    var oldShift = wish.Employee.Shifts[wish.Day].Shift;
                    var newShift = emp.Shifts[wish.Day].Shift;
                    wish.Employee.Shifts[wish.Day].Shift = newShift;
                    emp.Shifts[wish.Day].Shift = oldShift;
                    wish.Employee.Shifts[wish.Day].Locked = true;
                    break;
                }
            }
        }

        private List<int> PossibleShifts(int wanted) =>
            wanted switch
            {
                0 => new List<int>() { 0, 1, 2, 3},
                1 => new List<int>() { 1, 2, 3, 0},
                2 => new List<int>() { 2, 3, 1, 0},
                3 => new List<int>() { 3, 2, 1, 0},
                8 => new List<int>() { 8, 9, 10, 11},
                9 => new List<int>() { 9, 10, 8, 11},
                10 => new List<int>() { 10, 11, 9, 8},
                11 => new List<int>() { 11, 10, 9, 8},
                _ => throw new Exception("Something is broken, burn the evidence.")
            };

        private (List<int>, List<int>) GetOpensAndCloses(List<WorkShift> shifts, int start, int end)
        {
            var opens = new List<int>();
            var shuts = new List<int>();
            for (int i = start; i < end; i++)
            {
                if (shifts[i].Shift == ShiftEnum.Open)
                    opens.Add(i);
                if (shifts[i].Shift == ShiftEnum.Shut)
                    shuts.Add(i);
            }
            return (opens, shuts);
        }

        private void CheckTeacherWeek(Employee emp, (List<int>, List<int>) counts, Daycare dc)
        {
            if (counts.Item1.Count > 1)
                for (int i = 1; i < counts.Item1.Count; i++)
                {
                    var wish = new Wish(emp, 1, counts.Item1[i]);
                    Switch(dc, wish);
                }
            if (counts.Item2.Count > 1)
                for (int i = 1; i < counts.Item2.Count; i++)
                {
                    var wish = new Wish(emp, 10, counts.Item2[i]);
                    Switch(dc, wish);
                }
        }

        private void CheckPlantimeAvailable(Employee emp, Daycare dc)
        {
            var morningList = new List<int>();
            emp.Shifts.ForEach(s => morningList.Add(Convert.ToInt32(s.Shift)));
            var w1 = morningList.GetRange(1, 3).IndexOf(morningList.GetRange(1, 3).Min()) + 1;
            var w2 = morningList.GetRange(6, 3).IndexOf(morningList.GetRange(6, 3).Min()) + 6;
            var w3 = morningList.GetRange(11, 3).IndexOf(morningList.GetRange(11, 3).Min()) + 11;
            if (morningList[w1] >= 4)
            {
                var wish = new Wish(emp, 3, w1);
                Switch(dc, wish);
            }
            if (morningList[w2] >= 4)
            {
                var wish = new Wish(emp, 3, w2);
                Switch(dc, wish);
            }
            if (morningList[w3] >= 4)
            {
                var wish = new Wish(emp, 3, w3);
                Switch(dc, wish);
            }
        }
    }
}
