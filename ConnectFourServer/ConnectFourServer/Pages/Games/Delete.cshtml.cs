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
    public class DeleteModel : PageModel
    {
        private readonly ConnectFourServer.Data.ServerDatabase _context;

        public DeleteModel(ConnectFourServer.Data.ServerDatabase context)
        {
            _context = context;
        }

        [BindProperty]
      public Game Game { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string username, int gamid)
        {
            if (username == null || _context.TblGames == null)
            {
                return NotFound();
            }

            var game = await _context.TblGames.FirstOrDefaultAsync(m => m.Username == username && m.GameId == gamid);

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

        public async Task<IActionResult> OnPostAsync(string username, int gameid)
        {
            if (username == null || _context.TblGames == null)
            {
                return NotFound();
            }
            var game = await _context.TblGames.Where(p => p.Username == username && p.GameId == gameid).FirstOrDefaultAsync();
            var player = await _context.TblUsers.Where(p => p.Username == username).FirstOrDefaultAsync();

            if (game != null)
            {
                Game = game;
                _context.TblGames.Remove(Game);
                player.GamesPlayed--;
                _context.TblUsers.Update(player);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
