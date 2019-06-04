using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walton_Happy_Travel.Models
{
    public class EditStaffViewModel
    {
        [DataType(DataType.Text)]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Forename { get; set; }

        [Display(Name = "Middle Name(s)")]
        [DataType(DataType.Text)]
        public string MiddleNames { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Role { get; set; }
    }
}