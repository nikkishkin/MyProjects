using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using WorkWithJson.Auth;
using WorkWithJson.Controllers;
using WorkWithJson.DAL;

namespace WorkWithJson
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            ArtGalleryEntities context = new ArtGalleryEntities();

            HttpContext.Current.Items.Add(ExhibitionController.REQUEST_STORAGE_DBCONTEXT, context);
        }

        protected void Application_EndRequest()
        {
            ArtGalleryEntities context = (ArtGalleryEntities)HttpContext.Current.Items[ExhibitionController.REQUEST_STORAGE_DBCONTEXT];

            context.Dispose();
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                HttpCookie httpCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

                if (httpCookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(httpCookie.Value);

                    UserPrincipal userPrincipal = (UserPrincipal)Application[ticket.UserData];
                    if (userPrincipal != null)
                    {
                        UserPrincipal.CurrentPrincipal = userPrincipal;
                        return;
                    }
                }
            }

            UserPrincipal.CurrentPrincipal = UserPrincipal.Empty;
        }
    }
}