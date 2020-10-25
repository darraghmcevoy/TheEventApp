using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheEventApp.Models
{
    public class EventAttendee
    {
        public int Id { get; set; }
        public Event Event { get; set; }
        public ApplicationUser User { get; set; }
        public bool Accepted { get; set; }
    }
}