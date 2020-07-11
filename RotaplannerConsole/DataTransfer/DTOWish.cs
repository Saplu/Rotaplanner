using System;

namespace DataTransfer
{
    public class DTOWish
    {
        public int EmpId { get; set; }
        public int Shift { get; set; }
        public int Day { get; set; }
        public string Creator { get; set; }
        public string Set { get; set; }

        public DTOWish(int id, int shift, int day, string creator, string set)
        {
            EmpId = id;
            Shift = shift;
            Day = day;
            Creator = creator;
            Set = set;
        }
    }
}
