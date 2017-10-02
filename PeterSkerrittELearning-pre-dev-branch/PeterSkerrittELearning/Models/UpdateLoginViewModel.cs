using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PeterSkerrittELearning.Models
{
    public class UpdateLoginViewModel
    {
        public string Id { get; set; }

        [Required]
        
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [RegularExpression("([a-zA-Z]+(-)?(')?(\\s)?){1,3}", ErrorMessage = "Enter only your first name .Only alphabetical characters allowed!")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("([a-zA-Z]+(-)?(')?(\\s)?){1,5}", ErrorMessage = "Enter only your last name .Only alphabetical characters allowed!")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} is required")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Phone]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Ensure that your phone number is 10 digits long")]
        [Display(Name = "Number")]
        public string Number { get; set; }

        public string OldPassword { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        


    }
}