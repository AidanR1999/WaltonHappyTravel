using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Walton_Happy_Travel.Data;
using Walton_Happy_Travel.Models;

namespace Walton_Happy_Travel
{
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StaffController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Staff
        public async Task<IActionResult> Index()
        {
            //get staff from database
            var users = await _context.Users
                .Where(s => s.GetType() == typeof(Staff))
                .ToListAsync();

            //convert all users found into staff objects
            var model = new List<Staff>();
            foreach(var staff in users)
            {
                model.Add((Staff) staff);
            }

            //load page
            return View(model);
        }

        // GET: Staff/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Staff/Create
        public IActionResult Create()
        {
            ViewData["Roles"] = new SelectList(_context.Roles.Where(r => !r.Name.Equals("Customer")), "Name", "Name");
            return View(new CreateStaffViewModel());
        }

        // POST: Staff/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var staff = new Staff()
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

                await _userManager.CreateAsync(staff, model.Password);
                await _userManager.AddToRoleAsync(staff, model.Role);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Staff/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(staff);
            string role = "";

            foreach(var foundRoles in roles)
            {
                role = foundRoles;
            }

            var model = new EditStaffViewModel
            {
                Forename = staff.Forename,
                MiddleNames = staff.MiddleNames,
                Surname = staff.Surname,
                Email = staff.Email,
                DateOfBirth = staff.DateOfBirth,
                Role = role
            };

            ViewData["Roles"] = new SelectList(_context.Roles.Where(r => !r.Name.Equals("Customer")), "Name", "Name");
            return View(model);
        }

        // POST: Staff/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditStaffViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var staff = await _userManager.FindByIdAsync(model.Id);

                    staff.Forename = model.Forename;
                    staff.MiddleNames = model.MiddleNames;
                    staff.Surname = model.Surname;
                    staff.Email = model.Email;
                    staff.DateOfBirth = model.DateOfBirth;
                    
                    _context.Update(staff);

                    var roles = await _userManager.GetRolesAsync(staff);
                    await _userManager.RemoveFromRolesAsync(staff, roles);
                    await _userManager.AddToRoleAsync(staff, model.Role);
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(model.Id))
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

        // GET: Staff/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var staff = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            _context.Users.Remove(staff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        /// <summary>
        /// loads the page which allows for sales reports to be generated
        /// </summary>
        /// <returns>Reports page</returns>
        public ActionResult Reports()
        {
            //create the model
            var model = new StaffReportsViewModel()
            {
                Report = ""
            };

            //get the types of reports into the view
            ViewData["Reports"] = new SelectList(new List<string>
            {
                "",
                "Daily Booking Report",
                "Monthly Booking Report"
            });

            //load page
            return View(model);
        }

        /// <summary>
        /// gets the user's choice of report and generates the report
        /// </summary>
        /// <param name="model">View model for page</param>
        /// <returns>Generates report and reloads reports page</returns>
        [HttpPost]
        public async Task<IActionResult> Reports(StaffReportsViewModel model)
        {
            //get the types of reports into the view
            ViewData["Reports"] = new SelectList(new List<string>
            {
                "",
                "Daily Booking Report",
                "Monthly Booking Report"
            });

            //if no report has been selected, reload the page
            if(model.Report == null) return View(model);


            return View(model);
        }
    }
}
