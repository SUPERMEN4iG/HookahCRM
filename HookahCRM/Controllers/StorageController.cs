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
        public long branchId { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> model { get; set; }
        public bool isClose { get; set; }
    }

    [Authorize]
    public class StorageController : BaseApiController
    {
        [ActionName("Current")]
        public StorageModel Get(long branchId)
        {
			StorageModel stModel = new StorageModel().Bind(
					_session.QueryOver<D_Storage>()
					.Where(x => x.Branch.Id == branchId)
					.List()
					.Select(x => {
						x.StorageExpendable = x.StorageExpendable.Where(d => d.CreationDateTime.Date == DateTime.Today).ToList();
						x.StorageHookah = x.StorageHookah.Where(d => d.CreationDateTime.Date == DateTime.Today).ToList();
						return x;
					})
					.LastOrDefault()
				);

			return stModel;
        }

        [ActionName("ReportBlank")]
        public IList<IReportBlankModel> Get(long? branchId, bool isClosed)
        {
            if (branchId == null)
                throw new NullReferenceException("storageId is null");

            IList<IReportBlankModel> listModels = new List<IReportBlankModel>();

            StorageHookahModel stHookahModel = new StorageHookahModel().Bind(
                    _session.QueryOver<D_Branch>()
                    .Where(x => x.Id == branchId)
                    .List().LastOrDefault()
                    .Storage.StorageHookah
                    .Where(x => x.CreationDateTime.Date == DateTime.Today)
                    .ToList()
                    .LastOrDefault()
                );

            StorageExpendableModel stExpendableModel = new StorageExpendableModel().Bind(
                    _session.QueryOver<D_Branch>()
                    .Where(x => x.Id == branchId)
                    .List().LastOrDefault()
                    .Storage.StorageExpendable
                    .Where(x => x.CreationDateTime.Date == DateTime.Today)
                    .ToList()
                    .LastOrDefault()
                );

            listModels.Add(stHookahModel);
            listModels.Add(stExpendableModel);

            if (isClosed)
            {
                DayHookahSaleModel dsHookahModel = new DayHookahSaleModel().Bind(
                    _session.QueryOver<D_Branch>()
                    .Where(x => x.Id == branchId)
                    .List().LastOrDefault()
                    .Sales.LastOrDefault().DayHoohahSales
                    .Where(x => x.CreationDateTime.Date == DateTime.Today)
                    .ToList()
                    .LastOrDefault()
                );

                DayAdditionSalesModel dsAdditionModel = new DayAdditionSalesModel().Bind(
                        _session.QueryOver<D_Branch>()
                        .Where(x => x.Id == branchId)
                        .List().LastOrDefault()
                        .Sales.LastOrDefault().DayAdditionSales
                        .Where(x => x.CreationDateTime.Date == DateTime.Today)
                        .ToList()
                        .LastOrDefault()
                );

                DayHookahSaleModel dsHookahModelAction = new DayHookahSaleModel().Bind(
                    _session.QueryOver<D_Branch>()
                    .Where(x => x.Id == branchId)
                    .List().LastOrDefault()
                    .Sales.LastOrDefault().DayHoohahSales
                    .Where(x => x.CreationDateTime.Date == DateTime.Today && x.ActionType == ActionType.FreeHookah)
                    .ToList()
                    .LastOrDefault()
                );

                DayAdditionSalesModel dsAdditionModelAction = new DayAdditionSalesModel().Bind(
                        _session.QueryOver<D_Branch>()
                        .Where(x => x.Id == branchId)
                        .List().LastOrDefault()
                        .Sales.LastOrDefault().DayAdditionSales
                        .Where(x => x.CreationDateTime.Date == DateTime.Today && x.ActionType == ActionType.FreeHookah)
                        .ToList()
                        .LastOrDefault()
                );

                listModels.Add(dsHookahModel);
                listModels.Add(dsAdditionModel);

                listModels.Add(dsHookahModelAction);
                listModels.Add(dsAdditionModelAction);
            }

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
                .Where(x => x.Branch.Id == obj.branchId)
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
            //storageModel.Branch.StorageId = storageModel.Id;
            //storageModel.Branch.Workers = new List<UserModel>();
            //storageModel.Branch.Workers.Add(stHookahModel.Worker);

            //d_storage.Worker = d_userCurrent;

            d_storage = storageModel.UnBind(d_storage);
            _session.SaveOrUpdate(d_storage);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}