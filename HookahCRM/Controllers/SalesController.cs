using HookahCRM.DataModel;
using HookahCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HookahCRM.Controllers
{
    public class SalesController : BaseApiController
    {
		//[ActionName("ActiveSalesList")]
		//public Dictionary<ActionType, List<DayHookahSaleModel>> Get()
		//{
		//	throw new NotImplementedException();

		//	//Dictionary<ActionType, List<DayHookahSaleModel>> listHookahSales = new Dictionary<ActionType, List<DayHookahSaleModel>>();
		//	//listHookahSales[ActionType.None] = new List<DayHookahSaleModel>();
		//	//listHookahSales[ActionType.FreeHookah] = new List<DayHookahSaleModel>();
		//	//IList<TobaccoModel> tobaccoList = _session.QueryOver<D_Tobacco>().List().Select(x => { return new TobaccoModel().Bind(x); }).ToList();

		//	//for (int i = 0; i < 2; i++)
		//	//{
		//	//	foreach (var item in tobaccoList)
		//	//	{
		//	//		listHookahSales[ActionType.None].Add(new DayHookahSaleModel()
		//	//		{
		//	//			ActionType = ActionType.None,
		//	//			Tobacco = item,
		//	//			Count = 0,

		//	//		});
		//	//	}
		//	//}

		//}
	}
}