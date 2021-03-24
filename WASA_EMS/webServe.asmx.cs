using Newtonsoft.Json;
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
    public class webServe : System.Web.Services.WebService
    {
        [WebMethod]
        public string WhatsAppData()
        {
            DateTime FinalTimeFrom = DateTime.Now.AddDays(-1).Date;
            DateTime FinalTimeTo = DateTime.Now.AddHours(0).AddDays(0).Date.AddSeconds(-1);

            return "";
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
            string S = "";
            string PG = "";
            string PL = "";
            string PF = "";
            string VM = "";
            string CL = "";
            string LS = "";
            string CT = "";
            string VT = "";
            string Auto = "";
            string Time = "";
            string Remote = "";
            string PGvaluee = "";
            string PFvaluee = "";
            string PLvaluee = "";
            string VMvaluee = "";
            string LSvaluee = "";
            string CLvaluee = "";

            string CTvaluee = "";
            string VTvaluee = "";
            string parameterValuesString = "";
            string datetimed = "";
            string markers = "[";
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            string q = "select r.ResourceLocation, r.ResourceID, r.CooridatesGoogle, t.TemplateName from tblResource r left join tblTemplate t on r.TemplateID = t.TemplateID  where t.CompanyID = " + c_id + "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd3 = new SqlCommand(q, conn);
                    using (SqlDataReader sdr = cmd3.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string q1 = "select distinct r.ResourceLocation,  t.TemplateName, p.ParameterName, e.ParameterValue, e.InsertionDateTime, rms.CurrentMotorOnOffStatus from tblEnergy e ";
                            q1 += "left join tblParameter p on e.ParameterID = p.ParameterID ";
                            q1 += "left join tblResource r on e.ResourceID = r.ResourceID ";
                            q1 += "left join tblRemoteSensor rms on r.ResourceID = rms.ResourceID ";
                            q1 += "left join tblTemplate t on r.TemplateID = t.TemplateID ";
                            q1 += "where e.InsertionDateTime = (select max(InsertionDateTime) from tblEnergy where ResourceID = " + sdr["ResourceID"] + ")";

                            q1 += "and r.ResourceID = " + sdr["ResourceID"] + "";
                            SqlCommand cmd1 = new SqlCommand(q1, conn);
                            int optionStatus = 0;
                            using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                            {
                                while (sdr1.Read())
                                {
                                    string valuee = "";






                                    parameterValuesString += "";
                                    parameterValuesString += sdr1["ParameterName"].ToString() + ": ";

                                    if (sdr1["ParameterName"].ToString() == "Manual")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                            Auto = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            Auto = "ON";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                            S = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            S = "ON";
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
                                            Remote = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            Remote = "ON";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "TimeSchedule.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                            Time = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            Time = "ON";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "Pressure(Bar)")
                                    {
                                        PGvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PF.")
                                    {
                                        PFvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PrimingLevel")
                                    {
                                        PLvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "vib_y")
                                    {
                                        VMvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "ChlorineLevel.")
                                    {
                                        CLvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "OutdoorLight")
                                    {
                                        LSvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "VoltageTrip.")
                                    {
                                        VTvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "CurrentTrip.")
                                    {
                                        CTvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else
                                    {
                                        valuee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }

                                    PG += PGvaluee;

                                    PF += PFvaluee;
                                    PL += PLvaluee;
                                    VM += VMvaluee;
                                    CL += CLvaluee;
                                    LS += LSvaluee;
                                    VT += VTvaluee;
                                    CT += CTvaluee;
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
                            string des = sdr["ResourceLocation"].ToString();
                            //des += "<br />";
                            // des += theStatus;
                            string result = string.Concat(des, " ", S);
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

                            markers += string.Format("'Auto': '{0}',", Auto);
                            markers += string.Format("'Remote': '{0}',", Remote);
                            markers += string.Format("'Time': '{0}',", Time);
                            markers += string.Format("'CT': '{0}',", CTvaluee);
                            markers += string.Format("'VT': '{0}',", VTvaluee);
                            markers += string.Format("'LS': '{0}',", LSvaluee);
                            markers += string.Format("'CL': '{0}',", CLvaluee);
                            markers += string.Format("'VM': '{0}',", PLvaluee);
                            markers += string.Format("'PF': '{0}',", PFvaluee);
                            markers += string.Format("'P': '{0}',", PGvaluee);
                            markers += string.Format("'PL': '{0}',", PLvaluee);
                            markers += string.Format("'Status': '{0}',", theStatus);
                            markers += string.Format("'Template': '{0}',", tempName);
                            markers += string.Format("'title': '{0}',", sdr["ResourceLocation"]);
                            markers += string.Format("'Dtae': '{0}',", datetimed);
                            //markers += string.Format("'time': '{0}',", sdr1["InsertionDateTime"]);
                            markers += string.Format("'lat':'{0}',", sdr["CooridatesGoogle"].ToString().Split(',')[0]);
                            markers += string.Format("'lnt':'{0}',", sdr["CooridatesGoogle"].ToString().Split(',')[1]);
                            //markers += string.Format("'parameter':'{0}',", sdr1["ParameterName"]);
                            //markers += string.Format("'value':'{0}'", sdr1["ParameterValue"]);
                            markers += string.Format("'DelTime': '{0}',", datetimed);
                            markers += string.Format("'description': '{0}'", result);
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

            markers += "]";
            return markers;
        }

        [WebMethod]
        public string GetDatawe()
        {
            // DateTime FinalTimeFrom = DateTime.Now;
            // DateTime FinalTimeTo = DateTime.Now;
            DateTime FinalTimeFrom = DateTime.Now.AddHours(0).AddDays(-30).Date;
            DateTime FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date;
            //  DateTime FinalTimeFrom = DateTime.Now.AddHours(5).Date;
            //  DateTime FinalTimeTo = DateTime.Now.AddHours(5).AddDays(1).Date;


            string scriptString = "";

            string datap = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {

                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'Well2Level(ft)' ";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'Well2Level(ft)'   and rtp.TemplateID = 62 order by cast(r.ResourceID as int) asc";
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        //  scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"WaterFlow (Cusec)\" },";
                        scriptString += "subtitles: [{text: \" All Tubewells Recent Water Flow  \" }],";
                        scriptString += "axisY: {suffix: \" Cusec\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
                        scriptString += " data: [";
                        //   scriptString +=  dtPar.Rows[1]["ResourceID"];

                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            scriptString += drPar["ResourceID"];
                            // scriptString += drPar["ResourceID"];

                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            //aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime  > DATE_SUB(NOW(),â€‹INTERVAL 1";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            // aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + 130 + " or e.ParameterID = " + 118 + "and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14001 Order by InsertionDateTime DESC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            // Loc += drPar["ResourceName"].ToString();
                            scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} Cusec</strong> at {x}\", ";

                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
                                // scriptString += drVal["ParameterValue"];
                                // if (dtVal.Rows.IndexOf(drVal) != 0)
                                // {
                                //  dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(dt).AddHours(-5).AddMinutes(1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5).AddMinutes(-1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                // dt = Convert.ToDateTime(drVal["InsertionDateTime"]);
                                // }
                                //else
                                //{
                                dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                dt = Convert.ToDateTime(drVal["InsertionDateTime"]);
                                //}
                                //dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }
                            scriptString += "dataPoints: " + JsonConvert.SerializeObject(dataPoints) + "";
                            datap = JsonConvert.SerializeObject(dataPoints) + "";
                            scriptString += "},";
                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString = scriptString + "]";
                        scriptString = scriptString + "}";
                        scriptString += ");";
                    }
                }

                catch (Exception ex)
                {

                }
            }
            string NewscripString = datap;
            return NewscripString;
        }
        [WebMethod]
        public string GetDatawe1()
        {
            DateTime FinalTimeFrom = DateTime.Now.AddHours(0).AddDays(-30).Date;
            DateTime FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date;
            string scriptString = "";
            string datap = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {

                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'Well1Level(ft)' ";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'Well1Level(ft)'   and rtp.TemplateID = 61 order by cast(r.ResourceID as int) asc";
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        //  scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"WaterFlow (Cusec)\" },";
                        scriptString += "subtitles: [{text: \" All Tubewells Recent Water Flow  \" }],";
                        scriptString += "axisY: {suffix: \" Cusec\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
                        scriptString += " data: [";
                        //   scriptString +=  dtPar.Rows[1]["ResourceID"];

                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            scriptString += drPar["ResourceID"];
                            // scriptString += drPar["ResourceID"];

                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            //aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime  > DATE_SUB(NOW(),â€‹INTERVAL 1";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            // aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + 130 + " or e.ParameterID = " + 118 + "and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14001 Order by InsertionDateTime DESC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            // Loc += drPar["ResourceName"].ToString();
                            scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} Cusec</strong> at {x}\", ";

                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
                                dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                dt = Convert.ToDateTime(drVal["InsertionDateTime"]);
                            }
                            scriptString += "dataPoints: " + JsonConvert.SerializeObject(dataPoints) + "";
                            datap = JsonConvert.SerializeObject(dataPoints) + "";
                            scriptString += "},";
                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString = scriptString + "]";
                        scriptString = scriptString + "}";
                        scriptString += ");";
                    }
                }

                catch (Exception ex)
                {

                }
            }
            string NewscripString = datap;
            return NewscripString;
        }
        // today 
        // working C 2
        [WebMethod]
        public string GetWH()
        {
            // DateTime FinalTimeFrom = DateTime.Now;
            // DateTime FinalTimeTo = DateTime.Now;
            //   DateTime FinalTimeFrom = DateTime.Now.AddHours(0).AddDays(-30).Date;
            //  DateTime FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date;
            DateTime FinalTimeFrom = DateTime.Now.AddHours(5).Date;
            DateTime FinalTimeTo = DateTime.Now.AddHours(5).AddDays(1).Date;
            string scriptString = "";
            string datap = "";
            string tim = "";
            string t = "";

            string firstElem = "";

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {

                try
                {

                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'PumpStatus' ";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'   and rtp.TemplateID = 64 order by cast(r.ResourceID as int) asc";
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        //  scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"WaterFlow (Cusec)\" },";
                        scriptString += "subtitles: [{text: \" All Tubewells Recent Water Flow  \" }],";
                        scriptString += "axisY: {suffix: \" Cusec\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
                        scriptString += " data: [";
                        //   scriptString +=  dtPar.Rows[1]["ResourceID"];

                        // foreach (DataRow drPar in dtPar.Rows)
                        // {

                        if (dtPar.Rows.Count > 0)
                        {
                            DataRow drPar = dtPar.Rows[0];

                            // scriptString += drPar["ResourceID"];
                            // scriptString += drPar["ResourceID"];

                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            //aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime  > DATE_SUB(NOW(),â€‹INTERVAL 1";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            // aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + 130 + " or e.ParameterID = " + 118 + "and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime DESC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            // Loc += drPar["ResourceName"].ToString();
                            scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} Cusec</strong> at {x}\", ";

                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {

                                dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                dt = Convert.ToDateTime(drVal["InsertionDateTime"]);

                                if (drVal["ParameterValue"].ToString() == "1")
                                {
                                    // tim += dt.ToString();
                                    tim += dt.Hour.ToString();
                                    int distinctCount = tim.Distinct().Count();
                                    t = distinctCount.ToString();
                                    //DateTime dt1 = DateTime.Parse(tim);
                                    //dt1.ToString("HH:mm");
                                }

                                //  t += tim;

                                //datap = JsonConvert.SerializeObject(dataPoints) + "";
                                /* for (int i = 0; i < datap.Length; i++)
                                 {
                                     var YUe = dataPoints[i].y;

                                     if (YUe == 1)
                                     {
                                       tim += dt.ToString();
                                     }
                                     t += tim;
                                 }*/

                                //}
                                // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]). - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }

                            scriptString += "dataPoints: " + JsonConvert.SerializeObject(dataPoints) + "";

                            datap = JsonConvert.SerializeObject(dataPoints) + "";

                            //var array = t.Split(' ');
                            //firstElem = array[1];

                            //Console.WriteLine(datap.Length);


                            // datap += dataPoints;
                            scriptString += "},";
                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString = scriptString + "]";
                        scriptString = scriptString + "}";
                        scriptString += ");";

                    }
                }

                catch (Exception ex)
                {

                }
            }
            string NewscripString = t;
            return NewscripString;
        }
        // working C 2 7


        // working A
        [WebMethod]
        public string GetWHA3()
        {
            // DateTime FinalTimeFrom = DateTime.Now;
            // DateTime FinalTimeTo = DateTime.Now;
            //   DateTime FinalTimeFrom = DateTime.Now.AddHours(0).AddDays(-30).Date;
            //  DateTime FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date;
            DateTime FinalTimeFrom = DateTime.Now.AddHours(5).Date;
            DateTime FinalTimeTo = DateTime.Now.AddHours(5).AddDays(1).Date;
            string scriptString = "";
            string datap = "";
            string tim = "";
            string t = "";

            string firstElem = "";

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {

                try
                {

                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'PumpStatus' ";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'   and rtp.TemplateID = 64 order by cast(r.ResourceID as int) asc";
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        //  scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"WaterFlow (Cusec)\" },";
                        scriptString += "subtitles: [{text: \" All Tubewells Recent Water Flow  \" }],";
                        scriptString += "axisY: {suffix: \" Cusec\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
                        scriptString += " data: [";
                        //   scriptString +=  dtPar.Rows[1]["ResourceID"];

                        // foreach (DataRow drPar in dtPar.Rows)
                        // {

                        if (dtPar.Rows.Count > 0)
                        {
                            DataRow drPar = dtPar.Rows[1];

                            // scriptString += drPar["ResourceID"];
                            // scriptString += drPar["ResourceID"];

                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            //aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime  > DATE_SUB(NOW(),â€‹INTERVAL 1";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            // aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + 130 + " or e.ParameterID = " + 118 + "and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime DESC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            // Loc += drPar["ResourceName"].ToString();
                            scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} Cusec</strong> at {x}\", ";

                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {

                                dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                dt = Convert.ToDateTime(drVal["InsertionDateTime"]);

                                if (drVal["ParameterValue"].ToString() == "1")
                                {
                                    // tim += dt.ToString();
                                    tim += dt.Hour.ToString();
                                    int distinctCount = tim.Distinct().Count();
                                    t = distinctCount.ToString();
                                    //DateTime dt1 = DateTime.Parse(tim);
                                    //dt1.ToString("HH:mm");
                                }

                                //  t += tim;

                                //datap = JsonConvert.SerializeObject(dataPoints) + "";
                                /* for (int i = 0; i < datap.Length; i++)
                                 {
                                     var YUe = dataPoints[i].y;

                                     if (YUe == 1)
                                     {
                                       tim += dt.ToString();
                                     }
                                     t += tim;
                                 }*/

                                //}
                                // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]). - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }

                            scriptString += "dataPoints: " + JsonConvert.SerializeObject(dataPoints) + "";

                            datap = JsonConvert.SerializeObject(dataPoints) + "";

                            //var array = t.Split(' ');
                            //firstElem = array[1];

                            //Console.WriteLine(datap.Length);


                            // datap += dataPoints;
                            scriptString += "},";
                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString = scriptString + "]";
                        scriptString = scriptString + "}";
                        scriptString += ");";

                    }
                }

                catch (Exception ex)
                {

                }
            }
            string NewscripString = t;
            return NewscripString;






        }
        // working a 7

        // working D
        [WebMethod]
        public string GetWHD()
        {
            // DateTime FinalTimeFrom = DateTime.Now;
            // DateTime FinalTimeTo = DateTime.Now;
            //   DateTime FinalTimeFrom = DateTime.Now.AddHours(0).AddDays(-30).Date;
            //  DateTime FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date;
            DateTime FinalTimeFrom = DateTime.Now.AddHours(5).Date;
            DateTime FinalTimeTo = DateTime.Now.AddHours(5).AddDays(1).Date;
            string scriptString = "";
            string datap = "";
            string tim = "";
            string t = "";

            string firstElem = "";

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {

                try
                {

                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'PumpStatus' ";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'   and rtp.TemplateID = 64 order by cast(r.ResourceID as int) asc";
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        //  scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"WaterFlow (Cusec)\" },";
                        scriptString += "subtitles: [{text: \" All Tubewells Recent Water Flow  \" }],";
                        scriptString += "axisY: {suffix: \" Cusec\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
                        scriptString += " data: [";
                        //   scriptString +=  dtPar.Rows[1]["ResourceID"];

                        // foreach (DataRow drPar in dtPar.Rows)
                        // {

                        if (dtPar.Rows.Count > 0)
                        {
                            DataRow drPar = dtPar.Rows[2];

                            // scriptString += drPar["ResourceID"];
                            // scriptString += drPar["ResourceID"];

                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            //aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime  > DATE_SUB(NOW(),â€‹INTERVAL 1";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            // aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + 130 + " or e.ParameterID = " + 118 + "and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime DESC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            // Loc += drPar["ResourceName"].ToString();
                            scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} Cusec</strong> at {x}\", ";

                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {

                                dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                dt = Convert.ToDateTime(drVal["InsertionDateTime"]);

                                if (drVal["ParameterValue"].ToString() == "1")
                                {
                                    // tim += dt.ToString();
                                    tim += dt.Hour.ToString();
                                    int distinctCount = tim.Distinct().Count();
                                    t = distinctCount.ToString();
                                    //DateTime dt1 = DateTime.Parse(tim);
                                    //dt1.ToString("HH:mm");
                                }

                                //  t += tim;

                                //datap = JsonConvert.SerializeObject(dataPoints) + "";
                                /* for (int i = 0; i < datap.Length; i++)
                                 {
                                     var YUe = dataPoints[i].y;

                                     if (YUe == 1)
                                     {
                                       tim += dt.ToString();
                                     }
                                     t += tim;
                                 }*/

                                //}
                                // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]). - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }

                            scriptString += "dataPoints: " + JsonConvert.SerializeObject(dataPoints) + "";

                            datap = JsonConvert.SerializeObject(dataPoints) + "";

                            //var array = t.Split(' ');
                            //firstElem = array[1];

                            //Console.WriteLine(datap.Length);


                            // datap += dataPoints;
                            scriptString += "},";
                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString = scriptString + "]";
                        scriptString = scriptString + "}";
                        scriptString += ");";

                    }
                }

                catch (Exception ex)
                {

                }
            }
            string NewscripString = t;
            return NewscripString;






        }
        // working d 7

        // working E
        [WebMethod]
        public string GetWHE()
        {
            // DateTime FinalTimeFrom = DateTime.Now;
            // DateTime FinalTimeTo = DateTime.Now;
            //   DateTime FinalTimeFrom = DateTime.Now.AddHours(0).AddDays(-30).Date;
            //  DateTime FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date;
            DateTime FinalTimeFrom = DateTime.Now.AddHours(5).Date;
            DateTime FinalTimeTo = DateTime.Now.AddHours(5).AddDays(1).Date;
            string scriptString = "";
            string datap = "";
            string tim = "";
            string t = "";

            string firstElem = "";

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {

                try
                {

                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'PumpStatus' ";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'   and rtp.TemplateID = 64 order by cast(r.ResourceID as int) asc";
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        //  scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"WaterFlow (Cusec)\" },";
                        scriptString += "subtitles: [{text: \" All Tubewells Recent Water Flow  \" }],";
                        scriptString += "axisY: {suffix: \" Cusec\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
                        scriptString += " data: [";
                        //   scriptString +=  dtPar.Rows[1]["ResourceID"];

                        // foreach (DataRow drPar in dtPar.Rows)
                        // {

                        if (dtPar.Rows.Count > 0)
                        {
                            DataRow drPar = dtPar.Rows[3];

                            // scriptString += drPar["ResourceID"];
                            // scriptString += drPar["ResourceID"];

                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            //aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime  > DATE_SUB(NOW(),â€‹INTERVAL 1";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            // aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + 130 + " or e.ParameterID = " + 118 + "and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime DESC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            // Loc += drPar["ResourceName"].ToString();
                            scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} Cusec</strong> at {x}\", ";

                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {

                                dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                dt = Convert.ToDateTime(drVal["InsertionDateTime"]);

                                if (drVal["ParameterValue"].ToString() == "1")
                                {
                                    // tim += dt.ToString();
                                    tim += dt.Hour.ToString();
                                    int distinctCount = tim.Distinct().Count();
                                    t = distinctCount.ToString();
                                    //DateTime dt1 = DateTime.Parse(tim);
                                    //dt1.ToString("HH:mm");
                                }

                                //  t += tim;

                                //datap = JsonConvert.SerializeObject(dataPoints) + "";
                                /* for (int i = 0; i < datap.Length; i++)
                                 {
                                     var YUe = dataPoints[i].y;

                                     if (YUe == 1)
                                     {
                                       tim += dt.ToString();
                                     }
                                     t += tim;
                                 }*/

                                //}
                                // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]). - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }

                            scriptString += "dataPoints: " + JsonConvert.SerializeObject(dataPoints) + "";

                            datap = JsonConvert.SerializeObject(dataPoints) + "";

                            //var array = t.Split(' ');
                            //firstElem = array[1];

                            //Console.WriteLine(datap.Length);


                            // datap += dataPoints;
                            scriptString += "},";
                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString = scriptString + "]";
                        scriptString = scriptString + "}";
                        scriptString += ");";

                    }
                }

                catch (Exception ex)
                {

                }
            }
            string NewscripString = t;
            return NewscripString;






        }



        // working campus
        [WebMethod]
        public string GetWHCp()
        {
            // DateTime FinalTimeFrom = DateTime.Now;
            // DateTime FinalTimeTo = DateTime.Now;
            //   DateTime FinalTimeFrom = DateTime.Now.AddHours(0).AddDays(-30).Date;
            //  DateTime FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date;
            DateTime FinalTimeFrom = DateTime.Now.AddHours(5).Date;
            DateTime FinalTimeTo = DateTime.Now.AddHours(5).AddDays(1).Date;
            string scriptString = "";
            string datap = "";
            string tim = "";
            string t = "";

            string firstElem = "";

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {

                try
                {

                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'PumpStatus' ";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'   and rtp.TemplateID = 64 order by cast(r.ResourceID as int) asc";
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        //  scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"WaterFlow (Cusec)\" },";
                        scriptString += "subtitles: [{text: \" All Tubewells Recent Water Flow  \" }],";
                        scriptString += "axisY: {suffix: \" Cusec\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
                        scriptString += " data: [";
                        //   scriptString +=  dtPar.Rows[1]["ResourceID"];

                        // foreach (DataRow drPar in dtPar.Rows)
                        // {

                        if (dtPar.Rows.Count > 0)
                        {
                            DataRow drPar = dtPar.Rows[4];

                            // scriptString += drPar["ResourceID"];
                            // scriptString += drPar["ResourceID"];

                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            //aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime  > DATE_SUB(NOW(),â€‹INTERVAL 1";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            // aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + 130 + " or e.ParameterID = " + 118 + "and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime DESC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            // Loc += drPar["ResourceName"].ToString();
                            scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} Cusec</strong> at {x}\", ";

                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {

                                dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                dt = Convert.ToDateTime(drVal["InsertionDateTime"]);

                                if (drVal["ParameterValue"].ToString() == "1")
                                {
                                    // tim += dt.ToString();
                                    tim += dt.Hour.ToString();
                                    int distinctCount = tim.Distinct().Count();
                                    t = distinctCount.ToString();
                                    //DateTime dt1 = DateTime.Parse(tim);
                                    //dt1.ToString("HH:mm");
                                }

                                //  t += tim;

                                //datap = JsonConvert.SerializeObject(dataPoints) + "";
                                /* for (int i = 0; i < datap.Length; i++)
                                 {
                                     var YUe = dataPoints[i].y;

                                     if (YUe == 1)
                                     {
                                       tim += dt.ToString();
                                     }
                                     t += tim;
                                 }*/

                                //}
                                // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]). - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }

                            scriptString += "dataPoints: " + JsonConvert.SerializeObject(dataPoints) + "";

                            datap = JsonConvert.SerializeObject(dataPoints) + "";

                            //var array = t.Split(' ');
                            //firstElem = array[1];

                            //Console.WriteLine(datap.Length);


                            // datap += dataPoints;
                            scriptString += "},";
                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString = scriptString + "]";
                        scriptString = scriptString + "}";
                        scriptString += ");";

                    }
                }

                catch (Exception ex)
                {

                }
            }
            string NewscripString = t;
            return NewscripString;






        }
        // working camp 7


        // working F
        [WebMethod]
        public string GetWHF()
        {
            // DateTime FinalTimeFrom = DateTime.Now;
            // DateTime FinalTimeTo = DateTime.Now;
            //   DateTime FinalTimeFrom = DateTime.Now.AddHours(0).AddDays(-30).Date;
            //  DateTime FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date;
            DateTime FinalTimeFrom = DateTime.Now.AddHours(5).Date;
            DateTime FinalTimeTo = DateTime.Now.AddHours(5).AddDays(1).Date;
            string scriptString = "";
            string datap = "";
            string tim = "";
            string t = "";

            string firstElem = "";

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {

                try
                {

                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'PumpStatus' ";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'   and rtp.TemplateID = 64 order by cast(r.ResourceID as int) asc";
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        //  scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"WaterFlow (Cusec)\" },";
                        scriptString += "subtitles: [{text: \" All Tubewells Recent Water Flow  \" }],";
                        scriptString += "axisY: {suffix: \" Cusec\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
                        scriptString += " data: [";
                        //   scriptString +=  dtPar.Rows[1]["ResourceID"];

                        // foreach (DataRow drPar in dtPar.Rows)
                        // {

                        if (dtPar.Rows.Count > 0)
                        {
                            DataRow drPar = dtPar.Rows[5];

                            // scriptString += drPar["ResourceID"];
                            // scriptString += drPar["ResourceID"];

                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            //aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime  > DATE_SUB(NOW(),â€‹INTERVAL 1";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            // aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + 130 + " or e.ParameterID = " + 118 + "and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime DESC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            // Loc += drPar["ResourceName"].ToString();
                            scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} Cusec</strong> at {x}\", ";

                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {

                                dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                dt = Convert.ToDateTime(drVal["InsertionDateTime"]);

                                if (drVal["ParameterValue"].ToString() == "1")
                                {
                                    // tim += dt.ToString();
                                    tim += dt.Hour.ToString();
                                    int distinctCount = tim.Distinct().Count();
                                    t = distinctCount.ToString();
                                    //DateTime dt1 = DateTime.Parse(tim);
                                    //dt1.ToString("HH:mm");
                                }

                                //  t += tim;

                                //datap = JsonConvert.SerializeObject(dataPoints) + "";
                                /* for (int i = 0; i < datap.Length; i++)
                                 {
                                     var YUe = dataPoints[i].y;

                                     if (YUe == 1)
                                     {
                                       tim += dt.ToString();
                                     }
                                     t += tim;
                                 }*/

                                //}
                                // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]). - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }

                            scriptString += "dataPoints: " + JsonConvert.SerializeObject(dataPoints) + "";

                            datap = JsonConvert.SerializeObject(dataPoints) + "";

                            //var array = t.Split(' ');
                            //firstElem = array[1];

                            //Console.WriteLine(datap.Length);


                            // datap += dataPoints;
                            scriptString += "},";
                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString = scriptString + "]";
                        scriptString = scriptString + "}";
                        scriptString += ");";

                    }
                }

                catch (Exception ex)
                {

                }
            }
            string NewscripString = t;
            return NewscripString;






        }



        // working j
        [WebMethod]
        public string GetWHJ()
        {
            // DateTime FinalTimeFrom = DateTime.Now;
            // DateTime FinalTimeTo = DateTime.Now;
            //   DateTime FinalTimeFrom = DateTime.Now.AddHours(0).AddDays(-30).Date;
            //  DateTime FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date;
            DateTime FinalTimeFrom = DateTime.Now.AddHours(5).Date;
            DateTime FinalTimeTo = DateTime.Now.AddHours(5).AddDays(1).Date;
            string scriptString = "";
            string datap = "";
            string tim = "";
            string t = "";

            string firstElem = "";

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {

                try
                {

                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'PumpStatus' ";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'   and rtp.TemplateID = 64 order by cast(r.ResourceID as int) asc";
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        //  scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"WaterFlow (Cusec)\" },";
                        scriptString += "subtitles: [{text: \" All Tubewells Recent Water Flow  \" }],";
                        scriptString += "axisY: {suffix: \" Cusec\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
                        scriptString += " data: [";
                        //   scriptString +=  dtPar.Rows[1]["ResourceID"];

                        // foreach (DataRow drPar in dtPar.Rows)
                        // {

                        if (dtPar.Rows.Count > 0)
                        {
                            DataRow drPar = dtPar.Rows[6];

                            // scriptString += drPar["ResourceID"];
                            // scriptString += drPar["ResourceID"];

                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            //aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime  > DATE_SUB(NOW(),â€‹INTERVAL 1";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            // aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + 130 + " or e.ParameterID = " + 118 + "and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime DESC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            // Loc += drPar["ResourceName"].ToString();
                            scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} Cusec</strong> at {x}\", ";

                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {

                                dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                dt = Convert.ToDateTime(drVal["InsertionDateTime"]);

                                if (drVal["ParameterValue"].ToString() == "1")
                                {
                                    // tim += dt.ToString();
                                    tim += dt.Hour.ToString();
                                    int distinctCount = tim.Distinct().Count();
                                    t = distinctCount.ToString();
                                    //DateTime dt1 = DateTime.Parse(tim);
                                    //dt1.ToString("HH:mm");
                                }

                                //  t += tim;

                                //datap = JsonConvert.SerializeObject(dataPoints) + "";
                                /* for (int i = 0; i < datap.Length; i++)
                                 {
                                     var YUe = dataPoints[i].y;

                                     if (YUe == 1)
                                     {
                                       tim += dt.ToString();
                                     }
                                     t += tim;
                                 }*/

                                //}
                                // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]). - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }

                            scriptString += "dataPoints: " + JsonConvert.SerializeObject(dataPoints) + "";

                            datap = JsonConvert.SerializeObject(dataPoints) + "";

                            //var array = t.Split(' ');
                            //firstElem = array[1];

                            //Console.WriteLine(datap.Length);


                            // datap += dataPoints;
                            scriptString += "},";
                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString = scriptString + "]";
                        scriptString = scriptString + "}";
                        scriptString += ");";

                    }
                }

                catch (Exception ex)
                {

                }
            }
            string NewscripString = t;
            return NewscripString;






        }


        [WebMethod]
        public string GetWH2()
        {
            // DateTime FinalTimeFrom = DateTime.Now;
            // DateTime FinalTimeTo = DateTime.Now;
            //   DateTime FinalTimeFrom = DateTime.Now.AddHours(0).AddDays(-30).Date;
            //  DateTime FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date;
            DateTime FinalTimeFrom = DateTime.Now.AddHours(5).Date;
            DateTime FinalTimeTo = DateTime.Now.AddHours(5).AddDays(1).Date;
            string scriptString = "";
            string datap = "";
            string tim = "";
            string t = "";



            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {

                try
                {

                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'PumpStatus' ";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'   and rtp.TemplateID = 64 order by cast(r.ResourceID as int) asc";
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        //  scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"WaterFlow (Cusec)\" },";
                        scriptString += "subtitles: [{text: \" All Tubewells Recent Water Flow  \" }],";
                        scriptString += "axisY: {suffix: \" Cusec\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
                        scriptString += " data: [";
                        //   scriptString +=  dtPar.Rows[1]["ResourceID"];

                        // foreach (DataRow drPar in dtPar.Rows)
                        // {

                        if (dtPar.Rows.Count > 0)
                        {
                            DataRow drPar = dtPar.Rows[0];

                            // scriptString += drPar["ResourceID"];
                            // scriptString += drPar["ResourceID"];

                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            //aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime  > DATE_SUB(NOW(),â€‹INTERVAL 1";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            // aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + 130 + " or e.ParameterID = " + 118 + "and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime DESC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            // Loc += drPar["ResourceName"].ToString();
                            scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} Cusec</strong> at {x}\", ";

                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {

                                dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                dt = Convert.ToDateTime(drVal["InsertionDateTime"]);
                                datap = JsonConvert.SerializeObject(dataPoints) + "";

                                //}
                                // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]). - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }

                            scriptString += "dataPoints: " + JsonConvert.SerializeObject(dataPoints) + "";

                            datap = JsonConvert.SerializeObject(dataPoints) + "";



                            // datap += dataPoints;
                            scriptString += "},";
                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString = scriptString + "]";
                        scriptString = scriptString + "}";
                        scriptString += ");";

                    }
                }

                catch (Exception ex)
                {

                }
            }
            string NewscripString = datap;
            return NewscripString;






        }
    }
}



