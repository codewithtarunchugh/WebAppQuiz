using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppQuiz.Models;
using WebAppQuiz.ViewModel;

namespace WebAppQuiz.Controllers
{
    public class QuizController : Controller
    {

        private QuizDBEntities objQuizDBEntities;
        public QuizController()
        {
            objQuizDBEntities = new QuizDBEntities();
        }
        // GET: QuizController
        public ActionResult Index()
        {
            QuizCategoryViewModel objQuizCategoryViewModel = new QuizCategoryViewModel();
            objQuizCategoryViewModel.ListOfCategory = (from obj in objQuizDBEntities.Categories
                                                       select new SelectListItem()
                                                       {
                                                           Value = obj.CategoryId.ToString(),
                                                           Text = obj.CategoryName
                                                       }).ToList();
            return View(objQuizCategoryViewModel);

        }
        [HttpPost]
        public ActionResult Index(string CandidateName, int CategoryId)
        {
            User objUser = new User();
            objUser.UserName = CandidateName;
            objUser.LoginTime = DateTime.Now; ;
            objUser.CategoryId = CategoryId;
            objQuizDBEntities.Users.Add(objUser);
            objQuizDBEntities.SaveChanges();

            Session["CandidateName"] = CandidateName;
            Session["CategoryId"] = CategoryId;
            Session["UserId"] = objUser.UserId;
            return View("QuestionIndex");
        }
        public PartialViewResult UserQuestionAnswer(CandidateAnswer objCandidateAnswer)
        {
            bool IsLast = false;
            if (objCandidateAnswer.AnswerText != null)
            {
                List<CandidateAnswer> objCandidateAnswers = Session["CadQuestionAnswer"] as List<CandidateAnswer>;
                if (objCandidateAnswers == null)
                {
                    objCandidateAnswers = new List<CandidateAnswer>();
                }
                objCandidateAnswers.Add(objCandidateAnswer);
                Session["CadQuestionAnswer"] = objCandidateAnswers;
            }


            int pageSize = 1;
            int pageNumber = 0;
            int CategoryId = Convert.ToInt32(Session["CategoryId"]);
            if (Session["CadQuestionAnswer"] == null)
            {
                pageNumber = pageNumber + 1;

            }
            else
            {
                List<CandidateAnswer> canAnswer = Session["CadQuestionAnswer"] as List<CandidateAnswer>;
                pageNumber = canAnswer.Count + 1;
            }

            List<Question> listOfQuestion = new List<Question>();
            listOfQuestion = objQuizDBEntities.Questions.Where(model => model.CategoryId == CategoryId).ToList();

            if (pageNumber == listOfQuestion.Count)
            {
                IsLast = true;
            }

            QuizQuestionAnswerViewModel objQuizQuestionAnswerViewModel = new QuizQuestionAnswerViewModel();
            Question objQuestion = new Question();
            objQuestion = listOfQuestion.Skip((pageNumber - 1) * pageSize).Take(pageSize).FirstOrDefault();
            objQuizQuestionAnswerViewModel.IsLast = IsLast;
            objQuizQuestionAnswerViewModel.QuestionId = objQuestion.QuestionId;
            objQuizQuestionAnswerViewModel.QuestionName = objQuestion.QuestionName;
            objQuizQuestionAnswerViewModel.ListOfQuizOption = (from obj in objQuizDBEntities.Options
                                                               where obj.QuestionId == objQuestion.QuestionId
                                                               select new QuizOption()
                                                               {
                                                                   OptionName = obj.OptionName,
                                                                   OptionId = obj.OptionId
                                                               }).ToList();

            ModelState.Remove("QuestionId");
            return PartialView("QuizQuestionOption", objQuizQuestionAnswerViewModel);
        }

        public JsonResult SaveCandidateAnswer(CandidateAnswer objCandidateAnswer)
        {
            if (objCandidateAnswer.AnswerText != null)
            {
                List<CandidateAnswer> objCandidateAnswers = Session["CadQuestionAnswer"] as List<CandidateAnswer>;
                if (objCandidateAnswers == null)
                {
                    objCandidateAnswers = new List<CandidateAnswer>();
                }
                objCandidateAnswers.Add(objCandidateAnswer);
                Session["CadQuestionAnswer"] = objCandidateAnswers;
            }
            List<CandidateAnswer> CanAnswer = Session["CadQuestionAnswer"] as List<CandidateAnswer>;
            foreach (var item in CanAnswer)
            {
                Result objResult = new Result();
                objResult.AnswerText = item.AnswerText;
                objResult.QuestionId = item.QuestionId;
                objResult.UserId = Convert.ToInt32(Session["UserId"]);
                objQuizDBEntities.Results.Add(objResult);
                objQuizDBEntities.SaveChanges();
            }
            return Json(new { message = "Data Successfully Added", success = true }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetFinalResult()
        {
            List<CandidateAnswer> listOfQuestionAnswers;
            listOfQuestionAnswers = Session["CadQuestionAnswer"] as List<CandidateAnswer>;
            var UserResult = (from objResult in listOfQuestionAnswers
                              join objAnswers in objQuizDBEntities.Answers on objResult.QuestionId equals objAnswers.QuestionId
                              join objQuestion in objQuizDBEntities.Questions on objResult.QuestionId equals objQuestion.QuestionId


                              select new ResultModel()
                              {
                                  Question = objQuestion.QuestionName,
                                  Answer = objAnswers.AnswerText,
                                  AnswerByUser = objResult.AnswerText,
                                  Status = objAnswers.AnswerText == objResult.AnswerText ? "Correct" : "Wrong"
                              }
                              ).ToList();
            Session.Abandon();
            return View(UserResult);


        }

    }
}