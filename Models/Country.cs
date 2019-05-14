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
    /// The country of the accomodation/brochure
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Country Identifier
        /// </summary>
        /// <value>integer</value>
        [Key]
        public int CountryId { get; set; }

        /// <summary>
        /// Name of country
        /// </summary>
        /// <value>string</value>
        [Required]
        [Display(Name = "Name")]
        public string CountryName { get; set; }

        /// <summary>
        /// Parameterless constructor to initialise the Enumerable of accomodations
        /// </summary>
        public Country()
        {
            Accomodations = new List<Accomodation>();
        }

        //nav properties
        //1:M - Category:Brochure
        /// <summary>
        /// Accomodations that are in the country
        /// </summary>
        /// <value>Enumerable of accomodations</value>
        public virtual IEnumerable<Accomodation> Accomodations { get; set; }
    }
}