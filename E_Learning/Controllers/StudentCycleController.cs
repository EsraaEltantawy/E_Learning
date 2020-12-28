using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using E_Learning.Models;
using Microsoft.AspNet.Identity;

namespace E_Learning.Controllers
{
 [Authorize (Roles ="Student")]
    [HandleError]
    public class StudentCycleController : Controller
    {
        // GET: StudentCycle
        E_LearningEntities db = new E_LearningEntities();
        public ActionResult Index()
        {
            var stu = User.Identity.GetUserId();



            var StudentLevel = db.Students.FirstOrDefault(x => x.StuID == stu).levelID;
            var stusub1 = db.StudentSubjects.Where(x => x.StuId == stu).Select(x=>x.StuSubID);
            List<StuSubTea> stuSubTeas = new List<StuSubTea>();

            foreach (var item in stusub1)
            {
                var teachclass = db.StuSubTeas.Where(x => x.StuSubID == item);
                stuSubTeas.AddRange(teachclass);
            }

            return View(stuSubTeas);
        }
        public ActionResult Edit()
        {
            if (User.Identity.GetUserId() == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(User.Identity.GetUserId());
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.levelID = new SelectList(db.Classes, "LevelID", "ClassName", student.levelID);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student, HttpPostedFileBase upload)
        {
            string OldPath = Path.Combine(Server.MapPath("~/Image/Students") + student.StuPhoto);
            if (ModelState.IsValid)
            {

                if (upload != null)
                {
                    if (student.StuPhoto != null)
                    {
                        //EDIT
                        System.IO.File.Delete(OldPath);
                        string[] Oarr = upload.FileName.Split('.');
                        string Ofilename = student.StuID + "." + Oarr[Oarr.Length - 1];
                        string Opath = Path.Combine(Server.MapPath("~/Image/Students"), Ofilename);
                        upload.SaveAs(Opath);
                        //var u = db.Students.FirstOrDefault(x => x.StuID == student.StuID);
                        student.StuPhoto = Ofilename;

                    }
                    else
                    {
                        //First Time
                        string[] arr = upload.FileName.Split('.');
                        string filename = student.StuID + "." + arr[arr.Length - 1];
                        string path = Path.Combine(Server.MapPath("~/Image/Students"), filename);
                        upload.SaveAs(path);    
                        //var u = db.Students.FirstOrDefault(x => x.StuID == student.StuID);
                        student.StuPhoto = filename;
                    }




                }
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index"); 
            }
            ViewBag.levelID = new SelectList(db.Classes, "LevelID", "ClassName", student.levelID);
            return View(student);
        }
        public ActionResult Details()
        {

            if (User.Identity.GetUserId() == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(User.Identity.GetUserId());
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }
        //List of All subject for student level
        public ActionResult ListOFSubject()
        {
            // ViewBag.StuId =Sid;
            var studentdata = db.Students.Find(User.Identity.GetUserId());
            ViewBag.StuName = studentdata.StuName;
            var StudentLevel = studentdata.levelID;
            var listOfSubLevle = db.SubjectClasses.Where(x => x.LevelID == StudentLevel);

            return View(listOfSubLevle.ToList());
        }
        public ActionResult ListOFTeacher(int id)
        {
            var stu = User.Identity.GetUserId();
            var subjectclass = db.SubjectClasses.Find(id);
            var thesubject = subjectclass.Subject;
            if (db.StudentSubjects.Any(x => x.SubLevelID == id && x.StuId == stu))
            {
                var studsubj = db.StudentSubjects.FirstOrDefault(x => x.SubLevelID == id && x.StuId == stu).StuSubID;
                var listofrigestredtea = db.StuSubTeas.Where(x => x.StuSubID == studsubj).Select(x => x.Teacher).Where(y=>y.active==true).ToList();
                var listofteachersforsubject1 = thesubject.Teachers.Where(x => x.TeacherClasses.Any(y => y.ClassID == subjectclass.LevelID )&&x.active==true).Except(listofrigestredtea);
                var teachersclasses = listofteachersforsubject1.SelectMany(x => x.TeacherClasses.Where(y => y.ClassID == subjectclass.LevelID && y.Thevideos.Any())).ToList();
                return View(teachersclasses);
            }
            else
            {
                var listofteachersforsubject = thesubject.Teachers.Where(x => x.TeacherClasses.Any(y => y.ClassID == subjectclass.LevelID)&&x.active==true);
                var teachersclasses = listofteachersforsubject.SelectMany(x => x.TeacherClasses.Where(y => y.ClassID == subjectclass.LevelID && y.Thevideos.Any())).ToList();
              
                var introVedios = teachersclasses.Where(x => x.Thevideos.Any(y=>y.IsIntroVideo==true)).ToList();
              
                return View(introVedios);
            }
           
        }
        public ActionResult SaveCourse(int id)
        {
            var stu = User.Identity.GetUserId();
            var Lev = db.TeacherClasses.Find(id).ClassID;
            var subj = db.TeacherClasses.Find(id).Teacher.SubID;
            var subjLevel = db.SubjectClasses.FirstOrDefault(x => x.LevelID == Lev && x.SubId == subj).SubLevelID;
            ViewBag.subjLevel = subjLevel;
             var Teacher = db.TeacherClasses.Find(id).TeachId;

            if (db.StudentSubjects.Any(x => x.SubLevelID == subjLevel && x.StuId == stu))
            {
                var studentSubjectID = db.StudentSubjects.FirstOrDefault(x => x.SubLevelID == subjLevel && x.StuId == stu).StuSubID;

                StuSubTea subTea = new StuSubTea();
                subTea.StuSubID = studentSubjectID;
                subTea.TeachId = Teacher;
                db.StuSubTeas.Add(subTea);
                db.SaveChanges();
            }
            else
            {
                StudentSubject studentSubject = new StudentSubject();
                studentSubject.SubLevelID = subjLevel;
                studentSubject.StuId = User.Identity.GetUserId();
                db.StudentSubjects.Add(studentSubject);

                StuSubTea subTea = new StuSubTea();
                subTea.StuSubID = studentSubject.StuSubID;
                subTea.TeachId = Teacher;
                db.StuSubTeas.Add(subTea);
                db.SaveChanges();
            }
            
           
          
            return View();
        }

        public ActionResult IndexVideo(int? id)
        {
            var TId= db.StuSubTeas.Find(id).TeachId;
            var TCId = db.TeacherClasses.FirstOrDefault(x => x.TeachId == TId).TeaLevelID;

            return View(db.Thevideos.Where(x => x.TeaClassID == TCId).ToList());
        }
        public ActionResult IntroVideo(int? id)
        {
           
            return View(db.Thevideos.FirstOrDefault(x => x.TeaClassID == id && x.IsIntroVideo == true));
        }
    }
}