using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftCalculations;
using System.Collections.Generic;
using Moq;
using System;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class RotationTests
    {
        [TestMethod]
        public void TeamStatusCorrect()
        {
            var dc = new Daycare();
            var rc = new RotationCalculator();
            var actual = new List<WorkShift>();
            dc.Teams.ForEach(d => actual.AddRange(rc.TeamShiftsOfWeek(d, 0)));
            var expected = new List<WorkShift>()
            {
                new WorkShift(ShiftEnum.Open),
                new WorkShift(ShiftEnum.FirstMiddle),
                new WorkShift(ShiftEnum.FirstEvening),
                new WorkShift(ShiftEnum.Open),
                new WorkShift(ShiftEnum.FirstMiddle),

                new WorkShift(ShiftEnum.FirstMiddle),
                new WorkShift(ShiftEnum.FirstEvening),
                new WorkShift(ShiftEnum.Open),
                new WorkShift(ShiftEnum.FirstMiddle),
                new WorkShift(ShiftEnum.FirstEvening),

                new WorkShift(ShiftEnum.FirstEvening),
                new WorkShift(ShiftEnum.Open),
                new WorkShift(ShiftEnum.FirstMiddle),
                new WorkShift(ShiftEnum.FirstEvening),
                new WorkShift(ShiftEnum.Open),

                new WorkShift(ShiftEnum.Second),
                new WorkShift(ShiftEnum.SecondMiddle),
                new WorkShift(ShiftEnum.SecondEvening),
                new WorkShift(ShiftEnum.Second),
                new WorkShift(ShiftEnum.SecondMiddle),

                new WorkShift(ShiftEnum.SecondMiddle),
                new WorkShift(ShiftEnum.SecondEvening),
                new WorkShift(ShiftEnum.Second),
                new WorkShift(ShiftEnum.SecondMiddle),
                new WorkShift(ShiftEnum.SecondEvening),

                new WorkShift(ShiftEnum.SecondEvening),
                new WorkShift(ShiftEnum.Second),
                new WorkShift(ShiftEnum.SecondMiddle),
                new WorkShift(ShiftEnum.SecondEvening),
                new WorkShift(ShiftEnum.Second),

                new WorkShift(ShiftEnum.Third),
                new WorkShift(ShiftEnum.ThirdMiddle),
                new WorkShift(ShiftEnum.ThirdEvening),
                new WorkShift(ShiftEnum.Third),
                new WorkShift(ShiftEnum.ThirdMiddle),

                new WorkShift(ShiftEnum.ThirdMiddle),
                new WorkShift(ShiftEnum.ThirdEvening),
                new WorkShift(ShiftEnum.Third),
                new WorkShift(ShiftEnum.ThirdMiddle),
                new WorkShift(ShiftEnum.ThirdEvening),

                new WorkShift(ShiftEnum.ThirdEvening),
                new WorkShift(ShiftEnum.Third),
                new WorkShift(ShiftEnum.ThirdMiddle),
                new WorkShift(ShiftEnum.ThirdEvening),
                new WorkShift(ShiftEnum.Third),

                new WorkShift(ShiftEnum.Fourth),
                new WorkShift(ShiftEnum.FourthMiddle),
                new WorkShift(ShiftEnum.Shut),
                new WorkShift(ShiftEnum.Fourth),
                new WorkShift(ShiftEnum.FourthMiddle),

                new WorkShift(ShiftEnum.FourthMiddle),
                new WorkShift(ShiftEnum.Shut),
                new WorkShift(ShiftEnum.Fourth),
                new WorkShift(ShiftEnum.FourthMiddle),
                new WorkShift(ShiftEnum.Shut),

                new WorkShift(ShiftEnum.Shut),
                new WorkShift(ShiftEnum.Fourth),
                new WorkShift(ShiftEnum.FourthMiddle),
                new WorkShift(ShiftEnum.Shut),
                new WorkShift(ShiftEnum.Fourth)
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
            rc.DaycareShiftsOfThreeWeeks(dc, 0, wishes);
            var actual = dc.Employees.Select(e => (int)e.Shifts[1].Shift).ToList();

            var expected = new List<int>()
            {
                8, 4, 1, 0, 5, 9, 2, 6, 11, 3, 10, 7
            };
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
