using HookahCRM.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HookahCRM.Models
{
    public class DayHookahSaleModel : AbstractDataModel<D_DayHookahSale, DayHookahSaleModel>, IReportBlankModel
    {
        public TobaccoModel Tobacco { get; set; }

        public decimal Count { get; set; }

        public SalesModel Sales { get; set; }

        public long? SalesId { get; set; }

        public ActionType ActionType { get; set; }

        public override DayHookahSaleModel Bind(D_DayHookahSale @object)
        {
            if (@object == null)
                throw new ArgumentNullException("@object");

            base.Bind(@object);

            this.Tobacco = new TobaccoModel().Bind(@object.Tobacco);
            this.Count = @object.Count;
            this.SalesId = @object.Sales.Id;
            this.ActionType = @object.ActionType;
            //this.Sales = new SalesModel().Bind(_session.QueryOver<D_Sales>().Where(x => x.Id == @object.Sales.Id).List().LastOrDefault());

            return this;
        }

        public override D_DayHookahSale UnBind(D_DayHookahSale @object = null)
        {
            if (@object == null)
                @object = new D_DayHookahSale();

			//@object.Tobacco = this.Tobacco.UnBind();
			@object.Tobacco = _session.QueryOver<D_Tobacco>().Where(x => x.Id == this.Tobacco.Id).List().FirstOrDefault();
            @object.Count = this.Count;
            @object.Sales = _session.QueryOver<D_Sales>().Where(x => x.Id == this.SalesId).List().LastOrDefault();
            @object.ActionType = this.ActionType;

            return @object;
        }
    }

    public class DayAdditionSalesModel : AbstractDataModel<D_DayAdditionSales, DayAdditionSalesModel>, IReportBlankModel
    {
        public D_Addition Addition { get; set; }

        public decimal Count { get; set; }

        public SalesModel Sales { get; set; }
        public long? SalesId { get; set; }

        public ActionType ActionType { get; set; }

        public override DayAdditionSalesModel Bind(D_DayAdditionSales @object)
        {
            base.Bind(@object);

            this.Addition = @object.Addition;
            this.Count = @object.Count;
            this.SalesId = @object.Sales.Id;
            this.ActionType = @object.ActionType;
            //this.Sales = new SalesModel().Bind(_session.QueryOver<D_Sales>().Where(x => x.Id == @object.Sales.Id).List().LastOrDefault());

            return this;
        }

        public override D_DayAdditionSales UnBind(D_DayAdditionSales @object = null)
        {
            if (@object == null)
                @object = new D_DayAdditionSales();

            @object.Addition = this.Addition;
            @object.Count = this.Count;
            @object.Sales = _session.QueryOver<D_Sales>().Where(x => x.Id == this.SalesId).List().LastOrDefault();
            @object.ActionType = this.ActionType;

            return @object;
        }
    }

    public class SalesModel : AbstractDataModel<D_Sales, SalesModel>, IReportBlankModel
    {
        public UserModel Worker { get; set; }

        //public BranchModel Branch { get; set; }

        public IList<DayHookahSaleModel> DayHookahSales { get; set; }

        public IList<DayAdditionSalesModel> DayAdditionSales { get; set; }

        public override SalesModel Bind(D_Sales @object)
        {
            if (@object == null)
                throw new ArgumentNullException("@object");

            base.Bind(@object);

            this.Worker = new UserModel().Bind(@object.Worker);
            //this.Branch = new BranchModel().Bind(@object.Branch);

            this.DayHookahSales = @object.DayHoohahSales.Select(x => { return new DayHookahSaleModel().Bind(x); }).ToList();
            this.DayAdditionSales = @object.DayAdditionSales.Select(x => { return new DayAdditionSalesModel().Bind(x); }).ToList();

            return this;
        }

        public override D_Sales UnBind(D_Sales @object = null)
        {
            if (@object == null)
                @object = new D_Sales();

            base.UnBind(@object);

			//@object.Branch = this.Branch.UnBind();
			//@object.Worker = this.Worker.UnBind();
			@object.Worker = _session.QueryOver<D_User>().Where(x => x.Id == this.Worker.Id).List().FirstOrDefault();
            @object.DayHoohahSales = this.DayHookahSales.Select(x => x.UnBind()).ToList();
            @object.DayAdditionSales = this.DayAdditionSales.Select(x => x.UnBind()).ToList();

            return @object;
        }
    }
}