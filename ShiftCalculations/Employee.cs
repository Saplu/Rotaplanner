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
        int _team;

        public int Id { get => _id; }
        public StatusEnum Status { get => _status; }
        public List<WorkShift> Shifts { get => _shifts; }
        public int Team { get => _team; }

        public Employee(int id, StatusEnum status)
        {
            _id = id;
            _status = status;
            _shifts = new List<WorkShift>();
            _team = Convert.ToInt32(Math.Floor((decimal)id / 3));
            //if (id <= 2)
            //    _team = 0;
            //else if (id <= 5)
            //    _team = 1;
            //else if (id <= 8)
            //    _team = 2;
            //else _team = 3;
        }

        public override bool Equals(object obj)
        {
            var toCompareWith = obj as Employee;
            if (toCompareWith != null)
                return toCompareWith.Id == this.Id;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _status, _shifts, _team, Id, Status, Shifts, Team);
        }
    }
}
