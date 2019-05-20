using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
<<<<<<< HEAD
=======
using Microsoft.AspNetCore.Http;
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walton_Happy_Travel.Models
{
<<<<<<< HEAD
    public abstract class Accomodation
    {
        [Key]
        public int AccomodationId { get; set; }
        public string AccomodationName { get; set; }
        public string AccomodationAddress { get; set; }
        public string Description { get; set; }

=======
    /// <summary>
    /// Super class of accomodations available on the service.
    /// Holds the basic information of every accomodation
    /// </summary>
    public abstract class Accomodation
    {
        /// <summary>
        /// Accomodation Identifier
        /// </summary>
        /// <value>integer</value>
        [Key]
        public int AccomodationId { get; set; }

        /// <summary>
        /// Name of Accomodation
        /// </summary>
        /// <value>string</value>
        [Required]
        [Display(Name = "Name")]
        public string AccomodationName { get; set; }

        /// <summary>
        /// Address of Accomodation
        /// </summary>
        /// <value>string</value>
        [Required]
        [Display(Name = "Address")]
        public string AccomodationAddress { get; set; }

        /// <summary>
        /// Description associated with accomodation
        /// </summary>
        /// <value>string</value>
        public string Description { get; set; }

        /// <summary>
        /// Parameterless constructure initailising the Enumerable brochures
        /// </summary>
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
        public Accomodation()
        {
            Brochures = new List<Brochure>();
        }
        
        //nav properties
<<<<<<< HEAD
        //1:M - Accomodation:Brochure
        public virtual IEnumerable<Brochure> Brochures { get; set; }

        //1:M - Country:Accomodation
=======

        //1:M - Accomodation:Brochure
        /// <summary>
        /// A list of every brochure associated with this accomodation
        /// </summary>
        /// <value>IEnumarable of Brochure</value>
        public virtual IEnumerable<Brochure> Brochures { get; set; }

        //1:M - Country:Accomodation
        /// <summary>
        /// The country this accomodation is in
        /// </summary>
        /// <value>Country</value>
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
        [InverseProperty("Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}