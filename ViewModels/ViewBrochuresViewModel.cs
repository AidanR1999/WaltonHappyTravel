using System;
using Walton_Happy_Travel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walton_Happy_Travel
{
    public class ViewBrochuresViewModel
    {
        public IEnumerable<Brochure> Brochures { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public ViewBrochuresViewModel()
        {
            Brochures = new List<Brochure>();
            Categories = new List<Category>();
        }
    }
}