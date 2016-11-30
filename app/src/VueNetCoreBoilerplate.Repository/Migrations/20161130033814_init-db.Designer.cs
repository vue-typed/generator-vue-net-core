using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using VueNetCoreBoilerplate.Repository;
using VueNetCoreBoilerplate.Global.Enums;

namespace VueNetCoreBoilerplate.Repository.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20161130033814_init-db")]
    partial class initdb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VueNetCoreBoilerplate.Domain.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("VueNetCoreBoilerplate.Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PasswordHash");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("VueNetCoreBoilerplate.Domain.Models.UserProfile", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<DateTime?>("Dob");

                    b.Property<string>("Email");

                    b.Property<string>("FullName");

                    b.Property<short>("Gender");

                    b.HasKey("UserId");

                    b.ToTable("UserProfile");
                });

            modelBuilder.Entity("VueNetCoreBoilerplate.Domain.Models.UserRole", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("VueNetCoreBoilerplate.Domain.Models.UserProfile", b =>
                {
                    b.HasOne("VueNetCoreBoilerplate.Domain.Models.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("VueNetCoreBoilerplate.Domain.Models.UserProfile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VueNetCoreBoilerplate.Domain.Models.UserRole", b =>
                {
                    b.HasOne("VueNetCoreBoilerplate.Domain.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VueNetCoreBoilerplate.Domain.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
