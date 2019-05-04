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
    /// Holds details of the brochures users can book
    /// </summary>
    public class Brochure
    {
        [Key]
        public int BrochureId { get; set; }
        public DateTime DepartureDate { get; set; }
        public int Duration { get; set; }
        public double PricePerPerson { get; set; }
        public string Description { get; set; }
        public Catering Catering { get; set; }
        public int MaxPeople { get; set; }
        public int MaxRooms { get; set; }

        public Brochure()
        {
            Bookings = new List<Booking>();
        }

        //nav properties
        //1:M - Brochure:Booking
        public virtual IEnumerable<Booking> Bookings { get; set; }

        //1:M - Category:Brochures
        [InverseProperty("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        //1:M - Accomodation:Brochure
        [InverseProperty("Accomodation")]
        public int AccomodationId { get; set; }
        public virtual Accomodation Accomodation { get; set; }
    }

    public enum Catering
    {
        ALL_INCLUSIVE, HALF_BOARD, SELF_CATERING
    }
}