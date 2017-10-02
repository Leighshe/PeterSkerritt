using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PeterSkerrittELearning.Entities
{
    public class Module
    {
        public Module()
        {
            //Assessment = new List<Assessments>();
            Sections = new List<Section>();
            DateCreated = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public int DefaultOptionCount { get; set; }
        public int QuestionCount { get; set; }
        public decimal Duration { get; set; }
        public int QuestionsPerTab { get; set; }
        public decimal MinPassScore { get; set; }
        public decimal MaximumScore { get; set; }
        public string State { get; set; }
        public string Reporting { get; set; }
        public bool Active { get; set; }
        public int AssessmentId { get; set; }

        [ForeignKey("AssessmentId")]
        public virtual Assessment Assessment { get; set; }

        public virtual ICollection<Section> Sections { get; set; }
    }
}