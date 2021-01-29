using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WASA_EMS.Models;

namespace WASA_EMS.Controllers
{
    public class TubewellsControllers : Controller
    {
        // GET: Tubewells
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult HMI()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            //string newResr = Session["TheResource"].ToString();
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 64))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ResourceSellector rs = new ResourceSellector();
            if (Session["TheResource"] != null)
            {
                rs.resourceName = Session["TheResource"].ToString();
                ViewBag.SelectedResource = rs.resourceName;
            }
            else
            {
                rs.resourceName = "C-II Block Johar Town";
            }
            rs.resourceType = "Tubewells";
            ViewBag.ResourceList = ResourceList;
            return View(rs);
        }
        [HttpPost]
        public ActionResult HMI(FormCollection hmi)
        {
            string resource = hmi["resource"].ToString();
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 64))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            ResourceSellector rs = new ResourceSellector();
            rs.resourceType = "Tubewells";
            rs.resourceName = resource;
            ViewBag.SelectedResource = resource;
            return View(rs);
        }
        public ActionResult SetVariable(string key, string value)
        {
            Session[key] = value;

            return this.Json(new { success = true });
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        //[OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 2000)]
        public PartialViewResult _HMIView(string resourceName, string resourceType)
        {
            ResourceSellector rs = new ResourceSellector();
            rs.resourceType = resourceType;
            rs.resourceName = resourceName;
            return PartialView(rs);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        //[OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 2000)]
        public PartialViewResult _TableRecords(string resourceName, string resourceType)
        {
            string resource = resourceName;
            //string getResID = "select ResourceID from tblResource where ResourceLocation = 'C-II Johar Town' ";
            if (resource == null)
            {
                resource = "C-II Block Johar Town";
            }
            string getResID = "select ResourceID from tblResource where ResourceLocation = '"+resource+ "' and TemplateID = 64 ";
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
            //string query = ";with cteRowNumber as ( ";
            //query += " select r.ResourceID, r.ResourceLocation, CONCAT(r.ResourceLocation, ' : ',e.InsertionDateTime) AS Location,p.ParameterID,p.ParameterName, e.CompanyID, e.ParameterValue, ";
            //query += " row_number() over(partition by p.ParameterID, r.ResourceID,r.ResourceLocation order by e.ID desc) as RowNum";
            //query += " from tblEnergy e";
            //query += " inner join tblResource r on e.ResourceID = r.ResourceID";
            //query += " inner join tblParameter p on e.ParameterID = p.ParameterID";
            //query += " ) ";
            //query += " select * ";
            //query += " from cteRowNumber ";
            //query += " where RowNum = 1  and CompanyID = " + cId + "";
            //query += " and ResourceID = '" + ResID + "' ";
            //query += " order by ResourceID,ParameterID ";
            string query = "";
            query += "SELECT e.ID, r.ResourceLocation as Location, p.ParameterName, e.ParameterValue, e.InsertionDateTime ";
            query += "FROM tblEnergy e ";
            query += "inner join tblParameter p on e.ParameterID = p.ParameterID ";
            query += "inner join tblResource r on e.ResourceID = r.ResourceID ";
            query += "WHERE ";
            query += "e.ResourceID = " + ResID + " AND ";
            query += "insertionDateTime = (Select max(insertionDateTime) from tblEnergy where ResourceID = "+ResID+")";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                var sda = new SqlDataAdapter(query, conn);
                try
                {
                    sda.Fill(dt);
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            //string location = dt.Rows[0][1].ToString();
            //string time = dt.Rows[0][2].ToString().Split(':')[1] + ':' + dt.Rows[0][2].ToString().Split(':')[2];
            string location = dt.Rows[0][1].ToString();
            string time = dt.Rows[0][4].ToString();
            double delTime = (Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString()) - Convert.ToDateTime(time.ToString())).TotalMinutes;
            Pivot pvt = new Pivot(dt);
            DataTable pivotTable = new DataTable();
            pivotTable = pvt.PivotData("ParameterName", "ParameterValue", AggregateFunction.Sum, "Location");
            var db = new WASA_EMS_Entities();
            double V1N, V2N, V3N, I1, I2, I3, Frequency, PKVA, PF, AutoModeOn, PumpStatus, CurrentTrip, VoltageTrip, RemoteControl, ChlorineLevel, WaterExtracted, PKVAR, PKW, V12, V13, V23, PrimingLevel, averageVoltage, averageVoltageSecond, averageCurrent, TotalWorkingHour, Scheduling, Manual, Pressure, vib_m, vib_ms, vib_ms2 = 0;
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
            vib_m = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "vib_M" select row[1]).ElementAt(0)), 2);
            vib_ms = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "vib_Ms" select row[1]).ElementAt(0)), 2);
            vib_ms2 = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "vib_Ms2" select row[1]).ElementAt(0)), 2);
            Pressure = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "Pressure(Bar)" select row[1]).ElementAt(0)), 2);
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
            ViewBag.TotalWorkingHour = getTotalWorkingHoursToday(ResID);
            ViewBag.Scheduling = Scheduling;
            int num = random.Next(100);
            ViewBag.FlowData = num;
            ViewBag.Location = location;
            ViewBag.RunTime = time;
            ViewBag.Manual = Manual;
            ViewBag.vib_m = vib_m;
            ViewBag.vib_ms = vib_ms;
            ViewBag.vib_ms2 = vib_ms2;
            ViewBag.Pressure = Pressure;
            ViewBag.DelTime = delTime;
            //return PartialView(pivotTable);
            ////////////////////////////////////////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////////////////////////////////////////
            //int num = random.Next(100);
            //ViewBag.FlowData = num;
            //return PartialView();
            return PartialView(pivotTable);
        }
        public string getTotalWorkingHoursToday(int resourID)
        {
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var tubewellDataList = new List<TubewellDataClass>();
            int resourceID = resourID;
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    string resourceLocation = "";
                    int ite = 0;
                    //iterate through the list of resources within the desired set of resources chosen
                    //getting resourceID 
                    //getting resourceLocation 
                    //query will get the list of data available against given resourceID (latest first)
                    string Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.resourceName AS Location, ";
                    Dashdtquery += " r.ResourceSpecification AS specifications, ";
                    Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                    Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                    Dashdtquery += "s.InsertionDateTime as tim ,";
                    Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 9,GETDATE ())) as DeltaMinutes ";
                    Dashdtquery += "FROM tblEnergy s ";
                    Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                    Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                    Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                    Dashdtquery += "where ";
                    Dashdtquery += "r.ResourceID = " + resourceID + " and ";
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,9,GETDATE())), 0) ";
                    Dashdtquery += ") ";
                    Dashdtquery += "AS SourceTable ";
                    Dashdtquery += "PIVOT ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SUM(pVal) FOR pID ";
                    Dashdtquery += "IN ";
                    Dashdtquery += "( ";
                    Dashdtquery += "[V1N.],[V2N.],[V3N.],[I1.],[I2.],[I3.],[Frequency.],[PKVA.],[PF.],[Remote.],[PumpStatus],[CurrentTrip.],[VoltageTrip.],[TimeSchedule.],[ChlorineLevel.],[WaterFlow(Cusec).],[PKVAR.],[PKW.],[V12],[V13],[V23],[PrimingLevel],[Pressure(Bar)],[Manual],[vib_M],[vib_Ms],[vib_Ms2] ";
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
                        TubewellDataClass sd = getWorkingHours(Dashdt);
                        tubewellDataList.Add(sd);
                    }
                    else
                    {
                        TubewellDataClass sd = new TubewellDataClass();
                        sd.locationName = "";
                        sd.pumpStatus = new List<double>();
                        tubewellDataList.Add(sd);
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
                return tubewellDataList.FirstOrDefault().workingHoursToday;
            }
        }

        public TubewellDataClass getWorkingHours(DataTable dt)
        {
            var tableData = new TubewellDataClass();
            var spelldata = new TubewellSpellData();
            //int resourceID = Convert.ToInt32(dt.Rows[0]["resourceID"]);
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus"])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<TubewellSpellData> spellDataList = new List<TubewellSpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus"])), 2);
                //double currValueRemote = Math.Round((Convert.ToDouble(dr["Remote."])), 2);
                //double currValueManual = Math.Round((Convert.ToDouble(dr["Manual"])), 2);
                //double currValueScheduling = Math.Round((Convert.ToDouble(dr["TimeSchedule."])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["WaterFlow(Cusec)."])), 2);
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
                                spelldata.SpellDataArray.Add(FlowRate);
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
                                spelldata = new TubewellSpellData();
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
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new TubewellSpellData();
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
                tableData.pumpStatus = new List<double>();
                tableData.waterFlow = new List<double>();
                tableData.chlorineLevel = new List<double>();
                tableData.powerFactor = new List<double>();
                tableData.V1N = new List<double>();
                tableData.V2N = new List<double>();
                tableData.V3N = new List<double>();
                tableData.V12 = new List<double>();
                tableData.V13 = new List<double>();
                tableData.V23 = new List<double>();
                tableData.voltageTrip = new List<double>();
                tableData.I1 = new List<double>();
                tableData.I2 = new List<double>();
                tableData.I3 = new List<double>();
                tableData.currentTrip = new List<double>();
                tableData.frequency = new List<double>();
                tableData.pkva = new List<double>();
                tableData.pkvar = new List<double>();
                tableData.pkw = new List<double>();
                tableData.remoteControll = new List<double>();
                tableData.schedulingStatus = new List<double>();
                tableData.manualStatus = new List<double>();
                tableData.primingTankLevel = new List<double>();
                tableData.pressure = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName = location;
                    tableData.pumpStatus.Add(Convert.ToInt32(dr["PumpStatus"]));
                    tableData.waterFlow.Add(Convert.ToDouble(dr["WaterFlow(Cusec)."]));
                    tableData.chlorineLevel.Add(Convert.ToDouble(dr["ChlorineLevel."]));
                    tableData.powerFactor.Add(Convert.ToDouble(dr["PF."]));
                    tableData.V1N.Add(Convert.ToDouble(dr["V1N."]));
                    tableData.V2N.Add(Convert.ToDouble(dr["V2N."]));
                    tableData.V3N.Add(Convert.ToDouble(dr["V3N."]));
                    tableData.V12.Add(Convert.ToDouble(dr["V12"]));
                    tableData.V13.Add(Convert.ToDouble(dr["V13"]));
                    tableData.V23.Add(Convert.ToDouble(dr["V23"]));
                    tableData.voltageTrip.Add(Convert.ToDouble(dr["VoltageTrip."]));
                    tableData.I1.Add(Convert.ToDouble(dr["I1."]));
                    tableData.I2.Add(Convert.ToDouble(dr["I2."]));
                    tableData.I3.Add(Convert.ToDouble(dr["I3."]));
                    tableData.currentTrip.Add(Convert.ToDouble(dr["CurrentTrip."]));
                    tableData.frequency.Add(Convert.ToDouble(dr["Frequency."]));
                    tableData.pkva.Add(Convert.ToDouble(dr["PKVA."]));
                    tableData.pkvar.Add(Convert.ToDouble(dr["PKVAR."]));
                    tableData.pkw.Add(Convert.ToDouble(dr["PKW."]));
                    //tableData.autoMode.Add(Convert.ToDouble(dr["PKW."]));
                    tableData.remoteControll.Add(Convert.ToDouble(dr["Remote."]));
                    tableData.schedulingStatus.Add(Convert.ToDouble(dr["TimeSchedule."]));
                    tableData.manualStatus.Add(Convert.ToDouble(dr["Manual"]));
                    tableData.primingTankLevel.Add(Convert.ToDouble(dr["PrimingLevel"]));
                    tableData.pressure.Add(Convert.ToDouble(dr["Pressure(Bar)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.workingHoursToday = pstr;
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                if (spellDataList.Count == 0)
                {
                    tableData.accWaterDischargePerDay = "0";
                }
                else
                {
                    double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                    if (avgWaterFlow == 0)
                    {
                        avgWaterFlow = 1;
                    }
                    tableData.accWaterDischargePerDay = (((Convert.ToDouble(spellDataList.DefaultIfEmpty().Sum(i => i.spellPeriod)) / 60) * 102) * avgWaterFlow).ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                }
            }
            else
            {
                tableData.pumpStatus = new List<double>();
                tableData.pumpStatus = new List<double>();
                tableData.waterFlow = new List<double>();
                tableData.chlorineLevel = new List<double>();
                tableData.powerFactor = new List<double>();
                tableData.V1N = new List<double>();
                tableData.V2N = new List<double>();
                tableData.V3N = new List<double>();
                tableData.V12 = new List<double>();
                tableData.V13 = new List<double>();
                tableData.V23 = new List<double>();
                tableData.voltageTrip = new List<double>();
                tableData.I1 = new List<double>();
                tableData.I2 = new List<double>();
                tableData.I3 = new List<double>();
                tableData.currentTrip = new List<double>();
                tableData.frequency = new List<double>();
                tableData.pkva = new List<double>();
                tableData.pkvar = new List<double>();
                tableData.pkw = new List<double>();
                tableData.remoteControll = new List<double>();
                tableData.schedulingStatus = new List<double>();
                tableData.manualStatus = new List<double>();
                tableData.primingTankLevel = new List<double>();
                tableData.pressure = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName = location;
                    tableData.pumpStatus.Add(Convert.ToInt32(dr["PumpStatus"]));
                    tableData.waterFlow.Add(Convert.ToDouble(dr["WaterFlow(Cusec)."]));
                    tableData.chlorineLevel.Add(Convert.ToDouble(dr["ChlorineLevel."]));
                    tableData.powerFactor.Add(Convert.ToDouble(dr["PF."]));
                    tableData.V1N.Add(Convert.ToDouble(dr["V1N."]));
                    tableData.V2N.Add(Convert.ToDouble(dr["V2N."]));
                    tableData.V3N.Add(Convert.ToDouble(dr["V3N."]));
                    tableData.V12.Add(Convert.ToDouble(dr["V12"]));
                    tableData.V13.Add(Convert.ToDouble(dr["V13"]));
                    tableData.V23.Add(Convert.ToDouble(dr["V23"]));
                    tableData.voltageTrip.Add(Convert.ToDouble(dr["VoltageTrip."]));
                    tableData.I1.Add(Convert.ToDouble(dr["I1."]));
                    tableData.I2.Add(Convert.ToDouble(dr["I2."]));
                    tableData.I3.Add(Convert.ToDouble(dr["I3."]));
                    tableData.currentTrip.Add(Convert.ToDouble(dr["CurrentTrip."]));
                    tableData.frequency.Add(Convert.ToDouble(dr["Frequency."]));
                    tableData.pkva.Add(Convert.ToDouble(dr["PKVA."]));
                    tableData.pkvar.Add(Convert.ToDouble(dr["PKVAR."]));
                    tableData.pkw.Add(Convert.ToDouble(dr["PKW."]));
                    //tableData.autoMode.Add(Convert.ToDouble(dr["PKW."]));
                    tableData.remoteControll.Add(Convert.ToDouble(dr["Remote."]));
                    tableData.schedulingStatus.Add(Convert.ToDouble(dr["TimeSchedule."]));
                    tableData.manualStatus.Add(Convert.ToDouble(dr["Manual"]));
                    tableData.primingTankLevel.Add(Convert.ToDouble(dr["PrimingLevel"]));
                    tableData.pressure.Add(Convert.ToDouble(dr["Pressure(Bar)"]));
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.workingHoursToday = pstr;
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                tableData.accWaterDischargePerDay = (((Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)) / 60) * 102) * avgWaterFlow).ToString();
            }
            return tableData;
        }

        public string getHMIStatus(string resourceName)
        {
            var hmi = new HMIstatus();
            //double ret = -1;s
            string resource = resourceName;
            //string resourceType = ViewBag.resourceType.toString();
            //string getResID = "select ResourceID from tblResource where ResourceLocation = 'C-II Johar Town' ";
            //if (ViewBag.SelectedResource == null)
            //{
            //    resource = "C-II Johar Town";
            //}
            string query1 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '"+resource+"' ) and ParameterID = 125 order by ID desc";
            string query2 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 129 order by ID desc";
            string query3 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 167 order by ID desc";
            string query4 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 130 order by ID desc";
            string query5 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 174 order by ID desc";
            string query6 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 115 order by ID desc";
            string query7 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 116 order by ID desc";
            string query8 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 117 order by ID desc";
            string querytime = "select top(1) insertionDateTime from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 117 order by ID desc";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                var cmd1 = new SqlCommand(query1, conn);
                var cmd2 = new SqlCommand(query2, conn);
                var cmd3 = new SqlCommand(query3, conn);
                var cmd4 = new SqlCommand(query4, conn);
                var cmd5 = new SqlCommand(query5, conn);
                var cmd6 = new SqlCommand(query6, conn);
                var cmd7 = new SqlCommand(query7, conn);
                var cmd8 = new SqlCommand(query8, conn);
                var cmdtime = new SqlCommand(querytime, conn);
                try
                {
                    hmi.pumpStatus = Math.Abs(Convert.ToDouble(cmd1.ExecuteScalar()));
                    hmi.chlorineStatus = Math.Abs(Convert.ToDouble(cmd2.ExecuteScalar()));
                    hmi.primingStatus = Math.Abs(Convert.ToDouble(cmd3.ExecuteScalar()));
                    hmi.flowRate = Math.Round(Convert.ToDouble(cmd4.ExecuteScalar()), 1);
                    hmi.pressureRate = Math.Round(Convert.ToDouble(cmd5.ExecuteScalar()), 1);
                    hmi.v1n = Math.Round(Convert.ToDouble(cmd6.ExecuteScalar()), 1);
                    hmi.v2n = Math.Round(Convert.ToDouble(cmd7.ExecuteScalar()), 1);
                    hmi.v3n = Math.Round(Convert.ToDouble(cmd8.ExecuteScalar()), 1);
                    hmi.lastTime = cmdtime.ExecuteScalar().ToString();
                    hmi.delTime = (Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString()) - Convert.ToDateTime(hmi.lastTime.ToString())).TotalMinutes;
                    
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            return JsonConvert.SerializeObject(hmi); ;
        }
        public ActionResult ParameterWiseReport()
        {
            var tubewellDataList = new List<TubewellDataClass>();
            DataTable dtParams = new DataTable();
            string scriptString = "";
            scriptString = "var chart = new CanvasJS.Chart(\"chartContainer1\", { animationEnabled: true, title:{ text: \"Tubewell All Parameters\" }, axisY:{labelFontSize: 10},axisX:{labelFontSize: 10}, toolTip: {fontSize: 10, shared: true }, data: [";
            string getAllParametersQuery = "select p.ParameterID, p.ParameterName from tblParameter p inner join tblTemplateParameter tp on tp.ParameterID = p.ParameterID where tp.TemplateID = 64 order by p.paramOrder";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter sdaParams = new SqlDataAdapter(getAllParametersQuery, conn);
                    sdaParams.Fill(dtParams);
                    foreach (DataRow dr in dtParams.Rows)
                    {
                        DataTable dtResource = new DataTable();
                        scriptString += "{ type: \"stackedColumn\", name: \"" + dr["ParameterName"].ToString() + "\", showInLegend: false, dataPoints: [";
                        string getAllResourceQuery = "select ResourceID, ResourceLocation from tblResource where TemplateID = 64 order by ResourceID";
                        SqlDataAdapter sdaResource = new SqlDataAdapter(getAllResourceQuery, conn);
                        sdaResource.Fill(dtResource);
                        foreach (DataRow drRe in dtResource.Rows)
                        {
                            string getParameterValue = "select top(1)ParameterValue from tblEnergy where ResourceID = " + Convert.ToInt32(drRe["ResourceID"]) + " and ParameterID = " + Convert.ToInt32(dr["ParameterID"]) + " order by ID DESC";
                            SqlCommand cmdVal = new SqlCommand(getParameterValue, conn);
                            double val = Math.Abs(Convert.ToDouble(cmdVal.ExecuteScalar()));
                            scriptString += "{ y: " + val + " , label: \"" + drRe["ResourceLocation"].ToString() + "\" },";
                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString += "]},";
                    }
                    scriptString = scriptString.Remove(scriptString.Length - 1);
                    scriptString += "] })";
                }
                catch (Exception ex)
                {

                }
            }
            string NewscripString = scriptString;
            NewscripString = NewscripString.Replace("&quot;", "\"");
            ViewData["chartData"] = NewscripString;
            DataTable dtRes = new DataTable();
            int resourceID = 0;
            ////////////////////////////////////////////////////////////////////////
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string getResFromTemp = "";
                    getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    string resourceLocation = "";
                    string resourceSpecification = "";
                    int ite = 0;
                    //iterate through the list of resources within the desired set of resources chosen
                    DataTable Dashdt = new DataTable();
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        //getting resourceID 
                        resourceID = Convert.ToInt32(drRes["ResourceID"]);
                        //getting resourceLocation 
                        resourceLocation = drRes["ResourceLocation"].ToString();
                        resourceSpecification = drRes["ResourceSpecification"].ToString();
                        //query will get the list of data available against given resourceID (latest first)
                        string Dashdtquery = ";WITH cte AS ( ";
                        Dashdtquery += "SELECT* FROM ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SELECT DISTINCT r.resourceName AS Location, ";
                        Dashdtquery += " r.ResourceSpecification AS specifications, ";
                        Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                        Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                        Dashdtquery += "s.InsertionDateTime as tim ,";
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 9,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID = " + resourceID + " and ";
                        //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                        Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + DateTime.Now.Date + "', 103), 121)   ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        Dashdtquery += "[V1N.],[V2N.],[V3N.],[I1.],[I2.],[I3.],[Frequency.],[PKVA.],[PF.],[Remote.],[PumpStatus],[CurrentTrip.],[VoltageTrip.],[TimeSchedule.],[ChlorineLevel.],[WaterFlow(Cusec).],[PKVAR.],[PKW.],[V12],[V13],[V23],[PrimingLevel],[Pressure(Bar)],[Manual],[vib_M],[vib_Ms],[vib_Ms2] ";
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
                            TubewellDataClass sd = getAllSpells(Dashdt, dtRes.Rows.IndexOf(drRes));
                            tubewellDataList.Add(sd);
                        }
                        else
                        {
                            TubewellDataClass sd = new TubewellDataClass();
                            sd.locationName = drRes["ResourceLocation"].ToString();
                            sd.Specification = drRes["ResourceSpecification"].ToString();
                            sd.pumpStatus = new List<double>();
                            tubewellDataList.Add(sd);
                        }
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
            string selectedResource = "";
            //if (resources == "All")
            //{
            //    selectedResource = "All Tubewell Locations";
            //}
            //else
            //{
            //    selectedResource = "" + resources + " Tubewell";
            //}
            //Session["ReportTitle"] = "Energy Monitoring Report of " + selectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
            return PartialView(tubewellDataList);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 10)]
        public PartialViewResult _ParameterReportView()
        {
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var tubewellDataList = new List<TubewellDataClass>();
            int resourceID = 0;

            ////////////////////////////////////////////////////////////////////////
            DataTable dtParams = new DataTable();
            string scriptString = "";
            scriptString = "var chart = new CanvasJS.Chart(\"chartContainer1\", { animationEnabled: true, title:{ text: \"Tubewell All Parameters\" }, axisY:{ }, toolTip: { shared: true, reversed: true }, data: [";
            string getAllParametersQuery = "select p.ParameterID, p.ParameterName from tblParameter p inner join tblTemplateParameter tp on tp.ParameterID = p.ParameterID where tp.TemplateID = 64 order by p.paramOrder";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter sdaParams = new SqlDataAdapter(getAllParametersQuery, conn);
                    sdaParams.Fill(dtParams);
                    foreach (DataRow dr in dtParams.Rows)
                    {
                        DataTable dtResource = new DataTable();
                        scriptString += "{ type: \"stackedColumn\", name: \"" + dr["ParameterName"].ToString() + "\", showInLegend: \"true\", dataPoints: [";
                        string getAllResourceQuery = "select ResourceID, ResourceLocation from tblResource where TemplateID = 64 order by ResourceID";
                        SqlDataAdapter sdaResource = new SqlDataAdapter(getAllResourceQuery, conn);
                        sdaResource.Fill(dtResource);
                        foreach (DataRow drRe in dtResource.Rows)
                        {
                            string getParameterValue = "select top(1)ParameterValue from tblEnergy where ResourceID = "+Convert.ToInt32(drRe["ResourceID"]) +" and ParameterID = "+ Convert.ToInt32(dr["ParameterID"]) + " order by ID DESC";
                            SqlCommand cmdVal = new SqlCommand(getParameterValue, conn);
                            double val = Math.Abs(Convert.ToDouble(cmdVal.ExecuteScalar()));
                            scriptString += "{ y: " + val + " , label: \"'" + drRe["ResourceLocation"].ToString() + "'\" },";
                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString += "]},";
                    }
                    scriptString = scriptString.Remove(scriptString.Length - 1);
                    scriptString += "] }";
                }
                catch (Exception ex)
                {

                }
            }
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;

            ////////////////////////////////////////////////////////////////////////
            
            return PartialView(tubewellDataList);
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
            var tubewellDataList = new List<TubewellDataClass>();
            int resourceID = 0;

            ////////////////////////////////////////////////////////////////////////

            string scriptString = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'PumpStatus'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'  and rtp.TemplateID = 64 order by cast(r.ResourceID as int) asc";
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"Tubewells Working Status\" },";
                        string TheSelectedResource = "All Tubewells";
                        //Session["ReportTitle"] = "Data Fetched for " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
                        scriptString += "subtitles: [{text: \" Data Fetched from " + TheSelectedResource + " Today   \" }],";
                        scriptString += "axisY: {suffix: \" \" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
                        scriptString += " data: [";
                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            //string parName = drPar["parameterName"].ToString();
                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,9,GETDATE())), 0)  ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime ASC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"hh:mm TT DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
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
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;

            ////////////////////////////////////////////////////////////////////////


            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    string resourceLocation = "";
                    int ite = 0;
                    //iterate through the list of resources within the desired set of resources chosen
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        //getting resourceID 
                        resourceID = Convert.ToInt32(drRes["ResourceID"]);
                        //getting resourceLocation 
                        resourceLocation = drRes["ResourceLocation"].ToString();
                        //query will get the list of data available against given resourceID (latest first)
                        string Dashdtquery = ";WITH cte AS ( ";
                        Dashdtquery += "SELECT* FROM ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SELECT DISTINCT r.resourceName AS Location, ";
                        Dashdtquery += " r.ResourceSpecification AS specifications, ";
                        Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                        Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                        Dashdtquery += "s.InsertionDateTime as tim ,";
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 9,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID = " + resourceID + " and ";
                        Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,9,GETDATE())), 0) ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        Dashdtquery += "[V1N.],[V2N.],[V3N.],[I1.],[I2.],[I3.],[Frequency.],[PKVA.],[PF.],[Remote.],[PumpStatus],[CurrentTrip.],[VoltageTrip.],[TimeSchedule.],[ChlorineLevel.],[WaterFlow(Cusec).],[PKVAR.],[PKW.],[V12],[V13],[V23],[PrimingLevel],[Pressure(Bar)],[Manual],[vib_M],[vib_Ms],[vib_Ms2] ";
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
                            TubewellDataClass sd = getAllSpells(Dashdt, dtRes.Rows.IndexOf(drRes));
                            tubewellDataList.Add(sd);
                        }
                        else
                        {
                            TubewellDataClass sd = new TubewellDataClass();
                            sd.locationName = drRes["ResourceLocation"].ToString();
                            sd.pumpStatus = new List<double>();
                            tubewellDataList.Add(sd);
                        }
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
            Session["ReportTitle"] = "Current Status of All Tubewell Locations at  " + DateTime.Now.AddHours(9).ToString("dd'/'MM'/'yyyy HH:mm:ss") + " (Reflected for Today)";
            return PartialView(tubewellDataList);
        }
        public ActionResult WaterDischargeReport()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("All");
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 64))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Tubewells";
            rs.resourceName = "All";
            rs.dateFrom = "";
            rs.timeFrom = "";
            rs.dateTo = "";
            rs.timeTo = "";
            return View(rs);
        }
        [HttpPost]
        public ActionResult WaterDischargeReport(FormCollection review)
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
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 64))
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
        public PartialViewResult _WaterDischargeReportView(string resources, string datFrom, string timFrom, string datTo, string timTo)
        {
            DateTime FinalTimeFrom = DateTime.Now;
            DateTime FinalTimeTo = DateTime.Now;
            if (datFrom == "" && timFrom == "" && datTo == "" && timTo == "")
            {
                FinalTimeFrom = DateTime.Now.AddHours(9).Date;
                FinalTimeTo = DateTime.Now.AddHours(9).AddDays(1).Date;
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
            var tubewellDataList = new List<TubewellDataClass>();
            int resourceID = 0;

            ////////////////////////////////////////////////////////////////////////

            string scriptString = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'WaterFlow(Cusec).'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        if (resources.ToLower() == "all")
                        {
                            getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'WaterFlow(Cusec).'  and rtp.TemplateID = 64 order by cast(r.ResourceID as int) asc";
                        }
                        else
                        {
                            getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'WaterFlow(Cusec).'  and rtp.TemplateID = 64 and r.ResourceName = '" + resources + "' order by cast(r.ResourceID as int) asc";
                        }
                        
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"Water Flow (m3/h)\" },";
                        string TheSelectedResource = "All Tubewells";
                        if (resources == "All")
                        {
                            TheSelectedResource = "All Tubewells ";
                        }
                        else
                        {
                            TheSelectedResource = "" + resources + " Tubewell";
                        }
                        //Session["ReportTitle"] = "Data Fetched for " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
                        scriptString += "subtitles: [{text: \" Data Fetched from " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "  \" }],";
                        scriptString += "axisY: {suffix: \" m3/h\" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
                        scriptString += " data: [";
                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            //string parName = drPar["parameterName"].ToString();
                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime ASC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"hh:mm TT DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} m3/h</strong> at {x}\", ";
                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
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
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;

            ////////////////////////////////////////////////////////////////////////


            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string getResFromTemp = "";
                    if (resources.ToLower() == "all")
                    {
                        getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells'";
                    }
                    else
                    {
                        getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells' and r.ResourceName = '" + resources + "'";
                    }
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    string resourceLocation = "";
                    int ite = 0;
                    //iterate through the list of resources within the desired set of resources chosen
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        //getting resourceID 
                        resourceID = Convert.ToInt32(drRes["ResourceID"]);
                        //getting resourceLocation 
                        resourceLocation = drRes["ResourceLocation"].ToString();
                        //query will get the list of data available against given resourceID (latest first)
                        string Dashdtquery = ";WITH cte AS ( ";
                        Dashdtquery += "SELECT* FROM ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SELECT DISTINCT r.resourceName AS Location, ";
                        Dashdtquery += " r.ResourceSpecification AS specifications, ";
                        Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                        Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                        Dashdtquery += "s.InsertionDateTime as tim ,";
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 9,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID = " + resourceID + " and ";
                        //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                        Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        Dashdtquery += "[V1N.],[V2N.],[V3N.],[I1.],[I2.],[I3.],[Frequency.],[PKVA.],[PF.],[Remote.],[PumpStatus],[CurrentTrip.],[VoltageTrip.],[TimeSchedule.],[ChlorineLevel.],[WaterFlow(Cusec).],[PKVAR.],[PKW.],[V12],[V13],[V23],[PrimingLevel],[Pressure(Bar)],[Manual],[vib_M],[vib_Ms],[vib_Ms2] ";
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
                            TubewellDataClass sd = getAllSpells(Dashdt, dtRes.Rows.IndexOf(drRes));
                            tubewellDataList.Add(sd);
                        }
                        else
                        {
                            TubewellDataClass sd = new TubewellDataClass();
                            sd.locationName = drRes["ResourceLocation"].ToString();
                            sd.pumpStatus = new List<double>();
                            tubewellDataList.Add(sd);
                        }
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
            string selectedResource = "";
            if (resources == "All")
            {
                selectedResource = "All Tubewells";
            }
            else
            {
                selectedResource = "" + resources + " Tubewell";
            }
            Session["ReportTitle"] = "Water Discharge Report of " + selectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
            return PartialView(tubewellDataList);
        }
        public ActionResult VibrationAnalysisReport()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("All");
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 64))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Tubewells";
            rs.resourceName = "All";
            rs.dateFrom = "";
            rs.timeFrom = "";
            rs.dateTo = "";
            rs.timeTo = "";
            return View(rs);
        }
        [HttpPost]
        public ActionResult VibrationAnalysisReport(FormCollection review)
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
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 64))
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
        public PartialViewResult _VibrationAnalysisReportView(string resources, string datFrom, string timFrom, string datTo, string timTo)
        {
            DateTime FinalTimeFrom = DateTime.Now;
            DateTime FinalTimeTo = DateTime.Now;
            if (datFrom == "" && timFrom == "" && datTo == "" && timTo == "")
            {
                FinalTimeFrom = DateTime.Now.AddHours(9).Date;
                FinalTimeTo = DateTime.Now.AddHours(9).AddDays(1).Date;
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
            var tubewellDataList = new List<TubewellDataClass>();
            int resourceID = 0;

            ////////////////////////////////////////////////////////////////////////

            string scriptString = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'vib_Ms'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        if (resources.ToLower() == "all")
                        {
                            getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'vib_Ms'  and rtp.TemplateID = 64 order by cast(r.ResourceID as int) asc";
                        }
                        else
                        {
                            getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'vib_Ms'  and rtp.TemplateID = 64 and r.ResourceName = '" + resources + "' order by cast(r.ResourceID as int) asc";
                        }

                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"Vibration Velocity (m/s)\" },";
                        string TheSelectedResource = "All Tubewells";
                        if (resources == "All")
                        {
                            TheSelectedResource = "All Tubewells ";
                        }
                        else
                        {
                            TheSelectedResource = "" + resources + " Tubewell";
                        }
                        //Session["ReportTitle"] = "Data Fetched for " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
                        scriptString += "subtitles: [{text: \" Data Fetched from " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "  \" }],";
                        scriptString += "axisY:{	stripLines:[ { startValue:-0.05, endValue:0.05, color:'#1f651e' } , { startValue:6.95, endValue:7.05, color:'#ff0000' }], interval: 2, minimum: -2},";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
                        scriptString += " data: [";
                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            //string parName = drPar["parameterName"].ToString();
                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime ASC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            scriptString += "{ type: \"line\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"hh:mm TT DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} m/s</strong> at {x}\", ";
                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
                                // if (dtVal.Rows.IndexOf(drVal) != 0)
                                // {
                                //  dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(dt).AddHours(-5).AddMinutes(1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5).AddMinutes(-1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                // dt = Convert.ToDateTime(drVal["InsertionDateTime"]);
                                // }
                                //else
                                //{
                                dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Math.Abs(Convert.ToDouble(drVal["ParameterValue"]))));
                                dt = Convert.ToDateTime(drVal["InsertionDateTime"]);
                                //}
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
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;

            ////////////////////////////////////////////////////////////////////////


            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string getResFromTemp = "";
                    if (resources.ToLower() == "all")
                    {
                        getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells'";
                    }
                    else
                    {
                        getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells' and r.ResourceName = '" + resources + "'";
                    }
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    string resourceLocation = "";
                    int ite = 0;
                    //iterate through the list of resources within the desired set of resources chosen
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        //getting resourceID 
                        resourceID = Convert.ToInt32(drRes["ResourceID"]);
                        //getting resourceLocation 
                        resourceLocation = drRes["ResourceLocation"].ToString();
                        //query will get the list of data available against given resourceID (latest first)
                        string Dashdtquery = ";WITH cte AS ( ";
                        Dashdtquery += "SELECT* FROM ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SELECT DISTINCT r.resourceName AS Location, ";
                        Dashdtquery += " r.ResourceSpecification AS specifications, ";
                        Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                        Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                        Dashdtquery += "s.InsertionDateTime as tim ,";
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 9,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID = " + resourceID + " and ";
                        //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                        Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        Dashdtquery += "[V1N.],[V2N.],[V3N.],[I1.],[I2.],[I3.],[Frequency.],[PKVA.],[PF.],[Remote.],[PumpStatus],[CurrentTrip.],[VoltageTrip.],[TimeSchedule.],[ChlorineLevel.],[WaterFlow(Cusec).],[PKVAR.],[PKW.],[V12],[V13],[V23],[PrimingLevel],[Pressure(Bar)],[Manual],[vib_M],[vib_Ms],[vib_Ms2] ";
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
                            TubewellDataClass sd = getAllSpells(Dashdt, dtRes.Rows.IndexOf(drRes));
                            tubewellDataList.Add(sd);
                        }
                        else
                        {
                            TubewellDataClass sd = new TubewellDataClass();
                            sd.locationName = drRes["ResourceLocation"].ToString();
                            sd.pumpStatus = new List<double>();
                            tubewellDataList.Add(sd);
                        }
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
            string selectedResource = "";
            if (resources == "All")
            {
                selectedResource = "All Tubewells";
            }
            else
            {
                selectedResource = "" + resources + " Tubewell";
            }
            Session["ReportTitle"] = "Vibration Report of " + selectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
            return PartialView(tubewellDataList);
        }
        public ActionResult EnergyMonitoringReport()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            //ResourceList.Add("All");
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 64))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Tubewells";
            //rs.resourceName = "All";
            rs.dateFrom = "";
            rs.timeFrom = "";
            rs.dateTo = "";
            rs.timeTo = "";
            //string scriptString = "";
            //using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            //{
            //    try
            //    {
            //        string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'PumpStatus'";
            //        SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
            //        DataTable dtRes = new DataTable();
            //        sdaRes.Fill(dtRes);
            //        int ite = 0;
            //        foreach (DataRow drRes in dtRes.Rows)
            //        {
            //            //string resName = drRes["resourceLocationName"].ToString();
            //            ite += 1;
            //            string getParamsFromRes = "";
            //            getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'  and rtp.TemplateID = 64 order by cast(r.ResourceID as int) asc";
            //            SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
            //            DataTable dtPar = new DataTable();
            //            sdaPar.Fill(dtPar);
            //            scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
            //            scriptString += "theme: \"light2\",";
            //            scriptString += "animationEnabled: true,";
            //            scriptString += "zoomEnabled: true, ";
            //            scriptString += "title: {text: \"PumpStatus\" },";
            //            scriptString += "subtitles: [{text: \" All Tubewells Recent Working  \" }],";
            //            scriptString += "axisY: {suffix: \" \" },";
            //            //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
            //            scriptString += "toolTip: { shared: false },";
            //            scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries},";
            //            scriptString += " data: [";
            //            foreach (DataRow drPar in dtPar.Rows)
            //            {
            //                //string parName = drPar["parameterName"].ToString();
            //                string aquery = ";WITH CTE AS ( ";
            //                aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
            //                aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
            //                aquery += "ORDER BY e.InsertionDateTime DESC) ";
            //                aquery += "FROM tblEnergy e ";
            //                aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
            //                aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= ' 2020-03-19 ' and e.InsertionDateTime < ' 2020-03-20 '  ";
            //                aquery += ") ";
            //                aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime ASC";
            //                string theQuery = aquery;
            //                SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
            //                DataTable dtVal = new DataTable();
            //                sdaVal.Fill(dtVal);
            //                scriptString += "{ type: \"line\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"HH:mm:ss DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
            //                List<DataPoint> dataPoints = new List<DataPoint>();
            //                DateTime dt = DateTime.Now;
            //                foreach (DataRow drVal in dtVal.Rows)
            //                {
            //                   // if (dtVal.Rows.IndexOf(drVal) != 0)
            //                   // {
            //                       //  dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(dt).AddHours(-5).AddMinutes(1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
            //                       // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5).AddMinutes(-1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
            //                       // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
            //                       // dt = Convert.ToDateTime(drVal["InsertionDateTime"]);
            //                   // }
            //                    //else
            //                    //{
            //                        dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
            //                        dt = Convert.ToDateTime(drVal["InsertionDateTime"]);
            //                    //}
            //                    //dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
            //                }
            //                scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints) + "";
            //                scriptString += "},";
            //            }
            //            scriptString = scriptString.Remove(scriptString.Length - 1);
            //            scriptString = scriptString + "]";
            //            scriptString = scriptString + "}";
            //            scriptString += ");";
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            //string NewscripString = scriptString;
            //ViewData["chartData"] = NewscripString;
            return View(rs);
        }
        [HttpPost]
        public ActionResult EnergyMonitoringReport(FormCollection review)
        {
            string resource = "";
            if (review["resource"] == null)
            {
                resource = "All";
            }
            else
            {
                resource = review["resource"].ToString();
                string[] resourcesArray = resource.Split(',');
                string newOne = "";
                for (int i = 0; i < resourcesArray.Count(); i++)
                {
                    newOne += "'";
                    newOne += resourcesArray[i];
                    newOne += "',";
                }
                newOne = newOne.Remove(newOne.Length - 1, 1);
                resource = newOne;
            }
            
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
            //ResourceList.Add("All");
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 64))
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
        [ValidateInput(false)]
        public PartialViewResult _EnergyMonitoringReportView(string resources, string datFrom, string timFrom, string datTo, string timTo)
        {
            resources = System.Web.HttpUtility.HtmlDecode(resources);
            DateTime FinalTimeFrom = DateTime.Now;
            if (resources == "")
            {
                resources = "All";
            }
            DateTime FinalTimeTo = DateTime.Now;
            if (datFrom == "" && timFrom == "" && datTo == "" && timTo == "")
            {
                CultureInfo culture = new CultureInfo("en-US");
                FinalTimeFrom = DateTime.Now.AddHours(9).Date;
                FinalTimeTo = DateTime.Now.AddHours(9).AddDays(1).Date;
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
            var tubewellDataList = new List<TubewellDataClass>();
            int resourceID = 0;

            ////////////////////////////////////////////////////////////////////////

            string scriptString = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'PumpStatus'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        if (resources.ToLower() == "all")
                        {
                            getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'  and rtp.TemplateID = 64 order by cast(r.ResourceID as int) asc";
                        }
                        else
                        {
                            getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'  and rtp.TemplateID = 64 and r.ResourceName in ("+resources+")  order by cast(r.ResourceID as int) asc";
                        }
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"Tubewells Working Status\" },";
                        string TheSelectedResource = "";
                        if (resources == "All")
                        {
                            TheSelectedResource = "All Tubewells";  
                        }
                        else
                        {
                            TheSelectedResource = "" + resources + " Tubewell";
                        }
                        //Session["ReportTitle"] = "Data Fetched for " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
                        scriptString += "subtitles: [{text: \" Data Fetched from " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "  \" }],";
                        scriptString += "axisY: {suffix: \" \" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false ,contentFormatter: function(e){ var str = \" \" ; for (var i = 0; i < e.entries.length; i++){ if(e.entries[i].dataPoint.y == 0){ var temp = e.entries[i].dataSeries.name + \" \" +\": OFF\"; str = str+temp; } else { var temp = e.entries[i].dataSeries.name + \" \" +\": ON\"; str = str+temp; } } return (str); }},";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
                        scriptString += " data: [";
                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            //string parName = drPar["parameterName"].ToString();
                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime ASC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"hh:mm TT DD-MM-YYYY\",  ";
                            //scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"hh:mm TT DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
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
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;

            ////////////////////////////////////////////////////////////////////////
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string getResFromTemp = "";
                    if (resources.ToLower() == "all")
                    {
                        getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells'";
                    }
                    else
                    {
                        getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells' and r.ResourceName in (" + resources + ")";
                    }
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    string resourceLocation = "";
                    string resourceSpecification = "";
                    int ite = 0;
                    //iterate through the list of resources within the desired set of resources chosen
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        //getting resourceID 
                        resourceID = Convert.ToInt32(drRes["ResourceID"]);
                        //getting resourceLocation 
                        resourceLocation = drRes["ResourceLocation"].ToString();
                        resourceSpecification = drRes["ResourceSpecification"].ToString();
                        //query will get the list of data available against given resourceID (latest first)
                        string Dashdtquery = ";WITH cte AS ( ";
                        Dashdtquery += "SELECT* FROM ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SELECT DISTINCT r.resourceName AS Location, ";
                        Dashdtquery += " r.ResourceSpecification AS specifications, ";
                        Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                        Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                        Dashdtquery += "s.InsertionDateTime as tim ,";
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 9,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID = " + resourceID + " and ";
                        //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                        Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        Dashdtquery += "[V1N.],[V2N.],[V3N.],[I1.],[I2.],[I3.],[Frequency.],[PKVA.],[PF.],[Remote.],[PumpStatus],[CurrentTrip.],[VoltageTrip.],[TimeSchedule.],[ChlorineLevel.],[WaterFlow(Cusec).],[PKVAR.],[PKW.],[V12],[V13],[V23],[PrimingLevel],[Pressure(Bar)],[Manual],[vib_M],[vib_Ms],[vib_Ms2] ";
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
                            TubewellDataClass sd = getAllSpells(Dashdt, dtRes.Rows.IndexOf(drRes));
                            tubewellDataList.Add(sd);
                        }
                        else
                        {
                            TubewellDataClass sd = new TubewellDataClass();
                            sd.locationName = drRes["ResourceLocation"].ToString();
                            sd.Specification = drRes["ResourceSpecification"].ToString();
                            sd.pumpStatus = new List<double>();
                            tubewellDataList.Add(sd);
                        }
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
            string selectedResource = "";
            if (resources == "All")
            {
                selectedResource = "All Tubewell Locations";
            }
            else
            {
                selectedResource = "" + resources + " Tubewell";
            }
            Session["ReportTitle"] = "Energy Monitoring Report of " + selectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo +"";
            return PartialView(tubewellDataList);
        }
        public ActionResult RemoteStatusReport()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("All");
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 64))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Tubewells";
            rs.resourceName = "All";
            rs.dateFrom = "";
            rs.timeFrom = "";
            rs.dateTo = "";
            rs.timeTo = "";
            return View(rs);
        }
        [HttpPost]
        public ActionResult RemoteStatusReport(FormCollection review)
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
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 64))
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
        public PartialViewResult _RemoteStatusReportView(string resources, string datFrom, string timFrom, string datTo, string timTo)
        {
            DateTime FinalTimeFrom = DateTime.Now;
            DateTime FinalTimeTo = DateTime.Now;
            if (datFrom == "" && timFrom == "" && datTo == "" && timTo == "")
            {
                FinalTimeFrom = DateTime.Now.AddHours(9).Date;
                FinalTimeTo = DateTime.Now.AddHours(9).AddDays(1).Date;
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
            var tubewellDataList = new List<TubewellDataClass>();
            int resourceID = 0;

            ////////////////////////////////////////////////////////////////////////

            string scriptString = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'PumpStatus'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        if (resources.ToLower() == "all")
                        {
                            getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'  and rtp.TemplateID = 64 order by cast(r.ResourceID as int) asc";
                        }
                        else
                        {
                            getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'  and rtp.TemplateID = 64 and r.ResourceName = '" + resources + "'  order by cast(r.ResourceID as int) asc";
                        }
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"Mode Status\" },";
                        string TheSelectedResource = "";
                        if (resources == "All")
                        {
                            TheSelectedResource = "All Tubewells";
                        }
                        else
                        {
                            TheSelectedResource = "" + resources + " Tubewell";
                        }
                        //Session["ReportTitle"] = "Data Fetched for " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
                        scriptString += "subtitles: [{text: \" Data Fetched from " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "  \" }],";
                        scriptString += "axisY: {suffix: \" \" },";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 15},";
                        scriptString += " data: [";
                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            //string parName = drPar["parameterName"].ToString();
                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            aquery += ") ";
                            aquery += "SELECT top 1400 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime ASC";
                            string theQuery = aquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"hh:mm TT DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
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
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;

            ////////////////////////////////////////////////////////////////////////


            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string getResFromTemp = "";
                    if (resources == "All")
                    {
                        getResFromTemp += "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells'";
                    }
                    else
                    {
                        getResFromTemp += "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells' and r.ResourceLocation = '" + resources+"' ";
                    }
                    
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    string resourceLocation = "";
                    int ite = 0;
                    //iterate through the list of resources within the desired set of resources chosen
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        //getting resourceID 
                        resourceID = Convert.ToInt32(drRes["ResourceID"]);
                        //getting resourceLocation 
                        resourceLocation = drRes["ResourceLocation"].ToString();
                        //query will get the list of data available against given resourceID (latest first)
                        string Dashdtquery = ";WITH cte AS ( ";
                        Dashdtquery += "SELECT* FROM ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SELECT DISTINCT r.resourceName AS Location, ";
                        Dashdtquery += " r.ResourceSpecification AS specifications, ";
                        Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                        Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                        Dashdtquery += "s.InsertionDateTime as tim ,";
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 9,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID = " + resourceID + " and ";
                        //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                        Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        Dashdtquery += "[V1N.],[V2N.],[V3N.],[I1.],[I2.],[I3.],[Frequency.],[PKVA.],[PF.],[Remote.],[PumpStatus],[CurrentTrip.],[VoltageTrip.],[TimeSchedule.],[ChlorineLevel.],[WaterFlow(Cusec).],[PKVAR.],[PKW.],[V12],[V13],[V23],[PrimingLevel],[Pressure(Bar)],[Manual],[vib_M],[vib_Ms],[vib_Ms2] ";
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
                            TubewellDataClass sd = getAllSpellsForRemoteStatus(Dashdt, dtRes.Rows.IndexOf(drRes));
                            tubewellDataList.Add(sd);
                        }
                        else
                        {
                            TubewellDataClass sd = new TubewellDataClass();
                            sd.locationName = drRes["ResourceLocation"].ToString();
                            sd.pumpStatus = new List<double>();
                            //sd.I1 = null;
                            //sd.I2 = null;
                            //sd.I3 = null;
                            //sd.manualStatus = null;
                            //sd.pkva = null;
                            //sd.pkvar = null;
                            //sd.pkw = null;
                            //sd.powerFactor = null;
                            //sd.pressure = null;
                            //sd.primingTankLevel = null;
                            //sd.remoteControll = null;
                            //sd.schedulingStatus = null;
                            //sd.V12 = null;
                            //sd.V13 = null;
                            //sd.V1N = null;
                            //sd.V23 = null;
                            //sd.V2N = null;
                            //sd.V3N = null;
                            //sd.voltageTrip = null;
                            //sd.waterDischarge = null;
                            //sd.waterFlow = null;
                            //sd.workingHoursToday = null;
                            sd.workingHoursTodayManual = "-";
                            sd.workingHoursTodayRemote = "-";
                            sd.workingHoursTodayScheduling = "-";
                            tubewellDataList.Add(sd);
                        }
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
            string selectedResource = "";
            if (resources == "All")
            {
                selectedResource = "All Tubewell Locations";
            }
            else
            {
                selectedResource = "" + resources + " Tubewell";
            }
            Session["ReportTitle"] = "Mode Status Report of " + selectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
            return PartialView(tubewellDataList);
        }
        public void report()
        {

        }
        public TubewellDataClass getAllSpells(DataTable dt, int order)
        {
            var tableData = new TubewellDataClass();
            var spelldata = new TubewellSpellData();
            //int resourceID = Convert.ToInt32(dt.Rows[0]["resourceID"]);
            string location = dt.Rows[0]["Location"].ToString();
            string specs = dt.Rows[0]["specifications"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus"])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<TubewellSpellData> spellDataList = new List<TubewellSpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus"])), 2);
                //double currValueRemote = Math.Round((Convert.ToDouble(dr["Remote."])), 2);
                //double currValueManual = Math.Round((Convert.ToDouble(dr["Manual"])), 2);
                //double currValueScheduling = Math.Round((Convert.ToDouble(dr["TimeSchedule."])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["WaterFlow(Cusec)."])), 2);
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
                                spelldata.SpellDataArray.Add(FlowRate);
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
                                    spelldata.spellDate = Convert.ToDateTime(lastTime).Date.ToString();
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.spellDate = Convert.ToDateTime(currTime).Date.ToString();
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.spellDate = Convert.ToDateTime(currTime).Date.ToString();
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
                                spelldata = new TubewellSpellData();
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
                            if (currValue < 1)
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.spellDate = Convert.ToDateTime(lastTime).Date.ToString();
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.spellDate = Convert.ToDateTime(currTime).Date.ToString();
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.spellDate = Convert.ToDateTime(currTime).Date.ToString();
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
                                spelldata = new TubewellSpellData();
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
                    spelldata.spellDate = Convert.ToDateTime(curtm).Date.ToString();
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
                tableData.pumpStatus = new List<double>();
                tableData.pumpStatus = new List<double>();
                tableData.waterFlow = new List<double>();
                tableData.chlorineLevel = new List<double>();
                tableData.powerFactor = new List<double>();
                tableData.V1N = new List<double>();
                tableData.V2N = new List<double>();
                tableData.V3N = new List<double>();
                tableData.V12 = new List<double>();
                tableData.V13 = new List<double>();
                tableData.V23 = new List<double>();
                tableData.voltageTrip = new List<double>();
                tableData.I1 = new List<double>();
                tableData.I2 = new List<double>();
                tableData.I3 = new List<double>();
                tableData.currentTrip = new List<double>();
                tableData.frequency = new List<double>();
                tableData.pkva = new List<double>();
                tableData.pkvar = new List<double>();
                tableData.pkw = new List<double>();
                tableData.remoteControll = new List<double>();
                tableData.schedulingStatus = new List<double>();
                tableData.manualStatus = new List<double>();
                tableData.primingTankLevel = new List<double>();
                tableData.pressure = new List<double>();
                tableData.Vibration_m = new List<double>();
                tableData.Vibration_m_s = new List<double>();
                tableData.Vibration_m_s_2 = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName = location;
                    tableData.Specification = specs;
                    tableData.pumpStatus.Add(Convert.ToInt32(dr["PumpStatus"]));
                    tableData.waterFlow.Add(Convert.ToDouble(dr["WaterFlow(Cusec)."]));
                    tableData.chlorineLevel.Add(Convert.ToDouble(dr["ChlorineLevel."]));
                    tableData.powerFactor.Add(Convert.ToDouble(dr["PF."]));
                    tableData.V1N.Add(Convert.ToDouble(dr["V1N."]));
                    tableData.V2N.Add(Convert.ToDouble(dr["V2N."]));
                    tableData.V3N.Add(Convert.ToDouble(dr["V3N."]));
                    tableData.V12.Add(Convert.ToDouble(dr["V12"]));
                    tableData.V13.Add(Convert.ToDouble(dr["V13"]));
                    tableData.V23.Add(Convert.ToDouble(dr["V23"]));
                    tableData.voltageTrip.Add(Convert.ToDouble(dr["VoltageTrip."]));
                    tableData.I1.Add(Convert.ToDouble(dr["I1."]));
                    tableData.I2.Add(Convert.ToDouble(dr["I2."]));
                    tableData.I3.Add(Convert.ToDouble(dr["I3."]));
                    tableData.currentTrip.Add(Convert.ToDouble(dr["CurrentTrip."]));
                    tableData.frequency.Add(Convert.ToDouble(dr["Frequency."]));
                    tableData.pkva.Add(Convert.ToDouble(dr["PKVA."]));
                    tableData.pkvar.Add(Convert.ToDouble(dr["PKVAR."]));
                    tableData.pkw.Add(Convert.ToDouble(dr["PKW."]));
                    //tableData.autoMode.Add(Convert.ToDouble(dr["PKW."]));
                    tableData.remoteControll.Add(Convert.ToDouble(dr["Remote."]));
                    tableData.schedulingStatus.Add(Convert.ToDouble(dr["TimeSchedule."]));
                    tableData.manualStatus.Add(Convert.ToDouble(dr["Manual"]));
                    tableData.primingTankLevel.Add(Convert.ToDouble(dr["PrimingLevel"]));
                    tableData.pressure.Add(Convert.ToDouble(dr["Pressure(Bar)"]));
                    object value = dr["vib_Ms2"];
                    if (value == DBNull.Value)
                    {
                        tableData.Vibration_m_s_2.Add(0);
                        tableData.Vibration_m_s.Add(0);
                        tableData.Vibration_m.Add(0);
                    }
                    else
                    {
                        tableData.Vibration_m_s_2.Add(Convert.ToDouble(dr["vib_Ms2"]));
                        tableData.Vibration_m_s.Add(Convert.ToDouble(dr["vib_Ms"]));
                        tableData.Vibration_m.Add(Convert.ToDouble(dr["vib_M"]));
                    }
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.workingHoursToday = pstr;
                tableData.WorkingInHours = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes)/60, 2);
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                if (spellDataList.Count == 0)
                {
                    tableData.accWaterDischargePerDay = "0";
                }
                else
                {
                    int s = 0;
                    double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                    if (avgWaterFlow == 0)
                    {
                        avgWaterFlow = 1;
                    }
                    tableData.accWaterDischargePerDay = (((Convert.ToDouble(spellDataList.DefaultIfEmpty().Sum(i => i.spellPeriod)) / 60) * 102) * avgWaterFlow).ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                }
            }
            else
            {
                tableData.pumpStatus = new List<double>();
                tableData.pumpStatus = new List<double>();
                tableData.waterFlow = new List<double>();
                tableData.chlorineLevel = new List<double>();
                tableData.powerFactor = new List<double>();
                tableData.V1N = new List<double>();
                tableData.V2N = new List<double>();
                tableData.V3N = new List<double>();
                tableData.V12 = new List<double>();
                tableData.V13 = new List<double>();
                tableData.V23 = new List<double>();
                tableData.voltageTrip = new List<double>();
                tableData.I1 = new List<double>();
                tableData.I2 = new List<double>();
                tableData.I3 = new List<double>();
                tableData.currentTrip = new List<double>();
                tableData.frequency = new List<double>();
                tableData.pkva = new List<double>();
                tableData.pkvar = new List<double>();
                tableData.pkw = new List<double>();
                tableData.remoteControll = new List<double>();
                tableData.schedulingStatus = new List<double>();
                tableData.manualStatus = new List<double>();
                tableData.primingTankLevel = new List<double>();
                tableData.pressure = new List<double>();
                tableData.Vibration_m = new List<double>();
                tableData.Vibration_m_s = new List<double>();
                tableData.Vibration_m_s_2 = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName = location;
                    tableData.Specification = specs;
                    tableData.pumpStatus.Add(Convert.ToInt32(dr["PumpStatus"]));
                    tableData.waterFlow.Add(Convert.ToDouble(dr["WaterFlow(Cusec)."]));
                    tableData.chlorineLevel.Add(Convert.ToDouble(dr["ChlorineLevel."]));
                    tableData.powerFactor.Add(Convert.ToDouble(dr["PF."]));
                    tableData.V1N.Add(Convert.ToDouble(dr["V1N."]));
                    tableData.V2N.Add(Convert.ToDouble(dr["V2N."]));
                    tableData.V3N.Add(Convert.ToDouble(dr["V3N."]));
                    tableData.V12.Add(Convert.ToDouble(dr["V12"]));
                    tableData.V13.Add(Convert.ToDouble(dr["V13"]));
                    tableData.V23.Add(Convert.ToDouble(dr["V23"]));
                    tableData.voltageTrip.Add(Convert.ToDouble(dr["VoltageTrip."]));
                    tableData.I1.Add(Convert.ToDouble(dr["I1."]));
                    tableData.I2.Add(Convert.ToDouble(dr["I2."]));
                    tableData.I3.Add(Convert.ToDouble(dr["I3."]));
                    tableData.currentTrip.Add(Convert.ToDouble(dr["CurrentTrip."]));
                    tableData.frequency.Add(Convert.ToDouble(dr["Frequency."]));
                    tableData.pkva.Add(Convert.ToDouble(dr["PKVA."]));
                    tableData.pkvar.Add(Convert.ToDouble(dr["PKVAR."]));
                    tableData.pkw.Add(Convert.ToDouble(dr["PKW."]));
                    //tableData.autoMode.Add(Convert.ToDouble(dr["PKW."]));
                    tableData.remoteControll.Add(Convert.ToDouble(dr["Remote."]));
                    tableData.schedulingStatus.Add(Convert.ToDouble(dr["TimeSchedule."]));
                    tableData.manualStatus.Add(Convert.ToDouble(dr["Manual"]));
                    tableData.primingTankLevel.Add(Convert.ToDouble(dr["PrimingLevel"]));
                    tableData.pressure.Add(Convert.ToDouble(dr["Pressure(Bar)"]));
                    object value = dr["vib_Ms2"];
                    if (value == DBNull.Value)
                    {
                        tableData.Vibration_m_s_2.Add(0);
                        tableData.Vibration_m_s.Add(0);
                        tableData.Vibration_m.Add(0);
                    }
                    else
                    {
                        tableData.Vibration_m_s_2.Add(Convert.ToDouble(dr["vib_Ms2"]));
                        tableData.Vibration_m_s.Add(Convert.ToDouble(dr["vib_Ms"]));
                        tableData.Vibration_m.Add(Convert.ToDouble(dr["vib_M"]));
                    }
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.workingHoursToday = pstr;
                tableData.WorkingInHours = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes)/60,2);
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Where(item => item > -1).Average());
                tableData.accWaterDischargePerDay = ((tableData.WorkingInHours) * avgWaterFlow).ToString();
            }
            return tableData;
        }

        public TubewellDataClass getAllSpellsForRemoteStatus(DataTable dt, int order)
        {
            var tableData = new TubewellDataClass();
            var spelldata = new TubewellSpellData();
            //int resourceID = Convert.ToInt32(dt.Rows[0]["resourceID"]);
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus"])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<TubewellSpellData> spellDataList = new List<TubewellSpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus"])), 2);
                double currValueRemote = Math.Round((Convert.ToDouble(dr["Remote."])), 2);
                double currValueManual = Math.Round((Convert.ToDouble(dr["Manual"])), 2);
                double currValueScheduling = Math.Round((Convert.ToDouble(dr["TimeSchedule."])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["WaterFlow(Cusec)."])), 2);
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
                                    if (currValueManual == 0 && currValueRemote == 0 && currValueScheduling == 0)
                                    {
                                        spelldata.spellMode = 1;
                                    }
                                    // Manual Mode 1
                                    else if (currValueManual == 1 && currValueRemote == 0 && currValueScheduling == 0)
                                    {
                                        spelldata.spellMode = 1;
                                    }
                                    // Remote Mode 2
                                    else if (currValueManual == 0 && currValueRemote == 1 && currValueScheduling == 0)
                                    {
                                        spelldata.spellMode = 2;
                                    }
                                    // Scheduling Mode 3
                                    else
                                    {
                                        spelldata.spellMode = 3;
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
                                if (currValueManual == 0 && currValueRemote == 0 && currValueScheduling == 0)
                                {
                                    spelldata.spellMode = 1;
                                }
                                // Manual Mode 1
                                else if (currValueManual == 1 && currValueRemote == 0 && currValueScheduling == 0)
                                {
                                    spelldata.spellMode = 1;
                                }
                                // Remote Mode 2
                                else if (currValueManual == 0 && currValueRemote == 1 && currValueScheduling == 0)
                                {
                                    spelldata.spellMode = 2;
                                }
                                // Scheduling Mode 3
                                else
                                {
                                    spelldata.spellMode = 3;
                                }
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
                                spelldata = new TubewellSpellData();
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
                                    if (currValueManual == 0 && currValueRemote == 0 && currValueScheduling == 0)
                                    {
                                        spelldata.spellMode = 1;
                                    }
                                    // Manual Mode 1
                                    else if (currValueManual == 1 && currValueRemote == 0 && currValueScheduling == 0)
                                    {
                                        spelldata.spellMode = 1;
                                    }
                                    // Remote Mode 2
                                    else if (currValueManual == 0 && currValueRemote == 1 && currValueScheduling == 0)
                                    {
                                        spelldata.spellMode = 2;
                                    }
                                    // Scheduling Mode 3
                                    else
                                    {
                                        spelldata.spellMode = 3;
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
                                if (currValueManual == 0 && currValueRemote == 0 && currValueScheduling == 0)
                                {
                                    spelldata.spellMode = 1;
                                }
                                // Manual Mode 1
                                else if (currValueManual == 1 && currValueRemote == 0 && currValueScheduling == 0)
                                {
                                    spelldata.spellMode = 1;
                                }
                                // Remote Mode 2
                                else if (currValueManual == 0 && currValueRemote == 1 && currValueScheduling == 0)
                                {
                                    spelldata.spellMode = 2;
                                }
                                // Scheduling Mode 3
                                else
                                {
                                    spelldata.spellMode = 3;
                                }
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
                                spelldata = new TubewellSpellData();
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
                spelldata.spellMode = 1;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                tableData.pumpStatus = new List<double>();
                tableData.pumpStatus = new List<double>();
                tableData.waterFlow = new List<double>();
                tableData.chlorineLevel = new List<double>();
                tableData.powerFactor = new List<double>();
                tableData.V1N = new List<double>();
                tableData.V2N = new List<double>();
                tableData.V3N = new List<double>();
                tableData.V12 = new List<double>();
                tableData.V13 = new List<double>();
                tableData.V23 = new List<double>();
                tableData.voltageTrip = new List<double>();
                tableData.I1 = new List<double>();
                tableData.I2 = new List<double>();
                tableData.I3 = new List<double>();
                tableData.currentTrip = new List<double>();
                tableData.frequency = new List<double>();
                tableData.pkva = new List<double>();
                tableData.pkvar = new List<double>();
                tableData.pkw = new List<double>();
                tableData.remoteControll = new List<double>();
                tableData.schedulingStatus = new List<double>();
                tableData.manualStatus = new List<double>();
                tableData.primingTankLevel = new List<double>();
                tableData.pressure = new List<double>();
                tableData.workingHoursTodayManual = "";
                tableData.workingHoursTodayRemote = "";
                tableData.workingHoursTodayScheduling = "";
                tableData.locationName = location;
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName = location;
                    tableData.pumpStatus.Add(Convert.ToInt32(dr["PumpStatus"]));
                    tableData.waterFlow.Add(Convert.ToDouble(dr["WaterFlow(Cusec)."]));
                    tableData.chlorineLevel.Add(Convert.ToDouble(dr["ChlorineLevel."]));
                    tableData.powerFactor.Add(Convert.ToDouble(dr["PF."]));
                    tableData.V1N.Add(Convert.ToDouble(dr["V1N."]));
                    tableData.V2N.Add(Convert.ToDouble(dr["V2N."]));
                    tableData.V3N.Add(Convert.ToDouble(dr["V3N."]));
                    tableData.V12.Add(Convert.ToDouble(dr["V12"]));
                    tableData.V13.Add(Convert.ToDouble(dr["V13"]));
                    tableData.V23.Add(Convert.ToDouble(dr["V23"]));
                    tableData.voltageTrip.Add(Convert.ToDouble(dr["VoltageTrip."]));
                    tableData.I1.Add(Convert.ToDouble(dr["I1."]));
                    tableData.I2.Add(Convert.ToDouble(dr["I2."]));
                    tableData.I3.Add(Convert.ToDouble(dr["I3."]));
                    tableData.currentTrip.Add(Convert.ToDouble(dr["CurrentTrip."]));
                    tableData.frequency.Add(Convert.ToDouble(dr["Frequency."]));
                    tableData.pkva.Add(Convert.ToDouble(dr["PKVA."]));
                    tableData.pkvar.Add(Convert.ToDouble(dr["PKVAR."]));
                    tableData.pkw.Add(Convert.ToDouble(dr["PKW."]));
                    //tableData.autoMode.Add(Convert.ToDouble(dr["PKW."]));
                    tableData.remoteControll.Add(Convert.ToDouble(dr["Remote."]));
                    tableData.schedulingStatus.Add(Convert.ToDouble(dr["TimeSchedule."]));
                    tableData.manualStatus.Add(Convert.ToDouble(dr["Manual"]));
                    tableData.primingTankLevel.Add(Convert.ToDouble(dr["PrimingLevel"]));
                    tableData.pressure.Add(Convert.ToDouble(dr["Pressure(Bar)"]));
                }
                if (spellDataList.Count == 0)
                {
                    tableData.workingHoursToday = "0 Minutes";
                    tableData.accWaterDischargePerDay = "0 Cusecs";
                    tableData.workingHoursTodayManual = "0";
                    tableData.workingHoursTodayRemote = "0";
                    tableData.workingHoursTodayScheduling = "0";
                    tableData.WorkingInHoursManual = 0;
                    tableData.WorkingInHoursRemote = 0;
                    tableData.WorkingInHoursScheduling = 0;
                }
                else
                {
                    var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                    int phour = (int)pp.TotalHours;
                    int pmin = (int)pp.Minutes;
                    int psec = (int)pp.Seconds;
                    string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                    tableData.workingHoursToday = pstr;
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                    tableData.accWaterDischargePerDay = (((Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)) / 60) * 102) * avgWaterFlow).ToString();
                    tableData.WorkingInHoursManual = spellDataList.Where(r => r.spellMode == 1).Sum(i => i.spellPeriod);
                    tableData.WorkingInHoursRemote = spellDataList.Where(r => r.spellMode == 2).Sum(i => i.spellPeriod);
                    tableData.WorkingInHoursScheduling = spellDataList.Where(r => r.spellMode == 3).Sum(i => i.spellPeriod);
                    tableData.workingHoursTodayManual = minutesToTime(spellDataList.Where(r => r.spellMode == 1).Sum(i => i.spellPeriod));
                    tableData.workingHoursTodayRemote = minutesToTime(spellDataList.Where(r => r.spellMode == 2).Sum(i => i.spellPeriod));
                    tableData.workingHoursTodayScheduling = minutesToTime(spellDataList.Where(r => r.spellMode == 3).Sum(i => i.spellPeriod));
                }
            }
            else
            {
                tableData.pumpStatus = new List<double>();
                tableData.pumpStatus = new List<double>();
                tableData.waterFlow = new List<double>();
                tableData.chlorineLevel = new List<double>();
                tableData.powerFactor = new List<double>();
                tableData.V1N = new List<double>();
                tableData.V2N = new List<double>();
                tableData.V3N = new List<double>();
                tableData.V12 = new List<double>();
                tableData.V13 = new List<double>();
                tableData.V23 = new List<double>();
                tableData.voltageTrip = new List<double>();
                tableData.I1 = new List<double>();
                tableData.I2 = new List<double>();
                tableData.I3 = new List<double>();
                tableData.currentTrip = new List<double>();
                tableData.frequency = new List<double>();
                tableData.pkva = new List<double>();
                tableData.pkvar = new List<double>();
                tableData.pkw = new List<double>();
                tableData.remoteControll = new List<double>();
                tableData.schedulingStatus = new List<double>();
                tableData.manualStatus = new List<double>();
                tableData.primingTankLevel = new List<double>();
                tableData.pressure = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName = location;
                    tableData.pumpStatus.Add(Convert.ToInt32(dr["PumpStatus"]));
                    tableData.waterFlow.Add(Convert.ToDouble(dr["WaterFlow(Cusec)."]));
                    tableData.chlorineLevel.Add(Convert.ToDouble(dr["ChlorineLevel."]));
                    tableData.powerFactor.Add(Convert.ToDouble(dr["PF."]));
                    tableData.V1N.Add(Convert.ToDouble(dr["V1N."]));
                    tableData.V2N.Add(Convert.ToDouble(dr["V2N."]));
                    tableData.V3N.Add(Convert.ToDouble(dr["V3N."]));
                    tableData.V12.Add(Convert.ToDouble(dr["V12"]));
                    tableData.V13.Add(Convert.ToDouble(dr["V13"]));
                    tableData.V23.Add(Convert.ToDouble(dr["V23"]));
                    tableData.voltageTrip.Add(Convert.ToDouble(dr["VoltageTrip."]));
                    tableData.I1.Add(Convert.ToDouble(dr["I1."]));
                    tableData.I2.Add(Convert.ToDouble(dr["I2."]));
                    tableData.I3.Add(Convert.ToDouble(dr["I3."]));
                    tableData.currentTrip.Add(Convert.ToDouble(dr["CurrentTrip."]));
                    tableData.frequency.Add(Convert.ToDouble(dr["Frequency."]));
                    tableData.pkva.Add(Convert.ToDouble(dr["PKVA."]));
                    tableData.pkvar.Add(Convert.ToDouble(dr["PKVAR."]));
                    tableData.pkw.Add(Convert.ToDouble(dr["PKW."]));
                    //tableData.autoMode.Add(Convert.ToDouble(dr["PKW."]));
                    tableData.remoteControll.Add(Convert.ToDouble(dr["Remote."]));
                    tableData.schedulingStatus.Add(Convert.ToDouble(dr["TimeSchedule."]));
                    tableData.manualStatus.Add(Convert.ToDouble(dr["Manual"]));
                    tableData.primingTankLevel.Add(Convert.ToDouble(dr["PrimingLevel"]));
                    tableData.pressure.Add(Convert.ToDouble(dr["Pressure(Bar)"]));
                }
                if (spellDataList.Count == 0)
                {
                    tableData.workingHoursToday = "0 Minutes";
                    tableData.accWaterDischargePerDay = "0 Cusecs";
                    tableData.workingHoursTodayManual = "0";
                    tableData.workingHoursTodayRemote = "0";
                    tableData.workingHoursTodayScheduling = "0";
                    tableData.WorkingInHoursManual = 0;
                    tableData.WorkingInHoursRemote = 0;
                    tableData.WorkingInHoursScheduling = 0;
                }
                else
                {
                    TimeSpan pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                    int phour = pp.Hours;
                    int pmin = pp.Minutes;
                    int psec = pp.Seconds;
                    string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                    tableData.workingHoursToday = pstr;
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                    tableData.accWaterDischargePerDay = (((Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)) / 60) * 102) * avgWaterFlow).ToString();
                    tableData.WorkingInHoursManual = spellDataList.Where(r => r.spellMode == 1).Sum(i => i.spellPeriod);
                    tableData.WorkingInHoursRemote = spellDataList.Where(r => r.spellMode == 2).Sum(i => i.spellPeriod);
                    tableData.WorkingInHoursScheduling = spellDataList.Where(r => r.spellMode == 3).Sum(i => i.spellPeriod);
                    tableData.workingHoursTodayManual = minutesToTime(tableData.WorkingInHoursManual);
                    tableData.workingHoursTodayRemote = minutesToTime(tableData.WorkingInHoursRemote);
                    tableData.workingHoursTodayScheduling = minutesToTime(tableData.WorkingInHoursScheduling);
                }
            }
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
        public string SwitchON(string id)
        {
            return "";
        }
    }
}