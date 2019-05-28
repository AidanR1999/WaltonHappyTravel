using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Walton_Happy_Travel.Data;
using Walton_Happy_Travel.Models;
using Walton_Happy_Travel.Utilities;
using Microsoft.AspNetCore.Hosting;

namespace Walton_Happy_Travel.Controllers
{
    public class BrochureController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hosting;

        public BrochureController(ApplicationDbContext context, IHostingEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }

        // GET: Brochure
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Brochures
                .Include(b => b.Accomodation)
                .Include(b => b.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Brochure/Details/5
        /// <summary>
        /// loads the details of the selected brochure
        /// </summary>
        /// <param name="id">id of brochure</param>
        /// <returns>Details page</returns>
        public async Task<IActionResult> Details(int? id)
        {
            //if id is null, return error message
            if (id == null)
            {
                return NotFound();
            }

            //get the brochure from the database
            var brochure = await _context.Brochures
                .Include(b => b.Accomodation)
                .Include(b => b.Category)
                .Include(b => b.Accomodation.Country)
                .SingleOrDefaultAsync(m => m.BrochureId == id);

            //if brochure doesnt exist, return error
            if (brochure == null)
            {
                return NotFound();
            }

            //load Details page, inject model
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
            

            ViewData["AccomodationId"] = new SelectList(_context.Accomodations, "AccomodationId", "AccomodationName");
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "CategoryName");
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
                //add brochure to database
                _context.Add(brochure);
                await _context.SaveChangesAsync();

                //get image from form
                string webRootPath = _hosting.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                //if image is uploaded
                if(files.Count != 0)
                {
                    //getting the extension and path of the image
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension = Path.GetExtension(files[0].FileName);

                    //using dependency injection to get a filestream using the path of images
                    using (var filestream = new FileStream(Path.Combine(uploads, brochure.BrochureId + extension), FileMode.Create))
                    {
                        //store image in images folder
                        await files[0].CopyToAsync(filestream);
                    }

                    //stores image location in object
                    brochure.ImageLink = @"\" + SD.ImageFolder + @"\" + brochure.BrochureId + extension;
                }
                //if image is not uploaded
                else
                {
                    //load brochure with set default image
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultBrochureImage);
                    System.IO.File.Copy(uploads,webRootPath + @"\" + SD.ImageFolder + @"\" + brochure.BrochureId + ".jpg");
                    brochure.ImageLink = @"\" + SD.ImageFolder + @"\" + brochure.BrochureId + ".jpg";
                }

                //update database
                await _context.SaveChangesAsync();

                //redirect to index page
                return RedirectToAction(nameof(Index));
            }

            //if fail, return back to view
            ViewData["Catering"] = new SelectList(new List<Catering>
            {
                Catering.ALL_INCLUSIVE,
                Catering.HALF_BOARD,
                Catering.SELF_CATERING
            });
            ViewData["AccomodationId"] = new SelectList(_context.Accomodations, "AccomodationId", "AccomodationName", brochure.AccomodationId);
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "CategoryName", brochure.CategoryId);
            return View(brochure);
        }

        // GET: Brochure/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brochure = await _context.Brochures
                .Include(b => b.Accomodation)
                .SingleOrDefaultAsync(m => m.BrochureId == id);

            if (brochure == null)
            {
                return NotFound();
            }

            ViewData["Catering"] = new SelectList(new List<Catering>
            {
                Catering.ALL_INCLUSIVE,
                Catering.HALF_BOARD,
                Catering.SELF_CATERING
            });
            ViewData["AccomodationId"] = new SelectList(_context.Accomodations, "AccomodationId", "AccomodationName");
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "CategoryName");

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
                    //get image from form
                    string webRootPath = _hosting.WebRootPath;
                    var files = HttpContext.Request.Form.Files;

                    //if image is uploaded
                    if(files.Count != 0)
                    {
                        //getting the extension and path of the image
                        var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                        var extension = Path.GetExtension(files[0].FileName);

                        //using dependency injection to get a filestream using the path of images
                        using (var filestream = new FileStream(Path.Combine(uploads, brochure.BrochureId + extension), FileMode.Create))
                        {
                            //store image in images folder
                            await files[0].CopyToAsync(filestream);
                        }

                        //stores image location in object
                        brochure.ImageLink = @"\" + SD.ImageFolder + @"\" + brochure.BrochureId + extension;
                    }

                    //update database
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
        public async Task<IActionResult> Browse(string filter)
        {
            //populate the model and inject into page
            ViewBrochuresViewModel model = new ViewBrochuresViewModel()
            {
                //get all brochures from database
                Brochures = await _context.Brochures.Include(b=> b.Accomodation)
                    .Include(b => b.Accomodation.Country)
                    .ToListAsync(),

                //get all categories of brochures from database
                Categories = await _context.Categorys.ToListAsync()
            };
            return View(model);
        }
    }
}
