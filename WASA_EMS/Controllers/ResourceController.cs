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
    public class ResourceController : Controller
    {
        WASA_EMS_Entities db = new WASA_EMS_Entities();

        // GET: Resource/Index
        public ActionResult Index()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            var tblResource = db.tblResources.Include(t => t.tblTemplate).Where(item => item.CompanyID == c_id);
            return View(tblResource.ToList());
        }

        /*
        // GET: Resource/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblResource tblResource = db.tblResources.Find(id);
            if (tblResource == null)
            {
                return HttpNotFound();
            }
            return View(tblResource);
        }
        */

        // GET: Resource/Create
        public ActionResult Create()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            //var tempID = db.tblTemplates.Where(a => a.CompanyID == Convert.ToInt32(Session["CompanyID"]));
            //ViewBag.TemplateID = new SelectList(tempID, "Account_Code", "Account_Desc");
            ViewBag.TemplateID = new SelectList(db.tblTemplates.OrderBy(s => s.TemplateName).Where(item => item.CompanyID == c_id), "TemplateID", "TemplateName");
            return View();
        }

        // POST: Resource/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResourceID,MobileNumber,ResourceLocation,TemplateID,CooridatesGoogle")] tblResource tblresource)
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            var item = Request.Form["TemplateID"];
            int ddlTemplateValue;
            if (item == null)
            {
                ddlTemplateValue = 0;
            }
            else
            {
                ddlTemplateValue = Convert.ToInt32(Request.Form["TemplateID"].ToString());
            }

            string location = Request.Form["ResourceLocation"].ToString();
            string number = Request.Form["MobileNumber"].ToString();
            string coor = Request.Form["CooridatesGoogle"].ToString();

            if (location.ToString() == "" || number.ToString() == "" || ddlTemplateValue == 0)
            {
                TempData["notice"] = "Please insert all information";
                DisplaySuccessMessage("Please insert all information");
                return RedirectToAction("Create");
            }

            else
            {
                string query = "insert into tblResource (MobileNumber ,ResourceLocation,TemplateID,CompanyID,CooridatesGoogle) values (";
                query += " '" + number + "' ,";
                query += " '" + location + "' ,";
                query += " " + ddlTemplateValue + " ,";
                query += " " + c_id + ", ";
                query += " '" + coor + "' )";

                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {

                    try
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                    }

                }
                return RedirectToAction("Index");
            }





            //if (ModelState.IsValid)
            //{
            //    db.tblResources.Add(tblresource);
            //    db.SaveChanges();
            //    DisplaySuccessMessage("Has append a tblResource record");
            //    return RedirectToAction("Index");
            //}

            //ViewBag.TemplateID = new SelectList(db.tblTemplates, "TemplateID", "TemplateName", tblresource.TemplateID);
            //DisplayErrorMessage();
            //return View(tblresource);
        }

        // GET: Resource/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblResource tblresource = db.tblResources.Find(id);
            if (tblresource == null)
            {
                return HttpNotFound();
            }
            ViewBag.TemplateID = new SelectList(db.tblTemplates, "TemplateID", "TemplateName", tblresource.TemplateID);
            return View(tblresource);
        }

        // POST: Resource/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResourceID,MobileNumber,ResourceLocation,TemplateID,tblEnergies,tblTemplate,CooridatesGoogle")] tblResource tblresource)
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            if (ModelState.IsValid)
            {
                tblresource.CompanyID = c_id;
                db.Entry(tblresource).State = EntityState.Modified;
                db.SaveChanges();
                DisplaySuccessMessage("Has update a tblResource record");
                return RedirectToAction("Index");
            }
            ViewBag.TemplateID = new SelectList(db.tblTemplates, "TemplateID", "TemplateName", tblresource.TemplateID);
            DisplayErrorMessage();
            return View(tblresource);
        }

        // GET: Resource/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblResource tblResource = db.tblResources.Find(id);
            if (tblResource == null)
            {
                return HttpNotFound();
            }
            return View(tblResource);
        }

        // POST: Resource/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblResource tblresource = db.tblResources.Find(id);
            db.tblResources.Remove(tblresource);
            db.SaveChanges();
            DisplaySuccessMessage("Has delete a tblResource record");
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
