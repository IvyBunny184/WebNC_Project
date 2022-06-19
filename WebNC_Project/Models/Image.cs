namespace WebNC_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Image
    {
        [Key]
        [StringLength(450)]
        public string URL { get; set; }

        [StringLength(10)]
        public string RoomID { get; set; }

        public virtual Room Room { get; set; }

        public static string GetDefaultURL(List<Image> images)
        {
            if (images.Count > 0) return images[0].URL;
            else return "https://firebasestorage.googleapis.com/v0/b/imageresort-e3879.appspot.com/o/imgsroom%2FROOM001%2Froom-b1.jpg?alt=media&token=7b9aa685-304d-42fb-8810-410ac572f6b5";
        }
    }
}
