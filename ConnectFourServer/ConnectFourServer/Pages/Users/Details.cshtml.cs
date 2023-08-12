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
    public class DetailsModel : PageModel
    {
        private readonly ConnectFourServer.Data.ServerDatabase _context;

        public DetailsModel(ConnectFourServer.Data.ServerDatabase context)
        {
            _context = context;
        }

      public Player Player { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.TblUsers == null)
            {
                return NotFound();
            }

            var player = await _context.TblUsers.FirstOrDefaultAsync(m => m.Username == id);
            if (player == null)
            {
                return NotFound();
            }
            else 
            {
                Player = player;
            }
            return Page();
        }
    }
}
