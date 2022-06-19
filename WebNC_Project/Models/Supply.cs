namespace WebNC_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Supply
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Supply()
        {
            SuppliesForRooms = new HashSet<SuppliesForRoom>();
        }

        [MaxLength(10, ErrorMessage = "{0} contains at most 10 characters")]
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Mã vật tư")]
        public string ID { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(30, ErrorMessage = "{0} contains at most 30 characters")]
        [Display(Name = "Tên vật tư")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Range(1,int.MaxValue,ErrorMessage = "Please enter a valid number")]
        [Display(Name = "Số lượng")]
        public int Total { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SuppliesForRoom> SuppliesForRooms { get; set; }
    }
}
