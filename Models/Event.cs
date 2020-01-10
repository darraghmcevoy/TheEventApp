using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace TheEventApp.Models
{
    public class Event
    {
        public int EventID { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }
    }

    public class EventContext : DbContext
    {
        public EventContext()
        {
            Database.Log = s => Debug.WriteLine(s);
        }
        public DbSet<Event> Events { get; set; }
        
    }
}