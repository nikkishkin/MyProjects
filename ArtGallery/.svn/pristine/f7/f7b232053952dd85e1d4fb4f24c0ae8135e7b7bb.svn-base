using System;
using System.Web;

namespace WorkWithJson
{
    public class TracingModule: IHttpModule
    {
        // In the Init function, register for HttpApplication 
        // events by adding your handlers.
        public void Init(HttpApplication application)
        {
            application.BeginRequest +=
                (new EventHandler(this.Application_BeginRequest));
            application.EndRequest +=
                (new EventHandler(this.Application_EndRequest));
        }

        private void Application_BeginRequest(Object source,
             EventArgs e)
        {
            // Create HttpApplication and HttpContext objects to access
            // request and response properties.
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            context.Response.Write("Begin request " + DateTime.Now);
        }

        private void Application_EndRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            context.Response.Write("End request " + DateTime.Now);
        }

        public void Dispose()
        {
        }
    }
}