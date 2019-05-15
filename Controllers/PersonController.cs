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
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Person
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Persons.Include(p => p.Booking);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Person/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .Include(p => p.Booking)
                .SingleOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: Person/Create
        public IActionResult Create()
        {
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId");
            return View();
        }

        // POST: Person/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,Forename,MiddleNames,Surname,DateOfBirth,BookingId")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId", person.BookingId);
            return View(person);
        }

        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons.SingleOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId", person.BookingId);
            return View(person);
        }

        // POST: Person/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonId,Forename,MiddleNames,Surname,DateOfBirth,BookingId")] Person person)
        {
            if (id != person.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonId))
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
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId", person.BookingId);
            return View(person);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .Include(p => p.Booking)
                .SingleOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Persons.SingleOrDefaultAsync(m => m.PersonId == id);
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.PersonId == id);
        }
        
        /// <summary>
        /// loads the add people page to allow users to select how many people are on this booking
        /// </summary>
        /// <param name="bookingId">bookingId for booking</param>
        /// <returns>AddPeople page</returns>
        public IActionResult AddPeople(int? bookingId)
        {
            //if brochureId is null, redirect to browse brochures page
            if(bookingId == null) return RedirectToAction(nameof(BrochureController.Browse));

            //populate the model and inject into the page
            AddPeopleViewModel model = new AddPeopleViewModel
            {
                BookingId = (int) bookingId,
                PeopleAdded = 1
            };
            return View(model);
        }

        /// <summary>
        /// gets the people the user has selected and updates the booking to accomodate
        /// </summary>
        /// <param name="model">Viewmodel from page</param>
        /// <returns>Redirects to AddPerson page</returns>
        [HttpPost]
        public async Task<IActionResult> AddPeople(AddPeopleViewModel model)
        {
            //get booking from database
            var booking = await _context.Bookings.FindAsync(model.BookingId);

            //if booking is not empty
            if(booking != null)
            {
                //create a new list of persons
                List<Person> persons = new List<Person>();

                //for every person the user wants added, add a blank person object   
                int i = 0;
                while (i < model.PeopleAdded)
                {
                    persons.Add(new Person
                    {
                        Forename = "",
                        MiddleNames = "",
                        Surname = "",
                        DateOfBirth = DateTime.Now,
                        BookingId = booking.BookingId
                    });

                    ++i;
                }

                //add persons to booking and update the database
                booking.Persons = persons;
                _context.Bookings.Update(booking);   
                _context.SaveChanges();

                //redirect to AddPerson action
                return RedirectToAction(nameof(PersonController.AddPerson), new { bookingId = model.BookingId });
            }

            //on fail, return the page
            return RedirectToAction(nameof(AddPeople), new { bookingId = model.BookingId });
        }

        public async Task<IActionResult> AddPerson(int? bookingId)
        {
            if(bookingId == null) return RedirectToAction(nameof(BrochureController.Browse));

            AddPersonToBookingViewModel model = new AddPersonToBookingViewModel
            {
                BookingId = (int) bookingId,
                PeopleToAdd = _context.Persons.Where(b => b.BookingId == bookingId).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPerson(AddPersonToBookingViewModel model)
        {

            return View(model);
        }
    }
}
