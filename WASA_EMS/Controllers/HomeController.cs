using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using WASA_EMS.Models;

namespace WASA_EMS.Controllers
{
    public class HomeController : Controller
    {
        public int pCounter;
        public int rCounter;
        public int tCounter;
        public int tpCounter;

        public ActionResult Schedule()
        {
            string query1from = "select TimeFrom from tblTubewellSchedule where slotNumber = 1 and ResourceID = 1068";
            string query1to = "select TimeTo from tblTubewellSchedule where slotNumber = 1 and ResourceID = 1068";
            string query2from = "select TimeFrom from tblTubewellSchedule where slotNumber = 2 and ResourceID = 1068";
            string query2to = "select TimeTo from tblTubewellSchedule where slotNumber = 2 and ResourceID = 1068";
            string query3from = "select TimeFrom from tblTubewellSchedule where slotNumber = 3 and ResourceID = 1068";
            string query3to = "select TimeTo from tblTubewellSchedule where slotNumber = 3 and ResourceID = 1068";
            string query4from = "select TimeFrom from tblTubewellSchedule where slotNumber = 4 and ResourceID = 1068";
            string query4to = "select TimeTo from tblTubewellSchedule where slotNumber = 4 and ResourceID = 1068";
            string time1from = "";
            string time1to = "";
            string time2from = "";
            string time2to = "";
            string time3from = "";
            string time3to = "";
            string time4from = "";
            string time4to = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd1from = new SqlCommand(query1from, conn);
                    SqlCommand cmd1to = new SqlCommand(query1to, conn);
                    SqlCommand cmd2from = new SqlCommand(query2from, conn);
                    SqlCommand cmd2to = new SqlCommand(query2to, conn);
                    SqlCommand cmd3from = new SqlCommand(query3from, conn);
                    SqlCommand cmd3to = new SqlCommand(query3to, conn);
                    SqlCommand cmd4from = new SqlCommand(query4from, conn);
                    SqlCommand cmd4to = new SqlCommand(query4to, conn);
                    time1from = cmd1from.ExecuteScalar().ToString();
                    time1to = cmd1to.ExecuteScalar().ToString();
                    time2from = cmd2from.ExecuteScalar().ToString();
                    time2to = cmd2to.ExecuteScalar().ToString();
                    time3from = cmd3from.ExecuteScalar().ToString();
                    time3to = cmd3to.ExecuteScalar().ToString();
                    time4from = cmd4from.ExecuteScalar().ToString();
                    time4to = cmd4to.ExecuteScalar().ToString();
                    conn.Close();
                    if (time1from.Length == 1)
                    {
                        time1from = "0" + time1from + ":00";
                    }
                    if (time1to.Length == 1)
                    {
                        time1to = "0" + time1to + ":00";
                    }
                    if (time2from.Length == 1)
                    {
                        time2from = "0" + time2from + ":00";
                    }
                    if (time2to.Length == 1)
                    {
                        time2to = "0" + time2to + ":00";
                    }
                    if (time3from.Length == 1)
                    {
                        time3from = "0" + time3from + ":00";
                    }
                    if (time3to.Length == 1)
                    {
                        time3to = "0" + time3to + ":00";
                    }
                    if (time4from.Length == 1)
                    {
                        time4from = "0" + time4from + ":00";
                    }
                    if (time4to.Length == 1)
                    {
                        time4to = "0" + time4to + ":00";
                    }
                    if (time1from.Length == 2)
                    {
                        time1from = time1from + ":00";
                    }
                    if (time1to.Length == 2)
                    {
                        time1to = time1to + ":00";
                    }
                    if (time2from.Length == 2)
                    {
                        time2from = time2from + ":00";
                    }
                    if (time2to.Length == 2)
                    {
                        time2to = time2to + ":00";
                    }
                    if (time3from.Length == 2)
                    {
                        time3from = time3from + ":00";
                    }
                    if (time3to.Length == 2)
                    {
                        time3to = time3to + ":00";
                    }
                    if (time4from.Length == 2)
                    {
                        time4from = time4from + ":00";
                    }
                    if (time4to.Length == 2)
                    {
                        time4to = time4to + ":00";
                    }
                }
                catch (Exception ex)
                {
                     time1from = "00:00";
                     time1to = "00:00";
                     time2from = "00:00";
                     time2to = "00:00";
                     time3from = "00:00";
                     time3to = "00:00";
                     time4from = "00:00";
                     time4to = "00:00";
                }
            }
            ViewBag.time1from = time1from;
            ViewBag.time1to = time1to;
            ViewBag.time2from = time2from;
            ViewBag.time2to = time2to;
            ViewBag.time3from = time3from;
            ViewBag.time3to = time3to;
            ViewBag.time4from = time4from;
            ViewBag.time4to = time4to;
            IList<string> ResourceList = new List<string>();
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.TemplateID == 64))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            ResourceList.Add("Update for All Locations");
            return View();
        }
        [HttpPost]
        public ActionResult ResetSchedule()
        {
            string query1from = "";
            string query1to = "";
            
            query1from = "update tblTubewellSchedule set TimeFrom = '00:00' ";
            query1to = "update tblTubewellSchedule set TimeTo = '00:00' ";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd1from = new SqlCommand(query1from, conn);
                    SqlCommand cmd1to = new SqlCommand(query1to, conn);
                    cmd1from.ExecuteNonQuery();
                    cmd1to.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                }
            }
            return RedirectToAction("Schedule");
        }
        [HttpPost]
        public ActionResult Schedule(FormCollection review)
        {
            string resource = review["resource"].ToString();
            string time1from = review["time1from"].ToString();
            string time1to = review["time1to"].ToString();
            string time2from = review["time2from"].ToString();
            string time2to = review["time2to"].ToString();
            string time3from = review["time3from"].ToString();
            string time3to = review["time3to"].ToString();
            string time4from = review["time4from"].ToString();
            string time4to = review["time4to"].ToString();
            string query1from = "";
            string query1to = "";
            string query2from = "";
            string query2to = "";
            string query3from = "";
            string query3to = "";
            string query4from = "";
            string query4to = "";
            if (resource == "Update for All Locations")
            {
                query1from = "update tblTubewellSchedule set TimeFrom = '" + time1from + "' where slotNumber = 1";
                query1to = "update tblTubewellSchedule set TimeTo = '" + time1to + "' where slotNumber = 1 ";
                query2from = "update tblTubewellSchedule set TimeFrom = '" + time2from + "' where slotNumber = 2 ";
                query2to = "update tblTubewellSchedule set TimeTo = '" + time2to + "' where slotNumber = 2 ";
                query3from = "update tblTubewellSchedule set TimeFrom = '" + time3from + "' where slotNumber = 3 ";
                query3to = "update tblTubewellSchedule set TimeTo = '" + time3to + "' where slotNumber = 3 ";
                query4from = "update tblTubewellSchedule set TimeFrom = '" + time4from + "' where slotNumber = 4 ";
                query4to = "update tblTubewellSchedule set TimeTo = '" + time4to + "' where slotNumber = 4 ";
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd1from = new SqlCommand(query1from, conn);
                        SqlCommand cmd1to = new SqlCommand(query1to, conn);
                        SqlCommand cmd2from = new SqlCommand(query2from, conn);
                        SqlCommand cmd2to = new SqlCommand(query2to, conn);
                        SqlCommand cmd3from = new SqlCommand(query3from, conn);
                        SqlCommand cmd3to = new SqlCommand(query3to, conn);
                        SqlCommand cmd4from = new SqlCommand(query4from, conn);
                        SqlCommand cmd4to = new SqlCommand(query4to, conn);
                        cmd1from.ExecuteNonQuery();
                        cmd1to.ExecuteNonQuery();
                        cmd2from.ExecuteNonQuery();
                        cmd2to.ExecuteNonQuery();
                        cmd3from.ExecuteNonQuery();
                        cmd3to.ExecuteNonQuery();
                        cmd4from.ExecuteNonQuery();
                        cmd4to.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        time1from = "00:00";
                        time1to = "00:00";
                        time2from = "00:00";
                        time2to = "00:00";
                        time3from = "00:00";
                        time3to = "00:00";
                        time4from = "00:00";
                        time4to = "00:00";
                    }
                }
            }
            else
            {
                query1from = "update tblTubewellSchedule set TimeFrom = '" + time1from + "' where slotNumber = 1 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
                query1to = "update tblTubewellSchedule set TimeTo = '" + time1to + "' where slotNumber = 1 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
                query2from = "update tblTubewellSchedule set TimeFrom = '" + time2from + "' where slotNumber = 2 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
                query2to = "update tblTubewellSchedule set TimeTo = '" + time2to + "' where slotNumber = 2 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
                query3from = "update tblTubewellSchedule set TimeFrom = '" + time3from + "' where slotNumber = 3 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
                query3to = "update tblTubewellSchedule set TimeTo = '" + time3to + "' where slotNumber = 3 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
                query4from = "update tblTubewellSchedule set TimeFrom = '" + time4from + "' where slotNumber = 4 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
                query4to = "update tblTubewellSchedule set TimeTo = '" + time4to + "' where slotNumber = 4 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd1from = new SqlCommand(query1from, conn);
                        SqlCommand cmd1to = new SqlCommand(query1to, conn);
                        SqlCommand cmd2from = new SqlCommand(query2from, conn);
                        SqlCommand cmd2to = new SqlCommand(query2to, conn);
                        SqlCommand cmd3from = new SqlCommand(query3from, conn);
                        SqlCommand cmd3to = new SqlCommand(query3to, conn);
                        SqlCommand cmd4from = new SqlCommand(query4from, conn);
                        SqlCommand cmd4to = new SqlCommand(query4to, conn);
                        cmd1from.ExecuteNonQuery();
                        cmd1to.ExecuteNonQuery();
                        cmd2from.ExecuteNonQuery();
                        cmd2to.ExecuteNonQuery();
                        cmd3from.ExecuteNonQuery();
                        cmd3to.ExecuteNonQuery();
                        cmd4from.ExecuteNonQuery();
                        cmd4to.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        time1from = "00:00";
                        time1to = "00:00";
                        time2from = "00:00";
                        time2to = "00:00";
                        time3from = "00:00";
                        time3to = "00:00";
                        time4from = "00:00";
                        time4to = "00:00";
                    }
                }
            }
            if (resource == "Update for All Locations") { 
                resource = "C-II Block Johar Town";
            }
            query1from = "select TimeFrom from tblTubewellSchedule where slotNumber = 1 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
            query1to = "select TimeTo from tblTubewellSchedule where slotNumber = 1 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
            query2from = "select TimeFrom from tblTubewellSchedule where slotNumber = 2 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
            query2to = "select TimeTo from tblTubewellSchedule where slotNumber = 2 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
            query3from = "select TimeFrom from tblTubewellSchedule where slotNumber = 3 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
            query3to = "select TimeTo from tblTubewellSchedule where slotNumber = 3 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
            query4from = "select TimeFrom from tblTubewellSchedule where slotNumber = 4 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
            query4to = "select TimeTo from tblTubewellSchedule where slotNumber = 4 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "')";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd1from = new SqlCommand(query1from, conn);
                    SqlCommand cmd1to = new SqlCommand(query1to, conn);
                    SqlCommand cmd2from = new SqlCommand(query2from, conn);
                    SqlCommand cmd2to = new SqlCommand(query2to, conn);
                    SqlCommand cmd3from = new SqlCommand(query3from, conn);
                    SqlCommand cmd3to = new SqlCommand(query3to, conn);
                    SqlCommand cmd4from = new SqlCommand(query4from, conn);
                    SqlCommand cmd4to = new SqlCommand(query4to, conn);
                    time1from = cmd1from.ExecuteScalar().ToString();
                    time1to = cmd1to.ExecuteScalar().ToString();
                    time2from = cmd2from.ExecuteScalar().ToString();
                    time2to = cmd2to.ExecuteScalar().ToString();
                    time3from = cmd3from.ExecuteScalar().ToString();
                    time3to = cmd3to.ExecuteScalar().ToString();
                    time4from = cmd4from.ExecuteScalar().ToString();
                    time4to = cmd4to.ExecuteScalar().ToString();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    time1from = "00:00";
                    time1to = "00:00";
                    time2from = "00:00";
                    time2to = "00:00";
                    time3from = "00:00";
                    time3to = "00:00";
                    time4from = "00:00";
                    time4to = "00:00";
                }
            }
            ViewBag.time1from = time1from;
            ViewBag.time1to = time1to;
            ViewBag.time2from = time2from;
            ViewBag.time2to = time2to;
            ViewBag.time3from = time3from;
            ViewBag.time3to = time3to;
            ViewBag.time4from = time4from;
            ViewBag.time4to = time4to;
            IList<string> ResourceList = new List<string>();
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.TemplateID == 64))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            ResourceList.Add("Update for All Locations");
            ViewBag.SelectedResource = resource;
            return View();
        }
        // GET: Home
        public ActionResult TestJs()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Overview()
        {
            return View();
        }
        public ActionResult SampleMap()
        {
            return View();
        }
        public ActionResult Welcome()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            string tempName = "";
            string query = "select TemplateID, TemplateName from tblTemplate where CompanyID = " + c_id + "";
            query += " and TemplateID in (select TemplateID from tblResource) ";
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con1;
                    con1.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["TemplateName"].ToString(),
                                Value = sdr["TemplateID"].ToString()
                            });
                        }
                    }
                    con1.Close();
                }
            }
            string parameterValuesString = "";
            string datetimed = "";
            string markers = "[";
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            string q = "select r.ResourceLocation, r.ResourceID, r.CooridatesGoogle, t.TemplateName from tblResource r left join tblTemplate t on r.TemplateID = t.TemplateID  where  t.TemplateID = 64 and t.CompanyID = " + c_id + "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(q, conn);
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            DataTable dt = new DataTable();
                            string q1 = "select distinct r.ResourceLocation,  t.TemplateName, p.ParameterName, p.paramOrder, e.ParameterValue, e.InsertionDateTime, rms.CurrentMotorOnOffStatus from tblEnergy e ";
                            q1 += "left join tblParameter p on e.ParameterID = p.ParameterID ";
                            q1 += "left join tblResource r on e.ResourceID = r.ResourceID ";
                            q1 += "left join tblRemoteSensor rms on r.ResourceID = rms.ResourceID ";
                            q1 += "left join tblTemplate t on r.TemplateID = t.TemplateID ";
                            q1 += "where e.InsertionDateTime = (select max(InsertionDateTime) from tblEnergy where ResourceID = " + sdr["ResourceID"] + ")";
                            q1 += "and r.ResourceID = " + sdr["ResourceID"] + "";
                            q1 += " and t.TemplateID = 64 order by p.paramOrder";
                            SqlCommand cmd1 = new SqlCommand(q1, conn);
                            SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                            dt.Clear();
                            sda.Fill(dt);
                            int optionStatus = 0;
                            using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                            {
                                while (sdr1.Read())
                                {
                                    string valuee = "";
                                    parameterValuesString += "";
                                    if (sdr1["ParameterName"].ToString() == "V1N.")
                                    {
                                        parameterValuesString += "V1N : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "V2N.")
                                    {
                                        parameterValuesString += "V2N : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "V3N.")
                                    {
                                        parameterValuesString += "V3N : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "I1.")
                                    {
                                        parameterValuesString += "I1 : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "I2.")
                                    {
                                        parameterValuesString += "I2 : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "I3.")
                                    {
                                        parameterValuesString += "I3 : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "Frequency.")
                                    {
                                        parameterValuesString += "Frequency : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PKVA.")
                                    {
                                        parameterValuesString += "PKVA : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PF.")
                                    {
                                        parameterValuesString += "Power Factor : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "Remote.")
                                    {
                                        parameterValuesString += "Remote Mode : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus")
                                    {
                                        parameterValuesString += "Pump Status : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "CurrentTrip.")
                                    {
                                        parameterValuesString += "Current Trip : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "VoltageTrip.")
                                    {
                                        parameterValuesString += "Voltage Trip : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "TimeSchedule.")
                                    {
                                        parameterValuesString += "Scheduling Mode : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "ChlorineLevel.")
                                    {
                                        parameterValuesString += "Chlorine Level : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "WaterFlow(Cusec).")
                                    {
                                        parameterValuesString += "Water Flow(m3/h) : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PKVAR.")
                                    {
                                        parameterValuesString += "PKVAR : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PKW.")
                                    {
                                        parameterValuesString += "PKW : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "V12")
                                    {
                                        parameterValuesString += "V12 : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "V13")
                                    {
                                        parameterValuesString += "V13 : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "V23")
                                    {
                                        parameterValuesString += "V23 : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PrimingLevel")
                                    {
                                        parameterValuesString += "Priming Level : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "Pressure(Bar)")
                                    {
                                        parameterValuesString += "Pressure(Bar) : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "Manual")
                                    {
                                        parameterValuesString += "Manual Mode : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "IndoorLight")
                                    {
                                        parameterValuesString += "Indoor Lights : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "OutdoorLight")
                                    {
                                        parameterValuesString += "Outdoor Lights : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "Exhaust Fan")
                                    {
                                        parameterValuesString += "Exhaust Fan : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus1.")
                                    {
                                        parameterValuesString += "Pump 1 Status : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus2.")
                                    {
                                        parameterValuesString += "Pump 2 Status : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus3.")
                                    {
                                        parameterValuesString += "Pump 3 Status : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus4.")
                                    {
                                        parameterValuesString += "Pump 4 Status : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus5.")
                                    {
                                        parameterValuesString += "Pump 5 Status : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "Well1Level(ft)")
                                    {
                                        parameterValuesString += "Well 1 Level (ft) : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus6.")
                                    {
                                        parameterValuesString += "Pump 6 Status : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus7.")
                                    {
                                        parameterValuesString += "Pump 7 Status : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus8.")
                                    {
                                        parameterValuesString += "Pump 8 Status : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus9.")
                                    {
                                        parameterValuesString += "Pump 9 Status : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatuss10.")
                                    {
                                        parameterValuesString += "Pump 10 Status : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "Well2Level(ft)")
                                    {
                                        parameterValuesString += "Well 2 Level (ft) : ";
                                    }
                                    else
                                    {
                                        parameterValuesString += sdr1["ParameterName"].ToString() + ": ";
                                    }
                                    //parameterValuesString += ssdr1["ParameterName"].ToString() + ": ";
                                    if (sdr1["ParameterName"].ToString() == "AutoModeOn")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus1.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus2.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus3.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus4.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus5.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus6.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus7.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus8.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus9.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatuss10.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "CurrentTrip.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "No Error";
                                        }
                                        else
                                        {
                                            valuee = "Error";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "VoltageTrip.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "No Error";
                                        }
                                        else
                                        {
                                            valuee = "Error";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "Remote.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "TimeSchedule.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "ChlorineLevel.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "LOW";
                                        }
                                        else
                                        {
                                            valuee = "FULL";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PrimingLevel")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "LOW";
                                        }
                                        else
                                        {
                                            valuee = "FULL";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "Manual")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "IndoorLight")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "OutdoorLight")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "Exhaust Fan")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                        }
                                    }
                                    else
                                    {
                                        valuee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    parameterValuesString += valuee;
                                    parameterValuesString += "<br />";
                                    datetimed = sdr1["InsertionDateTime"].ToString();
                                    //optionStatus = sdr1["CurrentMotorOnOffStatus"].ToString();
                                }
                            }
                            string theStatus = "False";
                            if (optionStatus > 0)
                            {
                                theStatus = "True";
                            }
                            tempName = sdr["TemplateName"].ToString().Substring(0, 1);
                            string newstring = "<b>" + sdr["TemplateName"].ToString() + "</b>";
                            newstring += "<br />";
                            newstring += "<b>" + sdr["ResourceLocation"].ToString() + "</b>";
                            newstring += "<br />";
                            newstring += datetimed;
                            newstring += "<br />";
                            newstring += parameterValuesString;
                            TimeSpan duration = (Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString()) - Convert.ToDateTime(datetimed.ToString()));
                            double minu = duration.TotalMinutes;
                            parameterValuesString = "";
                            markers += "{";
                            markers += string.Format("'Status': '{0}',", theStatus);
                            markers += string.Format("'Template': '{0}',", tempName);
                            markers += string.Format("'title': '{0}',", sdr["ResourceLocation"]);
                            //markers += string.Format("'time': '{0}',", sdr1["InsertionDateTime"]);
                            markers += string.Format("'lat':'{0}',", sdr["CooridatesGoogle"].ToString().Split(',')[0]);
                            markers += string.Format("'lnt':'{0}',", sdr["CooridatesGoogle"].ToString().Split(',')[1]);
                            //markers += string.Format("'parameter':'{0}',", sdr1["ParameterName"]);
                            //markers += string.Format("'value':'{0}'", sdr1["ParameterValue"]);
                            markers += string.Format("'DelTime': '{0}',", minu);
                            markers += string.Format("'description': '{0}'", newstring);
                            markers += "},";
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
                //return new SelectList(theResourceTypes, "Value", "Text", "id");
            }

            string p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, ptime, plevel1,plevel2 = "";
            int pp1= 0;
            int pp2 = 0;
            int pp3 = 0;
            int pp4 = 0;
            int pp5 = 0;
            int pp6 = 0;
            int pp7 = 0;
            int pp8 = 0;
            int pp9 = 0;
            int pp10 = 0;
            double tlevel1 = 0;
            double tlevel2 = 0;
            p1 = "select top(1) ParameterValue from tblEnergy where ParameterID = 144 order by ID DESC";
            p2 = "select top(1) ParameterValue from tblEnergy where ParameterID = 145 order by ID DESC";
            p3 = "select top(1) ParameterValue from tblEnergy where ParameterID = 146 order by ID DESC";
            p4 = "select top(1) ParameterValue from tblEnergy where ParameterID = 147 order by ID DESC";
            p5 = "select top(1) ParameterValue from tblEnergy where ParameterID = 148 order by ID DESC";
            p6 = "select top(1) ParameterValue from tblEnergy where ParameterID = 149 order by ID DESC";
            p7 = "select top(1) ParameterValue from tblEnergy where ParameterID = 150 order by ID DESC";
            p8 = "select top(1) ParameterValue from tblEnergy where ParameterID = 151 order by ID DESC";
            p9 = "select top(1) ParameterValue from tblEnergy where ParameterID = 152 order by ID DESC";
            p10 = "select top(1) ParameterValue from tblEnergy where ParameterID = 153 order by ID DESC";
            ptime = "select top(1) InsertionDateTime from tblEnergy where ParameterID = 153 order by ID DESC";
            plevel1 = "select top(1) ParameterValue from tblEnergy where ParameterID = 175 order by ID DESC";
            plevel2 = "select top(1) ParameterValue from tblEnergy where ParameterID = 176 order by ID DESC";

            //////////////////////////////////////////////////////////////////////////////////////////////////
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd1 = new SqlCommand(p1, conn);
                    SqlCommand cmd2 = new SqlCommand(p2, conn);
                    SqlCommand cmd3 = new SqlCommand(p3, conn);
                    SqlCommand cmd4 = new SqlCommand(p4, conn);
                    SqlCommand cmd5 = new SqlCommand(p5, conn);
                    SqlCommand cmd6 = new SqlCommand(p6, conn);
                    SqlCommand cmd7 = new SqlCommand(p7, conn);
                    SqlCommand cmd8 = new SqlCommand(p8, conn);
                    SqlCommand cmd9 = new SqlCommand(p9, conn);
                    SqlCommand cmd10 = new SqlCommand(p10, conn);
                    SqlCommand cmdtime = new SqlCommand(ptime, conn);
                    SqlCommand cmdlevel1 = new SqlCommand(plevel1, conn);
                    SqlCommand cmdlevel2 = new SqlCommand(plevel2, conn);
                    pp1 = Convert.ToInt32(cmd1.ExecuteScalar());
                    pp2 = Convert.ToInt32(cmd2.ExecuteScalar());
                    pp3 = Convert.ToInt32(cmd3.ExecuteScalar());
                    pp4 = Convert.ToInt32(cmd4.ExecuteScalar());
                    pp5 = Convert.ToInt32(cmd5.ExecuteScalar());
                    pp6 = Convert.ToInt32(cmd6.ExecuteScalar());
                    pp7 = Convert.ToInt32(cmd7.ExecuteScalar());
                    pp8 = Convert.ToInt32(cmd8.ExecuteScalar());
                    pp9 = Convert.ToInt32(cmd9.ExecuteScalar());
                    pp10 = Convert.ToInt32(cmd10.ExecuteScalar());
                    ptime = cmdtime.ExecuteScalar().ToString();
                    tlevel1 = Convert.ToDouble(cmdlevel1.ExecuteScalar());
                    tlevel2 = Convert.ToDouble(cmdlevel2.ExecuteScalar());
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
                if (tlevel1 == 0)
                {
                    plevel1 = "Empty";
                }
                else
                {
                    plevel1 = Math.Round(tlevel1, 1).ToString() + " ft";
                }
                if (tlevel2 == 0)
                {
                    plevel2 = "Empty";
                }
                else
                {
                    plevel2 = Math.Round(tlevel2, 1).ToString() + " ft";
                }
                if (pp1 == 0)
                {
                    p1 = "OFF";
                }
                else
                {
                    p1 = "ON";
                }
                if (pp2 == 0)
                {
                    p2 = "OFF";
                }
                else
                {
                    p2 = "ON";
                }
                if (pp3 == 0)
                {
                    p3 = "OFF";
                }
                else
                {
                    p3 = "ON";
                }
                if (pp4 == 0)
                {
                    p4 = "OFF";
                }
                else
                {
                    p4 = "ON";
                }
                if (pp5 == 0)
                {
                    p5 = "OFF";
                }
                else
                {
                    p5 = "ON";
                }
                if (pp6 == 0)
                {
                    p6 = "OFF";
                }
                else
                {
                    p6 = "ON";
                }
                if (pp7 == 0)
                {
                    p7 = "OFF";
                }
                else
                {
                    p7 = "ON";
                }
                if (pp8 == 0)
                {
                    p8 = "OFF";
                }
                else
                {
                    p8 = "ON";
                }
                if (pp9 == 0)
                {
                    p9 = "OFF";
                }
                else
                {
                    p9 = "ON";
                }
                if (pp10 == 0)
                {
                    p10 = "OFF";
                }
                else
                {
                    p10 = "ON";
                }
                //return new SelectList(theResourceTypes, "Value", "Text", "id");
            }
            string theStatusp = "False";
            if (pp1 + pp2 + pp3 + pp4 + pp5 + pp6 + pp7 + pp8 + pp9 + pp10 > 0)
            {
                theStatusp = "True";
            }
            
            parameterValuesString = "Pump 1 : "+p1;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump 2 : " + p2;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump 3 : " + p3;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump 4 : " + p4;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump 5 : " + p5;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump 6 : " + p6;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump 7 : " + p7;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump 8 : " + p8;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump 9 : " + p9;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump 10 : " + p10;
            parameterValuesString += "<br />";
            parameterValuesString += "Well 1 Level : " + plevel1;
            parameterValuesString += "<br />";
            parameterValuesString += "Well 2 Level : " + plevel2;
            parameterValuesString += "<br />";

            datetimed = ptime;

            tempName = "D";
            string newstringp = "<b>Disposal Station</b>";
            newstringp += "<br />";
            newstringp += "<b>Shaukat Khanum Disposal Station</b>";
            newstringp += "<br />";
            newstringp += datetimed;
            newstringp += "<br />";
            newstringp += parameterValuesString;
            TimeSpan durationp = (Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString()) - Convert.ToDateTime(datetimed.ToString()));
            double minup = durationp.TotalMinutes;
            parameterValuesString = "";
            markers += "{";
            markers += string.Format("'Status': '{0}',", theStatusp);
            markers += string.Format("'Template': '{0}',", tempName);
            markers += string.Format("'title': '{0}',", "Shaukat Khanum Disposal Station");
            //markers += string.Format("'time': '{0}',", sdr1["InsertionDateTime"]);
            markers += string.Format("'lat':'{0}',", "31.430021" );
            markers += string.Format("'lnt':'{0}',", "74.252829");
            markers += string.Format("'DelTime': '{0}',", minup);
            markers += string.Format("'description': '{0}'", newstringp);
            markers += "},";

            //////////////////////////////////////////////////////////////////////////////////////////////////
            markers += "]";
            ViewBag.Markers = markers;
            ViewBag.parameterList = db.tblParameters.AsEnumerable().Where(item => item.CompanyID == c_id);
            ViewBag.resourceList = db.tblResources.AsEnumerable().Where(item => item.CompanyID == c_id);
            return View();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        public PartialViewResult _Welcome()
        {
            int totalRunningTubewells = 0;
            int totalRunningDisposals = 0;
            int InactiveTubewells = 0;
            int InactiveDisposals = 0;
            string queryT1 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1068 and ParameterID = 125 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryT2 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1069 and ParameterID = 125 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryT3 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1070 and ParameterID = 125 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryT4 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1071 and ParameterID = 125 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryT5 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1072 and ParameterID = 125 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryT6 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1073 and ParameterID = 125 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryT7 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1074 and ParameterID = 125 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryD1p1 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1059 and ParameterID = 144 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryD1p2 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1059 and ParameterID = 145 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryD1p3 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1059 and ParameterID = 146 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryD1p4 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1059 and ParameterID = 147 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryD1p5 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1059 and ParameterID = 148 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryD2p1 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1060 and ParameterID = 149 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryD2p2 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1060 and ParameterID = 150 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryD2p3 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1060 and ParameterID = 151 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryD2p4 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1060 and ParameterID = 152 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            string queryD2p5 = "select TOP(1) ParameterValue as pVal from tblEnergy where ResourceID = 1060 and ParameterID = 153 and InsertionDateTime > Dateadd(minute,520,getdate()) order by InsertionDateTime DESC";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                var cmdT1 = new SqlCommand(queryT1, conn);
                var cmdT2 = new SqlCommand(queryT2, conn);
                var cmdT3 = new SqlCommand(queryT3, conn);
                var cmdT4 = new SqlCommand(queryT4, conn);
                var cmdT5 = new SqlCommand(queryT5, conn);
                var cmdT6 = new SqlCommand(queryT6, conn);
                var cmdT7 = new SqlCommand(queryT7, conn);
                var cmdD1p1 = new SqlCommand(queryD1p1, conn);
                var cmdD1p2 = new SqlCommand(queryD1p2, conn);
                var cmdD1p3 = new SqlCommand(queryD1p3, conn);
                var cmdD1p4 = new SqlCommand(queryD1p4, conn);
                var cmdD1p5 = new SqlCommand(queryD1p5, conn);
                var cmdD2p1 = new SqlCommand(queryD2p1, conn);
                var cmdD2p2 = new SqlCommand(queryD2p2, conn);
                var cmdD2p3 = new SqlCommand(queryD2p3, conn);
                var cmdD2p4 = new SqlCommand(queryD2p4, conn);
                var cmdD2p5 = new SqlCommand(queryD2p5, conn);

                var t1Val = cmdT1.ExecuteScalar();
                if (t1Val == null)
                {

                }
                else
                {
                    totalRunningTubewells += Convert.ToInt32(t1Val);
                }

                var t2Val = cmdT2.ExecuteScalar();
                if (t2Val == null)
                {

                }
                else
                {
                    totalRunningTubewells += Convert.ToInt32(t2Val);
                }

                var t3Val = cmdT3.ExecuteScalar();
                if (t3Val == null)
                {

                }
                else
                {
                    totalRunningTubewells += Convert.ToInt32(t3Val);
                }

                var t4Val = cmdT4.ExecuteScalar();
                if (t4Val == null)
                {

                }
                else
                {
                    totalRunningTubewells += Convert.ToInt32(t4Val);
                }

                var t5Val = cmdT5.ExecuteScalar();
                if (t5Val == null)
                {

                }
                else
                {
                    totalRunningTubewells += Convert.ToInt32(t5Val);
                }

                var t6Val = cmdT6.ExecuteScalar();
                if (t6Val == null)
                {

                }
                else
                {
                    totalRunningTubewells += Convert.ToInt32(t6Val);
                }

                var t7Val = cmdT7.ExecuteScalar();
                if (t7Val == null)
                {

                }
                else
                {
                    totalRunningTubewells += Convert.ToInt32(t7Val);
                }



                var cD1p1 = cmdD1p1.ExecuteScalar();
                if (cD1p1 == null)
                {

                }
                else
                {
                    totalRunningDisposals += Convert.ToInt32(cD1p1);
                }

                var cD1p2 = cmdD1p2.ExecuteScalar();
                if (cD1p2 == null)
                {

                }
                else
                {
                    totalRunningDisposals += Convert.ToInt32(cD1p2);
                }

                var cD1p3 = cmdD1p3.ExecuteScalar();
                if (cD1p3 == null)
                {

                }
                else
                {
                    totalRunningDisposals += Convert.ToInt32(cD1p3);
                }

                var cD1p4 = cmdD1p4.ExecuteScalar();
                if (cD1p4 == null)
                {

                }
                else
                {
                    totalRunningDisposals += Convert.ToInt32(cD1p4);
                }

                var cD1p5 = cmdD1p5.ExecuteScalar();
                if (cD1p5 == null)
                {

                }
                else
                {
                    totalRunningDisposals += Convert.ToInt32(cD1p5);
                }


                var cD2p1 = cmdD2p1.ExecuteScalar();
                if (cD2p1 == null)
                {

                }
                else
                {
                    totalRunningDisposals += Convert.ToInt32(cD2p1);
                }

                var cD2p2 = cmdD2p2.ExecuteScalar();
                if (cD2p2 == null)
                {

                }
                else
                {
                    totalRunningDisposals += Convert.ToInt32(cD2p2);
                }

                var cD2p3 = cmdD2p3.ExecuteScalar();
                if (cD2p3 == null)
                {

                }
                else
                {
                    totalRunningDisposals += Convert.ToInt32(cD2p3);
                }

                var cD2p4 = cmdD2p4.ExecuteScalar();
                if (cD2p4 == null)
                {

                }
                else
                {
                    totalRunningDisposals += Convert.ToInt32(cD2p4);
                }

                var cD2p5 = cmdD2p5.ExecuteScalar();
                if (cD2p5 == null)
                {

                }
                else
                {
                    totalRunningDisposals += Convert.ToInt32(cD2p5);
                }
                conn.Close();
            }
            if (totalRunningDisposals > 1)
            {
                totalRunningDisposals = 1;
            }
            InactiveTubewells = 7 - totalRunningTubewells;
            InactiveDisposals = 1 - totalRunningDisposals;
            ViewBag.TotalRunningTubewells = totalRunningTubewells.ToString();
            ViewBag.TotalRunningDisposals = totalRunningDisposals.ToString();
            ViewBag.InactiveTubewells = InactiveTubewells.ToString();
            ViewBag.InactiveDisposals = InactiveDisposals.ToString();
            return PartialView();
        }
        public ActionResult Dashboard()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();

            IList<string> resourceTypeList = new List<string>();
            foreach (var item in db.tblTemplates.AsQueryable().Where(item => item.CompanyID == c_id))
            {
                resourceTypeList.Add(item.TemplateName);
            }
            resourceTypeList.Add("All");
            tCounter = resourceTypeList.Count;
            ViewBag.ResourceTypeList = resourceTypeList;

            IList<string> ResourceList = new List<string>();
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ResourceList.Add("All");
            rCounter = ResourceList.Count;
            ViewBag.ResourceList = ResourceList;
            return View();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        //[OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 2000)]
        public PartialViewResult _DashboardView()
        {
            return PartialView();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        //[OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 2000)]
        public PartialViewResult _TableRecords()
        {
            string getResID = "select ResourceID from tblResource where ResourceLocation = 'C-II Johar Town' ";
            int ResID = 0;
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                SqlCommand newConC1 = new SqlCommand(getResID, conn);
                ResID = Convert.ToInt32(newConC1.ExecuteScalar());
                conn.Close();
            }
            Random random = new Random();
            ////////////////////////////////////////////////////////////////////////////////////////////////
            var cId = Convert.ToInt32(Session["CompanyID"]);
            var dt = new DataTable();
            string query = ";with cteRowNumber as ( ";
            query += " select r.ResourceID, r.ResourceLocation, CONCAT(r.ResourceLocation, ' : ',e.InsertionDateTime) AS Location,p.ParameterID,p.ParameterName, e.CompanyID, e.ParameterValue, ";
            query += " row_number() over(partition by p.ParameterID, r.ResourceID,r.ResourceLocation order by e.ID desc) as RowNum";
            query += " from tblEnergy e";
            query += " inner join tblResource r on e.ResourceID = r.ResourceID";
            query += " inner join tblParameter p on e.ParameterID = p.ParameterID";
            query += " ) ";
            query += " select * ";
            query += " from cteRowNumber ";
            query += " where RowNum = 1  and CompanyID = " + cId + "";
            query += " and ResourceID = '" + ResID + "' ";
            query += " order by ResourceID,ParameterID ";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                var sda = new SqlDataAdapter(query, conn);
                sda.Fill(dt);
                conn.Close();
            }
            string location = dt.Rows[0][1].ToString();
            string time = dt.Rows[0][2].ToString().Split(':')[1] + ':' + dt.Rows[0][2].ToString().Split(':')[2];
            Pivot pvt = new Pivot(dt);
            DataTable pivotTable = new DataTable();
            pivotTable = pvt.PivotData("ParameterName", "ParameterValue", AggregateFunction.Sum, "Location");
            var db = new WASA_EMS_Entities();
            double V1N, V2N, V3N, I1, I2, I3, Frequency, PKVA, PF, AutoModeOn, PumpStatus, CurrentTrip, VoltageTrip, RemoteControl, ChlorineLevel, WaterExtracted, PKVAR, PKW, V12, V13, V23, PrimingLevel, averageVoltage, averageVoltageSecond, averageCurrent, TotalWorkingHour, Scheduling, Manual = 0;
            V1N = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "V1N." select row[1]).ElementAt(0)), 2);
            V2N = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "V2N." select row[1]).ElementAt(0)), 2);
            V3N = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "V3N." select row[1]).ElementAt(0)), 2);
            I1 = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "I1." select row[1]).ElementAt(0)), 2);
            I2 = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "I2." select row[1]).ElementAt(0)), 2);
            I3 = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "I3." select row[1]).ElementAt(0)), 2);
            Frequency = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "Frequency." select row[1]).ElementAt(0)), 2);
            PKVA = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "PKVA." select row[1]).ElementAt(0)), 2);
            PF = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "PF." select row[1]).ElementAt(0)), 2);
            AutoModeOn = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "Remote." select row[1]).ElementAt(0)), 2);
            PumpStatus = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "PumpStatus" select row[1]).ElementAt(0)), 2);
            CurrentTrip = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "CurrentTrip." select row[1]).ElementAt(0)), 2);
            VoltageTrip = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "VoltageTrip." select row[1]).ElementAt(0)), 2);
            RemoteControl = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "Remote." select row[1]).ElementAt(0)), 2);
            ChlorineLevel = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "ChlorineLevel." select row[1]).ElementAt(0)), 2);
            WaterExtracted = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "WaterFlow(Cusec)." select row[1]).ElementAt(0)), 2);
            Scheduling = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "TimeSchedule." select row[1]).ElementAt(0)), 2);
            Manual = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "Manual" select row[1]).ElementAt(0)), 2);
            //PKVAR, PKW, V12, V13, V23, PrimingLevel
            PKVAR = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "PKVAR." select row[1]).ElementAt(0)), 2);
            PKW = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "PKW." select row[1]).ElementAt(0)), 2);
            V12 = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "V12" select row[1]).ElementAt(0)), 2);
            V13 = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "V13" select row[1]).ElementAt(0)), 2);
            V23 = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "V23" select row[1]).ElementAt(0)), 2);
            PrimingLevel = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "PrimingLevel" select row[1]).ElementAt(0)), 2);
            //var abbb = (from row in pivotTable.AsEnumerable()
            //     where row.Field<string>("ParameterName") == "V1N."
            //           //&& row.Field<int>("B") == 2
            //           //&& row.Field<int>("C") == 3
            //     select row[1]).ElementAt(0);
            //averageCurrent = (Convert.ToDouble(I1) + Convert.ToDouble(I2) + Convert.ToDouble(I3)).ToString();
            averageCurrent = Math.Round((I1 + I2 + I3) / 3, 2);
            averageVoltage = Math.Round((V1N + V2N + V3N) / 3, 2);
            averageVoltageSecond = Math.Round((V12 + V13 + V23) / 3, 2);

            ViewBag.parameterList = db.tblParameters.AsEnumerable().Where(item => item.CompanyID == cId);
            ViewBag.resourceList = db.tblResources.AsEnumerable().Where(item => item.CompanyID == cId);
            ViewBag.V1N = V1N;
            ViewBag.V2N = V2N;
            ViewBag.V3N = V3N;
            ViewBag.I1 = I1;
            ViewBag.I2 = I2;
            ViewBag.I3 = I3;
            ViewBag.Frequency = Frequency;
            ViewBag.PKVA = PKVA;
            ViewBag.PF = PF;
            ViewBag.AutoModeOn = AutoModeOn;
            ViewBag.PumpStatus = PumpStatus;
            ViewBag.CurrentTrip = CurrentTrip;
            ViewBag.VoltageTrip = VoltageTrip;
            ViewBag.RemoteControl = RemoteControl;
            ViewBag.ChlorineLevel = ChlorineLevel;
            ViewBag.WaterExtracted = WaterExtracted;
            ViewBag.PKVAR = PKVAR;
            ViewBag.PKW = PKW;
            ViewBag.V12 = V12;
            ViewBag.V13 = V13;
            ViewBag.V23 = V23;
            ViewBag.PrimingLevel = PrimingLevel;
            ViewBag.averageVoltage = averageVoltage;
            ViewBag.averageVoltageSecond = averageVoltageSecond;
            ViewBag.averageCurrent = averageCurrent;
            ViewBag.TotalWorkingHour = 0;
            ViewBag.Scheduling = Scheduling;
            int num = random.Next(100);
            ViewBag.FlowData = num;
            ViewBag.Location = location;
            ViewBag.RunTime = time;
            //return PartialView(pivotTable);
            ////////////////////////////////////////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////////////////////////////////////////
            //int num = random.Next(100);
            //ViewBag.FlowData = num;
            //return PartialView();
            return PartialView(pivotTable);
        }
        public string getHMIStatus()
        {
            var hmi = new HMIstatus();
            //double ret = -1;
            string query1 = "select top(1) ParameterValue from tblEnergy where resourceID = 1068 and ParameterID = 125 order by ID desc";
            string query2 = "select top(1) ParameterValue from tblEnergy where resourceID = 1068 and ParameterID = 129 order by ID desc";
            string query3 = "select top(1) ParameterValue from tblEnergy where resourceID = 1068 and ParameterID = 167 order by ID desc";
            string query4 = "select top(1) ParameterValue from tblEnergy where resourceID = 1068 and ParameterID = 130 order by ID desc";
            string query5 = "select top(1) ParameterValue from tblEnergy where resourceID = 1068 and ParameterID = 174 order by ID desc";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                var cmd1 = new SqlCommand(query1, conn);
                var cmd2 = new SqlCommand(query2, conn);
                var cmd3 = new SqlCommand(query3, conn);
                var cmd4 = new SqlCommand(query4, conn);
                var cmd5 = new SqlCommand(query5, conn);
                try
                {
                    hmi.pumpStatus = Math.Abs(Convert.ToDouble(cmd1.ExecuteScalar()));
                    hmi.chlorineStatus = Math.Abs(Convert.ToDouble(cmd2.ExecuteScalar()));
                    hmi.primingStatus = Math.Abs(Convert.ToDouble(cmd3.ExecuteScalar()));
                    hmi.flowRate = Math.Round(Convert.ToDouble(cmd4.ExecuteScalar()), 1);
                    hmi.pressureRate = Math.Round(Convert.ToDouble(cmd5.ExecuteScalar()), 1);
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            return JsonConvert.SerializeObject(hmi); ;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ScheduleIndexChanged(string value)
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> resourceList = new List<string>();
            var slots = new ScheduleSlot();
            string query1from = "select TimeFrom from tblTubewellSchedule where slotNumber = 1 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '"+value+"')";
            string query1to = "select TimeTo from tblTubewellSchedule where slotNumber = 1 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + value + "')";
            string query2from = "select TimeFrom from tblTubewellSchedule where slotNumber = 2 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + value + "')";
            string query2to = "select TimeTo from tblTubewellSchedule where slotNumber = 2 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + value + "')";
            string query3from = "select TimeFrom from tblTubewellSchedule where slotNumber = 3 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + value + "')";
            string query3to = "select TimeTo from tblTubewellSchedule where slotNumber = 3 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + value + "')";
            string query4from = "select TimeFrom from tblTubewellSchedule where slotNumber = 4 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + value + "')";
            string query4to = "select TimeTo from tblTubewellSchedule where slotNumber = 4 and ResourceID = (select ResourceID from tblResource where ResourceLocation = '" + value + "')";
            string time1from = "";
            string time1to = "";
            string time2from = "";
            string time2to = "";
            string time3from = "";
            string time3to = "";
            string time4from = "";
            string time4to = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd1from = new SqlCommand(query1from, conn);
                    SqlCommand cmd1to = new SqlCommand(query1to, conn);
                    SqlCommand cmd2from = new SqlCommand(query2from, conn);
                    SqlCommand cmd2to = new SqlCommand(query2to, conn);
                    SqlCommand cmd3from = new SqlCommand(query3from, conn);
                    SqlCommand cmd3to = new SqlCommand(query3to, conn);
                    SqlCommand cmd4from = new SqlCommand(query4from, conn);
                    SqlCommand cmd4to = new SqlCommand(query4to, conn);
                    time1from = cmd1from.ExecuteScalar().ToString();
                    time1to = cmd1to.ExecuteScalar().ToString();
                    time2from = cmd2from.ExecuteScalar().ToString();
                    time2to = cmd2to.ExecuteScalar().ToString();
                    time3from = cmd3from.ExecuteScalar().ToString();
                    time3to = cmd3to.ExecuteScalar().ToString();
                    time4from = cmd4from.ExecuteScalar().ToString();
                    time4to = cmd4to.ExecuteScalar().ToString();
                    conn.Close();
                    if (time1from.Length == 1)
                    {
                        time1from = "0" + time1from + ":00";
                    }
                    if (time1to.Length == 1)
                    {
                        time1to = "0" + time1to + ":00";
                    }
                    if (time2from.Length == 1)
                    {
                        time2from = "0" + time2from + ":00";
                    }
                    if (time2to.Length == 1)
                    {
                        time2to = "0" + time2to + ":00";
                    }
                    if (time3from.Length == 1)
                    {
                        time3from = "0" + time3from + ":00";
                    }
                    if (time3to.Length == 1)
                    {
                        time3to = "0" + time3to + ":00";
                    }
                    if (time4from.Length == 1)
                    {
                        time4from = "0" + time4from + ":00";
                    }
                    if (time4to.Length == 1)
                    {
                        time4to = "0" + time4to + ":00";
                    }
                    if (time1from.Length == 2)
                    {
                        time1from = time1from + ":00";
                    }
                    if (time1to.Length == 2)
                    {
                        time1to = time1to + ":00";
                    }
                    if (time2from.Length == 2)
                    {
                        time2from = time2from + ":00";
                    }
                    if (time2to.Length == 2)
                    {
                        time2to = time2to + ":00";
                    }
                    if (time3from.Length == 2)
                    {
                        time3from = time3from + ":00";
                    }
                    if (time3to.Length == 2)
                    {
                        time3to = time3to + ":00";
                    }
                    if (time4from.Length == 2)
                    {
                        time4from = time4from + ":00";
                    }
                    if (time4to.Length == 2)
                    {
                        time4to = time4to + ":00";
                    }
                }
                catch (Exception ex)
                {
                    time1from = "00:00";
                    time1to = "00:00";
                    time2from = "00:00";
                    time2to = "00:00";
                    time3from = "00:00";
                    time3to = "00:00";
                    time4from = "00:00";
                    time4to = "00:00";
                }
            }
            slots.time1From = time1from;
            slots.time1To = time1to;
            slots.time2From = time2from;
            slots.time2To = time2to;
            slots.time3From = time3from;
            slots.time3To = time3to;
            slots.time4From = time4from;
            slots.time4To = time4to;
            ViewBag.time1from = time1from;
            ViewBag.time1to = time1to;
            ViewBag.time2from = time2from;
            ViewBag.time2to = time2to;
            ViewBag.time3from = time3from;
            ViewBag.time3to = time3to;
            ViewBag.time4from = time4from;
            ViewBag.time4to = time4to;
            return Json(new { data = slots }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult IndexChanged(string value)
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> resourceList = new List<string>();
            //if (value == 0 || value == 0 || value == 0)



            if (value == "" || value == null || value == "All")
            {
                foreach (var item in db.tblResources.AsEnumerable().Where(item => item.CompanyID == c_id))
                {
                    resourceList.Add(item.ResourceLocation);
                }
                resourceList.Add("All");
                //return Json(new { resource = resourceList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //foreach ( var item in db.tblResources)
                //foreach (var item in db.tblResources.Where(item => item.TemplateID == Convert.ToInt32(value)))
                foreach (var item in (from template in db.tblTemplates join resource in db.tblResources on template.TemplateID equals resource.TemplateID where template.TemplateName == value && template.CompanyID == c_id select new { Resource = resource, Template = template }))

                //foreach (var item in db.tblResources.Where(item => item.TemplateID == Convert.ToInt32(db.tblTemplates.FirstOrDefault(p => p.TemplateName == value).TemplateID)))
                {
                    //foreach (var item in db.tblResources.Where(item => item.TemplateID == Convert.ToInt32(db.tblTemplates.FirstOrDefault(p => p.TemplateName == value.Trim()).TemplateID)))
                    resourceList.Add(item.Resource.ResourceLocation);
                    int id = item.Template.TemplateID;
                    //resourceList.Add(item.ResourceLocation.Where(item.TemplateID == Convert.ToInt32(db.tblTemplates.FirstOrDefault(p => p.TemplateName == value.Trim()).TemplateID)));
                }

            }
            return Json(new { resource = resourceList }, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ParameterIndexChanged(string value)
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> parameterList = new List<string>();
            //if (value == 0 || value == 0 || value == 0)
            if (value == "" || value == null || value == "All")
            {
                foreach (var item in db.tblParameters.AsEnumerable().Where(item => item.CompanyID == c_id))
                {
                    parameterList.Add(item.ParameterName);
                    parameterList.Add("All");
                }
                //return Json(new { resource = resourceList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //foreach ( var item in db.tblResources)
                //foreach (var item in db.tblResources.Where(item => item.TemplateID == Convert.ToInt32(value)))

                foreach (var item in (from templateparameter in db.tblTemplateParameters
                                      join template in db.tblTemplates
                                      on templateparameter.TemplateID equals template.TemplateID
                                      join parameter in db.tblParameters
                                      on templateparameter.ParameterID equals parameter.ParameterID
                                      where template.TemplateName == value && template.CompanyID == c_id
                                      select new
                                      {
                                          Parameter = parameter,
                                          Template = template,
                                          TemplateParameter = templateparameter
                                      }))

                //foreach (var item in db.tblResources.Where(item => item.TemplateID == Convert.ToInt32(db.tblTemplates.FirstOrDefault(p => p.TemplateName == value).TemplateID)))
                {
                    //foreach (var item in db.tblResources.Where(item => item.TemplateID == Convert.ToInt32(db.tblTemplates.FirstOrDefault(p => p.TemplateName == value.Trim()).TemplateID)))
                    parameterList.Add(item.Parameter.ParameterName);
                    int id = item.TemplateParameter.TemplateID;
                    //resourceList.Add(item.ResourceLocation.Where(item.TemplateID == Convert.ToInt32(db.tblTemplates.FirstOrDefault(p => p.TemplateName == value.Trim()).TemplateID)));
                }


            }
            return Json(new { parameter = parameterList }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetNotifications()
        {
            string query = "select top(1) readTime from tblNotifications order by readTime DESC";
            DateTime lastReadTime = DateTime.Now.AddDays(-2);
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                var cmd1 = new SqlCommand(query, conn);
                try
                {
                    lastReadTime = Convert.ToDateTime(cmd1.ExecuteScalar());
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            var notificationRegisterTime = lastReadTime;
            NotificationComponent NC = new NotificationComponent();
            var list = NC.GetData(notificationRegisterTime);

            //update session here for get only new added contacts (notification)  
            //Session["LastUpdated"] = DateTime.Now;
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public string GetNotificationsCount()
        {
            string query = "select top(1) readTime from tblNotifications order by readTime DESC";
            DateTime lastReadTime = DateTime.Now.AddDays(-2);
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                var cmd1 = new SqlCommand(query, conn);
                try
                {
                    lastReadTime = Convert.ToDateTime(cmd1.ExecuteScalar());
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            var notificationRegisterTime = lastReadTime;
            NotificationComponent NC = new NotificationComponent();
            var list = NC.GetData(notificationRegisterTime);

            //update session here for get only new added contacts (notification)  
            //Session["LastUpdated"] = DateTime.Now;
            return list.Count().ToString();
        }
        public ActionResult Notifications()
        {
            return View();
        }
        public void UpdateSession()
        {
            DateTime nowtime = DateTime.Now.AddHours(9);
            string query = "update tblNotifications set readTime = CONVERT(CHAR(24), CONVERT(DATETIME, '" + nowtime + "', 103), 121) where notificationTime = (select max(notificationTime) from tblNotifications)";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                var cmd1 = new SqlCommand(query, conn);
                try
                {
                    cmd1.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
        }

    }
}