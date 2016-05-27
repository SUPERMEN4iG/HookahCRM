﻿using HookahCRM.DataModel;
using HookahCRM.Lib.Filters;
using HookahCRM.Lib.Helpers;
using HookahCRM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.IO;
using DocumentFormat.OpenXml.Packaging;

using DocumentFormat.OpenXml.Spreadsheet;
using System.IO.Packaging;
using System.Xml;
using DocumentFormat.OpenXml;
using HookahCRM.Lib.Excel;
using System.Net.Http.Headers;

namespace HookahCRM.Controllers
{
    public struct PutReportBlank
    {
        public long branchId { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> model { get; set; }
        public bool isClose { get; set; }
    }

    [BasicAuthorize(typeof(D_WorkerRole), typeof(D_TraineeRole), typeof(D_AdministratorRole))]
    public class StorageController : BaseApiController
    {
        private readonly string _TemplateFile = AppDomain.CurrentDomain.BaseDirectory + "Content\\Templates\\";
        //private readonly string _GeneratedDirectorySource = "Content\\GeneratedDocuments\\";
        private readonly string _GeneratedDirectoryFile = AppDomain.CurrentDomain.BaseDirectory + "Content\\GeneratedDocuments\\";

        [ActionName("GetReportBlank")]
        public HttpResponseMessage Get(long branchId, bool val)
        {
            //string destinationFile = Path.Combine(_GeneratedDirectoryFile, string.Format("GeneratedDocument_{0}_{1}.docx", DateTime.Today.Date.ToString("dd.MM.yyyy"), Guid.NewGuid()));
            //string sourceFile = Path.Combine(_TemplateFile, "ReportTemplate.docx");
            //try
            //{
            //    File.Copy(sourceFile, destinationFile, true);
            //    Package pkg = Package.Open(destinationFile, FileMode.Open, FileAccess.ReadWrite);

            //    Uri uri = new Uri("/word/document.xml", UriKind.Relative);
            //    PackagePart part = pkg.GetPart(uri);

            //    XmlDocument xmlMainXMLDoc = new XmlDocument();
            //    xmlMainXMLDoc.Load(part.GetStream(FileMode.Open, FileAccess.Read));

            //    xmlMainXMLDoc.InnerXml = ReplacePlaceHoldersInTemplate("123", xmlMainXMLDoc.InnerXml);

            //    // Open the stream to write document
            //    StreamWriter partWrt = new StreamWriter(part.GetStream(FileMode.Open, FileAccess.Write));
            //    //doc.Save(partWrt);
            //    xmlMainXMLDoc.Save(partWrt);

            //    partWrt.Flush();
            //    partWrt.Close();
            //    pkg.Close();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //finally
            //{ 
            //    //Console.WriteLine(“\nPress Enter to continue…”);
            //    //Console.ReadLine();
            //}
            D_Branch d_branch = _session.QueryOver<D_Branch>().Where(x => x.Id == branchId).List().LastOrDefault();
            IList<IReportBlankModel> stModel = this.Get(branchId, isClosed: false);
            IList<IReportBlankModel> stModel2 = this.Get(branchId, isClosed: true);
            List<TobaccoModel> tobaccoList = new List<TobaccoModel>();
            tobaccoList.GetTobaccoList(ref _session);

            StorageHookahModel stHookahModelBefore = stModel.ElementAt(0) as StorageHookahModel;
            StorageExpendableModel stExpendableModelBefore = stModel.ElementAt(1) as StorageExpendableModel;
            StorageHookahModel stHookahModelAfter = stModel2.ElementAt(0) as StorageHookahModel;
            StorageExpendableModel stExpendableModelAfter = stModel2.ElementAt(1) as StorageExpendableModel;

            Dictionary<string, Dictionary<string, decimal>> resultDictionary = new Dictionary<string, Dictionary<string, decimal>>();

            foreach (var item in tobaccoList)
            {
                var listTobaccoStyle = new Dictionary<string, decimal>();

                foreach (var itemStyle in item.TobaccoList)
                {
                    var before = stHookahModelBefore.StorageTobaccoList.Where(x => x.TobaccoStyle.Id == itemStyle.Id).LastOrDefault();
                    var after = stHookahModelAfter.StorageTobaccoList.Where(x => x.TobaccoStyle.Id == itemStyle.Id).LastOrDefault();

                    listTobaccoStyle.Add(itemStyle.Name, after.Weight - before.Weight);
                }

                resultDictionary.Add(item.Name, listTobaccoStyle);
            }

            string realetivePath = string.Format("GeneratedDocument_{0}_{1}.xlsx", DateTime.Today.Date.ToString("dd.MM.yyyy"), Guid.NewGuid());
            string destinationFile = Path.Combine(_GeneratedDirectoryFile, realetivePath);
            
            ReportStorage newReport = new ReportStorage();
            newReport.BranchName = d_branch.Name;
            newReport.CreationDateTime = stHookahModelAfter.CreationDateTime.Date.ToString("dd.MM.yyyy");
            newReport.FIO = stHookahModelBefore.Worker.GetFullName();
            newReport.ResultDictionary = resultDictionary;
            byte[] memArray = newReport.CreatePackage(destinationFile);

			HttpResponseMessage result = null;
			try
			{
				result = Request.CreateResponse(HttpStatusCode.OK);
				result.Content = new ByteArrayContent(memArray);
				result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
				result.Content.Headers.ContentDisposition.FileName = realetivePath;

				return result;
			}
			catch (Exception)
			{
				return Request.CreateResponse(HttpStatusCode.Gone);
			}

			//var path = System.Web.HttpContext.Current.Server.MapPath("~/Content/GeneratedDocuments/" + realetivePath);
			//HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
			//var stream = new FileStream(path, FileMode.Open);
			//result.Content = new StreamContent(stream);
			//result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
			//result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(path);
			//result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
			//result.Content.Headers.ContentLength = stream.Length;
			//return result;   

			//try
			//{
			//    using (SpreadsheetDocument document = SpreadsheetDocument.Open(destinationFile, true))
			//    {
			//        WorkbookPart workbookPart = document.AddWorkbookPart();
			//        workbookPart.Workbook = new Workbook();

			//        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
			//        worksheetPart.Worksheet = new Worksheet(new SheetData());

			//        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
			//        Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Расход за день" };
			//        SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

			//        SharedStringTablePart sharedStringTablePart1 = workbookPart.AddNewPart<SharedStringTablePart>("rId4");
			//        GenerateSharedStringTablePart1Content(sharedStringTablePart1);

			//        //Cell refCell = null;
			//        //foreach (Cell cell in row.Elements<Cell>())
			//        //{
			//        //    if (string.Compare(cell.CellReference.Value, "B1", true) > 0)
			//        //    {
			//        //        refCell = cell;
			//        //        break;
			//        //    }
			//        //}

			//        //for (uint i = 1; i <= 10; i++)
			//        //{
			//        //    Row row;
			//        //    row = new Row() { RowIndex = i };
			//        //    sheetData.Append(row);

			//        //    Cell newCell = new Cell() { CellReference = "B" + i };
			//        //    row.InsertAt(newCell, 0);
			//        //    //row.InsertAfter(newCell, refCell);
			//        //    newCell.CellValue = new CellValue((100 * i).ToString());
			//        //    newCell.DataType = new EnumValue<CellValues>(CellValues.Number);
			//        //}

			//        sheets.Append(sheet);

			//        workbookPart.Workbook.Save();
			//    }
			//}
			//catch (Exception)
			//{

			//    throw;
			//}
		}

        private string ReplacePlaceHoldersInTemplate(string toChange, string templateBody)
        {
            templateBody = templateBody.Replace("#FIO#", toChange);
            return templateBody;
        }

        //private void ReplaceWordStub(string stubReplace, string text, Word.Document wordDocument)
        //{
        //    var range = wordDocument.Content;
        //    range.Find.ClearFormatting();
        //    range.Find.Execute(FindText: stubReplace, ReplaceWith: text);
        //}

        [ActionName("Current")]
        public StorageModel Get(long branchId)
        {
            //StorageModel stModel = new StorageModel().Bind(
            //        _session.QueryOver<D_Storage>()
            //        .Where(x => x.Branch.Id == branchId)
            //        .List()
            //        .Select(x => 
            //        {
            //            x.StorageExpendable = x.StorageExpendable.Where(d => d.CreationDateTime.Date == DateTime.Today).Select(ste =>
            //            {
            //                return new D_StorageExpendable()
            //                {
            //                    Id = ste.Id,
            //                    IsDisabled = ste.IsDisabled,
            //                    IsClosed = ste.IsClosed,
            //                    Storage = ste.Storage,
            //                    Worker = ModelHelper.ParseToSmallVersion(ste.Worker),
            //                    StorageExpendableListCount = ste.StorageExpendableListCount
            //                    //StorageExpendableListCount = ste.StorageExpendableListCount.Select(stEList => 
            //                    //{
            //                    //    return new D_StorageExpendableCount() 
            //                    //    {
            //                    //        Id = stEList.Id,
            //                    //        Expendable = stEList.Expendable,
            //                    //        Count = stEList.Count
            //                    //    };
            //                    //}).ToList()
            //                };
            //            }).ToList();

            //            x.StorageHookah = x.StorageHookah.Where(d => d.CreationDateTime.Date == DateTime.Today).Select(sth => 
            //            {
            //                return new D_StorageHookah()
            //                {
            //                    Id = sth.Id,
            //                    IsDisabled = sth.IsDisabled,
            //                    IsClosed = sth.IsClosed,
            //                    Storage = sth.Storage,
            //                    Worker = ModelHelper.ParseToSmallVersion(sth.Worker),
            //                    StorageTobaccoList = sth.StorageTobaccoList
            //                    //StorageTobaccoList = sth.StorageTobaccoList.Select(stHList => 
            //                    //{
            //                    //    return new D_StorageHookahStyle()
            //                    //    {
            //                    //        Id = stHList.Id,
            //                    //        TobaccoStyle = stHList.TobaccoStyle,
            //                    //        Weight = stHList.Weight
            //                    //    };
            //                    //}).ToList()
            //                }; 
            //            }).ToList();

            //            return x;
            //        })
            //        .LastOrDefault()
            //    );

            D_Storage d_storage = _session.QueryOver<D_Storage>().Where(x => x.Branch.Id == branchId).List().LastOrDefault();

            IList<D_StorageExpendable> stExpedableList = _session
                .QueryOver<D_StorageExpendable>()
                .Where(x => x.Storage.Id == d_storage.Id)
                .List()
                .Select(ste => 
                {
                    return new D_StorageExpendable() 
                    {
                        Id = ste.Id,
                        IsClosed = ste.IsClosed,
                        CreationDateTime = ste.CreationDateTime,
                        Worker = ModelHelper.ParseToSmallVersion(ste.Worker),
                        Storage = new D_Storage() { Id = ste.Storage.Id, IsDisabled = ste.Storage.IsDisabled },
                        StorageExpendableListCount = new List<D_StorageExpendableCount>(),
                    };
                }).ToList();


            IList<D_StorageHookah> stHookahList = _session
                .QueryOver<D_StorageHookah>()
                .Where(x => x.Storage.Id == d_storage.Id)
                .List()
                .Select(ste =>
                {
                    return new D_StorageHookah()
                    {
                        Id = ste.Id,
                        IsClosed = ste.IsClosed,
                        CreationDateTime = ste.CreationDateTime,
                        Worker = ModelHelper.ParseToSmallVersion(ste.Worker),
                        Storage = new D_Storage() { Id = ste.Storage.Id, IsDisabled = ste.Storage.IsDisabled },
                        StorageTobaccoList = new List<D_StorageHookahStyle>(),
                    };
                }).ToList();

            StorageModel stModel = new StorageModel().Bind(new D_Storage() 
            {
                StorageExpendable = stExpedableList.Where(d => d.CreationDateTime.Date == DateTime.Today).ToList(),
                StorageHookah = stHookahList.Where(d => d.CreationDateTime.Date == DateTime.Today).ToList(),
                Worker = ModelHelper.ParseToSmallVersion(d_storage.Worker)
            });

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
                    .Where(x => x.CreationDateTime.Date == DateTime.Today && x.IsClosed == isClosed)
                    .Select(obj =>
                    {
                        return new D_StorageHookah()
                        {
                            Id = obj.Id,
                            CreationDateTime = obj.CreationDateTime,
                            Worker = ModelHelper.ParseToSmallVersion(obj.Worker),
                            Storage = new D_Storage() { Id = obj.Storage.Id },
                            StorageTobaccoList = obj.StorageTobaccoList
                        };
                    })
                    .ToList()
                    .LastOrDefault()
                );

            StorageExpendableModel stExpendableModel = new StorageExpendableModel().Bind(
                    _session.QueryOver<D_Branch>()
                    .Where(x => x.Id == branchId)
                    .List().LastOrDefault()
                    .Storage.StorageExpendable
                    .Where(x => x.CreationDateTime.Date == DateTime.Today && x.IsClosed == isClosed)
                    .Select(obj =>
                    {
                        return new D_StorageExpendable()
                        {
                            Id = obj.Id,
                            CreationDateTime = obj.CreationDateTime,
                            Worker = ModelHelper.ParseToSmallVersion(obj.Worker),
                            Storage = new D_Storage() { Id = obj.Storage.Id },
                            StorageExpendableListCount = obj.StorageExpendableListCount
                        };
                    })
                    .ToList()
                    .LastOrDefault()
                );

            //StorageHookahModel stHookahModel = new StorageHookahModel().Bind(
            //        _session.QueryOver<D_Branch>()
            //        .Where(x => x.Id == branchId)
            //        .List().LastOrDefault()
            //        .Storage.StorageHookah
            //        .Where(x => x.CreationDateTime.Date == DateTime.Today)
            //        .ToList()
            //        .LastOrDefault()
            //    );

            //StorageExpendableModel stExpendableModel = new StorageExpendableModel().Bind(
            //        _session.QueryOver<D_Branch>()
            //        .Where(x => x.Id == branchId)
            //        .List().LastOrDefault()
            //        .Storage.StorageExpendable
            //        .Where(x => x.CreationDateTime.Date == DateTime.Today)
            //        .ToList()
            //        .LastOrDefault()
            //    );

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