using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Walton_Happy_Travel.Models
{
    /// <summary>
    /// Holds details of bookings that users make
    /// </summary>
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
<<<<<<< HEAD
        public int NoOfRooms { get; set; }
        public PaymentType PaymentType { get; set; }
        public double TotalPrice { get; set; }
        public double AmountPaid { get; set; }
        public string SpecialRequirements { get; set; }

=======

        /// <summary>
        /// Enum that holds the payment type of the booking
        /// </summary>
        /// <value>Enum PaymentType</value>
        [Required]
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// Date of Departure of the booking
        /// </summary>
        /// <value>DateTime</value>
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of departure")]
        public DateTime DepartureDate { get; set; }

        /// <summary>
        /// Total cost of the booking
        /// </summary>
        /// <value>double</value>
        [Required]
        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }

        /// <summary>
        /// Amount the user has paid so far for the booking
        /// </summary>
        /// <value>double</value>
        [Display(Name = "Amount Paid")]
        public double AmountPaid { get; set; }

        /// <summary>
        /// Outlines any special requirements attatched to the booking by the user
        /// </summary>
        /// <value>string</value>
        [Display(Name = "Special Requirements")]
        public string SpecialRequirements { get; set; }

        /// <summary>
        /// Shows ongoing status of the booking
        /// </summary>
        /// <value>string</value>
        [Required]
        public string Status { get; set; }

        /// <summary>
        /// Parameterless constructor to initialise the Enumerable of people
        /// </summary>
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
        public Booking()
        {
            Persons = new List<Person>();
        }

        //nav properties
<<<<<<< HEAD
        //1:M - User:Booking
=======

        //1:M - User:Booking
        /// <summary>
        /// The user that made the booking
        /// </summary>
        /// <value>User</value>
        [Required]
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
        [InverseProperty("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        //1:M - Brochure:Booking
<<<<<<< HEAD
=======
        /// <summary>
        /// The brochure that the booking is made under
        /// </summary>
        /// <value>Brochure</value>
        [Required]
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
        [InverseProperty("Brochure")]
        public int BrochureId { get; set; }
        public virtual Brochure Brochure { get; set; }

        //1:M - Booking:Person
<<<<<<< HEAD
        public virtual IEnumerable<Person> Persons { get; set; }
    }

    public enum PaymentType
    {
        CARD, PAYPAL
=======
        /// <summary>
        /// List of people that are attatched to the booking
        /// </summary>
        /// <value></value>
        public virtual IEnumerable<Person> Persons { get; set; }
    }

    /// <summary>
    /// Holds the payments types available on the service
    /// </summary>
    public enum PaymentType
    {
        STRIPE, PAYPAL
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
    }
}