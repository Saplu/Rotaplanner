using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
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
        private Mongo _mongo = new Mongo(ConnectionString.Connection);

        public DCShiftsController(ShiftContext context)
        {
            _context = context;
            _wishes = new List<Wish>();
            _dc = _context.Daycares[0];
            _calc = new RotationCalculator();
        }

        // GET: api/DCShifts
        [HttpGet]
        public async Task<string> GetDCShifts()
        {
            try
            {
                var numb = _context.DaycareSelector.Count();
                if (_context.DaycareSelector.Count() > 0)
                    _dc = _context.Daycares[_context.DaycareSelector.Last().Dc];
                else _dc = _context.Daycares[0];
                foreach(var item in _context.Wishes)
                {
                    var emp = _dc.Employees.Find(e => e.Id == item.EmpId);
                    _wishes.Add(new Wish(emp, item.Shift, item.Day));
                }
                var group = 0;
                if (_context.Groups.Count() > 0)
                    group = _context.Groups.Last().OpenGroup;
                await _calc.DaycareShiftsOfThreeWeeks(_dc, group, _wishes);
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

        [HttpGet("{id}/{group}/{creator}/{set}")]
        public async Task<string> GetDCShifts(int id, int group, string creator, string set)
        {
            try
            {
                _dc = _context.Daycares[id];
                var dtoWishes = await _mongo.GetWishes(set, creator);
                var wishes = ConvertDTOToWish(dtoWishes, _dc);

                await _calc.DaycareShiftsOfThreeWeeks(_dc, group, wishes);
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

        //[HttpPost]
        //public async Task<ActionResult<object>> PostData(Group group)
        //{
        //    _context.Groups.Add(new Group(group.Id, group.OpenGroup));
        //    await _context.SaveChangesAsync();
        //    return CreatedAtAction("GetDCShifts", new { id = group.Id }, group.OpenGroup);
        //}

        // DELETE: api/DCShifts
        //[HttpDelete()]
        //public async Task<ActionResult<List<Group>>> DeleteGroups()
        //{
        //    var groups = _context.Groups.ToList();
        //    _context.Groups.RemoveRange(_context.Groups);
        //    await _context.SaveChangesAsync();
        //    return groups;
        //}

        //DELETE: api/DCShifts/2
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<ShiftWish>> DeleteWish(long id)
        //{
        //    var wish = await _context.Wishes.FindAsync(id);
        //    if (wish == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Wishes.Remove(wish);
        //    await _context.SaveChangesAsync();
        //    return wish;
        //}

        private bool DCShiftsExists(long id)
        {
            return _context.Wishes.Any(e => e.Id == id);
        }

        private List<Wish> ConvertDTOToWish(List<DataTransfer.DTOWish> dtoWishes, Daycare dc)
        {
            var list = new List<Wish>();
            dtoWishes.ForEach(w => list.Add(new Wish(dc.Employees.Find(e => e.Id == w.EmpId), w.Shift, w.Day)));
            return list;
        }
    }
}
