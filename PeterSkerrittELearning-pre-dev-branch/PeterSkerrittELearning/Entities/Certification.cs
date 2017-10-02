using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PeterSkerrittELearning.Entities
{
    public class Certification
    {
        public Certification()
        {
            Material = new List<Material>();
            Assessments = new List<Assessment>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Material> Material { get; set; }
        public virtual ICollection<Assessment> Assessments { get; set; }
    }
}