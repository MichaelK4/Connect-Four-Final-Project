using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ConnectFourServer.Data;
using ConnectFourServer.Models;

namespace ConnectFourServer.Pages.Games
{
    public class CreateModel : PageModel
    {
        private readonly ConnectFourServer.Data.ServerDatabase _context;

        public CreateModel(ConnectFourServer.Data.ServerDatabase context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["Username"] = new SelectList(_context.TblUsers, "Username", "Username");
            return Page();
        }

        [BindProperty]
        public Game Game { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.TblGames == null || Game == null)
            {
                return Page();
            }

            _context.TblGames.Add(Game);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
