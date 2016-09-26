using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Project.SQL_Database
{
    public class MyDbContext : IdentityDbContext<Account>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MjecipeDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            optionsBuilder.UseSqlServer(connString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>().HasMany(pt => pt.Comments).WithOne(c => c.Account).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Account>().HasMany(pt => pt.Recipes).WithOne(r => r.Account).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Account>().HasMany(pt => pt.AccountRecipes).WithOne(pt => pt.Account).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AccountRecipe>().HasKey(ar => new { ar.AccountId, ar.RecipeId });
            modelBuilder.Entity<AccountRecipe>().HasOne(pt => pt.Account).WithMany(pt => pt.AccountRecipes).OnDelete(DeleteBehavior.Restrict).HasForeignKey(f => f.AccountId);

            modelBuilder.Entity<Recipe>().HasOne(pt => pt.Account).WithMany(r => r.Recipes).OnDelete(DeleteBehavior.Restrict).HasForeignKey(f=>f.AccountId);
            modelBuilder.Entity<Recipe>().HasMany(c => c.Comments).WithOne(r => r.Recipe).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Recipe>().HasMany(d => d.Directions).WithOne(r => r.Recipe).OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<AccountRecipe>().HasOne(pt => pt.Recipe).WithMany(pt => pt.AccountRecipes).OnDelete(DeleteBehavior.Restrict).HasForeignKey(f => f.RecipeId);

            modelBuilder.Entity<Comment>().HasOne(pt => pt.Recipe).WithMany(c => c.Comments).OnDelete(DeleteBehavior.Restrict).HasForeignKey(f => f.RecipeId);
            modelBuilder.Entity<Comment>().HasOne(pt => pt.Account).WithMany(c => c.Comments).OnDelete(DeleteBehavior.Restrict).HasForeignKey(f => f.AccountId);

            modelBuilder.Entity<Direction>().HasOne(pt => pt.Recipe).WithMany(d => d.Directions).OnDelete(DeleteBehavior.Restrict).HasForeignKey(f => f.RecipeId);
        }
        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Direction> Directions { get; set; }
        
    }
}
