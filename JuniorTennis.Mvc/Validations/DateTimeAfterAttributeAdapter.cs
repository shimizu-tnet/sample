using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace JuniorTennis.Mvc.Validations
{
    public class DateTimeAfterAttributeAdapter : AttributeAdapterBase<DateTimeAfterAttribute>
    {
        public DateTimeAfterAttributeAdapter(DateTimeAfterAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {
        }
        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-dateTimeAfter", this.GetErrorMessage(context));
            var beginProperty = this.Attribute.BeginPropertyName;
            MergeAttribute(context.Attributes, "data-val-dateTimeAfter-beginProperty", beginProperty);
            var allowEquivalent = this.Attribute.AllowEquivalent;
            MergeAttribute(context.Attributes, "data-val-dateTimeAfter-allowEquivalent", allowEquivalent.ToString());
        }
        public override string GetErrorMessage(ModelValidationContextBase validationContext) => this.Attribute.ErrorMessage;

    }

}
