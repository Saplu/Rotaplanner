using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftCalculations;
using System.Collections.Generic;
using Moq;
using System;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class TeamTests
    {
        [TestMethod]
        public void TeamGeneratedCorrectlyWithFalseInput()
        {
            var teams = new List<Team>()
            {
                new Team(5, 2),
                new Team(2, 1),
                new Team(6)
            };

            var dc = new Daycare(teams);
            Assert.AreEqual(0, dc.Teams[0].TeamNumber);
            Assert.AreEqual(1, dc.Teams[1].TeamNumber);
            Assert.AreEqual(2, dc.Teams[2].TeamNumber);

            var teachers = dc.Teams[2].TeamEmp.Where(e => e.Status == StatusEnum.Teacher).Count();
            Assert.AreEqual(1, teachers);
        }
    }
}
