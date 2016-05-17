﻿using HookahCRM.DataModel;
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
    public class UsersController : BaseApiController
    {
        // GET api/<controller>
        [ActionName("List")]
        public IEnumerable<UserModel> Get()
        {
            IEnumerable<UserModel> listUsers = _session.QueryOver<D_User>().Take(10).List().Select(x => { return new UserModel().Bind(x); });

            return listUsers;
        }

        [ActionName("Repository")]
        [HttpPut]
        public HttpResponseMessage Put([FromBody]UserModel model)
        {
            D_User d_user = _session.QueryOver<D_User>().Where(x => x.Id == model.Id).List().FirstOrDefault();

            if (d_user != null)
            {
                d_user = model.UnBind(d_user);
            }
            else
            {
                d_user = model.UnBind();
            }

            _session.SaveOrUpdate(d_user);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}