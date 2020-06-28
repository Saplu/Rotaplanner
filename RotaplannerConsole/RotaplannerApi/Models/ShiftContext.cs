using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RotaplannerApi.Models;
using ShiftCalculations;

namespace RotaplannerApi.Models
{
    public class ShiftContext : DbContext
    {
        public List<Daycare> Daycares { get; set; }
        public int CurrentDc { get; set; }
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

        public DbSet<ShiftWish> Wishes { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<DaycareSelector> DaycareSelector { get; set; }
    }
}
