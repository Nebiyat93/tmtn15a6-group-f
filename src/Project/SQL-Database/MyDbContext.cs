using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;

namespace Project.SQL_Database
{
    public class MyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MjecipeDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            optionsBuilder.UseSqlServer(connString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountRecipe>().HasKey(pt => new { pt.AccountId, pt.RecipeId });

            modelBuilder.Entity<AccountRecipe>().HasOne(pt => pt.Account).WithMany(pt => pt.Favorites).HasForeignKey(pt => pt.AccountId);
            modelBuilder.Entity<AccountRecipe>().HasOne(pt => pt.Recipe).WithMany(pt => pt.Favorites).HasForeignKey(pt => pt.RecipeId);
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Direction> Directions { get; set; }
        
    }
}
