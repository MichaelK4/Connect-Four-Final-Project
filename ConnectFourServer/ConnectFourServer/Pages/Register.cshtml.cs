using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConnectFourServer.Models;
using System.Threading.Tasks;
using ConnectFourServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ConnectFourServer.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ServerDatabase _context;

        public RegisterModel(ServerDatabase context)
        {
            _context = context;
        }

        [BindProperty]
        public Player Player { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

            if(!IsValidUsername(Player.Username))
            {
                ModelState.AddModelError("Player.Username", "Username must be between 4 and 20 characters long and contain only letters, numbers and ('_', '.') symbols.");
                return Page();
            }

            if (await _context.TblUsers.AnyAsync(u => u.ChoiceID == Player.ChoiceID))
            {
                ModelState.AddModelError("Player.ChoiceID", "ChoiceID is already used.");
                return Page();
            }

            string normalizedUsername = Player.Username.ToLower();

            bool usernameExists = await _context.TblUsers.AnyAsync(u =>
                u.Username.ToLower() == normalizedUsername);

            if (usernameExists)
            {
                ModelState.AddModelError("Player.Username", "The username is already taken.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }


            // Check if the ChoiceID is within the valid range (1-1000)
            if (Player.ChoiceID < 1 || Player.ChoiceID > 1000)
            {
                ModelState.AddModelError("Player.ChoiceID", "ChoiceID must be between 1 and 1000.");
                return Page();
            }

            if(!IsValidName(Player.FirstName))
            {
                ModelState.AddModelError("Player.FirstName", "Name should start with a capital and be atleast 2 letters long");
                return Page();
            }

            if(!IsValidName(Player.LastName))
            {
                ModelState.AddModelError("Player.LastName", "Name should start with a capital and be atleast 2 letters long");
                return Page();
            }


            if(!IsValidPhoneNumber(Player.PhoneNumber))
            {
                ModelState.AddModelError("Player.PhoneNumber", "Phone Number must be between 3 and 11 digits.");
                return Page();
            }

            

            // Save the player's information to the database
            _context.TblUsers.Add(Player);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Registration successful! Your information has been saved.";

            return RedirectToPage("./Index");
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length < 3 || phoneNumber.Length > 11)
            {
                return false;
            }
            if (!Regex.IsMatch(phoneNumber, @"^\d+$"))
            {
                return false;
            }
            return true; 
        }

        private bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2|| name.Length > 50 || !char.IsUpper(name[0]) || name.Contains(" "))
            {
                return false;
            }

            return true;
        }

        public bool IsValidUsername(string username)
        {
            if (username.Contains(" "))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }
            if (!Regex.IsMatch(username, @"[a-zA-Z]{4,}"))
            {
                return false;
            }
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_.]+$"))
            {
                return false;
            }
            if (username.Length > 20)
            {
                return false;
            }

            string lowercaseUsername = username.ToLower();

            if (Regex.IsMatch(lowercaseUsername, @"^\d+$"))
            {
                return false;
            }


            return true;
        }

        public void OnGet()
        {
        }
    }
}