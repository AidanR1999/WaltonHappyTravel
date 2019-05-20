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
<<<<<<< HEAD
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

=======
        /// <summary>
        /// Category Identifier
        /// </summary>
        /// <value>integer</value>
        [Key]
        public int CategoryId { get; set; }

        /// <summary>
        /// Name of the category
        /// </summary>
        /// <value>string</value>
        [Required]
        [Display(Name = "Name")]
        public string CategoryName { get; set; }

        /// <summary>
        /// Parameterless constructor initailising enumerable of brochures
        /// </summary>
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
        public Category()
        {
            Brochures = new List<Brochure>();
        }

        //nav properties
        //1:M - Category:Brochure
<<<<<<< HEAD
=======
        /// <summary>
        /// Enumerable of brochures associated with the category
        /// </summary>
        /// <value>Enumerable of brochures</value>
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
        public virtual IEnumerable<Brochure> Brochures { get; set; }
    }
}