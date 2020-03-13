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

        public WorkShift()
        {
            _shift = ShiftEnum.Middle;
        }

        public WorkShift(ShiftEnum shift)
        {
            _shift = shift;
        }

        public WorkShift(ShiftEnum shift, bool locked)
        {
            _shift = shift;
            Locked = locked;
        }
    }
}
