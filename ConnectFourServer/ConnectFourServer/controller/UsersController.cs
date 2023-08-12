using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ConnectFourServer.Models;
using ConnectFourServer.Data;
using Microsoft.EntityFrameworkCore;

namespace ConnectFourServer.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ServerDatabase _context;

        public UsersController(ServerDatabase context)
        {
            _context = context;
        }

        [HttpGet("getrandomcolumn")]
        public int GetRandomColumn()
        {
            int randCol = GenerateRandomColumn();
            return randCol;
        }

        private int GenerateRandomColumn()
        {
            return new System.Random().Next(0, 7);
        }
        [HttpPost("CheckUsername")]
        public async Task<ActionResult<bool>> CheckUsername([FromBody] string username)
        {
            var userCheck = _context.TblUsers.Where(u => u.Username.ToLower() == username.ToLower()).FirstOrDefault();
            if (userCheck == null)
            {
                return Ok(false);
            }
            return Ok(true);
        }


        public class GameData
        {
            public string Username { get; set; }
            public int GameId { get; set; }
            public string GameDuration { get; set; }
            public string GameResult { get; set; }
            public DateTime GameStartTime { get; set; }
        }


        [HttpPost("SaveGame")]
        public async Task<ActionResult<bool>> SaveGame([FromBody] GameData gameData)
        {

            var player = await _context.TblUsers.Where(p => p.Username.ToLower() == gameData.Username.ToLower()).FirstOrDefaultAsync();
            if (player != null)
            {
                var checkGame = await _context.TblGames.Where(g => g.GameId == gameData.GameId && g.Username == gameData.Username).FirstOrDefaultAsync();
                
                Game game = new Game();
                game.Username = gameData.Username;
                game.GameId = gameData.GameId;
                game.GameDuration = gameData.GameDuration;
                game.GameResult = gameData.GameResult;
                game.GameStartTime = gameData.GameStartTime;

                if (checkGame == null)
                {
                    _context.TblGames.Add(game);
                    player.GamesPlayed++;
                    _context.TblUsers.Update(player);
                }
                else
                {
                    checkGame.Username = gameData.Username;
                    checkGame.GameId = gameData.GameId;
                    checkGame.GameDuration = gameData.GameDuration;
                    checkGame.GameResult = gameData.GameResult;
                    checkGame.GameStartTime = gameData.GameStartTime;
                    _context.TblGames.Update(checkGame);
                }
                    

                await _context.SaveChangesAsync();
                return Ok(true);
            }

                return Ok(false);
        }

        



    }
}
