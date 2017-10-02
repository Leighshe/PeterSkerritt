using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PeterSkerrittELearning.Entities
{
    public class Material
    {
        public Material()
        {
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public byte[] Data { get; set; }
        public bool Active { get; set; }
        public int CertificationId { get; set; }

        [ForeignKey("CertificationId")]
        public virtual Certification Certification { get; set; }
    }
}