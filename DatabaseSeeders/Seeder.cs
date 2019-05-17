using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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

        public Seeder(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager =  userManager;
            _context = context;
        }

        public void Seed()
        {
            if(!_context.Users.Any()) 
                UserSeed();
        }

        public async Task UserSeed()
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
    }
}