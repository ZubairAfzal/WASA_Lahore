using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WASA_EMS.Models;

namespace WASA_EMS.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LogOn()
        {

            if (!string.IsNullOrEmpty(Session["CompanyID"] as string))
            {
                return RedirectToAction("Welcome", "Home");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult LogOn(string UserName, string Password)
        {
            
            WASA_EMS_Entities ems_db = new WASA_EMS_Entities();
    
            if (UserName != null || Password != null)
            {
                User User = ems_db.Users.SingleOrDefault(item => item.UserName.Equals(UserName.Trim()));
                //IQueryable<User> user = from User in ems_db.Users where User.UserName.Equals(userName) select User;
                if (User != null)
                {
                    if (User.UserPassword.Trim().Equals(Password.Trim()))
                    {
                        var userLogged = (from u in ems_db.Users where u.UserName == UserName select u).FirstOrDefault();
                        int CompanyID = Convert.ToInt32(userLogged.CompanyID);
                        Session["UserName"] = User.UserName;
                        Session["CompanyID"] = CompanyID;
                        Session["UserID"] = User.UserID;
                        return RedirectToAction("Welcome", "Home");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                ViewBag.Login = false;
                ViewBag.message = "The user name or password provided is incorrect.";
                ViewBag.messageType = "error";
                //return Content("false");
                return View();
            }
            return View();
        }
        public ActionResult LogOff()
        {
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("LogOn", "Account");
        }
        [HttpGet]
        public ActionResult changePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult changePassword(FormCollection form)
        {
            string username = Session["UserName"].ToString().Trim();
            int companyID = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            User u = db.Users.Where(user => user.UserName == username).SingleOrDefault();
            string currentPass = u.UserPassword.ToString().Trim();
            int c_ID = Convert.ToInt32(u.CompanyID);
            if (form["currentPassword"].ToString() == currentPass)
            {
                u.UserPassword = form["newPassword"];
                db.SaveChanges();
                return RedirectToAction("LogOn");
            }
            else
            {
                return View();
            }

        }
    }
}