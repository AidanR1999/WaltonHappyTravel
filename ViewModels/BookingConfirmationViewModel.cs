using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walton_Happy_Travel.Models
{
    public class BookingConfirmationViewModel
    {
        public int BookingId { get; set; }
        public string AccomodationName { get; set; }
        public string CountryName { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DepartureDate { get; set; }
        public int Duration { get; set; }
        public Catering Catering { get; set; }
        public double TotalPrice { get; set; }
        public IEnumerable<Person> Persons { get; set; }
        public string Status { get; set; }
    }
}