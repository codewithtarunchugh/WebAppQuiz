using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAppQuiz.ViewModel
{
    public class QuizCategoryViewModel
    {
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }
        [Display(Name = "Candidate")]
        [Required(ErrorMessage = "Candidate is required.")]
        public string CandidateName { get; set; }
        public IEnumerable<SelectListItem> ListOfCategory { get; set; }
    }
}