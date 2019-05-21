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
    public class BrochureController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BrochureController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Brochure
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Brochures.Include(b => b.Accomodation).Include(b => b.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Brochure/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brochure = await _context.Brochures
                .Include(b => b.Accomodation)
                .Include(b => b.Category)
                .SingleOrDefaultAsync(m => m.BrochureId == id);
            if (brochure == null)
            {
                return NotFound();
            }

            return View(brochure);
        }

        // GET: Brochure/Create
        public IActionResult Create()
        {
            ViewData["Catering"] = new SelectList(new List<Catering>
            {
                Catering.ALL_INCLUSIVE,
                Catering.HALF_BOARD,
                Catering.SELF_CATERING
            });

            ViewData["AccomodationId"] = new SelectList(_context.Accomodations, "AccomodationId", "AccomodationId");
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "CategoryId");
            return View();
        }

        // POST: Brochure/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrochureId,Duration,PricePerPerson,Description,Catering,MaxPeople,CategoryId,AccomodationId")] Brochure brochure)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brochure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccomodationId"] = new SelectList(_context.Accomodations, "AccomodationId", "AccomodationId", brochure.AccomodationId);
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "CategoryId", brochure.CategoryId);
            return View(brochure);
        }

        // GET: Brochure/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brochure = await _context.Brochures.SingleOrDefaultAsync(m => m.BrochureId == id);
            if (brochure == null)
            {
                return NotFound();
            }
            ViewData["AccomodationId"] = new SelectList(_context.Accomodations, "AccomodationId", "Discriminator", brochure.AccomodationId);
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "CategoryId", brochure.CategoryId);
            return View(brochure);
        }

        // POST: Brochure/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrochureId,DepartureDate,Duration,PricePerPerson,Description,Catering,MaxPeople,MaxRooms,CategoryId,AccomodationId")] Brochure brochure)
        {
            if (id != brochure.BrochureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brochure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrochureExists(brochure.BrochureId))
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
            ViewData["AccomodationId"] = new SelectList(_context.Accomodations, "AccomodationId", "Discriminator", brochure.AccomodationId);
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "CategoryId", brochure.CategoryId);
            return View(brochure);
        }

        // GET: Brochure/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brochure = await _context.Brochures
                .Include(b => b.Accomodation)
                .Include(b => b.Category)
                .SingleOrDefaultAsync(m => m.BrochureId == id);
            if (brochure == null)
            {
                return NotFound();
            }

            return View(brochure);
        }

        // POST: Brochure/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brochure = await _context.Brochures.SingleOrDefaultAsync(m => m.BrochureId == id);
            _context.Brochures.Remove(brochure);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrochureExists(int id)
        {
            return _context.Brochures.Any(e => e.BrochureId == id);
        }

        /// <summary>
        /// loads the browse brochures page with all brochures available to book
        /// </summary>
        /// <returns>Browse page</returns>
        public async Task<IActionResult> Browse()
        {
            //populate the model and inject into page
            ViewBrochuresViewModel model = new ViewBrochuresViewModel()
            {
                //get all brochures from database
                Brochures = await _context.Brochures.Include(b=> b.Accomodation).ToListAsync(),

                //get all categories of brochures from database
                Categories = await _context.Categorys.ToListAsync()
            };
            return View(model);
        }
    }
}
