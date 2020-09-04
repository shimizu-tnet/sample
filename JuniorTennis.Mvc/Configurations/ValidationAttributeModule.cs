using JuniorTennis.Mvc.Validations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace JuniorTennis.Mvc.Configurations
{
    public class ValidationAttributeModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // AdapterProviderを登録
            services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();
        }
    }
}
