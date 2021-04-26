using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace WASA_EMS
{
    public class BAL
    {
        public char seperator;
        public int c_id;
        public BAL()
        {

        }
        public int saveData(string sender, string messageText, string sentTime)
        {
            string lastUpdateTime = "";
            double lastUpdateData = 0.0;
            string datetime = "";
            double timeFromLastUpdate = 0.0;
            //FOR TIME
            if (sentTime == "x" || sentTime == "X")
            {
                datetime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString();
            }
            else
            {
                DateTime dt = Convert.ToDateTime(sentTime.ToString());
                string Pakdatetime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString();
                string theSentTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dt, "Pakistan Standard Time").ToString();
                if (dt > Convert.ToDateTime(Pakdatetime.ToString()))
                {
                    dt = Convert.ToDateTime(Pakdatetime.ToString());
                }
                datetime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                if (datetime == "1800-01-01 12:00:00")
                {
                    datetime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString();
                }
            }
            //Set Current Pak Time Anyway (Default)
            datetime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").AddMinutes(-2).ToString();
            bool flag = false;
            int ResourceID = 0;
            //FOR COMPANY_ID
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    string query = "select companyID from tblResource Where resourceCode = '" + sender + "' ";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    var dr = cmd.ExecuteScalar();
                    c_id = Convert.ToInt32(dr.ToString());
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            //FOR SEPARATOR
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    string query = "select t.separator from tblResource r ";
                    query += "INNER JOIN tblResourceType t ";
                    query += "on r.resourceTypeID = t.resourceTypeID ";
                    query += "where r.resourceCode = '" + sender + "' ";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    var dr = cmd.ExecuteScalar();
                    seperator = Char.Parse(dr.ToString());
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            string[] parts = messageText.Split(Convert.ToChar(seperator));
            //FOR RESOURCE_ID
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    string query = "select resourceID, resourceCode from tblResource where resourceCode = '" + sender + "' ";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandText = query;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ResourceID = Int32.Parse(dr[0].ToString());
                        flag = true;
                    }
                }
                catch (Exception ex)
                {

                }
                try
                {
                    //Check if the data is updated after a long time
                    //Setting a reference 0 on both ends
                    string queryForLastUpdate = "select top (1) s.sheetInsertionDateTime, s.parameterValue from tblSheet s inner join tblResource r on s.resourceID = r.resourceID inner join tblResourceType rt on r.resourceTypeID = rt.resourceTypeID where s.resourceID = "+ResourceID+" and rt.resourceTypeName = 'Ponding Points' order by sheetInsertionDateTime desc";
                    SqlCommand cmdlu = new SqlCommand(queryForLastUpdate, conn);
                    SqlDataReader ludr = cmdlu.ExecuteReader();
                    while (ludr.Read())
                    {
                        lastUpdateTime = ludr[0].ToString();
                        lastUpdateData = Convert.ToDouble(ludr[1]);
                        timeFromLastUpdate = Math.Abs((Convert.ToDateTime(datetime) - Convert.ToDateTime(lastUpdateTime)).TotalMinutes);
                        if (timeFromLastUpdate > 30)
                        {
                            DateTime rareTime = Convert.ToDateTime(lastUpdateTime).AddMinutes(1);
                            DateTime frontTime = Convert.ToDateTime(datetime).AddMinutes(-1);
                            string sqlReferenceInsertOnBothEnds = "insert into tblsheet (resourceID, parameterID, parameterValue, sheetInsertionDateTime, companyID) ";
                            sqlReferenceInsertOnBothEnds += "values(" + ResourceID + " , ";
                            sqlReferenceInsertOnBothEnds += "(select parameterID from ";
                            sqlReferenceInsertOnBothEnds += "(select tp.*, ROW_NUMBER() over ";
                            sqlReferenceInsertOnBothEnds += "(order by parameterID ASC) as rnum ";
                            sqlReferenceInsertOnBothEnds += "from tblResource r ";
                            sqlReferenceInsertOnBothEnds += "INNER JOIN tblResourceTypeParameter tp  on r.resourceTypeID = tp.resourceTypeID ";
                            sqlReferenceInsertOnBothEnds += "where r.resourceID = 1) tp where rnum = 1), 0, '"+ rareTime + "' ,"+ c_id + ") , " ;
                            sqlReferenceInsertOnBothEnds += " (" + ResourceID + " , ";
                            sqlReferenceInsertOnBothEnds += "(select parameterID from ";
                            sqlReferenceInsertOnBothEnds += "(select tp.*, ROW_NUMBER() over ";
                            sqlReferenceInsertOnBothEnds += "(order by parameterID ASC) as rnum ";
                            sqlReferenceInsertOnBothEnds += "from tblResource r ";
                            sqlReferenceInsertOnBothEnds += "INNER JOIN tblResourceTypeParameter tp  on r.resourceTypeID = tp.resourceTypeID ";
                            sqlReferenceInsertOnBothEnds += "where r.resourceID = 1) tp where rnum = 1), 0, '" + frontTime + "' ," + c_id + ") ";
                            SqlCommand cmd1 = new SqlCommand(sqlReferenceInsertOnBothEnds, conn);
                            cmd1.ExecuteNonQuery();
                        }
                    }
                    ///////////////////////
                    string lat = parts[1];
                    string lng = parts[2];
                    if (Convert.ToDouble(lat) < 31 || Convert.ToDouble(lng) < 74)
                    {

                    }
                    else
                    {
                        string query1 = "update r set r.resourceGeoLocatin = '" + lat + "," + lng + "' ";
                        query1 += " from tblResource r, tblResourceType rt ";
                        query1 += " where rt.resourceTypeID = r.resourceTypeID ";
                        query1 += "and (";
                        query1 += "rt.resourceTypeName = 'Ponding Points' ";
                        //query1 += "OR rt.resourceTypeName = 'Rain Guages' ";
                        query1 += ") ";
                        query1 += "and r.resourceID = " + ResourceID + " ";
                        SqlCommand cmd1 = new SqlCommand(query1, conn);
                        cmd1.ExecuteNonQuery();
                    }
                    ///////////////////////
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            if (flag)
            {
                for (int counter = 0; counter < parts.Length; counter++)
                {
                    double msg = 0;
                    string resultMessage = parts[counter];
                    var data = Regex.Match(resultMessage, @"^-?\d+(?:\.\d+)?").Value;
                    if (data == "" || data == "nan" || data == "Nan" || data == "NAn" || data == "NaN" || data == "nAn" || data == "nAN" || data == "naN" || data == "NAN")
                    {
                        msg = -0.1;
                    }
                    else
                    {
                        msg = Convert.ToDouble(data);
                    }
                    if (msg < -0.1)
                    {
                        msg = 0;
                    }
                    //Thresold for Human Error in "Ponding"; 24 inches
                    if (Math.Abs(msg - lastUpdateData) < 60.96)
                    {
                        //msg = lastUpdateData;
                        using (SqlConnection conn1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
                        {
                            try
                            {
                                string query2 = "insert into tblSheet(sheetInsertionDateTime, resourceID,parameterID,parameterValue,companyID) ";
                                //return 0;
                                query2 += "values(";
                                query2 += " '" + datetime + "', ";
                                query2 += "" + ResourceID + ",";
                                query2 += " (select parameterID from (select tp.*, ROW_NUMBER() over ";
                                query2 += " (order by parameterID ASC) as rnum ";
                                query2 += " from tblResource r ";
                                query2 += " INNER JOIN tblResourceTypeParameter tp ";
                                query2 += " on r.resourceTypeID = tp.resourceTypeID ";
                                query2 += " where r.resourceID = " + ResourceID + ") ";
                                query2 += " tp where rnum = " + counter + "+" + 1 + "), ";
                                query2 += " " + msg + " ";
                                query2 += ", " + c_id + " )";
                                SqlCommand cmdIn = new SqlCommand(query2, conn1);
                                conn1.Open();
                                cmdIn.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }
                            conn1.Close();
                        }
                    }
                    else
                    {
                        //msg = lastUpdateData;
                        using (SqlConnection conn1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
                        {
                            try
                            {
                                string query2 = "insert into tblSheet(sheetInsertionDateTime, resourceID,parameterID,parameterValue,companyID) ";
                                //return 0;
                                query2 += "values(";
                                query2 += " '" + datetime + "', ";
                                query2 += "" + ResourceID + ",";
                                query2 += " (select parameterID from (select tp.*, ROW_NUMBER() over ";
                                query2 += " (order by parameterID ASC) as rnum ";
                                query2 += " from tblResource r ";
                                query2 += " INNER JOIN tblResourceTypeParameter tp ";
                                query2 += " on r.resourceTypeID = tp.resourceTypeID ";
                                query2 += " where r.resourceID = " + ResourceID + ") ";
                                query2 += " tp where rnum = " + counter + "+" + 1 + "), ";
                                query2 += " " + msg + " ";
                                query2 += ", " + c_id + " )";
                                SqlCommand cmdIn = new SqlCommand(query2, conn1);
                                conn1.Open();
                                cmdIn.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }
                            conn1.Close();
                        }
                    }
                }
                return 1;
            }
            else
            {
                return 0;
            }

        }
        public string getRemoteMode(string sender)
        {
            DataTable dt = new DataTable();
            string jsonObj = "";
            string query = "select Mode from tblSetMode where ResourceID = (select ResourceID from tblResource where MobileNumber = '" + sender + "')";
            using (SqlConnection conn1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    SqlCommand cmdIn = new SqlCommand(query, conn1);
                    conn1.Open();
                    jsonObj = cmdIn.ExecuteScalar().ToString();
                    if (jsonObj.ToLower() == "true")
                    {
                        jsonObj = "1";
                    }
                    else
                    {
                        jsonObj = "0";
                    }
                }
                catch (Exception ex)
                {

                }
                conn1.Close();
            }
            return jsonObj;
        }
        public string getScheduleTime(string sender)
        {
            DataTable dt = new DataTable();
            string jsonObj = "";
            string query = "select TimeFrom, TimeTo from tblTubewellSchedule where ResourceID = (select ResourceID from tblResource where MobileNumber = '"+sender+"')";
            using (SqlConnection conn1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    SqlCommand cmdIn = new SqlCommand(query, conn1);
                    conn1.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmdIn);
                    sda.Fill(dt);
                    jsonObj = DataTableToJSONWithJSONNet(dt);
                }
                catch (Exception ex)
                {

                }
                conn1.Close();
            }
            return jsonObj;
        }
        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
        public int saveData(string sender, string messageText, string sentTime, string lat, string lng)
        {
            string datetime = "";
            //FOR TIME
            if (sentTime == "x" || sentTime == "X")
            {
                datetime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString();
            }
            else
            {
                DateTime dt = Convert.ToDateTime(sentTime.ToString());
                datetime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                if (datetime == "1800-01-01 12:00:00")
                {
                    datetime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString();
                }
            }

            bool flag = false;
            int ResourceID = 0;
            //FOR COMPANY_ID
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    string query = "select companyID from tblResource Where resourceCode = '" + sender + "' ";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    var dr = cmd.ExecuteScalar();
                    c_id = Convert.ToInt32(dr.ToString());
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            //FOR SEPARATOR
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    string query = "select t.separator from tblResource r ";
                    query += "INNER JOIN tblResourceType t ";
                    query += "on r.resourceTypeID = t.resourceTypeID ";
                    query += "where r.resourceCode = '" + sender + "' ";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    var dr = cmd.ExecuteScalar();
                    seperator = Char.Parse(dr.ToString());
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }

            string[] parts = messageText.Split(Convert.ToChar(seperator));
            //FOR RESOURCE_ID
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    string query = "select resourceID, resourceCode from tblResource where resourceCode = '" + sender + "' ";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandText = query;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ResourceID = Int32.Parse(dr[0].ToString());
                        flag = true;
                    }
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            if (flag)
            {
                for (int counter = 0; counter < parts.Length; counter++)
                {
                    double msg = 0;
                    string resultMessage = parts[counter];
                    var data = Regex.Match(resultMessage, @"^-?\d+(?:\.\d+)?").Value;
                    if (data == "" || data == "nan" || data == "Nan" || data == "NAn" || data == "NaN" || data == "nAn" || data == "nAN" || data == "naN" || data == "NAN")
                    {
                        msg = -0.1;
                    }
                    else
                    {
                        msg = Convert.ToDouble(data);
                    }
                    if (msg < -0.1)
                    {
                        msg = 0;
                    }
                    //SAVING RECORD
                    using (SqlConnection conn1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
                    {
                        try
                        {
                            string query2 = "insert into tblSheet(sheetInsertionDateTime, resourceID,parameterID,parameterValue,companyID) ";
                            query2 += "values(";
                            query2 += " '" + datetime + "', ";
                            query2 += "" + ResourceID + ",";
                            query2 += " (select parameterID from (select tp.*, ROW_NUMBER() over ";
                            query2 += " (order by parameterID ASC) as rnum ";
                            query2 += " from tblResource r ";
                            query2 += " INNER JOIN tblResourceTypeParameter tp ";
                            query2 += " on r.resourceTypeID = tp.resourceTypeID ";
                            query2 += " where r.resourceID = " + ResourceID + ") ";
                            query2 += " tp where rnum = " + counter + "+" + 1 + "), ";
                            query2 += " " + msg + " ";
                            query2 += ", " + c_id + " )";
                            SqlCommand cmdIn = new SqlCommand(query2, conn1);
                            conn1.Open();
                            cmdIn.ExecuteNonQuery();

                            ///////////
                            string query1 = "update tblResource set CooridatesGoogle = '" + lat + "," + lng + "' where resourceID = " + ResourceID + " ";
                            SqlCommand cmd1 = new SqlCommand(query1, conn1);
                            cmd1.ExecuteNonQuery();
                            ///////////
                        }
                        catch (Exception ex)
                        {

                        }
                        conn1.Close();
                    }

                }
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int getCompanyID(string userName, string password)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    string query = "SELECT companyID FROM tblUser Where userLoginName = '" + userName + "' and userPassword = '" + password + "' ";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }

            return result;
        }

        public string execQuery(string query)
        {
            string ret = "";
            using (SqlConnection conn1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    string query2 = query;
                    SqlCommand cmdIn = new SqlCommand(query2, conn1);
                    conn1.Open();
                    cmdIn.ExecuteNonQuery();
                    ret = "Successful";
                }
                catch (Exception ex)
                {
                    ret = "Unsuccessful : " + ex.Message.ToString() + "";
                }
            }
            return ret;
        }

        public string DisposalCurrentDayReport()
        {
            DisposalDataClassFinal df = new DisposalDataClassFinal();
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
                        df.locationName1 = "Pump Set 1";
                        df.WorkingInHoursPump1 = sd.WorkingInHoursPump1;
                        df.Well1Level_Average = sd.Well1Level.DefaultIfEmpty().Average().ToString();
                        df.WorkingHoursPump1 = sd.WorkingHoursPump1;
                    }
                    else
                    {
                        df.locationName1 = "Pump Set 1";
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
                        df.WorkingInHoursPump2 = sd.WorkingInHoursPump2;
                        df.WorkingHoursPump2 = sd.WorkingHoursPump2;
                    }
                    else
                    {
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
                        df.WorkingInHoursPump3 = sd.WorkingInHoursPump3;
                        df.WorkingHoursPump3 = sd.WorkingHoursPump3;
                    }
                    else
                    {
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
                        df.WorkingInHoursPump4 = sd.WorkingInHoursPump4;
                        df.WorkingHoursPump4 = sd.WorkingHoursPump4;
                    }
                    else
                    {
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
                        df.WorkingInHoursPump5 = sd.WorkingInHoursPump5;
                        df.WorkingHoursPump5 = sd.WorkingHoursPump5;
                    }
                    else
                    {
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
                        df.locationName2 = "Pump Set 2";
                        df.WorkingInHoursPump6 = sd.WorkingInHoursPump6;
                        df.Well2Level_Average = sd.Well2Level_Average;
                        df.WorkingHoursPump6 = sd.WorkingHoursPump6;
                    }
                    else
                    {
                        df.locationName2 = "Pump Set 2";
                        df.Well2Level_Average = "0";
                        df.WorkingInHoursPump5 = "0";
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
                        df.WorkingInHoursPump7 = sd.WorkingInHoursPump7;
                        df.WorkingHoursPump7 = sd.WorkingHoursPump7;
                    }
                    else
                    {
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
                        df.WorkingInHoursPump8 = sd.WorkingInHoursPump8;
                        df.WorkingHoursPump8 = sd.WorkingHoursPump8;
                    }
                    else
                    {
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
                        df.WorkingInHoursPump9 = sd.WorkingInHoursPump9;
                        df.WorkingHoursPump9 = sd.WorkingHoursPump9;
                    }
                    else
                    {
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
                        df.WorkingInHoursPump10 = sd.WorkingInHoursPump10;
                        df.WorkingHoursPump10 = sd.WorkingHoursPump10;
                    }
                    else
                    {
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
            var json = new JavaScriptSerializer().Serialize(disposalFinalDataList);
            return json;
        }

        public string DisposalLastDayReport()
        {
            DisposalDataClassFinal df = new DisposalDataClassFinal();
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
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                    Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                        df.locationName1 = "Pump Set 1";
                        df.WorkingInHoursPump1 = sd.WorkingInHoursPump1;
                        df.Well1Level_Average = sd.Well1Level.DefaultIfEmpty().Average().ToString();
                        df.WorkingHoursPump1 = sd.WorkingHoursPump1;
                    }
                    else
                    {
                        df.locationName1 = "Pump Set 1";
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
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                    Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                        df.WorkingInHoursPump2 = sd.WorkingInHoursPump2;
                        df.WorkingHoursPump2 = sd.WorkingHoursPump2;
                    }
                    else
                    {
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
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                    Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                        df.WorkingInHoursPump3 = sd.WorkingInHoursPump3;
                        df.WorkingHoursPump3 = sd.WorkingHoursPump3;
                    }
                    else
                    {
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
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                    Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                        df.WorkingInHoursPump4 = sd.WorkingInHoursPump4;
                        df.WorkingHoursPump4 = sd.WorkingHoursPump4;
                    }
                    else
                    {
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
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                    Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                        df.WorkingInHoursPump5 = sd.WorkingInHoursPump5;
                        df.WorkingHoursPump5 = sd.WorkingHoursPump5;
                    }
                    else
                    {
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
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                    Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                        df.locationName2 = "Pump Set 2";
                        df.WorkingInHoursPump6 = sd.WorkingInHoursPump6;
                        df.Well2Level_Average = sd.Well2Level_Average;
                        df.WorkingHoursPump6 = sd.WorkingHoursPump6;
                    }
                    else
                    {
                        df.locationName2 = "Pump Set 2";
                        df.Well2Level_Average = "0";
                        df.WorkingInHoursPump5 = "0";
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
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                    Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                        df.WorkingInHoursPump7 = sd.WorkingInHoursPump7;
                        df.WorkingHoursPump7 = sd.WorkingHoursPump7;
                    }
                    else
                    {
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
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                    Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                        df.WorkingInHoursPump8 = sd.WorkingInHoursPump8;
                        df.WorkingHoursPump8 = sd.WorkingHoursPump8;
                    }
                    else
                    {
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
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                    Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                        df.WorkingInHoursPump9 = sd.WorkingInHoursPump9;
                        df.WorkingHoursPump9 = sd.WorkingHoursPump9;
                    }
                    else
                    {
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
                    Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                    Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                        df.WorkingInHoursPump10 = sd.WorkingInHoursPump10;
                        df.WorkingHoursPump10 = sd.WorkingHoursPump10;
                    }
                    else
                    {
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
            var json = new JavaScriptSerializer().Serialize(disposalFinalDataList);
            return json;
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
        ///

        public string TubewellCurrentDayReport()
        {
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var tubewellDataList = new List<TubewellDataClass>();
            int resourceID = 0;
            ////////////////////////////////////////////////////////////////////////
            ///
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
                        Dashdtquery += " r.ResourceSpecification AS specifications, r.WaterLevel_m, r.PumpingWaterLevel_hpl, r.RatedDischarge_Q, r.RatedHead_H, r.Discharge_Dia_Dd, ";
                        Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                        Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                        Dashdtquery += "s.InsertionDateTime as tim ,";
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID = " + resourceID + " and ";
                        Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        Dashdtquery += "[V1N.],[V2N.],[V3N.],[I1.],[I2.],[I3.],[Frequency.],[PKVA.],[PF.],[Remote.],[PumpStatus],[CurrentTrip.],[VoltageTrip.],[TimeSchedule.],[ChlorineLevel.],[WaterFlow(Cusec).],[PKVAR.],[PKW.],[V12],[V13],[V23],[PrimingLevel],[Pressure(Bar)],[Manual],[vib_z],[vib_y],[vib_x] ";
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
                            TubewellDataClass sd = getAllSpellsForRemoteStatus(Dashdt, dtRes.Rows.IndexOf(drRes), DateTime.Now.Date);
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
            var json = new JavaScriptSerializer().Serialize(tubewellDataList);
            return json;
        }

        public string TubewellLastDayReport()
        {
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var tubewellDataList = new List<TubewellDataClass>();
            int resourceID = 0;
            ////////////////////////////////////////////////////////////////////////
            ///
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
                        Dashdtquery += " r.ResourceSpecification AS specifications, r.WaterLevel_m, r.PumpingWaterLevel_hpl, r.RatedDischarge_Q, r.RatedHead_H, r.Discharge_Dia_Dd, ";
                        Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                        Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                        Dashdtquery += "s.InsertionDateTime as tim ,";
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID = " + resourceID + " and ";
                        Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                        Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
                        //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        Dashdtquery += "[V1N.],[V2N.],[V3N.],[I1.],[I2.],[I3.],[Frequency.],[PKVA.],[PF.],[Remote.],[PumpStatus],[CurrentTrip.],[VoltageTrip.],[TimeSchedule.],[ChlorineLevel.],[WaterFlow(Cusec).],[PKVAR.],[PKW.],[V12],[V13],[V23],[PrimingLevel],[Pressure(Bar)],[Manual],[vib_z],[vib_y],[vib_x] ";
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
                            TubewellDataClass sd = getAllSpellsForRemoteStatus(Dashdt, dtRes.Rows.IndexOf(drRes), DateTime.Now.Date);
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
            var json = new JavaScriptSerializer().Serialize(tubewellDataList);
            return json;
        }

        public TubewellDataClass getAllSpellsForRemoteStatus(DataTable dt, int order, DateTime ftf)
        {
            var tableData = new TubewellDataClass();
            var spelldata = new TubewellSpellData();
            //int resourceID = Convert.ToInt32(dt.Rows[0]["resourceID"]);
            string location = dt.Rows[0]["Location"].ToString();
            string specs = dt.Rows[0]["specifications"].ToString();
            string WaterLevel_m = dt.Rows[0]["WaterLevel_m"].ToString();
            string PumpingWaterLevel_hpl = dt.Rows[0]["PumpingWaterLevel_hpl"].ToString();
            string RatedDischarge_Q = dt.Rows[0]["RatedDischarge_Q"].ToString();
            string RatedHead_H = dt.Rows[0]["RatedHead_H"].ToString();
            string Discharge_Dia_Dd = dt.Rows[0]["Discharge_Dia_Dd"].ToString();
            //double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus"])), 2);
            //string currentTime = dt.Rows[0]["tim"].ToString();
            //double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);

            var cms = dt.Rows[0]["PumpStatus"];
            double currentMotorStatus = 0;
            if (cms == DBNull.Value)
            {
                currentMotorStatus = 0;
            }
            else
            {
                currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus"])), 2);
            }
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = 0;
            var dm = dt.Rows[0]["DeltaMinutes"];
            if (dm == DBNull.Value)
            {
                DeltaMinutes = 0;
            }
            else
            {
                DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            }

            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<TubewellSpellData> spellDataList = new List<TubewellSpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                var currV = dr["PumpStatus"];
                var currVR = dr["Remote."];
                var currVM = dr["Manual"];
                var currVT = dr["TimeSchedule."];
                var wf = dr["WaterFlow(Cusec)."];
                double currValue = 0;
                double currValueRemote = 0;
                double currValueManual = 0;
                double currValueScheduling = 0;
                double FlowRate = 0;
                if (currV == DBNull.Value)
                { }
                else
                {
                    currValue = Math.Round((Convert.ToDouble(dr["PumpStatus"])), 2);
                }
                if (currVM == DBNull.Value)
                { }
                else
                {
                    currValueManual = Math.Round((Convert.ToDouble(dr["Manual"])), 2);
                }
                if (currVR == DBNull.Value)
                { }
                else
                {
                    currValueRemote = Math.Round((Convert.ToDouble(dr["Remote."])), 2);
                }
                if (currVT == DBNull.Value)
                { }
                else
                {
                    currValueScheduling = Math.Round((Convert.ToDouble(dr["TimeSchedule."])), 2);
                }
                if (wf == DBNull.Value)
                { }
                else
                {
                    FlowRate = Math.Round((Convert.ToDouble(dr["WaterFlow(Cusec)."])), 2);
                }
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

                tableData.Vibration_m = new List<double>();
                tableData.Vibration_m_s = new List<double>();
                tableData.Vibration_m_s_2 = new List<double>();
                tableData.LogTime = new List<string>();

                tableData.workingHoursTodayManual = "";
                tableData.workingHoursTodayRemote = "";
                tableData.workingHoursTodayScheduling = "";
                tableData.locationName = location;
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName = location;
                    tableData.Specification = specs;
                    tableData.WaterLevel_m = WaterLevel_m;
                    tableData.PumpingWaterLevel_hpl = PumpingWaterLevel_hpl;
                    tableData.RatedDischarge_Q = RatedDischarge_Q;
                    tableData.RatedHead_H = RatedHead_H;
                    tableData.Discharge_Dia_Dd = Discharge_Dia_Dd;


                    var ps = dr["PumpStatus"];
                    if (ps == DBNull.Value)
                    {
                        tableData.pumpStatus.Add(tableData.pumpStatus.LastOrDefault());
                    }
                    else
                    {
                        tableData.pumpStatus.Add(Convert.ToInt32(dr["PumpStatus"]));
                    }

                    ps = dr["WaterFlow(Cusec)."];
                    if (ps == DBNull.Value)
                    {
                        tableData.waterFlow.Add(tableData.waterFlow.LastOrDefault());
                    }
                    else
                    {
                        tableData.waterFlow.Add(Convert.ToDouble(dr["WaterFlow(Cusec)."]));
                    }

                    ps = dr["ChlorineLevel."];
                    if (ps == DBNull.Value)
                    {
                        tableData.chlorineLevel.Add(1.0);
                    }
                    else
                    {
                        tableData.chlorineLevel.Add(Convert.ToDouble(dr["ChlorineLevel."]));
                    }

                    ps = dr["PF."];
                    if (ps == DBNull.Value)
                    {
                        tableData.powerFactor.Add(tableData.powerFactor.LastOrDefault());
                    }
                    else
                    {
                        tableData.powerFactor.Add(Convert.ToDouble(dr["PF."]));
                    }

                    ps = dr["V1N."];
                    if (ps == DBNull.Value)
                    {
                        tableData.V1N.Add(tableData.V1N.LastOrDefault());
                    }
                    else
                    {
                        tableData.V1N.Add(Convert.ToDouble(dr["V1N."]));
                    }

                    ps = dr["V2N."];
                    if (ps == DBNull.Value)
                    {
                        tableData.V2N.Add(tableData.V2N.LastOrDefault());
                    }
                    else
                    {
                        tableData.V2N.Add(Convert.ToDouble(dr["V2N."]));
                    }

                    ps = dr["V3N."];
                    if (ps == DBNull.Value)
                    {
                        tableData.V3N.Add(tableData.V3N.LastOrDefault());
                    }
                    else
                    {
                        tableData.V3N.Add(Convert.ToDouble(dr["V3N."]));
                    }

                    ps = dr["V12"];
                    if (ps == DBNull.Value)
                    {
                        tableData.V12.Add(tableData.V12.LastOrDefault());
                    }
                    else
                    {
                        tableData.V12.Add(Convert.ToDouble(dr["V12"]));
                    }

                    ps = dr["V13"];
                    if (ps == DBNull.Value)
                    {
                        tableData.V13.Add(tableData.V13.LastOrDefault());
                    }
                    else
                    {
                        tableData.V13.Add(Convert.ToDouble(dr["V13"]));
                    }

                    ps = dr["V23"];
                    if (ps == DBNull.Value)
                    {
                        tableData.V23.Add(tableData.V23.LastOrDefault());
                    }
                    else
                    {
                        tableData.V23.Add(Convert.ToDouble(dr["V23"]));
                    }

                    ps = dr["VoltageTrip."];
                    if (ps == DBNull.Value)
                    {
                        tableData.voltageTrip.Add(tableData.voltageTrip.LastOrDefault());
                    }
                    else
                    {
                        tableData.voltageTrip.Add(Convert.ToDouble(dr["VoltageTrip."]));
                    }

                    ps = dr["I1."];
                    if (ps == DBNull.Value)
                    {
                        tableData.I1.Add(tableData.I1.LastOrDefault());
                    }
                    else
                    {
                        tableData.I1.Add(Convert.ToDouble(dr["I1."]));
                    }

                    ps = dr["I2."];
                    if (ps == DBNull.Value)
                    {
                        tableData.I2.Add(tableData.I2.LastOrDefault());
                    }
                    else
                    {
                        tableData.I2.Add(Convert.ToDouble(dr["I2."]));
                    }

                    ps = dr["I3."];
                    if (ps == DBNull.Value)
                    {
                        tableData.I3.Add(tableData.I3.LastOrDefault());
                    }
                    else
                    {
                        tableData.I3.Add(Convert.ToDouble(dr["I3."]));
                    }

                    ps = dr["CurrentTrip."];
                    if (ps == DBNull.Value)
                    {
                        tableData.currentTrip.Add(tableData.currentTrip.LastOrDefault());
                    }
                    else
                    {
                        tableData.currentTrip.Add(Convert.ToDouble(dr["CurrentTrip."]));
                    }

                    ps = dr["Frequency."];
                    if (ps == DBNull.Value)
                    {
                        tableData.frequency.Add(tableData.frequency.LastOrDefault());
                    }
                    else
                    {
                        tableData.frequency.Add(Convert.ToDouble(dr["Frequency."]));
                    }

                    ps = dr["PKVA."];
                    if (ps == DBNull.Value)
                    {
                        tableData.pkva.Add(tableData.pkva.LastOrDefault());
                    }
                    else
                    {
                        tableData.pkva.Add(Convert.ToDouble(dr["PKVA."]));
                    }

                    ps = dr["PKVAR."];
                    if (ps == DBNull.Value)
                    {
                        tableData.pkvar.Add(tableData.pkvar.LastOrDefault());
                    }
                    else
                    {
                        tableData.pkvar.Add(Convert.ToDouble(dr["PKVAR."]));
                    }

                    ps = dr["PKW."];
                    if (ps == DBNull.Value)
                    {
                        tableData.pkw.Add(tableData.pkw.LastOrDefault());
                    }
                    else
                    {
                        tableData.pkw.Add(Convert.ToDouble(dr["PKW."]));
                    }

                    ps = dr["Remote."];
                    if (ps == DBNull.Value)
                    {
                        tableData.remoteControll.Add(tableData.remoteControll.LastOrDefault());
                    }
                    else
                    {
                        tableData.remoteControll.Add(Convert.ToDouble(dr["Remote."]));
                    }

                    ps = dr["TimeSchedule."];
                    if (ps == DBNull.Value)
                    {
                        tableData.schedulingStatus.Add(1.0);
                    }
                    else
                    {
                        tableData.schedulingStatus.Add(Convert.ToDouble(dr["TimeSchedule."]));
                    }

                    ps = dr["Manual"];
                    if (ps == DBNull.Value)
                    {
                        tableData.manualStatus.Add(tableData.manualStatus.LastOrDefault());
                    }
                    else
                    {
                        tableData.manualStatus.Add(Convert.ToDouble(dr["Manual"]));
                    }

                    ps = dr["PrimingLevel"];
                    if (ps == DBNull.Value)
                    {
                        tableData.primingTankLevel.Add(tableData.primingTankLevel.LastOrDefault());
                    }
                    else
                    {
                        tableData.primingTankLevel.Add(Convert.ToDouble(dr["PrimingLevel"]));
                    }

                    ps = dr["Pressure(Bar)"];
                    if (ps == DBNull.Value)
                    {
                        tableData.pressure.Add(tableData.pressure.LastOrDefault());
                    }
                    else
                    {
                        tableData.pressure.Add(Convert.ToDouble(dr["Pressure(Bar)"]));
                    }

                    ps = dr["tim"];
                    if (ps == DBNull.Value)
                    {
                        tableData.LogTime.Add(tableData.LogTime.LastOrDefault());
                    }
                    else
                    {
                        tableData.LogTime.Add(dr["tim"].ToString());
                    }

                    object value = dr["vib_x"];
                    if (value == DBNull.Value)
                    {
                        tableData.Vibration_m_s_2.Add(0);
                        tableData.Vibration_m_s.Add(0);
                        tableData.Vibration_m.Add(0);
                    }
                    else
                    {
                        tableData.Vibration_m_s_2.Add(Convert.ToDouble(dr["vib_x"]));
                        tableData.Vibration_m_s.Add(Convert.ToDouble(dr["vib_y"]));
                        tableData.Vibration_m.Add(Convert.ToDouble(dr["vib_z"]));
                    }

                }
                if (spellDataList.Count == 0)
                {
                    tableData.workingHoursToday = "0 Hours, 0 Minutes";
                    tableData.accWaterDischargePerDay = "0 Cusecs";
                    tableData.workingHoursTodayManual = "0";
                    tableData.workingHoursTodayRemote = "0";
                    tableData.workingHoursTodayScheduling = "0";
                    tableData.WorkingInHoursManual = 0;
                    tableData.WorkingInHoursRemote = 0;
                    tableData.WorkingInHoursScheduling = 0;
                    tableData.logDate = ftf.ToShortDateString();
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
                    tableData.logDate = Convert.ToDateTime(currentTime).Date.ToShortDateString();
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
                tableData.LogTime = new List<string>();

                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName = location;
                    tableData.Specification = specs;
                    tableData.WaterLevel_m = WaterLevel_m;
                    tableData.PumpingWaterLevel_hpl = PumpingWaterLevel_hpl;
                    tableData.RatedDischarge_Q = RatedDischarge_Q;
                    tableData.RatedHead_H = RatedHead_H;
                    tableData.Discharge_Dia_Dd = Discharge_Dia_Dd;


                    var ps = dr["PumpStatus"];
                    if (ps == DBNull.Value)
                    {
                        tableData.pumpStatus.Add(tableData.pumpStatus.LastOrDefault());
                    }
                    else
                    {
                        tableData.pumpStatus.Add(Convert.ToInt32(dr["PumpStatus"]));
                    }

                    ps = dr["WaterFlow(Cusec)."];
                    if (ps == DBNull.Value)
                    {
                        tableData.waterFlow.Add(tableData.waterFlow.LastOrDefault());
                    }
                    else
                    {
                        tableData.waterFlow.Add(Convert.ToDouble(dr["WaterFlow(Cusec)."]));
                    }

                    ps = dr["ChlorineLevel."];
                    if (ps == DBNull.Value)
                    {
                        tableData.chlorineLevel.Add(1.0);
                    }
                    else
                    {
                        tableData.chlorineLevel.Add(Convert.ToDouble(dr["ChlorineLevel."]));
                    }

                    ps = dr["PF."];
                    if (ps == DBNull.Value)
                    {
                        tableData.powerFactor.Add(tableData.powerFactor.LastOrDefault());
                    }
                    else
                    {
                        tableData.powerFactor.Add(Convert.ToDouble(dr["PF."]));
                    }

                    ps = dr["V1N."];
                    if (ps == DBNull.Value)
                    {
                        tableData.V1N.Add(tableData.V1N.LastOrDefault());
                    }
                    else
                    {
                        tableData.V1N.Add(Convert.ToDouble(dr["V1N."]));
                    }

                    ps = dr["V2N."];
                    if (ps == DBNull.Value)
                    {
                        tableData.V2N.Add(tableData.V2N.LastOrDefault());
                    }
                    else
                    {
                        tableData.V2N.Add(Convert.ToDouble(dr["V2N."]));
                    }

                    ps = dr["V3N."];
                    if (ps == DBNull.Value)
                    {
                        tableData.V3N.Add(tableData.V3N.LastOrDefault());
                    }
                    else
                    {
                        tableData.V3N.Add(Convert.ToDouble(dr["V3N."]));
                    }

                    ps = dr["V12"];
                    if (ps == DBNull.Value)
                    {
                        tableData.V12.Add(tableData.V12.LastOrDefault());
                    }
                    else
                    {
                        tableData.V12.Add(Convert.ToDouble(dr["V12"]));
                    }

                    ps = dr["V13"];
                    if (ps == DBNull.Value)
                    {
                        tableData.V13.Add(tableData.V13.LastOrDefault());
                    }
                    else
                    {
                        tableData.V13.Add(Convert.ToDouble(dr["V13"]));
                    }

                    ps = dr["V23"];
                    if (ps == DBNull.Value)
                    {
                        tableData.V23.Add(tableData.V23.LastOrDefault());
                    }
                    else
                    {
                        tableData.V23.Add(Convert.ToDouble(dr["V23"]));
                    }

                    ps = dr["VoltageTrip."];
                    if (ps == DBNull.Value)
                    {
                        tableData.voltageTrip.Add(tableData.voltageTrip.LastOrDefault());
                    }
                    else
                    {
                        tableData.voltageTrip.Add(Convert.ToDouble(dr["VoltageTrip."]));
                    }

                    ps = dr["I1."];
                    if (ps == DBNull.Value)
                    {
                        tableData.I1.Add(tableData.I1.LastOrDefault());
                    }
                    else
                    {
                        tableData.I1.Add(Convert.ToDouble(dr["I1."]));
                    }

                    ps = dr["I2."];
                    if (ps == DBNull.Value)
                    {
                        tableData.I2.Add(tableData.I2.LastOrDefault());
                    }
                    else
                    {
                        tableData.I2.Add(Convert.ToDouble(dr["I2."]));
                    }

                    ps = dr["I3."];
                    if (ps == DBNull.Value)
                    {
                        tableData.I3.Add(tableData.I3.LastOrDefault());
                    }
                    else
                    {
                        tableData.I3.Add(Convert.ToDouble(dr["I3."]));
                    }

                    ps = dr["CurrentTrip."];
                    if (ps == DBNull.Value)
                    {
                        tableData.currentTrip.Add(tableData.currentTrip.LastOrDefault());
                    }
                    else
                    {
                        tableData.currentTrip.Add(Convert.ToDouble(dr["CurrentTrip."]));
                    }

                    ps = dr["Frequency."];
                    if (ps == DBNull.Value)
                    {
                        tableData.frequency.Add(tableData.frequency.LastOrDefault());
                    }
                    else
                    {
                        tableData.frequency.Add(Convert.ToDouble(dr["Frequency."]));
                    }

                    ps = dr["PKVA."];
                    if (ps == DBNull.Value)
                    {
                        tableData.pkva.Add(tableData.pkva.LastOrDefault());
                    }
                    else
                    {
                        tableData.pkva.Add(Convert.ToDouble(dr["PKVA."]));
                    }

                    ps = dr["PKVAR."];
                    if (ps == DBNull.Value)
                    {
                        tableData.pkvar.Add(tableData.pkvar.LastOrDefault());
                    }
                    else
                    {
                        tableData.pkvar.Add(Convert.ToDouble(dr["PKVAR."]));
                    }

                    ps = dr["PKW."];
                    if (ps == DBNull.Value)
                    {
                        tableData.pkw.Add(tableData.pkw.LastOrDefault());
                    }
                    else
                    {
                        tableData.pkw.Add(Convert.ToDouble(dr["PKW."]));
                    }

                    ps = dr["Remote."];
                    if (ps == DBNull.Value)
                    {
                        tableData.remoteControll.Add(tableData.remoteControll.LastOrDefault());
                    }
                    else
                    {
                        tableData.remoteControll.Add(Convert.ToDouble(dr["Remote."]));
                    }

                    ps = dr["TimeSchedule."];
                    if (ps == DBNull.Value)
                    {
                        tableData.schedulingStatus.Add(1.0);
                    }
                    else
                    {
                        tableData.schedulingStatus.Add(Convert.ToDouble(dr["TimeSchedule."]));
                    }

                    ps = dr["Manual"];
                    if (ps == DBNull.Value)
                    {
                        tableData.manualStatus.Add(tableData.manualStatus.LastOrDefault());
                    }
                    else
                    {
                        tableData.manualStatus.Add(Convert.ToDouble(dr["Manual"]));
                    }

                    ps = dr["PrimingLevel"];
                    if (ps == DBNull.Value)
                    {
                        tableData.primingTankLevel.Add(tableData.primingTankLevel.LastOrDefault());
                    }
                    else
                    {
                        tableData.primingTankLevel.Add(Convert.ToDouble(dr["PrimingLevel"]));
                    }

                    ps = dr["Pressure(Bar)"];
                    if (ps == DBNull.Value)
                    {
                        tableData.pressure.Add(tableData.pressure.LastOrDefault());
                    }
                    else
                    {
                        tableData.pressure.Add(Convert.ToDouble(dr["Pressure(Bar)"]));
                    }
                    ps = dr["tim"];
                    if (ps == DBNull.Value)
                    {
                        tableData.LogTime.Add(tableData.LogTime.LastOrDefault());
                    }
                    else
                    {
                        tableData.LogTime.Add(dr["tim"].ToString());
                    }
                    object value = dr["vib_x"];
                    if (value == DBNull.Value)
                    {
                        tableData.Vibration_m_s_2.Add(0);
                        tableData.Vibration_m_s.Add(0);
                        tableData.Vibration_m.Add(0);
                    }
                    else
                    {
                        tableData.Vibration_m_s_2.Add(Convert.ToDouble(dr["vib_x"]));
                        tableData.Vibration_m_s.Add(Convert.ToDouble(dr["vib_y"]));
                        tableData.Vibration_m.Add(Convert.ToDouble(dr["vib_z"]));
                    }

                }
                if (spellDataList.Count == 0)
                {
                    tableData.workingHoursToday = "0 Hours, 0 Minutes";
                    tableData.accWaterDischargePerDay = "0 Cusecs";
                    tableData.workingHoursTodayManual = "0";
                    tableData.workingHoursTodayRemote = "0";
                    tableData.workingHoursTodayScheduling = "0";
                    tableData.WorkingInHoursManual = 0;
                    tableData.WorkingInHoursRemote = 0;
                    tableData.WorkingInHoursScheduling = 0;
                    tableData.logDate = ftf.ToShortDateString();
                }
                else
                {
                    var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                    int phour = (int)pp.TotalHours;
                    int pmin = (int)pp.Minutes;
                    int psec = (int)pp.Seconds;
                    string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                    tableData.workingHoursToday = pstr;
                    //TimeSpan pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                    //int phour = pp.Hours;
                    //int pmin = pp.Minutes;
                    //int psec = pp.Seconds;
                    //string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                    //tableData.workingHoursToday = pstr;
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                    tableData.accWaterDischargePerDay = (((Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)) / 60) * 102) * avgWaterFlow).ToString();
                    tableData.WorkingInHoursManual = spellDataList.Where(r => r.spellMode == 1).Sum(i => i.spellPeriod);
                    tableData.WorkingInHoursRemote = spellDataList.Where(r => r.spellMode == 2).Sum(i => i.spellPeriod);
                    tableData.WorkingInHoursScheduling = spellDataList.Where(r => r.spellMode == 3).Sum(i => i.spellPeriod);
                    tableData.workingHoursTodayManual = minutesToTime(tableData.WorkingInHoursManual);
                    tableData.workingHoursTodayRemote = minutesToTime(tableData.WorkingInHoursRemote);
                    tableData.workingHoursTodayScheduling = minutesToTime(tableData.WorkingInHoursScheduling);
                    tableData.logDate = Convert.ToDateTime(currentTime).Date.ToShortDateString();
                }
            }
            tableData.WorkingInHours = (Convert.ToDouble(tableData.WorkingInHoursManual) +
                Convert.ToDouble(tableData.WorkingInHoursRemote) +
                Convert.ToDouble(tableData.WorkingInHoursScheduling)) / 60;
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

        public string RecyclingPlantCurrentDayReport()
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
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour,0,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID =  " + resourceID + "  and ";
                        Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                            RecylcingPlantClass sd = getAllSpellsForRecylcingPump1(Dashdt);
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
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour,0,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID =  " + resourceID + "  and ";
                        Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                        Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                            RecylcingPlantClass sd = getAllSpellsForRecylcingPump2(Dashdt);
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
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour,0,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID =  " + resourceID + "  and ";
                        Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                        Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                            RecylcingPlantClass sd = getAllSpellsForRecylcingPump3(Dashdt);
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
            /////////////////////////////////////////////////////////////////////////////////
            var json = new JavaScriptSerializer().Serialize(recycleDataList);
            return json;
        }

        public string RecyclingPlantLastDayReport()
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
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour,0,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID =  " + resourceID + "  and ";
                        Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                        Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                            RecylcingPlantClass sd = getAllSpellsForRecylcingPump1(Dashdt);
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
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour,0,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID =  " + resourceID + "  and ";
                        Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                        Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                            RecylcingPlantClass sd = getAllSpellsForRecylcingPump2(Dashdt);
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
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour,0,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID =  " + resourceID + "  and ";
                        Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(DAY,-1,GETDATE())), 0) and ";
                        Dashdtquery += "InsertionDateTime < DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
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
                            RecylcingPlantClass sd = getAllSpellsForRecylcingPump3(Dashdt);
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
            /////////////////////////////////////////////////////////////////////////////////
            var json = new JavaScriptSerializer().Serialize(recycleDataList);
            return json;
        }

        public RecylcingPlantClass getAllSpellsForRecylcingPump1(DataTable dt)
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
                                spelldata.spellPeriod = Math.Abs(Math.Floor((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes));
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
                    spelldata.spellPeriod = Math.Abs(Math.Floor((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes));
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
                tableData.WorkingHoursPump1 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes), 2);

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
                tableData.WorkingHoursPump1 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes), 2);

            }
            return tableData;
        }


        public RecylcingPlantClass getAllSpellsForRecylcingPump2(DataTable dt)
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
                                spelldata.spellPeriod = Math.Abs(Math.Floor((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes));
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
                                spelldata.spellPeriod = Math.Abs(Math.Floor((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes));
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
                tableData.WorkingHoursPump2 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes), 2);

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
                tableData.WorkingHoursPump2 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes), 2);

            }
            return tableData;
        }


        public RecylcingPlantClass getAllSpellsForRecylcingPump3(DataTable dt)
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
                                spelldata.spellPeriod = Math.Abs(Math.Floor((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes));
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
                    spelldata.spellPeriod = Math.Abs(Math.Floor((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes));
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
                tableData.WorkingHoursPump3 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes), 2);

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
                tableData.WorkingHoursPump3 = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes), 2);

            }
            return tableData;
        }
        
        public string getPondingTableListUpdated2()
        {
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var spellDataList = new List<SpellData>();
            int resourceID = 0;
            string resourceLocation = "";
            var tablList = new List<PondingTableClass>();
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    //getting resources belonging to ponding points
                    string getResFromTemp = "";
                    //if (Convert.ToInt32(Session["CompanyID"]) == 1)
                    //{
                    //    getResFromTemp = " select r.resourceID, r.resourceNumber, r.resourceLocationName from tblResource r inner join tblResourceType rt on r.resourceTypeID = rt.resourceTypeID where rt.resourceTypeName = 'Ponding Points' order by CAST(resourceNumber as int) ASC ";
                    //}
                    //else
                    //{
                    //    getResFromTemp = " select r.resourceID, r.resourceNumber, r.resourceLocationName from tblResource r inner join tblResourceType rt on r.resourceTypeID = rt.resourceTypeID where rt.resourceTypeName = 'Ponding Points' and r.managedBy = " + Convert.ToInt32(Session["UserID"]) + " order by CAST(resourceNumber as int) ASC ";
                    //}
                    getResFromTemp = " select r.resourceID, r.resourceNumber, r.resourceLocationName from tblResource r inner join tblResourceType rt on r.resourceTypeID = rt.resourceTypeID where rt.resourceTypeName = 'Ponding Points' order by CAST(resourceNumber as int) ASC ";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    int ite = 0;
                    //iterate through the list of resources within the ponding points
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        //getting resourceID 
                        resourceID = Convert.ToInt32(drRes["resourceID"]);
                        //getting resourceLocation 
                        resourceLocation = drRes["resourceLocationName"].ToString();
                        //query will get the list of data available against given resourceID (latest first)
                        string Dashdtquery = ";WITH cte AS ( SELECT DISTINCT r.resourceName AS Location, '3' AS Min_Level, s.parameterValue AS Current_Level, p.parameterUnit as pUnit, r.minThr as minTh, r.maxThr as maxTh, r.resourceNumber as rnum, DATEDIFF(minute, s.sheetInsertionDateTime, GETDATE ()) as DeltaMinutes, s.sheetInsertionDateTime as tim,  ROW_NUMBER() OVER (PARTITION BY s.resourceID ORDER BY s.sheetInsertionDateTime DESC) AS rn FROM tblSheet s inner join tblResource r on s.resourceID = r.resourceID inner join tblParameter p on s.parameterID = p.parameterID inner join tblResourceType rt on r.resourceTypeID = rt.resourceTypeID where r.resourceID = " + resourceID + " ) SELECT * FROM cte order by cast(rnum as INT) ASC, tim DESC ";
                        SqlCommand cmd = new SqlCommand(Dashdtquery, conn);
                        SqlDataAdapter sda = new SqlDataAdapter(Dashdtquery, conn);
                        Dashdt.Clear();
                        sda.Fill(Dashdt);
                        if (Dashdt.Rows.Count > 0)
                        {
                            PondingTableClass sd = getFirstSpells(Dashdt, Convert.ToInt32(drRes["resourceNumber"]));
                            tablList.Add(sd);
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
            var json = new JavaScriptSerializer().Serialize(tablList);
            return json;
        }

        public PondingTableClass getFirstSpells(DataTable dt, int order)
        {
            var tableData = new PondingTableClass();
            var spelldata = new SpellData();
            //int resourceID = Convert.ToInt32(dt.Rows[0]["resourceID"]);
            string location = dt.Rows[0]["Location"].ToString();
            double currentPonding = Math.Round((Convert.ToDouble(dt.Rows[0]["Current_Level"]) / 2.54), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool startOfSpell = false;
            bool endOfSpell = false;
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["Current_Level"]) / 2.54), 2);
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (DeltaMinutes > -520)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentPonding < 1)
                    {
                        if (endOfSpell == false && startOfSpell == false)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    if (Math.Abs(((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes) <= 30)
                                    {

                                    }
                                    else
                                    {
                                        endOfSpell = true;
                                        startOfSpell = true;
                                        spelldata.SpellDataArray.Add(lastvalue);
                                        spelldata.SpellTimeArray.Add(lastTime);
                                        spelldata.SpellEndTime = currTime;
                                        clearaceTime = currTime;
                                    }
                                }

                            }
                            else
                            {
                                if (Math.Abs(((Convert.ToDateTime(currentTime)) - (Convert.ToDateTime(currTime))).TotalMinutes) <= 30)
                                {
                                    endOfSpell = true;
                                    spelldata.SpellDataArray.Add(currValue);
                                    spelldata.SpellTimeArray.Add(currTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }
                                else
                                {
                                    endOfSpell = true;
                                    spelldata.SpellDataArray.Add(currentPonding);
                                    spelldata.SpellTimeArray.Add(currentTime);
                                    spelldata.SpellEndTime = currentTime;
                                    clearaceTime = currentTime;
                                }

                            }
                        }
                        else if (endOfSpell == true && startOfSpell == false)
                        {
                            if (currValue < 1)
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (Math.Abs(((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes) <= 30)
                                {
                                    spelldata.SpellStartTime = currentTime;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = lastTime;
                                }
                                startOfSpell = true;
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (Math.Abs(((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes) <= 30)
                                {
                                    spelldata.SpellDataArray.Add(currValue);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        else if (endOfSpell == true && startOfSpell == true)
                        {

                        }
                        else if (endOfSpell == false && startOfSpell == true)
                        {

                        }

                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (endOfSpell == false && startOfSpell == false)
                        {
                            if (currValue < 1)
                            {

                            }
                            else
                            {
                                endOfSpell = true;
                                spelldata.SpellDataArray.Add(currValue);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                            }
                        }
                        else if (endOfSpell == true && startOfSpell == false)
                        {
                            if (currValue < 1)
                            {
                                startOfSpell = true;
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (Math.Abs(((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes) <= 30)
                                {
                                    spelldata.SpellStartTime = currentTime;
                                    clearaceTime = currentTime;
                                }
                                else
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    clearaceTime = lastTime;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (Math.Abs(((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes) <= 30)
                                {
                                    spelldata.SpellDataArray.Add(currValue);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        else if (endOfSpell == true && startOfSpell == true)
                        {

                        }
                        else if (endOfSpell == false && startOfSpell == true)
                        {

                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }

            }
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentPonding);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (DeltaMinutes > -520)
            {
                tableData.deltaTime = DeltaMinutes.ToString(); ;
                tableData.clearanceTime = "-";
                tableData.comment = "Inactive";
                tableData.currLevel = "-";
                tableData.currTime = "-";
                tableData.estClTime = "-";
                tableData.flowRateDown = "-";
                tableData.flowRateUp = "-";
                tableData.maxLevel = "-";
                tableData.maxLevelTime = "-";
                tableData.pondingPeriod = "-";
                tableData.pondingLocation = location;
                tableData.srNo = order.ToString();
                tableData.minThr = (dt.Rows[0]["minTh"]).ToString();
                tableData.maxThr = (dt.Rows[0]["maxTh"]).ToString();
            }
            else
            {
                tableData.minThr = (dt.Rows[0]["minTh"]).ToString();
                tableData.maxThr = (dt.Rows[0]["maxTh"]).ToString();
                tableData.deltaTime = DeltaMinutes.ToString();
                string pondingPeriod = "";
                int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                tableData.srNo = order.ToString();
                tableData.pondingLocation = location;
                tableData.currLevel = currentPonding.ToString();
                tableData.currTime = currentTime;
                tableData.maxLevel = spelldata.SpellDataArray.DefaultIfEmpty().Max().ToString();
                tableData.maxLevelTime = spelldata.spellMaxTime;
                double pondinPeriod = 10;
                if (currentPonding < 1)
                {

                    pondinPeriod = Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                    TimeSpan runningTime = TimeSpan.FromMinutes(Convert.ToDouble(pondinPeriod));
                    int hour = runningTime.Hours;
                    int min = runningTime.Minutes;
                    int sec = runningTime.Seconds;
                    string str = " " + hour.ToString() + " Hours, " + min.ToString() + " Minutes";
                    pondingPeriod = str;
                    tableData.pondingPeriod = pondingPeriod;
                    tableData.clearanceTime = currentTime;
                    double DiffForFu = ((Convert.ToDateTime(spelldata.SpellStartTime)) - (Convert.ToDateTime(spelldata.spellMaxTime))).TotalMinutes;
                    if (DiffForFu == 0)
                    {
                        DiffForFu = 1;
                    }
                    double DiffForFd = ((Convert.ToDateTime(spelldata.spellMaxTime)) - (Convert.ToDateTime(spelldata.SpellEndTime))).TotalMinutes;
                    if (DiffForFd == 0)
                    {
                        DiffForFd = 1;
                    }
                    double fu = Math.Round((Convert.ToDouble(tableData.maxLevel) / DiffForFu), 2);
                    double fd = Math.Abs(Math.Round((Convert.ToDouble(tableData.maxLevel) / DiffForFd), 2));
                    tableData.flowRateUp = fu.ToString();
                    tableData.flowRateDown = fd.ToString();
                    if (Convert.ToInt32(pondinPeriod) < 1)
                    {
                        tableData.pondingPeriod = "-";
                        tableData.flowRateUp = "-";
                        tableData.flowRateDown = "-";
                        tableData.estClTime = "-";
                        tableData.comment = "No Ponding";
                        tableData.clearanceTime = "-";
                    }
                    else
                    {
                        tableData.estClTime = "Cleared";
                        tableData.comment = "Cleared";
                    }
                }
                else
                {
                    pondingPeriod = "-";
                    tableData.comment = "Continue...";
                    tableData.pondingPeriod = pondingPeriod;
                    tableData.clearanceTime = "-";
                    if (currentPonding < spelldata.SpellDataArray.DefaultIfEmpty().Max())
                    {
                        double DiffForFd = ((Convert.ToDateTime(spelldata.spellMaxTime)) - (Convert.ToDateTime(spelldata.SpellEndTime))).TotalMinutes;
                        if (DiffForFd == 0)
                        {
                            DiffForFd = 1;
                        }
                        double fd = Math.Abs(Math.Round((Convert.ToDouble(tableData.maxLevel) / DiffForFd), 2));
                        tableData.flowRateUp = "-";
                        tableData.flowRateDown = fd.ToString();
                        double minutesToClear = (currentPonding / fd);
                        TimeSpan runningTime = TimeSpan.FromMinutes(Convert.ToDouble(minutesToClear));
                        int hour = runningTime.Hours;
                        int min = runningTime.Minutes;
                        int sec = runningTime.Seconds;
                        string str = " " + hour.ToString() + " Hours, " + min.ToString() + " Minutes";
                        tableData.estClTime = str;
                    }
                    else
                    {
                        double DiffForFu = ((Convert.ToDateTime(spelldata.SpellStartTime)) - (Convert.ToDateTime(spelldata.spellMaxTime))).TotalMinutes;
                        if (DiffForFu == 0)
                        {
                            DiffForFu = 1;
                        }
                        double fu = Math.Round((Convert.ToDouble(tableData.maxLevel) / DiffForFu), 2);
                        tableData.flowRateUp = fu.ToString();
                        tableData.flowRateDown = "-";
                        tableData.estClTime = "In Progress";
                    }
                }
            }
            return tableData;
        }

        public string getRainTableList()
        {
            var tablList = new List<RainTableClass>();
            DataTable Dashdt = new DataTable();
            string Dashdtquery = ";WITH cte AS ( SELECT r.resourceName AS Location, '3' AS Min_Level, s.parameterValue AS Current_Level, s.sheetInsertionDateTime AS tim, p.parameterUnit as pUnit, CONVERT(float,s.parameterValue - LEAD(s.parameterValue) OVER (ORDER BY s.sheetInsertionDateTime DESC))/10 AS FLOW_RATE, r.resourceNumber as rnum, s.parameterValue/(CONVERT(float,s.parameterValue - LEAD(s.parameterValue) OVER (ORDER BY s.sheetInsertionDateTime DESC))/10+0.0001) AS EstimatedTime, ROW_NUMBER() OVER (PARTITION BY s.resourceID ORDER BY s.sheetInsertionDateTime DESC) AS rn FROM tblSheet s inner join tblResource r on s.resourceID = r.resourceID inner join tblParameter p on s.parameterID = p.parameterID inner join tblResourceType rt on r.resourceTypeID = rt.resourceTypeID where rt.resourceTypeName = 'Rain Guages' ) SELECT * FROM cte WHERE rn = 1  order by cast(rnum as INT) ASC ";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionRMS"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(Dashdtquery, conn);
                    SqlDataAdapter sda = new SqlDataAdapter(Dashdtquery, conn);
                    sda.Fill(Dashdt);
                    foreach (DataRow dr in Dashdt.Rows)
                    {
                        var tableclass = new RainTableClass();
                        tableclass.srNo = Convert.ToInt32(Dashdt.Rows.IndexOf(dr) + 1).ToString();
                        tableclass.pondingLocation = dr["Location"].ToString();
                        tableclass.currLevel = Math.Round(Convert.ToDouble(dr["Current_Level"])).ToString();
                        tableclass.pUnit = dr["pUnit"].ToString();
                        tableclass.RecTime = dr["tim"].ToString();
                        if (Convert.ToDouble(dr["FLOW_RATE"]) > 0)
                        {
                            tableclass.flowRateUp = Math.Abs(Math.Round((Convert.ToDouble(dr["FLOW_RATE"])), 1)).ToString();
                            tableclass.flowRateDown = "-";
                        }
                        else
                        {
                            tableclass.flowRateDown = Math.Abs(Math.Round((Convert.ToDouble(dr["FLOW_RATE"])), 1)).ToString();
                            tableclass.flowRateUp = "-";
                        }
                        tablList.Add(tableclass);
                    }
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            var json = new JavaScriptSerializer().Serialize(tablList);
            return json;
        }


    }
}