using CarApi.Models;
using Microsoft.EntityFrameworkCore;
using SpendingsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionsApi.Models;

namespace TransactionsApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<UserCars>()
        //        .HasNoKey();
        //}

        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<UserCars> UserCars { get; set; }
        public DbSet<Car> Car { get; set; }
    }
}

