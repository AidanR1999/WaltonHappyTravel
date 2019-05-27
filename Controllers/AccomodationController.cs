using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Walton_Happy_Travel.Data;
using Walton_Happy_Travel.Models;

namespace Walton_Happy_Travel.Controllers
{
    public class AccomodationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccomodationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accomodation
        public async Task<IActionResult> Index(string type)
        {
            ViewData["Type"] = type;
            if(type.Equals("Hotel"))
            {
                //find all hotels and returns the view
                return View(await _context.Accomodations.Where(a => a.GetType() == typeof(Hotel)).ToListAsync());
            }
            else if(type.Equals("Private"))
            {
                //find all private properties and returns the view
                return View(await _context.Accomodations.Where(a => a.GetType() == typeof(Private)).ToListAsync());
            }
            return RedirectToAction(nameof(BrochureController.Browse), "Brochure");
        }

        // GET: Accomodation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accomodation = await _context.Accomodations
                .SingleOrDefaultAsync(m => m.AccomodationId == id);
            if (accomodation == null)
            {
                return NotFound();
            }

            return View(accomodation);
        }

        // GET: Accomodation/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accomodation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAccomodationViewModel accomodationViewModel, string type)
        {
            Accomodation accomodation = null;

            if (ModelState.IsValid)
            {
                switch(type)
                {
                    case "Hotel": 
                        accomodation = new Hotel
                        {
                            AccomodationName = accomodationViewModel.AccomodationName,
                            AccomodationAddress = accomodationViewModel.AccomodationAddress,
                            Description = accomodationViewModel.Description,
                            CountryId = accomodationViewModel.CountryId,
                            Rating = 5
                        };
                        break;
                
                    case "Private":
                        accomodation = new Private
                        {
                            AccomodationName = accomodationViewModel.AccomodationName,
                            AccomodationAddress = accomodationViewModel.AccomodationAddress,
                            Description = accomodationViewModel.Description,
                            CountryId = accomodationViewModel.CountryId
                        };
                        break;
                
                    default:
                        return View(accomodationViewModel);
                }
                _context.Add(accomodation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accomodationViewModel);
        }

        // GET: Accomodation/Edit/5
        public async Task<IActionResult> Edit(int? id, string type)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accomodation = await _context.Accomodations.SingleOrDefaultAsync(m => m.AccomodationId == id);
            if (accomodation == null)
            {
                return NotFound();
            }

            ViewData["Type"] = type;
            ViewBag.Staff = new SelectList(_context.Users.Where(u => u.GetType() == typeof(Staff)), "Id", "Email");
            return View(accomodation);
        }

        // POST: Accomodation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hotel accomodation, string type)
        {
            if (id != accomodation.AccomodationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accomodation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccomodationExists(accomodation.AccomodationId))
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
            return View(accomodation);
        }

        // GET: Accomodation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accomodation = await _context.Accomodations
                .SingleOrDefaultAsync(m => m.AccomodationId == id);
            if (accomodation == null)
            {
                return NotFound();
            }

            return View(accomodation);
        }

        // POST: Accomodation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accomodation = await _context.Accomodations.SingleOrDefaultAsync(m => m.AccomodationId == id);
            _context.Accomodations.Remove(accomodation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccomodationExists(int id)
        {
            return _context.Accomodations.Any(e => e.AccomodationId == id);
        }
    }
}
