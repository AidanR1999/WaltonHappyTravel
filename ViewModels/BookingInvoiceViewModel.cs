using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walton_Happy_Travel.Models
{
    public class BookingInvoiceViewModel
    {
        public int BookingId { get; set; }
        public string BookingStatus { get; set; }
        public string AccomodationName { get; set; }
        public string CountryName { get; set; }
        public DateTime DepartureDate { get; set; }
        public int Duration { get; set; }
        public Catering Catering { get; set; }
        public double TotalPrice { get; set; }
        public double AmountPaid { get; set; }
        public IEnumerable<Person> Persons { get; set; }
        public string Image { get; set; }
    }
}