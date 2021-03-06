﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Walton_Happy_Travel.Models
{
    /// <summary>
    /// Holds info about every user in the system
    /// Inherits from Identity User
    /// </summary>
    public abstract class ApplicationUser : IdentityUser
    {
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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Date/Time of user registration
        /// </summary>
        /// <value>DateTime</value>
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Time of Registration")]
        public DateTime TimeOfRegistration { get; set; }

        /// <summary>
        /// Parameterless constructor initialising enumerable of bookings
        /// </summary>
        public ApplicationUser()
        {
            Bookings = new List<Booking>();
        }

        //nav properties
        //1:M - User:Bookings
        /// <summary>
        /// List of bookings the user has made
        /// </summary>
        /// <value>Enumerable of bookings</value>
        public virtual IEnumerable<Booking> Bookings { get; set; }
        
    }
}
