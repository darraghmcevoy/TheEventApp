using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
using System.Web;

namespace TheEventApp.Models
{
    public class Event
    {
        [Display(Name = "Event ID")]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the Name of this event")]
        [Display(Name = "Event Name")]
        [MaxLength(30)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter the description of this event")]
        [Display(Name = "Event Description")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter your Email address")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        [StringLength(500, MinimumLength = 10)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Event Time is required")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        public Nullable<System.TimeSpan> Time { get; set; }

        [Required(ErrorMessage = "Please enter your mobile number")]
        [Display(Name = "Contact Number")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }

        [Display(Name = "Invitee emails (comma seperated)")]
        [DataType(DataType.Text)]
        public string InviteEmail { get; set; }

        [Display(Name = "Image")]
        [DataType(DataType.Text)]
        public string EventImagePath { get; set; }

        public ApplicationUser CreatedBy { get; set; }
    }
}