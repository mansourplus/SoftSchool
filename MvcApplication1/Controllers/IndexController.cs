using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftSchool.Models;
namespace SoftSchool.Controllers
{
    public class IndexController : Controller
    {
        DBSoftSchoolEntities4 db = new DBSoftSchoolEntities4();
        public ActionResult Index()
        {
            ViewBag.Message = "";
            var model = db.Logiciels;
            return View(model.ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult DetailLog(int id)
        {
            var log = db.Logiciels.Single(l => l.LogicielID == id);
            return View(log);
        }
    }
}
