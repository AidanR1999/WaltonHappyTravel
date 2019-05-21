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
    /// Holds the details of people attatched to a booking
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Person Identifier
        /// </summary>
        /// <value>integer</value>
        [Key]
        public int PersonId { get; set; }

        /// <summary>
        /// First name of the person
        /// </summary>
        /// <value>string</value>
        [Required]
        public string Forename { get; set; }

        /// <summary>
        /// Middle names of the person
        /// </summary>
        /// <value>string</value>
        [Display(Name = "Middle Name(s)")]
        public string MiddleNames { get; set; }

        /// <summary>
        /// Last name of the person
        /// </summary>
        /// <value>string</value>
        [Required]
        public string Surname { get; set; }

        /// <summary>
        /// Birthdate of the person
        /// </summary>
        /// <value>DateTime</value>
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        //nav properties
        //1:M - Booking:Person
        /// <summary>
        /// Booking associated with the person
        /// </summary>
        /// <value>Booking</value>
        [InverseProperty("Booking")]
        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; }
    }
}