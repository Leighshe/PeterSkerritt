using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PeterSkerrittELearning.Models;
using PeterSkerrittELearning.Entities;
using PeterSkerrittELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PeterSkerrittELearning.Controllers
{
    public class AdminMenuController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AdminMenuController()
        {

        }

        public AdminMenuController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [Authorize(Roles = "Administrator")]
        // GET: UpdateLogin
        public async Task<ActionResult> UpdateLoginView()
        {
            UpdateLoginViewModel model = new UpdateLoginViewModel();
            var user = await UserManager.FindByNameAsync(User.Identity.Name);

            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                var admin = data.AdminUsers.FirstOrDefault(a => a.ApplicationUserId == user.Id);

                model.FullName = admin.FirstName + " " + admin.LastName;
                model.FirstName = admin.FirstName;
                model.LastName = admin.LastName;
                model.Email = user.Email;
                model.Username = user.UserName;
                model.Number = admin.PhoneNumber;
              
            }

            return View("UpdateLogin", model);
        }

        [Authorize(Roles = "Administrator")]
        // GET: UpdateLogin
        public async Task<ActionResult> UpdateLogin(UpdateLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(User.Identity.Name);

                //var newFullName = model.FullName;
                var newFirstName = model.FirstName;
                var newLastName = model.LastName;
                var oldPassword = model.OldPassword;
                var newPassword = model.Password;
                var newUsername = model.Username;
                var newNumber = model.Number;
                
                using (ApplicationDbContext data = new ApplicationDbContext())
                {
                    var admin = data.AdminUsers.FirstOrDefault(a => a.ApplicationUserId == user.Id);

                    if(admin.Username == newUsername && admin.PhoneNumber == newNumber && newPassword == null && admin.FirstName == newFirstName && admin.LastName == newLastName)
                    {
                        if(oldPassword != null)
                        {
                            ModelState.AddModelError("", "You have supplied an old password without specifying a new password");
                            return View(model);
                        }
                        ModelState.AddModelError("", "You haven't made any changes to any form fields");
                        return View(model);
                    }

                    if(newFirstName != null)
                    {
                        admin.FirstName = newFirstName;
                        data.Entry(admin).Property(m => m.FirstName).IsModified = true;
                    }

                    if(newLastName != null)
                    {
                        admin.LastName = newLastName;
                        data.Entry(admin).Property(m => m.LastName).IsModified = true;
                    }

                    if (newNumber != null)
                    {
                        admin.PhoneNumber = newNumber;
                        data.Entry(admin).Property(m => m.PhoneNumber).IsModified = true;
                    }

                    if (newUsername != null)
                    {
                        user.UserName = newUsername;
                        var updateResult = await UserManager.UpdateAsync(user);
                        
                        if (!updateResult.Succeeded)
                        {
                            AddErrors(updateResult);
                            return View(model);
                        }
                        else
                        {
                            admin.Username = model.Username;
                            data.Entry(admin).Property(m => m.Username).IsModified = true;
                            
                        }

                    }

                    if (oldPassword != null)
                    {
                        var passwordCheck = await UserManager.CheckPasswordAsync(user, oldPassword);
                       
                        if (passwordCheck == true)
                        {
                           
                            if(newPassword != null)
                            {
                                if(newPassword != oldPassword)
                                {
                                    var passwordResult = await UserManager.ChangePasswordAsync(user.Id, oldPassword, newPassword);

                                    if (!passwordResult.Succeeded)
                                    {
                                        AddErrors(passwordResult);
                                        return View(model);
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Your new password is the same as your old password. Please specify a different password");
                                    return View(model);
                                }

                                
                            }
                            else
                            {
                                ModelState.AddModelError("", "Your new password cannot be blank. Enter a password");
                                return View(model);
                            }
                            
                        }
                        else
                        {
                            ModelState.AddModelError("", "The Old Password you entered is incorrect");
                            return View(model);
                        }
                    }
                    else if (oldPassword == null && newPassword != null)
                    {
                        ModelState.AddModelError("", "You have entered a new password without supplying your old password");
                        return View(model);
                    }

                    data.SaveChanges();

                }

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");

            }
           
            // If we got this far, something failed, redisplay form
            return View(model);
            
        }

        [Authorize(Roles = "Administrator")]
        // GET: AddUser
        public ActionResult AddUser()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        // GET: AddUser
        public async Task<ActionResult> EditUserView(string Id)
        {
            UpdateLoginViewModel model = new UpdateLoginViewModel();
            var user = await UserManager.FindByIdAsync(Id);

            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                var admin = data.AdminUsers.FirstOrDefault(a => a.ApplicationUserId == user.Id);

                model.Id = Id;
                model.FullName = admin.FirstName + " " + admin.LastName;
                model.FirstName = admin.FirstName;
                model.LastName = admin.LastName;
                model.Email = user.Email;
                model.Username = user.UserName;
                model.Number = admin.PhoneNumber;

            }

            return View("EditUser", model);
        }

        [Authorize(Roles = "Administrator")]
        // GET: AddUser
        public async Task<ActionResult> EditUser(UpdateLoginViewModel model ,string Id)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(Id);

                string newFullName = model.FullName;

                string[] firstAndLastName = newFullName.Split(' ');

                var newFirstName = model.FirstName;
                var newLastName = model.LastName;
                var oldPassword = model.OldPassword;
                var newPassword = model.Password;
                var newUsername = model.Username;
                var newNumber = model.Number;

                using (ApplicationDbContext data = new ApplicationDbContext())
                {
                    var admin = data.AdminUsers.FirstOrDefault(a => a.ApplicationUserId == user.Id);

                    if (admin.Username == newUsername && admin.PhoneNumber == newNumber && newPassword == null && admin.FirstName == newFirstName && admin.LastName == newLastName)
                    {
                        if (oldPassword != null)
                        {
                            ModelState.AddModelError("", "You have supplied an old password without specifying a new password");
                            return View(model);
                        }
                        ModelState.AddModelError("", "You haven't made any changes to any form fields");
                        return View(model);
                    }

                    if(newFirstName != null && newLastName != null)
                    {
                        admin.FirstName = newFirstName;
                        admin.LastName = newLastName;

                        data.Entry(admin).Property(m => m.FirstName).IsModified = true;
                        data.Entry(admin).Property(m => m.LastName).IsModified = true;
                    }

                    if (newNumber != null)
                    {
                        admin.PhoneNumber = newNumber;
                    }

                    if (newUsername != null)
                    {
                        user.UserName = newUsername;
                        var updateResult = await UserManager.UpdateAsync(user);

                        if (!updateResult.Succeeded)
                        {
                            AddErrors(updateResult);
                            return View(model);
                        }
                        else
                        {
                            admin.Username = model.Username;

                            data.Entry(admin).Property(m => m.Username).IsModified = true;
                            
                        }

                    }
                    

                    if (oldPassword != null)
                    {
                        var passwordCheck = await UserManager.CheckPasswordAsync(user, oldPassword);

                        if (passwordCheck == true)
                        {

                            if (newPassword != null)
                            {
                                if (newPassword != oldPassword)
                                {
                                    var passwordResult = await UserManager.ChangePasswordAsync(user.Id, oldPassword, newPassword);

                                    if (!passwordResult.Succeeded)
                                    {
                                        AddErrors(passwordResult);
                                        return View(model);
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Your new password is the same as your old password. Please specify a different password");
                                    return View(model);
                                }


                            }
                            else
                            {
                                ModelState.AddModelError("", "Your new password cannot be blank. Enter a password");
                                return View(model);
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("", "The Old Password you entered is incorrect");
                            return View(model);
                        }
                    }
                    else if (oldPassword == null && newPassword != null)
                    {
                        ModelState.AddModelError("", "You have entered a new password without supplying your old password");
                        return View(model);
                    }

                    //Save changes made to the AdminUsers table
                    data.SaveChanges();

                }
                
                List<AdminUser> Users = new List<AdminUser>();

                using (ApplicationDbContext data = new ApplicationDbContext())
                {
                    Users = data.AdminUsers.Include("ApplicationUser").ToList();
                }

                return View("ManageUsers", Users);

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        // GET: AddUser
        public async Task<ActionResult> EditStudentView(string Id)
        {
            EditStudentViewModel model = new EditStudentViewModel();

            var studentUser = await UserManager.FindByIdAsync(Id);

            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                Student student = data.Students.FirstOrDefault(f => f.ApplicationUserId == studentUser.Id);

                model.Id = Id;
                model.Username = studentUser.UserName;
                model.Email = studentUser.Email;
                model.FirstName = student.FirstName;
                model.LastName = student.LastName;
                model.Company = student.Company;
                model.PhoneNumber = student.PhoneNumber;
                model.ReportGroup = student.ReportGroup;
                model.StudentCategory = student.StudentCategory;
                model.Comments = student.Comments;
            }
            
            return View("EditStudent", model);
        }

        [Authorize(Roles = "Administrator")]
        // GET: AddUser
        public async Task<ActionResult> EditStudent(EditStudentViewModel model, string Id)
        {
            if (ModelState.IsValid)
            {
                var studentUser = await UserManager.FindByIdAsync(Id);

                var newUsername = model.Username;
                var newFirstName = model.FirstName;
                var newLastName = model.LastName;
                var oldPassword = model.OldPassword;
                var newPassword = model.Password;
                var newCompany = model.Company;
                var newPhoneNumber = model.PhoneNumber;
                var newReportGroup = model.ReportGroup;
                var newStudentCategory = model.StudentCategory;
                var newComments = model.Comments;

                using (ApplicationDbContext data = new ApplicationDbContext())
                {
                    var student = data.Students.FirstOrDefault(s => s.ApplicationUserId == studentUser.Id);

                    if(student.Username == newUsername && student.FirstName == newFirstName && student.LastName == newLastName && student.Company == newCompany && student.PhoneNumber == newPhoneNumber && student.ReportGroup == newReportGroup && student.StudentCategory == newStudentCategory && student.Comments == newComments && newPassword == null)
                    {
                        if (oldPassword != null)
                        {
                            ModelState.AddModelError("", "You have supplied an old password without specifying a new password");
                            return View(model);
                        }

                        ModelState.AddModelError("", "You have not made any changes to any of the form fields.");
                        return View(model);
                    }

                    if(newFirstName != null)
                    {
                        student.FirstName = newFirstName;
                        data.Entry(student).Property(s => s.FirstName).IsModified = true;
                    }

                    if(newLastName != null)
                    {
                        student.LastName = newLastName;
                        data.Entry(student).Property(s => s.LastName).IsModified = true;
                    }

                    if (newCompany != null)
                    {
                        student.Company = newCompany;
                        data.Entry(student).Property(s => s.Company).IsModified = true;
                    }

                    if (newPhoneNumber != null)
                    {
                        student.PhoneNumber = newPhoneNumber;
                        data.Entry(student).Property(s => s.PhoneNumber).IsModified = true;
                    }

                    if (newReportGroup != null)
                    {
                        student.ReportGroup = newReportGroup;
                        data.Entry(student).Property(s => s.ReportGroup).IsModified = true;
                    }

                    if (newStudentCategory != null)
                    {
                        student.StudentCategory = newStudentCategory;
                        data.Entry(student).Property(s => s.StudentCategory).IsModified = true;
                    }

                    if (newComments != null)
                    {
                        student.Comments = newComments;
                        data.Entry(student).Property(s => s.Comments).IsModified = true;
                    }

                    if (newUsername != null)
                    {
                        studentUser.UserName = newUsername;
                        var updateResult = await UserManager.UpdateAsync(studentUser);

                        if (!updateResult.Succeeded)
                        {
                            AddErrors(updateResult);
                            return View(model);
                        }
                        else
                        {
                            student.Username = model.Username;
                            data.Entry(student).Property(m => m.Username).IsModified = true;
                        }

                    }

                    if (oldPassword != null)
                    {
                        var passwordCheck = await UserManager.CheckPasswordAsync(studentUser, oldPassword);

                        if (passwordCheck == true)
                        {

                            if (newPassword != null)
                            {
                                if (newPassword != oldPassword)
                                {
                                    var passwordResult = await UserManager.ChangePasswordAsync(studentUser.Id, oldPassword, newPassword);

                                    if (!passwordResult.Succeeded)
                                    {
                                        AddErrors(passwordResult);
                                        return View(model);
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("", "The new password is the same as the old password. Please specify a different password");
                                    return View(model);
                                }
                                
                            }
                            else
                            {
                                ModelState.AddModelError("", "The new password cannot be blank. Enter a password");
                                return View(model);
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("", "The Old Password you entered is incorrect");
                            return View(model);
                        }
                    }
                    else if (oldPassword == null && newPassword != null)
                    {
                        ModelState.AddModelError("", "You have entered a new password without supplying the old password");
                        return View(model);
                    }

                    //Save changes made to the AdminUsers table
                    data.SaveChanges();

                }

                List<Student> Users = new List<Student>();

                using (ApplicationDbContext data = new ApplicationDbContext())
                {
                    Users = data.Students.Include("ApplicationUser").ToList();
                }

                return View("ManageStudents", Users);

            }

            //If we got this far, something failed
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        // GET: ManageUsers
        public ActionResult ManageUsers()
        {
            List<AdminUser> Users = new List<AdminUser>();

            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                Users = data.AdminUsers.Include("ApplicationUser").ToList();
            }

            return View(Users);
        }

        [Authorize(Roles = "Administrator")]
        // GET: ManageUsers
        public ActionResult ManageStudents()
        {
            List<Student> Users = new List<Student>();

            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                Users = data.Students.Include("ApplicationUser").ToList();
            }

            return View(Users);
        }

        [Authorize(Roles = "Administrator")]
        // GET: ManageUsers
        public ActionResult _Users()
        {
            List<AdminUser> Users = new List<AdminUser>();

            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                Users = data.AdminUsers.Include("ApplicationUser").ToList();
            }

            return PartialView(Users);
        }

        [Authorize(Roles = "Administrator")]
        // GET: ManageUsers
        public ActionResult _Students()
        {
            List<Student> Users = new List<Student>();

            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                Users = data.Students.Include("ApplicationUser").ToList();
            }

            return PartialView(Users);
        }

        [Authorize(Roles = "Administrator")]
        // POST: RemoveUser
        public ActionResult RemoveUser(string Id)
        {
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                AdminUser User = data.AdminUsers.Include("ApplicationUser").FirstOrDefault(f => f.ApplicationUserId == Id);

                if (User.Active == true)
                {
                    User.Active = false;
                    User.ApplicationUser.LockoutEndDateUtc = DateTime.Now.AddYears(1000);
                }
                else
                {
                    User.Active = true;
                    User.ApplicationUser.LockoutEndDateUtc = DateTime.Now.AddYears(-1);
                }

                data.SaveChanges();
            }

            return null;
        }

        [Authorize(Roles = "Administrator")]
        // POST: RemoveUser
        public ActionResult RemoveStudent(string Id)
        {
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                Student User = data.Students.Include("ApplicationUser").FirstOrDefault(f => f.ApplicationUserId == Id);

                if (User.Active == true)
                {
                    User.Active = false;
                    User.ApplicationUser.LockoutEndDateUtc = DateTime.Now.AddYears(1000);
                }
                else
                {
                    User.Active = true;
                    User.ApplicationUser.LockoutEndDateUtc = DateTime.Now.AddYears(-1);
                }

                data.SaveChanges();
            }

            return null;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}