using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ConnectFourServer.Data;
using ConnectFourServer.Models;

namespace ConnectFourServer.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly ConnectFourServer.Data.ServerDatabase _context;

        public IndexModel(ConnectFourServer.Data.ServerDatabase context)
        {
            _context = context;
        }

        public IList<Player> Player { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.TblUsers != null)
            {
                Player = await _context.TblUsers.ToListAsync();
            }
        }
    }
}
