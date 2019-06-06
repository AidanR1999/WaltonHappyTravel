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
    /// ViewModel for the payment page
    /// </summary>
    public class MakePaymentViewModel
    {
        public int BookingId { get; set; }
        public double TotalPrice { get; set; }
        public double InitialPay { get; set; }
        public PaymentType PaymentType { get; set; }
        public Dictionary<DateTime, double> FuturePayments { get; set; }
    }
}