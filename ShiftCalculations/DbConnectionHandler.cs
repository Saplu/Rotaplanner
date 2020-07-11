using DataTransfer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShiftCalculations
{
    public class DbConnectionHandler
    {
        readonly DataAccess.Mongo db = new DataAccess.Mongo();
        public async Task<List<Wish>> GetWishes(string set, string creator, List<Employee> employees)
        {
            var dbWishes = await db.GetWishes(set, creator);
            var wishes = ConvertDTOToWish(dbWishes, employees);
            return wishes;
        }

        public async Task<List<DTOWish>> GetWishes(string set, string creator)
        {
            var dbWishes = await db.GetWishes(set, creator);
            return dbWishes;
        }

        public async Task<List<DTOWish>> GetWishes()
        {
            var dbWishes = await db.GetWishes();
            return dbWishes;
        }

        public async Task PostWish(DTOWish wish)
        {
            await db.CreateWish(wish);
        }

        private List<Wish>ConvertDTOToWish(List<DTOWish> dtoWishes, List<Employee> employees)
        {
            var list = new List<Wish>();
            dtoWishes.ForEach(w => list.Add(new Wish(employees.Find(e => e.Id == w.EmpId), w.Shift, w.Day)));
            return list;
        }
    }
}
