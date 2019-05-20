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
<<<<<<< HEAD
        public string Forename { get; set; }
        public string MiddleNames { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime TimeOfRegistration { get; set; }

=======
        /// <summary>
        /// First name of the user
        /// </summary>
        /// <value>string</value>
        [Required]
        public string Forename { get; set; }

        /// <summary>
        /// Middle names of the user
        /// </summary>
        /// <value>string</value>
        [Display(Name = "Middle Name(s)")]
        public string MiddleNames { get; set; }

        /// <summary>
        /// Last name of the user
        /// </summary>
        /// <value>string</value>
        [Required]
        public string Surname { get; set; }

        /// <summary>
        /// Birthdate of the user
        /// </summary>
        /// <value>DateTime</value>
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Date/Time of user registration
        /// </summary>
        /// <value>DateTime</value>
        [Required]
        public DateTime TimeOfRegistration { get; set; }

        /// <summary>
        /// Parameterless constructor initialising enumerable of bookings
        /// </summary>
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
        public ApplicationUser()
        {
            Bookings = new List<Booking>();
        }

        //nav properties
        //1:M - User:Bookings
<<<<<<< HEAD
=======
        /// <summary>
        /// List of bookings the user has made
        /// </summary>
        /// <value>Enumerable of bookings</value>
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
        public virtual IEnumerable<Booking> Bookings { get; set; }
    }
}
