using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walton_Happy_Travel.Models
{
    public class AddPersonToBookingViewModel
    {
        public int BookingId { get; set; }

        public IList<Person> PeopleToAdd { get; set; }

        public AddPersonToBookingViewModel()
        {
            PeopleToAdd = new List<Person>();
        }
    }
}