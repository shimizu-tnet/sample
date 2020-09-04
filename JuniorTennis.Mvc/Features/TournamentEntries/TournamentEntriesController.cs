using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JuniorTennis.Mvc.Features.TournamentEntries
{
    public class TournamentEntriesController : Controller
    {
        public IActionResult Index()
        {
            return this.View(new IndexViewModel());
        }
    }
}
