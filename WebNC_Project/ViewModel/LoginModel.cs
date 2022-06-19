using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebNC_Project.ViewModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [MinLength(6, ErrorMessage = "{0} contains at least 6 characters")]
        [MaxLength(20, ErrorMessage = "{0} contains at most 20 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [MinLength(6, ErrorMessage = "{0} contains at least 6 characters")]
        [MaxLength(20, ErrorMessage = "{0} contains at most 20 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}