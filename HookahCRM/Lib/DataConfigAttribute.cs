using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HookahCRM.Lib
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataConfigAttribute : Attribute
    {
        /// <summary>
        /// Тип прокси-объекта логики
        /// </summary>
        public Type LogicProxyType { get; set; }
    }
}