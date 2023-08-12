using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectFourServer.Data;
using ConnectFourServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ConnectFourServer.Pages
{
    public class StatisticsModel : PageModel
    {
        private readonly ServerDatabase _context;

        public StatisticsModel(ServerDatabase context)
        {
            _context = context;
        }

        // Properties to hold data for the various sections of the page
        public List<Player> Players { get; set; }

        public List<Game> PlayerSelectedGames { get; set; } = new List<Game>();

        public class PlayerLastGameSummary
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? LastGame { get; set; }
        }

        public List<PlayerLastGameSummary> PlayersLastGameList { get; set; }

        public List<Game> Games { get; set; }

        public string SelectedPlayerUsername { get; set; }
        public List<Game> PlayerGames { get; set; }

        public class PlayerGamesCount
        {
            public string Username { get; set; }
            public int GamesPlayed { get; set; }
        }

        public List<PlayerGamesCount> PlayersNumerOfGames { get; set; }

        public Dictionary<int, List<Player>> PlayersByGamesCount { get; set; }
        public Dictionary<string, List<Player>> PlayersByCountry { get; set; }

        public async Task<IActionResult> OnGetAsync(string? selectedPlayerUsername = null)
        {
            // Players list
            Players = await _context.TblUsers.OrderBy(p => p.FirstName).ToListAsync();

            // Players and their last game's date
            PlayersLastGameList = await _context.TblUsers
                .OrderByDescending(p => p.FirstName)
                .Select(p => new PlayerLastGameSummary
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    LastGame = _context.TblGames
                        .Where(g => g.Username == p.Username)
                        .OrderByDescending(g => g.GameStartTime)
                        .Select(g => g.GameStartTime)
                        .FirstOrDefault()
                })
                .ToListAsync();

            // All games
            Games = await _context.TblGames.ToListAsync();

            // Games of the selected player
            if (!string.IsNullOrEmpty(selectedPlayerUsername))
            {
                PlayerSelectedGames = await _context.TblGames
                    .Where(g => g.Username == selectedPlayerUsername)
                    .ToListAsync();
            }
            else
            {
                PlayerSelectedGames = new List<Game>(); // Assuming "Game" is the correct type
            }

            // Count of games each player played
            PlayersNumerOfGames = await _context.TblUsers
                .Select(p => new PlayerGamesCount
                {
                    Username = p.Username,
                    GamesPlayed = p.GamesPlayed
                })
                .ToListAsync();

            // Group players by the number of games they've played
            PlayersByGamesCount = await _context.TblUsers
                .GroupBy(p => p.GamesPlayed)
                .ToDictionaryAsync(g => g.Key, g => g.ToList());

            // Group players by country
            PlayersByCountry = await _context.TblUsers
                .GroupBy(p => p.Country)
                .ToDictionaryAsync(g => g.Key, g => g.ToList());

            return Page();
        }

    }
}