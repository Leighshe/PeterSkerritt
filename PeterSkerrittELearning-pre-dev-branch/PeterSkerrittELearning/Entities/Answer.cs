using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeterSkerrittELearning.Entities
{
    public class Answer
    {
        public Answer()
        {
        }

        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public bool Correct { get; set; }
        public int QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }
    }
}