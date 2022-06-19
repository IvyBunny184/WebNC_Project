namespace WebNC_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Service
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Service()
        {
            BookingServices = new HashSet<BookingServices>();
        }

        [StringLength(10, ErrorMessage = "{0} is contains at most 10 characters")]
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Mã dịch vụ")]
        public string ID { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Tên dịch vụ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Please enter valid price")]
        [Display(Name = "Giá dịch vụ")]
        public double Price { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingServices> BookingServices { get; set; }

        [NotMapped]
        public string Display
        {
            get
            {
                return $"{Name} (Price:{Price} $)";
            }
        }
    }
}
