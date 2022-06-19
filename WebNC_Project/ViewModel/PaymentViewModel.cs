using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebNC_Project.ViewModel
{
    public class PaymentViewModel
    {
        [Required]
        public int BillID { get; set; }

        public string CardName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string StripeToken { get; set; }
    }
}