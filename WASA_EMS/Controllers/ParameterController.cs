using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using WASA_EMS.Models;
using System.Data.Entity;
using System.Net;
using System.Data;

namespace WASA_EMS.Controllers
{
    public class ParameterController : Controller
    {
        WASA_EMS_Entities db = new WASA_EMS_Entities();

        // GET: Parameter/Index
        public ActionResult Index()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            ViewBag.parameterList = db.tblParameters.AsEnumerable().Where(item => item.CompanyID == c_id);
            return View();
        }

        /*
        // GET: Parameter/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblParameter tblParameter = db.tblParameters.Find(id);
            if (tblParameter == null)
            {
                return HttpNotFound();
            }
            return View(tblParameter);
        }
        */

        // GET: Parameter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Parameter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ParameterID,ParameterName,ParameterOrder,Mainformula")] tblParameter tblparameter)
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            if (ModelState.IsValid)
            {
                tblparameter.CompanyID = c_id;
                db.tblParameters.Add(tblparameter);
                db.SaveChanges();
                DisplaySuccessMessage("Has append a tblParameter record");
                return RedirectToAction("Index");
            }

            DisplayErrorMessage();
            return View(tblparameter);
        }

        // GET: Parameter/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblParameter tblparameter = db.tblParameters.Find(id);
            if (tblparameter == null)
            {
                return HttpNotFound();
            }

            return View(tblparameter);
        }

        // POST: Parameter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ParameterID,ParameterName,ParameterOrder,Mainformula")] tblParameter tblparameter)
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            if (ModelState.IsValid)
            {
                tblparameter.CompanyID = c_id;
                db.Entry(tblparameter).State = EntityState.Modified;
                db.SaveChanges();
                DisplaySuccessMessage("Has update a tblParameter record");
                return RedirectToAction("Index");
            }
            DisplayErrorMessage();
            return View(tblparameter);
        }

        // GET: Parameter/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblParameter tblparameter = db.tblParameters.Find(id);
            if (tblparameter == null)
            {
                return HttpNotFound();
            }
            return View(tblparameter);
        }

        // POST: Parameter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblParameter tblparameter = db.tblParameters.Find(id);
            db.tblParameters.Remove(tblparameter);
            db.SaveChanges();
            DisplaySuccessMessage("Has delete a tblParameter record");
            return RedirectToAction("Index");
        }

        private void DisplaySuccessMessage(string msgText)
        {
            TempData["SuccessMessage"] = msgText;
        }

        private void DisplayErrorMessage()
        {
            TempData["ErrorMessage"] = "Save changes was unsuccessful.";
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
