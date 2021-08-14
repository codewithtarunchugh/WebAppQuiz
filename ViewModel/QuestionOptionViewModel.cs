using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppQuiz.ViewModel
{
    public class QuestionOptionViewModel
    {
        public int CategoryId { get; set; }
        public string QuestionName { get; set; }
        public List<string> ListOfOptions { get; set; }
        public string AnswerText { get; set; }
    }
}