using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using RotaplannerApi.Models;
using ShiftCalculations;

namespace RotaplannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DCShiftsController : ControllerBase
    {
        private readonly ShiftContext _context;
        private RotationCalculator _calc;
        private Mongo _mongo = new Mongo(ConnectionString.Connection);

        public DCShiftsController(ShiftContext context)
        {
            _context = context;
            _calc = new RotationCalculator();
        }

        [HttpGet("{id}/{group}/{creator}/{set}/{up}")]
        public async Task<string> GetDCShifts(int id, int group, string creator, string set, int up)
        {
            try
            {
                var dc = _context.Daycares[id];
                var dtoWishes = await _mongo.GetWishes(set, creator);
                var wishes = ConvertDTOToWish(dtoWishes, dc);

                await _calc.DaycareShiftsOfThreeWeeks(dc, group, wishes, up);
                var allShifts = "";
                foreach (var team in dc.Teams)
                {
                    foreach (var emp in team.TeamEmp)
                    {
                        var shifts = emp.Id.ToString() + " " + emp.Status.ToString() + ": ";
                        foreach (var shift in emp.Shifts)
                        {
                            var num = Convert.ToInt32(shift.Shift);
                            shifts += num + ((num > 9) ? " " : "  ");
                        }
                        allShifts += ("\n" + shifts);
                    }
                }
                return allShifts;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private List<Wish> ConvertDTOToWish(List<DataTransfer.DTOWish> dtoWishes, Daycare dc)
        {
            var list = new List<Wish>();
            dtoWishes.ForEach(w => list.Add(new Wish(dc.Employees.Find(e => e.Id == w.EmpId), w.Shift, w.Day)));
            return list;
        }
    }
}
