namespace WebNC_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RoomType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoomType()
        {
            Rooms = new HashSet<Room>();
        }

        [Required(ErrorMessage = "{0} is required")]
        [MaxLength(10,ErrorMessage = "{0} contains at most 10 characters")]
        [MinLength(4, ErrorMessage = "{0} contains at least 4 characters")]
        [Display(Name = "Mã loại phòng")]
        public string ID { get; set; }

        [StringLength(30,ErrorMessage = "{0} contains at most 30 characters")]
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Tên loại phòng")]
        public string NameType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
