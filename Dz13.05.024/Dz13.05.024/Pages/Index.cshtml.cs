using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dz13._05._024.Models;

namespace Dz13._05._024.Pages {
    public class IndexModel : PageModel {
        private readonly Context db;
        public IList<Films> Film { get; set; } = default!;
        public IndexModel(Context context) => db = context;
        public async Task OnGetAsync() => Film = await db.Films.ToListAsync();
    }
}