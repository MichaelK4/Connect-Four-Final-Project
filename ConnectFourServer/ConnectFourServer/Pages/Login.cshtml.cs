using ConnectFourServer.Data;
using ConnectFourServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ConnectFourServer.Pages
{
    public class LoginModel : PageModel
    {

        private readonly ServerDatabase _context;

        public LoginModel(ServerDatabase context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public int ChoiceID { get; set; }




        public IActionResult OnPost()
        {
            string normalizedUsername = Username.Trim().ToLower();

            var user = _context.TblUsers.FirstOrDefault(u =>
                           u.Username.ToLower() == normalizedUsername && u.ChoiceID == ChoiceID);

            if(user == null || !IsValidUsername(normalizedUsername))
            {
                ModelState.AddModelError("LoginFailed", "Login failed. Invalid username or choice ID.");
                return Page();
            }

            return RedirectToPage("/Account", new { username = normalizedUsername});
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
