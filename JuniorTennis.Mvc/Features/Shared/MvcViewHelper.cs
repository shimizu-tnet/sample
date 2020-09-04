using JuniorTennis.SeedWork;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Mvc.Features.Shared
{
    public class MvcViewHelper
    {
        /// <summary>
        /// Enumeration の型から SelectListItem の一覧を生成します。
        /// </summary>
        /// <typeparam name="T">Enumeration の型。</typeparam>
        /// <param name="selectedId">選択したID。nullの場合選択しない。</param>
        /// <returns>SelectListItem 一覧。</returns>
        public static List<SelectListItem> CreateSelectListItem<T>(int? selectedId = null) where T : Enumeration
        {
            var items = Enumeration.GetAll<T>();
            if (selectedId.HasValue)
            {
                return items.Select((o, i) => new SelectListItem(o.Name, $"{o.Id}", o.Id == selectedId.Value)).ToList();
            }

            return items.Select((o, i) => new SelectListItem(o.Name, $"{o.Id}")).ToList();
        }
    }
}
