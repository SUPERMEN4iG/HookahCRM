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
    public struct PutReportBlank
    {
        public long branchId { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> model { get; set; }
        public bool isClose { get; set; }
    }

    [BasicAuthorize(typeof(D_WorkerRole), typeof(D_AdministratorRole))]
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
				//DayHookahSaleModel dsHookahModel = new DayHookahSaleModel().Bind(
				//    _session.QueryOver<D_Branch>()
				//    .Where(x => x.Id == branchId)
				//    .List().LastOrDefault()
				//    .Sales.LastOrDefault().DayHoohahSales
				//    .Where(x => x.CreationDateTime.Date == DateTime.Today)
				//    .ToList()
				//    .LastOrDefault()
				//);
				DayHookahSaleModel dsHookahModel = new DayHookahSaleModel();


				//DayAdditionSalesModel dsAdditionModel = new DayAdditionSalesModel().Bind(
				//                    _session.QueryOver<D_Branch>()
				//                    .Where(x => x.Id == branchId)
				//                    .List().LastOrDefault()
				//                    .Sales.LastOrDefault().DayAdditionSales
				//                    .Where(x => x.CreationDateTime.Date == DateTime.Today)
				//                    .ToList()
				//                    .LastOrDefault()
				//            );

				DayAdditionSalesModel dsAdditionModel = new DayAdditionSalesModel();

				//DayHookahSaleModel dsHookahModelAction = new DayHookahSaleModel().Bind(
				//                _session.QueryOver<D_Branch>()
				//                .Where(x => x.Id == branchId)
				//                .List().LastOrDefault()
				//                .Sales.LastOrDefault().DayHoohahSales
				//                .Where(x => x.CreationDateTime.Date == DateTime.Today && x.ActionType == ActionType.FreeHookah)
				//                .ToList()
				//                .LastOrDefault()
				//            );
				DayHookahSaleModel dsHookahModelAction = new DayHookahSaleModel();

				DayAdditionSalesModel dsAdditionModelAction = new DayAdditionSalesModel();


				//DayAdditionSalesModel dsAdditionModelAction = new DayAdditionSalesModel().Bind(
				//                    _session.QueryOver<D_Branch>()
				//                    .Where(x => x.Id == branchId)
				//                    .List().LastOrDefault()
				//                    .Sales.LastOrDefault().DayAdditionSales
				//                    .Where(x => x.CreationDateTime.Date == DateTime.Today && x.ActionType == ActionType.FreeHookah)
				//                    .ToList()
				//                    .LastOrDefault()
				//            );

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
            Dictionary<string, Dictionary<string, string>> hookahList;
            obj.model.TryGetValue("0", out hookahList);
            Dictionary<string, Dictionary<string, string>> expendableList;
            obj.model.TryGetValue("1", out expendableList);

			Dictionary<string, Dictionary<string, string>> salesList = new Dictionary<string, Dictionary<string, string>>();
			if (obj.model.ContainsKey("2"))
				obj.model.TryGetValue("2", out salesList);

			Dictionary<string, Dictionary<string, string>> salesActionList = new Dictionary<string, Dictionary<string, string>>();
			if (obj.model.ContainsKey("3"))
				obj.model.TryGetValue("3", out salesActionList);

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

            foreach (var tobacco in hookahList)
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

			StorageExpendableModel stExpendableModel = new StorageExpendableModel();
			stExpendableModel.StorageExpendableListCount = new List<StorageExpendableCountModel>();
			stExpendableModel.Worker = new UserModel().Bind(d_userCurrent);
			stExpendableModel.IsClosed = obj.isClose;
			stExpendableModel.StorageId = storageModel.Id;

			foreach (var expendable in expendableList)
			{
				foreach (var expendableValue in expendable.Value)
				{
					stExpendableModel.StorageExpendableListCount.Add(new StorageExpendableCountModel() {
						Expendable = new ExpendableModel().Bind(_session.QueryOver<D_Expendable>().Where(x => x.Id == long.Parse(expendableValue.Key)).List().FirstOrDefault()),
						Count = decimal.Parse(expendableValue.Value)
					});
				}
			}

			storageModel.StorageExpendable.Add(stExpendableModel);

            //storageModel.Branch.StorageId = storageModel.Id;
            //storageModel.Branch.Workers = new List<UserModel>();
            //storageModel.Branch.Workers.Add(stHookahModel.Worker);

            //d_storage.Worker = d_userCurrent;

			D_Sales d_sales = _session.QueryOver<D_Sales>()
				.List()
				.Where(x => x.Branch.Id == obj.branchId)
				.FirstOrDefault();

			SalesModel salesModel = new SalesModel().Bind(d_sales);

			if (salesList.Count != 0)
			{
				foreach (var item in salesList["0"])
				{
					salesModel.DayHookahSales.Add(new DayHookahSaleModel()
					{
						Tobacco = new TobaccoModel().Bind(_session.QueryOver<D_Tobacco>().Where(x => x.Id == long.Parse(item.Key)).List().FirstOrDefault()),
						Count = decimal.Parse(item.Value),
						ActionType = ActionType.None
					});
				}
			}

			if (salesActionList.Count != 0)
			{
				foreach (var item in salesActionList["0"])
				{
					salesModel.DayHookahSales.Add(new DayHookahSaleModel()
					{
						Tobacco = new TobaccoModel().Bind(_session.QueryOver<D_Tobacco>().Where(x => x.Id == long.Parse(item.Key)).List().FirstOrDefault()),
						Count = decimal.Parse(item.Value),
						ActionType = ActionType.FreeHookah
					});
				}
			}

			d_storage = storageModel.UnBind(d_storage);
			_session.SaveOrUpdate(d_storage);
			d_sales = salesModel.UnBind(d_sales);
			_session.SaveOrUpdate(d_sales);

			return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}