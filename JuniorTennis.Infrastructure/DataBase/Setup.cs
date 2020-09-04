using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace JuniorTennis.Infrastructure.DataBase
{
    public static class Setup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            var repositoryClasses = typeof(Setup).Assembly
                .ExportedTypes
                .Where(o => o.IsPublic)
                .Where(o => o.Name.EndsWith("Repository"))
                .Select(type =>
                {
                    var interfaceType = type.GetInterfaces()
                        .Where(o => o.Name.EndsWith(type.Name))
                        .FirstOrDefault();
                    return new { ClassType = type, InterfaceType = interfaceType };

                })
                .Where(o => o.InterfaceType != null);
            foreach (var item in repositoryClasses)
            {
                services.AddScoped(item.InterfaceType, item.ClassType);
            }
        }
    }
}
