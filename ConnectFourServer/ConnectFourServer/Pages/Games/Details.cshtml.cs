using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ConnectFourServer.Data;
using ConnectFourServer.Models;

namespace ConnectFourServer.Pages.Games
{
    public class DetailsModel : PageModel
    {
        private readonly ConnectFourServer.Data.ServerDatabase _context;

        public DetailsModel(ConnectFourServer.Data.ServerDatabase context)
        {
            _context = context;
        }

      public Game Game { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(string username, int gameid)
        {
            if (username == null || _context.TblGames == null)
            {
                return NotFound();
            }

            var game = await _context.TblGames.FirstOrDefaultAsync(m => m.Username == username && m.GameId == gameid);
            if (game == null)
            {
                return NotFound();
            }
            else 
            {
                Game = game;
            }
            return Page();
        }
    }
}
