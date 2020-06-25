using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftCalculations
{
    public class WorkShift
    {
        public bool Locked { get; set; } = false;
        public int Shift { get; set; }

        public WorkShift(int shift)
        {
            Shift = shift;
        }

        public WorkShift(int shift, bool locked)
        {
            Shift = shift;
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
            return HashCode.Combine(Locked, Shift);
        }
    }
}
