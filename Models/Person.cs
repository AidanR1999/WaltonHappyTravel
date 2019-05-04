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
        [Key]
        public int PersonId { get; set; }
        public string Forename { get; set; }
        public string MiddleNames { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }

        //nav properties
        //1:M - Booking:Person
        [InverseProperty("Booking")]
        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; }
    }
}