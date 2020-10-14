using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WASA_EMS.Models;
using System.Data.SqlClient;
using System.Net;

namespace WASA_EMS.Controllers
{
    public class TemplateController : Controller
    {
        private WASA_EMS_Entities db = new WASA_EMS_Entities();
        // GET: Template/Index  
        public int Company;
        public ActionResult Index()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            Company = c_id;
            return View(db.tblTemplates.Where(item => item.CompanyID == c_id).ToList());
        }

        /*
        // GET: Template/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTemplate tblTemplate = db.tblTemplates.Find(id);
            if (tblTemplate == null)
            {
                return HttpNotFound();
            }
            return View(tblTemplate);
        }
        */

        // GET: Template/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Template/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TemplateID,TemplateName,ParameterCount,Separator,tblResources,tblTemplateParameters")] tblTemplate tblTemplate)
        {
            if (ModelState.IsValid)
            {
                db.tblTemplates.Add(tblTemplate).CompanyID.Equals(Company);
                db.SaveChanges();
                DisplaySuccessMessage("Has append a tblTemplate record");
                return RedirectToAction("Index");
            }

            DisplayErrorMessage();
            return View(tblTemplate);
        }

        // GET: Template/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTemplate tblTemplate = db.tblTemplates.Find(id);
            if (tblTemplate == null)
            {
                return HttpNotFound();
            }
            return View(tblTemplate);
        }

        // POST: Template/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TemplateID,TemplateName,ParameterCount,Separator,tblResources,tblTemplateParameters")] tblTemplate tblTemplate)
        {
            string query = "update tblTemplate set TemplateName = '" + tblTemplate.TemplateName + "' where  TemplateID = " + tblTemplate.TemplateID + "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(query, conn);
                comm.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("Index");
        }

        // GET: Template/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTemplate tblTemplate = db.tblTemplates.Find(id);
            if (tblTemplate == null)
            {
                return HttpNotFound();
            }
            return View(tblTemplate);
        }

        // POST: Template/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblTemplate tblTemplate = db.tblTemplates.Find(id);
            //tblResource tblresource = db.tblResources.Where(item => item.TemplateID == id);
            //IQueryable<tblResource> tblres = (from p in db.tblResources where p.TemplateID == id select p).AsQueryable();
            //db.tblResources.Remove(tblres);
            var res = db.tblResources.Where(item => item.TemplateID == id);

            foreach (var reso in res)
            {
                db.tblResources.Remove(reso);
            }

            db.tblTemplates.Remove(tblTemplate);
            db.SaveChanges();
            DisplaySuccessMessage("Has delete a tblTemplate record");
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
