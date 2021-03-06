﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RotaplannerApi.Models;
using DataTransfer;
using DataAccess;

namespace RotaplannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftWishesController : ControllerBase
    {
        private Mongo _mongo = new Mongo(ConnectionString.Connection);

        public ShiftWishesController()
        {
            
        }

        // GET: api/ShiftWishes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShiftWish>>> GetWishes()
        {
            var dtoWishes = await _mongo.GetWishes();
            var shiftWishes = ConvertDTOToShiftWish(dtoWishes);
            return shiftWishes;
        }

        // GET: api/ShiftWishes/string/string
        [HttpGet("{creator}/{set}")]
        public async Task<ActionResult<IEnumerable<ShiftWish>>> GetShiftWish(string set, string creator)
        {
;
            var dtoWishes = await _mongo.GetWishes(set, creator);
            var shiftWishes = ConvertDTOToShiftWish(dtoWishes);
            return shiftWishes;
        }

        // POST: api/ShiftWishes
        [HttpPost]
        public async Task PostShiftWish(ShiftWish shiftWish)
        {
            var DTOWish = ConvertToDTO(shiftWish);
            await _mongo.CreateWish(DTOWish);
        }

        // DELETE: api/ShiftWishes/string/string
        [HttpDelete("{creator}/{set}")]
        public async Task DeleteShiftWishSet(string creator, string set)
        {
            await _mongo.DeleteWishSet(set, creator);
        }

        //DELETE: api/ShiftWishes/int
        [HttpDelete("{id}")]
        public async Task DeleteWish(long id)
        {
            await _mongo.DeleteWish(id);
        }

        private List<ShiftWish> ConvertDTOToShiftWish(List<DTOWish> dtoWishes)
        {
            var list = new List<ShiftWish>();
            dtoWishes.ForEach(w => list.Add(new ShiftWish(w.EmpId, w.Shift, w.Day, w.Creator, w.Set, w.Id)));
            return list;
        }

        private DTOWish ConvertToDTO(ShiftWish wish)
        {
            var DTOWish = new DTOWish(wish.EmpId, wish.Shift, wish.Day, wish.Creator, wish.Set, wish.Id);
            return DTOWish;
        }
    }
}
