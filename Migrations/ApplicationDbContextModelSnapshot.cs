﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using Walton_Happy_Travel.Data;
using Walton_Happy_Travel.Models;

namespace WaltonHappyTravel.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.Accomodation", b =>
                {
                    b.Property<int>("AccomodationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccomodationAddress");

                    b.Property<string>("AccomodationName");

                    b.Property<int>("CountryId");

                    b.Property<string>("Description");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.HasKey("AccomodationId");

                    b.HasIndex("CountryId");

                    b.ToTable("Accomodations");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Accomodation");
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("Forename");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("MiddleNames");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Surname");

                    b.Property<DateTime>("TimeOfRegistration");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ApplicationUser");
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.Booking", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("AmountPaid");

                    b.Property<int>("BrochureId");

                    b.Property<int>("NoOfRooms");

                    b.Property<int>("PaymentType");

                    b.Property<string>("SpecialRequirements");

                    b.Property<double>("TotalPrice");

                    b.Property<string>("UserId");

                    b.HasKey("BookingId");

                    b.HasIndex("BrochureId");

                    b.HasIndex("UserId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.Brochure", b =>
                {
                    b.Property<int>("BrochureId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccomodationId");

                    b.Property<int>("CategoryId");

                    b.Property<int>("Catering");

                    b.Property<DateTime>("DepartureDate");

                    b.Property<string>("Description");

                    b.Property<int>("Duration");

                    b.Property<int>("MaxPeople");

                    b.Property<int>("MaxRooms");

                    b.Property<double>("PricePerPerson");

                    b.HasKey("BrochureId");

                    b.HasIndex("AccomodationId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Brochures");
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CategoryName");

                    b.HasKey("CategoryId");

                    b.ToTable("Categorys");
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CountryName");

                    b.HasKey("CountryId");

                    b.ToTable("Countrys");
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.PaymentInfo", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("CardNumber");

                    b.Property<int>("CardType");

                    b.Property<DateTime>("ExpiryDate");

                    b.Property<string>("NameOnCard");

                    b.Property<string>("SecurityNumber");

                    b.Property<string>("UserId");

                    b.HasKey("PaymentId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("PaymentInfos");
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookingId");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Forename");

                    b.Property<string>("MiddleNames");

                    b.Property<string>("Surname");

                    b.HasKey("PersonId");

                    b.HasIndex("BookingId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.Hotel", b =>
                {
                    b.HasBaseType("Walton_Happy_Travel.Models.Accomodation");

                    b.Property<int>("Rating");

                    b.ToTable("Hotel");

                    b.HasDiscriminator().HasValue("Hotel");
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.Private", b =>
                {
                    b.HasBaseType("Walton_Happy_Travel.Models.Accomodation");


                    b.ToTable("Private");

                    b.HasDiscriminator().HasValue("Private");
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.Customer", b =>
                {
                    b.HasBaseType("Walton_Happy_Travel.Models.ApplicationUser");


                    b.ToTable("Customer");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.Staff", b =>
                {
                    b.HasBaseType("Walton_Happy_Travel.Models.ApplicationUser");


                    b.ToTable("Staff");

                    b.HasDiscriminator().HasValue("Staff");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Walton_Happy_Travel.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Walton_Happy_Travel.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Walton_Happy_Travel.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Walton_Happy_Travel.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.Accomodation", b =>
                {
                    b.HasOne("Walton_Happy_Travel.Models.Country", "Country")
                        .WithMany("Accomodations")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.Booking", b =>
                {
                    b.HasOne("Walton_Happy_Travel.Models.Brochure", "Brochure")
                        .WithMany("Bookings")
                        .HasForeignKey("BrochureId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Walton_Happy_Travel.Models.ApplicationUser", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.Brochure", b =>
                {
                    b.HasOne("Walton_Happy_Travel.Models.Accomodation", "Accomodation")
                        .WithMany("Brochures")
                        .HasForeignKey("AccomodationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Walton_Happy_Travel.Models.Category", "Category")
                        .WithMany("Brochures")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.PaymentInfo", b =>
                {
                    b.HasOne("Walton_Happy_Travel.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("Walton_Happy_Travel.Models.Person", b =>
                {
                    b.HasOne("Walton_Happy_Travel.Models.Booking", "Booking")
                        .WithMany("Persons")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
