using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShiftCalculations;

namespace RotaplannerApi.Models
{
    public class ShiftWish
    {
        public long Id { get; set; }
        public int? EmpId { get; set; }
        public int? Shift { get; set; }
        public int? Day { get; set; }

        public int? OpenGroup { get; set; }
    }
}
