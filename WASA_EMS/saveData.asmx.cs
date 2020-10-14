using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using WASA_EMS.Models;

namespace WASA_EMS
{
    /// <summary>
    /// Summary description for saveData
    /// </summary>
    /// FOR WASA TTLOGIX
    [WebService(Namespace = "http://wasalhr.ttlogix.com/")]
    /// FOR WASA AMAZON WEB SERVER
    //[WebService(Namespace = "http://http://35.166.74.72/")]
    /// FOR LOCALHOST
    //[WebService(Namespace = "http://localhost/")]


    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class saveData : System.Web.Services.WebService
    {
        public int c_id;
        [WebMethod]
        public string insertData(string code, string val, string time)
        {
            string Message;
            BAL bal = new BAL();
            int result = bal.saveData(code, val, time);
            if (result == 1)
            {
                Message = "Details are inserted successfully";
            }
            else
            {
                Message = "Details are not inserted successfully";
            }
            return Message;
        }

        [WebMethod]
        public string insertUpdateData(string code, string val, string time, string lat, string lng)
        {
            string Message;
            BAL bal = new BAL();
            int result = bal.saveData(code, val, time, lat, lng);
            if (result == 1)
            {
                Message = "Details are inserted successfully";
            }
            else
            {
                Message = "Details are not inserted successfully";
            }
            return Message;
        }
        [WebMethod]
        public string getRemoteStatus(string sender)
        {
            BAL bal = new BAL();
            string Mode = bal.getRemoteMode(sender);
            return Mode;
        }
        [WebMethod]
        public string getScheduleTime(string sender)
        {
            BAL bal = new BAL();
            string ScheduleTime = bal.getScheduleTime(sender);
            return ScheduleTime;
        }

        [WebMethod]
        public string execQuery(string query)
        {
            BAL bal = new BAL();
            string result = bal.execQuery(query);
            return result;
        }



        [WebMethod]
        public string GetData()
        {
            int c_id = 4;
            List<object> lists = new List<object>();
            //int c_id = Convert.ToInt32(Session["CompanyID"]);
            string tempName = "";
            string query = "select TemplateID, TemplateName from tblTemplate where CompanyID = " + c_id + "";
            query += " and TemplateID in (select TemplateID from tblResource) ";
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand cmd2 = new SqlCommand(query))
                {
                    cmd2.Connection = con1;
                    con1.Open();
                    using (SqlDataReader sdr = cmd2.ExecuteReader())
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
                            int manual = 0;
                            int remote = 0;
                            int scheduling = 0;
                            int pumpStatus = 0;
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
                                        parameterValuesString += "Water Flow (cfs) : ";
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
                                    else if (sdr1["ParameterName"].ToString() == "vib_M")
                                    {
                                        parameterValuesString += "Vibration (m) : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "vib_Ms")
                                    {
                                        parameterValuesString += "Vibration (m/s) : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "vib_Ms2")
                                    {
                                        parameterValuesString += "Vibration (m/s2) : ";
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
                                        manual = Convert.ToInt32(sdr1["ParameterValue"]);
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "Remote.")
                                    {
                                        if (manual == 0 && sdr1["ParameterValue"].ToString() == "1")
                                        {
                                            valuee = "ON";
                                        }
                                        else
                                        {
                                            valuee = "OFF";
                                        }
                                        remote = Convert.ToInt32(sdr1["ParameterValue"]);
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "TimeSchedule.")
                                    {
                                        if (manual == 0 && remote == 0 && sdr1["ParameterValue"].ToString() == "1")
                                        {
                                            valuee = "ON";
                                        }
                                        else
                                        {
                                            valuee = "OFF";
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
                                    else if (sdr1["ParameterName"].ToString() == "WaterFlow(Cusec).")
                                    {
                                        valuee = Math.Round((Convert.ToDouble(sdr1["ParameterValue"])/101), 2).ToString();
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
                            string templName = sdr["TemplateName"].ToString();
                            if (templName == "Tubewells")
                            {
                                templName = "Tubewell";
                            }
                            string newstring = "<b>" + templName + "</b>";
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

            string p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, ptime, plevel1, plevel2 = "";
            int pp1 = 0;
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

            parameterValuesString = "Pump 1 : " + p1;
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
            markers += string.Format("'lat':'{0}',", "31.430021");
            markers += string.Format("'lnt':'{0}',", "74.252829");
            markers += string.Format("'DelTime': '{0}',", minup);
            markers += string.Format("'description': '{0}'", newstringp);
            markers += "},";

            //////////////////////////////////////////////////////////////////////////////////////////////////
            markers += "]";
            return markers;
        }

    }
}
