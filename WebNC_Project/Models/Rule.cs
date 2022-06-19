namespace WebNC_Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Rule
    {
        [Key]
        [StringLength(10)]
        public string RuleCode { get; set; }

        public string Description { get; set; }

        public int Condition { get; set; }
    }
}
