using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CharityWeb.Models
{
    public class RateModels
    {
        public int Id { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public string Feedback { get; set; }

        [Required]
        public string SubmittedUser { get; set; }
    }
}