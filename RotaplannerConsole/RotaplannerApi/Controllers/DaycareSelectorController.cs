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

        //// GET: api/DaycareSelector
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<DaycareSelector>>> GetDaycareSelector()
        //{
        //    return await _context.DaycareSelector.ToListAsync();
        //}

        //// GET: api/DaycareSelector/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<DaycareSelector>> GetDaycareSelector(int id)
        //{
        //    var daycareSelector = await _context.DaycareSelector.FindAsync(id);

        //    if (daycareSelector == null)
        //    {
        //        return NotFound();
        //    }

        //    return daycareSelector;
        //}

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
