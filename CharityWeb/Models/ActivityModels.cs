using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CharityWeb.Models
{
    public class ActivityModels
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Time { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Info { get; set; }
    }
}