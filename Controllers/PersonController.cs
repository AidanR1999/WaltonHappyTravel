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
                return RedirectToAction(nameof(BookingController.Confirmation), "Booking", new { bookingId = person.BookingId });
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
        /// <returns>AddNumberOfPeople page</returns>
        public IActionResult AddNumberOfPeople(int? bookingId)
        {
            //if brochureId is null, redirect to browse brochures page
            if(bookingId == null) return RedirectToAction(nameof(BrochureController.Browse));

            //populate the model and inject into the page
            AddNumberOfPeopleViewModel model = new AddNumberOfPeopleViewModel
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
        /// <returns>Redirects to AddPeople page</returns>
        [HttpPost]
        public async Task<IActionResult> AddNumberOfPeople(AddNumberOfPeopleViewModel model)
        {
            //get booking from database
            var booking = await _context.Bookings.FindAsync(model.BookingId);

            //if booking is not empty
            if(booking != null)
            {
                var brochure = await _context.Brochures.FindAsync(booking.BrochureId);

                //updating the total price of the booking
                booking.TotalPrice = brochure.PricePerPerson * model.PeopleAdded;
                _context.Bookings.Update(booking);   
                _context.SaveChanges();

                //redirect to AddPeople action
                return RedirectToAction(nameof(PersonController.AddPeople), new { bookingId = model.BookingId, numberOfPeople = model.PeopleAdded });
            }

            //on fail, return the page
            return RedirectToAction(nameof(AddNumberOfPeople), new { bookingId = model.BookingId });
        }

        /// <summary>
        /// loads page for users to be able to add the details of people in the booking
        /// </summary>
        /// <param name="bookingId">bookingId of the booking</param>
        /// <returns>AddPeople Page</returns>
        public ActionResult AddPeople(int? bookingId, int? numberOfPeople)
        {
            //if bookingId is null, redirect to browse brochures page
            if(bookingId == null) return RedirectToAction(nameof(BrochureController.Browse));

            List<Person> peopleToAdd = new List<Person>();

            for(int i = 0; i < numberOfPeople; i++)
            {
                peopleToAdd.Add(new Person
                {
                    Forename = "",
                    MiddleNames = "",
                    Surname = "",
                    DateOfBirth = DateTime.Now
                });
            }

            //populate the model and inject into the page
            AddPeopleToBookingViewModel model = new AddPeopleToBookingViewModel
            {
                BookingId = (int) bookingId,
                NumberOfPeople = (int) numberOfPeople,
                PeopleToAdd = peopleToAdd
            };
            return View(model);
        }

        /// <summary>
        /// updates the booking in the database with the details of the people
        /// </summary>
        /// <param name="model">view model from page</param>
        /// <returns>Redirects to Confirmation page</returns>
        [HttpPost]
        public async Task<IActionResult> AddPeople(AddPeopleToBookingViewModel model)
        {
            //gets the booking from the database
            var booking = await _context.Bookings.FindAsync(model.BookingId);

            //convert IList to List as it can be casted as IEnumerable
            booking.Persons = model.PeopleToAdd.ToList();

            //update the database
            foreach(var person in booking.Persons)
            {
                _context.Persons.Add(person);
            }
            _context.SaveChanges();

            //redirect to Confirmation page
            return RedirectToAction(nameof(BookingController.Confirmation), "Booking", new { bookingId = model.BookingId });
        }
    }
}
