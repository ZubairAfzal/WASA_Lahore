using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WASA_EMS;
using WASA_EMS.Models.Models_RMS;

namespace wasaRms.Controllers
{
    public class StorageTankController : Controller
    {
        // GET: StorageTank
        public ActionResult Dashboard()
        {
            int pumpsRunning = 0;
            double tankLevel = 0;
            string tempName = "";
            string parameterValuesString = "";
            string datetimed = "";
            string markers = "[";
            //string queryP1Status = "select top (1) parameterValue from tblSheet where parameterID = 1026 order by sheetInsertionDateTime DESC";
            //string queryP2Status = "select top (1) parameterValue from tblSheet where parameterID = 1027 order by sheetInsertionDateTime DESC";
            //string queryTankLevel = "select top (1) parameterValue from tblSheet where parameterID = 1034 order by sheetInsertionDateTime DESC";
            string S13query = "select count(*) from tblResource where resourceTypeID = 1002";
            string S14query = ";WITH cte AS ( SELECT rt.resourceTypeName as Type_, r.resourceName as category, 'Lahore' as townName, r.resourceName as townName2, s.parameterValue AS ponding, p.parameterName as pName, p.parameterUnit as unit, SUBSTRING(r.resourceGeoLocatin, 1, CASE CHARINDEX(',', r.resourceGeoLocatin) WHEN 0 THEN LEN(r.resourceGeoLocatin) ELSE CHARINDEX(',', r.resourceGeoLocatin) - 1 END) AS lat ,SUBSTRING(r.resourceGeoLocatin, CASE CHARINDEX(',', r.resourceGeoLocatin) WHEN 0 THEN LEN(r.resourceGeoLocatin) + 1 ELSE CHARINDEX(',', r.resourceGeoLocatin) + 1 END, 1000) AS lng, ROW_NUMBER() OVER (PARTITION BY s.resourceID ORDER BY s.sheetInsertionDateTime DESC) AS rn, s.sheetInsertionDateTime, DATEDIFF(minute, s.sheetInsertionDateTime, GETDATE ()) as DeltaMinutes FROM tblSheet s inner join tblResource r on s.resourceID = r.resourceID inner join tblResourceType rt on r.resourceTypeID = rt.resourceTypeID inner join tblResourceTypeParameter rtp on rt.resourceTypeID = rtp.resourceTypeID inner join tblParameter p on rtp.parameterID = p.parameterID where rt.resourceTypeName = 'Storm Water Tank') SELECT COUNT(*) FROM cte WHERE DeltaMinutes <= -520 AND rn = 1";
            string S15query = ";WITH cte AS ( SELECT rt.resourceTypeName as Type_, r.resourceName as category, 'Lahore' as townName, r.resourceName as townName2, s.parameterValue AS ponding, p.parameterName as pName, p.parameterUnit as unit, SUBSTRING(r.resourceGeoLocatin, 1, CASE CHARINDEX(',', r.resourceGeoLocatin) WHEN 0 THEN LEN(r.resourceGeoLocatin) ELSE CHARINDEX(',', r.resourceGeoLocatin) - 1 END) AS lat ,SUBSTRING(r.resourceGeoLocatin, CASE CHARINDEX(',', r.resourceGeoLocatin) WHEN 0 THEN LEN(r.resourceGeoLocatin) + 1 ELSE CHARINDEX(',', r.resourceGeoLocatin) + 1 END, 1000) AS lng, ROW_NUMBER() OVER (PARTITION BY s.resourceID ORDER BY s.sheetInsertionDateTime DESC) AS rn, s.sheetInsertionDateTime, DATEDIFF(minute, s.sheetInsertionDateTime, GETDATE ()) as DeltaMinutes FROM tblSheet s inner join tblResource r on s.resourceID = r.resourceID inner join tblResourceType rt on r.resourceTypeID = rt.resourceTypeID inner join tblResourceTypeParameter rtp on rt.resourceTypeID = rtp.resourceTypeID inner join tblParameter p on rtp.parameterID = p.parameterID where rt.resourceTypeName = 'Storm Water Tank') SELECT COUNT(*) FROM cte WHERE DeltaMinutes > -520 AND rn = 1";

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd1 = new SqlCommand(S13query, conn);
                    SqlCommand cmd2 = new SqlCommand(S14query, conn);
                    SqlCommand cmd3 = new SqlCommand(S15query, conn);
                    ViewBag.TotalStorageTank = Convert.ToInt32(cmd1.ExecuteScalar()).ToString();
                    ViewBag.ActiveStorageTank = Convert.ToInt32(cmd2.ExecuteScalar()).ToString();
                    ViewBag.InactiveStorageTank = Convert.ToInt32(cmd3.ExecuteScalar()).ToString();
                    string q1 = "";
                    q1 += "select top(28)  r.resourceLocationName,  t.resourceTypeName, p.ParameterName, e.parameterValue, e.sheetInsertionDateTime  from tblSheet e ";
                    q1 += "left join tblParameter p on e.parameterID = p.parameterID ";
                    q1 += "left join tblResource r on e.resourceID = r.resourceID ";
                    q1 += "left join tblResourceType t on r.resourceTypeID = t.resourceTypeID ";
                    q1 += "where e.sheetInsertionDateTime = (select max(sheetInsertionDateTime) from tblSheet where ResourceID = 1085) ";
                    q1 += "and r.resourceID = 1085 ";
                    SqlCommand cmd = new SqlCommand(q1, conn);
                    using (SqlDataReader sdr1 = cmd.ExecuteReader())
                    {
                        while (sdr1.Read())
                        {
                            string valuee = "";
                            parameterValuesString += "";
                            parameterValuesString += sdr1["ParameterName"].ToString() + ": ";
                            if (sdr1["parameterName"].ToString() == "P1 Status")
                            {
                                if (sdr1["parameterValue"].ToString() == "0")
                                {
                                    valuee = "OFF";
                                }
                                else
                                {
                                    valuee = "ON";
                                }
                            }
                            else if (sdr1["parameterName"].ToString() == "P2 Status")
                            {
                                if (sdr1["parameterValue"].ToString() == "0")
                                {
                                    valuee = "OFF";
                                }
                                else
                                {
                                    valuee = "ON";
                                }
                            }
                            else if (sdr1["parameterName"].ToString() == "Current Trip 1")
                            {
                                if (sdr1["parameterValue"].ToString() == "0")
                                {
                                    valuee = "No Error";
                                }
                                else
                                {
                                    valuee = "Error";
                                }
                            }
                            else if (sdr1["parameterName"].ToString() == "Current Trip 2")
                            {
                                if (sdr1["parameterValue"].ToString() == "0")
                                {
                                    valuee = "No Error";
                                }
                                else
                                {
                                    valuee = "Error";
                                }
                            }
                            else if (sdr1["parameterName"].ToString() == "Voltage Trip 1")
                            {
                                if (sdr1["parameterValue"].ToString() == "0")
                                {
                                    valuee = "No Error";
                                }
                                else
                                {
                                    valuee = "Error";
                                }
                            }
                            else if (sdr1["parameterName"].ToString() == "Voltage Trip 2")
                            {
                                if (sdr1["parameterValue"].ToString() == "0")
                                {
                                    valuee = "No Error";
                                }
                                else
                                {
                                    valuee = "Error";
                                }
                            }
                            else
                            {
                                valuee = Math.Round(Convert.ToDouble(sdr1["parameterValue"]), 2).ToString();
                            }
                            parameterValuesString += valuee;
                            parameterValuesString += "<br />";
                            datetimed = sdr1["sheetInsertionDateTime"].ToString();
                        }
                    }
                    tempName = "S";
                    string newstring = "<b>Storm Water Storage Tank</b>";
                    newstring += "<br />";
                    newstring += "<b>Lawrence Road</b>";
                    newstring += "<br />";
                    newstring += datetimed;
                    newstring += "<br />";
                    newstring += parameterValuesString;
                    TimeSpan duration = (Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString()) - Convert.ToDateTime(datetimed.ToString()));
                    double minu = duration.TotalMinutes;
                    parameterValuesString = "";
                    markers += "{";
                    markers += string.Format("'Template': '{0}',", tempName);
                    markers += string.Format("'title': '{0}',", "Lawrence Road");
                    markers += string.Format("'lat':'{0}',", "31.55407167");
                    markers += string.Format("'lnt':'{0}',", "74.32604167");
                    markers += string.Format("'description': '{0}'", newstring);
                    markers += "},";
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }
            markers = markers.Remove(markers.Length - 1, 1);
            markers += "]";
            var data = new { status = "Success" };
            ViewBag.PumpsRunning = pumpsRunning.ToString();
            ViewBag.TankLevel = tankLevel.ToString();
            ViewBag.MapMarkers = markers;
            return View();
        }


        public JsonResult LoadStorageChartData()
        {
            DataTable dt = new DataTable();
            string JSONresult = "";
            string query = "";
            query += ";WITH cte AS ( SELECT rt.resourceTypeName as Type_, r.maxThr as maxThrs, r.resourceName as category, r.resourceNumber as resnum, 'Lahore' as townName, r.resourceName as townName2, p.parameterName as pName, abs(s.parameterValue) AS ponding, p.parameterUnit as unit, SUBSTRING(r.resourceGeoLocatin, 1, CASE CHARINDEX(',', r.resourceGeoLocatin) WHEN 0 THEN LEN(r.resourceGeoLocatin) ELSE CHARINDEX(',', r.resourceGeoLocatin) - 1 END) AS lat ,SUBSTRING(r.resourceGeoLocatin, CASE CHARINDEX(',', r.resourceGeoLocatin) WHEN 0 THEN LEN(r.resourceGeoLocatin) + 1 ELSE CHARINDEX(',', r.resourceGeoLocatin) + 1 END, 1000) AS lng, DATEDIFF(minute, s.sheetInsertionDateTime, CONVERT(CHAR(24), CONVERT(DATETIME, GETDATE(), 103), 121)) as DeltaMinutes, ROW_NUMBER() OVER (PARTITION BY s.resourceID ORDER BY s.sheetInsertionDateTime DESC) AS rn, s.sheetInsertionDateTime FROM tblSheet s inner join tblResource r on s.resourceID = r.resourceID inner join tblResourceType rt on r.resourceTypeID = rt.resourceTypeID inner join tblResourceTypeParameter rtp on rt.resourceTypeID = rtp.resourceTypeID inner join tblParameter p on rtp.parameterID = p.parameterID where rt.resourceTypeName = 'Storm Water Tank' and p.parameterName = 'Tank Level2 (ft)') SELECT * FROM cte WHERE rn = 28 order by cast(resnum as int) asc";
            //query += "SELECT top(28) e.sheetID, r.resourceName as Location, ";
            //query += "SUBSTRING(r.resourceGeoLocatin, 1, CASE CHARINDEX(',', r.resourceGeoLocatin) WHEN 0 THEN LEN(r.resourceGeoLocatin) ELSE CHARINDEX(',', r.resourceGeoLocatin) - 1 END) AS lat, SUBSTRING(r.resourceGeoLocatin, CASE CHARINDEX(',', r.resourceGeoLocatin) WHEN 0 THEN LEN(r.resourceGeoLocatin) + 1 ELSE CHARINDEX(',', r.resourceGeoLocatin) + 1 END, 1000) AS lng, ";
            //query += "p.parameterName as pName, e.parameterValue as pVal, e.sheetInsertionDateTime as tim , ";
            //query += "DATEDIFF(minute, e.sheetInsertionDateTime, CONVERT(CHAR(24), CONVERT(DATETIME, GETDATE(), 103), 121)) as DeltaMinutes ";
            //query += "FROM tblSheet e ";
            //query += "inner join tblParameter p on e.parameterID = p.parameterID ";
            //query += "inner join tblResource r on e.resourceID = r.resourceID ";
            //query += "WHERE ";
            //query += "e.resourceID = 1085 AND ";
            //query += "sheetInsertionDateTime = (Select max(sheetInsertionDateTime) from tblSheet where resourceID = 1085) ";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                    sda.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt);
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            var data = new { status = "Success" };
            return Json(JSONresult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PumpStatusReport()
        {
            DateTime fromDt = DateTime.Now.AddHours(9).Date;
            DateTime toDt = DateTime.Now.AddHours(33).Date;
            ttlogixc_rmsWasa01Entities db = new ttlogixc_rmsWasa01Entities();
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("All");
            if (Convert.ToInt32(Session["CompanyID"]) == 1)
            {
                foreach (var item in db.tblResources.AsQueryable().Where(item => item.tblResourceType.resourceTypeName == "Storm Water Tank"))
                {
                    ResourceList.Add(item.resourceLocationName);
                }
                ViewBag.ResourceList = ResourceList;
            }
            else
            {
                int uID = Convert.ToInt32(Session["UserID"]);
                foreach (var item in db.tblResources.AsQueryable().Where(item => item.tblResourceType.resourceTypeName == "Storm Water Tank" && item.managedBy == uID))
                {
                    ResourceList.Add(item.resourceLocationName);
                }
                ViewBag.ResourceList = ResourceList;
            }
            /////////////////////////////////////////////////////////////////////////////////////////
            string scriptString = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    string getResFromTemp = "select  parameterID from tblParameter where parameterName = 'Tank Level2 (ft)'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes = new DataTable();
                    sdaRes.Fill(dtRes);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        if (Convert.ToInt32(Session["CompanyID"]) == 1)
                        {
                            getParamsFromRes = "select r.resourceID, r.resourceLocationName, r.resourceNumber as resnum from tblResource r inner join tblResourceTypeParameter rtp on r.resourceTypeID = rtp.resourceTypeID inner join tblParameter p on rtp.parameterID = p.parameterID where p.parameterName = 'Tank Level2 (ft)'  order by cast(r.resourceNumber as int) asc";
                        }
                        else
                        {
                            getParamsFromRes = "select r.resourceID, r.resourceLocationName, r.resourceNumber as resnum from tblResource r inner join tblResourceTypeParameter rtp on r.resourceTypeID = rtp.resourceTypeID inner join tblParameter p on rtp.parameterID = p.parameterID where p.parameterName = 'Tank Level2 (ft)'  and r.managedBy = " + Convert.ToInt32(Session["UserID"]) + " order by cast(r.resourceNumber as int) asc";
                        }
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"Tank Level\" },";
                        //Report of All Ponding Locations between " + fd + " to " + td + "
                        scriptString += "subtitles: [{text: \" Storage Tank Level between " + fromDt + " to " + toDt + " \" }],";
                        scriptString += "axisY: {suffix: \" inch\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries},";
                        scriptString += " data: [";
                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            //string parName = drPar["parameterName"].ToString();
                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.parameterID, e.parameterValue, e.sheetInsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.parameterID ";
                            aquery += "ORDER BY e.sheetInsertionDateTime DESC) ";
                            aquery += "FROM tblSheet e ";
                            aquery += "inner join tblResource r on e.resourceID = r.resourceID ";
                            aquery += "WHERE e.resourceID = " + Convert.ToInt32(drPar["resourceID"]) + " and e.parameterValue >= 0 and e.parameterID = " + Convert.ToInt32(drRes["parameterID"]) + " ";
                            //aquery += " and e.sheetInsertionDateTime >= DATEADD(day,-2,GETDATE()) ";
                            aquery += " and e.sheetInsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + fromDt + "', 103), 121) ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 parameterID, parameterValue, sheetInsertionDateTime FROM CTE WHERE RN < 14401 Order by sheetInsertionDateTime ASC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            scriptString += "{ type: \"line\", name: \"" + drPar["resourceLocationName"].ToString() + "\", showInLegend: true,  markerSize: 0, xValueType: \"dateTime\", xValueFormatString: \"DD-MM-YYYY hh:mm:ss TT\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} ft</strong> at {x}\", ";
                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
                                if (dtVal.Rows.IndexOf(drVal) != 0)
                                {
                                    if (Convert.ToDateTime(drVal["sheetInsertionDateTime"]).Subtract(dt).TotalSeconds < 6000)
                                    {
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                        dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                    }
                                    else
                                    {
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(dt).AddHours(-5).AddMinutes(1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5).AddMinutes(-1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                        dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                    }
                                }
                                else
                                {
                                    dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                    dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                }
                                //dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }
                            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints) + "";
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
            var dtabc = getStorageTankTableList(fromDt, toDt, "");
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;
            ////////////////////////////////////////////////////////////////////////////////////////////

            //var tablList = getStorageTankTableList(fromDt, toDt, "All");
            //return View(tablList);
            return View(dtabc);
        }
        //public List<StorageTankTableData> getStorageTankTableList(DateTime fromDT, DateTime toDt, string res)
        [HttpPost]
        public ActionResult PumpStatusReport(FormCollection review)
        {
            ttlogixc_rmsWasa01Entities db = new ttlogixc_rmsWasa01Entities();
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
                tt_time = "11:59 PM";
            }
            DateTime FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
            DateTime FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            DateTime fromDt = FinalTimeFrom;
            DateTime toDt = FinalTimeTo;
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("All");
            if (Convert.ToInt32(Session["CompanyID"]) == 1)
            {
                foreach (var item in db.tblResources.AsQueryable().Where(item => item.tblResourceType.resourceTypeName == "Storm Water Tank"))
                {
                    ResourceList.Add(item.resourceLocationName);
                }
                ViewBag.ResourceList = ResourceList;
            }
            else
            {
                int uID = Convert.ToInt32(Session["UserID"]);
                foreach (var item in db.tblResources.AsQueryable().Where(item => item.tblResourceType.resourceTypeName == "Storm Water Tank" && item.managedBy == uID))
                {
                    ResourceList.Add(item.resourceLocationName);
                }
                ViewBag.ResourceList = ResourceList;
            }
            /////////////////////////////////////////////////////////////////////////////////////////
            string scriptString = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    string getResFromTemp = "select  parameterID from tblParameter where parameterName = 'Tank Level2 (ft)'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes = new DataTable();
                    sdaRes.Fill(dtRes);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        if (Convert.ToInt32(Session["CompanyID"]) == 1)
                        {
                            getParamsFromRes = "select r.resourceID, r.resourceLocationName, r.resourceNumber as resnum from tblResource r inner join tblResourceTypeParameter rtp on r.resourceTypeID = rtp.resourceTypeID inner join tblParameter p on rtp.parameterID = p.parameterID where p.parameterName = 'Tank Level2 (ft)'  order by cast(r.resourceNumber as int) asc";
                        }
                        else
                        {
                            getParamsFromRes = "select r.resourceID, r.resourceLocationName, r.resourceNumber as resnum from tblResource r inner join tblResourceTypeParameter rtp on r.resourceTypeID = rtp.resourceTypeID inner join tblParameter p on rtp.parameterID = p.parameterID where p.parameterName = 'Tank Level2 (ft)'  and r.managedBy = " + Convert.ToInt32(Session["UserID"]) + " order by cast(r.resourceNumber as int) asc";
                        }
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"Tank Level\" },";
                        //Report of All Ponding Locations between " + fd + " to " + td + "
                        scriptString += "subtitles: [{text: \" Storage Tank Level between " + fromDt + " to " + toDt + " \" }],";
                        scriptString += "axisY: {suffix: \" inch\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries},";
                        scriptString += " data: [";
                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            //string parName = drPar["parameterName"].ToString();
                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.parameterID, e.parameterValue, e.sheetInsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.parameterID ";
                            aquery += "ORDER BY e.sheetInsertionDateTime DESC) ";
                            aquery += "FROM tblSheet e ";
                            aquery += "inner join tblResource r on e.resourceID = r.resourceID ";
                            aquery += "WHERE e.resourceID = " + Convert.ToInt32(drPar["resourceID"]) + " and e.parameterValue >= 0 and e.parameterID = " + Convert.ToInt32(drRes["parameterID"]) + " ";
                            //aquery += " and e.sheetInsertionDateTime >= DATEADD(day,-2,GETDATE()) ";
                            aquery += " and e.sheetInsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + fromDt + "', 103), 121) ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 parameterID, parameterValue, sheetInsertionDateTime FROM CTE WHERE RN < 14401 Order by sheetInsertionDateTime ASC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            scriptString += "{ type: \"line\", name: \"" + drPar["resourceLocationName"].ToString() + "\", showInLegend: true,  markerSize: 0, xValueType: \"dateTime\", xValueFormatString: \"DD-MM-YYYY hh:mm:ss TT\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} ft</strong> at {x}\", ";
                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
                                if (dtVal.Rows.IndexOf(drVal) != 0)
                                {
                                    if (Convert.ToDateTime(drVal["sheetInsertionDateTime"]).Subtract(dt).TotalSeconds < 6000)
                                    {
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                        dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                    }
                                    else
                                    {
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(dt).AddHours(-5).AddMinutes(1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5).AddMinutes(-1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                        dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                    }
                                }
                                else
                                {
                                    dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                    dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                }
                                //dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }
                            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints) + "";
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
            var dtabc = getStorageTankTableList(fromDt, toDt, "");
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;
            ////////////////////////////////////////////////////////////////////////////////////////////

            //var tablList = getStorageTankTableList(fromDt, toDt, "All");
            //return View(tablList);
            ViewBag.SelectedResource = resource;
            ViewBag.SelectedTimeFrom = TF;
            ViewBag.SelectedTimeTo = TT;
            ViewBag.SelectedTimeFrom = TF.ToString();
            ViewBag.SelectedTimeTo = TT.ToString();
            ViewBag.timeFrom = TF;
            ViewBag.timeTo = TT;
            ViewBag.dateFrom = df_date;
            ViewBag.dateTo = dt_date;
            return View(dtabc);
        }

        public ActionResult EnergyMonitoringReport()
        {
            DateTime fromDt = DateTime.Now.AddHours(9).Date;
            DateTime toDt = DateTime.Now.AddHours(33).Date;
            ttlogixc_rmsWasa01Entities db = new ttlogixc_rmsWasa01Entities();
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("All");
            if (Convert.ToInt32(Session["CompanyID"]) == 1)
            {
                foreach (var item in db.tblResources.AsQueryable().Where(item => item.tblResourceType.resourceTypeName == "Storm Water Tank"))
                {
                    ResourceList.Add(item.resourceLocationName);
                }
                ViewBag.ResourceList = ResourceList;
            }
            else
            {
                int uID = Convert.ToInt32(Session["UserID"]);
                foreach (var item in db.tblResources.AsQueryable().Where(item => item.tblResourceType.resourceTypeName == "Storm Water Tank" && item.managedBy == uID))
                {
                    ResourceList.Add(item.resourceLocationName);
                }
                ViewBag.ResourceList = ResourceList;
            }
            /////////////////////////////////////////////////////////////////////////////////////////
            string scriptString = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    string getResFromTemp = "select  parameterID from tblParameter where parameterName = 'Tank Level2 (ft)'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes = new DataTable();
                    sdaRes.Fill(dtRes);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        if (Convert.ToInt32(Session["CompanyID"]) == 1)
                        {
                            getParamsFromRes = "select r.resourceID, r.resourceLocationName, r.resourceNumber as resnum from tblResource r inner join tblResourceTypeParameter rtp on r.resourceTypeID = rtp.resourceTypeID inner join tblParameter p on rtp.parameterID = p.parameterID where p.parameterName = 'Tank Level2 (ft)'  order by cast(r.resourceNumber as int) asc";
                        }
                        else
                        {
                            getParamsFromRes = "select r.resourceID, r.resourceLocationName, r.resourceNumber as resnum from tblResource r inner join tblResourceTypeParameter rtp on r.resourceTypeID = rtp.resourceTypeID inner join tblParameter p on rtp.parameterID = p.parameterID where p.parameterName = 'Tank Level2 (ft)'  and r.managedBy = " + Convert.ToInt32(Session["UserID"]) + " order by cast(r.resourceNumber as int) asc";
                        }
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"Tank Level\" },";
                        //Report of All Ponding Locations between " + fd + " to " + td + "
                        scriptString += "subtitles: [{text: \" Storage Tank Level between " + fromDt + " to " + toDt + " \" }],";
                        scriptString += "axisY: {suffix: \" inch\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries},";
                        scriptString += " data: [";
                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            //string parName = drPar["parameterName"].ToString();
                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.parameterID, e.parameterValue, e.sheetInsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.parameterID ";
                            aquery += "ORDER BY e.sheetInsertionDateTime DESC) ";
                            aquery += "FROM tblSheet e ";
                            aquery += "inner join tblResource r on e.resourceID = r.resourceID ";
                            aquery += "WHERE e.resourceID = " + Convert.ToInt32(drPar["resourceID"]) + " and e.parameterValue >= 0 and e.parameterID = " + Convert.ToInt32(drRes["parameterID"]) + " ";
                            //aquery += " and e.sheetInsertionDateTime >= DATEADD(day,-2,GETDATE()) ";
                            aquery += " and e.sheetInsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + fromDt + "', 103), 121) ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 parameterID, parameterValue, sheetInsertionDateTime FROM CTE WHERE RN < 14401 Order by sheetInsertionDateTime ASC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            scriptString += "{ type: \"line\", name: \"" + drPar["resourceLocationName"].ToString() + "\", showInLegend: true,  markerSize: 0, xValueType: \"dateTime\", xValueFormatString: \"DD-MM-YYYY hh:mm:ss TT\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} ft</strong> at {x}\", ";
                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
                                if (dtVal.Rows.IndexOf(drVal) != 0)
                                {
                                    if (Convert.ToDateTime(drVal["sheetInsertionDateTime"]).Subtract(dt).TotalSeconds < 6000)
                                    {
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                        dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                    }
                                    else
                                    {
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(dt).AddHours(-5).AddMinutes(1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5).AddMinutes(-1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                        dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                    }
                                }
                                else
                                {
                                    dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                    dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                }
                                //dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }
                            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints) + "";
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
            var dtabc = getStorageTankTableList(fromDt, toDt, "");
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;
            ////////////////////////////////////////////////////////////////////////////////////////////

            //var tablList = getStorageTankTableList(fromDt, toDt, "All");
            //return View(tablList);
            return View(dtabc);
        }
        //public List<StorageTankTableData> getStorageTankTableList(DateTime fromDT, DateTime toDt, string res)
        [HttpPost]
        public ActionResult EnergyMonitoringReport(FormCollection review)
        {
            ttlogixc_rmsWasa01Entities db = new ttlogixc_rmsWasa01Entities();
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
                tt_time = "11:59 PM";
            }
            DateTime FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
            DateTime FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            DateTime fromDt = FinalTimeFrom;
            DateTime toDt = FinalTimeTo;
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("All");
            if (Convert.ToInt32(Session["CompanyID"]) == 1)
            {
                foreach (var item in db.tblResources.AsQueryable().Where(item => item.tblResourceType.resourceTypeName == "Storm Water Tank"))
                {
                    ResourceList.Add(item.resourceLocationName);
                }
                ViewBag.ResourceList = ResourceList;
            }
            else
            {
                int uID = Convert.ToInt32(Session["UserID"]);
                foreach (var item in db.tblResources.AsQueryable().Where(item => item.tblResourceType.resourceTypeName == "Storm Water Tank" && item.managedBy == uID))
                {
                    ResourceList.Add(item.resourceLocationName);
                }
                ViewBag.ResourceList = ResourceList;
            }
            /////////////////////////////////////////////////////////////////////////////////////////
            string scriptString = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    string getResFromTemp = "select  parameterID from tblParameter where parameterName = 'Tank Level2 (ft)'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes = new DataTable();
                    sdaRes.Fill(dtRes);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        if (Convert.ToInt32(Session["CompanyID"]) == 1)
                        {
                            getParamsFromRes = "select r.resourceID, r.resourceLocationName, r.resourceNumber as resnum from tblResource r inner join tblResourceTypeParameter rtp on r.resourceTypeID = rtp.resourceTypeID inner join tblParameter p on rtp.parameterID = p.parameterID where p.parameterName = 'Tank Level2 (ft)'  order by cast(r.resourceNumber as int) asc";
                        }
                        else
                        {
                            getParamsFromRes = "select r.resourceID, r.resourceLocationName, r.resourceNumber as resnum from tblResource r inner join tblResourceTypeParameter rtp on r.resourceTypeID = rtp.resourceTypeID inner join tblParameter p on rtp.parameterID = p.parameterID where p.parameterName = 'Tank Level2 (ft)'  and r.managedBy = " + Convert.ToInt32(Session["UserID"]) + " order by cast(r.resourceNumber as int) asc";
                        }
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"Tank Level\" },";
                        //Report of All Ponding Locations between " + fd + " to " + td + "
                        scriptString += "subtitles: [{text: \" Storage Tank Level between " + fromDt + " to " + toDt + " \" }],";
                        scriptString += "axisY: {suffix: \" inch\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries},";
                        scriptString += " data: [";
                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            //string parName = drPar["parameterName"].ToString();
                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.parameterID, e.parameterValue, e.sheetInsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.parameterID ";
                            aquery += "ORDER BY e.sheetInsertionDateTime DESC) ";
                            aquery += "FROM tblSheet e ";
                            aquery += "inner join tblResource r on e.resourceID = r.resourceID ";
                            aquery += "WHERE e.resourceID = " + Convert.ToInt32(drPar["resourceID"]) + " and e.parameterValue >= 0 and e.parameterID = " + Convert.ToInt32(drRes["parameterID"]) + " ";
                            //aquery += " and e.sheetInsertionDateTime >= DATEADD(day,-2,GETDATE()) ";
                            aquery += " and e.sheetInsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + fromDt + "', 103), 121) ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 parameterID, parameterValue, sheetInsertionDateTime FROM CTE WHERE RN < 14401 Order by sheetInsertionDateTime ASC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            scriptString += "{ type: \"line\", name: \"" + drPar["resourceLocationName"].ToString() + "\", showInLegend: true,  markerSize: 0, xValueType: \"dateTime\", xValueFormatString: \"DD-MM-YYYY hh:mm:ss TT\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} ft</strong> at {x}\", ";
                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
                                if (dtVal.Rows.IndexOf(drVal) != 0)
                                {
                                    if (Convert.ToDateTime(drVal["sheetInsertionDateTime"]).Subtract(dt).TotalSeconds < 6000)
                                    {
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                        dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                    }
                                    else
                                    {
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(dt).AddHours(-5).AddMinutes(1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5).AddMinutes(-1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                        dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                    }
                                }
                                else
                                {
                                    dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                    dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                }
                                //dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }
                            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints) + "";
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
            var dtabc = getStorageTankTableList(fromDt, toDt, "");
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;
            ////////////////////////////////////////////////////////////////////////////////////////////

            //var tablList = getStorageTankTableList(fromDt, toDt, "All");
            //return View(tablList);
            ViewBag.SelectedResource = resource;
            ViewBag.SelectedTimeFrom = TF;
            ViewBag.SelectedTimeTo = TT;
            ViewBag.SelectedTimeFrom = TF.ToString();
            ViewBag.SelectedTimeTo = TT.ToString();
            ViewBag.timeFrom = TF;
            ViewBag.timeTo = TT;
            ViewBag.dateFrom = df_date;
            ViewBag.dateTo = dt_date;
            return View(dtabc);
        }

        public ActionResult TankStorageReport()
        {
            DateTime fromDt = DateTime.Now.AddHours(9).Date;
            DateTime toDt = DateTime.Now.AddHours(33).Date;
            ttlogixc_rmsWasa01Entities db = new ttlogixc_rmsWasa01Entities();
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("All");
            if (Convert.ToInt32(Session["CompanyID"]) == 1)
            {
                foreach (var item in db.tblResources.AsQueryable().Where(item => item.tblResourceType.resourceTypeName == "Storm Water Tank"))
                {
                    ResourceList.Add(item.resourceLocationName);
                }
                ViewBag.ResourceList = ResourceList;
            }
            else
            {
                int uID = Convert.ToInt32(Session["UserID"]);
                foreach (var item in db.tblResources.AsQueryable().Where(item => item.tblResourceType.resourceTypeName == "Storm Water Tank" && item.managedBy == uID))
                {
                    ResourceList.Add(item.resourceLocationName);
                }
                ViewBag.ResourceList = ResourceList;
            }
            /////////////////////////////////////////////////////////////////////////////////////////
            string scriptString = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    string getResFromTemp = "select  parameterID from tblParameter where parameterName = 'Tank Level2 (ft)'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes = new DataTable();
                    sdaRes.Fill(dtRes);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        if (Convert.ToInt32(Session["CompanyID"]) == 1)
                        {
                            getParamsFromRes = "select r.resourceID, r.resourceLocationName, r.resourceNumber as resnum from tblResource r inner join tblResourceTypeParameter rtp on r.resourceTypeID = rtp.resourceTypeID inner join tblParameter p on rtp.parameterID = p.parameterID where p.parameterName = 'Tank Level2 (ft)'  order by cast(r.resourceNumber as int) asc";
                        }
                        else
                        {
                            getParamsFromRes = "select r.resourceID, r.resourceLocationName, r.resourceNumber as resnum from tblResource r inner join tblResourceTypeParameter rtp on r.resourceTypeID = rtp.resourceTypeID inner join tblParameter p on rtp.parameterID = p.parameterID where p.parameterName = 'Tank Level2 (ft)'  and r.managedBy = " + Convert.ToInt32(Session["UserID"]) + " order by cast(r.resourceNumber as int) asc";
                        }
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"Tank Level\" },";
                        //Report of All Ponding Locations between " + fd + " to " + td + "
                        scriptString += "subtitles: [{text: \" Storage Tank Level between " + fromDt + " to " + toDt + " \" }],";
                        scriptString += "axisY: {suffix: \" inch\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries},";
                        scriptString += " data: [";
                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            //string parName = drPar["parameterName"].ToString();
                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.parameterID, e.parameterValue, e.sheetInsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.parameterID ";
                            aquery += "ORDER BY e.sheetInsertionDateTime DESC) ";
                            aquery += "FROM tblSheet e ";
                            aquery += "inner join tblResource r on e.resourceID = r.resourceID ";
                            aquery += "WHERE e.resourceID = " + Convert.ToInt32(drPar["resourceID"]) + " and e.parameterValue >= 0 and e.parameterID = " + Convert.ToInt32(drRes["parameterID"]) + " ";
                            //aquery += " and e.sheetInsertionDateTime >= DATEADD(day,-2,GETDATE()) ";
                            aquery += " and e.sheetInsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + fromDt + "', 103), 121) ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 parameterID, parameterValue, sheetInsertionDateTime FROM CTE WHERE RN < 14401 Order by sheetInsertionDateTime ASC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            scriptString += "{ type: \"line\", name: \"" + drPar["resourceLocationName"].ToString() + "\", showInLegend: true,  markerSize: 0, xValueType: \"dateTime\", xValueFormatString: \"DD-MM-YYYY hh:mm:ss TT\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} ft</strong> at {x}\", ";
                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
                                if (dtVal.Rows.IndexOf(drVal) != 0)
                                {
                                    if (Convert.ToDateTime(drVal["sheetInsertionDateTime"]).Subtract(dt).TotalSeconds < 6000)
                                    {
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                        dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                    }
                                    else
                                    {
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(dt).AddHours(-5).AddMinutes(1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5).AddMinutes(-1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                        dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                    }
                                }
                                else
                                {
                                    dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                    dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                }
                                //dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }
                            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints) + "";
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
            var dtabc = getStorageTankTableList(fromDt, toDt, "");
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;
            ////////////////////////////////////////////////////////////////////////////////////////////

            //var tablList = getStorageTankTableList(fromDt, toDt, "All");
            //return View(tablList);
            return View(dtabc);
        }
        //public List<StorageTankTableData> getStorageTankTableList(DateTime fromDT, DateTime toDt, string res)
        [HttpPost]
        public ActionResult TankStorageReport(FormCollection review)
        {
            ttlogixc_rmsWasa01Entities db = new ttlogixc_rmsWasa01Entities();
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
                tt_time = "11:59 PM";
            }
            DateTime FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
            DateTime FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            DateTime fromDt = FinalTimeFrom;
            DateTime toDt = FinalTimeTo;
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("All");
            if (Convert.ToInt32(Session["CompanyID"]) == 1)
            {
                foreach (var item in db.tblResources.AsQueryable().Where(item => item.tblResourceType.resourceTypeName == "Storm Water Tank"))
                {
                    ResourceList.Add(item.resourceLocationName);
                }
                ViewBag.ResourceList = ResourceList;
            }
            else
            {
                int uID = Convert.ToInt32(Session["UserID"]);
                foreach (var item in db.tblResources.AsQueryable().Where(item => item.tblResourceType.resourceTypeName == "Storm Water Tank" && item.managedBy == uID))
                {
                    ResourceList.Add(item.resourceLocationName);
                }
                ViewBag.ResourceList = ResourceList;
            }
            /////////////////////////////////////////////////////////////////////////////////////////
            string scriptString = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    string getResFromTemp = "select  parameterID from tblParameter where parameterName = 'Tank Level2 (ft)'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes = new DataTable();
                    sdaRes.Fill(dtRes);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        if (Convert.ToInt32(Session["CompanyID"]) == 1)
                        {
                            getParamsFromRes = "select r.resourceID, r.resourceLocationName, r.resourceNumber as resnum from tblResource r inner join tblResourceTypeParameter rtp on r.resourceTypeID = rtp.resourceTypeID inner join tblParameter p on rtp.parameterID = p.parameterID where p.parameterName = 'Tank Level2 (ft)'  order by cast(r.resourceNumber as int) asc";
                        }
                        else
                        {
                            getParamsFromRes = "select r.resourceID, r.resourceLocationName, r.resourceNumber as resnum from tblResource r inner join tblResourceTypeParameter rtp on r.resourceTypeID = rtp.resourceTypeID inner join tblParameter p on rtp.parameterID = p.parameterID where p.parameterName = 'Tank Level2 (ft)'  and r.managedBy = " + Convert.ToInt32(Session["UserID"]) + " order by cast(r.resourceNumber as int) asc";
                        }
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"Tank Level\" },";
                        //Report of All Ponding Locations between " + fd + " to " + td + "
                        scriptString += "subtitles: [{text: \" Storage Tank Level between " + fromDt + " to " + toDt + " \" }],";
                        scriptString += "axisY: {suffix: \" inch\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries},";
                        scriptString += " data: [";
                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            //string parName = drPar["parameterName"].ToString();
                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.parameterID, e.parameterValue, e.sheetInsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.parameterID ";
                            aquery += "ORDER BY e.sheetInsertionDateTime DESC) ";
                            aquery += "FROM tblSheet e ";
                            aquery += "inner join tblResource r on e.resourceID = r.resourceID ";
                            aquery += "WHERE e.resourceID = " + Convert.ToInt32(drPar["resourceID"]) + " and e.parameterValue >= 0 and e.parameterID = " + Convert.ToInt32(drRes["parameterID"]) + " ";
                            //aquery += " and e.sheetInsertionDateTime >= DATEADD(day,-2,GETDATE()) ";
                            aquery += " and e.sheetInsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + fromDt + "', 103), 121) ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 parameterID, parameterValue, sheetInsertionDateTime FROM CTE WHERE RN < 14401 Order by sheetInsertionDateTime ASC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            scriptString += "{ type: \"line\", name: \"" + drPar["resourceLocationName"].ToString() + "\", showInLegend: true,  markerSize: 0, xValueType: \"dateTime\", xValueFormatString: \"DD-MM-YYYY hh:mm:ss TT\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} ft</strong> at {x}\", ";
                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
                                if (dtVal.Rows.IndexOf(drVal) != 0)
                                {
                                    if (Convert.ToDateTime(drVal["sheetInsertionDateTime"]).Subtract(dt).TotalSeconds < 6000)
                                    {
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                        dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                    }
                                    else
                                    {
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(dt).AddHours(-5).AddMinutes(1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5).AddMinutes(-1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                        dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                    }
                                }
                                else
                                {
                                    dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["sheetInsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["parameterValue"]) / 1));
                                    dt = Convert.ToDateTime(drVal["sheetInsertionDateTime"]);
                                }
                                //dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }
                            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints) + "";
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
            var dtabc = getStorageTankTableList(fromDt, toDt, "");
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;
            ////////////////////////////////////////////////////////////////////////////////////////////

            //var tablList = getStorageTankTableList(fromDt, toDt, "All");
            //return View(tablList);
            ViewBag.SelectedResource = resource;
            ViewBag.SelectedTimeFrom = TF;
            ViewBag.SelectedTimeTo = TT;
            ViewBag.SelectedTimeFrom = TF.ToString();
            ViewBag.SelectedTimeTo = TT.ToString();
            ViewBag.timeFrom = TF;
            ViewBag.timeTo = TT;
            ViewBag.dateFrom = df_date;
            ViewBag.dateTo = dt_date;
            return View(dtabc);
        }

        public List<StorageTankTableData> getStorageTankTableList(DateTime fromDT, DateTime toDt, string res)
        {
            DataTable dt = new DataTable();
            string query = ";WITH cte AS ( SELECT * FROM ( SELECT DISTINCT r.resourceName AS Location, r.resourceID, p.parameterName AS pID, s.parameterValue AS pVal, s.sheetInsertionDateTime as tim , DATEDIFF(minute, s.sheetInsertionDateTime, DATEADD(hour, 10,GETDATE ())) as DeltaMinutes FROM tblSheet s inner join tblResource r on s.resourceID = r.resourceID inner join tblParameter p on s.parameterID = p.parameterID inner join tblResourceType rt on r.resourceTypeID = rt.resourceTypeID where r.resourceID = 1085 and sheetInsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + fromDT + "', 103), 121) and sheetInsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + toDt + "', 103), 121)  ) AS SourceTable PIVOT ( SUM(pVal) FOR pID IN ( [V1N (V)],[V2N (V)],[V3N (V)],[I1 (A)],[I2 (A)],[I3 (A)],[W (kwatt)],[VAR (kvar)],[VA (kva)],[VA-SUM (kva)],[PF],[Freq (Hz)],[V12 (v)],[V23 (v)],[V13 (v)],[V1 THD (%)],[V2 THD (%)],[V3 THD (%)],[P1 Status],[P2 Status],[Current Trip 1],[Current Trip 2],[Voltage Trip 1],[Voltage Trip 2],[P1 Auto/Mannual],[P2 Auto/Mannual],[Tank Level1 (ft)],[Tank Level2 (ft)] ) ) AS PivotTable ) SELECT * FROM cte order by cast(resourceID as INT) ASC, tim DESC ";
            using (SqlConnection con1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                con1.Open();
                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = con1;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            if (dt.Rows.Count == 1)
            {

            }
            var abc = getPump1WorkingHours(dt);
            var bca = getPump2WorkingHours(dt);
            //var storageList = new List<StorageTankTableData>();
            //ViewBag.Pump1WorkingHours = abc.workingHoursTodayP1;
            ViewBag.Pump2WorkingHours = bca.workingHoursTodayP2;
            abc.workingHoursTodayP2 = bca.workingHoursTodayP2;
            abc.pumpStatus2 = bca.pumpStatus2;
            var abclist = new List<StorageTankTableData>();
            abclist.Add(abc);
            return abclist;
        }
        public string getPump1WorkingHoursManual(DateTime fromDT, DateTime toDt, string res)
        {
            return "";
        }
        public string getPump1WorkingHoursAuto(DateTime fromDT, DateTime toDt, string res)
        {
            return "";
        }
        public string getPump1WorkingHoursRemote(DateTime fromDT, DateTime toDt, string res)
        {
            return "";
        }
        public StorageTankTableData getPump1WorkingHours(DataTable dt)
        {
            var tableData = new StorageTankTableData();
            var spelldata = new StoragePump1SpellData();
            if (dt.Rows.Count > 1)
            {
                string location = dt.Rows[0]["Location"].ToString();
                double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["P1 Status"])), 2);
                string currentTime = dt.Rows[0]["tim"].ToString();
                double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
                bool S = false;
                bool E = false;
                bool T = true;
                bool F = false;
                int spell = 0;
                List<StoragePump1SpellData> spellDataList = new List<StoragePump1SpellData>();
                string curtm = "";
                foreach (DataRow dr in dt.Rows)
                {
                    double currValue = Math.Round((Convert.ToDouble(dr["P1 Status"])), 2);
                    double FlowRate = Math.Round((Convert.ToDouble(dr["Tank Level2 (ft)"])), 2);
                    double currValueManual = Math.Round((Convert.ToDouble(dr["P1 Auto/Mannual"])), 2);
                    string currTime = dr["tim"].ToString();
                    string clearaceTime = "";
                    //start scenario 3 (inactive)
                    if (DeltaMinutes > 28800)
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
                                        if (currValueManual == 0)
                                        {
                                            spelldata.SpellMode = 1;
                                        }
                                        else
                                        {
                                            spelldata.SpellMode = 2;
                                        }
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
                                    if (currValueManual == 0)
                                    {
                                        spelldata.SpellMode = 1;
                                    }
                                    else
                                    {
                                        spelldata.SpellMode = 2;
                                    }
                                }
                            }
                            else if (E == T && S == F)
                            {
                                if (currValue < 1)
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
                                if (spelldata.SpellDataArray.Count > 0 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                                {
                                    spelldata.SpellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                    if (spelldata.SpellPeriod == 0)
                                    {
                                        spelldata.SpellPeriod = 1;
                                    }
                                    spellDataList.Add(spelldata);
                                    spelldata = new StoragePump1SpellData();
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
                                        if (currValueManual == 0)
                                        {
                                            spelldata.SpellMode = 1;
                                        }
                                        else
                                        {
                                            spelldata.SpellMode = 2;
                                        }
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
                                    if (currValueManual == 0)
                                    {
                                        spelldata.SpellMode = 1;
                                    }
                                    else
                                    {
                                        spelldata.SpellMode = 2;
                                    }
                                }
                            }
                            else if (E == T && S == F)
                            {
                                if (currValue < 1)
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
                                if (spelldata.SpellDataArray.Count > 0 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                                {
                                    //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                    //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                    //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                    //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                    //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                    spelldata.SpellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                    if (spelldata.SpellPeriod == 0)
                                    {
                                        spelldata.SpellPeriod = 1;
                                    }
                                    //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                    //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                    spellDataList.Add(spelldata);
                                    spelldata = new StoragePump1SpellData();
                                    string s = JsonConvert.SerializeObject(spellDataList);
                                }
                            }
                        }
                        // end  scenario 2 (uncleared/ ponding continues)
                    }
                    curtm = currTime;
                    if (dr == dt.Rows[dt.Rows.Count - 1] && currValue > 0)
                    {
                        spelldata.SpellStartTime = currTime;
                        if (spelldata.SpellDataArray.Count > 0 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                        {
                            //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                            //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                            //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                            //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                            //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                            spelldata.SpellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                            if (spelldata.SpellPeriod == 0)
                            {
                                spelldata.SpellPeriod = 1;
                            }
                            //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                            //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                            spellDataList.Add(spelldata);
                            spelldata = new StoragePump1SpellData();
                            string s = JsonConvert.SerializeObject(spellDataList);
                        }
                    }
                }
                if (spellDataList.Count < 1)
                {
                    if (spelldata.SpellDataArray.Count > 0)
                    {
                        spelldata.SpellStartTime = curtm;
                        spelldata.SpellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                        if (spelldata.SpellPeriod == 0)
                        {
                            spelldata.SpellPeriod = 1;
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
                    spelldata.SpellMode = 1;
                }
                if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
                {
                    tableData.pumpStatus1 = new List<double>();
                    tableData.TankLevel2ft = new List<double>();
                    tableData.SpellTimeArray = new List<string>();
                    tableData.V1N_V = new List<double>();
                    tableData.V2N_V = new List<double>();
                    tableData.V3N_V = new List<double>();
                    tableData.I1A = new List<double>();
                    tableData.I2A = new List<double>();
                    tableData.I3A = new List<double>();
                    tableData.W_kwatt = new List<double>();
                    tableData.VAR_kvar = new List<double>();
                    tableData.VA_kva = new List<double>();
                    tableData.VA_SUM_kva = new List<double>();
                    tableData.PF = new List<double>();
                    tableData.FreqHz = new List<double>();
                    tableData.V12_V = new List<double>();
                    tableData.V23_V = new List<double>();
                    tableData.V13_V = new List<double>();
                    tableData.V1THD = new List<double>();
                    tableData.V2THD = new List<double>();
                    tableData.V3THD = new List<double>();
                    tableData.CurrentTrip1 = new List<double>();
                    tableData.CurrentTrip2 = new List<double>();
                    tableData.VoltageTrip1 = new List<double>();
                    tableData.VoltageTrip2 = new List<double>();
                    tableData.P1AutoMannual = new List<double>();
                    tableData.P2AutoMannual = new List<double>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        tableData.locationName = location;
                        tableData.pumpStatus1.Add(Convert.ToInt32(dr["P1 Status"]));
                        tableData.TankLevel2ft.Add(Convert.ToDouble(dr["Tank Level2 (ft)"]));
                        tableData.SpellTimeArray.Add((dr["tim"]).ToString());
                        ///////////////////////////////
                        tableData.V1N_V.Add(Convert.ToDouble(dr["V1N (V)"]));
                        tableData.V2N_V.Add(Convert.ToDouble(dr["V2N (V)"]));
                        tableData.V3N_V.Add(Convert.ToDouble(dr["V3N (V)"]));
                        tableData.I1A.Add(Convert.ToDouble(dr["I1 (A)"]));
                        tableData.I2A.Add(Convert.ToDouble(dr["I2 (A)"]));
                        tableData.I3A.Add(Convert.ToDouble(dr["I3 (A)"]));
                        tableData.W_kwatt.Add(Convert.ToDouble(dr["W (kwatt)"]));
                        tableData.VAR_kvar.Add(Convert.ToDouble(dr["VAR (kvar)"]));
                        tableData.VA_kva.Add(Convert.ToDouble(dr["VA (kva)"]));
                        tableData.VA_SUM_kva.Add(Convert.ToDouble(dr["VA-SUM (kva)"]));
                        tableData.PF.Add(Convert.ToDouble(dr["PF"]));
                        tableData.FreqHz.Add(Convert.ToDouble(dr["Freq (Hz)"]));
                        tableData.V12_V.Add(Convert.ToDouble(dr["V12 (v)"]));
                        tableData.V23_V.Add(Convert.ToDouble(dr["V23 (v)"]));
                        tableData.V13_V.Add(Convert.ToDouble(dr["V13 (v)"]));
                        tableData.V1THD.Add(Convert.ToDouble(dr["V1 THD (%)"]));
                        tableData.V2THD.Add(Convert.ToDouble(dr["V2 THD (%)"]));
                        tableData.V3THD.Add(Convert.ToDouble(dr["V3 THD (%)"]));
                        tableData.CurrentTrip1.Add(Convert.ToDouble(dr["Current Trip 1"]));
                        tableData.CurrentTrip2.Add(Convert.ToDouble(dr["Current Trip 2"]));
                        tableData.VoltageTrip1.Add(Convert.ToDouble(dr["Voltage Trip 1"]));
                        tableData.VoltageTrip2.Add(Convert.ToDouble(dr["Voltage Trip 2"]));
                        tableData.P1AutoMannual.Add(Convert.ToDouble(dr["P1 Auto/Mannual"]));
                        tableData.P2AutoMannual.Add(Convert.ToDouble(dr["P2 Auto/Mannual"]));
                        ///////////////////////////////
                    }
                    var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.SpellPeriod)));
                    int phour = (int)pp.TotalHours;
                    int pmin = (int)pp.Minutes;
                    int psec = (int)pp.Seconds;
                    string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                    tableData.workingHoursTodayP1 = pstr;
                    double workingInHoursManual = spellDataList.Where(r => r.SpellMode == 1).Sum(i => i.SpellPeriod);
                    double workingInHoursAuto = spellDataList.Where(r => r.SpellMode == 2).Sum(i => i.SpellPeriod);
                    //tableData.WorkingInHoursPump1 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    tableData.workingHoursTodayManualP1 = minutesToTime(spellDataList.Where(r => r.SpellMode == 1).Sum(i => i.SpellPeriod));
                    tableData.workingHoursTodaySchedulingP1 = minutesToTime(spellDataList.Where(r => r.SpellMode == 2).Sum(i => i.SpellPeriod));
                    if (spellDataList.Count == 0)
                    {
                        tableData.tankLevelAverage = "0";
                    }
                    else
                    {
                        double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                        if (avgWaterFlow == 0)
                        {
                            avgWaterFlow = 1;
                        }
                        tableData.tankLevelAverage = tableData.TankLevel2ft.DefaultIfEmpty().Average().ToString();
                        //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                        //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                    }
                }
                else
                {
                    tableData.pumpStatus1 = new List<double>();
                    tableData.TankLevel2ft = new List<double>();
                    tableData.SpellTimeArray = new List<string>();
                    tableData.V1N_V = new List<double>();
                    tableData.V2N_V = new List<double>();
                    tableData.V3N_V = new List<double>();
                    tableData.I1A = new List<double>();
                    tableData.I2A = new List<double>();
                    tableData.I3A = new List<double>();
                    tableData.W_kwatt = new List<double>();
                    tableData.VAR_kvar = new List<double>();
                    tableData.VA_kva = new List<double>();
                    tableData.VA_SUM_kva = new List<double>();
                    tableData.PF = new List<double>();
                    tableData.FreqHz = new List<double>();
                    tableData.V12_V = new List<double>();
                    tableData.V23_V = new List<double>();
                    tableData.V13_V = new List<double>();
                    tableData.V1THD = new List<double>();
                    tableData.V2THD = new List<double>();
                    tableData.V3THD = new List<double>();
                    tableData.CurrentTrip1 = new List<double>();
                    tableData.CurrentTrip2 = new List<double>();
                    tableData.VoltageTrip1 = new List<double>();
                    tableData.VoltageTrip2 = new List<double>();
                    tableData.P1AutoMannual = new List<double>();
                    tableData.P2AutoMannual = new List<double>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        tableData.locationName = location;
                        tableData.pumpStatus1.Add(Convert.ToInt32(dr["P1 Status"]));
                        tableData.TankLevel2ft.Add(Convert.ToDouble(dr["Tank Level2 (ft)"]));
                        tableData.SpellTimeArray.Add((dr["tim"]).ToString());
                        ///////////////////////////////
                        tableData.V1N_V.Add(Convert.ToDouble(dr["V1N (V)"]));
                        tableData.V2N_V.Add(Convert.ToDouble(dr["V2N (V)"]));
                        tableData.V3N_V.Add(Convert.ToDouble(dr["V3N (V)"]));
                        tableData.I1A.Add(Convert.ToDouble(dr["I1 (A)"]));
                        tableData.I2A.Add(Convert.ToDouble(dr["I2 (A)"]));
                        tableData.I3A.Add(Convert.ToDouble(dr["I3 (A)"]));
                        tableData.W_kwatt.Add(Convert.ToDouble(dr["W (kwatt)"]));
                        tableData.VAR_kvar.Add(Convert.ToDouble(dr["VAR (kvar)"]));
                        tableData.VA_kva.Add(Convert.ToDouble(dr["VA (kva)"]));
                        tableData.VA_SUM_kva.Add(Convert.ToDouble(dr["VA-SUM (kva)"]));
                        tableData.PF.Add(Convert.ToDouble(dr["PF"]));
                        tableData.FreqHz.Add(Convert.ToDouble(dr["Freq (Hz)"]));
                        tableData.V12_V.Add(Convert.ToDouble(dr["V12 (v)"]));
                        tableData.V23_V.Add(Convert.ToDouble(dr["V23 (v)"]));
                        tableData.V13_V.Add(Convert.ToDouble(dr["V13 (v)"]));
                        tableData.V1THD.Add(Convert.ToDouble(dr["V1 THD (%)"]));
                        tableData.V2THD.Add(Convert.ToDouble(dr["V2 THD (%)"]));
                        tableData.V3THD.Add(Convert.ToDouble(dr["V3 THD (%)"]));
                        tableData.CurrentTrip1.Add(Convert.ToDouble(dr["Current Trip 1"]));
                        tableData.CurrentTrip2.Add(Convert.ToDouble(dr["Current Trip 2"]));
                        tableData.VoltageTrip1.Add(Convert.ToDouble(dr["Voltage Trip 1"]));
                        tableData.VoltageTrip2.Add(Convert.ToDouble(dr["Voltage Trip 2"]));
                        tableData.P1AutoMannual.Add(Convert.ToDouble(dr["P1 Auto/Mannual"]));
                        tableData.P2AutoMannual.Add(Convert.ToDouble(dr["P2 Auto/Mannual"]));
                        ///////////////////////////////
                    }
                    var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.SpellPeriod)));
                    int phour = (int)pp.TotalHours;
                    int pmin = (int)pp.Minutes;
                    int psec = (int)pp.Seconds;
                    string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                    tableData.workingHoursTodayP1 = pstr;
                    double workingInHoursManual = spellDataList.Where(r => r.SpellMode == 1).Sum(i => i.SpellPeriod);
                    double workingInHoursAuto = spellDataList.Where(r => r.SpellMode == 2).Sum(i => i.SpellPeriod);
                    //tableData.WorkingInHoursPump1 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    tableData.workingHoursTodayManualP1 = minutesToTime(spellDataList.Where(r => r.SpellMode == 1).Sum(i => i.SpellPeriod));
                    tableData.workingHoursTodaySchedulingP1 = minutesToTime(spellDataList.Where(r => r.SpellMode == 2).Sum(i => i.SpellPeriod));
                    double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                    tableData.tankLevelAverage = tableData.TankLevel2ft.DefaultIfEmpty().Average().ToString();
                }
                tableData.tankLevelAverage = tableData.TankLevel2ft.DefaultIfEmpty().Average().ToString();
            }
            //tableData.Pump1TimeArray = spellDataList.FirstOrDefault().SpellTimeArray;
            //return tableData;
            return tableData;
        }
        public string minutesToTime(double minutes)
        {
            var pTime = TimeSpan.FromMinutes(minutes);
            int phour = (int)pTime.TotalHours;
            int pmin = (int)pTime.Minutes;
            int psec = (int)pTime.Seconds;
            string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
            return pstr;
        }
        public string getPump2WorkingHoursManual(DateTime fromDT, DateTime toDt, string res)
        {
            return "";
        }
        public string getPump2WorkingHoursAuto(DateTime fromDT, DateTime toDt, string res)
        {
            return "";
        }
        public string getPump2WorkingHoursRemote(DateTime fromDT, DateTime toDt, string res)
        {
            return "";
        }
        public StorageTankTableData getPump2WorkingHours(DataTable dt)
        {
            var tableData = new StorageTankTableData();
            var spelldata = new StoragePump2SpellData();
            if (dt.Rows.Count > 1)
            {
                string location = dt.Rows[0]["Location"].ToString();
                double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["P2 Status"])), 2);
                string currentTime = dt.Rows[0]["tim"].ToString();
                double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
                bool S = false;
                bool E = false;
                bool T = true;
                bool F = false;
                int spell = 0;
                List<StoragePump2SpellData> spellDataList = new List<StoragePump2SpellData>();
                string curtm = "";
                foreach (DataRow dr in dt.Rows)
                {
                    double currValue = Math.Round((Convert.ToDouble(dr["P2 Status"])), 2);
                    double FlowRate = Math.Round((Convert.ToDouble(dr["Tank Level2 (ft)"])), 2);
                    double currValueManual = Math.Round((Convert.ToDouble(dr["P2 Auto/Mannual"])), 2);
                    string currTime = dr["tim"].ToString();
                    string clearaceTime = "";
                    //start scenario 3 (inactive)
                    if (DeltaMinutes > 28800)
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
                                        if (currValueManual == 0)
                                        {
                                            spelldata.SpellMode = 1;
                                        }
                                        else
                                        {
                                            spelldata.SpellMode = 2;
                                        }
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
                                    if (currValueManual == 0)
                                    {
                                        spelldata.SpellMode = 1;
                                    }
                                    else
                                    {
                                        spelldata.SpellMode = 2;
                                    }
                                }
                            }
                            else if (E == T && S == F)
                            {
                                if (currValue < 1)
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
                                    spelldata.SpellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                    if (spelldata.SpellPeriod == 0)
                                    {
                                        spelldata.SpellPeriod = 1;
                                    }
                                    spellDataList.Add(spelldata);
                                    spelldata = new StoragePump2SpellData();
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
                                        if (currValueManual == 0)
                                        {
                                            spelldata.SpellMode = 1;
                                        }
                                        else
                                        {
                                            spelldata.SpellMode = 2;
                                        }
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
                                    if (currValueManual == 0)
                                    {
                                        spelldata.SpellMode = 1;
                                    }
                                    else
                                    {
                                        spelldata.SpellMode = 2;
                                    }
                                }
                            }
                            else if (E == T && S == F)
                            {
                                if (currValue < 1)
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
                                    spelldata.SpellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                    if (spelldata.SpellPeriod == 0)
                                    {
                                        spelldata.SpellPeriod = 1;
                                    }
                                    //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                    //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                    spellDataList.Add(spelldata);
                                    spelldata = new StoragePump2SpellData();
                                    string s = JsonConvert.SerializeObject(spellDataList);
                                }
                            }
                        }
                        // end  scenario 2 (uncleared/ ponding continues)
                    }
                    curtm = currTime;
                    if (dr == dt.Rows[dt.Rows.Count - 1] && currValue > 0)
                    {
                        spelldata.SpellStartTime = currTime;
                        if (spelldata.SpellDataArray.Count > 0 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                        {
                            //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                            //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                            //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                            //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                            //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                            spelldata.SpellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                            if (spelldata.SpellPeriod == 0)
                            {
                                spelldata.SpellPeriod = 1;
                            }
                            //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                            //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                            spellDataList.Add(spelldata);
                            spelldata = new StoragePump2SpellData();
                            string s = JsonConvert.SerializeObject(spellDataList);
                        }
                    }
                }
                if (spellDataList.Count < 1)
                {
                    if (spelldata.SpellDataArray.Count > 0)
                    {
                        spelldata.SpellStartTime = curtm;
                        spelldata.SpellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                        if (spelldata.SpellPeriod == 0)
                        {
                            spelldata.SpellPeriod = 1;
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
                    spelldata.SpellMode = 1;
                }
                if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
                {
                    tableData.pumpStatus2 = new List<double>();
                    //tableData.PumpStatus2 = new List<double>();
                    //tableData.PumpStatus3 = new List<double>();
                    //tableData.PumpStatus4 = new List<double>();
                    //tableData.PumpStatus5 = new List<double>();
                    //tableData.PumpStatus6 = new List<double>();
                    //tableData.PumpStatus7 = new List<double>();
                    //tableData.PumpStatus8 = new List<double>();
                    //tableData.PumpStatus9 = new List<double>();
                    //tableData.PumpStatus10 = new List<double>();
                    tableData.TankLevel2ft = new List<double>();
                    tableData.SpellTimeArray = new List<string>();
                    //tableData.Well2Level = new List<double>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        tableData.locationName = location;
                        tableData.pumpStatus2.Add(Convert.ToInt32(dr["P2 Status"]));
                        tableData.TankLevel2ft.Add(Convert.ToDouble(dr["Tank Level2 (ft)"]));
                        tableData.SpellTimeArray.Add((dr["tim"]).ToString());
                    }
                    var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.SpellPeriod)));
                    int phour = (int)pp.TotalHours;
                    int pmin = (int)pp.Minutes;
                    int psec = (int)pp.Seconds;
                    string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                    tableData.workingHoursTodayP2 = pstr;
                    double workingInHoursManual = spellDataList.Where(r => r.SpellMode == 1).Sum(i => i.SpellPeriod);
                    double workingInHoursAuto = spellDataList.Where(r => r.SpellMode == 2).Sum(i => i.SpellPeriod);
                    //tableData.WorkingInHoursPump1 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    tableData.workingHoursTodayManualP2 = minutesToTime(spellDataList.Where(r => r.SpellMode == 1).Sum(i => i.SpellPeriod));
                    tableData.workingHoursTodaySchedulingP2 = minutesToTime(spellDataList.Where(r => r.SpellMode == 2).Sum(i => i.SpellPeriod));
                    if (spellDataList.Count == 0)
                    {
                        tableData.tankLevelAverage = "0";
                    }
                    else
                    {
                        double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                        if (avgWaterFlow == 0)
                        {
                            avgWaterFlow = 1;
                        }
                        tableData.tankLevelAverage = tableData.TankLevel2ft.DefaultIfEmpty().Average().ToString();
                        //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                        //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                    }
                }
                else
                {
                    tableData.pumpStatus2 = new List<double>();
                    //tableData.PumpStatus2 = new List<double>();
                    //tableData.PumpStatus3 = new List<double>();
                    //tableData.PumpStatus4 = new List<double>();
                    //tableData.PumpStatus5 = new List<double>();
                    //tableData.PumpStatus6 = new List<double>();
                    //tableData.PumpStatus7 = new List<double>();
                    //tableData.PumpStatus8 = new List<double>();
                    //tableData.PumpStatus9 = new List<double>();
                    //tableData.PumpStatus10 = new List<double>();
                    tableData.TankLevel2ft = new List<double>();
                    tableData.SpellTimeArray = new List<string>();
                    //tableData.Well2Level = new List<double>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        tableData.locationName = location;
                        tableData.pumpStatus2.Add(Convert.ToInt32(dr["P2 Status"]));
                        tableData.TankLevel2ft.Add(Convert.ToDouble(dr["Tank Level2 (ft)"]));
                        tableData.SpellTimeArray.Add((dr["tim"]).ToString());
                    }
                    var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.SpellPeriod)));
                    int phour = (int)pp.TotalHours;
                    int pmin = (int)pp.Minutes;
                    int psec = (int)pp.Seconds;
                    string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                    tableData.workingHoursTodayP2 = pstr;
                    double workingInHoursManual = spellDataList.Where(r => r.SpellMode == 1).Sum(i => i.SpellPeriod);
                    double workingInHoursAuto = spellDataList.Where(r => r.SpellMode == 2).Sum(i => i.SpellPeriod);
                    //tableData.WorkingInHoursPump1 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2).ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    tableData.workingHoursTodayManualP2 = minutesToTime(spellDataList.Where(r => r.SpellMode == 1).Sum(i => i.SpellPeriod));
                    tableData.workingHoursTodaySchedulingP2 = minutesToTime(spellDataList.Where(r => r.SpellMode == 2).Sum(i => i.SpellPeriod));
                    double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                    tableData.tankLevelAverage = tableData.TankLevel2ft.DefaultIfEmpty().Average().ToString();
                }
                tableData.tankLevelAverage = tableData.TankLevel2ft.DefaultIfEmpty().Average().ToString();
                //tableData.Pump1TimeArray = spellDataList.FirstOrDefault().SpellTimeArray;
                //return tableData;
            }
            return tableData;
        }

    }
}