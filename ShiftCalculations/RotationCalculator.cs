﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShiftCalculations
{
    public class RotationCalculator
    {
        public RotationCalculator()
        {

        }

        public async Task DaycareShiftsOfThreeWeeks(Daycare dc, int openingTeam, List<Wish> wishes, int up = 0)
        {
            bool upOrDown = Convert.ToBoolean(up);
            CheckDuplicates(wishes);
            dc.Teams.ForEach(t => TeamShiftsOfWeek(t, openingTeam, dc.Teams.Count, upOrDown));
            openingTeam++;
            if (openingTeam > dc.Teams.Count - 1)
                openingTeam = 0;
            dc.RotateTeamsOneWeek();
            dc.Teams.ForEach(t => TeamShiftsOfWeek(t, openingTeam, dc.Teams.Count, upOrDown));
            openingTeam++;
            if (openingTeam > dc.Teams.Count - 1)
                openingTeam = 0;
            dc.RotateTeamsOneWeek();
            dc.Teams.ForEach(t => TeamShiftsOfWeek(t, openingTeam, dc.Teams.Count, upOrDown));

            foreach(var wish in wishes)
                Switch(dc, wish);
            CheckTeacherSwitches(dc);
        }
        public List<WorkShift> TeamShiftsOfWeek(Team team, int openingTeam, int teamCount, bool up)
        {
            var status = GetWeekStatus(team.TeamNumber, openingTeam, teamCount);
            var shifts = GetWeekShifts(status, team, teamCount, up);

            return shifts;
        }

        private void Switch(Daycare dc, Wish wish)
        {
            try
            {
                var team = dc.Teams[wish.Employee.Team];
                AdjustTeamByWish(team, wish, dc.Employees.Count);
                var current = Convert.ToInt32(wish.Employee.Shifts[wish.Day].Shift);
                var wanted = wish.WantedShift;
                if (current != wanted)
                    AdjustDaycareByWish(dc, wish, current);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private void CheckTeacherSwitches(Daycare dc)
        {
            foreach(var emp in dc.Employees)
            {
                if (emp.Status == StatusEnum.Teacher)
                {
                    var counts = GetOpensAndCloses(emp.Shifts, 0, 5, dc.Teams.Count);
                    CheckTeacherWeek(emp, counts, dc);
                    counts = GetOpensAndCloses(emp.Shifts, 5, 10, dc.Teams.Count);
                    CheckTeacherWeek(emp, counts, dc);
                    counts = GetOpensAndCloses(emp.Shifts, 10, 15, dc.Teams.Count);
                    CheckTeacherWeek(emp, counts, dc);
                    CheckPlantimeAvailable(emp, dc);
                }
            }
        }

        private int GetWeekStatus(int team, int openingTeam, int groups)
        {
            if (openingTeam == 0)
                return team;
            if (openingTeam == team)
                return 0;
            if (team > openingTeam)
                return team - openingTeam;
            return groups - (openingTeam - team);

        }

        private List<WorkShift> GetWeekShifts(int status, Team team, int teamCount, bool up)
        {
            var shifts = new List<WorkShift>();
            var teamShifts = GetTeamShifts(status, teamCount, up);

            for (int i = 0; i < team.TeamEmp.Count; i++)
            {
                team.TeamEmp[i].Shifts.Add(new WorkShift(teamShifts[i]));
                team.TeamEmp[i].Shifts.Add(new WorkShift(teamShifts[i+1]));
                team.TeamEmp[i].Shifts.Add(new WorkShift(teamShifts[i+2]));
                team.TeamEmp[i].Shifts.Add(new WorkShift(teamShifts[i+3]));
                team.TeamEmp[i].Shifts.Add(new WorkShift(teamShifts[i+4]));
                shifts.AddRange(team.TeamEmp[i].Shifts);
            }
            return shifts;
        }

        private List<int> GetTeamShifts(int status, int teamCount, bool up)
        {
            var list = new List<int>()
                {
                    status, status + teamCount, status + (teamCount*2),
                    status, status + teamCount, status + (teamCount*2), status
                };
            if (up) return list;
            list.Reverse();
            return list;
        }

        private void AdjustTeamByWish(Team team, Wish wish, int empCount)
        {
            var teamFirst = team.TeamEmp.FindIndex(e => e.Shifts[wish.Day].Shift <= (empCount / 3 - 1));
            var teamLast = team.TeamEmp.FindIndex(e => e.Shifts[wish.Day].Shift >= (empCount / 3 * 2));
            var teamMiddle = team.TeamEmp.FindIndex(e => e.Shifts[wish.Day].Shift >= (empCount / 3) &&
            e.Shifts[wish.Day].Shift <= (empCount / 3 * 2 - 1));
            var wisher = team.TeamEmp.FindIndex(e => e == wish.Employee);
            if (wish.WantedShift <= (empCount / 3 - 1) && team.TeamEmp[teamFirst] != wish.Employee)
            {
                if (team.TeamEmp[teamFirst].Shifts[wish.Day].Locked)
                    throw new Exception($"Two wishes in same team for same time on day {wish.Day}. Remove one.");
                var current = team.TeamEmp[teamFirst].Shifts[wish.Day];
                var newSwitch = wish.Employee.Shifts[wish.Day];
                team.TeamEmp[teamFirst].Shifts[wish.Day] = newSwitch;
                team.TeamEmp[wisher].Shifts[wish.Day] = current;
                
            }
            else if (wish.WantedShift <= (empCount / 3 * 2 - 1) && wish.WantedShift >= (empCount / 3) && team.TeamEmp[teamMiddle] != wish.Employee)
            {
                if (team.TeamEmp[teamMiddle].Shifts[wish.Day].Locked)
                    throw new Exception($"Two wishes in same team for same time on day {wish.Day}. Remove one.");
                var current = team.TeamEmp[teamMiddle].Shifts[wish.Day];
                var newSwitch = wish.Employee.Shifts[wish.Day];
                team.TeamEmp[teamMiddle].Shifts[wish.Day] = newSwitch;
                team.TeamEmp[wisher].Shifts[wish.Day] = current;
            }
            else if (wish.WantedShift >= (empCount / 3 * 2) && team.TeamEmp[teamLast] != wish.Employee)
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
            if (current >= dc.Employees.Count / 3 && current <= dc.Employees.Count / 3 * 2 - 1) return;
            var shiftList = PossibleShifts(wish.WantedShift, dc.Teams.Count);
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

        private List<int> PossibleShifts(int wanted, int teamCount)
        {
            var list = new List<int>();
            if (wanted < teamCount)
            {
                for (int i = 0; i < teamCount; i++)
                {
                    list.Add(i);
                }
            }
            else if (wanted > teamCount * 2 - 1)
            {
                for (int i = teamCount * 2; i < teamCount * 3; i++)
                {
                    list.Add(i);
                }
            }
            var sorter = GetSorterValue(wanted, teamCount * 3);
            list = list.OrderBy(n => Math.Abs(sorter - n)).ToList();
            return list;
        }

        private (List<int>, List<int>) GetOpensAndCloses(List<WorkShift> shifts, int start, int end, int teamCount)
        {
            var opens = new List<int>();
            var shuts = new List<int>();
            for (int i = start; i < end; i++)
            {
                if (shifts[i].Shift == 0)
                    opens.Add(i);
                if (shifts[i].Shift == ((teamCount * 3) - 1))
                    shuts.Add(i);
            }
            return (opens, shuts);
        }

        private void CheckTeacherWeek(Employee emp, (List<int>, List<int>) counts, Daycare dc)
        {
            if (counts.Item1.Count > 1)
                for (int i = 1; i < counts.Item1.Count; i++)
                {
                    var wish = new Wish(emp, dc.Teams.Count - 1, counts.Item1[i]);
                    Switch(dc, wish);
                }
            if (counts.Item2.Count > 1)
                for (int i = 1; i < counts.Item2.Count; i++)
                {
                    var wish = new Wish(emp, dc.Employees.Count - 2, counts.Item2[i]);
                    Switch(dc, wish);
                }
        }

        private void CheckPlantimeAvailable(Employee emp, Daycare dc)
        {
            var morningList = new List<int>();
            emp.Shifts.ForEach(s => morningList.Add(s.Shift));
            var w1 = morningList.GetRange(1, 3).IndexOf(morningList.GetRange(1, 3).Min()) + 1;
            var w2 = morningList.GetRange(6, 3).IndexOf(morningList.GetRange(6, 3).Min()) + 6;
            var w3 = morningList.GetRange(11, 3).IndexOf(morningList.GetRange(11, 3).Min()) + 11;
            if (morningList[w1] >= dc.Teams.Count)
            {
                var wish = new Wish(emp, dc.Teams.Count - 1, w1);
                Switch(dc, wish);
            }
            if (morningList[w2] >= dc.Teams.Count)
            {
                var wish = new Wish(emp, dc.Teams.Count - 1, w2);
                Switch(dc, wish);
            }
            if (morningList[w3] >= dc.Teams.Count)
            {
                var wish = new Wish(emp, dc.Teams.Count - 1, w3);
                Switch(dc, wish);
            }
        }

        private void CheckDuplicates(List<Wish> wishes)
        {
            if (wishes
                .GroupBy(w => new
                {
                    w.Day,
                    w.WantedShift
                })
                .Any(g => g.Count() > 1))
            {
                throw new ArgumentException("Duplicate wishes");
            }
        }

        private double GetSorterValue(int wanted, int empCount)
        {
            if (wanted < empCount / 6) return wanted - 0.1;
            if (wanted >= empCount / 6 * 5) return wanted + 0.1;
            else return wanted;
        }
    }
}
