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
    public class EqualityTests
    {
        [TestMethod]
        public void EmployeesWithSameIdAreEqual()
        {
            var emp1 = new Employee(1, StatusEnum.Nurse);
            var emp2 = new Employee(1, StatusEnum.Teacher);

            Assert.AreEqual(emp1, emp2);
        }

        [TestMethod]
        public void WorkShiftsWithOnlyLockedAsDifferenceAreNotEqual()
        {
            var shift1 = new WorkShift(1, false);
            var shift2 = new WorkShift(1, true);
            var actual = shift1.Equals(shift2);
            Assert.AreEqual(false, actual);
        }
    }
}
