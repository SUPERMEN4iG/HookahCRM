using HookahCRM.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HookahCRM.Models
{
    public class StorageHookahStyleModel : AbstractDataModel<D_StorageHookahStyle, StorageHookahStyleModel>
    {
        public decimal Weight { get; set; }
        public TobaccoStyleModel TobaccoStyle { get; set; }

        public override StorageHookahStyleModel Bind(D_StorageHookahStyle @object)
        {
            if (@object == null)
                throw new ArgumentNullException("@object");

            base.Bind(@object);

            this.Weight = @object.Weight;
            this.TobaccoStyle = new TobaccoStyleModel().Bind(@object.TobaccoStyle);

            return this;
        }

        public override D_StorageHookahStyle UnBind(D_StorageHookahStyle @object = null)
        {
            if (@object == null)
                @object = new D_StorageHookahStyle();

            @object = base.UnBind(@object);

            @object.Weight = this.Weight;
            @object.TobaccoStyle = _session.QueryOver<D_TobaccoStyle>().Where(x => x.Id == TobaccoStyle.Id).List().FirstOrDefault();

            return @object;
        }
    }

    public interface IReportBlankModel
    {
    }

    public class StorageHookahModel : AbstractDataModel<D_StorageHookah, StorageHookahModel>, IReportBlankModel
    {
		public StorageModel Storage { get; set; }
		public long? StorageId { get; set; }
        public UserModel Worker { get; set; }
        public bool IsClosed { get; set; }
        public IList<StorageHookahStyleModel> StorageTobaccoList { get; set; }

        public override StorageHookahModel Bind(D_StorageHookah @object)
        {
            if (@object == null)
                return null;

            base.Bind(@object);

            this.Worker = new UserModel().Bind(@object.Worker);
            this.IsClosed = @object.IsClosed;
            this.StorageTobaccoList = @object.StorageTobaccoList.Select(x => { return new StorageHookahStyleModel().Bind(x); }).ToList();
            this.StorageId = @object.Storage.Id;
            //this.Storage = new StorageModel().Bind(@object.Storage);
            //this.Storage = new StorageModel().Bind(_session.QueryOver<D_Storage>().Where(x => x.Id == @object.Storage.Id).List().FirstOrDefault());

            return this;
        }

        public override D_StorageHookah UnBind(D_StorageHookah @object = null)
        {
            if (@object == null)
                @object = new D_StorageHookah();

            @object = base.UnBind(@object);

            @object.Worker = _session.QueryOver<D_User>().Where(x => x.Id == this.Worker.Id).List().FirstOrDefault();
            @object.IsClosed = this.IsClosed;
            @object.StorageTobaccoList = this.StorageTobaccoList.Select(x => { return x.UnBind(); }).ToList();
            @object.Storage = _session.QueryOver<D_Storage>().Where(x => x.Id == this.StorageId).List().LastOrDefault();

            return @object;
        }
    }

    public class ExpendableModel : AbstractDataModel<D_Expendable, ExpendableModel>
	{
		public string Name { get; set; }

        public ExpendableType Type { get; set; }

        public override ExpendableModel Bind(D_Expendable @object)
        {
            if (@object == null)
                throw new ArgumentNullException("@object");

            base.Bind(@object);

            this.Name = @object.Name;
            this.Type = @object.Type;

            return this;
        }

        public override D_Expendable UnBind(D_Expendable @object = null)
        {
            if (@object == null)
                @object = new D_Expendable();

            @object = base.UnBind(@object);

            @object.Name = this.Name;
            @object.Type = this.Type;

            return @object;
        }
	}

    public class StorageExpendableCountModel : AbstractDataModel<D_StorageExpendableCount, StorageExpendableCountModel>
    {
        public decimal Count { get; set; }
        public ExpendableModel Expendable { get; set; }

        public override StorageExpendableCountModel Bind(D_StorageExpendableCount @object)
        {
            if (@object == null)
                throw new ArgumentNullException("@object");

            base.Bind(@object);

            this.Count = @object.Count;
            this.Expendable = new ExpendableModel().Bind(@object.Expendable);

            return this;
        }

        public override D_StorageExpendableCount UnBind(D_StorageExpendableCount @object = null)
        {
            if (@object == null)
                @object = new D_StorageExpendableCount();

            @object = base.UnBind(@object);

            @object.Count = this.Count;
            @object.Expendable = _session.QueryOver<D_Expendable>().Where(x => x.Id == Expendable.Id).List().FirstOrDefault();

            return @object;
        }
    }

    public class StorageExpendableModel : AbstractDataModel<D_StorageExpendable, StorageExpendableModel>, IReportBlankModel
    {
        public UserModel Worker { get; set; }
        public bool IsClosed { get; set; }
        public IList<StorageExpendableCountModel> StorageExpendableListCount { get; set; }

        public override StorageExpendableModel Bind(D_StorageExpendable @object)
        {
            if (@object == null)
                return null;

            base.Bind(@object);

            this.Worker = new UserModel().Bind(@object.Worker);
            this.IsClosed = @object.IsClosed;
            this.StorageExpendableListCount = @object.StorageExpendableListCount.Select(x => { return new StorageExpendableCountModel().Bind(x); }).ToList();

            return this;
        }

        public override D_StorageExpendable UnBind(D_StorageExpendable @object = null)
        {
            if (@object == null)
                @object = new D_StorageExpendable();

            @object = base.UnBind(@object);

            @object.Worker = _session.QueryOver<D_User>().Where(x => x.Id == this.Worker.Id).List().FirstOrDefault();
            @object.IsClosed = this.IsClosed;
            @object.StorageExpendableListCount = this.StorageExpendableListCount.Select(x => { return x.UnBind(); }).ToList();

            return @object;
        }
    }

    public class StorageModel : AbstractDataModel<D_Storage, StorageModel>
    {
        /// <summary>
        /// Главный по складу
        /// </summary>
        public UserModel Worker { get; set; }

        //public long? BranchId { get; set; }

        public IList<StorageHookahModel> StorageHookah { get; set; }

        public IList<StorageExpendableModel> StorageExpendable { get; set; }

        //[JsonIgnore]
        //public BranchModel Branch { get; set; }

        public override StorageModel Bind(D_Storage @object)
        {
            if (@object == null)
                throw new ArgumentNullException("@object");

            base.Bind(@object);

            //this.Worker = new UserModel().Bind(_session.QueryOver<D_User>().Where(x => x.Id == @object.Worker.Id).List().FirstOrDefault());
            this.Worker = new UserModel().Bind(@object.Worker);
            //this.Branch = new BranchModel().Bind(_session.QueryOver<D_Branch>().Where(x => x.Id == @object.Branch.Id).List().FirstOrDefault());

            this.StorageHookah = @object.StorageHookah.Select(x => { return new StorageHookahModel().Bind(x); }).ToList();
            this.StorageExpendable = @object.StorageExpendable.Select(x => { return new StorageExpendableModel().Bind(x); }).ToList();

            //this.StorageHookah = _session.QueryOver<D_StorageHookah>().Where(x => x.Storage.Id == @object.Id)
            //    .List()
            //    .Select(x => { return new StorageHookahModel().Bind(x); })
            //    .ToList();

            //this.StorageExpendable = _session.QueryOver<D_StorageExpendable>().Where(x => x.Storage.Id == @object.Id)
            //    .List()
            //    .Select(x => { return new StorageExpendableModel().Bind(x); })
            //    .ToList();

			return this;
        }

        public override D_Storage UnBind(D_Storage @object = null)
        {
            if (@object == null)
                @object = new D_Storage();

            @object = base.UnBind(@object);

            var temp = this.StorageHookah.LastOrDefault();

            @object.StorageHookah.Add(temp.UnBind());

            //@object.StorageHookah = this.StorageHookah.Select(x => x.UnBind()).ToList();

			@object.Worker = _session.QueryOver<D_User>().Where(x => x.Id == Worker.Id).List().FirstOrDefault();
            //@object.Branch = _session.QueryOver<D_Branch>().Where(x => x.Id == Branch.Id).List().FirstOrDefault();

            return @object;
        }
    }
}