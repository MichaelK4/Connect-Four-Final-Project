using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConnectFourServer.Data;
using ConnectFourServer.Models;

namespace ConnectFourServer.Pages.Games
{
    public class EditModel : PageModel
    {
        private readonly ConnectFourServer.Data.ServerDatabase _context;

        public EditModel(ConnectFourServer.Data.ServerDatabase context)
        {
            _context = context;
        }

        [BindProperty]
        public Game Game { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.TblGames == null)
            {
                return NotFound();
            }

            var game =  await _context.TblGames.FirstOrDefaultAsync(m => m.Username == id);
            if (game == null)
            {
                return NotFound();
            }
            Game = game;
           ViewData["Username"] = new SelectList(_context.TblUsers, "Username", "Username");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(Game.Username))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool GameExists(string id)
        {
          return (_context.TblGames?.Any(e => e.Username == id)).GetValueOrDefault();
        }
    }
}
