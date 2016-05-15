using HookahCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HookahCRM.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Registration(UserModel model)
        //{
        //    ModelState.Clear();
        //}
	}
}