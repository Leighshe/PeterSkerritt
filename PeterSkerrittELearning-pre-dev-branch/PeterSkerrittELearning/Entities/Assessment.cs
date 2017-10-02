using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PeterSkerrittELearning.Entities
{
    public class Assessment
    {
        public Assessment()
        {
            Modules = new List<Module>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public int CertificationId { get; set; }

        [ForeignKey("CertificationId")]
        public virtual Certification Certification { set; get; }

        public virtual ICollection<Module> Modules { get; set; }
    }
}