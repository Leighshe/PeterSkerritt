using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeterSkerrittELearning.Models
{
    public class MediaViewModel
    {
        public int Id { get; set; }
        public string MediaType { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
    }
}