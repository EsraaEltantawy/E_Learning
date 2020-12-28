using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using E_Learning.Models;

using Microsoft.AspNet.Identity.Owin;


namespace E_Learning.Controllers
{
    [Authorize(Roles = "Admin")]

    [HandleError]
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;
        E_LearningEntities db = new E_LearningEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private void AddErrors(Microsoft.AspNet.Identity.IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        // GET: /Account/RegisterTeacher
        [Authorize(Roles = "Admin")]
        public ActionResult RegisterTeacher()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterTeacher(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "Teacher");
                    Teacher teacher = new Teacher();
                    teacher.TeachId = user.Id;
                    teacher.active = true;
                    db.Teachers.Add(teacher);
                    db.SaveChanges();
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Edit", new { id = teacher.TeachId });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public ActionResult IndexTeach()
        {
            var AcivatedTeachers = db.Teachers.Where(x => x.active == true);
            return View(AcivatedTeachers.ToList());
        }
        public ActionResult DeactiveTeacher(string id)
        {
            var teacherss = db.Teachers.Find(id);
            return View(teacherss);
        }
        [HttpPost, ActionName("DeactiveTeacher")]

        public ActionResult DeactiveTeacherConfirm(string id)
        {

            var xx = db.Teachers.Find(id);
            xx.active = false;
            db.Entry(xx).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexTeach");
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubID = new SelectList(db.Subjects, "SubId", "SubName", teacher.SubID);
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(Teacher teacher, HttpPostedFileBase upload)
        {
            string OldPath = Path.Combine(Server.MapPath("~/Image/Teachers") + teacher.TeachId);
            if (ModelState.IsValid)
            {

                if (upload != null)
                {
                    if (teacher.TeachPhoto != null)
                    {
                        System.IO.File.Delete(OldPath);
                        string[] arr = upload.FileName.Split('.');
                        string filename = teacher.TeachId + "." + arr[arr.Length - 1];
                        string path = Path.Combine(Server.MapPath("~/Image/Teachers"), filename);
                        upload.SaveAs(path);
                        //var u = db.Students.FirstOrDefault(x => x.StuID == student.StuID);
                        teacher.TeachPhoto = filename;

                    }
                    else
                    {
                        System.IO.File.Delete(OldPath);
                        string[] arr = upload.FileName.Split('.');
                        string filename = teacher.TeachId + "." + arr[arr.Length - 1];
                        string path = Path.Combine(Server.MapPath("~/Image/Teachers"), filename);
                        upload.SaveAs(path);
                        //var u = db.Students.FirstOrDefault(x => x.StuID == student.StuID);
                        teacher.TeachPhoto = filename;

                    }

                    teacher.active = true;

                }
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AddLevelTeach", new { id = teacher.TeachId });
            }
            ViewBag.SubID = new SelectList(db.Subjects, "SubId", "SubName", teacher.SubID);
            return View(teacher);
        }
        

        public ActionResult AddLevelTeach(string id)
        {
            var teach = db.Teachers.Find(id);
            var ListOfClass = db.SubjectClasses.Where(x => x.SubId == teach.SubID);
            ViewBag.teachid = id;
            ViewBag.subname = teach.Subject.SubName;



            return View(ListOfClass.ToList());
        }
        [HttpPost]
        public ActionResult AddLevelTeach(IEnumerable<SubjectClass> subclass, string id)
        {
            foreach (var item in subclass)
            {
                if (item.IsSelected)
                {
                    TeacherClass tot = new TeacherClass();
                    tot.ClassID = item.Class.LevelID;
                    tot.TeachId = id;
                    db.TeacherClasses.Add(tot);
                    db.SaveChanges();

                }
            }

            return RedirectToAction("IndexTeach");
        }
        public ActionResult EditLevelTeach(string id)
        {
            ViewBag.TeachId = id;
            ViewBag.subname = db.Teachers.Find(id).Subject.SubName;
            var teach = db.Teachers.FirstOrDefault(x => x.TeachId == id);
            var ListOfClass = db.SubjectClasses.Where(x => x.SubId == teach.SubID);
            var ListOfTeachClass = db.TeacherClasses.Where(x => x.TeachId == id);
            foreach (var item in ListOfClass)
            {
                foreach (var it in ListOfTeachClass)
                {
                    if (item.LevelID == it.ClassID)
                    {
                        item.IsSelected = true;
                    }
                }
            }

            return View(ListOfClass);
        }
        [HttpPost]
        public ActionResult EditLevelTeach(IEnumerable<SubjectClass> subclass, string id)
        {
            var ListOfteachclass = db.TeacherClasses.Where(x => x.TeachId == id);
            foreach (var item in ListOfteachclass)
            {

                if (!item.Thevideos.Any())
                {
                    db.TeacherClasses.Remove(item);
                }


            }

            db.SaveChanges();

            foreach (var item in subclass)
            {
                if (!ListOfteachclass.Any(x => x.ClassID == item.Class.LevelID))
                {
                    if (item.IsSelected)
                    {

                        TeacherClass tecclss = new TeacherClass();
                        tecclss.ClassID = item.Class.LevelID;
                        tecclss.TeachId = id;
                        db.TeacherClasses.Add(tecclss);

                    }
                }


            }

            db.SaveChanges();
            //var listOfVideosToTeach = db.Thevideos.Where(x => x.TeaClassID ==)

            return RedirectToAction("IndexTeach");
        }

        public ActionResult TeachLevel(string id)
        {
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



        public ActionResult IndexVideo(int? id)
        {


            return View(db.Thevideos.Where(x => x.TeaClassID == id).ToList());
        }
        // GET: Thevideos/Create
        public ActionResult CreateVideo(int id )
        {
            ViewBag.TCID = id;
            ViewBag.TName = db.TeacherClasses.Find(id).Teacher.TeachName;
            ViewBag.CName = db.TeacherClasses.Find(id).Class.ClassName;

            return View();
        }

        // POST: Thevideos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateVideo(Thevideo thevideo, int id, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
               
                string[] arr = upload.FileName.Split('.');
                string filename = thevideo.VideoId + "." + arr[arr.Length - 1];
                string path = Path.Combine(Server.MapPath("~/video"), filename);
                upload.SaveAs(path);
                //var u = db.Students.FirstOrDefault(x => x.StuID == student.StuID);
                thevideo.VideoPath = filename;
                //string[] arr = path.Split('=');
                //path = arr[arr.Length - 1];
                var Tid = db.TeacherClasses.Find(id).Teacher.TeachId;
                ViewBag.Tid = Tid;
                thevideo.TeaClassID = id;
                db.Thevideos.Add(thevideo);
                db.SaveChanges();
                return RedirectToAction("TeachLevel", "Admin", new { id = Tid });
            }

            ViewBag.TeaClassID = new SelectList(db.TeacherClasses, "TeaLevelID", "TeachId", thevideo.TeaClassID);
            return View(thevideo);
        }


        // GET: Thevideos/Delete/5
        public ActionResult DeleteVideo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thevideo thevideo = db.Thevideos.Find(id);
            if (thevideo == null)
            {
                return HttpNotFound();
            }
            return View(thevideo);
        }

        // POST: Thevideos/Delete/5
        [HttpPost, ActionName("DeleteVideo")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var TId = db.Thevideos.Find(id).TeacherClass.TeaLevelID;
            Thevideo thevideo = db.Thevideos.Find(id);
            db.Thevideos.Remove(thevideo);
            db.SaveChanges();
            return RedirectToAction("IndexVideo", new { id = TId });
        }
        public ActionResult StudentIndex()
        {
            return View(db.Students.Where(x => x.active == true).ToList());
        }
    }
   
}