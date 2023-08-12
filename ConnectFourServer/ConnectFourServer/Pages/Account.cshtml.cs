using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConnectFourServer.Models;
using ConnectFourServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ConnectFourServer.Pages.Games;
using ConnectFourServer.Pages.Users;


namespace ConnectFourServer.Pages
{
    public class AccountModel : PageModel
    {

        private readonly ServerDatabase _context;

        public AccountModel(ServerDatabase context)
        {
            _context = context;
        }

        [BindProperty]
        public Player Player { get; set; }
        public List<Game> games { get; set; }


        public async Task<IActionResult> OnGetAsync(string username)
        {
            Player = await _context.TblUsers.FirstOrDefaultAsync(p => p.Username.ToLower() == username.ToLower());
            games = await _context.TblGames.Where(g => g.Username.ToLower() == username.ToLower()).ToListAsync();
            //games = await _context.TblGames.Where(g => g.Username.ToLower() == username.ToLower()).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostEditInfo(string username)
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            var player = await _context.TblUsers.FirstOrDefaultAsync(p => p.Username.ToLower() == username.ToLower());
            if(player == null)
            {
                return NotFound();
            }

            player.FirstName = Player.FirstName;
            player.LastName = Player.LastName;
            player.PhoneNumber = Player.PhoneNumber;
            player.Country = Player.Country;

            _context.Attach(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!PlayerExists(Player.Username))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("/Account", new { username = Player.Username });

        }
        private bool PlayerExists(string id)
        {
            return (_context.TblUsers?.Any(e => e.Username == id)).GetValueOrDefault();
        }


        public async Task<IActionResult> OnPostDeleteGame(int gameid)
        {
            var gameToDelete = _context.TblGames.FirstOrDefault(g => g.Username.ToLower() == Player.Username.ToLower() && g.GameId == gameid);
            if (gameToDelete != null)
            {
                _context.TblGames.Remove(gameToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Account", new { username = Player.Username });

        }
        
        public async Task<IActionResult> OnPostDeleteAccountAsync()
        {
            var gamesToDelete = _context.TblGames.Where(g => g.Username.ToLower() == Player.Username.ToLower());
            _context.TblGames.RemoveRange(gamesToDelete);

            var playerToDelete = _context.TblUsers.FirstOrDefault(p => p.Username.ToLower() == Player.Username.ToLower());
            if(playerToDelete != null)
            {
                _context.TblUsers.Remove(playerToDelete);
                await _context.SaveChangesAsync();
                TempData["Delete"] = "Account deleted successfully";
                return RedirectToPage("/Index");
            }

            return NotFound();
           
        }
    }
}