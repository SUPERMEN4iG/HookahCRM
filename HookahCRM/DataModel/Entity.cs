using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Linq;
using FluentNHibernate.Mapping;
using NHibernate.Event;
using HookahCRM.Models;
using HookahCRM.Lib;

namespace HookahCRM.DataModel
{
    public interface IEntityObject
    {

    }

    public abstract class D_BaseObject : IEntityObject
    {
        public D_BaseObject()
        {
            this.Guid = Guid.NewGuid();
        }

        public virtual long Id { get; set; }
        public virtual Guid Guid { get; set; }
        public virtual DateTime CreationDateTime { get; set; }
        public virtual bool IsDisabled { get; set; }
        public virtual Guid? Id_UserDelete { get; set; }
        public virtual DateTime? DeleteDateTime { get; set; }
    }

    public class D_User : D_BaseObject 
    {
        public D_User() 
        {
            Roles = new List<D_AbstractRole>();
            BranchList = new List<D_Branch>();
        }

        public virtual IList<D_AbstractRole> Roles { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Photo { get; set; }
        public virtual IList<D_Branch> BranchList { get; set; }
    }

    public class D_AbstractRole : D_BaseObject
    {
        public virtual D_User User { get; set; }
        public virtual RoleType RoleType { get; set; }
    }

    public class D_WorkerRole : D_AbstractRole
    {
        public D_WorkerRole()
        {
            RoleType = DataModel.RoleType.Worker;
        }
    }

    public class D_AdministratorRole : D_AbstractRole
    {
        public D_AdministratorRole()
        {
            RoleType = DataModel.RoleType.Administrator;
        }
    }

    public class D_TraineeRole : D_AbstractRole
    {
        public D_TraineeRole()
        {
            RoleType = DataModel.RoleType.Trainee;
        }
    }

    public enum RoleType
    {
        Banned = 0,
        Administrator = 1,
        Worker = 2,
        Trainee = 3,
        Manager = 4
    }

    public static class RoleTypeExtension
    {
        public static string ToStringName(this RoleType role)
        {
            switch (role)
            {
                case RoleType.Banned:
                    return "Заблокированный";
                case RoleType.Administrator:
                    return "Администратор";
                case RoleType.Worker:
                    return "Работник";
                case RoleType.Trainee:
                    return "Стажёр";
                case RoleType.Manager:
                    return "Менеджер";
                default:
                    break;
            }

            return string.Empty;
        }

        public static Type ToTypeFromName(this string roleName)
        {
            switch (roleName)
            {
                case "Заблокированный":
                    return typeof(D_AbstractRole);
                case "Администратор":
                    return typeof(D_AdministratorRole);
                case "Работник":
                    return typeof(D_WorkerRole);
                case "Стажёр":
                    return typeof(D_TraineeRole);
                case "Менеджер":
                    return typeof(D_AbstractRole);
                default:
                    break;
            }

            return null;
        }

        public static RoleType ToEnumFromName(this string roleName)
        {
            switch (roleName)
            {
                case "Заблокированный":
                    return RoleType.Banned;
                case "Администратор":
                    return RoleType.Administrator;
                case "Работник":
                    return RoleType.Worker;
                case "Стажёр":
                    return RoleType.Trainee;
                case "Менеджер":
                    return RoleType.Manager;
                default:
                    break;
            }

            return RoleType.Banned;
        }
    }

    /// <summary>
    /// Заведение
    /// </summary>
    public class D_Branch : D_BaseObject
    {
        public D_Branch()
        {
            Workers = new List<D_User>();
            Sales = new List<D_Sales>();

            HooahPriceDirectory = new List<D_HookahPriceDirectory>();
            AdditionPriceDirectory = new List<D_AdditionPriceDirectory>();
        }

        public virtual string Name { get; set; }
        public virtual string Address { get; set; }

        /// <summary>
        /// Склад
        /// </summary>
        public virtual D_Storage Storage { get; set; }

        /// <summary>
        /// Прикреплённые люди к заведению
        /// </summary>
        public virtual IList<D_User> Workers { get; set; }

        /// <summary>
        /// Список продаж
        /// </summary>
        public virtual IList<D_Sales> Sales { get; set; }

        /// <summary>
        /// Справочник стоимостей
        /// кальянов
        /// </summary>
        public virtual IList<D_HookahPriceDirectory> HooahPriceDirectory { get; set; }

        /// <summary>
        /// Справочник стоимостей дополнений
        /// к кальяну
        /// </summary>
        public virtual IList<D_AdditionPriceDirectory> AdditionPriceDirectory { get; set; }
    }

    /// <summary>
    /// Продажи за день
    /// </summary>
    public class D_Sales : D_BaseObject
    {
        public D_Sales()
        {
            DayHoohahSales = new List<D_DayHookahSale>();
            DayAdditionSales = new List<D_DayAdditionSales>();
        }

        /// <summary>
        /// Работник в текущий день
        /// </summary>
        public virtual D_User Worker { get; set; }

        /// <summary>
        /// Заведение
        /// </summary>
        public virtual D_Branch Branch { get; set; }

        /// <summary>
        /// Продажа в текущий день
        /// </summary>
        public virtual IList<D_DayHookahSale> DayHoohahSales { get; set; }
        public virtual IList<D_DayAdditionSales> DayAdditionSales { get; set; }
    }

    public enum ActionType : int
    {
        None = 0,
        FreeHookah = 1
    }

    public abstract class D_DaySales : D_BaseObject
    {
        public virtual decimal Count { get; set; }
        public virtual D_Sales Sales { get; set; }

        public virtual ActionType ActionType { get; set; }
    }

    /// <summary>
    /// Продажа кальяна
    /// </summary>
    public class D_DayHookahSale : D_DaySales
    {
        public virtual D_Tobacco Tobacco { get; set; }
    }

    /// <summary>
    /// Продажа дополнения
    /// </summary>
    public class D_DayAdditionSales : D_DaySales
    {
        public virtual D_Addition Addition { get; set; }
    }

    /// <summary>
    /// Справочник стомости табаков
    /// </summary>
    public class D_HookahPriceDirectory : D_BaseObject
    {
        //public virtual IDictionary<D_TobaccoStyle, decimal> HookahPriceDictionary { get; set; }
        public D_HookahPriceDirectory()
        {
        }

        public virtual D_Tobacco Tobacco { get; set; }
        public virtual decimal Price { get; set; }
        public virtual D_Branch Branch { get; set; }
    }

    /// <summary>
    /// Справочник стоимости дополнений к кальяну
    /// </summary>
    public class D_AdditionPriceDirectory : D_BaseObject
    {
        public virtual D_Addition Addition { get; set; }
        public virtual decimal Price { get; set; }
        public virtual D_Branch Branch { get; set; }
    }

    /// <summary>
    /// Дополнение к кальяну
    /// </summary>
    public class D_Addition : D_BaseObject
    {
        /// <summary>
        /// Название дополнения
        /// </summary>
        public virtual string Name { get; set; }
    }

    /// <summary>
    /// Склад
    /// </summary>
    public class D_Storage : D_BaseObject
    {
        public D_Storage()
        {
            StorageHookah = new List<D_StorageHookah>();
            StorageExpendable = new List<D_StorageExpendable>();
        }

        /// <summary>
        /// Заведение
        /// в котором находится склад
        /// </summary>
        public virtual D_Branch Branch { get; set; }

        /// <summary>
        /// Главный по складу
        /// </summary>
        public virtual D_User Worker { get; set; }

        public virtual IList<D_StorageHookah> StorageHookah { get; set; }
        public virtual IList<D_StorageExpendable> StorageExpendable { get; set; }
    }

    public class D_StorageHookah : D_BaseObject
    {
        public D_StorageHookah()
        {
            StorageTobaccoList = new List<D_StorageHookahStyle>();
        }

        public virtual D_Storage Storage { get; set; }

        public virtual bool IsClosed { get; set; }
        /// <summary>
        /// Пользователь, который заведует складом на сегодня
        /// </summary>
        public virtual D_User Worker { get; set; }
        public virtual IList<D_StorageHookahStyle> StorageTobaccoList { get; set; }
    }

    public class D_StorageHookahStyle : D_BaseObject
    {
        /// <summary>
        /// Кол-во грам табака
        /// </summary>
        public virtual decimal Weight { get; set; }

        public virtual D_TobaccoStyle TobaccoStyle { get; set; }
    }

    public class D_StorageExpendable : D_BaseObject
    {
        public D_StorageExpendable()
        {
            StorageExpendableListCount = new List<D_StorageExpendableCount>();
        }

        public virtual bool IsClosed { get; set; }
        public virtual D_Storage Storage { get; set; }
        /// <summary>
        /// Пользователь, который заведует складом на сегодня
        /// </summary>
        public virtual D_User Worker { get; set; }
        public virtual IList<D_StorageExpendableCount> StorageExpendableListCount { get; set; }
    }

    public enum ExpendableType : int 
    {
        Equipment = 0,
        ExpendableMaterial = 1,
        OfficeItem = 2,
    }

    /// <summary>
    /// Расходник
    /// </summary>
    public class D_Expendable : D_BaseObject
    {
        public virtual string Name { get; set; }
        public virtual ExpendableType Type { get; set; }
    }

    /// <summary>
    /// Склад расходников
    /// </summary>
    public class D_StorageExpendableCount : D_BaseObject
    {
        public virtual D_Expendable Expendable { get; set; }
        /// <summary>
        /// Кол-во расходников
        /// </summary>
        public virtual decimal Count { get; set; }
    }

    /// <summary>
    /// Названия табаков
    /// </summary>
    public class D_Tobacco : D_BaseObject
    {
        public D_Tobacco()
        {
            TobaccoList = new List<D_TobaccoStyle>();
        }

        /// <summary>
        /// Название табака
        /// </summary>
        public virtual string Name { get; set; }
        public virtual string ShortName { get; set; }

        /// <summary>
        /// Страна-производитель
        /// </summary>
        public virtual string Country { get; set; }

        /// <summary>
        /// Тяжесть табака
        /// </summary>
        public virtual TobaccoSeverity Severity { get; set; }

        public virtual IList<D_TobaccoStyle> TobaccoList { get; set; }
    }

    public enum TobaccoSeverity
    {
        VeryEasy = 0,
        Easy = 1,
        Middle = 2,
        Hard = 3,
        VeryHard = 4
    }

    /// <summary>
    /// Вкусы тобака
    /// </summary>
    public class D_TobaccoStyle : D_BaseObject
    {
        public virtual string Name { get; set; }
        public virtual D_Tobacco Tobacco { get; set; }
        public virtual TobaccoSeverity Severity { get; set; }
    }

    #region Mapping
    public abstract class D_BaseObject_Map<TObject> : ClassMap<TObject>
        where TObject : D_BaseObject
    {
        public D_BaseObject_Map()
        {
            Id(x => x.Id).GeneratedBy.HiLo("1000").CustomType<Int64>();
            Map(x => x.Guid).Not.Nullable();

            Map(x => x.CreationDateTime).Not.Nullable();
            Map(x => x.IsDisabled).Default("0").Not.Nullable();

            Map(x => x.Id_UserDelete).Nullable();
            Map(x => x.DeleteDateTime).Nullable();
        }
    }

    public class D_User_Map : D_BaseObject_Map<D_User>
    {
        public D_User_Map()
        {
            Map(x => x.Login).Nullable().Unique().Length(64);
            Map(x => x.Password).Nullable().Length(256);
            Map(x => x.FirstName).Nullable().Length(128);
            Map(x => x.LastName).Nullable().Length(128);
            Map(x => x.MiddleName).Nullable().Length(128);
            Map(x => x.Phone).Nullable().Unique().Length(64);
            Map(x => x.Photo).Nullable().Length(256);

            HasMany(x => x.Roles).KeyColumn("UserId").Cascade.All();
            HasManyToMany(x => x.BranchList).Cascade.All();
        }
    }

    #region Roles
    public class D_AbstractRole_Map : D_BaseObject_Map<D_AbstractRole>
    {
        public D_AbstractRole_Map()
        {
            References(x => x.User).Column("UserId").Cascade.SaveUpdate();

            DiscriminateSubClassesOnColumn<RoleType>("RoleType", RoleType.Worker);
        }
    }

    public class D_WorkerRole_Map : SubclassMap<D_WorkerRole>
    {
        public D_WorkerRole_Map()
        {
            DiscriminatorValue(RoleType.Worker);
        }
    }

    public class D_AdministratorRole_Map : SubclassMap<D_AdministratorRole>
    {
        public D_AdministratorRole_Map()
        {
            DiscriminatorValue(RoleType.Administrator);
        }
    }

    public class D_TraineeRole_Map : SubclassMap<D_TraineeRole>
    {
        public D_TraineeRole_Map()
        {
            DiscriminatorValue(RoleType.Trainee);
        }
    }
    #endregion

    #region Tobacco
    public class D_Tobacco_Map : D_BaseObject_Map<D_Tobacco>
    {
        public D_Tobacco_Map()
        {
            Map(x => x.Name).Nullable().Unique().Length(128);
            Map(x => x.ShortName).Nullable().Unique().Length(64);
            HasMany(x => x.TobaccoList).KeyColumn("TobaccoId").Cascade.All();
            Map(x => x.Severity).CustomType<TobaccoSeverity>();
        }
    }

    public class D_TobaccoStyle_Map : D_BaseObject_Map<D_TobaccoStyle>
    {
        public D_TobaccoStyle_Map()
        {
            Map(x => x.Name).Nullable().Length(128);
            Map(x => x.Severity).CustomType<TobaccoSeverity>();
            References(x => x.Tobacco).Column("TobaccoId").Cascade.SaveUpdate();
        }
    }
    #endregion

    public class D_Addition_Map : D_BaseObject_Map<D_Addition>
    {
        public D_Addition_Map()
        {
            Map(x => x.Name).Length(256);
        }
    }

    #region Справочники стоимостей
    public class D_HookahPriceDirectory_Map : D_BaseObject_Map<D_HookahPriceDirectory>
    {
        public D_HookahPriceDirectory_Map()
        {
            References(x => x.Branch).Not.Insert().Not.Update();
            References(x => x.Tobacco).Column("TobaccoId").Cascade.SaveUpdate();
            Map(x => x.Price).Precision(10).Scale(2);
        }
    }

    public class D_AdditionPriceDirectory_Map : D_BaseObject_Map<D_AdditionPriceDirectory>
    {
        public D_AdditionPriceDirectory_Map()
        {
            References(x => x.Branch).Not.Insert().Not.Update();
            References(x => x.Addition).Column("AdditionId").Cascade.SaveUpdate();
            Map(x => x.Price).Precision(10).Scale(2);
        }
    }
    #endregion

    #region Заведение
    public class D_Branch_Map : D_BaseObject_Map<D_Branch>
    {
        public D_Branch_Map()
        {
            Map(x => x.Name).Nullable().Length(256);
            Map(x => x.Address).Nullable().Length(512);
            //Map(x => x.Storage).Nullable().Column("Storage_Id");
            //HasMany(x => x.Sales).KeyColumn("Sales_Id").Cascade.All();
            HasManyToMany(x => x.Workers).Inverse().Cascade.All();

            //References(x => x.Storage).ForeignKey("Storage_Id").Cascade.All();
            HasOne(x => x.Storage).PropertyRef(s => s.Branch).Cascade.All();
            HasMany(x => x.Sales).Inverse().Cascade.All();

            HasMany<D_HookahPriceDirectory>(x => x.HooahPriceDirectory).Inverse().Cascade.All();
            HasMany<D_AdditionPriceDirectory>(x => x.AdditionPriceDirectory).Inverse().Cascade.All();

            //HasManyToMany(x => x.AdditionPriceDirectory).Cascade.All();
        }
    }
    #endregion

    #region Продажи
    public class D_DayHookahSale_Map : D_BaseObject_Map<D_DayHookahSale>
    {
        public D_DayHookahSale_Map()
        {
            References(x => x.Sales).Column("DayHoohahSales_Id").Cascade.SaveUpdate();
            References(x => x.Tobacco).Column("Tobacco_Id").Cascade.SaveUpdate();
            Map(x => x.ActionType).CustomType<ActionType>().Default("0");
            Map(x => x.Count).Length(128);
        }
    }

    public class D_DayAdditionSales_Map : D_BaseObject_Map<D_DayAdditionSales>
    {
        public D_DayAdditionSales_Map()
        {
            References(x => x.Sales).Column("DayAdditionSales_Id").Cascade.SaveUpdate();
            References(x => x.Addition).Column("Addition_Id").Cascade.SaveUpdate();
            Map(x => x.ActionType).CustomType<ActionType>().Default("0");
            Map(x => x.Count).Length(128);
        }
    }

    public class D_Sales_Map : D_BaseObject_Map<D_Sales>
    {
        public D_Sales_Map()
        {
            References(x => x.Branch).Column("Branch_Id").Cascade.SaveUpdate();
            References(x => x.Worker).Column("Worker_Id").Cascade.SaveUpdate();
            HasMany(x => x.DayHoohahSales).KeyColumn("DayHoohahSales_Id").Cascade.All();
            HasMany(x => x.DayAdditionSales).KeyColumn("DayAdditionSales_Id").Cascade.All();
        }
    }
    #endregion

    #region Склад
    public class D_StorageHookahStyle_Map : D_BaseObject_Map<D_StorageHookahStyle>
    {
        public D_StorageHookahStyle_Map()
        {
            References(x => x.TobaccoStyle).Column("TobaccoStyle_Id").Cascade.SaveUpdate();
            Map(x => x.Weight).Precision(10).Scale(2);
        }
    }

    public class D_StorageHookah_Map : D_BaseObject_Map<D_StorageHookah>
    {
        public D_StorageHookah_Map()
        {
            Map(x => x.IsClosed).Default("0").Not.Nullable();
            References(x => x.Storage).Column("Storage_Id").Cascade.SaveUpdate();
            References(x => x.Worker).Column("Worker_Id").Cascade.SaveUpdate();
            HasMany(x => x.StorageTobaccoList).Cascade.All();
        }
    }

    public class D_Expendable_Map : D_BaseObject_Map<D_Expendable>
    {
        public D_Expendable_Map()
        {
            Map(x => x.Name).Length(256).Nullable();
            Map(x => x.Type).CustomType<ExpendableType>().Default("0");
        }
    }

    public class D_StorageExpendableCount_Map : D_BaseObject_Map<D_StorageExpendableCount>
    {
        public D_StorageExpendableCount_Map()
        {
            Map(x => x.Count).Precision(10).Scale(2);
            References(x => x.Expendable).Column("Expendable_Id").Cascade.SaveUpdate();
        }
    }

    public class D_StorageExpendable_Map : D_BaseObject_Map<D_StorageExpendable>
    {
        public D_StorageExpendable_Map()
        {
            Map(x => x.IsClosed).Default("0").Not.Nullable();
            References(x => x.Storage).Column("Storage_Id").Cascade.SaveUpdate();
            References(x => x.Worker).Column("Worker_Id").Cascade.SaveUpdate();
            HasMany(x => x.StorageExpendableListCount).Cascade.All();
        }
    }

    public class D_Storage_Map : D_BaseObject_Map<D_Storage>
    {
        public D_Storage_Map()
        {
			//References(x => x.Branch).Unique();
            References(x => x.Branch, "STORAGE_FK_BRANCH").Not.Nullable();
            References(x => x.Worker).Column("Worker_Id").Cascade.SaveUpdate();
            HasMany(x => x.StorageHookah).KeyColumn("StorageHookah_Id").Cascade.All();
            HasMany(x => x.StorageExpendable).KeyColumn("StorageExpendable_Id").Cascade.All();
        }
    }
    #endregion

    #endregion

    #region Event listeners
    public class PreInsertEvent : IPreInsertEventListener
    {
        public bool OnPreInsert(NHibernate.Event.PreInsertEvent @event)
        {
            D_BaseObject baseObject = (@event.Entity as D_BaseObject);

            DataConfigAttribute dataConfigAttribute = baseObject.GetType().GetCustomAttributes(typeof(DataConfigAttribute), true).Cast<DataConfigAttribute>().FirstOrDefault();

            if (baseObject == null)
                return false;

            #region Задаю время создания
            int createdDateTimeIndex = Array.IndexOf(@event.Persister.PropertyNames, "CreationDateTime");

            DateTime creationDate = DateTime.UtcNow;
            @event.State[createdDateTimeIndex] = creationDate;
            baseObject.CreationDateTime = creationDate;
            #endregion

            if (dataConfigAttribute != null)
            {
                if (dataConfigAttribute.LogicProxyType != null)
                {
                    INhibernateEvent nhibernateEvent = Activator.CreateInstance(dataConfigAttribute.LogicProxyType, new object[] { baseObject }) as INhibernateEvent;
                    nhibernateEvent.OnPreInsert(@event);
                }
            }

            return false;
        }
    }

    public class PreUpdateEvent : IPreUpdateEventListener
    {
        public bool OnPreUpdate(NHibernate.Event.PreUpdateEvent @event)
        {
            D_BaseObject baseObject = (@event.Entity as D_BaseObject);

            DataConfigAttribute dataConfigAttribute = baseObject.GetType().GetCustomAttributes(typeof(DataConfigAttribute), true).Cast<DataConfigAttribute>().FirstOrDefault();

            if (baseObject == null)
                return false;

            if (dataConfigAttribute != null)
            {
                if (dataConfigAttribute.LogicProxyType != null)
                {
                    INhibernateEvent nhibernateEvent = Activator.CreateInstance(dataConfigAttribute.LogicProxyType, new object[] { baseObject }) as INhibernateEvent;
                    nhibernateEvent.OnPreUpdate(@event);
                }
            }

            return false;
        }
    }
    #endregion
}