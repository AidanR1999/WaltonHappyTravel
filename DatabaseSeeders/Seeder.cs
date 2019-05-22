using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json;
using Walton_Happy_Travel.Models;
using Walton_Happy_Travel.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;

namespace Walton_Happy_Travel.DatabaseSeeders
{
    public class Seeder 
    {
        private static ApplicationDbContext _context;
        private static UserManager<ApplicationUser> _userManager;
        private static RoleManager<IdentityRole> _roleManager;

        public Seeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public static async Task Initialise()
        {
            await _context.Database.EnsureCreatedAsync();

            if(!_context.UserRoles.Any())
                await RoleSeed();

            if(!_context.Users.Any()) 
            {
                await UserSeed();
                await AddUsersToRoles();
            }

            if(!await _context.Brochures.AnyAsync())
            {
                await CountrySeed();
                await CategorySeed();
                await HotelSeed();
                await PrivateAccomodationSeed();
                await BrochureSeed();
            } 
        }

        private static async Task RoleSeed()
        {
            if(!await _roleManager.RoleExistsAsync("Customer")) await _roleManager.CreateAsync(new IdentityRole("Customer"));
            if(!await _roleManager.RoleExistsAsync("Admin")) await _roleManager.CreateAsync(new IdentityRole("Admin"));
            if(!await _roleManager.RoleExistsAsync("ShopManager")) await _roleManager.CreateAsync(new IdentityRole("ShopManager"));
            if(!await _roleManager.RoleExistsAsync("AssistantManager")) await _roleManager.CreateAsync(new IdentityRole("AssistantManager"));
            if(!await _roleManager.RoleExistsAsync("TravelAssistant")) await _roleManager.CreateAsync(new IdentityRole("TravelAssistant"));
            if(!await _roleManager.RoleExistsAsync("InvoiceClerk")) await _roleManager.CreateAsync(new IdentityRole("InvoiceClerk"));
            await _context.SaveChangesAsync();
        }

        private static async Task UserSeed()
        {
            var userdata = System.IO.File.ReadAllText(@"DatabaseSeeders/StaffData.json");
            var users = JsonConvert.DeserializeObject<List<Staff>>(userdata);

            foreach(var user in users)
            {
                if(await _userManager.FindByEmailAsync(user.Email) == null)
                {
                    user.EmailConfirmed = true;
                    user.TimeOfRegistration = DateTime.Now;
                    user.UserName = user.Email;
                    await _userManager.CreateAsync(user, "Password1!");
                }
            }
            await _context.SaveChangesAsync();
        }

        private static async Task AddUsersToRoles()
        {
            var sysAdmin = await _userManager.FindByEmailAsync("sysadmin@whtravel.com");
            await _userManager.AddToRoleAsync(sysAdmin, "Admin");

            var shopManager = await _userManager.FindByEmailAsync("shopmanager@whtravel.com");
            await _userManager.AddToRoleAsync(shopManager, "ShopManager");

            var assistantMan = await _userManager.FindByEmailAsync("assistantman@whtravel.com");
            await _userManager.AddToRoleAsync(assistantMan, "AssistantManager");

            var travelAssistant = await _userManager.FindByEmailAsync("travelassistant@whtravel.com");
            await _userManager.AddToRoleAsync(travelAssistant, "TravelAssistant");

            var invoiceClerk = await _userManager.FindByEmailAsync("invoiceclerk@whtravel.com");
            await _userManager.AddToRoleAsync(invoiceClerk, "InvoiceClerk");
        }

        private static async Task CountrySeed()
        {
            var file = System.IO.File.ReadAllText(@"DatabaseSeeders/CountryData.json");
            var countries = JsonConvert.DeserializeObject<List<Country>>(file);

            foreach(var country in countries)
            {
                if(await _context.Countrys.Where(c => c.CountryName.Equals(country.CountryName)).FirstOrDefaultAsync() == null)
                {
                    await _context.Countrys.AddAsync(country);
                }
            }
            await _context.SaveChangesAsync();
        }

        private static async Task CategorySeed()
        {
            var file = System.IO.File.ReadAllText(@"DatabaseSeeders/CategoryData.json");
            var categories = JsonConvert.DeserializeObject<List<Category>>(file);

            foreach(var category in categories)
            {
                if(await _context.Categorys.Where(c => c.CategoryName.Equals(category.CategoryName)).FirstOrDefaultAsync() == null)
                {
                    await _context.Categorys.AddAsync(category);
                }
            }
            await _context.SaveChangesAsync();
        }

        private static async Task HotelSeed()
        {
            var file = System.IO.File.ReadAllText(@"DatabaseSeeders/HotelData.json");
            var hotels = JsonConvert.DeserializeObject<List<Hotel>>(file);

            foreach(var hotel in hotels)
            {
                if(await _context.Accomodations.Where(h => h.AccomodationName.Equals(hotel.AccomodationName)).FirstOrDefaultAsync() == null)
                {
                    await _context.Accomodations.AddAsync(hotel);
                }
            }
            await _context.SaveChangesAsync();
        }

        private static async Task PrivateAccomodationSeed()
        {
            var file = System.IO.File.ReadAllText(@"DatabaseSeeders/PrivateAccomodationData.json");
            var privateAccomodations = JsonConvert.DeserializeObject<List<Private>>(file);

            foreach(var privateAccomodation in privateAccomodations)
            {
                if(await _context.Accomodations.Where(h => h.AccomodationName.Equals(privateAccomodation.AccomodationName)).FirstOrDefaultAsync() == null)
                {
                    await _context.Accomodations.AddAsync(privateAccomodation);
                }
            }
            await _context.SaveChangesAsync();
        }

        private static async Task BrochureSeed()
        {
            var file = System.IO.File.ReadAllText(@"DatabaseSeeders/BrochureData.json");
            var brochures = JsonConvert.DeserializeObject<List<Brochure>>(file);

            foreach(var brochure in brochures)
            {
                await _context.Brochures.AddAsync(brochure);
            }
            await _context.SaveChangesAsync();
        }
    }
}