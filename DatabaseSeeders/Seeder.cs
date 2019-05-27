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
    /// <summary>
    /// on launch populate the database
    /// </summary>
    public class Seeder 
    {
        /// <summary>
        /// Context for database
        /// </summary>
        private static ApplicationDbContext _context;

        /// <summary>
        /// Usermanager to allow for user creation
        /// </summary>
        private static UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Rolemanager to allow for role creation
        /// </summary>
        private static RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Uses dependency injection to initialise the parameters
        /// </summary>
        /// <param name="userManager">User Manager</param>
        /// <param name="roleManager">Role Manager</param>
        /// <param name="context">Context for database</param>
        public Seeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            //initialise global variables
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Calls all other methods to populate the database
        /// </summary>
        /// <returns></returns>
        public static async Task Initialise()
        {
            //checks if the database has been created
            await _context.Database.EnsureCreatedAsync();

            //if there are no user roles, create roles
            if(!_context.UserRoles.Any())
                await RoleSeed();

            //if there are no users, create users and add them to roles
            if(!_context.Users.Any()) 
            {
                await UserSeed();
                await AddUsersToRoles();
            }

            //if there are no brochures, seed all the necessary data in order to create them
            if(!await _context.Brochures.AnyAsync())
            {
                await CountrySeed();
                await CategorySeed();
                await HotelSeed();
                await PrivateAccomodationSeed();
                await BrochureSeed();
            } 
        }

        /// <summary>
        /// Creates user roles
        /// </summary>
        /// <returns></returns>
        private static async Task RoleSeed()
        {
            //customer
            if(!await _roleManager.RoleExistsAsync("Customer")) await _roleManager.CreateAsync(new IdentityRole("Customer"));

            //admin
            if(!await _roleManager.RoleExistsAsync("Admin")) await _roleManager.CreateAsync(new IdentityRole("Admin"));

            //shop manager
            if(!await _roleManager.RoleExistsAsync("ShopManager")) await _roleManager.CreateAsync(new IdentityRole("ShopManager"));

            //assistant manager
            if(!await _roleManager.RoleExistsAsync("AssistantManager")) await _roleManager.CreateAsync(new IdentityRole("AssistantManager"));

            //travel assistant
            if(!await _roleManager.RoleExistsAsync("TravelAssistant")) await _roleManager.CreateAsync(new IdentityRole("TravelAssistant"));
            
            //invoice clerk
            if(!await _roleManager.RoleExistsAsync("InvoiceClerk")) await _roleManager.CreateAsync(new IdentityRole("InvoiceClerk"));

            //save database
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// creates users
        /// </summary>
        /// <returns></returns>
        private static async Task UserSeed()
        {
            //get the file
            var userdata = System.IO.File.ReadAllText(@"DatabaseSeeders/StaffData.json");
            //convert all data in the file
            var users = JsonConvert.DeserializeObject<List<Staff>>(userdata);

            //for every user in users
            foreach(var user in users)
            {
                //if user doesnt exist
                if(await _userManager.FindByEmailAsync(user.Email) == null)
                {
                    //create the user
                    user.EmailConfirmed = true;
                    user.TimeOfRegistration = DateTime.Now;
                    user.UserName = user.Email;
                    await _userManager.CreateAsync(user, "Password1!");
                }
            }

            //save database
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// adds created users to their roles
        /// </summary>
        /// <returns></returns>
        private static async Task AddUsersToRoles()
        {
            //admin
            var sysAdmin = await _userManager.FindByEmailAsync("sysadmin@whtravel.com");
            await _userManager.AddToRoleAsync(sysAdmin, "Admin");

            //shop manager
            var shopManager = await _userManager.FindByEmailAsync("shopmanager@whtravel.com");
            await _userManager.AddToRoleAsync(shopManager, "ShopManager");

            //assistant manager
            var assistantMan = await _userManager.FindByEmailAsync("assistantman@whtravel.com");
            await _userManager.AddToRoleAsync(assistantMan, "AssistantManager");

            //travel assistant
            var travelAssistant = await _userManager.FindByEmailAsync("travelassistant@whtravel.com");
            await _userManager.AddToRoleAsync(travelAssistant, "TravelAssistant");

            //invoice clerk
            var invoiceClerk = await _userManager.FindByEmailAsync("invoiceclerk@whtravel.com");
            await _userManager.AddToRoleAsync(invoiceClerk, "InvoiceClerk");
        }

        /// <summary>
        /// creates default list of countries
        /// </summary>
        /// <returns></returns>
        private static async Task CountrySeed()
        {
            //get file
            var file = System.IO.File.ReadAllText(@"DatabaseSeeders/CountryData.json");
            //convert data from file
            var countries = JsonConvert.DeserializeObject<List<Country>>(file);

            //for every country in file
            foreach(var country in countries)
            {
                //if country doesnt exist
                if(await _context.Countrys.Where(c => c.CountryName.Equals(country.CountryName)).FirstOrDefaultAsync() == null)
                {
                    //add country to database
                    await _context.Countrys.AddAsync(country);
                }
            }

            //save database
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// creates default list of categories
        /// </summary>
        /// <returns></returns>
        private static async Task CategorySeed()
        {
            //get file
            var file = System.IO.File.ReadAllText(@"DatabaseSeeders/CategoryData.json");
            //convert data from file
            var categories = JsonConvert.DeserializeObject<List<Category>>(file);

            //for every category in file
            foreach(var category in categories)
            {
                //if category doesnt exist
                if(await _context.Categorys.Where(c => c.CategoryName.Equals(category.CategoryName)).FirstOrDefaultAsync() == null)
                {
                    //add category to database
                    await _context.Categorys.AddAsync(category);
                }
            }
            //save database
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// creates default list of hotels
        /// </summary>
        /// <returns></returns>
        private static async Task HotelSeed()
        {
            //get file
            var file = System.IO.File.ReadAllText(@"DatabaseSeeders/HotelData.json");
            //convert data from file
            var hotels = JsonConvert.DeserializeObject<List<Hotel>>(file);

            //find the travel assistant
            var user = await _userManager.FindByEmailAsync("travelassistant@whtravel.com");

            //for every hotel in file
            foreach(var hotel in hotels)
            {
                //if hotel doesnt exist
                if(await _context.Accomodations.Where(h => h.AccomodationName.Equals(hotel.AccomodationName)).FirstOrDefaultAsync() == null)
                {
                    //assign travel assistant to accomodation
                    hotel.UserId = user.Id;

                    //add hotel to database
                    await _context.Accomodations.AddAsync(hotel);
                }
            }

            //save database
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// creates default list of accomodation
        /// </summary>
        /// <returns></returns>
        private static async Task PrivateAccomodationSeed()
        {
            //get file
            var file = System.IO.File.ReadAllText(@"DatabaseSeeders/PrivateAccomodationData.json");
            //convert data from file
            var privateAccomodations = JsonConvert.DeserializeObject<List<Private>>(file);

            //for every accomodation in file
            foreach(var privateAccomodation in privateAccomodations)
            {
                //if accomodation doesnt exist
                if(await _context.Accomodations.Where(h => h.AccomodationName.Equals(privateAccomodation.AccomodationName)).FirstOrDefaultAsync() == null)
                {
                    //add accomodation to database
                    await _context.Accomodations.AddAsync(privateAccomodation);
                }
            }

            //save database
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// creates default list of brochures
        /// </summary>
        /// <returns></returns>
        private static async Task BrochureSeed()
        {
            //get file
            var file = System.IO.File.ReadAllText(@"DatabaseSeeders/BrochureData.json");
            //convert data from file
            var brochures = JsonConvert.DeserializeObject<List<Brochure>>(file);

            //for every brochure in file
            foreach(var brochure in brochures)
            {
                brochure.ImageLink = @"\images\BrochureImages\default_image.jpg";
                //add brochure to database
                await _context.Brochures.AddAsync(brochure);
            }

            //save database
            await _context.SaveChangesAsync();
        }
    }
}