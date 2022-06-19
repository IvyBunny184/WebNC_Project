namespace WebNC_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            Bookings = new HashSet<Booking>();
            Birth = new DateTime(2000, 1, 1);
        }

        [MaxLength(20, ErrorMessage = "Username is contains at most 20 characters")]
        [MinLength(6, ErrorMessage = "Username is contains at least 6 characters")]
        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} is required")]
        public string ID { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "{0} is too long")]
        [Display(Name = "Họ tên")]
        public string Name { get; set; }

        [Column(TypeName = "datetime2")]
        [Display(Name = "Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Birth { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(10)]
        [Display(Name = "SĐT")]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Giới tính")]
        public bool Gender { get; set; }

        [MaxLength(20, ErrorMessage = "{0} contains at most 20 characters")]
        [MinLength(6, ErrorMessage = "{0} contains at least 6 characters")]
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }

        [NotMapped]
        public string IDWithName { get { return Name + " : " + ID; } }
    }
}
