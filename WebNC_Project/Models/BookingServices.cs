using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebNC_Project.Models
{
    public class BookingServices
    {
        [Key, Column(Order = 1)]
        [Required]
        public int BookingID { get; set; }

        [Key, Column(Order = 2)]
        [MaxLength(10)]
        [Required]
        public string ServiceID { get; set; }

        public virtual Booking Booking { get; set; }

        public virtual Service Service { get; set; }
    }
}