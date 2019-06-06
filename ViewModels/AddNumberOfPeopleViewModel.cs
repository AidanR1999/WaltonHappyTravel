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
    /// ViewModel for adding a number of people to the booking
    /// </summary>
    public class AddNumberOfPeopleViewModel
    {
        public int BookingId { get; set; }

        [Display(Name = "How many people on the booking?")]
        public int PeopleAdded { get; set; }

        public string ErrorMessage { get; set; }

        public int MaxPeople { get; set; }
    }
}