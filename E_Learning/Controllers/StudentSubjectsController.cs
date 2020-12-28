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
    [HandleError]
    public class StudentSubjectsController : Controller
    {
        private E_LearningEntities db = new E_LearningEntities();

        // GET: StudentSubjects
        public ActionResult Index(string id)
        {
            if (id==null)
            {
              //  var studentSubjects = db.StudentSubjects.Include(s => s.Student).Include(s => s.SubjectClass);
             //   return View(studentSubjects.ToList());
            }

            ViewBag.StuId = id;
            ViewBag.StuName = db.Students.FirstOrDefault(x => x.StuID == id).StuName;
            var StudentLevel = db.Students.FirstOrDefault(x => x.StuID == id).levelID;
            var listOfSubLevle = db.SubjectClasses.Where(x => x.LevelID == StudentLevel);
            //ViewBag.StuId = id;
            //ViewBag.StuName = db.Students.FirstOrDefault(x => x.StuID == id).StuName;
            //var studentSubjects1 = db.StudentSubjects.Where(s => s.StuId==id).Include(s => s.SubjectClass);
            return View(listOfSubLevle.ToList());
        }

        // GET: StudentSubjects/Details/5
        public ActionResult Details(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentSubject studentSubject = db.StudentSubjects.Find(id);
            if (studentSubject == null)
            {
                return HttpNotFound();
            }
            return View(studentSubject);
        }

        // GET: StudentSubjects/Create
        public ActionResult chooseTeacher(StudentSubject studentSubject,int id)
        {

            //var SublevelId = db.SubjectClasses.FirstOrDefault(x => x.SubLevelID == id);
            //var levilid = db.SubjectClasses.FirstOrDefault(x => x.SubLevelID == SublevelId).LevelID;
            //var subid = db.SubjectClasses.FirstOrDefault(x => x.SubLevelID == SublevelId).SubId;
            //var liOfTeac = db.TeacherClasses.Where(x => x.ClassID == levilid).Include(y => y.Teacher).Where(s => s.Teacher.SubID == subid);
            ////var ListOfTeach1 = db.Teachers.Where(x => x.SubID == subid).Include(y=>y.TeacherClasses).Include(z=>z.Subject).Select(s=>s.TeachName);
            //var ListOfTeach11 = db.Teachers.Where(x => x.SubID == subid).Select(s => s.TeachName);
            //var listofTeach2 = db.TeacherClasses.Where(x=>x.ClassID== levilid);

            // ViewBag.SubLevelID = new SelectList(db.SubjectClasses, "SubLevelID","SubLevelID");
            return View();
        }

        // POST: StudentSubjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( StudentSubject studentSubject)
        {

            


            if (ModelState.IsValid)
            {
                studentSubject.Date = DateTime.Now;
                db.StudentSubjects.Add(studentSubject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StuId = new SelectList(db.Students, "StuID", "StuName", studentSubject.StuId);
            ViewBag.SubLevelID = new SelectList(db.SubjectClasses, "SubLevelID", "SubLevelID", studentSubject.SubLevelID);
            return View(studentSubject);
        }

        public ActionResult chooseTeacher()
        {
            return View();
        }
        // GET: StudentSubjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentSubject studentSubject = db.StudentSubjects.Find(id);
            if (studentSubject == null)
            {
                return HttpNotFound();
            }
            ViewBag.StuId = new SelectList(db.Students, "StuID", "StuName", studentSubject.StuId);
            ViewBag.SubLevelID = new SelectList(db.SubjectClasses, "SubLevelID", "SubLevelID", studentSubject.SubLevelID);
            return View(studentSubject);
        }

        // POST: StudentSubjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StuSubID,SubLevelID,StuId,Date")] StudentSubject studentSubject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentSubject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StuId = new SelectList(db.Students, "StuID", "StuName", studentSubject.StuId);
            ViewBag.SubLevelID = new SelectList(db.SubjectClasses, "SubLevelID", "SubLevelID", studentSubject.SubLevelID);
            return View(studentSubject);
        }

        // GET: StudentSubjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentSubject studentSubject = db.StudentSubjects.Find(id);
            if (studentSubject == null)
            {
                return HttpNotFound();
            }
            return View(studentSubject);
        }

        // POST: StudentSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentSubject studentSubject = db.StudentSubjects.Find(id);
            db.StudentSubjects.Remove(studentSubject);
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
