using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WASA_EMS.Models;

namespace WASA_EMS.Controllers
{
    public class DisposalsController : Controller
    {
        // GET: Disposals
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult WellReport()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("Shaukat Khanum Disposal");
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Disposals";
            rs.resourceName = "Shaukat Khanum Disposal";
            rs.dateFrom = "";
            rs.timeFrom = "";
            rs.dateTo = "";
            rs.timeTo = "";
            ViewBag.ResourceList = ResourceList;
            return View(rs);
        }
        [HttpPost]
        public ActionResult WellReport(FormCollection review)
        {
            string resource = review["resource"].ToString();
            DateTime dateFrom = DateTime.Parse(review["dateFrom"].ToString());
            DateTime dateTo = DateTime.Parse(review["dateTo"].ToString());
            string df_date = dateFrom.ToString("d");
            string dt_date = dateTo.ToString("d");
            string TF = review["timeFrom"];
            string TT = review["timeTo"];
            string abc = review["timeFrom"];
            string[] abc1 = abc.Split(',');
            string a = abc1[0];
            if (abc1.Length > 1)
            {
                TF = abc1[1];
            }
            else
            {
                TF = abc1[0];
            }
            DataTable dt121 = new DataTable();
            Session["TimeFrom"] = TF;
            DateTime timeFrom = DateTime.Parse(TF);
            string cba = review["timeTo"];
            string[] cba1 = cba.Split(',');
            TT = cba1[0];
            DateTime timeTo = DateTime.Parse(TT);
            string tf_time = timeFrom.ToString("t");
            string tt_time = timeTo.ToString("t");
            if (tt_time == "12:00 AM")
            {
                tt_time = "11:59:59 PM";
            }
            DateTime FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
            DateTime FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("Shaukat Khanum Disposal");
            ViewBag.ResourceList = ResourceList;
            ViewBag.SelectedResource = resource;
            ViewBag.SelectedTimeFrom = TF;
            ViewBag.SelectedTimeTo = TT;
            ViewBag.SelectedTimeFrom = TF.ToString();
            ViewBag.SelectedTimeTo = TT.ToString();
            ViewBag.timeFrom = TF;
            ViewBag.timeTo = TT;
            ViewBag.dateFrom = df_date;
            ViewBag.dateTo = dt_date;
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Disposals";
            rs.resourceName = resource;
            rs.dateFrom = dateFrom.ToString();
            rs.timeFrom = TF;
            rs.dateTo = dateTo.ToString();
            rs.timeTo = TT;
            return View(rs);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        public PartialViewResult _WellsReportView(string resources, string datFrom, string timFrom, string datTo, string timTo)
        {
            DateTime FinalTimeFrom = DateTime.Now;
            DateTime FinalTimeTo = DateTime.Now;
            if (datFrom == "" && timFrom == "" && datTo == "" && timTo == "")
            {
                FinalTimeFrom = DateTime.Now.AddHours(0).Date;
                FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date;
            }
            else
            {
                DateTime dateFrom = DateTime.Parse(datFrom);
                DateTime dateTo = DateTime.Parse(datTo);
                string df_date = dateFrom.ToString("d");
                string dt_date = dateTo.ToString("d");
                string TF = timFrom;
                string TT = timTo;
                string abc = timFrom;
                string[] abc1 = abc.Split(',');
                string a = abc1[0];
                if (abc1.Length > 1)
                {
                    TF = abc1[1];
                }
                else
                {
                    TF = abc1[0];
                }
                DataTable dt121 = new DataTable();
                Session["TimeFrom"] = TF;
                DateTime timeFrom = DateTime.Parse(TF);
                string cba = timTo;
                string[] cba1 = cba.Split(',');
                TT = cba1[0];
                DateTime timeTo = DateTime.Parse(TT);
                string tf_time = timeFrom.ToString("t");
                string tt_time = timeTo.ToString("t");
                if (tt_time == "12:00 AM")
                {
                    tt_time = "11:59:59 PM";
                }
                FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
                FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            }
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var tubewellDataList = new List<DisposalDataClass>();
            //DisposalDataClass df = new DisposalDataClass();
            DisposalDataClassFinal df = new DisposalDataClassFinal();
            df.PumpStatus1 = new List<double>();
            df.PumpStatus6 = new List<double>();
            df.Well1Level = new List<double>();
            df.Well2Level = new List<double>();
            var disposalFinalDataList = new List<DisposalDataClassFinal>();
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1059  and ";
                    //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                    Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus1.],[Well1Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    SqlCommand cmd = new SqlCommand(Dashdtquery, conn);
                    SqlDataAdapter sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump1(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.locationName1 = "Pump Set 1";
                        df.PumpStatus1 = sd.PumpStatus1;
                        df.WorkingInHoursPump1 = sd.WorkingInHoursPump1;
                        df.Well1Level = sd.Well1Level;
                        df.Well1Level_Average = df.Well1Level.DefaultIfEmpty().Average().ToString();
                        df.Pump1TimeArray = sd.Pump1TimeArray;
                        df.WorkingHoursPump1 = sd.WorkingHoursPump1;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.locationName1 = "Pump Set 1";
                        df.PumpStatus1 = new List<double>();
                        df.Well1Level = new List<double>();
                        df.Pump1TimeArray = new List<string>();
                        df.Well1Level_Average = "0";
                        df.WorkingInHoursPump1 = "0";
                        df.WorkingHoursPump1 = 0;
                    }
                    //////////////////////////////////////////////////
                    //////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1060  and ";
                    //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                    Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus6.],[Well2Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump6(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.locationName2 = "Pump Set 2";
                        df.PumpStatus6 = sd.PumpStatus6;
                        df.WorkingInHoursPump6 = sd.WorkingInHoursPump6;
                        df.Well2Level = sd.Well2Level;
                        df.Well2Level_Average = sd.Well2Level_Average;
                        df.Pump6TimeArray = sd.Pump6TimeArray;
                        df.WorkingHoursPump6 = sd.WorkingHoursPump6;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.locationName2 = "Pump Set 2";
                        df.PumpStatus6 = new List<double>();
                        df.Well2Level = new List<double>();
                        df.Well2Level_Average = "0";
                        df.WorkingInHoursPump6 = "0";
                        df.Pump6TimeArray = new List<string>();
                        df.WorkingHoursPump6 = 0;
                    }
                    ////////////////////////////////////////////////////
                    disposalFinalDataList.Add(df);
                }
                catch (Exception ex)
                {
                    // Get stack trace for the exception with source file information
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                }
                conn.Close();
            }
            Session["ReportTitle"] = "Wet Wells Report at Shaukat Khanum Disposal Station between " + FinalTimeFrom + " to " + FinalTimeTo + "";
            string jsonPacket = JsonConvert.SerializeObject(disposalFinalDataList);
            ViewData["TheDisposalData"] = jsonPacket;


            /////////////////////////////////////////////////////////////////////////////

            string scriptString = "";
            scriptString += "var chart1 = new CanvasJS.Chart(\"chartContainer1\", {";
            scriptString += "theme: \"light2\",";
            scriptString += "animationEnabled: true,";
            scriptString += "zoomEnabled: true, ";
            scriptString += "title: {text: \"Shaukat Khanum Disposal Station\" },exportEnabled: true,";
            scriptString += "subtitles: [{text: \" Wet Wells Level  \" }],";
            scriptString += "axisY: {suffix: \" \" },";
            scriptString += "toolTip: { shared: false },";
            scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
            scriptString += " data: [";
            
            scriptString += "{ type: \"area\", name: \"Wet Well Level No.1\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} ft</strong> at {x}\", ";
            List<DataPoint> dataPointsTank1 = new List<DataPoint>();
            for (int i = 0; i < df.Pump1TimeArray.Count; i++)
            {
                dataPointsTank1.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump1TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.Well1Level[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsTank1) + "";
            scriptString += "},";


            scriptString += "{ type: \"area\", name: \"Wet Well Level No. 2\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} ft</strong> at {x}\", ";
            List<DataPoint> dataPointsTank2 = new List<DataPoint>();
            for (int i = 0; i < df.Pump6TimeArray.Count; i++)
            {
                dataPointsTank2.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump6TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.Well2Level[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsTank2) + "";
            scriptString += "}";
            /////////////
            scriptString = scriptString + "]";
            scriptString = scriptString + "}";
            scriptString += ");";
            ViewData["chartData"] = scriptString;
            /////////////////////////////////////////////////////////////////////////////////






            return PartialView(disposalFinalDataList);
        }

        public ActionResult PumpsReport()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("Shaukat Khanum Disposal");
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Disposals";
            rs.resourceName = "Shaukat Khanum Disposal";
            rs.dateFrom = "";
            rs.timeFrom = "";
            rs.dateTo = "";
            rs.timeTo = "";
            ViewBag.ResourceList = ResourceList;
            return View(rs);
        }
        [HttpPost]
        public ActionResult PumpsReport(FormCollection review)
        {
            string resource = review["resource"].ToString();
            DateTime dateFrom = DateTime.Parse(review["dateFrom"].ToString());
            DateTime dateTo = DateTime.Parse(review["dateTo"].ToString());
            string df_date = dateFrom.ToString("d");
            string dt_date = dateTo.ToString("d");
            string TF = review["timeFrom"];
            string TT = review["timeTo"];
            string abc = review["timeFrom"];
            string[] abc1 = abc.Split(',');
            string a = abc1[0];
            if (abc1.Length > 1)
            {
                TF = abc1[1];
            }
            else
            {
                TF = abc1[0];
            }
            DataTable dt121 = new DataTable();
            Session["TimeFrom"] = TF;
            DateTime timeFrom = DateTime.Parse(TF);
            string cba = review["timeTo"];
            string[] cba1 = cba.Split(',');
            TT = cba1[0];
            DateTime timeTo = DateTime.Parse(TT);
            string tf_time = timeFrom.ToString("t");
            string tt_time = timeTo.ToString("t");
            if (tt_time == "12:00 AM")
            {
                tt_time = "11:59:59 PM";
            }
            DateTime FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
            DateTime FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("Shaukat Khanum Disposal");
            ViewBag.ResourceList = ResourceList;
            ViewBag.SelectedResource = resource;
            ViewBag.SelectedTimeFrom = TF;
            ViewBag.SelectedTimeTo = TT;
            ViewBag.SelectedTimeFrom = TF.ToString();
            ViewBag.SelectedTimeTo = TT.ToString();
            ViewBag.timeFrom = TF;
            ViewBag.timeTo = TT;
            ViewBag.dateFrom = df_date;
            ViewBag.dateTo = dt_date;
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Disposals";
            rs.resourceName = resource;
            rs.dateFrom = dateFrom.ToString();
            rs.timeFrom = TF;
            rs.dateTo = dateTo.ToString();
            rs.timeTo = TT;
            return View(rs);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        public PartialViewResult _PumpsReportReportView(string resources, string datFrom, string timFrom, string datTo, string timTo)
        {
            DateTime FinalTimeFrom = DateTime.Now;
            DateTime FinalTimeTo = DateTime.Now;
            if (datFrom == "" && timFrom == "" && datTo == "" && timTo == "")
            {
                FinalTimeFrom = DateTime.Now.AddHours(0).Date;
                FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date;
            }
            else
            {
                DateTime dateFrom = DateTime.Parse(datFrom);
                DateTime dateTo = DateTime.Parse(datTo);
                string df_date = dateFrom.ToString("d");
                string dt_date = dateTo.ToString("d");
                string TF = timFrom;
                string TT = timTo;
                string abc = timFrom;
                string[] abc1 = abc.Split(',');
                string a = abc1[0];
                if (abc1.Length > 1)
                {
                    TF = abc1[1];
                }
                else
                {
                    TF = abc1[0];
                }
                DataTable dt121 = new DataTable();
                Session["TimeFrom"] = TF;
                DateTime timeFrom = DateTime.Parse(TF);
                string cba = timTo;
                string[] cba1 = cba.Split(',');
                TT = cba1[0];
                DateTime timeTo = DateTime.Parse(TT);
                string tf_time = timeFrom.ToString("t");
                string tt_time = timeTo.ToString("t");
                if (tt_time == "12:00 AM")
                {
                    tt_time = "11:59:59 PM";
                }
                FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
                FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            }
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var tubewellDataList = new List<DisposalDataClass>();
            //DisposalDataClass df = new DisposalDataClass();
            DisposalDataClassFinal df = new DisposalDataClassFinal();
            df.PumpStatus1 = new List<double>();
            df.PumpStatus2 = new List<double>();
            df.PumpStatus3 = new List<double>();
            df.PumpStatus4 = new List<double>();
            df.PumpStatus5 = new List<double>();
            df.PumpStatus6 = new List<double>();
            df.PumpStatus7 = new List<double>();
            df.PumpStatus8 = new List<double>();
            df.PumpStatus9 = new List<double>();
            df.PumpStatus10 = new List<double>();
            df.Well1Level = new List<double>();
            df.Well2Level = new List<double>();
            var disposalFinalDataList = new List<DisposalDataClassFinal>();
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1059  and ";
                    //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                    Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus1.],[Well1Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    SqlCommand cmd = new SqlCommand(Dashdtquery, conn);
                    SqlDataAdapter sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump1(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.locationName1 = "Pump Set 1";
                        df.PumpStatus1 = sd.PumpStatus1;
                        df.WorkingInHoursPump1 = sd.WorkingInHoursPump1;
                        df.Well1Level = sd.Well1Level;
                        df.Well1Level_Average = df.Well1Level.DefaultIfEmpty().Average().ToString();
                        df.Pump1TimeArray = sd.Pump1TimeArray;
                        df.WorkingHoursPump1 = sd.WorkingHoursPump1;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.locationName1 = "Pump Set 1";
                        df.PumpStatus1 = new List<double>();
                        df.Well1Level = new List<double>();
                        df.Pump1TimeArray = new List<string>();
                        df.Well1Level_Average = "0";
                        df.WorkingInHoursPump1 = "0";
                        df.WorkingHoursPump1 = 0;
                    }
                    //////////////////////////////////////////////////
                    //////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1059  and ";
                    //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                    Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus2.],[Well1Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump2(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus2 = sd.PumpStatus2;
                        df.WorkingInHoursPump2 = sd.WorkingInHoursPump2;
                        df.WorkingHoursPump2 = sd.WorkingHoursPump2;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.PumpStatus2 = new List<double>();
                        df.WorkingInHoursPump2 = "0";
                        df.WorkingHoursPump2 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1059  and ";
                    //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                    Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus3.],[Well1Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump3(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus3 = sd.PumpStatus3;
                        df.WorkingInHoursPump3 = sd.WorkingInHoursPump3;
                        df.WorkingHoursPump3 = sd.WorkingHoursPump3;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        tubewellDataList.Add(sd);
                        df.PumpStatus3 = new List<double>();
                        df.WorkingInHoursPump3 = "0";
                        df.WorkingHoursPump3 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1059  and ";
                    //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                    Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus4.],[Well1Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump4(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus4 = sd.PumpStatus4;
                        df.WorkingInHoursPump4 = sd.WorkingInHoursPump4;
                        df.WorkingHoursPump4 = sd.WorkingHoursPump4;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.PumpStatus4 = new List<double>();
                        df.WorkingInHoursPump4 = "0";
                        df.WorkingHoursPump4 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1059  and ";
                    //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                    Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus5.],[Well1Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump5(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus5 = sd.PumpStatus5;
                        df.WorkingInHoursPump5 = sd.WorkingInHoursPump5;
                        df.WorkingHoursPump5 = sd.WorkingHoursPump5;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        sd.PumpStatus5 = new List<double>();
                        df.WorkingInHoursPump5 = "0";
                        df.WorkingHoursPump5 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1060  and ";
                    //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                    Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus6.],[Well2Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump6(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.locationName2 = "Pump Set 2";
                        df.PumpStatus6 = sd.PumpStatus6;
                        df.WorkingInHoursPump6 = sd.WorkingInHoursPump6;
                        df.Well2Level = sd.Well2Level;
                        df.Well2Level_Average = sd.Well2Level_Average;
                        df.Pump6TimeArray = sd.Pump6TimeArray;
                        df.WorkingHoursPump6 = sd.WorkingHoursPump6;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.locationName2 = "Pump Set 2";
                        df.PumpStatus6 = new List<double>();
                        df.Well2Level = new List<double>();
                        df.Well2Level_Average = "0";
                        df.WorkingInHoursPump6 = "0";
                        df.Pump6TimeArray = new List<string>();
                        df.WorkingHoursPump6 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1060  and ";
                    //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                    Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus7.],[Well2Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump7(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus7 = sd.PumpStatus7;
                        df.WorkingInHoursPump7 = sd.WorkingInHoursPump7;
                        df.WorkingHoursPump7 = sd.WorkingHoursPump7;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.PumpStatus7 = new List<double>();
                        df.WorkingInHoursPump7 = "0";
                        df.WorkingHoursPump7 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1060  and ";
                    //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                    Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus8.],[Well2Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump8(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus8 = sd.PumpStatus8;
                        df.WorkingInHoursPump8 = sd.WorkingInHoursPump8;
                        df.WorkingHoursPump8 = sd.WorkingHoursPump8;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.PumpStatus8 = new List<double>();
                        df.WorkingInHoursPump8 = "0";
                        df.WorkingHoursPump8 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1060  and ";
                    //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                    Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus9.],[Well2Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump9(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus9 = sd.PumpStatus9;
                        df.WorkingInHoursPump9 = sd.WorkingInHoursPump9;
                        df.WorkingHoursPump9 = sd.WorkingHoursPump9;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.PumpStatus9 = new List<double>();
                        df.WorkingInHoursPump9 = "0";
                        df.WorkingHoursPump9 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1060  and ";
                    //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                    Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatuss10.],[Well2Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump10(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus10 = sd.PumpStatus10;
                        df.WorkingInHoursPump10 = sd.WorkingInHoursPump10;
                        df.WorkingHoursPump10 = sd.WorkingHoursPump10;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.PumpStatus10 = new List<double>();
                        df.WorkingInHoursPump10 = "0";
                        df.WorkingHoursPump10 = 0;
                    }
                    df.TotalWorkingHours = df.WorkingHoursPump1 +
                        df.WorkingHoursPump2 +
                        df.WorkingHoursPump3 +
                        df.WorkingHoursPump4 +
                        df.WorkingHoursPump5 +
                        df.WorkingHoursPump6 +
                        df.WorkingHoursPump7 +
                        df.WorkingHoursPump8 +
                        df.WorkingHoursPump9 +
                        df.WorkingHoursPump10;
                    var pp = TimeSpan.FromMinutes(df.TotalWorkingHours*1);
                    int phour = (int)pp.TotalHours;
                    int pmin = (int)pp.Minutes;
                    int psec = (int)pp.Seconds;
                    string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                    df.TotalWorkingInHours = pstr;
                    disposalFinalDataList.Add(df);
                }
                catch (Exception ex)
                {
                    // Get stack trace for the exception with source file information
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                }
                conn.Close();
            }
            Session["ReportTitle"] = "Pumps Working Report at Shaukat Khanum Disposal Station between " + FinalTimeFrom + " to " + FinalTimeTo + "";
            string jsonPacket = JsonConvert.SerializeObject(disposalFinalDataList);
            ViewData["TheDisposalData"] = jsonPacket;


            /////////////////////////////////////////////////////////////////////////////

            string scriptString = "";
            scriptString += "var chart1 = new CanvasJS.Chart(\"chartContainer1\", {";
            scriptString += "theme: \"light2\",";
            scriptString += "animationEnabled: true,";
            scriptString += "zoomEnabled: true, ";
            scriptString += "title: {text: \"Shaukat Khanum Disposal Station\" },exportEnabled: true,";
            scriptString += "subtitles: [{text: \" Pumps Status  \" }],";
            //scriptString += "axisY: {suffix: \" \" },";
            scriptString += "axisY: {labelFontSize: 10, labelFormatter: function(){ return \" \"; }},";
            //scriptString += "toolTip: { shared: false },";
            scriptString += "toolTip: { shared: false , contentFormatter: function(e){ var str = \" \" ; for (var i = 0; i < e.entries.length; i++){ var utcSeconds = e.entries[i].dataPoint.x; var d = new Date(utcSeconds); if(e.entries[i].dataPoint.y == 0){ var temp = e.entries[i].dataSeries.name + \" \" +\": OFF at  \" + d.toLocaleString('en-IN'); str = str+temp; } else { var temp = e.entries[i].dataSeries.name + \" \" +\": ON at \" + d.toLocaleString('en-IN'); str = str+temp; } } return (str); }},";
            scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
            scriptString += " data: [";
            scriptString += "{ type: \"area\", name: \"Pump No. 1\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\",   ";
            List<DataPoint> dataPointsPump1 = new List<DataPoint>();
            for(int i = 0; i<df.Pump1TimeArray.Count; i++)
            {
                dataPointsPump1.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump1TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus1[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump1) + "";
            scriptString += "},";

            scriptString += "{ type: \"area\", name: \"Pump No. 2\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\",   ";
            List<DataPoint> dataPointsPump2 = new List<DataPoint>();
            for (int i = 0; i < df.Pump1TimeArray.Count; i++)
            {
                dataPointsPump2.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump1TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus2[i])));

            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump2) + "";
            scriptString += "},";

            scriptString += "{ type: \"area\", name: \"Pump No. 3\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\",   ";
            List<DataPoint> dataPointsPump3 = new List<DataPoint>();
            for (int i = 0; i < df.Pump1TimeArray.Count; i++)
            {
                dataPointsPump3.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump1TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus3[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump3) + "";
            scriptString += "},";

            scriptString += "{ type: \"area\", name: \"Pump No. 4\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\",   ";
            List<DataPoint> dataPointsPump4 = new List<DataPoint>();
            for (int i = 0; i < df.Pump1TimeArray.Count; i++)
            {
                dataPointsPump4.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump1TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus4[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump4) + "";
            scriptString += "},";

            scriptString += "{ type: \"area\", name: \"Pump No. 5\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\",   ";
            List<DataPoint> dataPointsPump5 = new List<DataPoint>();
            for (int i = 0; i < df.Pump1TimeArray.Count; i++)
            {
                dataPointsPump5.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump1TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus5[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump5) + "";
            scriptString += "},";

            scriptString += "{ type: \"area\", name: \"Pump No. 6\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\",   ";
            List<DataPoint> dataPointsPump6 = new List<DataPoint>();
            for (int i = 0; i < df.Pump6TimeArray.Count; i++)
            {
                dataPointsPump6.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump6TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus6[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump6) + "";
            scriptString += "},";



            scriptString += "{ type: \"area\", name: \"Pump No. 7\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\",   ";
            List<DataPoint> dataPointsPump7 = new List<DataPoint>();
            for (int i = 0; i < df.Pump6TimeArray.Count; i++)
            {
                dataPointsPump7.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump6TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus7[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump7) + "";
            scriptString += "},";


            scriptString += "{ type: \"area\", name: \"Pump No. 8\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\",   ";
            List<DataPoint> dataPointsPump8 = new List<DataPoint>();
            for (int i = 0; i < df.Pump6TimeArray.Count; i++)
            {
                dataPointsPump8.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump6TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus8[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump8) + "";
            scriptString += "},";



            scriptString += "{ type: \"area\", name: \"Pump No. 9\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\",   ";
            List<DataPoint> dataPointsPump9 = new List<DataPoint>();
            for (int i = 0; i < df.Pump6TimeArray.Count; i++)
            {
                dataPointsPump9.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump6TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus9[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump9) + "";
            scriptString += "},";


            scriptString += "{ type: \"area\", name: \"Pump No. 10\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\",   ";
            List<DataPoint> dataPointsPump10 = new List<DataPoint>();
            for (int i = 0; i < df.Pump6TimeArray.Count; i++)
            {
                dataPointsPump10.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump6TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus10[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump10) + "";
            
            scriptString += "}";
            /////////////
            scriptString = scriptString + "]";
            scriptString = scriptString + "}";
            scriptString += ");";
            ViewData["chartData"] = scriptString;
            /////////////////////////////////////////////////////////////////////////////////






            return PartialView(disposalFinalDataList);
        }

        public ActionResult StatusReport()
        {
            return View();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        public PartialViewResult _StatusReportView()
        {
            DisposalDataClassFinal df = new DisposalDataClassFinal();
            df.PumpStatus1 = new List<double>();
            df.PumpStatus2 = new List<double>();
            df.PumpStatus3 = new List<double>();
            df.PumpStatus4 = new List<double>();
            df.PumpStatus5 = new List<double>();
            df.PumpStatus6 = new List<double>();
            df.PumpStatus7 = new List<double>();
            df.PumpStatus8 = new List<double>();
            df.PumpStatus9 = new List<double>();
            df.PumpStatus10 = new List<double>();
            df.Well1Level = new List<double>();
            df.Well2Level = new List<double>();
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var tubewellDataList = new List<DisposalDataClass>();
            var disposalFinalDataList = new List<DisposalDataClassFinal>();
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1059  and ";
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus1.],[Well1Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    SqlCommand cmd = new SqlCommand(Dashdtquery, conn);
                    SqlDataAdapter sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump1(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.locationName1 = "Pump Set 1";
                        df.PumpStatus1 = sd.PumpStatus1;
                        df.WorkingInHoursPump1 = sd.WorkingInHoursPump1;
                        df.Well1Level = sd.Well1Level;
                        df.Well1Level_Average = df.Well1Level.DefaultIfEmpty().Average().ToString();
                        df.Pump1TimeArray = sd.Pump1TimeArray;
                        df.WorkingHoursPump1 = sd.WorkingHoursPump1;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.locationName1 = "Pump Set 1";
                        df.PumpStatus1 = new List<double>();
                        df.Well1Level = new List<double>();
                        df.Well1Level_Average = "0";
                        df.WorkingInHoursPump1 = "0";
                        df.Pump1TimeArray = new List<string>();
                        df.WorkingHoursPump1 = 0;
                    }
                    //////////////////////////////////////////////////
                    //////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1059  and ";
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus2.],[Well1Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump2(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus2 = sd.PumpStatus2;
                        df.WorkingInHoursPump2 = sd.WorkingInHoursPump2;
                        df.WorkingHoursPump2 = sd.WorkingHoursPump2;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.PumpStatus2 = new List<double>();
                        df.WorkingInHoursPump2 = "0";
                        df.WorkingHoursPump2 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1059  and ";
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus3.],[Well1Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump3(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus3 = sd.PumpStatus3;
                        df.WorkingInHoursPump3 = sd.WorkingInHoursPump3;
                        df.WorkingHoursPump3 = sd.WorkingHoursPump3;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        tubewellDataList.Add(sd);
                        df.PumpStatus3 = new List<double>();
                        df.WorkingInHoursPump3 = "0";
                        df.WorkingHoursPump3 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour,0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1059  and ";
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus4.],[Well1Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump4(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus4 = sd.PumpStatus4;
                        df.WorkingInHoursPump4 = sd.WorkingInHoursPump4;
                        df.WorkingHoursPump4 = sd.WorkingHoursPump4;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.PumpStatus4 = new List<double>();
                        df.WorkingInHoursPump4 = "0";
                        df.WorkingHoursPump4 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour,0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1059  and ";
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus5.],[Well1Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump5(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus5 = sd.PumpStatus5;
                        df.WorkingInHoursPump5 = sd.WorkingInHoursPump5;
                        df.WorkingHoursPump5 = sd.WorkingHoursPump5;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        sd.PumpStatus5 = new List<double>();
                        df.WorkingInHoursPump5 = "0";
                        df.WorkingHoursPump5 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1060  and ";
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus6.],[Well2Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump6(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.locationName2 = "Pump Set 2";
                        df.PumpStatus6 = sd.PumpStatus6;
                        df.WorkingInHoursPump6 = sd.WorkingInHoursPump6;
                        df.Well2Level = sd.Well2Level;
                        df.Well2Level_Average = sd.Well2Level_Average;
                        df.Pump6TimeArray = sd.Pump6TimeArray;
                        df.WorkingHoursPump6 = sd.WorkingHoursPump6;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.locationName2 = "Pump Set 2";
                        df.PumpStatus6 = new List<double>();
                        df.Well2Level = new List<double>();
                        df.Well2Level_Average = "0";
                        df.WorkingInHoursPump5 = "0";
                        df.Pump6TimeArray = new List<string>();
                        df.WorkingHoursPump6 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1060  and ";
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus7.],[Well2Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump7(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus7 = sd.PumpStatus7;
                        df.WorkingInHoursPump7 = sd.WorkingInHoursPump7;
                        df.WorkingHoursPump7 = sd.WorkingHoursPump7;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.PumpStatus7 = new List<double>();
                        df.WorkingInHoursPump7 = "0";
                        df.WorkingHoursPump7 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1060  and ";
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus8.],[Well2Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump8(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus8 = sd.PumpStatus8;
                        df.WorkingInHoursPump8 = sd.WorkingInHoursPump8;
                        df.WorkingHoursPump8 = sd.WorkingHoursPump8;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.PumpStatus8 = new List<double>();
                        df.WorkingInHoursPump8 = "0";
                        df.WorkingHoursPump8 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1060  and ";
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatus9.],[Well2Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump9(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus9 = sd.PumpStatus9;
                        df.WorkingInHoursPump9 = sd.WorkingInHoursPump9;
                        df.WorkingHoursPump9 = sd.WorkingHoursPump9;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.PumpStatus9 = new List<double>();
                        df.WorkingInHoursPump9 = "0";
                        df.WorkingHoursPump9 = 0;
                    }
                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1060  and ";
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[PumpStatuss10.],[Well2Level(ft)] ";
                    Dashdtquery += ") ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "AS PivotTable ";
                    Dashdtquery += ")  ";
                    Dashdtquery += "SELECT* FROM cte ";
                    Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    Dashdtquery += "tim DESC";
                    cmd = new SqlCommand(Dashdtquery, conn);
                    sda = new SqlDataAdapter(Dashdtquery, conn);
                    Dashdt.Clear();
                    sda.Fill(Dashdt);
                    if (Dashdt.Rows.Count > 0)
                    {
                        DisposalDataClass sd = getAllSpellsForPump10(Dashdt);
                        //tubewellDataList.Add(sd);
                        df.PumpStatus10 = sd.PumpStatus10;
                        df.WorkingInHoursPump10 = sd.WorkingInHoursPump10;
                        df.WorkingHoursPump10 = sd.WorkingHoursPump10;
                    }
                    else
                    {
                        DisposalDataClass sd = new DisposalDataClass();
                        //tubewellDataList.Add(sd);
                        df.PumpStatus10 = new List<double>();
                        df.WorkingInHoursPump10 = "0";
                        df.WorkingHoursPump10 = 0;
                    }
                    disposalFinalDataList.Add(df);
                }
                catch (Exception ex)
                {
                    // Get stack trace for the exception with source file information
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                }
                conn.Close();
            }
            Session["ReportTitle"] = "Current Status of Shaukat Khanum Disposal Station at  " + DateTime.Now.AddHours(0).ToString("dd'/'MM'/'yyyy HH:mm:ss") + " (Reflected for Today)";

            /////////////////////////////////////////////////////////////////////////////

            string scriptString = "";
            scriptString += "var chart1 = new CanvasJS.Chart(\"chartContainer1\", {";
            scriptString += "theme: \"light2\",";
            scriptString += "animationEnabled: true,";
            scriptString += "zoomEnabled: true, ";
            scriptString += "title: {text: \"Shaukat Khanum Disposal Station\" },exportEnabled: true,";
            scriptString += "subtitles: [{text: \" Pumps Working and Wells Level in ft  \" }],";
            scriptString += "axisY: {suffix: \" \" },";
            scriptString += "toolTip: { shared: false },";
            scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
            scriptString += " data: [";
            scriptString += "{ type: \"area\", name: \"Pump No. 1\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
            List<DataPoint> dataPointsPump1 = new List<DataPoint>();
            for (int i = 0; i < df.Pump1TimeArray.Count; i++)
            {
                dataPointsPump1.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump1TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus1[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump1) + "";
            scriptString += "},";

            scriptString += "{ type: \"area\", name: \"Pump No. 2\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
            List<DataPoint> dataPointsPump2 = new List<DataPoint>();
            for (int i = 0; i < df.Pump1TimeArray.Count; i++)
            {
                dataPointsPump2.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump1TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus2[i])));

            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump2) + "";
            scriptString += "},";

            scriptString += "{ type: \"area\", name: \"Pump No. 3\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
            List<DataPoint> dataPointsPump3 = new List<DataPoint>();
            for (int i = 0; i < df.Pump1TimeArray.Count; i++)
            {
                dataPointsPump3.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump1TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus3[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump3) + "";
            scriptString += "},";

            scriptString += "{ type: \"area\", name: \"Pump No. 4\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
            List<DataPoint> dataPointsPump4 = new List<DataPoint>();
            for (int i = 0; i < df.Pump1TimeArray.Count; i++)
            {
                dataPointsPump4.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump1TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus4[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump4) + "";
            scriptString += "},";

            scriptString += "{ type: \"area\", name: \"Pump No. 5\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
            List<DataPoint> dataPointsPump5 = new List<DataPoint>();
            for (int i = 0; i < df.Pump1TimeArray.Count; i++)
            {
                dataPointsPump5.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump1TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus5[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump5) + "";
            scriptString += "},";


            scriptString += "{ type: \"area\", name: \"Pump No. 6\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
            List<DataPoint> dataPointsPump6 = new List<DataPoint>();
            for (int i = 0; i < df.Pump6TimeArray.Count; i++)
            {
                dataPointsPump6.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump6TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus6[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump6) + "";
            scriptString += "},";



            scriptString += "{ type: \"area\", name: \"Pump No. 7\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
            List<DataPoint> dataPointsPump7 = new List<DataPoint>();
            for (int i = 0; i < df.Pump6TimeArray.Count; i++)
            {
                dataPointsPump7.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump6TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus7[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump7) + "";
            scriptString += "},";


            scriptString += "{ type: \"area\", name: \"Pump No. 8\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
            List<DataPoint> dataPointsPump8 = new List<DataPoint>();
            for (int i = 0; i < df.Pump6TimeArray.Count; i++)
            {
                dataPointsPump8.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump6TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus8[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump8) + "";
            scriptString += "},";



            scriptString += "{ type: \"area\", name: \"Pump No. 9\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
            List<DataPoint> dataPointsPump9 = new List<DataPoint>();
            for (int i = 0; i < df.Pump6TimeArray.Count; i++)
            {
                dataPointsPump9.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump6TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus9[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump9) + "";
            scriptString += "},";


            scriptString += "{ type: \"area\", name: \"Pump No. 10\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
            List<DataPoint> dataPointsPump10 = new List<DataPoint>();
            for (int i = 0; i < df.Pump6TimeArray.Count; i++)
            {
                dataPointsPump10.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump6TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus10[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump10) + "";
            scriptString += "},";


            scriptString += "{ type: \"area\", name: \"Wet Well Level No. 1\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} ft</strong> at {x}\", ";
            List<DataPoint> dataPointsTank1 = new List<DataPoint>();
            for (int i = 0; i < df.Pump1TimeArray.Count; i++)
            {
                dataPointsTank1.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump1TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.Well1Level[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsTank1) + "";
            scriptString += "},";

            scriptString += "{ type: \"area\", name: \"Wet Well Level No. 2\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} ft</strong> at {x}\", ";
            List<DataPoint> dataPointsTank2 = new List<DataPoint>();
            for (int i = 0; i < df.Pump6TimeArray.Count; i++)
            {
                dataPointsTank2.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.Pump6TimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.Well2Level[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsTank2) + "";
            scriptString += "}";
            /////////////
            scriptString = scriptString + "]";
            scriptString = scriptString + "}";
            scriptString += ");";
            ViewData["chartData"] = scriptString;
            /////////////////////////////////////////////////////////////////////////////////


            return PartialView(disposalFinalDataList);
        }

        public DisposalDataClass getAllSpellsForPump1(DataTable dt)
        {
            var tableData = new DisposalDataClass();
            var spelldata = new DisposalPump1SpellData();
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus1."])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<DisposalPump1SpellData> spellDataList = new List<DisposalPump1SpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus1."])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["Well1Level(ft)"])), 2);
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 1)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump1SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump1SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            string c = JsonConvert.SerializeObject(spellDataList);
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                tableData.Well1Level = new List<double>();
                tableData.Pump1TimeArray = new List<string>();
                //tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus1.Add(Convert.ToInt32(dr["PumpStatus1."]));
                    tableData.Well1Level.Add(Convert.ToDouble(dr["Well1Level(ft)"]));
                    tableData.Pump1TimeArray.Add((dr["tim"]).ToString());
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump1 = pstr;
                tableData.WorkingHoursPump1 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump1 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                if (spellDataList.Count == 0)
                {
                    tableData.Well1Level_Average = "0";
                }
                else
                {
                    double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                    if (avgWaterFlow == 0)
                    {
                        avgWaterFlow = 1;
                    }
                    tableData.Well1Level_Average = tableData.Well1Level.DefaultIfEmpty().Average().ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                }
            }
            else
            {
                tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                tableData.Well1Level = new List<double>();
                tableData.Pump1TimeArray = new List<string>();
                //tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus1.Add(Convert.ToInt32(dr["PumpStatus1."]));
                    tableData.Well1Level.Add(Convert.ToDouble(dr["Well1Level(ft)"]));
                    tableData.Pump1TimeArray.Add((dr["tim"]).ToString());
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump1 = pstr;
                tableData.WorkingHoursPump1 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump1 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                tableData.Well1Level_Average = tableData.Well1Level.DefaultIfEmpty().Average().ToString();
            }
            tableData.Well1Level_Average = tableData.Well1Level.DefaultIfEmpty().Average().ToString();
            //tableData.Pump1TimeArray = spellDataList.FirstOrDefault().SpellTimeArray;
            return tableData;
        }

        /// <spell for Pump 2>
        public DisposalDataClass getAllSpellsForPump2(DataTable dt)
        {
            var tableData = new DisposalDataClass();
            var spelldata = new DisposalPump2SpellData();
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus2."])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<DisposalPump2SpellData> spellDataList = new List<DisposalPump2SpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus2."])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["Well1Level(ft)"])), 2);
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 1)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump2SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump2SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            string c = JsonConvert.SerializeObject(spellDataList);
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                //tableData.PumpStatus1 = new List<double>();
                tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                tableData.Well1Level = new List<double>();
                //tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus2.Add(Convert.ToInt32(dr["PumpStatus2."]));
                    tableData.Well1Level.Add(Convert.ToDouble(dr["Well1Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump2 = pstr;
                tableData.WorkingHoursPump2 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump2 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                if (spellDataList.Count == 0)
                {
                    tableData.Well1Level_Average = "0";
                }
                else
                {
                    double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                    if (avgWaterFlow == 0)
                    {
                        avgWaterFlow = 1;
                    }
                    tableData.Well1Level_Average = tableData.Well1Level.DefaultIfEmpty().Average().ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                }
            }
            else
            {
                //tableData.PumpStatus1 = new List<double>();
                tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                tableData.Well1Level = new List<double>();
                //tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus2.Add(Convert.ToInt32(dr["PumpStatus2."]));
                    tableData.Well1Level.Add(Convert.ToDouble(dr["Well1Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump2 = pstr;
                tableData.WorkingHoursPump2 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump2 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                tableData.Well1Level_Average = tableData.Well1Level.DefaultIfEmpty().Average().ToString();
            }
            tableData.Well1Level_Average = tableData.Well1Level.DefaultIfEmpty().Average().ToString();
            return tableData;
        }
        /// <spell for Pump 2>

        /////////////////////////////Spell For Pump 3////////////////////////////////////////
        public DisposalDataClass getAllSpellsForPump3(DataTable dt)
        {
            var tableData = new DisposalDataClass();
            var spelldata = new DisposalPump3SpellData();
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus3."])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<DisposalPump3SpellData> spellDataList = new List<DisposalPump3SpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus3."])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["Well1Level(ft)"])), 2);
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 1)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump3SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump3SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            string c = JsonConvert.SerializeObject(spellDataList);
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                tableData.Well1Level = new List<double>();
                //tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus3.Add(Convert.ToInt32(dr["PumpStatus3."]));
                    tableData.Well1Level.Add(Convert.ToDouble(dr["Well1Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump3 = pstr;
                tableData.WorkingHoursPump3 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump3 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                if (spellDataList.Count == 0)
                {
                    tableData.Well1Level_Average = "0";
                }
                else
                {
                    double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                    if (avgWaterFlow == 0)
                    {
                        avgWaterFlow = 1;
                    }
                    tableData.Well1Level_Average = tableData.Well1Level.DefaultIfEmpty().Average().ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                }
            }
            else
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                tableData.Well1Level = new List<double>();
                //tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus3.Add(Convert.ToInt32(dr["PumpStatus3."]));
                    tableData.Well1Level.Add(Convert.ToDouble(dr["Well1Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump3 = pstr;
                tableData.WorkingHoursPump3 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump3 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                tableData.Well1Level_Average = tableData.Well1Level.DefaultIfEmpty().Average().ToString();
            }
            tableData.Well1Level_Average = tableData.Well1Level.DefaultIfEmpty().Average().ToString();
            return tableData;
        }
        /////////////////////////////Spell For Pump 3////////////////////////////////////////

        /// <Spell For Pump 4>
        /// /////////////////////////////////////////////////////////////////
        public DisposalDataClass getAllSpellsForPump4(DataTable dt)
        {
            var tableData = new DisposalDataClass();
            var spelldata = new DisposalPump4SpellData();
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus4."])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<DisposalPump4SpellData> spellDataList = new List<DisposalPump4SpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus4."])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["Well1Level(ft)"])), 2);
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 1)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump4SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump4SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            string c = JsonConvert.SerializeObject(spellDataList);
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                tableData.Well1Level = new List<double>();
                //tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus4.Add(Convert.ToInt32(dr["PumpStatus4."]));
                    tableData.Well1Level.Add(Convert.ToDouble(dr["Well1Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump4 = pstr;
                tableData.WorkingHoursPump4 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump4 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                if (spellDataList.Count == 0)
                {
                    tableData.Well1Level_Average = "0";
                }
                else
                {
                    double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                    if (avgWaterFlow == 0)
                    {
                        avgWaterFlow = 1;
                    }
                    tableData.Well1Level_Average = tableData.Well1Level.DefaultIfEmpty().Average().ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                }
            }
            else
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                tableData.Well1Level = new List<double>();
                //tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus4.Add(Convert.ToInt32(dr["PumpStatus4."]));
                    tableData.Well1Level.Add(Convert.ToDouble(dr["Well1Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump4 = pstr;
                tableData.WorkingHoursPump4 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump4 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                tableData.Well1Level_Average = tableData.Well1Level.DefaultIfEmpty().Average().ToString();
            }
            tableData.Well1Level_Average = tableData.Well1Level.DefaultIfEmpty().Average().ToString();
            return tableData;
        }
        /// <Spell For Pump 4>

        ////////////////////////////Spell For Pump 5///////////////////////////////////////////
        
        public DisposalDataClass getAllSpellsForPump5(DataTable dt)
        {
            var tableData = new DisposalDataClass();
            var spelldata = new DisposalPump5SpellData();
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus5."])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<DisposalPump5SpellData> spellDataList = new List<DisposalPump5SpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus5."])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["Well1Level(ft)"])), 2);
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 1)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump5SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump5SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            string c = JsonConvert.SerializeObject(spellDataList);
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                tableData.Well1Level = new List<double>();
                //tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus5.Add(Convert.ToInt32(dr["PumpStatus5."]));
                    tableData.Well1Level.Add(Convert.ToDouble(dr["Well1Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump5 = pstr;
                tableData.WorkingHoursPump5 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump5 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                if (spellDataList.Count == 0)
                {
                    tableData.Well1Level_Average = "0";
                }
                else
                {
                    double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                    if (avgWaterFlow == 0)
                    {
                        avgWaterFlow = 1;
                    }
                    tableData.Well1Level_Average = tableData.Well1Level.DefaultIfEmpty().Average().ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                }
            }
            else
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                tableData.Well1Level = new List<double>();
                //tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus5.Add(Convert.ToInt32(dr["PumpStatus5."]));
                    tableData.Well1Level.Add(Convert.ToDouble(dr["Well1Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump5 = pstr;
                tableData.WorkingHoursPump5 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump5 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                tableData.Well1Level_Average = tableData.Well1Level.DefaultIfEmpty().Average().ToString();
            }
            tableData.Well1Level_Average = tableData.Well1Level.DefaultIfEmpty().Average().ToString();
            return tableData;
        }
        ////////////////////////////Spell For Pump 5///////////////////////////////////////////


        ////////////////////////////Spell For Pump 6///////////////////////////////////////////

        public DisposalDataClass getAllSpellsForPump6(DataTable dt)
        {
            var tableData = new DisposalDataClass();
            var spelldata = new DisposalPump6SpellData();
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus6."])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<DisposalPump6SpellData> spellDataList = new List<DisposalPump6SpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus6."])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["Well2Level(ft)"])), 2);
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 1)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump6SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump6SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            string c = JsonConvert.SerializeObject(spellDataList);
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                //tableData.Well1Level = new List<double>();
                tableData.Well2Level = new List<double>();
                tableData.Pump6TimeArray = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus6.Add(Convert.ToInt32(dr["PumpStatus6."]));
                    tableData.Well2Level.Add(Convert.ToDouble(dr["Well2Level(ft)"]));
                    tableData.Pump6TimeArray.Add((dr["tim"]).ToString());
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump6 = pstr;
                tableData.WorkingHoursPump6 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump6 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                if (spellDataList.Count == 0)
                {
                    tableData.Well2Level_Average = "0";
                }
                else
                {
                    double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                    if (avgWaterFlow == 0)
                    {
                        avgWaterFlow = 1;
                    }
                    tableData.Well2Level_Average = tableData.Well2Level.DefaultIfEmpty().Average().ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                }
            }
            else
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                //tableData.Well1Level = new List<double>();
                tableData.Well2Level = new List<double>();
                tableData.Pump6TimeArray = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus6.Add(Convert.ToInt32(dr["PumpStatus6."]));
                    tableData.Well2Level.Add(Convert.ToDouble(dr["Well2Level(ft)"]));
                    tableData.Pump6TimeArray.Add((dr["tim"]).ToString());
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump6 = pstr;
                tableData.WorkingHoursPump6 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump6 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                tableData.Well2Level_Average = tableData.Well2Level.DefaultIfEmpty().Average().ToString();
            }
            tableData.Well2Level_Average = tableData.Well2Level.DefaultIfEmpty().Average().ToString();
            //tableData.Pump6TimeArray = spellDataList.FirstOrDefault().SpellTimeArray;
            return tableData;
        }
        ////////////////////////////Spell For Pump 6///////////////////////////////////////////



        ////////////////////////////Spell For Pump 7///////////////////////////////////////////

        public DisposalDataClass getAllSpellsForPump7(DataTable dt)
        {
            var tableData = new DisposalDataClass();
            var spelldata = new DisposalPump7SpellData();
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus7."])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<DisposalPump7SpellData> spellDataList = new List<DisposalPump7SpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus7."])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["Well2Level(ft)"])), 2);
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 1)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump7SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump7SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            string c = JsonConvert.SerializeObject(spellDataList);
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                //tableData.Well1Level = new List<double>();
                tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus7.Add(Convert.ToInt32(dr["PumpStatus7."]));
                    tableData.Well2Level.Add(Convert.ToDouble(dr["Well2Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump7 = pstr;
                tableData.WorkingHoursPump7 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump7 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                if (spellDataList.Count == 0)
                {
                    tableData.Well2Level_Average = "0";
                }
                else
                {
                    double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                    if (avgWaterFlow == 0)
                    {
                        avgWaterFlow = 1;
                    }
                    tableData.Well2Level_Average = tableData.Well2Level.DefaultIfEmpty().Average().ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                }
            }
            else
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                //tableData.Well1Level = new List<double>();
                tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus7.Add(Convert.ToInt32(dr["PumpStatus7."]));
                    tableData.Well2Level.Add(Convert.ToDouble(dr["Well2Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump7 = pstr;
                tableData.WorkingHoursPump7 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump7 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                tableData.Well2Level_Average = tableData.Well2Level.DefaultIfEmpty().Average().ToString();
            }
            tableData.Well2Level_Average = tableData.Well2Level.DefaultIfEmpty().Average().ToString();
            return tableData;
        }
        ////////////////////////////Spell For Pump 7///////////////////////////////////////////


        ////////////////////////////Spell For Pump 8///////////////////////////////////////////

        public DisposalDataClass getAllSpellsForPump8(DataTable dt)
        {
            var tableData = new DisposalDataClass();
            var spelldata = new DisposalPump8SpellData();
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus8."])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<DisposalPump8SpellData> spellDataList = new List<DisposalPump8SpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus8."])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["Well2Level(ft)"])), 2);
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 1)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump8SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump8SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            string c = JsonConvert.SerializeObject(spellDataList);
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                //tableData.Well1Level = new List<double>();
                tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus8.Add(Convert.ToInt32(dr["PumpStatus8."]));
                    tableData.Well2Level.Add(Convert.ToDouble(dr["Well2Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump8 = pstr;
                tableData.WorkingHoursPump8 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump8 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                if (spellDataList.Count == 0)
                {
                    tableData.Well2Level_Average = "0";
                }
                else
                {
                    double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                    if (avgWaterFlow == 0)
                    {
                        avgWaterFlow = 1;
                    }
                    tableData.Well2Level_Average = tableData.Well2Level.DefaultIfEmpty().Average().ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                }
            }
            else
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                //tableData.Well1Level = new List<double>();
                tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus8.Add(Convert.ToInt32(dr["PumpStatus8."]));
                    tableData.Well2Level.Add(Convert.ToDouble(dr["Well2Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump8 = pstr;
                tableData.WorkingHoursPump8 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump8 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                tableData.Well2Level_Average = tableData.Well2Level.DefaultIfEmpty().Average().ToString();
            }
            tableData.Well2Level_Average = tableData.Well2Level.DefaultIfEmpty().Average().ToString();
            return tableData;
        }
        ////////////////////////////Spell For Pump 8///////////////////////////////////////////


        ////////////////////////////Spell For Pump 9///////////////////////////////////////////

        public DisposalDataClass getAllSpellsForPump9(DataTable dt)
        {
            var tableData = new DisposalDataClass();
            var spelldata = new DisposalPump9SpellData();
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus9."])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<DisposalPump9SpellData> spellDataList = new List<DisposalPump9SpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus9."])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["Well2Level(ft)"])), 2);
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 1)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump9SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump9SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            string c = JsonConvert.SerializeObject(spellDataList);
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                //tableData.Well1Level = new List<double>();
                tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus9.Add(Convert.ToInt32(dr["PumpStatus9."]));
                    tableData.Well2Level.Add(Convert.ToDouble(dr["Well2Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump9 = pstr;
                tableData.WorkingHoursPump9 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump9 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                if (spellDataList.Count == 0)
                {
                    tableData.Well2Level_Average = "0";
                }
                else
                {
                    double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                    if (avgWaterFlow == 0)
                    {
                        avgWaterFlow = 1;
                    }
                    tableData.Well2Level_Average = tableData.Well2Level.DefaultIfEmpty().Average().ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                }
            }
            else
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                //tableData.Well1Level = new List<double>();
                tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus9.Add(Convert.ToInt32(dr["PumpStatus9."]));
                    tableData.Well2Level.Add(Convert.ToDouble(dr["Well2Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump9 = pstr;
                tableData.WorkingHoursPump9 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump9 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                tableData.Well2Level_Average = tableData.Well2Level.DefaultIfEmpty().Average().ToString();
            }
            tableData.Well2Level_Average = tableData.Well2Level.DefaultIfEmpty().Average().ToString();
            return tableData;
        }
        ////////////////////////////Spell For Pump 9///////////////////////////////////////////


        ////////////////////////////Spell For Pump 10///////////////////////////////////////////

        public DisposalDataClass getAllSpellsForPump10(DataTable dt)
        {
            var tableData = new DisposalDataClass();
            var spelldata = new DisposalPump10SpellData();
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatuss10."])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<DisposalPump10SpellData> spellDataList = new List<DisposalPump10SpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["PumpStatuss10."])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["Well2Level(ft)"])), 2);
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 1)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump10SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new DisposalPump10SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            string c = JsonConvert.SerializeObject(spellDataList);
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                tableData.PumpStatus10 = new List<double>();
                //tableData.Well1Level = new List<double>();
                tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus10.Add(Convert.ToInt32(dr["PumpStatuss10."]));
                    tableData.Well2Level.Add(Convert.ToDouble(dr["Well2Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump10 = pstr;
                tableData.WorkingHoursPump10 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump10 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                if (spellDataList.Count == 0)
                {
                    tableData.Well2Level_Average = "0";
                }
                else
                {
                    double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                    if (avgWaterFlow == 0)
                    {
                        avgWaterFlow = 1;
                    }
                    tableData.Well2Level_Average = tableData.Well2Level.DefaultIfEmpty().Average().ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                }
            }
            else
            {
                //tableData.PumpStatus1 = new List<double>();
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                tableData.PumpStatus10 = new List<double>();
                //tableData.Well1Level = new List<double>();
                tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName1 = location;
                    tableData.PumpStatus10.Add(Convert.ToInt32(dr["PumpStatuss10."]));
                    tableData.Well2Level.Add(Convert.ToDouble(dr["Well2Level(ft)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump10 = pstr;
                tableData.WorkingHoursPump10 = Math.Floor(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 1);
                //tableData.WorkingInHoursPump10 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                tableData.Well2Level_Average = tableData.Well2Level.DefaultIfEmpty().Average().ToString();
            }
            tableData.Well2Level_Average = tableData.Well2Level.DefaultIfEmpty().Average().ToString();
            return tableData;
        }
        ////////////////////////////Spell For Pump 10///////////////////////////////////////////
        public ActionResult WetWellReport()
        {
            return View();
        }

        public string getHMIStatus(string resourceName)
        {
            var hmi = new DisposalHMIstatus();
            //double ret = -1;s
            string resource = resourceName;
            //string resourceType = ViewBag.resourceType.toString();
            //string getResID = "select ResourceID from tblResource where ResourceLocation = 'C-II Johar Town' ";
            //if (ViewBag.SelectedResource == null)
            //{
            //    resource = "C-II Johar Town";
            //}
            string queryp1 = "select top(1) ParameterValue from tblEnergy where resourceID = 1059 and ParameterID = 144 order by ID desc";
            string queryp2 = "select top(1) ParameterValue from tblEnergy where resourceID = 1059 and ParameterID = 145 order by ID desc";
            string queryp3 = "select top(1) ParameterValue from tblEnergy where resourceID = 1059 and ParameterID = 146 order by ID desc";
            string queryp4 = "select top(1) ParameterValue from tblEnergy where resourceID = 1059 and ParameterID = 147 order by ID desc";
            string queryp5 = "select top(1) ParameterValue from tblEnergy where resourceID = 1059 and ParameterID = 148 order by ID desc";
            string queryp6 = "select top(1) ParameterValue from tblEnergy where resourceID = 1060 and ParameterID = 149 order by ID desc";
            string queryp7 = "select top(1) ParameterValue from tblEnergy where resourceID = 1060 and ParameterID = 150 order by ID desc";
            string queryp8 = "select top(1) ParameterValue from tblEnergy where resourceID = 1060 and ParameterID = 151 order by ID desc";
            string queryp9 = "select top(1) ParameterValue from tblEnergy where resourceID = 1060 and ParameterID = 152 order by ID desc";
            string queryp10 = "select top(1) ParameterValue from tblEnergy where resourceID = 1060 and ParameterID = 153 order by ID desc";
            string queryw1 = "select top(1) ParameterValue from tblEnergy where resourceID = 1059 and ParameterID = 175 order by ID desc";
            string queryw2 = "select top(1) ParameterValue from tblEnergy where resourceID = 1060 and ParameterID = 176 order by ID desc";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                var cmdp1 = new SqlCommand(queryp1, conn);
                var cmdp2 = new SqlCommand(queryp2, conn);
                var cmdp3 = new SqlCommand(queryp3, conn);
                var cmdp4 = new SqlCommand(queryp4, conn);
                var cmdp5 = new SqlCommand(queryp5, conn);
                var cmdp6 = new SqlCommand(queryp6, conn);
                var cmdp7 = new SqlCommand(queryp7, conn);
                var cmdp8 = new SqlCommand(queryp8, conn);
                var cmdp9 = new SqlCommand(queryp9, conn);
                var cmdp10 = new SqlCommand(queryp10, conn);
                var cmdw1 = new SqlCommand(queryw1, conn);
                var cmdw2 = new SqlCommand(queryw2, conn);
                try
                {
                    hmi.pumpStatus1 = Math.Abs(Convert.ToDouble(cmdp1.ExecuteScalar()));
                    hmi.pumpStatus2 = Math.Abs(Convert.ToDouble(cmdp2.ExecuteScalar()));
                    hmi.pumpStatus3 = Math.Abs(Convert.ToDouble(cmdp3.ExecuteScalar()));
                    hmi.pumpStatus4 = Math.Abs(Convert.ToDouble(cmdp4.ExecuteScalar()));
                    hmi.pumpStatus5 = Math.Abs(Convert.ToDouble(cmdp5.ExecuteScalar()));
                    hmi.pumpStatus6 = Math.Abs(Convert.ToDouble(cmdp6.ExecuteScalar()));
                    hmi.pumpStatus7 = Math.Abs(Convert.ToDouble(cmdp7.ExecuteScalar()));
                    hmi.pumpStatus8 = Math.Abs(Convert.ToDouble(cmdp8.ExecuteScalar()));
                    hmi.pumpStatus9 = Math.Abs(Convert.ToDouble(cmdp9.ExecuteScalar()));
                    hmi.pumpStatus10 = Math.Abs(Convert.ToDouble(cmdp10.ExecuteScalar()));
                    hmi.wellLevel1 = Math.Abs(Convert.ToDouble(cmdw1.ExecuteScalar()));
                    hmi.wellLevel2 = Math.Abs(Convert.ToDouble(cmdw2.ExecuteScalar()));
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            ViewBag.PumpStatus1 = Convert.ToInt32(hmi.pumpStatus1);
            ViewBag.PumpStatus2 = Convert.ToInt32(hmi.pumpStatus2);
            ViewBag.PumpStatus3 = Convert.ToInt32(hmi.pumpStatus3);
            ViewBag.PumpStatus4 = Convert.ToInt32(hmi.pumpStatus4);
            ViewBag.PumpStatus5 = Convert.ToInt32(hmi.pumpStatus5);
            ViewBag.PumpStatus6 = Convert.ToInt32(hmi.pumpStatus6);
            ViewBag.PumpStatus7 = Convert.ToInt32(hmi.pumpStatus7);
            ViewBag.PumpStatus8 = Convert.ToInt32(hmi.pumpStatus8);
            ViewBag.PumpStatus9 = Convert.ToInt32(hmi.pumpStatus9);
            ViewBag.PumpStatus10 = Convert.ToInt32(hmi.pumpStatus10);
            ViewBag.WellLevel1 = Convert.ToInt32(hmi.wellLevel1);
            ViewBag.WellLevel2 = Convert.ToInt32(hmi.wellLevel2);
            return JsonConvert.SerializeObject(hmi); ;
        }
        public ActionResult HMI()
        {
            return View();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        //[OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 2000)]
        public PartialViewResult _HMIView()
        {
            var hmi = new DisposalHMIstatus();
            //double ret = -1;
            //string resourceType = ViewBag.resourceType.toString();
            //string getResID = "select ResourceID from tblResource where ResourceLocation = 'C-II Johar Town' ";
            //if (ViewBag.SelectedResource == null)
            //{
            //    resource = "C-II Johar Town";
            //}
            string queryp1 = "select top(1) ParameterValue from tblEnergy where resourceID = 1059 and ParameterID = 144 order by ID desc";
            string queryp2 = "select top(1) ParameterValue from tblEnergy where resourceID = 1059 and ParameterID = 145 order by ID desc";
            string queryp3 = "select top(1) ParameterValue from tblEnergy where resourceID = 1059 and ParameterID = 146 order by ID desc";
            string queryp4 = "select top(1) ParameterValue from tblEnergy where resourceID = 1059 and ParameterID = 147 order by ID desc";
            string queryp5 = "select top(1) ParameterValue from tblEnergy where resourceID = 1059 and ParameterID = 148 order by ID desc";
            string queryp6 = "select top(1) ParameterValue from tblEnergy where resourceID = 1060 and ParameterID = 149 order by ID desc";
            string queryp7 = "select top(1) ParameterValue from tblEnergy where resourceID = 1060 and ParameterID = 150 order by ID desc";
            string queryp8 = "select top(1) ParameterValue from tblEnergy where resourceID = 1060 and ParameterID = 151 order by ID desc";
            string queryp9 = "select top(1) ParameterValue from tblEnergy where resourceID = 1060 and ParameterID = 152 order by ID desc";
            string queryp10 = "select top(1) ParameterValue from tblEnergy where resourceID = 1060 and ParameterID = 153 order by ID desc";
            string queryw1 = "select top(1) ParameterValue from tblEnergy where resourceID = 1059 and ParameterID = 175 order by ID desc";
            string queryw2 = "select top(1) ParameterValue from tblEnergy where resourceID = 1060 and ParameterID = 176 order by ID desc";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                var cmdp1 = new SqlCommand(queryp1, conn);
                var cmdp2 = new SqlCommand(queryp2, conn);
                var cmdp3 = new SqlCommand(queryp3, conn);
                var cmdp4 = new SqlCommand(queryp4, conn);
                var cmdp5 = new SqlCommand(queryp5, conn);
                var cmdp6 = new SqlCommand(queryp6, conn);
                var cmdp7 = new SqlCommand(queryp7, conn);
                var cmdp8 = new SqlCommand(queryp8, conn);
                var cmdp9 = new SqlCommand(queryp9, conn);
                var cmdp10 = new SqlCommand(queryp10, conn);
                var cmdw1 = new SqlCommand(queryw1, conn);
                var cmdw2 = new SqlCommand(queryw2, conn);
                try
                {
                    hmi.pumpStatus1 = Math.Abs(Convert.ToDouble(cmdp1.ExecuteScalar()));
                    hmi.pumpStatus2 = Math.Abs(Convert.ToDouble(cmdp2.ExecuteScalar()));
                    hmi.pumpStatus3 = Math.Abs(Convert.ToDouble(cmdp3.ExecuteScalar()));
                    hmi.pumpStatus4 = Math.Abs(Convert.ToDouble(cmdp4.ExecuteScalar()));
                    hmi.pumpStatus5 = Math.Abs(Convert.ToDouble(cmdp5.ExecuteScalar()));
                    hmi.pumpStatus6 = Math.Abs(Convert.ToDouble(cmdp6.ExecuteScalar()));
                    hmi.pumpStatus7 = Math.Abs(Convert.ToDouble(cmdp7.ExecuteScalar()));
                    hmi.pumpStatus8 = Math.Abs(Convert.ToDouble(cmdp8.ExecuteScalar()));
                    hmi.pumpStatus9 = Math.Abs(Convert.ToDouble(cmdp9.ExecuteScalar()));
                    hmi.pumpStatus10 = Math.Abs(Convert.ToDouble(cmdp10.ExecuteScalar()));
                    hmi.wellLevel1 = Math.Abs(Convert.ToDouble(cmdw1.ExecuteScalar()));
                    hmi.wellLevel2 = Math.Abs(Convert.ToDouble(cmdw2.ExecuteScalar()));
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            ViewBag.PumpStatus1 = hmi.pumpStatus1;
            ViewBag.PumpStatus2 = hmi.pumpStatus2;
            ViewBag.PumpStatus3 = hmi.pumpStatus3;
            ViewBag.PumpStatus4 = hmi.pumpStatus4;
            ViewBag.PumpStatus5 = hmi.pumpStatus5;
            ViewBag.PumpStatus6 = hmi.pumpStatus6;
            ViewBag.PumpStatus7 = hmi.pumpStatus7;
            ViewBag.PumpStatus8 = hmi.pumpStatus8;
            ViewBag.PumpStatus9 = hmi.pumpStatus9;
            ViewBag.PumpStatus10 = hmi.pumpStatus10;
            ViewBag.WellLevel1 = hmi.wellLevel1;
            ViewBag.WellLevel2 = hmi.wellLevel2;
            return PartialView();
        }
    }
}