using Microsoft.AspNetCore.Mvc.RazorPages;
using Dz13._05._024.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dz13._05._024.Pages {
    public class IndexModel : PageModel {
        public IList<Films> Film { get; set; } = default!;
        public async Task OnGetAsync([FromServices] IRepository rep) => Film = await rep.GetFilms();
    }
}