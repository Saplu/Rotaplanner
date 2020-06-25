﻿using System;
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
        private List<Daycare> _daycares;
        private Daycare _dc;
        private RotationCalculator _calc;

        public DCShiftsController(ShiftContext context)
        {
            _context = context;
            _daycares = new List<Daycare>()
            {
                new Daycare(new List<Team>()
                {
                    new Team(0, 2),
                    new Team(1),
                    new Team(2, 2),
                    new Team(3)
                }),
                new Daycare(new List<Team>()
                {
                    new Team(0, 2),
                    new Team(1)
                })
            };
            _wishes = new List<Wish>();
            _dc = _daycares[0];
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
                var group = 0;
                foreach (var item in _context.Groups)
                {
                    group = item.OpenGroup;
                }
                _calc.DaycareShiftsOfThreeWeeks(_dc, group, _wishes);
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

        [HttpPost]
        public async Task<ActionResult<object>> PostData(Group group)
        {
            _context.Groups.Add(new Group(group.Id, group.OpenGroup));
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDCShifts", new { id = group.Id }, group.OpenGroup);
        }

        // DELETE: api/DCShifts
        [HttpDelete()]
        public async Task<ActionResult<List<ShiftWish>>> DeleteWishes()
        {
            var wishes = _context.Wishes.ToList();
            _context.Wishes.RemoveRange(_context.Wishes);
            await _context.SaveChangesAsync();
            return wishes;
        }

        //DELETE: api/DCShifts/2
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
