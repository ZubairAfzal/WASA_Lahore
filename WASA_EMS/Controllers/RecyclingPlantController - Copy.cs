using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WASA_EMS.Controllers
{
    public class RecyclingPlantControllers : Controller
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
            RecylcingPlantClass df = new RecylcingPlantClass();
            df.PumpStatus1 = new List<double>();
            df.PumpStatus2 = new List<double>();
            df.PumpStatus3 = new List<double>();
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var recycleDataList = new List<RecylcingPlantClass>();
            var recycleFinalDataList = new List<RecylcingPlantClass>();
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
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 9,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID =  1059  and ";
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,9,GETDATE())), 0) ";
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
                        //tubewellDataList.Add(sd);
                        df.WorkingInHoursPump1 = sd.WorkingInHoursPump1;
                        df.PumpTimeArray = sd.PumpTimeArray;
                        df.WorkingHoursPump1 = sd.WorkingHoursPump1;
                    }
                    else
                    {
                        RecylcingPlantClass sd = new RecylcingPlantClass();
                        df.PumpStatus1 = new List<double>();
                        df.WorkingInHoursPump1 = "0";
                        df.PumpTimeArray = new List<string>();
                        df.WorkingHoursPump1 = 0;
                    }
                    //////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    //Dashdtquery = ";WITH cte AS ( ";
                    //Dashdtquery += "SELECT* FROM ";
                    //Dashdtquery += "( ";
                    //Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    //Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    //Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    //Dashdtquery += "s.InsertionDateTime as tim ,";
                    //Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 9,GETDATE ())) as DeltaMinutes ";
                    //Dashdtquery += "FROM tblEnergy s ";
                    //Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    //Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    //Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    //Dashdtquery += "where ";
                    //Dashdtquery += "r.ResourceID =  1059  and ";
                    //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,9,GETDATE())), 0) ";
                    //Dashdtquery += ") ";
                    //Dashdtquery += "AS SourceTable ";
                    //Dashdtquery += "PIVOT ";
                    //Dashdtquery += "( ";
                    //Dashdtquery += "SUM(pVal) FOR pID ";
                    //Dashdtquery += "IN ";
                    //Dashdtquery += "( ";
                    //Dashdtquery += "[RecyclePumpStatus1]  ";
                    //Dashdtquery += ") ";
                    //Dashdtquery += ")  ";
                    //Dashdtquery += "AS PivotTable ";
                    //Dashdtquery += ")  ";
                    //Dashdtquery += "SELECT* FROM cte ";
                    //Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    //Dashdtquery += "tim DESC";
                    //cmd = new SqlCommand(Dashdtquery, conn);
                    //sda = new SqlDataAdapter(Dashdtquery, conn);
                    //Dashdt.Clear();
                    //sda.Fill(Dashdt);
                    //if (Dashdt.Rows.Count > 0)
                    //{
                    //    RecylcingPlantClass sd = getAllSpellsForPump2(Dashdt);
                    //    df.PumpStatus2 = sd.PumpStatus2;
                    //    df.WorkingInHoursPump2 = sd.WorkingInHoursPump2;
                    //    df.WorkingHoursPump2 = sd.WorkingHoursPump2;
                    //}
                    //else
                    //{
                    //    RecylcingPlantClass sd = new RecylcingPlantClass();
                    //    df.PumpStatus2 = new List<double>();
                    //    df.WorkingInHoursPump2 = "0";
                    //    df.WorkingHoursPump2 = 0;
                    //}
                    //////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////
                    //Dashdtquery = ";WITH cte AS ( ";
                    //Dashdtquery += "SELECT* FROM ";
                    //Dashdtquery += "( ";
                    //Dashdtquery += "SELECT DISTINCT r.ResourceLocation AS Location, ";
                    //Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    //Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    //Dashdtquery += "s.InsertionDateTime as tim ,";
                    //Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 9,GETDATE ())) as DeltaMinutes ";
                    //Dashdtquery += "FROM tblEnergy s ";
                    //Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    //Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    //Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    //Dashdtquery += "where ";
                    //Dashdtquery += "r.ResourceID =  1059  and ";
                    //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,9,GETDATE())), 0) ";
                    //Dashdtquery += ") ";
                    //Dashdtquery += "AS SourceTable ";
                    //Dashdtquery += "PIVOT ";
                    //Dashdtquery += "( ";
                    //Dashdtquery += "SUM(pVal) FOR pID ";
                    //Dashdtquery += "IN ";
                    //Dashdtquery += "( ";
                    //Dashdtquery += "[PumpStatus3.],[Well1Level(ft)] ";
                    //Dashdtquery += ") ";
                    //Dashdtquery += ")  ";
                    //Dashdtquery += "AS PivotTable ";
                    //Dashdtquery += ")  ";
                    //Dashdtquery += "SELECT* FROM cte ";
                    //Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                    //Dashdtquery += "tim DESC";
                    //cmd = new SqlCommand(Dashdtquery, conn);
                    //sda = new SqlDataAdapter(Dashdtquery, conn);
                    //Dashdt.Clear();
                    //sda.Fill(Dashdt);
                    //if (Dashdt.Rows.Count > 0)
                    //{
                    //    RecylcingPlantClass sd = getAllSpellsForPump3(Dashdt);
                    //    df.PumpStatus3 = sd.PumpStatus3;
                    //    df.WorkingInHoursPump3 = sd.WorkingInHoursPump3;
                    //    df.WorkingHoursPump3 = sd.WorkingHoursPump3;
                    //}
                    //else
                    //{
                    //    RecylcingPlantClass sd = new RecylcingPlantClass();
                    //    recycleDataList.Add(sd);
                    //    df.PumpStatus3 = new List<double>();
                    //    df.WorkingInHoursPump3 = "0";
                    //    df.WorkingHoursPump3 = 0;
                    //}
                    ////////////////////////////////////////////////////
                    recycleFinalDataList.Add(df);
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
            Session["ReportTitle"] = "Current Status of Shaukat Khanum Disposal Station at  " + DateTime.Now.AddHours(9).ToString("dd'/'MM'/'yyyy HH:mm:ss") + " (Reflected for Today)";

            /////////////////////////////////////////////////////////////////////////////

            string scriptString = "";
            scriptString += "var chart1 = new CanvasJS.Chart(\"chartContainer1\", {";
            scriptString += "theme: \"light2\",";
            scriptString += "animationEnabled: true,";
            scriptString += "zoomEnabled: true, ";
            scriptString += "title: {text: \"Shaukat Khanum Disposal Station\" },";
            scriptString += "subtitles: [{text: \" Pumps Working and Wells Level in ft  \" }],";
            scriptString += "axisY: {suffix: \" \" },";
            scriptString += "toolTip: { shared: false },";
            scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
            scriptString += " data: [";
            scriptString += "{ type: \"area\", name: \"Pump 1\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
            List<DataPoint> dataPointsPump1 = new List<DataPoint>();
            for (int i = 0; i < df.PumpTimeArray.Count; i++)
            {
                dataPointsPump1.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.PumpTimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus1[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump1) + "";
            scriptString += "},";

            scriptString += "{ type: \"area\", name: \"Pump 2\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
            List<DataPoint> dataPointsPump2 = new List<DataPoint>();
            for (int i = 0; i < df.PumpTimeArray.Count; i++)
            {
                dataPointsPump2.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.PumpTimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus2[i])));

            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump2) + "";
            scriptString += "},";

            scriptString += "{ type: \"area\", name: \"Pump 3\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
            List<DataPoint> dataPointsPump3 = new List<DataPoint>();
            for (int i = 0; i < df.PumpTimeArray.Count; i++)
            {
                dataPointsPump3.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(df.PumpTimeArray[i]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(df.PumpStatus3[i])));
            }
            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPointsPump3) + "";
            scriptString += "},";
            

            /////////////
            scriptString = scriptString + "]";
            scriptString = scriptString + "}";
            scriptString += ");";
            ViewData["chartData"] = scriptString;
            /////////////////////////////////////////////////////////////////////////////////


            return PartialView(recycleFinalDataList);
        }

        public RecylcingPlantClass getAllSpellsForPump1(DataTable dt)
        {
            var tableData = new RecylcingPlantClass();
            var spelldata = new RecyclePump1SpellData();
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus1."])), 2);
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
                double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus1."])), 2);
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
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
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
                tableData.PumpTimeArray = new List<string>();
                //tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.LocationName = location;
                    tableData.PumpStatus1.Add(Convert.ToInt32(dr["PumpStatus1."]));
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
                //tableData.PumpStatus2 = new List<double>();
                //tableData.PumpStatus3 = new List<double>();
                //tableData.PumpStatus4 = new List<double>();
                //tableData.PumpStatus5 = new List<double>();
                //tableData.PumpStatus6 = new List<double>();
                //tableData.PumpStatus7 = new List<double>();
                //tableData.PumpStatus8 = new List<double>();
                //tableData.PumpStatus9 = new List<double>();
                //tableData.PumpStatus10 = new List<double>();
                tableData.PumpTimeArray = new List<string>();
                //tableData.Well2Level = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.LocationName = location;
                    tableData.PumpStatus1.Add(Convert.ToInt32(dr["PumpStatus1."]));
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

        ///// <spell for Pump 2>
        //public RecylcingPlantClass getAllSpellsForPump2(DataTable dt)
        //{
        //    var tableData = new DisposalDataClass();
        //    var spelldata = new DisposalPump2SpellData();
        //    string location = dt.Rows[0]["Location"].ToString();
        //    double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus2."])), 2);
        //    string currentTime = dt.Rows[0]["tim"].ToString();
        //    double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
        //    bool S = false;
        //    bool E = false;
        //    bool T = true;
        //    bool F = false;
        //    int spell = 0;
        //    List<DisposalPump2SpellData> spellDataList = new List<DisposalPump2SpellData>();
        //    string curtm = "";
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus2."])), 2);
        //        string currTime = dr["tim"].ToString();
        //        string clearaceTime = "";
        //        //start scenario 3 (inactive)
        //        if (DeltaMinutes > 28800)
        //        {

        //        }
        //        // end  scenario 3 (inactive)
        //        else
        //        {
        //            //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
        //            if (currentMotorStatus < 1)
        //            {
        //                if (E == F && S == F)
        //                {
        //                    if (currValue < 1)
        //                    {
        //                        if (spelldata.SpellDataArray.Count > 0)
        //                        {
        //                            string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
        //                            double lastvalue = spelldata.SpellDataArray.LastOrDefault();
        //                            E = T;
        //                            S = T;
        //                            spelldata.SpellDataArray.Add(lastvalue);
        //                            spelldata.SpellTimeArray.Add(lastTime);
        //                            spelldata.SpellEndTime = currTime;
        //                            clearaceTime = currTime;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        E = T;
        //                        spell = spell + 1;
        //                        spelldata.SpellNumber = spell;
        //                        spelldata.SpellTimeArray.Add(currTime);
        //                        spelldata.SpellEndTime = currTime;
        //                        clearaceTime = currTime;

        //                    }
        //                }
        //                else if (E == T && S == F)
        //                {
        //                    if (currValue < 1)
        //                    {
        //                        string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
        //                        if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
        //                        {
        //                            spelldata.SpellStartTime = lastTime;
        //                            S = T;
        //                        }
        //                        else
        //                        {

        //                            spelldata.SpellStartTime = currTime;
        //                            S = T;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
        //                        if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
        //                        {
        //                            spelldata.SpellStartTime = currTime;
        //                            S = T;
        //                        }
        //                        else
        //                        {
        //                            spelldata.SpellTimeArray.Add(currTime);
        //                        }
        //                    }
        //                }
        //                if (E == T && S == T)
        //                {
        //                    E = F;
        //                    S = F;
        //                    if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
        //                    {
        //                        spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
        //                        if (spelldata.spellPeriod == 0)
        //                        {
        //                            spelldata.spellPeriod = 1;
        //                        }
        //                        spellDataList.Add(spelldata);
        //                        spelldata = new DisposalPump2SpellData();
        //                        string s = JsonConvert.SerializeObject(spellDataList);
        //                    }
        //                }
        //            }
        //            // end  scenario 1 (No Ponding since many time/cleared/ zero received)
        //            //////////////////////////////////////////////////////////////////////
        //            //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
        //            else
        //            {
        //                if (E == F && S == F)
        //                {
        //                    if (currValue < 1)
        //                    {
        //                        if (spelldata.SpellDataArray.Count > 0)
        //                        {
        //                            string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
        //                            double lastvalue = spelldata.SpellDataArray.LastOrDefault();
        //                            E = T;
        //                            S = T;
        //                            spelldata.SpellDataArray.Add(lastvalue);
        //                            spelldata.SpellTimeArray.Add(lastTime);
        //                            spelldata.SpellEndTime = currTime;
        //                            clearaceTime = currTime;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        E = T;
        //                        spell = spell + 1;
        //                        spelldata.SpellNumber = spell;
        //                        spelldata.SpellTimeArray.Add(currTime);
        //                        spelldata.SpellEndTime = currTime;
        //                        clearaceTime = currTime;

        //                    }
        //                }
        //                else if (E == T && S == F)
        //                {
        //                    if (currValue < 1)
        //                    {
        //                        string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
        //                        if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
        //                        {
        //                            spelldata.SpellStartTime = lastTime;
        //                            S = T;
        //                        }
        //                        else
        //                        {

        //                            spelldata.SpellStartTime = currTime;
        //                            S = T;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
        //                        if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
        //                        {
        //                            spelldata.SpellStartTime = currTime;
        //                            S = T;
        //                        }
        //                        else
        //                        {
        //                            spelldata.SpellTimeArray.Add(currTime);
        //                        }
        //                    }
        //                }
        //                if (E == T && S == T)
        //                {
        //                    E = F;
        //                    S = F;
        //                    if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
        //                    {
        //                        //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
        //                        //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
        //                        //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
        //                        //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
        //                        //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
        //                        spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
        //                        if (spelldata.spellPeriod == 0)
        //                        {
        //                            spelldata.spellPeriod = 1;
        //                        }
        //                        //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
        //                        //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
        //                        spellDataList.Add(spelldata);
        //                        spelldata = new DisposalPump2SpellData();
        //                        string s = JsonConvert.SerializeObject(spellDataList);
        //                    }
        //                }
        //            }
        //            // end  scenario 2 (uncleared/ ponding continues)
        //        }
        //        curtm = currTime;
        //    }
        //    if (spellDataList.Count < 1)
        //    {
        //        if (spelldata.SpellDataArray.Count > 0)
        //        {
        //            spelldata.SpellStartTime = curtm;
        //            spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
        //            if (spelldata.spellPeriod == 0)
        //            {
        //                spelldata.spellPeriod = 1;
        //            }
        //            spellDataList.Add(spelldata);
        //        }
        //    }
        //    string c = JsonConvert.SerializeObject(spellDataList);
        //    if (spelldata.SpellDataArray.Count == 0)
        //    {
        //        spelldata.SpellDataArray.Add(currentMotorStatus);
        //        spelldata.SpellTimeArray.Add(currentTime);
        //        spelldata.SpellStartTime = currentTime;
        //        spelldata.SpellEndTime = currentTime;
        //    }
        //    if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
        //    {
        //        //tableData.PumpStatus1 = new List<double>();
        //        tableData.PumpStatus2 = new List<double>();
        //        //tableData.PumpStatus3 = new List<double>();
        //        //tableData.PumpStatus4 = new List<double>();
        //        //tableData.PumpStatus5 = new List<double>();
        //        //tableData.PumpStatus6 = new List<double>();
        //        //tableData.PumpStatus7 = new List<double>();
        //        //tableData.PumpStatus8 = new List<double>();
        //        //tableData.PumpStatus9 = new List<double>();
        //        //tableData.PumpStatus10 = new List<double>();
        //        //tableData.Well2Level = new List<double>();
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            tableData.locationName1 = location;
        //            tableData.PumpStatus2.Add(Convert.ToInt32(dr["PumpStatus2."]));
        //        }
        //        var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
        //        int phour = (int)pp.TotalHours;
        //        int pmin = (int)pp.Minutes;
        //        int psec = (int)pp.Seconds;
        //        string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
        //        tableData.WorkingInHoursPump2 = pstr;
        //        tableData.WorkingHoursPump2 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2);
                
        //    }
        //    else
        //    {
        //        //tableData.PumpStatus1 = new List<double>();
        //        tableData.PumpStatus2 = new List<double>();
        //        //tableData.Well2Level = new List<double>();
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            tableData.locationName1 = location;
        //            tableData.PumpStatus2.Add(Convert.ToInt32(dr["PumpStatus2."]));
        //        }
        //        var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
        //        int phour = (int)pp.TotalHours;
        //        int pmin = (int)pp.Minutes;
        //        int psec = (int)pp.Seconds;
        //        string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
        //        tableData.WorkingInHoursPump2 = pstr;
        //        tableData.WorkingHoursPump2 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2);
                
        //    }
        //    return tableData;
        //}
        ///// <spell for Pump 2>

        ///////////////////////////////Spell For Pump 3////////////////////////////////////////
        //public RecylcingPlantClass getAllSpellsForPump3(DataTable dt)
        //{
        //    var tableData = new DisposalDataClass();
        //    var spelldata = new DisposalPump3SpellData();
        //    string location = dt.Rows[0]["Location"].ToString();
        //    double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus3."])), 2);
        //    string currentTime = dt.Rows[0]["tim"].ToString();
        //    double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
        //    bool S = false;
        //    bool E = false;
        //    bool T = true;
        //    bool F = false;
        //    int spell = 0;
        //    List<DisposalPump3SpellData> spellDataList = new List<DisposalPump3SpellData>();
        //    string curtm = "";
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus3."])), 2);
        //        string currTime = dr["tim"].ToString();
        //        string clearaceTime = "";
        //        //start scenario 3 (inactive)
        //        if (DeltaMinutes > 28800)
        //        {

        //        }
        //        // end  scenario 3 (inactive)
        //        else
        //        {
        //            //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
        //            if (currentMotorStatus < 1)
        //            {
        //                if (E == F && S == F)
        //                {
        //                    if (currValue < 1)
        //                    {
        //                        if (spelldata.SpellDataArray.Count > 0)
        //                        {
        //                            string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
        //                            double lastvalue = spelldata.SpellDataArray.LastOrDefault();
        //                            E = T;
        //                            S = T;
        //                            spelldata.SpellDataArray.Add(lastvalue);
        //                            spelldata.SpellTimeArray.Add(lastTime);
        //                            spelldata.SpellEndTime = currTime;
        //                            clearaceTime = currTime;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        E = T;
        //                        spell = spell + 1;
        //                        spelldata.SpellNumber = spell;
        //                        spelldata.SpellTimeArray.Add(currTime);
        //                        spelldata.SpellEndTime = currTime;
        //                        clearaceTime = currTime;

        //                    }
        //                }
        //                else if (E == T && S == F)
        //                {
        //                    if (currValue < 1)
        //                    {
        //                        string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
        //                        if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
        //                        {
        //                            spelldata.SpellStartTime = lastTime;
        //                            S = T;
        //                        }
        //                        else
        //                        {

        //                            spelldata.SpellStartTime = currTime;
        //                            S = T;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
        //                        if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
        //                        {
        //                            spelldata.SpellStartTime = currTime;
        //                            S = T;
        //                        }
        //                        else
        //                        {
        //                            spelldata.SpellTimeArray.Add(currTime);
        //                        }
        //                    }
        //                }
        //                if (E == T && S == T)
        //                {
        //                    E = F;
        //                    S = F;
        //                    if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
        //                    {
        //                        spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
        //                        if (spelldata.spellPeriod == 0)
        //                        {
        //                            spelldata.spellPeriod = 1;
        //                        }
        //                        spellDataList.Add(spelldata);
        //                        spelldata = new DisposalPump3SpellData();
        //                        string s = JsonConvert.SerializeObject(spellDataList);
        //                    }
        //                }
        //            }
        //            // end  scenario 1 (No Ponding since many time/cleared/ zero received)
        //            //////////////////////////////////////////////////////////////////////
        //            //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
        //            else
        //            {
        //                if (E == F && S == F)
        //                {
        //                    if (currValue < 1)
        //                    {
        //                        if (spelldata.SpellDataArray.Count > 0)
        //                        {
        //                            string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
        //                            double lastvalue = spelldata.SpellDataArray.LastOrDefault();
        //                            E = T;
        //                            S = T;
        //                            spelldata.SpellDataArray.Add(lastvalue);
        //                            spelldata.SpellTimeArray.Add(lastTime);
        //                            spelldata.SpellEndTime = currTime;
        //                            clearaceTime = currTime;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        E = T;
        //                        spell = spell + 1;
        //                        spelldata.SpellNumber = spell;
        //                        spelldata.SpellTimeArray.Add(currTime);
        //                        spelldata.SpellEndTime = currTime;
        //                        clearaceTime = currTime;

        //                    }
        //                }
        //                else if (E == T && S == F)
        //                {
        //                    if (currValue < 1)
        //                    {
        //                        string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
        //                        if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
        //                        {
        //                            spelldata.SpellStartTime = lastTime;
        //                            S = T;
        //                        }
        //                        else
        //                        {

        //                            spelldata.SpellStartTime = currTime;
        //                            S = T;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
        //                        if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
        //                        {
        //                            spelldata.SpellStartTime = currTime;
        //                            S = T;
        //                        }
        //                        else
        //                        {
        //                            spelldata.SpellTimeArray.Add(currTime);
        //                        }
        //                    }
        //                }
        //                if (E == T && S == T)
        //                {
        //                    E = F;
        //                    S = F;
        //                    if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
        //                    {
        //                        //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
        //                        //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
        //                        //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
        //                        //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
        //                        //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
        //                        spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
        //                        if (spelldata.spellPeriod == 0)
        //                        {
        //                            spelldata.spellPeriod = 1;
        //                        }
        //                        //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
        //                        //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
        //                        spellDataList.Add(spelldata);
        //                        spelldata = new DisposalPump3SpellData();
        //                        string s = JsonConvert.SerializeObject(spellDataList);
        //                    }
        //                }
        //            }
        //            // end  scenario 2 (uncleared/ ponding continues)
        //        }
        //        curtm = currTime;
        //    }
        //    if (spellDataList.Count < 1)
        //    {
        //        if (spelldata.SpellDataArray.Count > 0)
        //        {
        //            spelldata.SpellStartTime = curtm;
        //            spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
        //            if (spelldata.spellPeriod == 0)
        //            {
        //                spelldata.spellPeriod = 1;
        //            }
        //            spellDataList.Add(spelldata);
        //        }
        //    }
        //    string c = JsonConvert.SerializeObject(spellDataList);
        //    if (spelldata.SpellDataArray.Count == 0)
        //    {
        //        spelldata.SpellDataArray.Add(currentMotorStatus);
        //        spelldata.SpellTimeArray.Add(currentTime);
        //        spelldata.SpellStartTime = currentTime;
        //        spelldata.SpellEndTime = currentTime;
        //    }
        //    if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
        //    {
        //        //tableData.PumpStatus1 = new List<double>();
        //        //tableData.PumpStatus2 = new List<double>();
        //        tableData.PumpStatus3 = new List<double>();
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            tableData.locationName1 = location;
        //            tableData.PumpStatus3.Add(Convert.ToInt32(dr["PumpStatus3."]));
        //        }
        //        var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
        //        int phour = (int)pp.TotalHours;
        //        int pmin = (int)pp.Minutes;
        //        int psec = (int)pp.Seconds;
        //        string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
        //        tableData.WorkingInHoursPump3 = pstr;
        //        tableData.WorkingHoursPump3 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2);
                
        //    }
        //    else
        //    {
        //        //tableData.PumpStatus1 = new List<double>();
        //        //tableData.PumpStatus2 = new List<double>();
        //        tableData.PumpStatus3 = new List<double>();
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            tableData.locationName1 = location;
        //            tableData.PumpStatus3.Add(Convert.ToInt32(dr["PumpStatus3."]));
        //        }
        //        var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
        //        int phour = (int)pp.TotalHours;
        //        int pmin = (int)pp.Minutes;
        //        int psec = (int)pp.Seconds;
        //        string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
        //        tableData.WorkingInHoursPump3 = pstr;
        //        tableData.WorkingHoursPump3 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2);
                
        //    }
        //    return tableData;
        //}
        ///////////////////////////////Spell For Pump 3////////////////////////////////////////



    }
}