using System.ComponentModel.DataAnnotations;

namespace ConnectFourServer.Models
{
    public class Player
    {
        [Key]
        public string Username { get; set; }
        public int ChoiceID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public int GamesPlayed { get; set; }

        // represent the relationship between player and game
        //public List<Game> Games { get; set; }

    }
}
