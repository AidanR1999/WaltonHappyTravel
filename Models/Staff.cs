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
    /// Discriminator for staff on the application
    /// </summary>
    public class Staff : ApplicationUser
    {
        public virtual IEnumerable<Accomodation> AccomodationsAssigned { get; set; }

        public Staff()
        {
            AccomodationsAssigned = new List<Accomodation>();
        }
    }
}