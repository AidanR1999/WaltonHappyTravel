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
    /// Holds details of a hotel.
    /// Inherits from accomodation
    /// </summary>
    public class Hotel : Accomodation
    {
<<<<<<< HEAD
=======
        /// <summary>
        /// Rating of the hotel
        /// </summary>
        /// <value>integer</value>
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
        public int Rating { get; set; }
    }
}