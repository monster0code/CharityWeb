using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CharityWeb.Models
{
    public class NursingAppointmentModels
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public int HomeId { get; set; }

        [Required]
        public DateTime YourDate { get; set; }
    }
}