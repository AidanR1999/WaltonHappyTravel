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
<<<<<<< HEAD
        [Key]
        public int BrochureId { get; set; }
        public DateTime DepartureDate { get; set; }
        public int Duration { get; set; }
        public double PricePerPerson { get; set; }
        public string Description { get; set; }
        public Catering Catering { get; set; }
        public int MaxPeople { get; set; }
        public int MaxRooms { get; set; }

=======
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
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
        public Brochure()
        {
            Bookings = new List<Booking>();
        }

        //nav properties
<<<<<<< HEAD
        //1:M - Brochure:Booking
        public virtual IEnumerable<Booking> Bookings { get; set; }

        //1:M - Category:Brochures
=======

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
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
        [InverseProperty("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        //1:M - Accomodation:Brochure
<<<<<<< HEAD
=======
        /// <summary>
        /// Accomodation associated with the brochure
        /// </summary>
        /// <value>Accomodation</value>
        [Required]
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
        [InverseProperty("Accomodation")]
        public int AccomodationId { get; set; }
        public virtual Accomodation Accomodation { get; set; }
    }

<<<<<<< HEAD
=======
    /// <summary>
    /// Enum of catering available
    /// </summary>
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
    public enum Catering
    {
        ALL_INCLUSIVE, HALF_BOARD, SELF_CATERING
    }
}