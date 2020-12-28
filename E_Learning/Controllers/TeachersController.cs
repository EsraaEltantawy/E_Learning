using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Learning.Models;
using Microsoft.AspNet.Identity;

namespace E_Learning.Controllers
{
    //[Authorize(Roles = "Teacher")]

    [HandleError]
    public class TeachersController : Controller
    {
        private E_LearningEntities db = new E_LearningEntities();

        // GET: Teachers
        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            if (db.Teachers.Find(id).active == false)
            {
                ViewBag.ErrorMessage = "This teacher is Deactived , contact with Admin";
                return View("Error");
            }
            else
            {
                ViewBag.teachName = db.Teachers.FirstOrDefault(x => x.TeachId == id).TeachName;
                return View(db.TeacherClasses.Where(x => x.TeachId == id));
            }
        }

        // GET: Teachers/Details/5
        public ActionResult Details()
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Teacher teacher = db.Teachers.Find(User.Identity.GetUserId());
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        public ActionResult IndexVideo(int? id)
        {


            return View(db.Thevideos.Where(x => x.TeaClassID == id).ToList());
        }
    }
}
