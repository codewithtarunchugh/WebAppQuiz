using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebAppQuiz.Models;
using WebAppQuiz.ViewModel;

namespace WebAppQuiz.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private QuizDBEntities objQuizDBEntities;

        public AdminController()
        {
            objQuizDBEntities = new QuizDBEntities();
        }
        // GET: Admin
        public ActionResult Index()
        {
            CategoryViewModel objCategoryViewModel = new CategoryViewModel();
            objCategoryViewModel.ListofCategory = (from objCat in objQuizDBEntities.Categories
                                                   select new SelectListItem()
                                                   {
                                                       Value = objCat.CategoryId.ToString(),
                                                       Text = objCat.CategoryName
                                                   }
                                                 ).ToList();
            return View(objCategoryViewModel);
        }
        [HttpPost]
        public ActionResult Index(QuestionOptionViewModel QuestionOption)
        {
            Question objQuestion = new Question();
            objQuestion.QuestionName = QuestionOption.QuestionName;
            objQuestion.CategoryId = QuestionOption.CategoryId;
            objQuestion.IsActive = true;
            objQuestion.IsMultiple = false;
            objQuizDBEntities.Questions.Add(objQuestion);
            objQuizDBEntities.SaveChanges();

            int questionId = objQuestion.QuestionId;

            foreach (var item in QuestionOption.ListOfOptions)
            {
                Option objOption = new Option();
                objOption.OptionName = item;
                objOption.QuestionId = questionId;
                objQuizDBEntities.Options.Add(objOption);
                objQuizDBEntities.SaveChanges();
            }

            Answer objAnswer = new Answer();
            objAnswer.AnswerText = QuestionOption.AnswerText;
            objAnswer.QuestionId = questionId;
            objQuizDBEntities.Answers.Add(objAnswer);
            objQuizDBEntities.SaveChanges();

            return Json(new { message = "Data Successfully Added." }, JsonRequestBehavior.AllowGet);
        }
    }
}