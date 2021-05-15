using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpendingsApi.Models;

namespace SpendingsApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<UserCars>()
        //        .HasNoKey();
        //}

        public DbSet<SpendingsApi.Models.Spendings> Spendings { get; set; }
        public DbSet<CarApi.Models.Car> Car { get; set; }
        public DbSet<CostsApi.Models.Costs> Costs { get; set; }
        public DbSet<UserCars> UserCars { get; set; }
        public DbSet<Log> Logs { get; set; }

    }
}