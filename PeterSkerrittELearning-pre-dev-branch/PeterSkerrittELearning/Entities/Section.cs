using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PeterSkerrittELearning.Entities
{
    public class Section
    {
        public Section()
        {
            Questions = new List<Question>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MinPassScore { get; set; }
        public decimal MaximumScore { get; set; }
        public int ModuleId { get; set; }

        [ForeignKey("ModuleId")]
        public virtual Module Module { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}