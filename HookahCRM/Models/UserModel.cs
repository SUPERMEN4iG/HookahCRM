﻿using HookahCRM.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace HookahCRM.Models
{
    public class UserModel : AbstractDataModel<D_User, UserModel>
    {
        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string PhotoPath { get; set; }

        [Required]
        public string Phone { get; set; }

        //[JsonIgnore]
        public IList<D_AbstractRole> Roles { get; set; }

        //[JsonIgnore]
        public IList<D_Branch> Branches { get; set; }

        [Required]
        public string BranchName { get; set; }

        [Required]
        public string RoleName { get; set; }

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
            this.Phone = @object.Phone;
            this.Roles = @object.Roles;

            if (@object.BranchList == null)
            {
                this.Branches = new List<D_Branch>();
                this.Branches.Add(_session.QueryOver<D_Branch>().Where(x => x.Name == this.BranchName).List().FirstOrDefault());
            }
            else
            {
                this.Branches = @object.BranchList;
            }

            return this;
        }

        public override D_User UnBind(D_User @object = null)
        {
            if (@object == null)
                @object = new D_User();

            @object.Login = this.Login;
            @object.Password = Crypto.HashPassword(this.Password);
            @object.FirstName = this.FirstName;
            @object.LastName = this.LastName;
            @object.MiddleName = this.MiddleName;
            @object.Photo = this.PhotoPath;
            @object.Phone = this.Phone;

            if (this.Roles == null)
            {
                @object.Roles = new List<D_AbstractRole>();
                Type roleType = this.RoleName.ToTypeFromName();
                D_AbstractRole d_role;
                //object d_role = Activator.CreateInstance(roleType);
                if (roleType == typeof(D_WorkerRole))
                {
                    d_role = new D_WorkerRole() { RoleType = this.RoleName.ToEnumFromName(), User = @object };
                }
                else if (roleType == typeof(D_TraineeRole))
                {
                    d_role = new D_TraineeRole() { RoleType = this.RoleName.ToEnumFromName(), User = @object };
                }
                else if (roleType == typeof(D_AdministratorRole))
                {
                    d_role = new D_AdministratorRole() { RoleType = this.RoleName.ToEnumFromName(), User = @object };
                }
                else
                {
                    d_role = new D_WorkerRole() { RoleType = RoleType.Worker, User = @object };
                }
                

                @object.Roles.Add(d_role);
            }
            else
            {
                @object.Roles = this.Roles;
            }

            if (this.Branches == null)
            {
                @object.BranchList = new List<D_Branch>();
                @object.BranchList.Add(_session.QueryOver<D_Branch>().Where(x => x.Name == this.BranchName).List().FirstOrDefault());
            }
            else
            {
                @object.BranchList = this.Branches;
            }

            return @object;
        }

        public string GetFullName()
        {
            return String.Format("{0}.{1}.{2}", this.FirstName, this.LastName[0], this.MiddleName[0]);
        }
    }
}