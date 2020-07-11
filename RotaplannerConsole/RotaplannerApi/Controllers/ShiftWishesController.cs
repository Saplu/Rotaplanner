using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RotaplannerApi.Models;
using System.Text.Json;
using ShiftCalculations;
using DataTransfer;

namespace RotaplannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftWishesController : ControllerBase
    {
        private readonly ShiftContext _context;
        private DbConnectionHandler _dbConn = new DbConnectionHandler();

        public ShiftWishesController(ShiftContext context)
        {
            _context = context;
        }

        // GET: api/ShiftWishes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShiftWish>>> GetWishes()
        {
            var dtoWishes = await _dbConn.GetWishes();
            var shiftWishes = ConvertDTOToShiftWish(dtoWishes);
            return shiftWishes;
        }

        // GET: api/ShiftWishes/string/string
        [HttpGet("{creator}/{set}")]
        public async Task<ActionResult<IEnumerable<ShiftWish>>> GetShiftWish(string set, string creator)
        {
            var dtoWishes = await _dbConn.GetWishes(set, creator);
            var shiftWishes = ConvertDTOToShiftWish(dtoWishes);
            return shiftWishes;
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
        public async Task PostShiftWish(ShiftWish shiftWish)
        {
            var DTOWish = ConvertToDTO(shiftWish);
            //_context.Wishes.Add(shiftWish);
            //await _context.SaveChangesAsync();
            await _dbConn.PostWish(DTOWish);
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

        [HttpDelete]
        public async Task<ActionResult<List<ShiftWish>>> DeleteWishes()
        {
            var wishes = _context.Wishes.ToList();
            _context.Wishes.RemoveRange(_context.Wishes);
            await _context.SaveChangesAsync();
            return wishes;
        }

        private bool ShiftWishExists(long id)
        {
            return _context.Wishes.Any(e => e.Id == id);
        }

        private List<ShiftWish> ConvertDTOToShiftWish(List<DTOWish> dtoWishes)
        {
            var list = new List<ShiftWish>();
            dtoWishes.ForEach(w => list.Add(new ShiftWish(w.EmpId, w.Shift, w.Day, w.Creator, w.Set)));
            return list;
        }

        private DTOWish ConvertToDTO(ShiftWish wish)
        {
            var DTOWish = new DTOWish(wish.EmpId, wish.Shift, wish.Day, wish.Creator, wish.Set);
            return DTOWish;
        }
    }
}
