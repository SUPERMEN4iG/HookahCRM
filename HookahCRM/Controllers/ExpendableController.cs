using HookahCRM.DataModel;
using HookahCRM.Lib.Filters;
using HookahCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HookahCRM.Controllers
{
    [BasicAuthorize(typeof(D_WorkerRole), typeof(D_TraineeRole), typeof(D_AdministratorRole))]
    public class ExpendableController : BaseApiController
    {
        [ActionName("ActiveExpendableList")]
        public IList<ExpendableModel> Get()
        {
            IList<ExpendableModel> listExpendables = _session.QueryOver<D_Expendable>().List().Select(x => { return new ExpendableModel().Bind(x); }).ToList();

            return listExpendables;
        }
    }
}