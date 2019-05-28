using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Walton_Happy_Travel.Models;

namespace Walton_Happy_Travel.Data
{
    /// <summary>
    /// Context for the database. Interacts with the database
    /// </summary>
    /// <typeparam name="ApplicationUser"></typeparam>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Accomodation> Accomodations { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Brochure> Brochures { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Country> Countrys { get; set; }
        public DbSet<PaymentInfo> PaymentInfos { get; set; }
        public DbSet<Person> Persons { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Hotel>();
            builder.Entity<Private>();
            builder.Entity<Staff>();
            builder.Entity<Customer>();

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Walton_Happy_Travel.Models.Hotel> Hotel { get; set; }

        public DbSet<Walton_Happy_Travel.Models.Private> Private { get; set; }
    }
}
