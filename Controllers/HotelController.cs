using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Walton_Happy_Travel.Data;
using Walton_Happy_Travel.Models;

namespace Walton_Happy_Travel
{
    /// <summary>
    /// Manages the pages that relate to the creating, editing and removing hotels
    /// </summary>
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hotel
        [Authorize(Roles = "Admin, ShopManager")]
        public async Task<IActionResult> Index()
        {
            //get all hotels from the database
            var applicationDbContext = await _context.Accomodations
                .Where(h => h.GetType() == typeof(Hotel))
                .Include(h => h.Country)
                .Include(h => h.StaffAssigned)
                .ToListAsync();

            //convert accomodations found to hotel objects
            List<Hotel> model = new List<Hotel>();
            foreach(var accomodation in applicationDbContext)
            {
                var hotel = (Hotel) accomodation;
                model.Add(hotel);
            }
            
            //load page
            return View(model);
        }

        // GET: Hotel/Details/5
        [Authorize(Roles = "Admin, ShopManager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Accomodations
                .Include(h => h.Country)
                .SingleOrDefaultAsync(m => m.AccomodationId == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // GET: Hotel/Create
        [Authorize(Roles = "Admin, ShopManager")]
        public IActionResult Create()
        {
            ViewData["Staff"] = new SelectList(_context.Users.Where(p => p.GetType() == typeof(Staff)), "Id", "Email");
            ViewData["CountryId"] = new SelectList(_context.Countrys, "CountryId", "CountryName");
            return View();
        }

        // POST: Hotel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ShopManager")]
        public async Task<IActionResult> Create([Bind("Rating,AccomodationId,AccomodationName,AccomodationAddress,Description,CountryId,UserId")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Staff"] = new SelectList(_context.Users.Where(p => p.GetType() == typeof(Staff)), "Id", "Email");
            ViewData["CountryId"] = new SelectList(_context.Countrys, "CountryId", "CountryName", hotel.CountryId);
            return View(hotel);
        }

        // GET: Hotel/Edit/5
        [Authorize(Roles = "Admin, ShopManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Accomodations.SingleOrDefaultAsync(m => m.AccomodationId == id);
            if (hotel == null)
            {
                return NotFound();
            }
            ViewData["Staff"] = new SelectList(_context.Users.Where(p => p.GetType() == typeof(Staff)), "Id", "Email");
            ViewData["CountryId"] = new SelectList(_context.Countrys, "CountryId", "CountryName", hotel.CountryId);
            return View(hotel);
        }

        // POST: Hotel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ShopManager")]
        public async Task<IActionResult> Edit(int id, [Bind("Rating,AccomodationId,AccomodationName,AccomodationAddress,Description,CountryId,UserId")] Hotel hotel)
        {
            if (id != hotel.AccomodationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.AccomodationId))
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
            ViewData["CountryId"] = new SelectList(_context.Countrys, "CountryId", "CountryName", hotel.CountryId);
            return View(hotel);
        }

        // GET: Hotel/Delete/5
        [Authorize(Roles = "Admin, ShopManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Accomodations
                .Include(h => h.Country)
                .Include(h => h.StaffAssigned)
                .SingleOrDefaultAsync(m => m.AccomodationId == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Hotel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ShopManager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hotel = await _context.Accomodations.SingleOrDefaultAsync(m => m.AccomodationId == id);
            _context.Accomodations.Remove(hotel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(int id)
        {
            return _context.Accomodations.Any(e => e.AccomodationId == id && e.GetType() == typeof(Private));
        }
    }
}
