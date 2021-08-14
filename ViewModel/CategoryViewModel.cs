using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAppQuiz.ViewModel
{
    public class CategoryViewModel
    {
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Question is required.")]
        [Display(Name = "Question")]
        public string QuestionName { get; set; }

        public string OptionName { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<SelectListItem> ListofCategory { get; set; }
    }
}