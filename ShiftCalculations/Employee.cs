using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftCalculations
{
    public class Employee
    {
        int _id;
        StatusEnum _status;
        List<WorkShift> _shifts;

        public int Id { get => _id; }
        public StatusEnum Status { get => _status; }
        public List<WorkShift> Shifts { get => _shifts; }

        public Employee(int id, StatusEnum status)
        {
            _id = id;
            _status = status;
            _shifts = new List<WorkShift>();
        }
    }
}
