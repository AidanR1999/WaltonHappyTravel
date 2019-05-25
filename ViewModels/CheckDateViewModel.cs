using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walton_Happy_Travel.Models
{
    public class CheckDateViewModel
    {
        public int BrochureId { get; set; }
        public DateTime DepartureDate { get; set; }
        public IEnumerable<DateTime> UnavailableDates { get; set; }
    }
}