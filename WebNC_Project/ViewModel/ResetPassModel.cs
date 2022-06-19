using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebNC_Project.ViewModel
{
    public class ResetPassModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        [MaxLength(20, ErrorMessage = "{0} contains at most 20 characters")]
        [MinLength(6, ErrorMessage = "{0} contains at least 6 characters")]
        [Required(ErrorMessage = "{0} is required")]
        public string OldPass { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        [MaxLength(20, ErrorMessage = "{0} contains at most 20 characters")]
        [MinLength(6, ErrorMessage = "{0} contains at least 6 characters")]
        [Required(ErrorMessage = "{0} is required")]
        public string NewPass { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [MaxLength(20, ErrorMessage = "{0} contains at most 20 characters")]
        [MinLength(6, ErrorMessage = "{0} contains at least 6 characters")]
        [Compare("NewPass", ErrorMessage = "Enter exactly new password")]
        [Required(ErrorMessage = "{0} is required")]
        public string ConfirmPass { get; set; }
    }
}