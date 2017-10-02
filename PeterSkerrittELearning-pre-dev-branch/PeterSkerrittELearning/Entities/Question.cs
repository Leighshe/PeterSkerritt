using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeterSkerrittELearning.Entities
{
    public class Question
    {
        public Question()
        {
            Answers = new List<Answer>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string Type { get; set; }
        public int Number { get; set; }
        public int OptionCount { get; set; }
        public int Score { get; set; }
        public int SectionId { get; set; }

        [ForeignKey("SectionId")]
        public virtual Section Section { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}