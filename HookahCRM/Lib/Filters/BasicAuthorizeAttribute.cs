using HookahCRM.Controllers;
using HookahCRM.DataModel;
using HookahCRM.Lib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace HookahCRM.Lib.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class BasicAuthorizeAttribute : AuthorizeAttribute
	{
		private readonly List<Type> _AllowedRoleTypes = new List<Type>();
		public bool IsNeedSkipAuth { get; set; }

		//private readonly NHibernate.ISession _session = ;

		public BasicAuthorizeAttribute(params Type[] allowRoles)
		{
			if (allowRoles != null && allowRoles.Length > 0)
				_AllowedRoleTypes.AddRange(allowRoles);
		}

		public override void OnAuthorization(HttpActionContext actionContext)
		{
			base.OnAuthorization(actionContext);

			BaseApiController baseController = actionContext.ControllerContext.Controller as BaseApiController;

			if (baseController != null)
			{
				D_User d_user = baseController._session
					.QueryOver<D_User>()
					.Where(x => x.Login == baseController.User.Identity.Name)
					.List().LastOrDefault();

				if (d_user == null)
					throw new Exception("User is not valid");
				else if (IsNeedSkipAuth)
					return;
				else if (_AllowedRoleTypes.Count > 0)
				{
					bool isAccessDenied = true;

					foreach (var role in d_user.Roles)
					{
						if (_AllowedRoleTypes.Contains((role.GetType())))
							isAccessDenied = false;
					}

					if (isAccessDenied)
						throw new Exception("Access deniend");
				}
			}
		}
	}
}