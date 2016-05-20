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
    [BasicAuthorize(typeof(D_AdministratorRole), typeof(D_WorkerRole))]
    public class BranchController : BaseApiController
    {
        [ActionName("ActiveBranchList")]
        public IEnumerable<BranchModel> Get()
        {
            D_User d_currentUser = _session.QueryOver<D_User>().Where(x => x.Login == User.Identity.Name).List().FirstOrDefault();

            return d_currentUser.BranchList
                .Where(x => x.IsDisabled == false)
                .Select(o => { return new D_Branch() { Id = o.Id, IsDisabled = o.IsDisabled, Name = o.Name, Workers = o.Workers, Address = o.Address, Guid = o.Guid, CreationDateTime = o.CreationDateTime }; })
                .Select(ob => { return new BranchModel().Bind(ob); })
                .ToList();
        }
    }
}
