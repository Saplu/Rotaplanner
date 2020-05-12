using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RotaplannerApi.Models;
using ShiftCalculations;

namespace RotaplannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DCShiftsController : ControllerBase
    {
        private readonly ShiftContext _context;
        private List<Wish> _wishes;
        private Daycare _dc;
        private RotationCalculator _calc;

        public DCShiftsController(ShiftContext context)
        {
            _context = context;
            _wishes = new List<Wish>();
            _dc = new Daycare();
            _calc = new RotationCalculator();
        }

        // GET: api/DCShifts
        [HttpGet]
        public string GetDCShifts()
        {
            try
            {
                foreach(var item in _context.Wishes)
                {
                    var emp = _dc.Employees.Find(e => e.Id == item.EmpId);
                    _wishes.Add(new Wish(emp, item.Shift, item.Day));
                }
                _calc.DaycareShiftsOfThreeWeeks(_dc, 2, _wishes);
                var allShifts = "";
                foreach (var team in _dc.Teams)
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

        // POST: api/DCShifts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ShiftWish>> PostWish(ShiftWish wish)
        {

            _context.Wishes.Add(wish);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDCShifts", new { id = wish.Id }, wish);
        }

        // DELETE: api/DCShifts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ShiftWish>> DeleteWish(long id)
        {
            var wish = await _context.Wishes.FindAsync(id);
            if (wish == null)
            {
                return NotFound();
            }

            _context.Wishes.Remove(wish);
            await _context.SaveChangesAsync();

            return wish;
        }

        private bool DCShiftsExists(long id)
        {
            return _context.Wishes.Any(e => e.Id == id);
        }
    }
}
