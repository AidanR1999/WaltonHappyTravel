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
    /// Holds the payment details of a booking.
    /// Users can also optional add the payment info to their account
    /// </summary>
    public class PaymentInfo
    {
        /// <summary>
        /// Payment Identifier
        /// </summary>
        /// <value>integer</value>
        [Key]
        public int PaymentId { get; set; }
        public CardType CardType { get; set; }
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public string SecurityNumber { get; set; }
        public DateTime ExpiryDate { get; set; }

        //nav properties
        //1:1 - User:PaymentInfo
        [InverseProperty("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public enum CardType
    {
        VISA, MASTERCARD
    }
}