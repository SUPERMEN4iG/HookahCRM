using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using HookahCRM.Lib.Infrastructure;
using NHibernate;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HookahCRM
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
            {
                Response.Flush();
            }

            CurrentSessionContext.Bind(SessionManager.SessionFactory.OpenSession());
            //HookahCRM.Application.Instance.Initiliaze();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            ISession session = CurrentSessionContext.Unbind(SessionManager.SessionFactory);
            if (session != null)
            {
                if (session.Transaction != null &&
                    session.Transaction.IsActive)
                {
                    session.Transaction.Rollback();
                }
                else
                    session.Flush();
                session.Close();
            }
        }

        public void Application_Error(object sender, EventArgs e)
        {
            ISession session = CurrentSessionContext.Unbind(SessionManager.SessionFactory);

            if (session != null)
            {
                try
                {
                    if (session.Transaction.IsActive)
                    {
                        session.Transaction.Rollback();
                    }
                }
                catch (Exception)
                {
                    // log exception
                }
            }
        }
    }
}
