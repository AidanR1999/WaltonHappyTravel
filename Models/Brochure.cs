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
        /// <summary>
        /// Brochure Identifier
        /// </summary>
        /// <value>integer</value>
        [Key]
        public int BrochureId { get; set; }
        
        /// <summary>
        /// Duration of the brochure
        /// </summary>
        /// <value>integer</value>
        [Required]
        public int Duration { get; set; }

        /// <summary>
        /// Price per person of the brochure
        /// </summary>
        /// <value>double</value>
        [Required]
        public double PricePerPerson { get; set; }

        /// <summary>
        /// Description of the brochure
        /// </summary>
        /// <value>string</value>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// The catering available with the brochure
        /// </summary>
        /// <value>Enum Catering</value>
        [Required]
        public Catering Catering { get; set; }

        /// <summary>
        /// The max amount of people that can be added to booking this brochure
        /// </summary>
        /// <value></value>
        [Required]
        public int MaxPeople { get; set; }

        /// <summary>
        /// Parameterless contstructor to initialise the Enumerable of bookings
        /// </summary>
        public Brochure()
        {
            Bookings = new List<Booking>();
        }

        //nav properties

        //1:M - Brochure:Booking
        /// <summary>
        /// Enumerable of bookings using the brochure
        /// </summary>
        /// <value>Enumerable of bookings</value>
        public virtual IEnumerable<Booking> Bookings { get; set; }

        //1:M - Category:Brochures
        /// <summary>
        /// Holiday category associated with the brochure
        /// </summary>
        /// <value>Category</value>
        [Required]
        [InverseProperty("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        //1:M - Accomodation:Brochure
        /// <summary>
        /// Accomodation associated with the brochure
        /// </summary>
        /// <value>Accomodation</value>
        [Required]
        [InverseProperty("Accomodation")]
        public int AccomodationId { get; set; }
        public virtual Accomodation Accomodation { get; set; }
    }

    /// <summary>
    /// Enum of catering available
    /// </summary>
    public enum Catering
    {
        ALL_INCLUSIVE, HALF_BOARD, SELF_CATERING
    }
}