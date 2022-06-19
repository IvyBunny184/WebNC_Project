namespace WebNC_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Voucher
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Voucher()
        {
            Bookings = new HashSet<Booking>();
            FromDate = DateTime.Now;
            ToDate = DateTime.Now.AddDays(1);
        }

        [Key]
        [MaxLength(20,ErrorMessage = "{0} constain at most 20 characters")]
        [MinLength(6, ErrorMessage = "{0} constain at least 6 characters")]
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Mã voucher")]
        public string Code { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Range(1,100,ErrorMessage = "{0} from 1 to 100")]
        public int Discount { get; set; }

        [Column(TypeName = "datetime2")]
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Từ ngày")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "datetime2")]
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Đến ngày")]
        public DateTime ToDate { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Giá trị áp dụng tối thiểu")]
        public int Condition { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
