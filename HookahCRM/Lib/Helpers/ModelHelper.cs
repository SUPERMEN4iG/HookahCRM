using HookahCRM.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HookahCRM.Lib.Helpers
{
    public static class ModelHelper
    {
        public static D_User ParseToSmallVersion(D_User obj)
        {
            return new D_User()
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                MiddleName = obj.MiddleName,
                Login = obj.Login,
                Phone = obj.Phone,
                Id = obj.Id,
                CreationDateTime = obj.CreationDateTime,
                IsDisabled = obj.IsDisabled,
                Roles = obj.Roles.Select(rol =>
                {
                    return new D_AbstractRole()
                    {
                        Id = rol.Id,
                        RoleType = rol.RoleType
                    };
                }).ToList()
            };
        }

        public static D_TobaccoStyle ParseToSmallVersion(D_TobaccoStyle stList)
        {
            return new D_TobaccoStyle()
            {
                Id = stList.Id,
                IsDisabled = stList.IsDisabled,
                Name = stList.Name,
                Severity = stList.Severity,
                CreationDateTime = stList.CreationDateTime,
                Tobacco = new D_Tobacco()
            };
        }
    }
}