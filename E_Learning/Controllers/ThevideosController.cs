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
    //[Authorize (Roles ="")]
    [HandleError]
    public class ThevideosController : Controller
    {
        private E_LearningEntities db = new E_LearningEntities();

        // GET: Thevideos
       

        // GET: Thevideos/Details/5
        public ActionResult Details(int? id)
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

       
        // GET: Thevideos/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.TeaClassID = new SelectList(db.TeacherClasses, "TeaLevelID", "TeachId", thevideo.TeaClassID);
            return View(thevideo);
        }

        // POST: Thevideos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VideoId,VideoPath,Date,TeaClassID,NumOfView")] Thevideo thevideo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thevideo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeaClassID = new SelectList(db.TeacherClasses, "TeaLevelID", "TeachId", thevideo.TeaClassID);
            return View(thevideo);
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
