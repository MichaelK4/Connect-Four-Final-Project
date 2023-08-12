using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ConnectFourServer.Data;
using ConnectFourServer.Models;
using ConnectFourServer.controller;

namespace ConnectFourServer.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly ConnectFourServer.Data.ServerDatabase _context;

        public DeleteModel(ConnectFourServer.Data.ServerDatabase context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.TblUsers == null)
            {
                return NotFound();
            }
            var player = await _context.TblUsers.FindAsync(id);

            if (player != null)
            {
                var gamesToDelete = await _context.TblGames.Where(p => p.Username == id).ToListAsync();
                foreach (var game in gamesToDelete)
                {
                    _context.TblGames.Remove(game);
                }
                Player = player;
                _context.TblUsers.Remove(Player);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
