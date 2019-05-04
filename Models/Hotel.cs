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
        public int Rating { get; set; }
    }
}