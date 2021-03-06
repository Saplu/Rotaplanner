﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<Shifts> GetDCShifts(int id, int group, string creator, string set, int up)
        {
            try
            {
                var dc = _context.Daycares[id];
                var dtoWishes = await _mongo.GetWishes(set, creator);
                var wishes = ConvertDTOToWish(dtoWishes, dc);

                await _calc.DaycareShiftsOfThreeWeeks(dc, group, wishes, up);
                var dcShifts = new Shifts();
                foreach (var team in dc.Teams)
                {
                    List<Employee> sorted = team.TeamEmp.OrderBy(e => e.Id).ToList();
                    foreach (var emp in sorted)
                    {
                        var shifts = emp.Status == StatusEnum.Nurse ?
                            $"{emp.Id} {emp.Status}:   " :
                            $"{emp.Id} {emp.Status}: ";

                        foreach (var shift in emp.Shifts)
                        {
                            var num = Convert.ToInt32(shift.Shift) + 1;
                            shifts += num + ((num > 9) ? " " : "  ");
                        }
                        dcShifts.Add(shifts.Trim());
                    }
                }
                return dcShifts;
            }
            catch (Exception ex)
            {
                var fail = new Shifts();
                fail.AllShifts.Add(ex.Message);
                return fail;
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
