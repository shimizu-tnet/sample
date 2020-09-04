using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace JuniorTennis.Mvc.Validations
{
    public class DateConsistencyAttributeAdapter : AttributeAdapterBase<DateConsistencyAttribute>
    {
        public DateConsistencyAttributeAdapter(DateConsistencyAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        { }

        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-dateConsistency", this.GetErrorMessage(context));
            var yearPropatyName = this.Attribute.YearPropatyName;
            MergeAttribute(context.Attributes, "data-val-dateConsistency-yearPropatyName", yearPropatyName);
            var monthPropatyName = this.Attribute.MonthPropatyName;
            MergeAttribute(context.Attributes, "data-val-dateConsistency-monthPropatyName", monthPropatyName);
        }
        public override string GetErrorMessage(ModelValidationContextBase validationContext) => this.Attribute.ErrorMessage;
    }
}
