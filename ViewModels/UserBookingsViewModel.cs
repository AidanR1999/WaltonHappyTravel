using System;
using Walton_Happy_Travel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walton_Happy_Travel.Models
{
    public class UserBookingsViewModel
    {
        public IEnumerable<Booking> Bookings { get; set; }

        public UserBookingsViewModel()
        {
            Bookings = new List<Booking>();
        }
    }
}