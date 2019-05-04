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
    /// Holds info about every user in the system
    /// Inherits from Identity User
    /// </summary>
    public abstract class ApplicationUser : IdentityUser
    {
        public string Forename { get; set; }
        public string MiddleNames { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime TimeOfRegistration { get; set; }

        public ApplicationUser()
        {
            Bookings = new List<Booking>();
        }

        //nav properties
        //1:M - User:Bookings
        public virtual IEnumerable<Booking> Bookings { get; set; }
    }
}
