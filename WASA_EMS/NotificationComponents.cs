using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WASA_EMS.Models;

namespace WASA_EMS
{
    public class NotificationComponent
    {//Here we will add a function for register notification (will add sql dependency)  
        public void RegisterNotification(DateTime currentTime)
        {
            string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string sqlCommand = @"SELECT [ID],[Comment] from [dbo].[tblNotifications] where [notificationTime] > @AddedOn";
            //you can notice here I have added table name like this [dbo].[Contacts] with [dbo], its mendatory when you use Sql Dependency  
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@AddedOn", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                //SqlDependency sqlDep = new SqlDependency(cmd);
                //sqlDep.OnChange += sqlDep_OnChange;
                //we must have to execute the command here  
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now  
                }
            }
        }

        //void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        //{
        //    //or you can also check => if (e.Info == SqlNotificationInfo.Insert) , if you want notification only for inserted record  
        //    if (e.Type == SqlNotificationType.Change)
        //    {
        //        SqlDependency sqlDep = sender as SqlDependency;
        //        sqlDep.OnChange -= sqlDep_OnChange;

        //        //from here we will send notification message to client  
        //        var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        //        notificationHub.Clients.All.notify("added");
        //        //re-register notification  
        //        RegisterNotification(DateTime.Now);
        //    }
        //}

        public List<tblNotification> GetData(DateTime afterDate)
        {
            using (WASA_EMS_Entities dc = new WASA_EMS_Entities())
            {
                return dc.tblNotifications.Where(a => a.notificationTime > afterDate).OrderByDescending(a => a.notificationTime).ToList();
            }
        }
    }
}