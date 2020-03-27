using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftCalculations
{
    public class WorkShift
    {
        ShiftEnum _shift;
        public bool Locked { get; set; } = false;
        public ShiftEnum Shift { get => _shift; set => _shift = value; }

        public WorkShift(ShiftEnum shift)
        {
            _shift = shift;
        }

        public WorkShift(ShiftEnum shift, bool locked)
        {
            _shift = shift;
            Locked = locked;
        }

        public override bool Equals(object obj)
        {
            var toCompareWith = obj as WorkShift;
            if (toCompareWith != null)
                return (toCompareWith.Locked == this.Locked && toCompareWith.Shift == this.Shift);
            else return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_shift, Locked, Shift);
        }
    }
}
