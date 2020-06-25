using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RotaplannerApi.Models;

namespace RotaplannerApi.Models
{
    public class ShiftContext : DbContext
    {
        public ShiftContext(DbContextOptions<ShiftContext> options) : base(options)
        {
        }

        public DbSet<ShiftWish> Wishes { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<DaycareSelector> DaycareSelector { get; set; }
    }
}
