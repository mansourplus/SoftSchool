using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftSchool.Models;
using System.IO;

namespace SoftSchool.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrateur")]
    public class LogicielsController : Controller
    {
        private DBSoftSchoolEntities4 db = new DBSoftSchoolEntities4();

        private string StorageRoot
        {
            get { return Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Files/")); } //Path should! always end with '/'
        }

        //
        // GET: /Admin/Logiciels/

        public ViewResult Index()
        {
            return View(db.Logiciels.ToList());
        }

        //
        // GET: /Admin/Logiciels/Details/5

        public ViewResult Details(int id)
        {
            Logiciels logiciels = db.Logiciels.Single(l => l.LogicielID == id);
            return View(logiciels);
        }

        //
        // GET: /Admin/Logiciels/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Admin/Logiciels/Create

        [HttpPost]
        public ActionResult Create(Logiciels logiciels)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count == 0)
                    return View(logiciels);
                else
                {
                    HttpPostedFileBase SourceFile = Request.Files["Image"];
                    var inputStream = SourceFile.InputStream;
                    var fullName = StorageRoot + logiciels.Nom + "_" + logiciels.Version + Path.GetExtension(Request.Files["Image"].FileName);
                    logiciels.Image = "Files\\" + logiciels.Nom + "_" + logiciels.Version +  Path.GetExtension(Request.Files["Image"].FileName);
                    using (var fsi = new FileStream(fullName, FileMode.Append, FileAccess.Write))
                    {
                        var buffer = new byte[1024];
                        var l = inputStream.Read(buffer, 0, 1024);
                        while (l > 0)
                        {
                            fsi.Write(buffer, 0, l);
                            l = inputStream.Read(buffer, 0, 1024);
                        }
                        fsi.Flush();
                        fsi.Close();

                        SourceFile = Request.Files["Lien"];
                        inputStream = SourceFile.InputStream;
                        fullName = StorageRoot + "Logs\\" + logiciels.Nom + "_" + logiciels.Version + Path.GetExtension(Request.Files["Lien"].FileName);
                        logiciels.Lien = "Files\\Logs\\" + logiciels.Nom + "_" + logiciels.Version +  Path.GetExtension(Request.Files["Lien"].FileName);
                        using (var fsl = new FileStream(fullName, FileMode.Append, FileAccess.Write))
                        {
                            buffer = new byte[1024];
                            l = inputStream.Read(buffer, 0, 1024);
                            while (l > 0)
                            {
                                fsl.Write(buffer, 0, l);
                                l = inputStream.Read(buffer, 0, 1024);
                            }
                            fsl.Flush();
                            fsl.Close();
                        }

                        db.Logiciels.AddObject(logiciels);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(logiciels);
        }
        
        //
        // GET: /Admin/Logiciels/Edit/5
 
        public ActionResult Edit(int id)
        {
            Logiciels logiciels = db.Logiciels.Single(l => l.LogicielID == id);
            return View(logiciels);
        }

        //
        // POST: /Admin/Logiciels/Edit/5

        [HttpPost]
        public ActionResult Edit(Logiciels logiciels)
        {
            if (ModelState.IsValid)
            {
                db.Logiciels.Attach(logiciels);
                db.ObjectStateManager.ChangeObjectState(logiciels, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(logiciels);
        }

        //
        // GET: /Admin/Logiciels/Delete/5
 
        public ActionResult Delete(int id)
        {
            Logiciels logiciels = db.Logiciels.Single(l => l.LogicielID == id);
            return View(logiciels);
        }

        //
        // POST: /Admin/Logiciels/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Logiciels logiciels = db.Logiciels.Single(l => l.LogicielID == id);
            db.Logiciels.DeleteObject(logiciels);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}