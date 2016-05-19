using HookahCRM.Lib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace HookahCRM.Controllers
{
    public class BaseApiController : ApiController
    {
        public NHibernate.ISession _session = SessionManager.CurrentSession;

        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            //_session = SessionManager.SessionFactory.OpenSession();
            //_session.BeginTransaction();

            //HookahCRM.Application.Instance.Initiliaze();
        }

        protected override void Dispose(bool disposing)
        {
            //_session.Transaction.Commit();
            //_session.Transaction.Dispose();
            //_session.Dispose();

            base.Dispose(disposing);
        }
    }

    public class BaseController : Controller
    {
        public NHibernate.ISession _session = SessionManager.CurrentSession;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            //_session = SessionManager.SessionFactory.OpenSession();
            //_session.BeginTransaction();

            HookahCRM.Application.Instance.Initiliaze();
        }

        protected override void Dispose(bool disposing)
        {
            //_session.Transaction.Commit();
            //_session.Transaction.Dispose();
            //_session.Dispose();

            base.Dispose(disposing);
        }
    }
}