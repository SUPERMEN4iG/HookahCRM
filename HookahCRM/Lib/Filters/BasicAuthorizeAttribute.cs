using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace HookahCRM.Lib.Filters
{
	public class BasicAuthorizeAttribute : AuthorizeAttribute
	{
		private readonly List<Type> _AllowedRoleTypes = new List<Type>();
		public bool IsNeedSkipAuth { get; set; }

		public BasicAuthorizeAttribute(params Type[] allowRoles)
		{

		}

		public override void OnAuthorization(HttpActionContext actionContext)
		{
			base.OnAuthorization(actionContext);
		}
	}
}