﻿using HookahCRM.DataModel;
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
    public class GetTobaccoStyleModelList
    {
        public int take { get; set; }
        public int skip { get; set; }
        public List<int> idList { get; set; }
    }

    public class TobaccoController : BaseApiController
    {

        [ActionName("TobaccoCategory")]
        public IList<TobaccoModel> Get()
        {
            List<TobaccoModel> objList = new List<TobaccoModel>();

            objList.AddRange(_session.QueryOver<D_Tobacco>()
                .List()
                .Select(x => { return new TobaccoModel().Bind(x); }));

            return objList;
        }

        [ActionName("TobaccoStyle")]
        public IList<TobaccoStyleModel> Get([FromUri]GetTobaccoStyleModelList objSend)
        {
            List<TobaccoStyleModel> objList = new List<TobaccoStyleModel>();

            objList.AddRange(_session.QueryOver<D_TobaccoStyle>()
                .Skip(objSend.skip).Take(objSend.take)
                .List()
                .Where(x => { return (objSend.idList == null) ? true : objSend.idList.Any(f => f == x.Tobacco.Id); })
                .Select(x => { return new TobaccoStyleModel().Bind(x); }));

            return objList;
        }

        [ActionName("TobaccoStyle")]
        [HttpPut]
        public HttpResponseMessage Put([FromBody]TobaccoStyleModel model)
        {
            D_TobaccoStyle d_tobacco = _session.QueryOver<D_TobaccoStyle>().Where(x => x.Id == model.Id).List().FirstOrDefault();

            if (d_tobacco != null)
            {
                d_tobacco = model.UnBind(d_tobacco);
            }
            else
            {
                d_tobacco = model.UnBind();
            }

            _session.SaveOrUpdate(d_tobacco);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [ActionName("TobaccoStyle")]
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            D_TobaccoStyle d_tobacco = _session.QueryOver<D_TobaccoStyle>().Where(x => x.Id == id).List().FirstOrDefault();

            if (d_tobacco != null)
            {
                _session.Delete(d_tobacco);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}