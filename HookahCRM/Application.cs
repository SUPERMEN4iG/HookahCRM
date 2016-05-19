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
			D_Branch d_branch2, d_branch3;

            if (d_branch1 == null)
            {
                #region Заведение
                d_branch1 = new D_Branch();
                d_branch1.Name = "Шоколадница Бабефа";
                d_branch1.Address = "Branch1 Adress";

				d_branch2 = new D_Branch();
				d_branch2.Name = "NewYork";
				d_branch2.Address = "Branch1 Adress";

				d_branch3 = new D_Branch();
				d_branch3.Name = "Loft14";
				d_branch3.Address = "Branch1 Adress";

				//D_Addition d_addition1 = new D_Addition() { Name = "Addition1" };
				//D_Addition d_addition2 = new D_Addition() { Name = "Addition2" };
				//D_Addition d_addition3 = new D_Addition() { Name = "Addition3" };
				//{
				//    new D_AdditionPriceDirectory()
				//    { 
				//        Addition = d_addition1,
				//        Branch = d_branch1,
				//        Price = 10.0m
				//    },

				//    new D_AdditionPriceDirectory()
				//    { 
				//        Addition = d_addition2,
				//        Branch = d_branch1,
				//        Price = 20.0m
				//    },

				//    new D_AdditionPriceDirectory()
				//    { 
				//        Addition = d_addition3,
				//        Branch = d_branch1,
				//        Price = 30.0m
				//    }
				//};

				IList<D_TobaccoStyle> d_tobaccoList_AF = new List<D_TobaccoStyle>();
                IList<D_TobaccoStyle> d_tobaccoList_H = new List<D_TobaccoStyle>();
                IList<D_TobaccoStyle> d_tobaccoList_A = new List<D_TobaccoStyle>();
                IList<D_TobaccoStyle> d_tobaccoList_F = new List<D_TobaccoStyle>();
                IList<D_TobaccoStyle> d_tobaccoList_DS = new List<D_TobaccoStyle>();
                IList<D_TobaccoStyle> d_tobaccoList_T = new List<D_TobaccoStyle>();

                D_Tobacco d_tobacco_AF = new D_Tobacco()
                {
                    Name = "AlFackher",
                    ShortName = "AF",
                    Severity = TobaccoSeverity.Easy,
                    Country = "Арабские эмираты",
                    TobaccoList = d_tobaccoList_AF
                };

                D_Tobacco d_tobacco_A = new D_Tobacco()
                {
                    Name = "Adalya",
                    ShortName = "AD",
                    Severity = TobaccoSeverity.Easy,
                    Country = "Германия",
                    TobaccoList = d_tobaccoList_A
                };

                D_Tobacco d_tobacco_H = new D_Tobacco()
                {
                    Name = "Nakhla",
                    ShortName = "H",
                    Severity = TobaccoSeverity.Middle,
                    Country = "Египет",
                    TobaccoList = d_tobaccoList_H
                };

                D_Tobacco d_tobacco_F = new D_Tobacco()
                {
                    Name = "Fumari",
                    ShortName = "F",
                    Severity = TobaccoSeverity.VeryEasy,
                    Country = "США",
                    TobaccoList = d_tobaccoList_F
                };

                D_Tobacco d_tobacco_DS = new D_Tobacco()
                {
                    Name = "DarkSide",
                    ShortName = "DS",
                    Severity = TobaccoSeverity.Hard,
                    Country = "Россия",
                    TobaccoList = d_tobaccoList_DS
                };

                D_Tobacco d_tobacco_T = new D_Tobacco()
                {
                    Name = "Tangiers",
                    ShortName = "T",
                    Severity = TobaccoSeverity.VeryHard,
                    Country = "США",
                    TobaccoList = d_tobaccoList_T
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
                        Tobacco = d_tobacco_A,
                        Price = 300.0m
                    },

                    new D_HookahPriceDirectory()
                    {
                        Branch = d_branch1,
                        Tobacco = d_tobacco_H,
                        Price = 500.0m
                    },

                    new D_HookahPriceDirectory()
                    {
                        Branch = d_branch1,
                        Tobacco = d_tobacco_F,
                        Price = 500.0m
                    },

                    new D_HookahPriceDirectory()
                    {
                        Branch = d_branch1,
                        Tobacco = d_tobacco_DS,
                        Price = 550.0m
                    },

                    new D_HookahPriceDirectory()
                    {
                        Branch = d_branch1,
                        Tobacco = d_tobacco_T,
                        Price = 600.0m
                    }
                };

				d_branch2.HooahPriceDirectory = new List<D_HookahPriceDirectory>()
				{
					new D_HookahPriceDirectory()
					{
						Branch = d_branch2,
						Tobacco = d_tobacco_AF,
						Price = 300.0m
					},

					new D_HookahPriceDirectory()
					{
						Branch = d_branch2,
						Tobacco = d_tobacco_A,
						Price = 300.0m
					},

					new D_HookahPriceDirectory()
					{
						Branch = d_branch2,
						Tobacco = d_tobacco_H,
						Price = 500.0m
					},

					new D_HookahPriceDirectory()
					{
						Branch = d_branch2,
						Tobacco = d_tobacco_F,
						Price = 500.0m
					},

					new D_HookahPriceDirectory()
					{
						Branch = d_branch2,
						Tobacco = d_tobacco_DS,
						Price = 550.0m
					},

					new D_HookahPriceDirectory()
					{
						Branch = d_branch2,
						Tobacco = d_tobacco_T,
						Price = 600.0m
					}
				};

				d_branch3.HooahPriceDirectory = new List<D_HookahPriceDirectory>()
				{
					new D_HookahPriceDirectory()
					{
						Branch = d_branch3,
						Tobacco = d_tobacco_AF,
						Price = 300.0m
					},

					new D_HookahPriceDirectory()
					{
						Branch = d_branch3,
						Tobacco = d_tobacco_A,
						Price = 300.0m
					},

					new D_HookahPriceDirectory()
					{
						Branch = d_branch3,
						Tobacco = d_tobacco_H,
						Price = 500.0m
					},

					new D_HookahPriceDirectory()
					{
						Branch = d_branch3,
						Tobacco = d_tobacco_F,
						Price = 500.0m
					},

					new D_HookahPriceDirectory()
					{
						Branch = d_branch3,
						Tobacco = d_tobacco_DS,
						Price = 550.0m
					},

					new D_HookahPriceDirectory()
					{
						Branch = d_branch3,
						Tobacco = d_tobacco_T,
						Price = 600.0m
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

                session.SaveOrUpdate(d_branch1);
				session.SaveOrUpdate(d_branch2);
				session.SaveOrUpdate(d_branch3);

				d_branch1.Storage = new D_Storage();
                d_branch1.Storage.Branch = d_branch1;
                d_branch1.Storage.Worker = d_adminUser;

                session.SaveOrUpdate(d_branch1.Storage);

                d_branch1.Sales = new List<D_Sales>()
                {
                    new D_Sales()
                    {
                        Branch = d_branch1,
                        DayAdditionSales = new List<D_DayAdditionSales>(),
                        DayHoohahSales = new List<D_DayHookahSale>(),
                        Worker = d_adminUser
                    }
                };

				d_branch2.Storage = new D_Storage();
				d_branch2.Storage.Branch = d_branch2;
				d_branch2.Storage.Worker = d_adminUser;

				session.SaveOrUpdate(d_branch2.Storage);

				d_branch2.Sales = new List<D_Sales>()
				{
					new D_Sales()
					{
						Branch = d_branch2,
						DayAdditionSales = new List<D_DayAdditionSales>(),
						DayHoohahSales = new List<D_DayHookahSale>(),
						Worker = d_adminUser
					}
				};

				d_branch3.Storage = new D_Storage();
				d_branch3.Storage.Branch = d_branch3;
				d_branch3.Storage.Worker = d_adminUser;

				session.SaveOrUpdate(d_branch3.Storage);

				d_branch3.Sales = new List<D_Sales>()
				{
					new D_Sales()
					{
						Branch = d_branch3,
						DayAdditionSales = new List<D_DayAdditionSales>(),
						DayHoohahSales = new List<D_DayHookahSale>(),
						Worker = d_adminUser
					}
				};

				session.SaveOrUpdate(d_branch1.Sales.FirstOrDefault());
				session.SaveOrUpdate(d_branch2.Sales.FirstOrDefault());
				session.SaveOrUpdate(d_branch3.Sales.FirstOrDefault());

				new List<D_Expendable>() { 
                   new D_Expendable() { Name = "Шахта ХМ", Type = ExpendableType.Equipment },
                   new D_Expendable() { Name = "Колба ХМ", Type = ExpendableType.Equipment },
                   new D_Expendable() { Name = "Уголь", Type = ExpendableType.ExpendableMaterial },
                   new D_Expendable() { Name = "Мундштук", Type = ExpendableType.ExpendableMaterial },
                   new D_Expendable() { Name = "Фольга", Type = ExpendableType.ExpendableMaterial }
                }.ForEach(x => { session.SaveOrUpdate(x); });
            }

            //session.Transaction.Commit();
            //session.Dispose();
        }

        private static bool IsInitialized;

        public static bool IsTestBuild;
    }
}