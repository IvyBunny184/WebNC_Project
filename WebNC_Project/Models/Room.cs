namespace WebNC_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Room
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Room()
        {
            Bookings = new HashSet<Booking>();
            Images = new HashSet<Image>();
            SuppliesForRooms = new HashSet<SuppliesForRoom>();
        }

        [MaxLength(10,ErrorMessage = "{0} contains at most 10 characters")]
        [MinLength(4, ErrorMessage = "{0} contains at least 4 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        [Display(Name = "Mã phòng")]
        public string ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        [MaxLength(30, ErrorMessage = "{0} contains at most 30 characters")]
        [MinLength(6, ErrorMessage = "{0} contains at least 6 characters")]
        [Display(Name = "Tên phòng")]
        public string Name { get; set; }

        [MaxLength(10, ErrorMessage = "{0} contains at most 10 characters")]
        [MinLength(4, ErrorMessage = "{0} contains at least 4 characters")]
        [Display(Name = "Loại phòng")]
        [Required(ErrorMessage = "{0} is required")]
        public string TypeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        public string Status { get; set; }

        [Display(Name = "Số gười lớn")]
        [Required(ErrorMessage = "{0} is required")]
        [Range(1,10,ErrorMessage = "{0} is in range 1 to 10")]

        public int Adult { get; set; }

        [Display(Name = "Số trẻ em")]
        [Required(ErrorMessage = "{0} is required")]
        [Range(0, 10, ErrorMessage = "{0} is in range 1 to 10")]
        public int Child { get; set; }

        [Display(Name = "Giá thuê")]
        [Required(ErrorMessage = "{0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "{0} is at min 1")]
        public double Price { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Image> Images { get; set; }

        public virtual RoomType RoomType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SuppliesForRoom> SuppliesForRooms { get; set; }

        [NotMapped]
        public string Display
        {
            get
            {
                return $"{ID} : {Name}";
            }
        }
    }
}
