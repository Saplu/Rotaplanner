using Microsoft.AspNetCore.Razor.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RotaplannerApi.Models
{
    public class Shifts
    {
        public List<string> AllShifts { get; set; }

        private List<string> _noEmptyLines;

        public Shifts()
        {
            AllShifts = new List<string>();
            _noEmptyLines = new List<string>();
        }

        public void Add(string item)
        {
            if (_noEmptyLines.Count % 3 == 0 && _noEmptyLines.Count != 0)
            {
                AllShifts.Add(null);
            }
            AllShifts.Add(item);
            _noEmptyLines.Add(item);
        }
    }
}
