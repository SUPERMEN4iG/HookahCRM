using HookahCRM.DataModel;
using HookahCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HookahCRM.Controllers
{
    public class AdditionController : BaseApiController
    {
        [ActionName("List")]
        public IEnumerable<D_Addition> Get()
        {
            IList<D_Addition> listAdditions = _session.QueryOver<D_Addition>().List();

            return listAdditions;
        }
    }
}