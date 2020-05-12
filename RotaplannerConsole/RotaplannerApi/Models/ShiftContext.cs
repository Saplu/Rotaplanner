﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RotaplannerApi.Models
{
    public class ShiftContext : DbContext
    {
        public ShiftContext(DbContextOptions<ShiftContext> options) : base(options)
        {

        }

        public DbSet<ShiftWish> Wishes { get; set; }
    }
}
