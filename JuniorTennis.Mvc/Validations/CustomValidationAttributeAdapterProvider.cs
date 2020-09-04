using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace JuniorTennis.Mvc.Validations
{
    public class CustomValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider baseProvider = new ValidationAttributeAdapterProvider();
        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            switch (attribute)
            {
                case DateTimeAfterAttribute dateTimeAfterAttribute:
                    return new DateTimeAfterAttributeAdapter(dateTimeAfterAttribute, stringLocalizer);

                case RequiredWhenAttribute requiredWhenAttribute:
                    return new RequiredWhenAttributeAdapter(requiredWhenAttribute, stringLocalizer);

                case DateConsistencyAttribute dateConsistencyAttribute:
                    return new DateConsistencyAttributeAdapter(dateConsistencyAttribute, stringLocalizer);

                default:
                    return baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
            }
        }
    }
}
