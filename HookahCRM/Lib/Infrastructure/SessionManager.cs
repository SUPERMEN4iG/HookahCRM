using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using HookahCRM.DataModel;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HookahCRM.Lib.Infrastructure
{
    public class SessionManager
    {
        public static string ConnectionString { get; set; }

        private readonly ISessionFactory sessionFactory;
        public static ISessionFactory SessionFactory
        {
            get { return Instance.sessionFactory; }
        }

        private ISessionFactory GetSessionFactory()
        {
            return sessionFactory;
        }

        public static SessionManager Instance
        {
            get
            {
                return NestedSessionManager.sessionManager;
            }
        }

        public static ISession OpenSession()
        {
            return Instance.GetSessionFactory().OpenSession();
        }

        public static ISession CurrentSession
        {
            get
            {
                return Instance.GetSessionFactory().GetCurrentSession();
            }
        }

        private SessionManager()
        {
            var msSqlConfigurator = MsSqlConfiguration.MsSql2012.ShowSql();

            if (String.IsNullOrEmpty(ConnectionString))
                msSqlConfigurator.ConnectionString(c => c.FromConnectionStringWithKey("DefaultConnection"));
            else
                msSqlConfigurator.ConnectionString(ConnectionString);

            sessionFactory = Fluently.Configure()
              .Database(msSqlConfigurator)
              .Mappings(m => m.FluentMappings.AddFromAssemblyOf<WebApiApplication>())
              .ExposeConfiguration(cfg =>
              {
                  cfg.Properties.Add("current_session_context_class", "web");
                  cfg.EventListeners.PreInsertEventListeners = new NHibernate.Event.IPreInsertEventListener[] { new PreInsertEvent() };
                  cfg.EventListeners.PreUpdateEventListeners = new NHibernate.Event.IPreUpdateEventListener[] { new PreUpdateEvent() };
                  new NHibernate.Tool.hbm2ddl.SchemaUpdate(cfg).Execute(true, true);
              })
              .BuildSessionFactory();
        }

        class NestedSessionManager
        {
            internal static readonly SessionManager sessionManager =
                new SessionManager();
        }
    }
}