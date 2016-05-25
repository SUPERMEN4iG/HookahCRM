using HookahCRM.DataModel;
using HookahCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HookahCRM.Lib.Helpers
{
    public static class TobaccoHelper
    {
        public static List<TobaccoModel> GetTobaccoList(this List<TobaccoModel> list, ref NHibernate.ISession _session)
        {
            list.AddRange(_session.QueryOver<D_Tobacco>()
                .List()
                .Select(obj =>
                {
                    return new D_Tobacco()
                    {
                        Id = obj.Id,
                        Name = obj.Name,
                        Country = obj.Country,
                        Severity = obj.Severity,
                        ShortName = obj.ShortName,
                        TobaccoList = obj.TobaccoList.Select(tList =>
                        {
                            return new D_TobaccoStyle()
                            {
                                Id = tList.Id,
                                IsDisabled = tList.IsDisabled,
                                Name = tList.Name,
                                Severity = tList.Severity,
                                CreationDateTime = tList.CreationDateTime,
                                Tobacco = new D_Tobacco()
                            };
                        }).ToList()
                    };
                })
                .Select(x => { return new TobaccoModel().Bind(x); }));

            return list;
        }
    }
}