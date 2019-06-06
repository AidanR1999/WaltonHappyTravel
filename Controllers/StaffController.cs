using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Walton_Happy_Travel.Data;
using Walton_Happy_Travel.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Walton_Happy_Travel
{
    
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IHostingEnvironment _hostingEnvironment;

        public StaffController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
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
            var results = await _userManager.FindByEmailAsync(model.Email);
            if (ModelState.IsValid && results == null)
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
            ViewData["Roles"] = new SelectList(_context.Roles.Where(r => !r.Name.Equals("Customer")), "Name", "Name");
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
            //if no report is selected
            if(!ModelState.IsValid)
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

                //reload page
                return View(model);
            }

            //check what report is to be generated
            switch(model.Report)
            {
                case "Daily Booking Report":
                    await generateDailySales();
                    break;
                case "Monthly Booking Report":
                    await generateMonthlySales();
                    break;
            }
            
            //reload page
            return View(model);
        }

        /// <summary>
        /// creates and exports the daily booking reports
        /// </summary>
        /// <returns>Excel file</returns>
        private async Task generateDailySales()
        {
            //get all bookings made today from the db
            var bookings = await _context.Bookings.Where(b => b.Status.Equals("Completed")).Where(b => b.DateCompleted.Date.Equals(DateTime.Now.Date)).ToListAsync();

            //get number of rows required
            int numRows = bookings.Count();

            //if there are bookings made today
            if (numRows > 0)
            {
                //create excel file
                ExcelPackage excel = new ExcelPackage();

                //create the worksheet
                var workSheet = excel.Workbook.Worksheets.Add("Bookings");

                //load the data into the worksheet and format it
                workSheet.Cells[3, 1].LoadFromCollection(bookings, true);
                workSheet.Column(1).Style.Numberformat.Format = "yyyy-mm-dd HH:MM";

                //style cells
                workSheet.Cells[4, 1, numRows + 3, 2].Style.Font.Bold = true;

                //create headings for the data
                using (ExcelRange headings = workSheet.Cells[3, 1, 3, 7])
                {
                    headings.Style.Font.Bold = true;
                    var fill = headings.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.AliceBlue);
                }
                
                //auto resizes collumns
                workSheet.Cells.AutoFitColumns();

                //Adds title to the top of the sheet
                workSheet.Cells[1, 1].Value = "Daily Bookings Report";
                using (ExcelRange Rng = workSheet.Cells[1, 1, 1, 6])
                {
                    Rng.Merge = true; // Merge columns start and end range
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Font.Size = 18;
                    Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                //calculates and displays the creation of the report
                DateTime utcDate = DateTime.UtcNow;
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                var localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, timeZone);
                using (ExcelRange Rng = workSheet.Cells[2, 6])
                {

                    Rng.Value = "Created: " + localDate.ToShortTimeString() + " on " + localDate.ToShortDateString();
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Font.Size = 12;
                    Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                }

                //export the excel file and send to the user
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Headers["content-disposition"] = "attachment; filename=DailyBookingsReport.xlsx";
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.Body);
                }
            }
        }

        /// <summary>
        /// creates and exports the monthly booking reports
        /// </summary>
        /// <returns>Excel file</returns>
        private async Task generateMonthlySales()
        {
            //get all the bookings made in the last month from the db
            var bookings = await _context.Bookings.Where(b => b.Status.Equals("Completed")).Where(b => b.DateCompleted.Month == DateTime.Now.Month).ToListAsync();

            //get number of rows required
            int numRows = bookings.Count();

            //if there are bookings made today
            if (numRows > 0)
            {
                //create excel file
                ExcelPackage excel = new ExcelPackage();

                //create the worksheet
                var workSheet = excel.Workbook.Worksheets.Add("Bookings");

                //load the data into the worksheet and format it
                workSheet.Cells[3, 1].LoadFromCollection(bookings, true);
                workSheet.Column(1).Style.Numberformat.Format = "yyyy-mm-dd HH:MM";

                //style cells
                workSheet.Cells[4, 1, numRows + 3, 2].Style.Font.Bold = true;

                //create headings for the data
                using (ExcelRange headings = workSheet.Cells[3, 1, 3, 7])
                {
                    headings.Style.Font.Bold = true;
                    var fill = headings.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.AliceBlue);
                }
                
                //auto resizes collumns
                workSheet.Cells.AutoFitColumns();

                //Adds title to the top of the sheet
                workSheet.Cells[1, 1].Value = "Monthly Bookings Report";
                using (ExcelRange Rng = workSheet.Cells[1, 1, 1, 6])
                {
                    Rng.Merge = true; // Merge columns start and end range
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Font.Size = 18;
                    Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                //calculates and displays the creation of the report
                DateTime utcDate = DateTime.UtcNow;
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                var localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, timeZone);
                using (ExcelRange Rng = workSheet.Cells[2, 6])
                {

                    Rng.Value = "Created: " + localDate.ToShortTimeString() + " on " + localDate.ToShortDateString();
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Font.Size = 12;
                    Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                }

                //export the excel file and send to the user
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Headers["content-disposition"] = "attachment; filename=MonthlyBookingsReport.xlsx";
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.Body);
                }
            }
        }
    }
}
