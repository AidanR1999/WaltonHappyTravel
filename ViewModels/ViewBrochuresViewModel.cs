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
    /// <summary>
    /// ViewModel for browsing and searching brochures
    /// </summary>
    public class ViewBrochuresViewModel
    {
        public IEnumerable<Brochure> Brochures { get; set; }

        public ViewBrochuresViewModel()
        {
            Brochures = new List<Brochure>();
        }

        //filtering data
        public string Accomodation { get; set; }
        public string Country { get; set; }
        public string Catering { get; set; }
        public string Category { get; set; }
        public string ErrorMessage { get; set; }
    }
}