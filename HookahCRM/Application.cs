using HookahCRM.DataModel;
using HookahCRM.Lib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace HookahCRM
{
    public sealed class Application
    {
        static Application()
        {
            IsTestBuild = true;
        }

        public static Application Instance
        {
            get 
            {
                return new Application();
            }
        }

        public void Initiliaze()
        {
            if (IsInitialized)
                return;

            SetStartupParams();

            IsInitialized = true;
        }

        private void SetStartupParams()
        {
            NHibernate.ISession session = SessionManager.CurrentSession;
            //session.BeginTransaction();

            D_Branch d_branch1 = session.QueryOver<D_Branch>().Where(x => x.Name == "Branch1").List().FirstOrDefault();

            if (d_branch1 == null)
            {
                #region Заведение
                d_branch1 = new D_Branch();
                d_branch1.Name = "Branch1";
                d_branch1.Address = "Branch1 Adress";
                d_branch1.Workers = new List<D_User>();
                d_branch1.AdditionPriceDirectory = new List<D_AdditionPriceDirectory>() 
                {
                    new D_AdditionPriceDirectory()
                    { 
                        Addition = new D_Addition() { Name = "Addition1" },
                        Branch = d_branch1,
                        Price = 10.0m
                    },

                    new D_AdditionPriceDirectory()
                    { 
                        Addition = new D_Addition() { Name = "Addition2" },
                        Branch = d_branch1,
                        Price = 20.0m
                    },

                    new D_AdditionPriceDirectory()
                    { 
                        Addition = new D_Addition() { Name = "Addition3" },
                        Branch = d_branch1,
                        Price = 30.0m
                    }
                };

                IList<D_TobaccoStyle> d_tobaccoList_AF = new List<D_TobaccoStyle>();
                IList<D_TobaccoStyle> d_tobaccoList_H = new List<D_TobaccoStyle>();

                D_Tobacco d_tobacco_AF = new D_Tobacco()
                {
                    Name = "AlFackher",
                    ShortName = "AF",
                    Severity = TobaccoSeverity.Easy,
                    Country = "Арабские эмираты",
                    TobaccoList = d_tobaccoList_AF
                };

                D_Tobacco d_tobacco_H = new D_Tobacco()
                {
                    Name = "Nakhla",
                    ShortName = "H",
                    Severity = TobaccoSeverity.Middle,
                    Country = "Египет",
                    TobaccoList = d_tobaccoList_H
                };

                d_tobaccoList_AF.Add(new D_TobaccoStyle() { Name = "Мятная жевачка", Severity = TobaccoSeverity.Middle, Tobacco = d_tobacco_AF });
                d_tobaccoList_AF.Add(new D_TobaccoStyle() { Name = "Двойное яблоко", Severity = TobaccoSeverity.Middle, Tobacco = d_tobacco_AF });

                d_branch1.HooahPriceDirectory = new List<D_HookahPriceDirectory>()
                {
                    new D_HookahPriceDirectory()
                    {
                        Branch = d_branch1,
                        Tobacco = d_tobacco_AF,
                        Price = 300.0m
                    },

                    new D_HookahPriceDirectory()
                    {
                        Branch = d_branch1,
                        Tobacco = d_tobacco_H,
                        Price = 500.0m
                    }
                };

                d_branch1.Sales = new List<D_Sales>()
                {
                    new D_Sales()
                    {
                        Branch = d_branch1,
                        DayAdditionSales = new List<D_DayAdditionSales>(),
                        DayHoohahSales = new List<D_DayHoohahSale>()
                    }
                };

                #endregion

                D_User d_adminUser = new D_User(), 
                       d_workerUser = new D_User();

                if (session.QueryOver<D_User>().Where(x => x.Login == "admin" || x.Login == "worker").List().Count() == 0)
                {
                    d_adminUser.Login = "admin";
                    d_adminUser.FirstName = "Kuvardin";
                    d_adminUser.LastName = "Kuzma";
                    d_adminUser.MiddleName = "Michalovich";
                    d_adminUser.Phone = "+7000000";
                    d_adminUser.Password = Crypto.HashPassword("12345678");
                    d_adminUser.Roles = new List<D_AbstractRole>() { new D_AdministratorRole() { User = d_adminUser } };
                    d_adminUser.BranchList = new List<D_Branch>();
                    session.SaveOrUpdate(d_adminUser);

                    d_workerUser.Login = "worker";
                    d_workerUser.FirstName = "Kuvardin2";
                    d_workerUser.LastName = "Kuzma2";
                    d_workerUser.MiddleName = "Michalovich2";
                    d_workerUser.Phone = "+7000001";
                    d_workerUser.Password = Crypto.HashPassword("12345678");
                    d_workerUser.Roles = new List<D_AbstractRole>() { new D_WorkerRole() { User = d_workerUser } };
                    d_workerUser.BranchList = new List<D_Branch>();
                    session.SaveOrUpdate(d_workerUser);
                }

                d_branch1.Workers.Add(d_adminUser);
                d_branch1.Workers.Add(d_workerUser);
                d_adminUser.BranchList.Add(d_branch1);
                d_workerUser.BranchList.Add(d_branch1);

                d_branch1.Storage = new D_Storage();
                d_branch1.Storage.Branch = d_branch1;
                d_branch1.Storage.Worker = d_adminUser;

                session.SaveOrUpdate(d_branch1);
                session.SaveOrUpdate(d_branch1.Storage);
            }

            //session.Transaction.Commit();
            //session.Dispose();
        }

        private static bool IsInitialized;

        public static bool IsTestBuild;
    }
}