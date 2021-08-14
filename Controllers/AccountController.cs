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
    public class AccountController : Controller
    {
        private QuizDBEntities objQuizDBEntities;

        public AccountController()
        {
            objQuizDBEntities = new QuizDBEntities();
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(AdminViewModel objAdminViewModel)
        {
            if (ModelState.IsValid)
            {
                Admin objAdmin = objQuizDBEntities.Admins.SingleOrDefault(model => model.UserName == objAdminViewModel.UserName);
                if (objAdmin == null)
                {
                    ModelState.AddModelError(string.Empty, "Email is not exists.");
                }
                else if (objAdmin.UserPassword != objAdminViewModel.UserPassword)
                {
                    ModelState.AddModelError(string.Empty, "Email & Password is invalid.");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(objAdminViewModel.UserName, false);
                    var authTicket = new FormsAuthenticationTicket(1, objAdmin.UserName, DateTime.Now, DateTime.Now.AddMinutes(20), false, "Admin");

                    string encrypTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypTicket);
                    HttpContext.Response.Cookies.Add(authCookie);
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}