using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Learning.Models;

namespace E_Learning.Controllers
{
    [Authorize(Roles = "Admin")]
    [HandleError]
    public class SubjectClassesController : Controller
    {
        private E_LearningEntities db = new E_LearningEntities();

        // GET: SubjectClasses
        public ActionResult Index()
        {
            var subjectClasses = db.SubjectClasses.Include(s => s.Class).Include(s => s.Subject);
            return View(subjectClasses.ToList());
        }

        // GET: SubjectClasses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectClass subjectClass = db.SubjectClasses.Find(id);
            if (subjectClass == null)
            {
                return HttpNotFound();
            }
            return View(subjectClass);
        }

        // GET: SubjectClasses/Create
        public ActionResult Create()
        {
            ViewBag.LevelID = new SelectList(db.Classes, "LevelID", "ClassName");
            ViewBag.SubId = new SelectList(db.Subjects, "SubId", "SubName");
            return View();
        }

        // POST: SubjectClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( SubjectClass subjectClass)
        {
            if (ModelState.IsValid)
            {
                if (db.SubjectClasses.Any(x=>x.LevelID==subjectClass.LevelID)&&db.SubjectClasses.Any(y=>y.SubId==subjectClass.SubId))
                {
                    ViewBag.ErrorMsg = "this course is already Exist , enter anthor class or anthor subject";
                    ViewBag.LevelID = new SelectList(db.Classes, "LevelID", "ClassName", subjectClass.LevelID);
                    ViewBag.SubId = new SelectList(db.Subjects, "SubId", "SubName", subjectClass.SubId);
                    return View(subjectClass);
                }
                //var s = db.Classes.Where(x=>x.LevelID==subjectClass.LevelID).Select(x=>x.ClassName)+" "+db.Subjects.Where(y=>y.SubId==subjectClass.SubId).Select(z=>z.SubName);
                //  subjectClass.SubLevelName = s;
                subjectClass.Date = DateTime.Now;
                db.SubjectClasses.Add(subjectClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LevelID = new SelectList(db.Classes, "LevelID", "ClassName", subjectClass.LevelID);
            ViewBag.SubId = new SelectList(db.Subjects, "SubId", "SubName", subjectClass.SubId);
            return View(subjectClass);
        }

        // GET: SubjectClasses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectClass subjectClass = db.SubjectClasses.Find(id);
            if (subjectClass == null)
            {
                return HttpNotFound();
            }
            ViewBag.LevelID = new SelectList(db.Classes, "LevelID", "ClassName", subjectClass.LevelID);
            ViewBag.SubId = new SelectList(db.Subjects, "SubId", "SubName", subjectClass.SubId);
            return View(subjectClass);
        }

        // POST: SubjectClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( SubjectClass subjectClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subjectClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LevelID = new SelectList(db.Classes, "LevelID", "ClassName", subjectClass.LevelID);
            ViewBag.SubId = new SelectList(db.Subjects, "SubId", "SubName", subjectClass.SubId);
            return View(subjectClass);
        }

        // GET: SubjectClasses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectClass subjectClass = db.SubjectClasses.Find(id);
            if (subjectClass == null)
            {
                return HttpNotFound();
            }
            return View(subjectClass);
        }

        // POST: SubjectClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubjectClass subjectClass = db.SubjectClasses.Find(id);
            db.SubjectClasses.Remove(subjectClass);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
