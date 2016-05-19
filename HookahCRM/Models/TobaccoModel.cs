using HookahCRM.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HookahCRM.Models
{
    public class TobaccoStyleModel : AbstractDataModel<D_TobaccoStyle, TobaccoStyleModel>
    {
        public struct TobaccoCategory
        {
            public long Id { get; set; }

            [Required]
            public string Name { get; set; }

            public string ShortName { get; set; }

            public TobaccoSeverity Severity { get; set; }
        }

        [Required]
        public string Name { get; set; }

        public TobaccoSeverity Severity { get; set; }

        public TobaccoCategory Category { get; set; }

        public override TobaccoStyleModel Bind(D_TobaccoStyle @object)
        {
            if (@object == null)
                throw new ArgumentNullException("@object");

            base.Bind(@object);

            this.Name = @object.Name;
            this.Severity = @object.Severity;
            this.Category = new TobaccoCategory() 
            { 
                Name = @object.Tobacco.Name,
                ShortName = @object.Tobacco.ShortName,
                Severity = @object.Tobacco.Severity,
                Id = @object.Tobacco.Id
            };

            return this;
        }

        public override D_TobaccoStyle UnBind(D_TobaccoStyle @object = null)
        {
            if (@object == null)
                @object = new D_TobaccoStyle();

            @object.Name = this.Name;
            @object.Severity = this.Severity;
            @object.Tobacco = _session.QueryOver<D_Tobacco>().Where(x => x.Name == this.Category.Name).List().FirstOrDefault();

            return @object;
        }
    }

    public class TobaccoModel : AbstractDataModel<D_Tobacco, TobaccoModel>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ShortName { get; set; }

        [Required]
        public string Country { get; set; }

        public TobaccoSeverity Severity { get; set; }

        public IList<TobaccoStyleModel> TobaccoList { get; set; }

        public override TobaccoModel Bind(D_Tobacco @object)
        {
            if (@object == null)
                throw new ArgumentNullException("@object");

            base.Bind(@object);

            this.Name = @object.Name;
            this.ShortName = @object.ShortName;
            this.Severity = @object.Severity;
			this.Country = @object.Country;
            this.TobaccoList = @object.TobaccoList.Select(x => { return new TobaccoStyleModel().Bind(x); }).ToList();

            return this;
        }

		public override D_Tobacco UnBind(D_Tobacco @object = null)
		{
			if (@object == null)
				@object = new D_Tobacco();

			base.UnBind(@object);

			@object.Name = this.Name;
			@object.ShortName = this.ShortName;
			@object.Country = this.Country;
			@object.Severity = this.Severity;
			@object.TobaccoList = this.TobaccoList.Select(x => { return x.UnBind(); }).ToList();

			return @object;
		}
	}
}