using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftCalculations;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace UnitTests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void ShiftsGeneratesCorrectList()
        {
            var shifts = new RotaplannerApi.Models.Shifts();
            shifts.Add("asd");
            shifts.Add("dsad");
            shifts.Add("asdsad");
            shifts.Add("sada");
            shifts.Add("gfdsg");
            shifts.Add("2safd");
            shifts.Add("fdsag");

            var actual = shifts.AllShifts.Count();
            Assert.AreEqual(actual, 9);
        }
    }
}
