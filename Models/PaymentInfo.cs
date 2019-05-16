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
        
        /// <summary>
        /// Type of card
        /// </summary>
        /// <value>Enum of CardType</value>
        public CardType CardType { get; set; }

        /// <summary>
        /// Number on card
        /// </summary>
        /// <value>string</value>
        public string CardNumber { get; set; }

        /// <summary>
        /// Name on card
        /// </summary>
        /// <value>string</value>
        public string NameOnCard { get; set; }

        /// <summary>
        /// Security digit on back of card
        /// </summary>
        /// <value>string</value>
        public string SecurityNumber { get; set; }

        /// <summary>
        /// ExpiryDate of card
        /// </summary>
        /// <value>DateTime</value>
        public DateTime ExpiryDate { get; set; }

        //nav properties
        //1:1 - User:PaymentInfo
        /// <summary>
        /// User on the booking
        /// </summary>
        /// <value>ApplicationUser</value>
        [InverseProperty("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    /// <summary>
    /// Enumerable for types of card available
    /// </summary>
    public enum CardType
    {
        VISA, MASTERCARD
    }
}