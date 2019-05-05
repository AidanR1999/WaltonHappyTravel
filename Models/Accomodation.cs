using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walton_Happy_Travel.Models
{
    public abstract class Accomodation
    {
        [Key]
        public int AccomodationId { get; set; }
        public string AccomodationName { get; set; }
        public string AccomodationAddress { get; set; }
        public string Description { get; set; }

        public Accomodation()
        {
            Brochures = new List<Brochure>();
        }
        
        //nav properties
        //1:M - Accomodation:Brochure
        public virtual IEnumerable<Brochure> Brochures { get; set; }

        //1:M - Country:Accomodation
        [InverseProperty("Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}