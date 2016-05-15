using HookahCRM.DataModel;
using HookahCRM.Lib.Infrastructure;
using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HookahCRM.Models
{
    public interface IAbstractModel 
    {

    }
    public interface IDataModel
    {
        long? Id { get; set; }
    }

    public interface IDataBinding<TObject, out TModel>
        where TObject : D_BaseObject
        where TModel : class, IAbstractModel
    {
        TModel Bind(TObject @object);
        TObject UnBind(TObject @object = null);
    }

    public interface ILogicDataBinding<TLogicObject, out TModel>
        where TLogicObject : class, IAbstractBaseLogicObject
        where TModel : AbstractModel
    {
        TModel Bind(TLogicObject @object);
        TLogicObject UnBind(TLogicObject @object = null);
    }

    public interface IAbstractBaseLogicObject
    {
        D_BaseObject BaseLogicObject { get; }
    }

    public interface INhibernateEvent
    {
        void OnPreInsert(NHibernate.Event.PreInsertEvent @event);

        void OnPreUpdate(NHibernate.Event.PreUpdateEvent @event);
    }

    public abstract class AbstractLogicObject<TDataObject> : IAbstractBaseLogicObject, INhibernateEvent, IEntityObject
    where TDataObject : D_BaseObject
    {
        public AbstractLogicObject(TDataObject dataObject)
        {
            if (dataObject == null)
                throw new ArgumentNullException("logicObject");

            _Session = SessionManager.CurrentSession;
            _LogicObject = dataObject;
        }

        protected readonly ISession _Session;
        protected readonly TDataObject _LogicObject;

        public TDataObject LogicObject { get { return _LogicObject; } }

        public D_BaseObject BaseLogicObject
        {
            get
            {
                return _LogicObject;
            }
        }

        public virtual void OnPreInsert(NHibernate.Event.PreInsertEvent @event) { }

        public virtual void OnPreUpdate(NHibernate.Event.PreUpdateEvent @event) { }
    }

    public abstract class AbstractModel : IAbstractModel
    {
        protected readonly ISession _session = SessionManager.CurrentSession;

        #region Text resources
        protected readonly Dictionary<string, object> _TextResources = new Dictionary<string, object>();

        public Dictionary<string, object> TextResources
        {
            get
            {
                return _TextResources;
            }
        }
        #endregion
    }

    /// <summary>
    /// Базовая модель. 
    /// Используется для биндинга сущностей с уровня данных.
    /// </summary>
    public abstract class AbstractDataModel : AbstractModel, IDataModel, IDataBinding<D_BaseObject, AbstractDataModel>
    {
        [NonSerialized]
        [JsonIgnore]
        protected IAbstractBaseLogicObject _Object;

        #region Data lazy load
        [JsonIgnore]
        protected readonly Dictionary<string, bool> _LazyLoadProperties = new Dictionary<string, bool>();
        #endregion

        [HiddenInput(DisplayValue = false)]
        public long? Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime CreationDateTime { get; set; }

        #region General Bind/Unbind
        private void GeneralBind()
        {
            foreach (var key in _LazyLoadProperties.Keys.ToList())
            {
                _LazyLoadProperties[key] = true;
            }
        }

        private void GeneralUnbind()
        {

        }
        #endregion

        #region For data objects
        public virtual AbstractDataModel Bind(D_BaseObject @object)
        {
            GeneralBind();

            Id = @object.Id;
            CreationDateTime = @object.CreationDateTime;

            return this;
        }

        public virtual D_BaseObject UnBind(D_BaseObject @object = null)
        {
            GeneralUnbind();

            if (@object == null)
                throw new ArgumentOutOfRangeException("object");

            return @object;
        }
        #endregion

        #region For logic objects
        public virtual AbstractDataModel Bind(IAbstractBaseLogicObject @object)
        {
            if (@object == null)
                throw new ArgumentNullException("@object");

            _Object = @object;

            Id = @object.BaseLogicObject.Id;
            CreationDateTime = @object.BaseLogicObject.CreationDateTime;

            return this;
        }

        public virtual IAbstractBaseLogicObject UnBind(IAbstractBaseLogicObject @object = null)
        {
            if (@object == null)
                throw new ArgumentOutOfRangeException("object");

            return @object;
        }
        #endregion
    }

    /// <summary>
    /// Базовая модель данных. Для объектов данных
    /// </summary>
    /// <typeparam name="TObject">Тип объекта данных</typeparam>
    /// <typeparam name="TModel">Тип модели</typeparam>
    public abstract class AbstractDataModel<TObject, TModel> : AbstractDataModel, IDataBinding<TObject, TModel>
        where TObject : D_BaseObject
        where TModel : class, IAbstractModel
    {
        [NonSerialized]
        new protected TObject _Object;

        public virtual TModel Bind(TObject @object)
        {
            base.Bind(@object);

            _Object = @object;

            return this as TModel;
        }

        public virtual TObject UnBind(TObject @object = null)
        {
            return (TObject)base.UnBind(@object);
        }
    }

    /// <summary>
    /// Базовая модель данных. Для логических объектов
    /// </summary>
    /// <typeparam name="TLogicObject">Тип логического объекта данных. Proxy-объекта</typeparam>
    /// <typeparam name="TDataObject">Тип объекта данных</typeparam>
    /// <typeparam name="TModel">Тип модели</typeparam>
    public abstract class AbstractDataModel<TLogicObject, TDataObject, TModel> : AbstractDataModel, ILogicDataBinding<TLogicObject, TModel>
        where TLogicObject : AbstractLogicObject<TDataObject>
        where TDataObject : D_BaseObject, new()
        where TModel : AbstractModel
    {
        new protected TLogicObject _Object;

        public virtual TModel Bind(TLogicObject @object)
        {
            if (@object == null)
                throw new ArgumentNullException("@object");

            _Object = @object;

            base.Bind(@object);

            return this as TModel;
        }

        public virtual TLogicObject UnBind(TLogicObject @object = null)
        {
            throw new NotImplementedException();
        }
    }

    #region Exceptions
    public abstract class ModelException : ApplicationException
    {
        public ModelException() : base() { }
        public ModelException(string message) : base(message) { }
        public ModelException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ModelInvalidException : ModelException
    {
        public ModelInvalidException() : base() { }
        public ModelInvalidException(string message) : base(message) { }
        public ModelInvalidException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class BindNotCallException<TDataObject> : ModelException
      where TDataObject : IEntityObject
    {
        public BindNotCallException()
            : base()
        {
            _Message = String.Format("{0} Bind not call", typeof(TDataObject).Name);
        }

        private readonly string _Message;

        public override string Message
        {
            get
            {
                return _Message;
            }
        }
    }
    #endregion
}