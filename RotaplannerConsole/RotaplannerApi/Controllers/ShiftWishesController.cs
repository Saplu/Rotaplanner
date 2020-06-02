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
    public class ShiftWishesController : ControllerBase
    {
        private readonly ShiftContext _context;

        public ShiftWishesController(ShiftContext context)
        {
            _context = context;
        }

        // GET: api/ShiftWishes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShiftWish>>> GetWishes()
        {
            return await _context.Wishes.ToListAsync();
        }

        // GET: api/ShiftWishes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShiftWish>> GetShiftWish(long id)
        {
            var shiftWish = await _context.Wishes.FindAsync(id);

            if (shiftWish == null)
            {
                return NotFound();
            }

            return shiftWish;
        }

        // PUT: api/ShiftWishes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutShiftWish(long id, ShiftWish shiftWish)
        //{
        //    if (id != shiftWish.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(shiftWish).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ShiftWishExists(id))
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

        // POST: api/ShiftWishes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<object>> PostShiftWish(ShiftWish shiftWish)
        {
            _context.Wishes.Add(shiftWish);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDCShifts", new { id = shiftWish.Id }, shiftWish);
        }

        // DELETE: api/ShiftWishes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ShiftWish>> DeleteShiftWish(long id)
        {
            var shiftWish = await _context.Wishes.FindAsync(id);
            if (shiftWish == null)
            {
                return NotFound();
            }

            _context.Wishes.Remove(shiftWish);
            await _context.SaveChangesAsync();

            return shiftWish;
        }

        private bool ShiftWishExists(long id)
        {
            return _context.Wishes.Any(e => e.Id == id);
        }
    }
}
