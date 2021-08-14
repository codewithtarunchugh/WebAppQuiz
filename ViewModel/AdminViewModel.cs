using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebAppQuiz.ViewModel
{
    public class AdminViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter valid email.")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Passowrd is required")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
    }
}