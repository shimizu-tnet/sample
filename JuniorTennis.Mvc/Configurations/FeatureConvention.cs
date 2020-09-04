using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Linq;
using System.Reflection;

namespace JuniorTennis.Mvc.Configurations
{
    /// <summary>
    /// ControllerModelにfeatureを追加します。
    /// </summary>

    public class FeatureConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var featureName = this.GetFeatureName(controller.ControllerType);
            controller.Properties.Add("feature", featureName);
        }

        private string GetFeatureName(TypeInfo controllerType)
        {
            var tokens = controllerType.FullName.Split('.');
            if (!tokens.Any(t => t == "Features"))
            {
                return string.Empty;
            }

            var featureName = tokens
                .Reverse()
                .Skip(1)
                .FirstOrDefault();
            return featureName;
        }
    }
}
