using HookahCRM.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HookahCRM.Models
{
    public class BranchModel : AbstractDataModel<D_Branch, BranchModel>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public long? StorageId { get; set; }

        public IList<UserModel> Workers { get; set; }

        public override BranchModel Bind(D_Branch @object)
        {
            if (@object == null)
                throw new ArgumentNullException("@object");

            base.Bind(@object);

            this.Name = @object.Name;
            this.Address = @object.Address;
            this.StorageId = _session.QueryOver<D_Storage>().Where(x => x.Branch.Id == @object.Id).List().FirstOrDefault().Id;

            return this;
        }

        public override D_Branch UnBind(D_Branch @object = null)
        {
            if (@object == null)
                @object = new D_Branch();

            base.UnBind(@object);

            @object.Name = this.Name;
            @object.Address = this.Address;
            @object.Storage = _session.QueryOver<D_Storage>().Where(x => x.Branch.Id == this.StorageId).List().FirstOrDefault();

            return @object;
        }
    }
}