using HookahCRM.DataModel;
using HookahCRM.Lib.Filters;
using HookahCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HookahCRM.Controllers
{
    [BasicAuthorize(typeof(D_WorkerRole), typeof(D_TraineeRole), typeof(D_AdministratorRole))]
    public class AdditionController : BaseApiController
    {
        [ActionName("List")]
        public IList<D_Addition> Get()
        {
            IList<D_Addition> listAdditions = _session.QueryOver<D_Addition>().List();

            return listAdditions;
        }
    }
}