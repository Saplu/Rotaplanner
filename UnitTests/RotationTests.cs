using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftCalculations;
using System.Collections.Generic;

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
    }
}
