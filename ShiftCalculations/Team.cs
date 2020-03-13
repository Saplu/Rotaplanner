using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftCalculations
{
    public class Team
    {
        public List<Employee> TeamEmp { get; }
        public int TeamNumber { get; }

        public Team(int teamNumber, int teachers = 1)
        {
            TeamNumber = teamNumber;
            TeamEmp = new List<Employee>();
            switch(teachers)
            {
                case 1: TeamEmp.Add(new Employee(3 * teamNumber, StatusEnum.Teacher));
                    TeamEmp.Add(new Employee(3 * teamNumber + 1, StatusEnum.Nurse));
                    TeamEmp.Add(new Employee(3 * teamNumber + 2, StatusEnum.Nurse));
                    break;
                case 2: TeamEmp.Add(new Employee(3 * teamNumber, StatusEnum.Teacher));
                    TeamEmp.Add(new Employee(3 * teamNumber + 1, StatusEnum.Teacher));
                    TeamEmp.Add(new Employee(3 * teamNumber + 2, StatusEnum.Nurse));
                    break;
                default: throw new ArgumentException("Must be either 1 or 2 teachers per team.");
            }
        }
    }
}
