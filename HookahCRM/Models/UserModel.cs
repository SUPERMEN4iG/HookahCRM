using HookahCRM.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HookahCRM.Models
{
    public class UserModel : AbstractDataModel<D_User, UserModel>
    {
        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [JsonIgnore]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string PhotoPath { get; set; }

        public string Phone { get; set; }

        [JsonIgnore]
        public IEnumerable<D_AbstractRole> Roles { get; set; }

        //[JsonIgnore]
        public IEnumerable<D_Branch> Branches { get; set; }

        public override UserModel Bind(D_User @object)
        {
            if (@object == null)
                throw new ArgumentNullException("@object");

            base.Bind(@object);

            this.Login = @object.Login;
            this.Password = @object.Password;
            this.FirstName = @object.FirstName;
            this.LastName = @object.LastName;
            this.MiddleName = @object.MiddleName;
            this.PhotoPath = @object.Photo;
            this.Phone = @object.Photo;
            this.Roles = @object.Roles;
            this.Branches = @object.BranchList;

            return this;
        }

        public override D_User UnBind(D_User @object = null)
        {
            if (@object == null)
                @object = new D_User();

            @object.Login = this.Login;
            @object.Password = this.Password;

            return @object;
        }
    }
}