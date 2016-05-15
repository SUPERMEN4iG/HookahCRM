using HookahCRM.DataModel;
using HookahCRM.Lib.Infrastructure;
using HookahCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;

namespace HookahCRM.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            //NHibernate.ISession session = SessionManager.CurrentSession;
            //session.BeginTransaction();

            ////D_User d_user = new D_User();
            ////d_user.CreationDateTime = DateTime.Now;
            ////d_user.Login = "Name1";

            ////session.Save(d_user);
            ////D_User d_user = new D_User() { Login = "user1", Password = Crypto.HashPassword("12345678") };

            ////d_user.Roles = new List<D_AbstractRole>() { new D_WorkerRole() { User = d_user } };

            //IList<D_TobaccoStyle> d_tobaccoList_AF = new List<D_TobaccoStyle>();
            //IList<D_TobaccoStyle> d_tobaccoList_H = new List<D_TobaccoStyle>();

            //D_Tobacco d_tobacco_AF = new D_Tobacco()
            //{
            //    Name = "AlFackher",
            //    ShortName = "AF",
            //    Severity = TobaccoSeverity.Easy,
            //    Country = "Арабские имираты",
            //    TobaccoList = d_tobaccoList_AF
            //};

            //D_Tobacco d_tobacco_H = new D_Tobacco()
            //{
            //    Name = "Nakhla",
            //    ShortName = "H",
            //    Severity = TobaccoSeverity.Middle,
            //    Country = "Египет",
            //    TobaccoList = d_tobaccoList_H
            //};

            //d_tobaccoList_AF.Add(new D_TobaccoStyle() { Name = "Мятная жевачка", Severity = TobaccoSeverity.Middle, Tobacco = d_tobacco_AF });
            //d_tobaccoList_AF.Add(new D_TobaccoStyle() { Name = "Двойное яблоко", Severity = TobaccoSeverity.Middle, Tobacco = d_tobacco_AF });

            //D_Branch d_branch = new D_Branch();
            //List<D_HookahPriceDirectory> d_hpdirectory = new List<D_HookahPriceDirectory>();
            //d_branch.HooahPriceDirectory = d_hpdirectory;
            //d_hpdirectory.Add(new D_HookahPriceDirectory() { Price = 500.0m, Tobacco = d_tobacco_AF });
            //d_hpdirectory.Add(new D_HookahPriceDirectory() { Price = 650.0m, Tobacco = d_tobacco_AF });

            //session.SaveOrUpdate(d_branch);
            //session.Transaction.Commit();

            //NHibernate.ISession session = SessionManager.CurrentSession;
            //_session.BeginTransaction();

            //D_User d_user = new D_User();
            //d_user.CreationDateTime = DateTime.Now;
            //d_user.Login = "admin";
            //d_user.Password = Crypto.HashPassword("12345678");
            //d_user.Roles.Add(new D_WorkerRole() { User = d_user });

            //_session.SaveOrUpdate(d_user);

            //D_User d_user2 = new D_User();
            //d_user2.CreationDateTime = DateTime.Now;
            //d_user2.Login = "worker";
            //d_user2.Password = Crypto.HashPassword("12345678");
            //d_user2.Roles.Add(new D_WorkerRole() { User = d_user2 });

            //_session.SaveOrUpdate(d_user2);
            //_session.Transaction.Commit();
            //_session.Dispose();

            return HttpNotFound("using web api to access this");
        }
    }
}
