using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Infrastructure.Hubs;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.Infrastructure.AppServices
{
    public class NotificationService
    {
        public IEnumerable<NotificationViewModel> GetNotifications()
        {
            
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AppEntities"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(@"SELECT [Id],[Title],[Description]
                FROM [dbo].[Notification]", connection))
                    {
                        // Make sure the command object does not already have
                        // a notification object associated with it.
                        command.Notification = null;

                        SqlDependency dependency = new SqlDependency(command);
                        dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                        if (connection.State == ConnectionState.Closed)
                            connection.Open();

                        using (var reader = command.ExecuteReader())
                            return reader.Cast<IDataRecord>()
                                .Select(x => new NotificationViewModel()
                                {
                                    Id = x.GetInt32(0),
                                    Title = x.GetString(1),
                                    Description = x.GetString(2),
                                }).ToList();



                    }
                }
            
           
        }
        public List<NotificationViewModel> GetNotificationsAC(long organizationId,long BranchId)
        {
            List<NotificationViewModel> _listObj = new List<NotificationViewModel>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AppEntities"].ConnectionString;

                string commandText = null;

                using (var db = new AppEntities())
                {
                    var query = db.Notifications.Where(a => a.IsRead == false && a.OrganizationId == organizationId && a.BranchId == BranchId) as DbQuery<Notification>;

                    commandText = query.ToString();
                    commandText = commandText.Replace("@p__linq__0", organizationId.ToString()).Replace("@p__linq__1", BranchId.ToString());
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();

                        var sqlDependency = new SqlDependency(command);

                        sqlDependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                        // NOTE: You have to execute the command, or the notification will never fire.
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {

                                    NotificationViewModel obj = new NotificationViewModel();

                                    obj.Id = (long)reader["Id"];
                                    obj.OrganizationId = (long)reader["OrganizationId"];
                                    obj.BranchId = (long)reader["BranchId"];
                                    obj.Type = (NotificationType)reader["Type"];
                                    obj.Priority = (NotificationPriority)reader["Priority"];
                                    obj.Title = (string)reader["Title"];
                                    obj.Description = (string)reader["Description"];

                                    _listObj.Add(obj);
                                }

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            

            return _listObj;
        }
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            NotificationsHub.Show();
        }
    }
}