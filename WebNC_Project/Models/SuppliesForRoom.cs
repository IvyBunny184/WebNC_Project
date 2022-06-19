namespace WebNC_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SuppliesForRoom
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        [Required]
        [Display(Name = "Phòng")]
        public string RoomID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        [Required]
        [Display(Name = "Vật tư")]
        public string SupplyID { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Số lượng")]
        [Range(1,int.MaxValue,ErrorMessage = "Please enter number with min value equal 1")]
        public int Count { get; set; }

        public virtual Room Room { get; set; }

        public virtual Supply Supply { get; set; }
    }
}
