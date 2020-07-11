using DataTransfer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShiftCalculations
{
    public class DbConnectionHandler
    {
        public async Task<List<Wish>> GetWishes(string set, List<Employee> employees)
        {
            var db = new DataAccess.Mongo();
            var dbWishes = await db.GetWishes(set);
            var wishes = ConvertDTOToWish(dbWishes, employees);
            return wishes;
        }

        public List<Wish>ConvertDTOToWish(List<DTOWish> dtoWishes, List<Employee> employees)
        {
            var list = new List<Wish>();
            dtoWishes.ForEach(w => list.Add(new Wish(employees.Find(e => e.Id == w.EmpId), w.Shift, w.Day)));
            return list;
        }
    }
}
