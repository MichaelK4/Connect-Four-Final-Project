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
using System.Numerics;

namespace ConnectFourServer.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly ConnectFourServer.Data.ServerDatabase _context;

        public EditModel(ConnectFourServer.Data.ServerDatabase context)
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

            var player =  await _context.TblUsers.FirstOrDefaultAsync(m => m.Username == id);
            if (player == null)
            {
                return NotFound();
            }
            Player = player;
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

            _context.Attach(Player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(Player.Username))
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

        private bool PlayerExists(string id)
        {
          return (_context.TblUsers?.Any(e => e.Username == id)).GetValueOrDefault();
        }
    }
}
