using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace JuniorTennis.Mvc.Validations
{
    public class RequiredWhenAttributeAdapter : AttributeAdapterBase<RequiredWhenAttribute>
    {
        public RequiredWhenAttributeAdapter(RequiredWhenAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {
        }
        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-requiredWhen", this.GetErrorMessage(context));
            var targetPropertyName = this.Attribute.TargetPropertyName;
            MergeAttribute(context.Attributes, "data-val-requiredWhen-targetPropertyName", targetPropertyName);
            var targetValues = this.Attribute.TargetValues;
            MergeAttribute(context.Attributes, "data-val-requiredWhen-targetValues", targetValues);
        }
        public override string GetErrorMessage(ModelValidationContextBase validationContext) => this.Attribute.ErrorMessage;

    }
}
