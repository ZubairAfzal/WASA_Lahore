using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

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
    }
}