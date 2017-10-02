using PeterSkerrittELearning.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PeterSkerrittELearning.Entities
{
    public class Student
    {
        public Student()
        {
            //Certifications = new List<Certifications>();
            DateCreated = DateTime.Now;
        }

        [Key]
        public string ApplicationUserId { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Comments { get; set; }
        public string ReportGroup { get; set; }
        public string StudentCategory { get; set; }
        public string StudentActivity { get; set; }
        public string StudentNumber { get; set; }

        public bool Active { get; set; }

        //public virtual ICollection<Certifications> Certifications { get; set; }
        //public virtual ICollection<Exams> Exams { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}