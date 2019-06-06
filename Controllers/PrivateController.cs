using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Walton_Happy_Travel.Data;
using Walton_Happy_Travel.Models;
using Microsoft.AspNetCore.Authorization;

namespace Walton_Happy_Travel
{
    /// <summary>
    /// Manages all pages related to creating, editing and removing private properties
    /// </summary>
    public class PrivateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrivateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Private
        [Authorize(Roles = "Admin, ShopManager")]
        public async Task<IActionResult> Index()
        {
            //get all private properties from the database
            var applicationDbContext = await _context.Accomodations
                .Where(a => a.GetType() == typeof(Private))
                .Include(p => p.Country)
                .Include(p => p.StaffAssigned)
                .ToListAsync();

            //convert every accomodation found into private objects
            List<Private> model = new List<Private>();
            foreach(var accomodation in applicationDbContext)
            {
                var @private = (Private) accomodation;
                model.Add(@private);
            }
            
            //load page
            return View(model);
        }

        // GET: Private/Details/5
        [Authorize(Roles = "Admin, ShopManager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @private = await _context.Accomodations
                .Include(p => p.Country)
                .Include(p => p.StaffAssigned)
                .SingleOrDefaultAsync(m => m.AccomodationId == id);
            if (@private == null)
            {
                return NotFound();
            }

            return View(@private);
        }

        // GET: Private/Create
        [Authorize(Roles = "Admin, ShopManager")]
        public IActionResult Create()
        {
            ViewData["Staff"] = new SelectList(_context.Users.Where(p => p.GetType() == typeof(Staff)), "Id", "Email");
            ViewData["CountryId"] = new SelectList(_context.Countrys, "CountryId", "CountryName");
            return View();
        }

        // POST: Private/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ShopManager")]
        public async Task<IActionResult> Create([Bind("AccomodationId,AccomodationName,AccomodationAddress,Description,CountryId,UserId")] Private @private)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@private);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Staff"] = new SelectList(_context.Users.Where(p => p.GetType() == typeof(Staff)), "Id", "Email");
            ViewData["CountryId"] = new SelectList(_context.Countrys, "CountryId", "CountryName", @private.CountryId);
            return View(@private);
        }

        // GET: Private/Edit/5
        [Authorize(Roles = "Admin, ShopManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @private = await _context.Accomodations.SingleOrDefaultAsync(m => m.AccomodationId == id);
            if (@private == null)
            {
                return NotFound();
            }

            ViewData["Staff"] = new SelectList(_context.Users.Where(p => p.GetType() == typeof(Staff)), "Id", "Email");
            ViewData["CountryId"] = new SelectList(_context.Countrys, "CountryId", "CountryName", @private.CountryId);
            return View(@private);
        }

        // POST: Private/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ShopManager")]
        public async Task<IActionResult> Edit(int id, [Bind("AccomodationId,AccomodationName,AccomodationAddress,Description,CountryId,UserId")] Private @private)
        {
            if (id != @private.AccomodationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@private);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrivateExists(@private.AccomodationId))
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

            ViewData["Staff"] = new SelectList(_context.Users.Where(p => p.GetType() == typeof(Staff)), "Id", "Email");
            ViewData["CountryId"] = new SelectList(_context.Countrys, "CountryId", "CountryName", @private.CountryId);
            return View(@private);
        }

        // GET: Private/Delete/5
        [Authorize(Roles = "Admin, ShopManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @private = await _context.Accomodations
                .Include(p => p.Country)
                .Include(p => p.StaffAssigned)
                .SingleOrDefaultAsync(m => m.AccomodationId == id);
            if (@private == null)
            {
                return NotFound();
            }

            return View(@private);
        }

        // POST: Private/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ShopManager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @private = await _context.Accomodations.SingleOrDefaultAsync(m => m.AccomodationId == id);
            _context.Accomodations.Remove(@private);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrivateExists(int id)
        {
            return _context.Accomodations.Any(e => e.AccomodationId == id && e.GetType() == typeof(Private));
        }
    }
}
