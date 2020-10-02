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
        public long Id { get; set; }

        public DTOWish(int empId, int shift, int day, string creator, string set, long id)
        {
            EmpId = empId;
            Shift = shift;
            Day = day;
            Creator = creator;
            Set = set;
            Id = id;
        }
    }
}
