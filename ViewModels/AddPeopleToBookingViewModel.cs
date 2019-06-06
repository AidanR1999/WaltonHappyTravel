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
    /// ViewModel for adding people info to the booking
    /// </summary>
    public class AddPeopleToBookingViewModel
    {
        public int BookingId { get; set; }
        public int NumberOfPeople { get; set; }
        public IList<Person> PeopleToAdd { get; set; }
        public string SpecialRequirements { get; set; }
        public string ErrorMessage { get; set; }
        

        public AddPeopleToBookingViewModel()
        {
            PeopleToAdd = new List<Person>();
        }
    }
}