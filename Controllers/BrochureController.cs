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
    /// <summary>
    /// Manages all pages related to making and showing brochures
    /// </summary>
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
                if(!files.FirstOrDefault().FileName.Equals(""))
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
        public async Task<IActionResult> Edit(int id, Brochure brochure)
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
                    if(!files.FirstOrDefault().FileName.Equals(""))
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
                ErrorMessage = ""
            };

            //populate catering data for filtering
            ViewData["Catering"] = new SelectList(new List<string>
            {
                "",
                Catering.ALL_INCLUSIVE.ToString(),
                Catering.HALF_BOARD.ToString(),
                Catering.SELF_CATERING.ToString()
            });

            //populate accomodation data for filtering
            ViewData["Accomodation"] = new SelectList(new List<string>
            {
                "",
                "Hotel",
                "Private"
            });

            //populate categories data for filtering from the database
            var allCategories = await _context.Categorys.ToListAsync();
            var categories = new List<string>{""};
            foreach(var category in allCategories)
            {
                categories.Add(category.CategoryName);
            }
            ViewData["Category"] = new SelectList(categories);
            
            //populate countries data for filtering from the database
            var allCountries = await _context.Countrys.ToListAsync();
            var countries = new List<string>{""};
            foreach(var country in allCountries)
            {
                countries.Add(country.CountryName);
            }
            ViewData["Country"] = new SelectList(countries);

            //load page
            return View(model);
        }

        /// <summary>
        /// searc function for the browse brocures page
        /// </summary>
        /// <param name="model">ViewModel that contains the search data</param>
        /// <returns>Browse page</returns>
        [HttpPost]
        public async Task<IActionResult> Browse(ViewBrochuresViewModel model)
        {
            //if there is search criteria, do the search
            if(model.Accomodation != null || model.Category != null || model.Catering != null || model.Country != null)
            {
                //gets all brochures with the matching category
                var brochuresWithCategory = _context.Brochures
                    .Where(b => b.Category.CategoryName.Equals(model.Category))
                    .Include(b => b.Accomodation.Country)
                    .ToAsyncEnumerable()
                    .ToEnumerable();

                //gets all brochures with the matching country
                var brochuresWithCatering = _context.Brochures
                    .Where(b => b.Catering.ToString().Equals(model.Catering))
                    .Include(b => b.Accomodation.Country)
                    .ToAsyncEnumerable()
                    .ToEnumerable();
                
                //gets all brochures with the matching country
                var brochuresWithCountry = _context.Brochures
                    .Where(b => b.Accomodation.Country.CountryName.Equals(model.Country))
                    .Include(b => b.Accomodation.Country)
                    .ToAsyncEnumerable()
                    .ToEnumerable();

                //instantiate empty list
                IEnumerable<Brochure> brochuresWithAccomodation;

                //if no value for accomodation is selected, create a new list
                if(model.Accomodation == null)
                    brochuresWithAccomodation = new List<Brochure>();

                //if hotel is selected, get all hotels from the database
                else if(model.Accomodation.Equals("Hotel"))
                    brochuresWithAccomodation =  _context.Brochures
                        .Where(b => b.Accomodation.GetType() == typeof(Hotel))
                        .Include(b => b.Accomodation.Country)
                        .ToAsyncEnumerable()
                        .ToEnumerable();

                //if private property is selected, get all private accomodations from the database
                else if(model.Accomodation.Equals("Private"))
                    brochuresWithAccomodation = _context.Brochures
                        .Where(b => b.Accomodation.GetType() == typeof(Private))
                        .Include(b => b.Accomodation.Country)
                        .ToAsyncEnumerable()
                        .ToEnumerable();

                //if reached this far, something has failed
                else
                    brochuresWithAccomodation = new List<Brochure>();

                //instantiate a list to contain all other lists
                List<IEnumerable<Brochure>> allLists = new List<IEnumerable<Brochure>>();

                //if category brochures have data, add list to allLists
                if(brochuresWithCategory.Count() > 0)
                    allLists.Add(brochuresWithCategory);

                //if catering brochures have data, add list to allLists
                if(brochuresWithCatering.Count() > 0)
                    allLists.Add(brochuresWithCatering);

                //if country brochures have data, add list to allLists
                if(brochuresWithCountry.Count() > 0)
                    allLists.Add(brochuresWithCountry);

                //if accomodation brochures have data, add list to allLists
                if(brochuresWithAccomodation.Count() > 0)
                    allLists.Add(brochuresWithAccomodation);

                //if allLists contains a single list
                if(allLists.Count() > 0)
                {
                    //iterate through each list, find all brochures that match and store in seperate list
                    var brochures = allLists
                        .Skip(1)
                        .Aggregate(
                            new HashSet<Brochure>(allLists.First()),
                            (h, e) => { h.IntersectWith(e); return h; }
                        );

                    //add filtered brochures to model
                    model.Brochures = brochures.ToList();

                    model.ErrorMessage = "";
                }
                //no matches have been found, display all brochures
                else
                {
                    model.Brochures = await _context.Brochures.Include(b=> b.Accomodation)
                        .Include(b => b.Accomodation.Country)
                        .ToListAsync();

                        model.ErrorMessage = "No results match search criteria";
                }
            }
            else
            {
                model.Brochures = await _context.Brochures.Include(b=> b.Accomodation)
                        .Include(b => b.Accomodation.Country)
                        .ToListAsync();
                model.ErrorMessage = "";
            }

            //populate catering data for filtering
            ViewData["Catering"] = new SelectList(new List<string>
            {
                "",
                Catering.ALL_INCLUSIVE.ToString(),
                Catering.HALF_BOARD.ToString(),
                Catering.SELF_CATERING.ToString()
            });

            //populate accomodation data for filtering
            ViewData["Accomodation"] = new SelectList(new List<string>
            {
                "",
                "Hotel",
                "Private"
            });

            //populate categories data for filtering from the database
            var allCategories = await _context.Categorys.ToListAsync();
            var categories = new List<string>{""};
            foreach(var category in allCategories)
            {
                categories.Add(category.CategoryName);
            }
            ViewData["Category"] = new SelectList(categories);
            
            //populate countries data for filtering from the database
            var allCountries = await _context.Countrys.ToListAsync();
            var countries = new List<string>{""};
            foreach(var country in allCountries)
            {
                countries.Add(country.CountryName);
            }
            ViewData["Country"] = new SelectList(countries);

            //load page
            return View(model);
        }
    }
}
