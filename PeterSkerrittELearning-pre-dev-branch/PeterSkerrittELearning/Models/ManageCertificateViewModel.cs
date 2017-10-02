using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PeterSkerrittELearning.Entities;

namespace PeterSkerrittELearning.Models
{
    public class ManageCertificateViewModel
    {
        public ManageCertificateViewModel()
        {
            TestAndQuestions = new List<TestAndQuestion>();
            AllQuestions = new List<Question>();
            Material = new List<Material>();
        }
        
        public List<Certification> Certificates { get; set; }
        public List<Assessment> Assessments { get; set; }
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
        public List<Module> Modules { get; set; }
        public List<Section> Sections { get; set; }
        public WizardIds wizardIds { get; set; }
        public List<TestAndQuestion> TestAndQuestions { get; set; }
        public List<Question> AllQuestions { get; set; }
        public List<Material> Material { get; set; }
        public string FileExtension { get; set; }
    }

    public class WizardIds
    {
        public int CertificationId { get; set; }
        public int AssessmentId { get; set; }
        public int ModuleId { get; set; }
        public int SectioId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
    }

    public class TestAndQuestion
    {
        public Module Test { get; set; }
        public List<Question> Questions { get; set; }
    }
}