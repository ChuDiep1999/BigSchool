namespace BigSchool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        public int Id { get; set; }
        public string Name;
        [Required]
        [StringLength(128)]
        public string LecturedId { get; set; }

        [Required]
        [StringLength(255)]
        public string Place { get; set; }

        public DateTime DateTime { get; set; }

        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }
        public List<Category> listCategory = new List<Category>();
    }
}
