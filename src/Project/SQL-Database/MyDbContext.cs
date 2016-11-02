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
    public class MyDbContext : IdentityDbContext<AccountIdentity>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connString = "Data Source=groupfinstance.cnycsjrffnil.eu-central-1.rds.amazonaws.com;Integrated Security=False;User ID=awsMjecipes;Password=******;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Initial Catalog=groupfinstance";
            optionsBuilder.UseSqlServer(connString);
        }

        /// <summary>  
        /// Overriding Save Changes  
        /// </summary>  
        /// <returns></returns>  
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccountIdentity>().HasMany(pt => pt.Comments).WithOne(c => c.AccountIdentity).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AccountIdentity>().HasMany(pt => pt.Recipes).WithOne(r => r.AccountIdentity).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AccountIdentity>().HasMany(pt => pt.Favorites).WithOne(pt => pt.AccountIdentity);/*.OnDelete(DeleteBehavior.Cascade);*/
            modelBuilder.Entity<Recipe>().HasOne(pt => pt.AccountIdentity).WithMany(r => r.Recipes);
            modelBuilder.Entity<Recipe>().HasMany(c => c.Comments).WithOne(r => r.Recipe).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Recipe>().HasMany(d => d.Directions).WithOne(r => r.Recipe);
            modelBuilder.Entity<Comment>().HasOne(pt => pt.Recipe).WithMany(c => c.Comments);
            modelBuilder.Entity<Comment>().HasOne(pt => pt.AccountIdentity).WithMany(c => c.Comments);

            modelBuilder.Entity<Direction>().HasOne(pt => pt.Recipe).WithMany(d => d.Directions);

            modelBuilder.Entity<Favorites>().HasKey(ar => new { ar.AccountId, ar.RecipeId });
            modelBuilder.Entity<Favorites>().HasOne(pt => pt.AccountIdentity).WithMany(pt => pt.Favorites).OnDelete(DeleteBehavior.Restrict).HasForeignKey(f => f.AccountId);
            modelBuilder.Entity<Favorites>().HasOne(pt => pt.Recipe).WithMany(pt => pt.Favorites).OnDelete(DeleteBehavior.Restrict).HasForeignKey(f => f.RecipeId);

        }

        
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Direction> Directions { get; set; }
        
    }
}
