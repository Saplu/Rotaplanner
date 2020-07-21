using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<int[]>> GetDaycareSelector(int id)
        {
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
    }
}
