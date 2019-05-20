using System;
using System.Linq;
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

namespace Walton_Happy_Travel.DatabaseSeeders
{
    public class Seeder 
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Seeder(UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager =  userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public void Seed()
        {
            if(_context.UserRoles.Any())
                RoleSeed();
            if(!_context.Users.Any()) 
            {
                UserSeed();
                AddUsersToRoles();
            } 
        }

        private async Task RoleSeed()
        {
            await _roleManager.CreateAsync(new IdentityRole("Customer"));
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _roleManager.CreateAsync(new IdentityRole("ShopManager"));
            await _roleManager.CreateAsync(new IdentityRole("AssistantManager"));
            await _roleManager.CreateAsync(new IdentityRole("TravelAssistant"));
            await _roleManager.CreateAsync(new IdentityRole("InvoiceClerk"));
        }

        private async Task UserSeed()
        {
            var userdata = System.IO.File.ReadAllText(@"DatabaseSeeders/UserSeed.json");
            var users = JsonConvert.DeserializeObject<List<ApplicationUser>>(userdata);

            foreach(var user in users)
            {
                user.EmailConfirmed = true;
                user.TimeOfRegistration = DateTime.Now;
                user.UserName = user.Email;
                await _userManager.CreateAsync(user, "Password1!");
            }
        }

        private async Task AddUsersToRoles()
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
    }
}