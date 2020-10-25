using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheEventApp.Models
{
    public class EventDetailModel
    {
        public Event Event { get; set; }
        public List<EventAttendee> EventAttendees { get; set; }


    }
}