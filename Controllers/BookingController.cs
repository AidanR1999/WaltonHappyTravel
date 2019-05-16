using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Walton_Happy_Travel.Data;
using Walton_Happy_Travel.Models;
using System.Security.Claims;
using Stripe;

namespace Walton_Happy_Travel.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bookings.Include(b => b.Brochure).Include(b => b.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Brochure)
                .Include(b => b.User)
                .SingleOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Booking/Create
        public IActionResult Create(int? brochureId, bool? dateAvailable, DateTime? date)
        {

            return View();
        }

        // POST: Booking/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking model)
        {
            Booking booking = null;
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrochureId"] = new SelectList(_context.Brochures, "BrochureId", "BrochureId", booking.BrochureId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", booking.UserId);
            return View(booking);
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.SingleOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["BrochureId"] = new SelectList(_context.Brochures, "BrochureId", "BrochureId", booking.BrochureId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", booking.UserId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,NoOfRooms,PaymentType,TotalPrice,AmountPaid,SpecialRequirements,UserId,BrochureId")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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
            ViewData["BrochureId"] = new SelectList(_context.Brochures, "BrochureId", "BrochureId", booking.BrochureId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", booking.UserId);
            return View(booking);
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Brochure)
                .Include(b => b.User)
                .SingleOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.SingleOrDefaultAsync(m => m.BookingId == id);
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }

        /// <summary>
        /// loads the page where users can check date availability for booking
        /// </summary>
        /// <param name="brochureId">ID of brochure selected by the user</param>
        /// <returns>CheckDate view</returns>
        public IActionResult CheckDate(int? brochureId)
        {
            //if brochureId is null, redirect to browse brochures page
            if (brochureId == null) return RedirectToAction(nameof(BrochureController.Browse));

            //populate the model and inject into the page
            CheckDateViewModel model = new CheckDateViewModel
            {
                BrochureId = (int) brochureId,
                DepartureDate = null
            };
            return View(model);
        }

        /// <summary>
        /// checks if date is available, if so: create a new booking to add to the database.
        /// </summary>
        /// <param name="model">Viewmodel for page</param>
        /// <returns>Redrect to AddPeople action</returns>
        [HttpPost]
        public async Task<IActionResult> CheckDate(CheckDateViewModel model)
        {
            //get the brochure from the database
            Brochure brochure = await _context.Brochures.FindAsync(model.BrochureId);

            //look for all bookings under that brochure and check is selected date is taken
            var results = brochure.Bookings.Where(b => b.DepartureDate.Equals(model.DepartureDate));

            //result should return 0 if date is available
            if(results.Count() == 0)
            {
                //create a new booking
                Booking booking = new Booking
                {
                    BrochureId = model.BrochureId,
                    Brochure = await _context.Brochures.FindAsync(model.BrochureId),
                    DepartureDate = (DateTime) model.DepartureDate,
                    PaymentType = PaymentType.STRIPE,
                    TotalPrice = brochure.PricePerPerson,
                    AmountPaid = 0,
                    SpecialRequirements = "",
                    Status = "IN_PROGRESS",
                    UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                //add booking to database
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                //redirect to AddPeople action
                return RedirectToAction("AddNumberOfPeople", "Person", new { bookingId = booking.BookingId });
            }

            //on fail return to previous page
            return RedirectToAction(nameof(BrochureController.Browse));
        }

        /// <summary>
        /// loads the confirmation page to finalise and make payments
        /// </summary>
        /// <param name="bookingId">id of booking</param>
        /// <returns>Confirmation page</returns>
        public async Task<IActionResult> Confirmation(int? bookingId)
        {
            //gets all data from the database related to the booking
            Booking booking = await _context.Bookings.FindAsync(bookingId);
            Brochure brochure = await _context.Brochures.FindAsync(booking.BrochureId);
            Accomodation accomodation = await _context.Accomodations.FindAsync(brochure.AccomodationId);
            Country country = await _context.Countrys.FindAsync(accomodation.CountryId);
            List<Models.Person> persons = await _context.Persons.Where(p => p.BookingId == (int) bookingId).ToListAsync();

            //updates the total price of the booking in the database
            booking.TotalPrice = brochure.PricePerPerson * persons.Count;
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            //populate the viewmodel and inject into the view
            BookingConfirmationViewModel model = new BookingConfirmationViewModel
            {
                BookingId = (int) bookingId,
                AccomodationName = accomodation.AccomodationName,
                CountryName = country.CountryName,
                DepartureDate = booking.DepartureDate,
                Duration = brochure.Duration,
                Catering = brochure.Catering,
                TotalPrice = booking.TotalPrice,
                Persons = persons
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment(int? bookingId, string stripeEmail, string stripeToken)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            var customerService = new CustomerService();
            var chargeService = new ChargeService();

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            var charge = await chargeService.CreateAsync(new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(booking.TotalPrice * 100),
                Description = "Booking Id: " + booking.BookingId,
                Currency = "gbp",
                CustomerId = customer.Id
            });

            booking.Status = "Completed";
            booking.AmountPaid = booking.TotalPrice;
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Invoice), new { bookingId = booking.BookingId });
        }

        public async Task<IActionResult> Invoice(int? bookingId)
        {
            Booking booking = await _context.Bookings.FindAsync(bookingId);

            var user = await _context.Users.FindAsync(booking.UserId);
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(currentUserId == null) 
                return RedirectToAction(nameof(BrochureController.Browse), "Brochure");
            if((!currentUserId.Equals(user.Id) && !User.IsInRole("Staff"))) 
                return RedirectToAction(nameof(BrochureController.Browse), "Brochure");

            Brochure brochure = await _context.Brochures.FindAsync(booking.BrochureId);
            Accomodation accomodation = await _context.Accomodations.FindAsync(brochure.AccomodationId);
            Country country = await _context.Countrys.FindAsync(accomodation.CountryId);
            List<Models.Person> persons = await _context.Persons.Where(p => p.BookingId == (int) bookingId).ToListAsync();

            BookingInvoiceViewModel model = new BookingInvoiceViewModel
            {
                BookingId = (int) bookingId,
                BookingStatus = booking.Status,
                AccomodationName = accomodation.AccomodationName,
                CountryName = country.CountryName,
                DepartureDate = booking.DepartureDate,
                Duration = brochure.Duration,
                Catering = brochure.Catering,
                TotalPrice = booking.TotalPrice,
                AmountPaid = booking.AmountPaid,
                Persons = persons
            };
            
            if(booking.Status.Equals("IN PROGRESS")) 
                return RedirectToAction(nameof(BookingController.Confirmation), new { bookingId = bookingId });

            return View(model);
        }
    }

    
}
