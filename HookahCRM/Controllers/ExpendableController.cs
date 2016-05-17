using HookahCRM.DataModel;
using HookahCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HookahCRM.Controllers
{
    [Authorize]
    public class ExpendableController : BaseApiController
    {
        [ActionName("ActiveExpendableList")]
        public IEnumerable<ExpendableModel> Get()
        {
            IEnumerable<ExpendableModel> listExpendables = _session.QueryOver<D_Expendable>().List().Select(x => { return new ExpendableModel().Bind(x); }).ToList();

            return listExpendables;
        }
    }
}