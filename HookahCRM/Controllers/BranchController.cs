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
    public class BranchController : BaseApiController
    {
        [ActionName("ActiveBranchList")]
        public IEnumerable<BranchModel> Get()
        {
            return _session.QueryOver<D_Branch>()
                .List()
                .Where(x => x.IsDisabled == false)
                .Select(ob => { return new BranchModel().Bind(ob); })
                .ToList();
        }
    }
}
