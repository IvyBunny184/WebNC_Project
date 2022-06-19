using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebNC_Project.ViewModel
{
    public class FeedBackViewModel
    {
        [Display(Name = "Nội dung")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        public string Content { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Range(1,5)]
        public int Rate { get; set; }

        public int BillID { get; set; }
    }
}