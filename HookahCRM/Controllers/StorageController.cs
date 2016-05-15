using HookahCRM.DataModel;
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
    public struct PutReportBlank
    {
        public long storageId { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> model { get; set; }
        public bool isClose { get; set; }
    }

    [Authorize]
    public class StorageController : BaseApiController
    {
        [ActionName("Current")]
        public StorageModel Get(long branchId)
        {
            return new StorageModel().Bind(
                    _session.QueryOver<D_Storage>()
                    .Where(x => x.Branch.Id == branchId)
                    .List().LastOrDefault()
                );
        }

        [ActionName("ReportBlank")]
        public IList<IStorageModel> Get(long? storageId, bool isClosed)
        {
            if (storageId == null)
                throw new NullReferenceException("storageId is null");

            IList<IStorageModel> listModels = new List<IStorageModel>();

            StorageHookahModel stHookahModel = new StorageHookahModel().Bind(
                    _session.QueryOver<D_StorageHookah>()
                    .Where(x => x.Storage.Id == storageId && x.CreationDateTime.Date == DateTime.Today)
                    .List().LastOrDefault()
                );

            StorageExpendableModel stExpendableModel = new StorageExpendableModel().Bind(
                    _session.QueryOver<D_StorageExpendable>()
                    .Where(x => x.Storage.Id == storageId && x.CreationDateTime.Date == DateTime.Today)
                    .List().LastOrDefault()
                );

            listModels.Add(stHookahModel);
            listModels.Add(stExpendableModel);

            return listModels;
        }

        [ActionName("ReportBlank")]
        [HttpPut]
        public HttpResponseMessage Put(PutReportBlank obj)
        {
            Dictionary<string, Dictionary<string, string>> hookah;
            obj.model.TryGetValue("0", out hookah);
            Dictionary<string, Dictionary<string, string>> expendable;
            obj.model.TryGetValue("1", out expendable);

            D_Storage d_storage = _session.QueryOver<D_Storage>()
                .List()
                .Where(x => x.Id == obj.storageId)
                .FirstOrDefault();

            D_User d_userCurrent = _session.QueryOver<D_User>()
                .Where(x => x.Login == User.Identity.Name)
                .List()
                .FirstOrDefault();

            StorageModel storageModel = new StorageModel().Bind(d_storage);

            StorageHookahModel stHookahModel = new StorageHookahModel();
            stHookahModel.StorageTobaccoList = new List<StorageHookahStyleModel>();
            stHookahModel.Worker = new UserModel().Bind(d_userCurrent);
            stHookahModel.IsClosed = obj.isClose;
            stHookahModel.StorageId = storageModel.Id;

            foreach (var tobacco in hookah)
            {
                foreach (var tobaccoStyle in tobacco.Value)
                {
                    stHookahModel.StorageTobaccoList.Add(new StorageHookahStyleModel() {
                        TobaccoStyle = new TobaccoStyleModel().Bind(_session.QueryOver<D_TobaccoStyle>().Where(x => x.Id == long.Parse(tobaccoStyle.Key)).List().FirstOrDefault()),
                        Weight = decimal.Parse(tobaccoStyle.Value)
                    });
                }
            }

            storageModel.StorageHookah.Add(stHookahModel);
            storageModel.Branch.StorageId = storageModel.Id;
            storageModel.Branch.Workers = new List<UserModel>();
            storageModel.Branch.Workers.Add(stHookahModel.Worker);

            d_storage.Worker = d_userCurrent;

            d_storage = storageModel.UnBind(d_storage);
            _session.SaveOrUpdate(d_storage);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}