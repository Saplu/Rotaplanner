using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftCalculations;
using System.Collections.Generic;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class RotationTests
    {
        [TestMethod]
        public void NonreversedGivesCorrectShifts()
        {
            var dc = new Daycare();
            var rc = new RotationCalculator();
            var actual = new List<WorkShift>();
            dc.Teams.ForEach(d => actual.AddRange(rc.TeamShiftsOfWeek(d, 0, dc.Teams.Count, true)));
            var expected = new List<WorkShift>()
            {
                new WorkShift(0),
                new WorkShift(4),
                new WorkShift(8),
                new WorkShift(0),
                new WorkShift(4),

                new WorkShift(4),
                new WorkShift(8),
                new WorkShift(0),
                new WorkShift(4),
                new WorkShift(8),

                new WorkShift(8),
                new WorkShift(0),
                new WorkShift(4),
                new WorkShift(8),
                new WorkShift(0),

                new WorkShift(1),
                new WorkShift(5),
                new WorkShift(9),
                new WorkShift(1),
                new WorkShift(5),

                new WorkShift(5),
                new WorkShift(9),
                new WorkShift(1),
                new WorkShift(5),
                new WorkShift(9),

                new WorkShift(9),
                new WorkShift(1),
                new WorkShift(5),
                new WorkShift(9),
                new WorkShift(1),

                new WorkShift(2),
                new WorkShift(6),
                new WorkShift(10),
                new WorkShift(2),
                new WorkShift(6),

                new WorkShift(6),
                new WorkShift(10),
                new WorkShift(2),
                new WorkShift(6),
                new WorkShift(10),

                new WorkShift(10),
                new WorkShift(2),
                new WorkShift(6),
                new WorkShift(10),
                new WorkShift(2),

                new WorkShift(3),
                new WorkShift(7),
                new WorkShift(11),
                new WorkShift(3),
                new WorkShift(7),

                new WorkShift(7),
                new WorkShift(11),
                new WorkShift(3),
                new WorkShift(7),
                new WorkShift(11),

                new WorkShift(11),
                new WorkShift(3),
                new WorkShift(7),
                new WorkShift(11),
                new WorkShift(3)
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReversedGivesCorrectShifts()
        {
            var dc = new Daycare();
            var rc = new RotationCalculator();
            var actual = new List<WorkShift>();
            dc.Teams.ForEach(d => actual.AddRange(rc.TeamShiftsOfWeek(d, 0, dc.Teams.Count, false)));
            var expected = new List<WorkShift>()
            {
                new WorkShift(0),
                new WorkShift(8),
                new WorkShift(4),
                new WorkShift(0),
                new WorkShift(8),

                new WorkShift(8),
                new WorkShift(4),
                new WorkShift(0),
                new WorkShift(8),
                new WorkShift(4),

                new WorkShift(4),
                new WorkShift(0),
                new WorkShift(8),
                new WorkShift(4),
                new WorkShift(0),

                new WorkShift(1),
                new WorkShift(9),
                new WorkShift(5),
                new WorkShift(1),
                new WorkShift(9),

                new WorkShift(9),
                new WorkShift(5),
                new WorkShift(1),
                new WorkShift(9),
                new WorkShift(5),

                new WorkShift(5),
                new WorkShift(1),
                new WorkShift(9),
                new WorkShift(5),
                new WorkShift(1),

                new WorkShift(2),
                new WorkShift(10),
                new WorkShift(6),
                new WorkShift(2),
                new WorkShift(10),

                new WorkShift(10),
                new WorkShift(6),
                new WorkShift(2),
                new WorkShift(10),
                new WorkShift(6),

                new WorkShift(6),
                new WorkShift(2),
                new WorkShift(10),
                new WorkShift(6),
                new WorkShift(2),

                new WorkShift(3),
                new WorkShift(11),
                new WorkShift(7),
                new WorkShift(3),
                new WorkShift(11),

                new WorkShift(11),
                new WorkShift(7),
                new WorkShift(3),
                new WorkShift(11),
                new WorkShift(7),

                new WorkShift(7),
                new WorkShift(3),
                new WorkShift(11),
                new WorkShift(7),
                new WorkShift(3)
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EmployeeRotation()
        {
            var dc = new Daycare();
            dc.RotateTeamsOneWeek();
            var expected = new List<StatusEnum>() { StatusEnum.Teacher, StatusEnum.Nurse, StatusEnum.Teacher,
            StatusEnum.Nurse, StatusEnum.Nurse, StatusEnum.Teacher,
            StatusEnum.Teacher, StatusEnum.Nurse, StatusEnum.Teacher,
            StatusEnum.Nurse, StatusEnum.Nurse, StatusEnum.Teacher};
            var actual = new List<StatusEnum>();
            dc.Teams.ForEach(t => t.TeamEmp.ForEach(e => actual.Add(e.Status)));

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SwitchWorksCorrectly()
        {
            var dc = new Daycare();
            var rc = new RotationCalculator();
            var wishes = new List<Wish>()
            {
                new Wish(dc.Employees.Find(e => e.Id == 1), 1, 1),
                new Wish(dc.Employees.Find(e => e.Id == 9), 10, 1)
            };
            rc.DaycareShiftsOfThreeWeeks(dc, 0, wishes, 1);
            var actual = dc.Employees.Select(e => (int)e.Shifts[1].Shift).ToList();

            var expected = new List<int>()
            {
                8, 4, 1, 0, 5, 9, 2, 6, 11, 3, 10, 7
            };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task ThrowsExceptionOnSimilarWishesWithinTeam()
        {
            var dc = new Daycare();
            var rc = new RotationCalculator();
            var wishes = new List<Wish>()
            {
                new Wish(dc.Employees.Find(e => e.Id == 2), 10, 1),
                new Wish(dc.Employees.Find(e => e.Id == 1), 11, 1)
            };
            await rc.DaycareShiftsOfThreeWeeks(dc, 0, wishes, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ThrowsArgumentExceptionOnCrossTeamDuplicateWish()
        {
            var dc = new Daycare();
            var rc = new RotationCalculator();
            var wishes = new List<Wish>()
            {
                new Wish(dc.Employees.Find(e => e.Id == 2), 10, 1),
                new Wish(dc.Employees.Find(e => e.Id == 10), 10, 1)
            };
            await rc.DaycareShiftsOfThreeWeeks(dc, 0, wishes, 0);
        }

        [TestMethod]
        public void CalculatesCorrectShiftsForSmallDaycare()
        {
            var teams = new List<Team>()
            {
                new Team(0, 1),
                new Team(1, 2)
            };
            var dc = new Daycare(teams);
            var rc = new RotationCalculator();
            var wishes = new List<Wish>();
            rc.DaycareShiftsOfThreeWeeks(dc, 0, wishes, 1);
            var emp0Shifts = dc.Teams[0].TeamEmp[0].Shifts;
            var emp1Shifts = dc.Teams[0].TeamEmp[1].Shifts;
            var emp2Shifts = dc.Teams[0].TeamEmp[2].Shifts;
            var emp3Shifts = dc.Teams[1].TeamEmp[0].Shifts;
            var emp4Shifts = dc.Teams[1].TeamEmp[1].Shifts;
            var emp5Shifts = dc.Teams[1].TeamEmp[2].Shifts;

            var expShifts1 = new List<WorkShift>()
            {
                new WorkShift(0),
                new WorkShift(2),
                new WorkShift(4),
                new WorkShift(1, true),
                new WorkShift(2),

                new WorkShift(5),
                new WorkShift(1),
                new WorkShift(3),
                new WorkShift(4, true),
                new WorkShift(0),

                new WorkShift(2),
                new WorkShift(4),
                new WorkShift(0),
                new WorkShift(2),
                new WorkShift(5)
            };

            CollectionAssert.AreEqual(expShifts1, emp1Shifts);
        }
    }
}
