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
    public class RecyclingPlantController_With_13Hours_Increment : Controller
    {
        // GET: RecyclingPlant
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StatusReport()
        {
            return View();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        public PartialViewResult _StatusReportView()
        {
            
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var recycleDataList = new List<RecylcingPlantClass>();
            var recycleFinalDataList = new List<RecylcingPlantClass>();
            int resourceID = 0;
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();

                    string getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateID = 67";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    string resourceLocation = "";
                    int ite = 0;
                    //iterate through the list of resources within the desired set of resources chosen
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        RecylcingPlantClass df = new RecylcingPlantClass();
                        df.PumpStatus1 = new List<double>();
                        df.PumpStatus2 = new List<double>();
                        df.PumpStatus3 = new List<double>();
                        resourceID = Convert.ToInt32(drRes["ResourceID"]);
                        resourceLocation = drRes["ResourceLocation"].ToString();
                        string Dashdtquery = ";WITH cte AS ( ";
                        Dashdtquery += "SELECT* FROM ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                        Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                        Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                        Dashdtquery += "s.InsertionDateTime as tim ,";
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour,13,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID =  " + resourceID + "  and ";
                        Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,13,GETDATE())), 0) ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        Dashdtquery += "[RecyclePumpStatus1] ";
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
                            RecylcingPlantClass sd = getAllSpellsForPump1(Dashdt);
                            df.LocationName = resourceLocation;
                            df.PumpStatus1 = sd.PumpStatus1;
                            //tubewellDataList.Add(sd);
                            df.WorkingInHoursPump1 = sd.WorkingInHoursPump1;
                            df.PumpTimeArray = sd.PumpTimeArray;
                            df.WorkingHoursPump1 = sd.WorkingHoursPump1;
                        }
                        else
                        {
                            RecylcingPlantClass sd = new RecylcingPlantClass();
                            df.PumpStatus1 = new List<double>();
                            df.LocationName = resourceLocation;
                            df.WorkingInHoursPump1 = "0";
                            df.PumpTimeArray = new List<string>();
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
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour,13,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID =  " + resourceID + "  and ";
                        Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,13,GETDATE())), 0) ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        Dashdtquery += "[RecyclePumpStatus2]  ";
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
                            RecylcingPlantClass sd = getAllSpellsForPump2(Dashdt);
                            df.PumpStatus2 = sd.PumpStatus2;
                            df.LocationName = resourceLocation;
                            df.WorkingInHoursPump2 = sd.WorkingInHoursPump2;
                            df.WorkingHoursPump2 = sd.WorkingHoursPump2;
                        }
                        else
                        {
                            RecylcingPlantClass sd = new RecylcingPlantClass();
                            df.PumpStatus2 = new List<double>();
                            df.LocationName = resourceLocation;
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
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour,13,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID =  " + resourceID + "  and ";
                        Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,13,GETDATE())), 0) ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        Dashdtquery += "[RecyclePumpStatus3] ";
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
                            RecylcingPlantClass sd = getAllSpellsForPump3(Dashdt);
                            df.PumpStatus3 = sd.PumpStatus3;
                            df.LocationName = resourceLocation;
                            df.WorkingInHoursPump3 = sd.WorkingInHoursPump3;
                            df.WorkingHoursPump3 = sd.WorkingHoursPump3;
                        }
                        else
                        {
                            RecylcingPlantClass sd = new RecylcingPlantClass();
                            //recycleDataList.Add(sd);
                            df.PumpStatus3 = new List<double>();
                            df.LocationName = resourceLocation;
                            df.WorkingInHoursPump3 = "0";
                            df.WorkingHoursPump3 = 0;
                        }
                        recycleDataList.Add(df);
                        ////////////////////////////////////////////////////
                        ///
                    }
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
            Session["ReportTitle"] = "Current Status of Shaukat Khanum Disposal Station at  " + DateTime.Now.AddHours(13).ToString("dd'/'MM'/'yyyy HH:mm:ss") + " (Reflected for Today)";

            /////////////////////////////////////////////////////////////////////////////
            string scriptString = "";
            scriptString += "var chart1 = new CanvasJS.Chart(\"chartContainer1\", {";
            scriptString += "theme: \"light2\",";
            scriptString += "animationEnabled: true,";
            scriptString += "zoomEnabled: true, ";
            scriptString += "title: {text: \" \" },";
            scriptString += "subtitles: [{text: \" Submersible Pump  \" }],";
            scriptString += "axisY: {labelFontSize: 10, labelFormatter: function(){ return \" \"; }},";
            scriptString += "toolTip: { shared: false , contentFormatter: function(e){ var str = \" \" ; for (var i = 0; i < e.entries.length; i++){ var utcSeconds = e.entries[i].dataPoint.x; var d = new Date(utcSeconds); if(e.entries[i].dataPoint.y == 0){ var temp = e.entries[i].dataSeries.name + \" \" +\": OFF at  \" + d.toLocaleString('en-IN'); str = str+temp; } else { var temp = e.entries[i].dataSeries.name + \" \" +\": ON at \" + d.toLocaleString('en-IN'); str = str+temp; } } return (str); }},";
            //scriptString += "toolTip: { shared: false },";
            scriptString += "legend: { cursor: \"pointer\", fontSize: 10},";
            scriptString += " data: [";
            foreach (var item in recycleDataList)
            {
                if (item.PumpTimeArray.Count > 0)
                {
                    //scriptString += "{ type: \"area\", name: \"" + item.LocationName + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
                    scriptString += "{ type: \"area\", name: \"" + item.LocationName + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\",  ";
                    List<DataPoint> dataPointsPump1 = new List<DataPoint>();
                    for (int i = 0; i < item.PumpTimeArray.Count; i++)
                    {
                        dataPointsPump1.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(item.PumpTimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(item.PumpStatus1[i])));
                    }
                    scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump1) + "";
                    scriptString += "},";
                }
            }            /////////////
            scriptString = scriptString + "]";
            scriptString = scriptString + "}";
            scriptString += ");";
            ViewData["chartData1"] = scriptString;
            /////////////////////////////////////////////////////////////////////////////////
            ///
            /////////////////////////////////////////////////////////////////////////////
            string scriptString2 = "";
            scriptString2 += "var chart2 = new CanvasJS.Chart(\"chartContainer2\", {";
            scriptString2 += "theme: \"light2\",";
            scriptString2 += "animationEnabled: true,";
            scriptString2 += "zoomEnabled: true, ";
            scriptString2 += "title: {text: \" \" },";
            scriptString2 += "subtitles: [{text: \" Filtered Water Pump  \" }],";
            scriptString2 += "axisY: {labelFontSize: 10, labelFormatter: function(){ return \" \"; }},";
            scriptString2 += "toolTip: { shared: false , contentFormatter: function(e){ var str = \" \" ; for (var i = 0; i < e.entries.length; i++){ var utcSeconds = e.entries[i].dataPoint.x; var d = new Date(utcSeconds); if(e.entries[i].dataPoint.y == 0){ var temp = e.entries[i].dataSeries.name + \" \" +\": OFF at  \" + d.toLocaleString('en-IN'); str = str+temp; } else { var temp = e.entries[i].dataSeries.name + \" \" +\": ON at \" + d.toLocaleString('en-IN'); str = str+temp; } } return (str); }},";
            scriptString2 += "legend: { cursor: \"pointer\", fontSize: 10},";
            scriptString2 += " data: [";
            foreach (var item in recycleDataList)
            {
                if (item.PumpTimeArray.Count > 0)
                {
                    scriptString2 += "{ type: \"area\", name: \"" + item.LocationName + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\",  ";
                    List<DataPoint> dataPointsPump2 = new List<DataPoint>();
                    for (int i = 0; i < item.PumpTimeArray.Count; i++)
                    {
                        dataPointsPump2.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(item.PumpTimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(item.PumpStatus2[i])));
                    }
                    scriptString2 += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump2) + "";
                    scriptString2 += "},";
                }
                
            }            /////////////
            scriptString2 = scriptString2 + "]";
            scriptString2 = scriptString2 + "}";
            scriptString2 += ");";
            ViewData["chartData2"] = scriptString2;
            /////////////////////////////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////////////////////////
            string scriptString3 = "";
            scriptString3 += "var chart3 = new CanvasJS.Chart(\"chartContainer3\", {";
            scriptString3 += "theme: \"light2\",";
            scriptString3 += "animationEnabled: true,";
            scriptString3 += "zoomEnabled: true, ";
            scriptString3 += "title: {text: \" \" },";
            scriptString3 += "subtitles: [{text: \" Fresh Water Pump  \" }],";
            scriptString3 += "axisY: {labelFontSize: 10, labelFormatter: function(){ return \" \"; }},";
            scriptString3 += "toolTip: { shared: false , contentFormatter: function(e){ var str = \" \" ; for (var i = 0; i < e.entries.length; i++){ var utcSeconds = e.entries[i].dataPoint.x; var d = new Date(utcSeconds); if(e.entries[i].dataPoint.y == 0){ var temp = e.entries[i].dataSeries.name + \" \" +\": OFF at  \" + d.toLocaleString('en-IN'); str = str+temp; } else { var temp = e.entries[i].dataSeries.name + \" \" +\": ON at \" + d.toLocaleString('en-IN'); str = str+temp; } } return (str); }},";
            scriptString3 += "legend: { cursor: \"pointer\", fontSize: 10},";
            scriptString3 += " data: [";
            foreach (var item in recycleDataList)
            {
                if (item.PumpTimeArray.Count > 0)
                {
                    scriptString3 += "{ type: \"area\", name: \"" + item.LocationName + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\",  ";
                    List<DataPoint> dataPointsPump3 = new List<DataPoint>();
                    for (int i = 0; i < item.PumpTimeArray.Count; i++)
                    {
                        dataPointsPump3.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(item.PumpTimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(item.PumpStatus3[i])));
                    }
                    scriptString3 += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump3) + "";
                    scriptString3 += "},";
                }
            }            /////////////
            scriptString3 = scriptString3 + "]";
            scriptString3 = scriptString3 + "}";
            scriptString3 += ");";
            ViewData["chartData3"] = scriptString3;
            /////////////////////////////////////////////////////////////////////////////////

            return PartialView(recycleDataList);
        }

        public RecylcingPlantClass getAllSpellsForPump1(DataTable dt)
        {
            var tableData = new RecylcingPlantClass();
            var spelldata = new RecyclePump1SpellData();
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["RecyclePumpStatus1"])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<RecyclePump1SpellData> spellDataList = new List<RecyclePump1SpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["RecyclePumpStatus1"])), 2);
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
                                    spelldata.SpellDataArray.Add(currValue);
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
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

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
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellTimeArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new RecyclePump1SpellData();
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
                                    spelldata.SpellDataArray.Add(currValue);
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
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

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
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellTimeArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new RecyclePump1SpellData();
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
                if (spelldata.SpellTimeArray.Count > 0)
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
            if (spelldata.SpellTimeArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                tableData.PumpStatus1 = new List<double>();
                tableData.PumpTimeArray = new List<string>();
                //tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.LocationName = location;
                    tableData.PumpStatus1.Add(Convert.ToInt32(dr["RecyclePumpStatus1"]));
                    tableData.PumpTimeArray.Add((dr["tim"]).ToString());
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump1 = pstr;
                tableData.WorkingHoursPump1 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2);
                
            }
            else
            {
                tableData.PumpStatus1 = new List<double>();
                tableData.PumpTimeArray = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.LocationName = location;
                    tableData.PumpStatus1.Add(Convert.ToInt32(dr["RecyclePumpStatus1"]));
                    tableData.PumpTimeArray.Add((dr["tim"]).ToString());
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump1 = pstr;
                tableData.WorkingHoursPump1 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2);
                
            }
            return tableData;
        }


        public RecylcingPlantClass getAllSpellsForPump2(DataTable dt)
        {
            var tableData = new RecylcingPlantClass();
            var spelldata = new RecyclePump2SpellData();
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["RecyclePumpStatus2"])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<RecyclePump2SpellData> spellDataList = new List<RecyclePump2SpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["RecyclePumpStatus2"])), 2);
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
                                    spelldata.SpellDataArray.Add(currValue);
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
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

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
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellTimeArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new RecyclePump2SpellData();
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
                                    spelldata.SpellDataArray.Add(currValue);
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
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

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
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellTimeArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new RecyclePump2SpellData();
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
                if (spelldata.SpellTimeArray.Count > 0)
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
            if (spelldata.SpellTimeArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                tableData.PumpStatus2 = new List<double>();
                tableData.PumpTimeArray = new List<string>();
                //tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.LocationName = location;
                    tableData.PumpStatus2.Add(Convert.ToInt32(dr["RecyclePumpStatus2"]));
                    tableData.PumpTimeArray.Add((dr["tim"]).ToString());
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump2 = pstr;
                tableData.WorkingHoursPump2 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2);

            }
            else
            {
                tableData.PumpStatus2 = new List<double>();
                tableData.PumpTimeArray = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.LocationName = location;
                    tableData.PumpStatus2.Add(Convert.ToInt32(dr["RecyclePumpStatus2"]));
                    tableData.PumpTimeArray.Add((dr["tim"]).ToString());
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump2 = pstr;
                tableData.WorkingHoursPump2 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2);

            }
            return tableData;
        }


        public RecylcingPlantClass getAllSpellsForPump3(DataTable dt)
        {
            var tableData = new RecylcingPlantClass();
            var spelldata = new RecyclePump3SpellData();
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["RecyclePumpStatus3"])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<RecyclePump3SpellData> spellDataList = new List<RecyclePump3SpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["RecyclePumpStatus3"])), 2);
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
                                    spelldata.SpellDataArray.Add(currValue);
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
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

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
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellTimeArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new RecyclePump3SpellData();
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
                                    spelldata.SpellDataArray.Add(currValue);
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
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

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
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellTimeArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new RecyclePump3SpellData();
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
                if (spelldata.SpellTimeArray.Count > 0)
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
            if (spelldata.SpellTimeArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                tableData.PumpStatus3 = new List<double>();
                tableData.PumpTimeArray = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.LocationName = location;
                    tableData.PumpStatus3.Add(Convert.ToInt32(dr["RecyclePumpStatus3"]));
                    tableData.PumpTimeArray.Add((dr["tim"]).ToString());
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump3 = pstr;
                tableData.WorkingHoursPump3 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2);

            }
            else
            {
                tableData.PumpStatus3 = new List<double>();
                tableData.PumpTimeArray = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.LocationName = location;
                    tableData.PumpStatus3.Add(Convert.ToInt32(dr["RecyclePumpStatus3"]));
                    tableData.PumpTimeArray.Add((dr["tim"]).ToString());
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.WorkingInHoursPump3 = pstr;
                tableData.WorkingHoursPump3 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2);

            }
            return tableData;
        }

        public ActionResult PumpReport()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("All");
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 67))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Recycling Plants";
            rs.resourceName = "All";
            rs.dateFrom = "";
            rs.timeFrom = "";
            rs.dateTo = "";
            rs.timeTo = "";
            return View(rs);
        }

        [HttpPost]
        public ActionResult PumpReport(FormCollection review)
        {
            var ress = review["resource"];
            if (ress == null)
            {
                ress = "All";
            }
            string resource = ress;
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
            if (tt_time == "12:00 AM" || tt_time == "11:59 PM")
            {
                tt_time = "11:59:59 PM";
            }
            DateTime FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
            DateTime FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("All");
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 67))
            {
                ResourceList.Add(item.ResourceLocation);
            }
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
            rs.resourceType = "Tubewells";
            rs.resourceName = resource;
            rs.dateFrom = dateFrom.ToString();
            rs.timeFrom = TF;
            rs.dateTo = dateTo.ToString();
            rs.timeTo = TT;
            return View(rs);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        public ActionResult _PumpReportView(string resources, string datFrom, string timFrom, string datTo, string timTo)
        {
            DateTime FinalTimeFrom = DateTime.Now;
            DateTime FinalTimeTo = DateTime.Now;
            if (datFrom == "" && timFrom == "" && datTo == "" && timTo == "")
            {
                FinalTimeFrom = DateTime.Now.AddHours(13).Date;
                FinalTimeTo = DateTime.Now.AddHours(13).AddDays(1).Date;
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
                if (tt_time == "12:00 AM" || tt_time == "11:59 PM")
                {
                    tt_time = "11:59:59 PM";
                }
                FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
                FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            }
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var recycleDataList = new List<RecylcingPlantClass>();
            var recycleFinalDataList = new List<RecylcingPlantClass>();
            int resourceID = 0;
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();

                    string getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateID = 67";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    string resourceLocation = "";
                    int ite = 0;
                    //iterate through the list of resources within the desired set of resources chosen
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        RecylcingPlantClass df = new RecylcingPlantClass();
                        df.PumpStatus1 = new List<double>();
                        df.PumpStatus2 = new List<double>();
                        df.PumpStatus3 = new List<double>();
                        resourceID = Convert.ToInt32(drRes["ResourceID"]);
                        resourceLocation = drRes["ResourceLocation"].ToString();
                        string Dashdtquery = ";WITH cte AS ( ";
                        Dashdtquery += "SELECT* FROM ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                        Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                        Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                        Dashdtquery += "s.InsertionDateTime as tim ,";
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 13,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID =  " + resourceID + "  and ";
                        Dashdtquery += "s.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and s.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121) ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        Dashdtquery += "[RecyclePumpStatus1] ";
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
                            RecylcingPlantClass sd = getAllSpellsForPump1(Dashdt);
                            df.LocationName = resourceLocation;
                            df.PumpStatus1 = sd.PumpStatus1;
                            //tubewellDataList.Add(sd);
                            df.WorkingInHoursPump1 = sd.WorkingInHoursPump1;
                            df.PumpTimeArray = sd.PumpTimeArray;
                            df.WorkingHoursPump1 = sd.WorkingHoursPump1;
                        }
                        else
                        {
                            RecylcingPlantClass sd = new RecylcingPlantClass();
                            df.PumpStatus1 = new List<double>();
                            df.LocationName = resourceLocation;
                            df.WorkingInHoursPump1 = "0";
                            df.PumpTimeArray = new List<string>();
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
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 13,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID =  " + resourceID + "  and ";
                        Dashdtquery += "s.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and s.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121) ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        Dashdtquery += "[RecyclePumpStatus2]  ";
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
                            RecylcingPlantClass sd = getAllSpellsForPump2(Dashdt);
                            df.PumpStatus2 = sd.PumpStatus2;
                            df.LocationName = resourceLocation;
                            df.WorkingInHoursPump2 = sd.WorkingInHoursPump2;
                            df.WorkingHoursPump2 = sd.WorkingHoursPump2;
                        }
                        else
                        {
                            RecylcingPlantClass sd = new RecylcingPlantClass();
                            df.PumpStatus2 = new List<double>();
                            df.LocationName = resourceLocation;
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
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour,13,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID =  " + resourceID + "  and ";
                        Dashdtquery += "s.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and s.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121) ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        Dashdtquery += "[RecyclePumpStatus3] ";
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
                            RecylcingPlantClass sd = getAllSpellsForPump3(Dashdt);
                            df.PumpStatus3 = sd.PumpStatus3;
                            df.LocationName = resourceLocation;
                            df.WorkingInHoursPump3 = sd.WorkingInHoursPump3;
                            df.WorkingHoursPump3 = sd.WorkingHoursPump3;
                        }
                        else
                        {
                            RecylcingPlantClass sd = new RecylcingPlantClass();
                            //recycleDataList.Add(sd);
                            df.PumpStatus3 = new List<double>();
                            df.LocationName = resourceLocation;
                            df.WorkingInHoursPump3 = "0";
                            df.WorkingHoursPump3 = 0;
                        }
                        recycleDataList.Add(df);
                        ////////////////////////////////////////////////////
                        ///
                    }
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
            Session["ReportTitle"] = "Current Status of Shaukat Khanum Disposal Station at  " + DateTime.Now.AddHours(13).ToString("dd'/'MM'/'yyyy HH:mm:ss") + " (Reflected for Today)";

            /////////////////////////////////////////////////////////////////////////////
            string scriptString = "";
            scriptString += "var chart1 = new CanvasJS.Chart(\"chartContainer1\", {";
            scriptString += "theme: \"light2\",";
            scriptString += "animationEnabled: true,";
            scriptString += "zoomEnabled: true, ";
            scriptString += "title: {text: \" \" },";
            scriptString += "subtitles: [{text: \" Submersible Pump  \" }],";
            scriptString += "axisY: {labelFontSize: 10, labelFormatter: function(){ return \" \"; }},";
            scriptString += "toolTip: { shared: false , contentFormatter: function(e){ var str = \" \" ; for (var i = 0; i < e.entries.length; i++){ var utcSeconds = e.entries[i].dataPoint.x; var d = new Date(utcSeconds); if(e.entries[i].dataPoint.y == 0){ var temp = e.entries[i].dataSeries.name + \" \" +\": OFF at  \" + d.toLocaleString('en-IN'); str = str+temp; } else { var temp = e.entries[i].dataSeries.name + \" \" +\": ON at \" + d.toLocaleString('en-IN'); str = str+temp; } } return (str); }},";
            //scriptString += "toolTip: { shared: false },";
            scriptString += "legend: { cursor: \"pointer\", fontSize: 10},";
            scriptString += " data: [";
            foreach (var item in recycleDataList)
            {
                if (item.PumpTimeArray.Count > 0)
                {
                    //scriptString += "{ type: \"area\", name: \"" + item.LocationName + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
                    scriptString += "{ type: \"area\", name: \"" + item.LocationName + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\",  ";
                    List<DataPoint> dataPointsPump1 = new List<DataPoint>();
                    for (int i = 0; i < item.PumpTimeArray.Count; i++)
                    {
                        dataPointsPump1.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(item.PumpTimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(item.PumpStatus1[i])));
                    }
                    scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump1) + "";
                    scriptString += "},";
                }
            }            /////////////
            scriptString = scriptString + "]";
            scriptString = scriptString + "}";
            scriptString += ");";
            ViewData["chartData1"] = scriptString;
            /////////////////////////////////////////////////////////////////////////////////
            ///
            /////////////////////////////////////////////////////////////////////////////
            string scriptString2 = "";
            scriptString2 += "var chart2 = new CanvasJS.Chart(\"chartContainer2\", {";
            scriptString2 += "theme: \"light2\",";
            scriptString2 += "animationEnabled: true,";
            scriptString2 += "zoomEnabled: true, ";
            scriptString2 += "title: {text: \" \" },";
            scriptString2 += "subtitles: [{text: \" Filtered Water Pump  \" }],";
            scriptString2 += "axisY: {labelFontSize: 10, labelFormatter: function(){ return \" \"; }},";
            scriptString2 += "toolTip: { shared: false , contentFormatter: function(e){ var str = \" \" ; for (var i = 0; i < e.entries.length; i++){ var utcSeconds = e.entries[i].dataPoint.x; var d = new Date(utcSeconds); if(e.entries[i].dataPoint.y == 0){ var temp = e.entries[i].dataSeries.name + \" \" +\": OFF at  \" + d.toLocaleString('en-IN'); str = str+temp; } else { var temp = e.entries[i].dataSeries.name + \" \" +\": ON at \" + d.toLocaleString('en-IN'); str = str+temp; } } return (str); }},";
            scriptString2 += "legend: { cursor: \"pointer\", fontSize: 10},";
            scriptString2 += " data: [";
            foreach (var item in recycleDataList)
            {
                if (item.PumpTimeArray.Count > 0)
                {
                    scriptString2 += "{ type: \"area\", name: \"" + item.LocationName + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\",  ";
                    List<DataPoint> dataPointsPump2 = new List<DataPoint>();
                    for (int i = 0; i < item.PumpTimeArray.Count; i++)
                    {
                        dataPointsPump2.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(item.PumpTimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(item.PumpStatus2[i])));
                    }
                    scriptString2 += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump2) + "";
                    scriptString2 += "},";
                }

            }            /////////////
            scriptString2 = scriptString2 + "]";
            scriptString2 = scriptString2 + "}";
            scriptString2 += ");";
            ViewData["chartData2"] = scriptString2;
            /////////////////////////////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////////////////////////
            string scriptString3 = "";
            scriptString3 += "var chart3 = new CanvasJS.Chart(\"chartContainer3\", {";
            scriptString3 += "theme: \"light2\",";
            scriptString3 += "animationEnabled: true,";
            scriptString3 += "zoomEnabled: true, ";
            scriptString3 += "title: {text: \" \" },";
            scriptString3 += "subtitles: [{text: \" Fresh Water Pump  \" }],";
            scriptString3 += "axisY: {labelFontSize: 10, labelFormatter: function(){ return \" \"; }},";
            scriptString3 += "toolTip: { shared: false , contentFormatter: function(e){ var str = \" \" ; for (var i = 0; i < e.entries.length; i++){ var utcSeconds = e.entries[i].dataPoint.x; var d = new Date(utcSeconds); if(e.entries[i].dataPoint.y == 0){ var temp = e.entries[i].dataSeries.name + \" \" +\": OFF at  \" + d.toLocaleString('en-IN'); str = str+temp; } else { var temp = e.entries[i].dataSeries.name + \" \" +\": ON at \" + d.toLocaleString('en-IN'); str = str+temp; } } return (str); }},";
            scriptString3 += "legend: { cursor: \"pointer\", fontSize: 10},";
            scriptString3 += " data: [";
            foreach (var item in recycleDataList)
            {
                if (item.PumpTimeArray.Count > 0)
                {
                    scriptString3 += "{ type: \"area\", name: \"" + item.LocationName + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\",  ";
                    List<DataPoint> dataPointsPump3 = new List<DataPoint>();
                    for (int i = 0; i < item.PumpTimeArray.Count; i++)
                    {
                        dataPointsPump3.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(item.PumpTimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(item.PumpStatus3[i])));
                    }
                    scriptString3 += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump3) + "";
                    scriptString3 += "},";
                }
            }            /////////////
            scriptString3 = scriptString3 + "]";
            scriptString3 = scriptString3 + "}";
            scriptString3 += ");";
            ViewData["chartData3"] = scriptString3;
            /////////////////////////////////////////////////////////////////////////////////

            return PartialView(recycleDataList);
        }



    }
}