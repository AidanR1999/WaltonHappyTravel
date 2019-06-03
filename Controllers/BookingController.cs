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
using jsreport.AspNetCore;
using jsreport.Types;
using Walton_Happy_Travel.Services;
using System.Text.Encodings.Web;

namespace Walton_Happy_Travel.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;


        public BookingController(ApplicationDbContext context,
                                IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            //get the current logged in user
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = new List<Booking>();

            //get every completed booking if user is admin
            if(User.IsInRole("Admin"))
            {
                model = await _context.Bookings
                .Include(b => b.Brochure)
                .Include(b => b.User)
                .Include(b => b.Brochure.Accomodation)
                .Where(b => b.Status == "Completed").ToListAsync();
            }
            //only get bookings linked to staff member logged in
            else
            {
                model = await _context.Bookings
                .Include(b => b.Brochure)
                .Include(b => b.User)
                .Include(b => b.Brochure.Accomodation)
                .Where(b => b.Brochure.Accomodation.UserId == userId)
                .Where(b => b.Status == "Completed").ToListAsync();
            }
            
            //return view
            return View(model);
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
            //if user is not logged in, send to login screen
            if(!User.Identity.IsAuthenticated) return RedirectToAction(nameof(AccountController.Login), "Account");

            //if brochureId is null, redirect to browse brochures page
            if (brochureId == null) return RedirectToAction(nameof(BrochureController.Browse), "Brochure");

            //populate the model and inject into the page
            CheckDateViewModel model = new CheckDateViewModel
            {
                BrochureId = (int) brochureId,
                DepartureDate = DateTime.Now.Date,
                ErrorMessage = ""
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
            //if date is in the past, reload page
            if(model.DepartureDate < DateTime.Now || model.DepartureDate == null)
            {
                return View(model);
            }

            //get all the bookings from the database where it shares the same brochure
            var bookings = _context.Bookings.Where(b => b.BrochureId == model.BrochureId && b.Status.Equals("Completed")).Include(b => b.Brochure);
            List<DateTime> unavailableDates = new List<DateTime>();

            //store the dates for every booking
            foreach(var oldBooking in bookings)
            {
                //store departure date
                unavailableDates.Add(oldBooking.DepartureDate);

                //store the dates included in the duration after departure
                var duration = oldBooking.Brochure.Duration;
                var daysBefore = duration - 1;

                while(duration > 0)
                {
                    unavailableDates.Add(oldBooking.DepartureDate.AddDays(duration));
                    duration--;
                }

                //store the dates included in the lead upto the departure so bookings dont overlap
                while(daysBefore > 0)
                {
                    unavailableDates.Add(oldBooking.DepartureDate.AddDays(-daysBefore));
                    daysBefore--;
                }
            }

            if(unavailableDates.Contains(model.DepartureDate))
            {
                model.ErrorMessage = "Date is already booked";
                return View(model);
            }

            //get the brochure from the database
            Brochure brochure = await _context.Brochures.FindAsync(model.BrochureId);

            //check if user already has a booking in progress
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userResults = _context.Bookings.Where(b => b.UserId.Equals(userId)).Where(b => b.Status.Equals("IN_PROGRESS"));

            //remove booking from database if one exists
            if(await userResults.CountAsync() > 0)
            {
                foreach(var oldBooking in userResults)
                {
                    _context.Remove(oldBooking);
                }
                //var oldBooking = await userResults.FirstOrDefaultAsync();
            }

            //create a new booking
            Booking booking = new Booking
            {
                BrochureId = model.BrochureId,
                Brochure = await _context.Brochures.FindAsync(model.BrochureId),
                DepartureDate = (DateTime) model.DepartureDate,
                PaymentType = PaymentType.FULL,
                TotalPrice = brochure.PricePerPerson,
                AmountPaid = 0,
                SpecialRequirements = "",
                Status = "IN_PROGRESS",
                UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                DateCompleted = DateTime.Now
            };

            //add booking to database
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            //redirect to AddPeople action
            return RedirectToAction("AddNumberOfPeople", "Person", new { bookingId = booking.BookingId });
        }

        /// <summary>
        /// loads the confirmation page to finalise and make payments
        /// </summary>
        /// <param name="bookingId">id of booking</param>
        /// <returns>Confirmation page</returns>
        public async Task<IActionResult> Confirmation(int? bookingId)
        {
            //if booking id is null go to browse brochures
            if(bookingId == null) return RedirectToAction(nameof(BrochureController.Browse), "Brochure");

            //gets all data from the database related to the booking
            var booking = await _context.Bookings
                .Where(b => b.BookingId == bookingId)
                .Include(b => b.Brochure)
                .Include(b => b.Brochure.Accomodation)
                .Include(b => b.Brochure.Accomodation.Country)
                .Include(b => b.Persons)
                .FirstOrDefaultAsync();

            //if booking doesnt exist, redirect to browse brochures
            if(booking == null) return RedirectToAction(nameof(BrochureController.Browse), "Brochure");

            //updates the total price of the booking in the database
            booking.TotalPrice = booking.Brochure.PricePerPerson * booking.Persons.Count();
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            //populate the viewmodel and inject into the view
            BookingConfirmationViewModel model = new BookingConfirmationViewModel
            {
                BookingId = (int) bookingId,
                AccomodationName = booking.Brochure.Accomodation.AccomodationName,
                CountryName = booking.Brochure.Accomodation.Country.CountryName,
                DepartureDate = booking.DepartureDate,
                Duration = booking.Brochure.Duration,
                Catering = booking.Brochure.Catering,
                TotalPrice = booking.TotalPrice,
                Persons = booking.Persons,
                Status = booking.Status,
                SpecialRequirements = booking.SpecialRequirements
            };
            return View(model);
        }

        /// <summary>
        /// checks what payment method the user wishes to use
        /// </summary>
        /// <param name="model">View model for the page</param>
        /// <returns>Redirects to make payment page</returns>
        [HttpPost]
        public async Task<IActionResult> Confirmation(BookingConfirmationViewModel model)
        {
            //get booking from the database
            var booking = await _context.Bookings.FindAsync(model.BookingId);

            //if booking doesnt exist redirect to the previous page
            if(booking == null) return RedirectToAction(nameof(BookingController.Confirmation), new { bookingId = model.BookingId });

            //update payment method in the database
            booking.PaymentType = model.PaymentType;
            _context.Update(booking);
            await _context.SaveChangesAsync();

            //redirect to make payment page
            return RedirectToAction(nameof(BookingController.MakePayment), new { bookingId = model.BookingId });
        }

        public async Task<IActionResult> MakePayment(int? bookingId)
        {
            //get booking from database
            var booking = await _context.Bookings
                .Where(b => b.BookingId == bookingId)
                .Include(b => b.Persons).FirstOrDefaultAsync();

            //instatiate the fields required to load future payments
            double initialPay;
            Dictionary<DateTime, double> futurePayments = new Dictionary<DateTime, double>();

            //if paying in full, set initial pay to booking total
            if(booking.PaymentType.Equals(PaymentType.FULL))
                initialPay = booking.TotalPrice;

            //if low deposit, charge £25 per person
            else if(booking.PaymentType.Equals(PaymentType.LOW))
            {
                initialPay = (booking.Persons.Count() * 25);
                futurePayments = findLowDeposit(booking, initialPay);
            }
                
            //if standard deposit, charge half of the total booking
            else if(booking.PaymentType.Equals(PaymentType.STANDARD)) 
            {
                initialPay = (booking.TotalPrice / 2);
                futurePayments = findStandardDeposit(booking, initialPay);
            }
                
            //if pay monthly, charge £25 per person
            else if(booking.PaymentType.Equals(PaymentType.MONTHLY))
            {
                initialPay = (booking.Persons.Count() * 25);
                futurePayments = findMonthlyPayments(booking, initialPay);
            }
            //else return error
            else
            {
                return NotFound();
            }

            //populate model
            var model = new MakePaymentViewModel
            {
                BookingId = booking.BookingId,
                TotalPrice = booking.TotalPrice,
                PaymentType = booking.PaymentType,
                InitialPay = initialPay,
                FuturePayments = futurePayments
            };

            //load make payment page
            return View(model);
        }

        /// <summary>
        /// Loads the payment details into stripe API
        /// </summary>
        /// <param name="bookingId">id of booking</param>
        /// <param name="stripeEmail">email user has entered</param>
        /// <param name="stripeToken">the token of payment details</param>
        /// <returns>Invoice page</returns>
        [HttpPost]
        public async Task<IActionResult> MakePayment(int? bookingId, double initialPay, string stripeEmail, string stripeToken)
        {
            //gets the booking from the database
            var booking = await _context.Bookings
                .Where(b => b.BookingId == bookingId)
                .Include(b => b.Persons).FirstOrDefaultAsync();

            //creates new objects required from the stripe API
            var customerService = new CustomerService();
            var chargeService = new ChargeService();

            //creating a customer using the API
            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            //charging the customer using the details from the booking
            var charge = await chargeService.CreateAsync(new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(initialPay * 100),
                Description = "Booking Id: " + booking.BookingId,
                Currency = "gbp",
                CustomerId = customer.Id
            });

            //get the url of the invoice
            var callbackUrl = Url.Action(
                "Invoice",
                "Booking",
                new
                {
                    bookingId = bookingId
                },
                protocol: Request.Scheme);

            //send email to address entered in stripe payment
            await _emailSender.SendEmailAsync(stripeEmail, "Invoice for booking",
                $"Please see your booking invoice by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            //set the status of the booking to complete
            booking.Status = "Completed";

            //update the amount paid
            booking.AmountPaid = initialPay;

            //set the date when booking was complete
            booking.DateCompleted = DateTime.Now;

            //update the booking in the database
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            //load the invoice page
            return RedirectToAction(nameof(Invoice), new { bookingId = booking.BookingId });
        }

        /// <summary>
        /// Confirms the booking and loads the invoice page
        /// </summary>
        /// <param name="bookingId">id of booking</param>
        /// <returns>Invoice page</returns>
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> Invoice(int? bookingId)
        {
            //if booking id is null go to browse brochures
            if(bookingId == null) return RedirectToAction(nameof(BrochureController.Browse), "Brochure");

            //get booking from database
            var booking = await _context.Bookings
                .Where(b => b.BookingId == bookingId)
                .Include(b => b.Brochure)
                .Include(b => b.Brochure.Accomodation)
                .Include(b => b.Brochure.Accomodation.Country)
                .Include(b => b.Persons)
                .Include(b => b.User)
                .FirstOrDefaultAsync();

            //get current user
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //if user is not logged in, redirect to browse
            if(currentUserId == null) 
                return RedirectToAction(nameof(BrochureController.Browse), "Brochure");

            //if users dont match or is not staff, redirect to browse
            if((!currentUserId.Equals(booking.User.Id) && !User.Identity.IsAuthenticated)) 
                return RedirectToAction(nameof(BrochureController.Browse), "Brochure");

            //instantiate fields for loading payments
            double initialPay;
            Dictionary<DateTime, double> futurePayments = new Dictionary<DateTime, double>();

            //if paying in full, set initial pay to booking total
            if(booking.PaymentType.Equals(PaymentType.FULL))
                initialPay = booking.TotalPrice;

            //if low deposit, charge £25 per person
            else if(booking.PaymentType.Equals(PaymentType.LOW))
            {
                initialPay = (booking.Persons.Count() * 25);
                futurePayments = findLowDeposit(booking, initialPay);
            }
                
            //if standard deposit, charge half of the total booking
            else if(booking.PaymentType.Equals(PaymentType.STANDARD)) 
            {
                initialPay = (booking.TotalPrice / 2);
                futurePayments = findStandardDeposit(booking, initialPay);
            }
                
            //if pay monthly, charge £25 per person
            else if(booking.PaymentType.Equals(PaymentType.MONTHLY))
            {
                initialPay = (booking.Persons.Count() * 25);
                futurePayments = findMonthlyPayments(booking, initialPay);
            }
            //else return error
            else
            {
                return NotFound();
            }

            if(booking.SpecialRequirements == null)
            {
                booking.SpecialRequirements = "";
            }

            //populate the model with the necessary data
            BookingInvoiceViewModel model = new BookingInvoiceViewModel
            {
                BookingId = (int) bookingId,
                BookingStatus = booking.Status,
                AccomodationName = booking.Brochure.Accomodation.AccomodationName,
                CountryName = booking.Brochure.Accomodation.Country.CountryName,
                DepartureDate = booking.DepartureDate,
                Duration = booking.Brochure.Duration,
                Catering = booking.Brochure.Catering,
                TotalPrice = booking.TotalPrice,
                AmountPaid = booking.AmountPaid,
                Persons = booking.Persons,
                Image = booking.Brochure.ImageLink,
                SpecialRequirements = booking.SpecialRequirements,
                PaymentType = booking.PaymentType,
                InitialPay = initialPay,
                FuturePayments = futurePayments
            };
            
            //if booking is not complete, redirect to booking confirmation
            if(booking.Status.Equals("IN PROGRESS")) 
                return RedirectToAction(nameof(BookingController.Confirmation), new { bookingId = bookingId });

            //loads page, converts view to pdf
            HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);
            return View(model);
        }

        /// <summary>
        /// allows users to cancel a booking
        /// </summary>
        /// <param name="bookingId">identification for the booking</param>
        /// <returns>Cancel page</returns>
        public async Task<IActionResult> Cancel(int? bookingId)
        {
            //get the booking from the database
            var model = await _context.Bookings
                .Where(b => b.BookingId == bookingId)
                .Include(b => b.Brochure)
                .Include(b => b.Brochure.Accomodation)
                .FirstOrDefaultAsync();

            //load page
            return View(model);
        }

        /// <summary>
        /// confirms cancellation of booking and charges customer
        /// </summary>
        /// <param name="bookingId">id for booking</param>
        /// <param name="stripeEmail">email attatched to stripe payment</param>
        /// <param name="stripeToken">token generated by stripe client</param>
        /// <returns>Redirects to my bookings page</returns>
        [HttpPost]
        [ActionName("Cancel")]
        public async Task<IActionResult> CancelConfirmed(int? bookingId, string stripeEmail, string stripeToken)
        {
            //gets the booking from the database
            var booking = await _context.Bookings.FindAsync(bookingId);

            //finds out how many days are between the booking and cancelling
            var daysBetween = (booking.DepartureDate - DateTime.Now).TotalDays;
            var price = booking.TotalPrice - booking.AmountPaid;

            //if its less than 7 days before departure, charge a 75% fee of the initial payment
            if(daysBetween > 7)
            {
                price *= 0.75;
            }
            
            //creates new objects required from the stripe API
            if(!booking.PaymentType.ToString().Equals("FULL"))
            {
                var customerService = new CustomerService();
                var chargeService = new ChargeService();

                //creating a customer using the API
                var customer = customerService.Create(new CustomerCreateOptions
                {
                    Email = stripeEmail,
                    SourceToken = stripeToken
                });

                //charging the customer using the details from the booking
                var charge = await chargeService.CreateAsync(new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(price * 100),
                    Description = "Booking Id: " + booking.BookingId,
                    Currency = "gbp",
                    CustomerId = customer.Id
                });
            }
            
            //update booking in the database
            booking.Status = "Cancelled";
            _context.Update(booking);
            await _context.SaveChangesAsync();

            //redirect to my bookings page
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    
        /// <summary>
        /// calculation for finding the dates and price of monthly payments
        /// </summary>
        /// <param name="booking">booking object</param>
        /// <param name="initialPay">the deposit user has to pay</param>
        /// <returns>a dictionary of dates and prices</returns>
        private Dictionary<DateTime, double> findMonthlyPayments(Booking booking, double initialPay)
        {
            //create new dictionary
            var futurePayments = new Dictionary<DateTime, double>();
            
            //get months between current date and date of departure
            int monthsBetween = ((booking.DepartureDate.Year - DateTime.Now.Year) * 12) + booking.DepartureDate.Month - DateTime.Now.Month;

            //find the price to spread equally
            var price = (booking.TotalPrice - initialPay) / monthsBetween;

            //for every month between, add the date and price to the dictionary
            for(int i = 1; i < monthsBetween + 1; i++)
            {
                futurePayments.Add(DateTime.Now.AddMonths(i), price);
            }

            //return dictionary of payments
            return futurePayments;
        }

        /// <summary>
        /// calculation for finding the dates and price of a standard deposit
        /// </summary>
        /// <param name="booking">booking object</param>
        /// <param name="initialPay">the deposit user has to pay</param>
        /// <returns>a dictionary of dates and prices</returns>
        private Dictionary<DateTime, double> findStandardDeposit(Booking booking, double initialPay)
        {
            //create a dictionary
            var futurePayments = new Dictionary<DateTime, double>();

            //half the total price
            var price = booking.TotalPrice / 2;

            //add the payment 14 days before departure
            futurePayments.Add(booking.DepartureDate.AddDays(-14), price);

            //return dictionary of payments
            return futurePayments;
        }

        /// <summary>
        /// calculation for finding the dates and price of a low deposit payment
        /// </summary>
        /// <param name="booking">booking object</param>
        /// <param name="initialPay">the deposit user has to pay</param>
        /// <returns>a dictionary of dates and prices</returns>
        private Dictionary<DateTime, double> findLowDeposit(Booking booking, double initialPay)
        {
            //create a dictionary
            var futurePayments = new Dictionary<DateTime, double>();

            //find the price of each payment
            var price = (booking.TotalPrice - initialPay) / 3;
            
            //add payment for the next 2 months
            for(var i = 1; i < 3; i++)
            {
                futurePayments.Add(DateTime.Now.AddMonths(i), price);
            }

            //add a payment 14 days before departure
            futurePayments.Add(booking.DepartureDate.AddDays(-14), price);

            //return dictionary of payments
            return futurePayments;
        }
    }
}
