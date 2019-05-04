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
    /// The category of brochures
    /// </summary>
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Category()
        {
            Brochures = new List<Brochure>();
        }

        //nav properties
        //1:M - Category:Brochure
        public virtual IEnumerable<Brochure> Brochures { get; set; }
    }
}