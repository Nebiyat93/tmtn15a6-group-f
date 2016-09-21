using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Project.SQL_Database;

namespace Project.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20160921055409_DbMigration")]
    partial class DbMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Project.Models.Account", b =>
                {
                    b.Property<string>("Id");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 12);

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Project.Models.AccountRecipe", b =>
                {
                    b.Property<string>("AccountId");

                    b.Property<int>("RecipeId");

                    b.HasKey("AccountId", "RecipeId");

                    b.HasIndex("AccountId");

                    b.HasIndex("RecipeId");

                    b.ToTable("AccountRecipe");
                });

            modelBuilder.Entity("Project.Models.Comment", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("AccountId");

                    b.Property<string>("AccountId1");

                    b.Property<int>("Created");

                    b.Property<int>("Grade");

                    b.Property<string>("Image")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<int>("RecipeId");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 400);

                    b.HasKey("Id");

                    b.HasIndex("AccountId1");

                    b.HasIndex("RecipeId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Project.Models.Direction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 120);

                    b.Property<int>("Order");

                    b.Property<int>("RecipeId");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("Directions");
                });

            modelBuilder.Entity("Project.Models.Recipe", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("AccountId");

                    b.Property<int>("Created");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 300);

                    b.Property<string>("Image")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 70);

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("Project.Models.AccountRecipe", b =>
                {
                    b.HasOne("Project.Models.Account", "Account")
                        .WithMany("AccountRecipes")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Project.Models.Recipe", "Recipe")
                        .WithMany("AccountRecipes")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Project.Models.Comment", b =>
                {
                    b.HasOne("Project.Models.Account", "Account")
                        .WithMany("Comments")
                        .HasForeignKey("AccountId1");

                    b.HasOne("Project.Models.Recipe", "Recipe")
                        .WithMany("Comments")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Project.Models.Direction", b =>
                {
                    b.HasOne("Project.Models.Recipe", "Recipe")
                        .WithMany("Directions")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Project.Models.Recipe", b =>
                {
                    b.HasOne("Project.Models.Account", "Account")
                        .WithMany("Recipes")
                        .HasForeignKey("AccountId");
                });
        }
    }
}
