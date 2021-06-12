using Microsoft.EntityFrameworkCore;
using System;
using PublicHolidays.Data.Models;

namespace PublicHolidays.Data
{
    public class PublicHolidaysContext : DbContext
    {
        public PublicHolidaysContext(DbContextOptions options) : base(options){ }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Holiday> Holidays { get; set; }

    }
}
