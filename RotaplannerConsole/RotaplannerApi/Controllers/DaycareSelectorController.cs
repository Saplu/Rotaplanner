using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RotaplannerApi.Models;

namespace RotaplannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DaycareSelectorController : ControllerBase
    {
        private readonly ShiftContext _context;

        public DaycareSelectorController(ShiftContext context)
        {
            _context = context;
        }

        // GET: api/DaycareSelector
        [HttpGet]
        public async Task<ActionResult<int>> GetDaycareSelector()
        {
            var value = 0;
            if (_context.DaycareSelector.Count() > 0)
                value = _context.DaycareSelector.Last().Dc;
            //var value = new int[_context.Daycares[_context.CurrentDc].Teams.Count];
            //for (int i = 0; i < value.Length; i++)
            //{
            //    value[i] = i;
            //}
            return value;
        }

        // GET: api/DaycareSelector/5
        [HttpGet("{id}")]
        public async Task<ActionResult<int[]>> GetDaycareSelector(int id)
        {
            _context.CurrentDc = id;
            try
            {
                if (_context.Daycares[id] != null)
                {
                    var value = new int[_context.Daycares[id].Teams.Count];
                    for (int i = 0; i < value.Length; i++)
                    {
                        value[i] = i;
                    }
                    return value;
                }
                else return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
        }

        //// PUT: api/DaycareSelector/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutDaycareSelector(int id, DaycareSelector daycareSelector)
        //{
        //    if (id != daycareSelector.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(daycareSelector).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DaycareSelectorExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/DaycareSelector
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<int> PostDaycareSelector(DaycareSelector daycareSelector)
        {
            _context.CurrentDc = daycareSelector.Dc;
            _context.DaycareSelector.Add(daycareSelector);
            await _context.SaveChangesAsync();

            return daycareSelector.Dc;
        }

        //// DELETE: api/DaycareSelector/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<DaycareSelector>> DeleteDaycareSelector(int id)
        //{
        //    var daycareSelector = await _context.DaycareSelector.FindAsync(id);
        //    if (daycareSelector == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.DaycareSelector.Remove(daycareSelector);
        //    await _context.SaveChangesAsync();

        //    return daycareSelector;
        //}

        private bool DaycareSelectorExists(int id)
        {
            return _context.DaycareSelector.Any(e => e.Id == id);
        }
    }
}
