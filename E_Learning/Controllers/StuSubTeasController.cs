using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Learning.Models;

namespace E_Learning.Controllers
{
    [HandleError]
    public class StuSubTeasController : Controller
    {
        private E_LearningEntities db = new E_LearningEntities();

        // GET: StuSubTeas
        public ActionResult Index()
        {
            var stuSubTeas = db.StuSubTeas.Include(s => s.StudentSubject).Include(s => s.Teacher);
            return View(stuSubTeas.ToList());
        }

        // GET: StuSubTeas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StuSubTea stuSubTea = db.StuSubTeas.Find(id);
            if (stuSubTea == null)
            {
                return HttpNotFound();
            }
            return View(stuSubTea);
        }

        // GET: StuSubTeas/Create
        public ActionResult chooseTeacher(int id)
        {
            var SublevelId = db.StudentSubjects.FirstOrDefault(x => x.StuSubID == id).SubLevelID;
            var levilid = db.SubjectClasses.FirstOrDefault(x => x.SubLevelID == SublevelId).LevelID;
            var subid = db.SubjectClasses.FirstOrDefault(x => x.SubLevelID == SublevelId).SubId;
            var liOfTeac = db.TeacherClasses.Where(x => x.ClassID == levilid).Include(y => y.Teacher).Where(s => s.Teacher.SubID == subid);
            //var ListOfTeach1 = db.Teachers.Where(x => x.SubID == subid).Include(y=>y.TeacherClasses).Include(z=>z.Subject).Select(s=>s.TeachName);
            //var ListOfTeach11 = db.Teachers.Where(x => x.SubID == subid).Select(s => s.TeachName);
            //var listofTeach2 = db.TeacherClasses.Where(x=>x.ClassID== levilid);
           


            ViewBag.StuSubID = new SelectList(db.StudentSubjects, "StuSubID", "StuSubID");
            ViewBag.TeachId = new SelectList(db.Teachers, "TeachId", "TeachName");
            return View(liOfTeac.ToList());
        }

        // To protect from overposting attacks, enable the sp
        // POST: StuSubTeas/Createecific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StuSubTeaID,StuSubID,TeachId,ReDate")] StuSubTea stuSubTea)
        {
            if (ModelState.IsValid)
            {
                db.StuSubTeas.Add(stuSubTea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StuSubID = new SelectList(db.StudentSubjects, "StuSubID", "StuSubID", stuSubTea.StuSubID);
            ViewBag.TeachId = new SelectList(db.Teachers, "TeachId", "TeachName", stuSubTea.TeachId);
            return View(stuSubTea);
        }

        // GET: StuSubTeas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StuSubTea stuSubTea = db.StuSubTeas.Find(id);
            if (stuSubTea == null)
            {
                return HttpNotFound();
            }
            ViewBag.StuSubID = new SelectList(db.StudentSubjects, "StuSubID", "StuSubID", stuSubTea.StuSubID);
            ViewBag.TeachId = new SelectList(db.Teachers, "TeachId", "TeachName", stuSubTea.TeachId);
            return View(stuSubTea);
        }

        // POST: StuSubTeas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StuSubTeaID,StuSubID,TeachId,ReDate")] StuSubTea stuSubTea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stuSubTea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StuSubID = new SelectList(db.StudentSubjects, "StuSubID", "StuSubID", stuSubTea.StuSubID);
            ViewBag.TeachId = new SelectList(db.Teachers, "TeachId", "TeachName", stuSubTea.TeachId);
            return View(stuSubTea);
        }

        // GET: StuSubTeas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StuSubTea stuSubTea = db.StuSubTeas.Find(id);
            if (stuSubTea == null)
            {
                return HttpNotFound();
            }
            return View(stuSubTea);
        }

        // POST: StuSubTeas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StuSubTea stuSubTea = db.StuSubTeas.Find(id);
            db.StuSubTeas.Remove(stuSubTea);
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
