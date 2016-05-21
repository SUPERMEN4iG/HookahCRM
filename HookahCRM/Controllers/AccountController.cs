using HookahCRM.DataModel;
using HookahCRM.Lib.Filters;
using HookahCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using HookahCRM.Lib.Helpers;

namespace HookahCRM.Controllers
{
    [BasicAuthorize(typeof(D_WorkerRole), typeof(D_TraineeRole), typeof(D_AdministratorRole))]
    public class AccountController : BaseApiController
    {
        [ActionName("user")]
        public UserModel Get()
        {
            UserModel user = new UserModel().Bind(
                _session.QueryOver<D_User>()
                .Where(x => x.Login == User.Identity.Name)
                .List()
                .Select(obj => ModelHelper.ParseToSmallVersion(obj))
                .FirstOrDefault());

            return user;
        }

        [ActionName("user")]
        public void Post(UserModel user)
        {
            if (user.Login != User.Identity.Name)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("You have no premission to change {0}", user.Login)),
                    ReasonPhrase = "No premissions to change"
                };

                throw new HttpResponseException(resp);
            }

            D_User d_user = user.UnBind();

            _session.SaveOrUpdate(d_user);
        }
    }
}