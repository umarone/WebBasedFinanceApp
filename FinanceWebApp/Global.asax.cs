using log4net;
using MudasirRehmanAlp.AppDefinitions.ErrorsSettings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MudasirRehmanAlp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(MvcApplication));      //type of class
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();
            SqlDependency.Start(ConfigurationManager.ConnectionStrings["AppEntities"].ConnectionString);

        }
        protected void Application_End()
        {
            SqlDependency.Stop(ConfigurationManager.ConnectionStrings["AppEntities"].ConnectionString);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            string violationMessage = "";


            RouteData routeData = new RouteData();
            routeData.Values["controller"] = "Errors";
            routeData.Values["requestedUrl"] = Request.Url.OriginalString;

            if (httpException == null)
            {
                var message = exception.Message;
                var innerException = exception.InnerException;
                while (innerException != null)
                {
                    message = innerException.Message;
                    innerException = innerException.InnerException;
                }
                bool PrimaryKey = message.Contains("Violation of PRIMARY KEY");
                bool ForginKey = message.Contains("REFERENCE");
                bool UniqueKey = message.Contains("UNIQUE KEY");
                if (PrimaryKey || UniqueKey)
                {
                    violationMessage = "This Record is already added in Database.";
                }
                else
                {
                    string[] arr = message.Split('.');
                    if (arr.Length > 0)
                    {
                        violationMessage = arr[0];
                    }
                }

                log.Error("Exception=========>>>>>>>> "+ violationMessage);
                /* return; *///We ignore the rest of the method
                             //TODO: custom errors handling
                routeData.Values.Add("action", "Exception");


            }
            else
            {
                switch (httpException.GetHttpCode())
                {
                    case 400:
                        routeData.Values.Add("action", "Http400");
                        break;
                    case 404:                       
                        routeData.Values.Add("action", "Http404");
                        break;
                    case 500:                       
                        routeData.Values.Add("action", "Http500");
                        break;
                    default:
                        return;
                }
            }


            HttpContext httpContext = ((MvcApplication)sender).Context;
            httpContext.Response.Clear();
            httpContext.Response.ContentType = "text/html";
            httpContext.Response.TrySkipIisCustomErrors = true;
            httpContext.Response.StatusCode = httpException != null ? httpException.GetHttpCode() : 500;
            Server.ClearError();

            // Call target Controller and pass the routeData.
            IController errorController = new ErrorsController();
            try
            {
                errorController.Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
            }
            catch (Exception ex)
            {
                //log.Info("Error in Global.asax: Application_Error():");
                //log.Info(ex.StackTrace);

                string err = "<b>Error Caught in Global.asax: Application_Error() event</b><hr><br>" +
                        "<br><b>Error in: </b>" + Request.Url.ToString() +
                        "<br><b>Error Message: </b>" + ex.Message.ToString() +
                        "<br><b>Stack Trace:</b><br>" + ex.StackTrace.ToString();
                Response.Write(err.ToString());
            }
        }

    }
}
