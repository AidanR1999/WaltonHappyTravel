using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walton_Happy_Travel.Models
{
    public class AddNumberOfPeopleViewModel
    {
        public int BookingId { get; set; }

        [Display(Name = "How many people on the booking?")]
        public int PeopleAdded { get; set; }
    }
}