using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Learning.Models;

namespace E_Learning.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        E_LearningEntities db = new E_LearningEntities(); 
        public ActionResult Index()
        {
            repo repo = new repo();
            repo.SubjectClasses = db.SubjectClasses.Take(6).ToList();
            repo.Teachers = db.Teachers.Where(x=>x.active==true).Take(4).ToList();
          
            return View(repo);
        }
        public ActionResult Subject()
        {
          

            return View(db.SubjectClasses.ToList());
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}