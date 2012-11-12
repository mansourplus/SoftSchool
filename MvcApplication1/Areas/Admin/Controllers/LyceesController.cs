using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftSchool.Models;

namespace SoftSchool.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrateur")]
    public class LyceesController : Controller
    {
        private DBSoftSchoolEntities4 db = new DBSoftSchoolEntities4();

        //
        // GET: /Admin/Lycees/

        public ViewResult Index()
        {
            var lycees = db.Lycees.Include("Villes");
            return View(lycees.ToList());
        }

        //
        // GET: /Admin/Lycees/Details/5

        public ViewResult Details(int id)
        {
            Lycees lycees = db.Lycees.Single(l => l.LyceeID == id);
            ViewBag.DREID = db.Villes.First(v => v.DreID == id).DreID;
            return View(lycees);
        }

        //
        // GET: /Admin/Lycees/Create

        public ActionResult Create(int id)
        {
            ViewBag.VilleId = new SelectList(db.Villes.Where(v=>v.DreID==id), "VilleID", "Nom_Ar");
            ViewBag.DREID = db.Villes.First(v => v.DreID == id).DreID;
            return View();
        } 

        //
        // POST: /Admin/Lycees/Create

        [HttpPost]
        public ActionResult Create(Lycees lycees)
        {
            if (ModelState.IsValid)
            {
                db.Lycees.AddObject(lycees);
                try
                {
                    db.SaveChanges();
                }
                catch { }
                return RedirectToAction("Lycees/"+lycees.Villes.DreID, "Direction");
            }

            ViewBag.VilleId = new SelectList(db.Villes, "VilleID", "Nom_Ar", lycees.VilleId);
            return View(lycees);
        }
        
        //
        // GET: /Admin/Lycees/Edit/5
 
        public ActionResult Edit(int id)
        {
            Lycees lycees = db.Lycees.Single(l => l.LyceeID == id);
            ViewBag.DREID = db.Villes.First(v => v.DreID == id).DreID;
            ViewBag.VilleId = new SelectList(db.Villes.Where(v=>v.DreID== lycees.Villes.DreID), "VilleID", "Nom_Ar", lycees.VilleId);
            return View(lycees);
        }

        //
        // POST: /Admin/Lycees/Edit/5

        [HttpPost]
        public ActionResult Edit(Lycees lycees)
        {
            if (ModelState.IsValid)
            {
                db.Lycees.Attach(lycees);
                db.ObjectStateManager.ChangeObjectState(lycees, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Lycees/" + lycees.Villes.DreID, "Direction");
            }
            ViewBag.VilleId = new SelectList(db.Villes, "VilleID", "Nom_Ar", lycees.VilleId);
            return View(lycees);
        }

        //
        // GET: /Admin/Lycees/Delete/5
 
        public ActionResult Delete(int id)
        {
            Lycees lycees = db.Lycees.Single(l => l.LyceeID == id);
            ViewBag.DREID = db.Villes.First(v => v.DreID == id).DreID;
            return View(lycees);
        }

        //
        // POST: /Admin/Lycees/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Lycees lycees = db.Lycees.Include(v=>v.Villes).Single(l => l.LyceeID == id);
            Villes ville = db.Villes.Single(v => v.VilleID == lycees.VilleId);
            db.Lycees.DeleteObject(lycees);
            db.SaveChanges();
            return RedirectToAction("Lycees/" + ville.DreID, "Direction");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}