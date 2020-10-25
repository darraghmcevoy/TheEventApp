using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheEventApp.Models
{
    public class EventModel : Event
    {
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}