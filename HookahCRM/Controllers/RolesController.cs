using HookahCRM.DataModel;
using HookahCRM.Lib.Filters;
using HookahCRM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace HookahCRM.Controllers
{
    public class RolesModel
    {
       public RoleType Role { get; set; }
       public String Name { get; set; }
    }

    [BasicAuthorize(typeof(D_WorkerRole), typeof(D_TraineeRole), typeof(D_AdministratorRole))]
    public class RolesController : BaseApiController
    {
        [ActionName("ActiveRolesList")]
        public IList<RolesModel> Get()
        {
            IList<RolesModel> listStrings = new List<RolesModel>();

            listStrings.Add(new RolesModel() { Role = RoleType.Banned, Name = RoleType.Banned.ToStringName() });
            listStrings.Add(new RolesModel() { Role = RoleType.Administrator, Name = RoleType.Administrator.ToStringName() });
            listStrings.Add(new RolesModel() { Role = RoleType.Worker, Name = RoleType.Worker.ToStringName() });
            listStrings.Add(new RolesModel() { Role = RoleType.Trainee, Name = RoleType.Trainee.ToStringName() });
            listStrings.Add(new RolesModel() { Role = RoleType.Manager, Name = RoleType.Manager.ToStringName() });

            return listStrings;
        }
    }
}