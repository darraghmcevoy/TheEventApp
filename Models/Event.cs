using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace TheEventApp.Models
{
    public class Event
    {
        [Required(ErrorMessage = "Please Enter Your EventID")]
        [Display(Name = "Event ID")]
        public int EventID { get; set; }
        [Required(ErrorMessage = "Please enter the name of this event")]
        [Display(Name = "Event Name")]
        [MaxLength(30)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter the date of your event")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter your Email address")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]


        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your mobile number")]
        [Display(Name = "Contact number")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
       
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