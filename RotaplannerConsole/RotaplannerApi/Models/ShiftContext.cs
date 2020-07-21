using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ShiftCalculations;

namespace RotaplannerApi.Models
{
    public class ShiftContext : DbContext
    {
        public List<Daycare> Daycares { get; set; }
        public ShiftContext(DbContextOptions<ShiftContext> options) : base(options)
        {
            Daycares = new List<Daycare>()
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
        }
    }
}
