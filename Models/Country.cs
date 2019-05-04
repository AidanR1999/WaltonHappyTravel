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
        [Key]
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public Country()
        {
            Accomodations = new List<Accomodation>();
        }

        //nav properties
        //1:M - Category:Brochure
        public virtual IEnumerable<Accomodation> Accomodations { get; set; }
    }
}