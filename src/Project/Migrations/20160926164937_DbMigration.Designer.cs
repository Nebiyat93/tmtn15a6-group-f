using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Project.SQL_Database;

namespace Project.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20160926164937_DbMigration")]
    partial class DbMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Project.Models.Account", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<double?>("Latitude");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<double?>("Longitude");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
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

                    b.Property<string>("AccountId")
                        .IsRequired();

                    b.Property<int>("Created");

                    b.Property<int>("Grade");

                    b.Property<string>("Image")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<int>("RecipeId");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 400);

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Project.Models.Account")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Project.Models.Account")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Project.Models.Account")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Project.Models.AccountRecipe", b =>
                {
                    b.HasOne("Project.Models.Account", "Account")
                        .WithMany("AccountRecipes")
                        .HasForeignKey("AccountId");

                    b.HasOne("Project.Models.Recipe", "Recipe")
                        .WithMany("AccountRecipes")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("Project.Models.Comment", b =>
                {
                    b.HasOne("Project.Models.Account", "Account")
                        .WithMany("Comments")
                        .HasForeignKey("AccountId");

                    b.HasOne("Project.Models.Recipe", "Recipe")
                        .WithMany("Comments")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("Project.Models.Direction", b =>
                {
                    b.HasOne("Project.Models.Recipe", "Recipe")
                        .WithMany("Directions")
                        .HasForeignKey("RecipeId");
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
