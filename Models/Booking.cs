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
        public int NoOfRooms { get; set; }
        public PaymentType PaymentType { get; set; }
        public double TotalPrice { get; set; }
        public double AmountPaid { get; set; }
        public string SpecialRequirements { get; set; }

        public Booking()
        {
            Persons = new List<Person>();
        }

        //nav properties
        //1:M - User:Booking
        [InverseProperty("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        //1:M - Brochure:Booking
        [InverseProperty("Brochure")]
        public int BrochureId { get; set; }
        public virtual Brochure Brochure { get; set; }

        //1:M - Booking:Person
        public virtual IEnumerable<Person> Persons { get; set; }
    }

    public enum PaymentType
    {
        CARD, PAYPAL
    }
}