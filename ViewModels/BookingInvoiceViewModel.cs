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
    /// ViewModel to show info in Invoices
    /// </summary>
    public class BookingInvoiceViewModel
    {
        public int BookingId { get; set; }
        public string BookingStatus { get; set; }
        public string AccomodationName { get; set; }
        public string CountryName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DepartureDate { get; set; }
        public int Duration { get; set; }
        public Catering Catering { get; set; }
        public double TotalPrice { get; set; }
        public double AmountPaid { get; set; }
        public IEnumerable<Person> Persons { get; set; }
        public string Image { get; set; }
        public string SpecialRequirements { get; set; }
        public double InitialPay { get; set; }
        public PaymentType PaymentType { get; set; }
        public Dictionary<DateTime, double> FuturePayments { get; set; }
    }
}