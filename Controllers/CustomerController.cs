using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Walton_Happy_Travel.Controllers;
using Walton_Happy_Travel.Data;
using Walton_Happy_Travel.Models;

namespace Walton_Happy_Travel
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            //get all customers in database
            var users = await _context.Users
                .Where(s => s.GetType() == typeof(Customer))
                .ToListAsync();

            //convert users found into customer objects
            var model = new List<Customer>();
            foreach(var customer in users)
            {
                model.Add((Customer) customer);
            }

            //load page
            return View(model);
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer()
                {
                    Forename = model.Forename,
                    MiddleNames = model.MiddleNames,
                    Surname = model.Surname,
                    Email = model.Email,
                    UserName = model.Email,
                    DateOfBirth = model.DateOfBirth,
                    TimeOfRegistration = DateTime.Now,
                    EmailConfirmed = true
                };
                
                await _userManager.CreateAsync(customer, model.Password);
                await _userManager.AddToRoleAsync(customer, "Customer");
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            var model = new EditCustomerViewModel
            {
                Forename = customer.Forename,
                MiddleNames = customer.MiddleNames,
                Surname = customer.Surname,
                Email = customer.Email,
                DateOfBirth = customer.DateOfBirth
            };
            return View(model);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditCustomerViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var customer = await _userManager.FindByIdAsync(model.Id);

                    customer.Forename = model.Forename;
                    customer.MiddleNames = model.MiddleNames;
                    customer.Surname = model.Surname;
                    customer.Email = model.Email;
                    customer.DateOfBirth = model.DateOfBirth;

                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var customer = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            _context.Users.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        /// <summary>
        /// displays all bookings the user has made
        /// </summary>
        /// <returns></returns>
        public ActionResult UserBookings()
        {
            //if user is not logged in, redirect to home
            if(!User.Identity.IsAuthenticated) return RedirectToAction(nameof(HomeController.Index), "Home");

            //get user id
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //populate and inject view model
            UserBookingsViewModel model = new UserBookingsViewModel
            {
                Bookings = _context.Bookings.Where(b => b.UserId.Equals(currentUserId))
                    .Include(b => b.Brochure)
                    .Include(b => b.Brochure.Accomodation)
                    .Include(b => b.Persons)
            };

            //load page
            return View(model);
        }
    }
}
