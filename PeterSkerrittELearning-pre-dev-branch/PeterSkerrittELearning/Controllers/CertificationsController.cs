using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PeterSkerrittELearning.Models;
using System.Data.Entity;
using PeterSkerrittELearning.Entities;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using HttpPostedFileHelper;

namespace PeterSkerrittELearning.Controllers
{
    public class CertificationsController : Controller
    {
       

        WizardIds wid = new WizardIds();
        [Authorize(Roles = "Administrator")]
        // GET: AddCertification
        public ActionResult AddCertification()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        // GET: ManageCertifications
        public ActionResult ManageCertifications()
        {

            ManageCertificateViewModel VM = new ManageCertificateViewModel();

            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                VM.Certificates = data.Certifications.ToList();
                VM.Assessments = data.Assessments.ToList();
                VM.Modules = data.Modules.ToList();
                VM.Sections = data.Sections.ToList();
                VM.Questions = data.Questions.ToList();
                VM.Answers = data.Answers.ToList();
                VM.wizardIds = wid;
                
            }


            return View(VM);
        }

        public static void GetMediaFile(HttpPostedFileBase file)
        {
            //file = Request.Files[0];
        }
          
        [HttpPost]
        public ActionResult CopyMediaToServer(HttpPostedFileBase file, int CertificationId)
        {
            FileHelper fHelper = new FileHelper();

            if (file != null)
            {
                var videoDirectory = System.Web.HttpContext.Current.Server.MapPath("~/Media");
                var savePath = Path.Combine(videoDirectory, file.FileName);

                //file.SaveAs(savePath);

                Material newMaterial = new Material()
                {
                    Name = file.FileName,
                    Active = true,
                    CertificationId = CertificationId,
                    Data = null,
                    Path = savePath
                };
                
                using (ApplicationDbContext data = new ApplicationDbContext())
                {
                    //await CopyMediaToServer(file, CertificationId);
                    fHelper.FilePath ="~/Media";
                    fHelper.ProcessFile(file);
                    
                    data.Material.Add(newMaterial);
                    data.SaveChanges();
                }
                return View();
            }

            return View();
            
        }

        public ActionResult UploadMaterial(int CertificationId)
        {
            var len = Request.Files.Count;
            HttpPostedFileBase file = null;
            if(len != 0)
            {
                file = Request.Files[0];
            }
            else
            {
                return View();
            }
            
            //Thread fileThread = new Thread(new ThreadStart(GetMediaFile(file)));
            
            if (file != null)
            {
                byte[] buffer = null;
                if (file != null)
                {
                    string extension = Path.GetExtension(file.FileName);

                    if (extension == ".wmv" || extension == ".mp4" || extension == ".mp3" || extension == ".mkv" || extension == ".3gp" || extension == ".flv")
                    {
                        
                        CopyMediaToServer(file, CertificationId);
                    }
                    else
                    {
                        buffer = new byte[file.InputStream.Length];
                        file.InputStream.Seek(0, SeekOrigin.Begin);
                        file.InputStream.Read(buffer, 0, buffer.Length);

                        Material newMaterial = new Material()
                        {
                            Name = file.FileName,
                            Active = true,
                            CertificationId = CertificationId,
                            Data = buffer
                        };

                        using (ApplicationDbContext data = new ApplicationDbContext())
                        {
                            data.Material.Add(newMaterial);
                            data.SaveChanges();
                        }
                    }

                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult StreamMedia(int Id)
        {
            MediaViewModel model = new MediaViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                string fileName = data.Material.FirstOrDefault(f => f.Id == Id).Name;
                string filePath = data.Material.FirstOrDefault(f => f.Id == Id).Path;
                string extension = Path.GetExtension(fileName);
                
                model.Id = Id;
                model.Extension = extension.Substring(1,3);

                switch(extension)
                {
                    case ".mp4":
                    case ".3gp":
                    case ".flv":
                    case ".mkv":
                        model.MediaType = "video";
                        break;
                       
                    default:
                        model.MediaType = "audio";
                        break;
                }

                model.Path = filePath;
                model.Name = fileName;
                
                return View("VideoStreaming", model);
                    
              
            }

        }

        public FileResult Download(int Id)
        {
            using (ApplicationDbContext data = new ApplicationDbContext())
            {

                
                string fileName = data.Material.FirstOrDefault(f => f.Id == Id).Name;
                string filePath = data.Material.FirstOrDefault(f => f.Id == Id).Path;
                string extension = Path.GetExtension(fileName);

                if(extension == ".wmv" || extension == ".mp4" || extension == ".mp3" || extension == ".wav" || extension == ".flv" || extension == ".mkv")
                {
                    if(extension == ".wmv")
                    {
                        Response.ContentType = "video/x-ms-wmv";
                    }
                    else if(extension == ".mp4")
                    {
                        Response.ContentType = "video/mp4";
                    }
                    else if (extension == ".flv")
                    {
                        Response.ContentType = "video/x-flv";
                    }
                    else if (extension == ".mp3")
                    {
                        Response.ContentType = "audio/mpeg";
                    }
                    else if(extension == ".mkv")
                    {
                        Response.ContentType = "video/divx";
                    }

                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.TransmitFile(filePath);
                    Response.End();
                    return null;
                    //byte[] fileBytes = System.IO.File.ReadAllBytes(data.Material.FirstOrDefault(f => f.Id == Id).Path);
                    //return File(fileBytes, MimeMapping.GetMimeMapping(fileName), fileName);
                }
                else
                {
                    byte[] fileBytes = data.Material.FirstOrDefault(f => f.Id == Id).Data;
                    return File(fileBytes, MimeMapping.GetMimeMapping(fileName), fileName);
                }
                
                
            }
        }

        public ActionResult _Certificate()
        {
            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                VM.Certificates = data.Certifications.ToList();
                VM.wizardIds = wid;
            }

            return PartialView(VM);
        }

        public ActionResult _Assessment(int Id)
        {
            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                VM.Assessments = data.Assessments.Where(X => X.CertificationId == Id).ToList();
                VM.Material = data.Material.Where(w => w.CertificationId == Id && w.Active == true).ToList();

                wid.CertificationId = Id;

                VM.wizardIds = wid;
            }

            return PartialView(VM);
        }

        public ActionResult _Question(int Id)
        {
            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                VM.Questions = data.Questions.Where(x => x.SectionId == Id).ToList();
                wid.SectioId = Id;
                VM.wizardIds = wid;

                int CertId = data.Sections.FirstOrDefault(f => f.Id == Id).Module.Assessment.CertificationId;

                data.Modules.Include("Sections.Questions").Where(w => w.Assessment.CertificationId == CertId).ToList().ForEach(f =>
                {
                    List<Question> questions = new List<Question>();

                    f.Sections.ToList().ForEach(ff =>
                    {
                        questions.AddRange(ff.Questions);
                    });

                    if (questions.Count() > 0)
                    {
                        TestAndQuestion TQ = new TestAndQuestion() { Test = f, Questions = questions };

                        VM.TestAndQuestions.Add(TQ);
                    }
                });
            }

            return PartialView(VM);
        }

        public ActionResult _Answer(int Id)
        {
            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                int TestId = data.Questions.FirstOrDefault(f => f.Id == Id).Section.Module.Id;

                VM.Answers = data.Answers.Where(x => x.QuestionId == Id).ToList();
                wid.QuestionId = Id;
                VM.wizardIds = wid;
                VM.AllQuestions = data.Questions.Include("Answers").Where(w => w.Section.Module.Id == TestId).ToList();
            }

            return PartialView(VM);
        }

        public ActionResult _Module(int Id)
        {
            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                VM.Modules = data.Modules.Where(X => X.AssessmentId == Id).ToList();
                wid.AssessmentId = Id;
                VM.wizardIds = wid;
            }

            return PartialView(VM);
        }

        public ActionResult _Section(int Id)
        {
            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                VM.Sections = data.Sections.Where(X => X.ModuleId == Id).ToList();
                wid.ModuleId = Id;
                VM.wizardIds = wid;
            }

            return PartialView(VM);
        }


        public ActionResult LoadAssessmentByCertification(int Id)
        {
            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                VM.Assessments = data.Assessments.Where(X => X.CertificationId == Id).ToList();
            }

            wid.CertificationId = Id;

            VM.wizardIds = wid;

            return null;
        }

        public ActionResult LoadModulesByAssessment(int Id)
        {
            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                VM.Modules = data.Modules.Where(X => X.AssessmentId == Id).ToList();

                wid.AssessmentId = Id;
                VM.wizardIds = wid;
            }

            return null;
        }

        public ActionResult LoadAnswersByQuestion(int Id)
        {
            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                VM.Answers = data.Answers.Where(X => X.QuestionId == Id).ToList();

                wid.AnswerId = Id;
                VM.wizardIds = wid;
            }

            return null;
        }

        public ActionResult LoadSectionsByModules(int Id)
        {
            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                VM.Sections = data.Sections.Where(X => X.ModuleId == Id).ToList();
                wid.ModuleId = Id;
                VM.wizardIds = wid;
            }

            return null;
        }

        public ActionResult LoadQuestionBySections(int Id)
        {
            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                VM.Questions = data.Questions.Where(X => X.SectionId == Id).ToList();
                wid.SectioId = Id;
                VM.wizardIds = wid;
            }

            return null;
        }



        public ActionResult SaveCertificate(Entities.Certification obj)
        {
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                if (obj.Id == 0)
                {
                    data.Certifications.Add(obj);
                }
                else
                {
                    Certification cert = data.Certifications.FirstOrDefault(f => f.Id == obj.Id);
                    cert.Name = obj.Name;
                    cert.Description = obj.Description;
                }

                data.SaveChanges();
            }

            return null;
        }

        public ActionResult SaveAssessment(Entities.Assessment obj)
        {
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                if (obj.Id == 0)
                {
                    data.Assessments.Add(obj);
                }
                else
                {
                    Assessment asses = data.Assessments.FirstOrDefault(f => f.Id == obj.Id);
                    asses.Name = obj.Name;
                    asses.Description = obj.Description;
                }
                
                data.SaveChanges();
            }

            return null;
        }

        public ActionResult SaveModule(Entities.Module obj)
        {
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                if (obj.Id == 0)
                {
                    data.Modules.Add(obj);
                }
                else
                {
                    Module mod = data.Modules.FirstOrDefault(f => f.Id == obj.Id);
                    mod.MaximumScore = obj.MaximumScore;
                    mod.MinPassScore = obj.MinPassScore;
                    mod.Name = obj.Name;
                    mod.QuestionCount = obj.QuestionCount;
                    mod.Duration = obj.Duration;
                    mod.Description = obj.Description; 
                }
                
                data.SaveChanges();
            }

            return null;
        }

        public ActionResult SaveSection(Entities.Section obj)
        {
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                if (obj.Id == 0)
                {
                    data.Sections.Add(obj);
                }
                else
                {
                    Section sect = data.Sections.FirstOrDefault(f => f.Id == obj.Id);

                    sect.Description = obj.Description;
                    sect.Name = obj.Name;
                    sect.MinPassScore = obj.MinPassScore;
                    sect.MaximumScore = obj.MaximumScore;
                }

                data.SaveChanges();
            }

            return null;
        }


        public ActionResult SaveQuestion(Entities.Question obj)
        {
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                if (obj.Id == 0)
                {
                    data.Questions.Add(obj);
                }
                else
                {
                    Question que = data.Questions.FirstOrDefault(f => f.Id == obj.Id);

                    que.Name = obj.Name;
                    que.Score = obj.Score;
                    que.Description = obj.Description;
                }
                
                data.SaveChanges();

            }

            return null;
        }

        public ActionResult AddExistingAnswers(int[] AnswerIds, int CurrentQuestion)
        {
            AnswerIds.ToList().ForEach(f =>
            {
                AddExistingAnswer(CurrentQuestion, f);
            });

            return null;
        }

        public void AddExistingAnswer(int CurrentQuestionId, int ExistingAnswerId)
        {
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                Answer answer = data.Answers.FirstOrDefault(f => f.id == ExistingAnswerId);
                Answer newAnswer = new Answer() { Name = answer.Name, Description = answer.Description, QuestionId = CurrentQuestionId };
                data.Answers.Add(newAnswer);
                data.SaveChanges();
            }
        }

        public ActionResult AddExistingQuestions(int[] QuestionIds, int CurrentSection)
        {
            QuestionIds.ToList().ForEach(f =>
            {
                AddExistingQuestion(CurrentSection, f);
            });

            return null;
        }

        public void AddExistingQuestion(int CurrentSectionId, int ExistingQuestionId)
        {
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                Question question = data.Questions.FirstOrDefault(f => f.Id == ExistingQuestionId);
                Question newQuestion = new Question() { Name = question.Name, Description = question.Description, Score = question.Score, SectionId = CurrentSectionId };
                data.Questions.Add(newQuestion);
                data.SaveChanges();

                question.Answers.ToList().ForEach(f =>
                {
                    Answer newAnswer = new Answer() { Name = f.Name, Description = f.Description, Correct = f.Correct, QuestionId = newQuestion.Id };
                    data.Answers.Add(newAnswer);
                    data.SaveChanges();
                });
            }
        }

        public ActionResult SaveAnswer(Entities.Answer obj)
        {
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                if (obj.id == 0)
                {
                    data.Answers.Add(obj);
                }
                else
                {
                    Answer answ = data.Answers.FirstOrDefault(f => f.id == obj.id);

                    answ.Name = obj.Name;
                    answ.Correct = obj.Correct;
                    answ.Description = obj.Description;
                }

                data.SaveChanges();
            }

            return null;
        }

        public ActionResult DeleteCertificate(int Id)
        {

            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                var results = data.Certifications.FirstOrDefault(X => X.Id == Id);
                if (results != null)
                {
                    data.Certifications.Remove(results);
                    data.SaveChanges();
                }


                VM.Certificates = data.Certifications.ToList();
                VM.wizardIds = wid;
            }

            return PartialView("_Certificate", VM);
        }


        public ActionResult DeleteAssessment(int Id)
        {

            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                var results = data.Assessments.FirstOrDefault(X => X.Id == Id);
                if (results != null)
                {
                    data.Assessments.Remove(results);
                    data.SaveChanges();
                }


                VM.Assessments = data.Assessments.ToList();
                VM.wizardIds = wid;
            }

            return PartialView("_Assessment", VM);
        }

        public ActionResult DeleteModule(int Id)
        {

            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                var results = data.Modules.FirstOrDefault(X => X.Id == Id);
                if (results != null)
                {
                    data.Modules.Remove(results);
                    data.SaveChanges();
                }


                VM.Modules = data.Modules.ToList();
                VM.wizardIds = wid;
            }

            return PartialView("_Module", VM);
        }

        public ActionResult DeleteSection(int Id)
        {

            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                var results = data.Sections.FirstOrDefault(X => X.Id == Id);
                if (results != null)
                {
                    data.Sections.Remove(results);
                    data.SaveChanges();
                }


                VM.Sections = data.Sections.ToList();
                VM.wizardIds = wid;
            }

            return PartialView("_Section", VM);
        }

        public ActionResult DeleteQuestion(int Id)
        {

            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                var results = data.Questions.FirstOrDefault(X => X.Id == Id);
                if (results != null)
                {
                    data.Questions.Remove(results);
                    data.SaveChanges();
                }


                VM.Questions = data.Questions.ToList();
                VM.wizardIds = wid;

            }

            return PartialView("_Question", VM);
        }

        public class CertificationVM
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public string GetCertification(int Id)
        {
            Certification results = new Certification();
            CertificationVM returnResults = new CertificationVM();
            string json;

            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                results = data.Certifications.FirstOrDefault(X => X.Id == Id);
            }

            returnResults.Name = results.Name;
            returnResults.Description = results.Description;

            json = JsonConvert.SerializeObject(returnResults);

            return json;
        }

        public class AssessmentVM
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public string GetAssessment(int Id)
        {
            Assessment results = new Assessment();
            AssessmentVM returnResults = new AssessmentVM();
            string json;

            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                results = data.Assessments.FirstOrDefault(X => X.Id == Id);
            }

            returnResults.Name = results.Name;
            returnResults.Description = results.Description;

            json = JsonConvert.SerializeObject(returnResults);

            return json;
        }

        public class ModuleVM
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int QuestionCount { get; set; }
            public decimal Duration { get; set; }
            public decimal MinPassScore { get; set; }
            public decimal MaximumScore { get; set; }
        }

        public string GetModule(int Id)
        {
            Module results = new Module();
            ModuleVM returnResults = new ModuleVM();
            string json;

            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                results = data.Modules.FirstOrDefault(X => X.Id == Id);
            }

            returnResults.Name = results.Name;
            returnResults.Description = results.Description;
            returnResults.QuestionCount = results.QuestionCount;
            returnResults.Duration = results.Duration;
            returnResults.MinPassScore = results.MinPassScore;
            returnResults.MaximumScore = results.MaximumScore;

            json = JsonConvert.SerializeObject(returnResults);

            return json;
        }

        public class SectionVM
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public string GetSection(int Id)
        {
            Section results = new Section();
            SectionVM returnResults = new SectionVM();
            string json;

            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                results = data.Sections.FirstOrDefault(X => X.Id == Id);
            }

            returnResults.Name = results.Name;
            returnResults.Description = results.Description;

            json = JsonConvert.SerializeObject(returnResults);

            return json;
        }

        public class QuestionVM
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int Score { get; set; }
        }

        public string GetQuestion(int Id)
        {
            Question results = new Question();
            QuestionVM returnResults = new QuestionVM();
            string json;

            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                results = data.Questions.FirstOrDefault(X => X.Id == Id);
            }

            returnResults.Name = results.Name;
            returnResults.Description = results.Description;
            returnResults.Score = results.Score;

            json = JsonConvert.SerializeObject(returnResults);

            return json;
        }

        public class AnswerVM
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public bool Correct { get; set; }
        }

        public string GetAnswer(int Id)
        {
            Answer results = new Answer();
            AnswerVM returnResults = new AnswerVM();
            string json;

            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                results = data.Answers.FirstOrDefault(X => X.id == Id);
            }

            returnResults.Name = results.Name;
            returnResults.Description = results.Description;
            returnResults.Correct = results.Correct;

            json = JsonConvert.SerializeObject(returnResults);

            return json;
        }

        public ActionResult DeleteAnswer(int Id)
        {

            ManageCertificateViewModel VM = new ManageCertificateViewModel();
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                var results = data.Answers.FirstOrDefault(X => X.id == Id);
                if (results != null)
                {
                    data.Answers.Remove(results);
                    data.SaveChanges();
                }


                VM.Answers = data.Answers.ToList();
                VM.wizardIds = wid;
            }

            return PartialView("_Answer", VM);
        }

        //

        [Authorize(Roles = "Administrator")]
        // GET: DeactivatedCertifications
        public ActionResult DeactivatedCertifications()
        {
            return View();
        }

        public ActionResult RemoveMaterial(int Id)
        {
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                data.Material.FirstOrDefault(f => f.Id == Id).Active = false;

                data.SaveChanges();
            }

            return null;
        }

        
    }
}