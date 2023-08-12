using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ConnectFourClient
{
    public partial class ConnectFourGame : Form
    {
        private AboutBox ab;
        private DatabaseBox db;
        GameAPIClient gameAPIClient = new GameAPIClient();

        // game board info
        private const int ROWS = 6;
        private const int COLS = 7;
        private const int WINNING_NUMBER = 4;
        private const int EMPTY = 0;
        private const int USER_TOKEN = 1;
        private const int COMPUTER_TOKEN = 2;
        private int[,] GameBoard;
        private List<Token> BoardTokens = new List<Token>(); // list to store token during animation
        private int currentPlayer = USER_TOKEN;
        private const int DROP_SPEED_ANIMATION = 30;
        private Timer animationTimer; // timer for the animation


        // button info
        private bool AboutClicked = false;
        private bool RestartClicked = false;
        private bool PauseClicked = false;
        private bool NewGameClicked = false;

        private bool GAME_LOADED = false;

        private DateTime StartTime;

        // user and game info
        private string USER_NAME;
        private int GAME_ID;
        private int GAME_ID_TO_SERVER;


        // game duration
        //private bool isUpdatingGameDuration = false;
        private DateTime GameStartTime;
        private TimeSpan GameDuration;
        private Timer GameTimer;


        //
        // Token info
        private const int TOKEN_SIZE = 80; 
        private const int START_X = 105; // starting x position of the game board
        private const int START_Y = 229; // starting y position of the game board

        private const int X_OFFSET = 90; // offset between each column
        private const int Y_OFFSET = 87; // offset between each row
        //

        private bool IsUserTurn = true;
        private bool IsComputerTurn = false;
        private bool GameOver = false;
        private bool isAnimating = false;
        private bool WON = false;
        private bool DRAW = false;
        private bool LOST = false;
        

        private Random rand = new Random(); // random primarily for the computer's move

        private TimeSpan totalPausedTime = TimeSpan.Zero;



        public ConnectFourGame(string username, int gameid)
        {
            USER_NAME = username.ToLower();
            GAME_ID = gameid;
            GAME_ID_TO_SERVER = gameid;
            InitializeComponent();
            MaximizeBox = false;
        }

        private void ConnectFourGame_Load(object sender, EventArgs e)
        {
            InitializeGameBoard();

            animationTimer = new Timer();
            animationTimer.Interval = 32;
            animationTimer.Tick += new EventHandler(AnimationTimer_Tick);

            this.Paint += new PaintEventHandler(ConnectFourGame_Paint);

            UpdateTurnLbl(currentPlayer);
            UpdateWelcomeLbl();
            UpdateGameDurationLbl();

            PauseGameBtn.Enabled = false;
            NewGameBtn.Enabled = false;
            EndGameBtn.Enabled = false;

            db = new DatabaseBox();
        }
        private void InitializeGameBoard()
        {
            // initialize the game board
            GameBoard = new int[ROWS, COLS];
            for (int i = 0; i < ROWS; i++)
                for (int j = 0; j < COLS; j++) 
                    GameBoard[i, j] = EMPTY;
        }


        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            Timer timer = (Timer)sender;
            Point p = (Point)timer.Tag;

            int row = p.X;
            int col = p.Y;

            int currentY = BoardTokens.Find(t => t.Row == row && t.Column == col).Step;
            int nextY = currentY + DROP_SPEED_ANIMATION;

            if (nextY < START_Y + (row + 1) * Y_OFFSET)
            {
                BoardTokens.Find(t => t.Row == row && t.Column == col).Step = nextY;
                this.Invalidate();
            }
            else
            {
                animationTimer.Enabled = false;
                BoardTokens.Find(t => t.Row == row && t.Column == col).Step = START_Y + (row + 1) * Y_OFFSET;

                GameBoard[row, col] = currentPlayer;

                this.Invalidate();
                if (!CheckWinner())
                {
                    if (currentPlayer == USER_TOKEN)
                    {
                        IsUserTurn = false;
                        IsComputerTurn = true;
                        currentPlayer = COMPUTER_TOKEN;
                        UpdateTurnLbl(currentPlayer);
                    }
                    else
                    {
                        IsUserTurn = true;
                        IsComputerTurn = false;
                        currentPlayer = USER_TOKEN;
                        UpdateTurnLbl(currentPlayer);
                    }
                }

                isAnimating = false;
                EnableColBtn(true);
                //CheckWinner();
                ComputerMove();
                EnableColBtn(true);
                EndGameBtn.Enabled = true;
                PauseGameBtn.Enabled = true;
                NewGameBtn.Enabled = true;
                RestartGameBtn.Enabled = true;
                if (WON || DRAW || LOST || GameOver)
                    EnableColBtn(false);
                if (WON || DRAW || LOST || GameOver)
                { 
                    PauseGameBtn.Enabled = false;
                }
            }
        }

        private async void ComputerMove()
        {
            if (IsComputerTurn)
            {
                int randCol = await gameAPIClient.GetRandomColumn();
                //int randCol = rand.Next(0, COLS);
                int randRow = GetNextAvailableRow(randCol);
               do
               {
                    randCol = await gameAPIClient.GetRandomColumn();
                    //randCol = rand.Next(0, COLS);
                    if (randCol == -1)
                        break;
                    randRow = GetNextAvailableRow(randCol);
                }while (randRow == -1 && !(WON || DRAW || LOST || GameOver)) ;
                

                if (!WON && !DRAW && !LOST && !GameOver)
                {
                    Button[] DropBtn = { DropBtnCol1, DropBtnCol2, DropBtnCol3, DropBtnCol4, DropBtnCol5, DropBtnCol6, DropBtnCol7 };
                    if (randCol >= 0 && randCol < DropBtn.Length)
                    {
                        DropBtn[randCol].PerformClick();
                        IsComputerTurn = false;
                    }
                    else
                    {
                        WelcomeLabel.Text = "Computer has no moves left";
                        this.Invalidate();
                    }
                }
            }
        }

        private bool CheckWinner()
        {
            for(int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    int player = GameBoard[row, col];
                    if (player != EMPTY)
                    {
                        // check horizontal
                        if (col + 3 < COLS && GameBoard[row, col] == player &&
                            GameBoard[row, col + 1] == player &&
                            GameBoard[row, col + 2] == player &&
                            GameBoard[row, col + 3] == player)

                        {
                            DeclareWinner(player);
                            return true;
                        }

                        // check vertical
                        if (row + 3 < ROWS && GameBoard[row, col] == player &&
                            GameBoard[row + 1, col] == player &&
                            GameBoard[row + 2, col] == player &&
                            GameBoard[row + 3, col] == player)
                        {
                            DeclareWinner(player);
                            return true;
                        }

                        // check diagonal left
                        if (col + 3 < COLS &&
                            row + 3 < ROWS &&
                            GameBoard[row, col] == player &&
                            GameBoard[row + 1, col + 1] == player &&
                            GameBoard[row + 2, col + 2] == player &&
                            GameBoard[row + 3, col + 3] == player)
                        {
                            DeclareWinner(player);
                            return true;
                        }

                        // check diagonal right
                        if (col >= WINNING_NUMBER - 1 &&
                            row <= ROWS - WINNING_NUMBER &&
                            GameBoard[row, col] == player &&
                            GameBoard[row + 1, col - 1] == player &&
                            GameBoard[row + 2, col - 2] == player &&
                            GameBoard[row + 3, col - 3] == player)
                        {
                            DeclareWinner(player);
                            return true;
                        }
                    }


                }
            }
            CheckDraw();
            return false;
        }

        private bool CheckDraw()
        {
            int maxSteps = 0;
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    if (GameBoard[row, col] != EMPTY)
                    {
                        maxSteps++;
                    }
                }
            }
            if (maxSteps == ROWS * COLS)
            {
                GameTimer.Stop();
                WelcomeLabel.Text = "It's a Draw, Better Luck Next Time";
                GameOver = true;
                //InitializeGameBoard();
                this.Invalidate();
                DRAW = true;
                EnableColBtn(false);
                GameDuration = DateTime.Now - GameStartTime;
                PauseGameBtn.Enabled = false;
                TurnLabel.Text = "Game Over";
                TurnLabel.ForeColor = Color.Black;
                db.SaveToDatabase(USER_NAME, ref GAME_ID, WON, DRAW, LOST, GameDuration, StartTime, GameBoard, BoardTokens);
                return true;
            }
            return false;
        }

        private void DeclareWinner(int player)
        {
            GameTimer.Stop();
            //string winnerName = (player == USER_TOKEN) ? "You" : "Computer";
            if (player == USER_TOKEN)
            {
                WelcomeLabel.Text = "You Won The Game! Well Done!";
                this.Invalidate();
                WON = true;
            } 
            else
            { 
                WelcomeLabel.Text = "You Lost The Game! Better Luck Next Time!";
                this.Invalidate();
                LOST = true;
            }
            GameOver = true;
            EnableColBtn(false);
            currentPlayer = player;
            GameDuration = DateTime.Now - GameStartTime;
            PauseGameBtn.Enabled = false;
            TurnLabel.Text = "Game Over";
            TurnLabel.ForeColor = Color.Black;
            //db.SaveToDatabase(USER_NAME, ref GAME_ID, WON, DRAW, GameDuration, GameBoard, BoardTokens);
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            //isUpdatingGameDuration = true;
            TimeSpan duration = DateTime.Now - GameStartTime;
            string durationString = $"{duration.Hours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}";
            GameDurationLabel.Text = durationString;
            //isUpdatingGameDuration = false;

        }

        private void DropBtnCol_Click(object sender, EventArgs e)
        {
            
            if (isAnimating || GameOver)
                return;

            if(!GameOver)
            {
                if(!GAME_LOADED)
                    StartTime = DateTime.Now;
                EndGameBtn.Enabled = true;
                PauseGameBtn.Enabled = true;
                NewGameBtn.Enabled = true;
                RestartGameBtn.Enabled = true;
                // enable the buttons because the game started and can be paused/ended or begin a new game
                if (IsComputerTurn)
                {
                    PauseGameBtn.Enabled = false;
                    NewGameBtn.Enabled = false;
                    EndGameBtn.Enabled = false;
                    RestartGameBtn.Enabled = false;
                }

                // get the column number
                Button dBtn = (Button)sender;
                int col = Convert.ToInt32(dBtn.Tag);

                // find the first empty row in the column
                int row = GetNextAvailableRow(col);

                if(row >= 0)
                {
                    if(!GameTimer.Enabled)
                    {
                        GameStartTime = DateTime.Now;
                        GameTimer.Start();
                    }
                    animationTimer.Stop();
                    DropToken(row, col, currentPlayer);


                    animationTimer.Start();
                }
                else
                {
                    WelcomeLabel.Text = "This column is full, please select another one";
                    this.Invalidate();
                }
            }
        }


        private int GetNextAvailableRow(int col)
        {
            if(col != -1)
                for (int row = ROWS - 1; row >= 0; row--)
                {
                    if (GameBoard[row, col] == EMPTY)
                    {
                        return row;
                    }
                }
            return -1;
        }


        private void DropToken(int row, int col, int player)
        {
            int startX = START_X + (col + 1) * X_OFFSET;
            int startY = START_Y;
            
            int targetY = startY + (row + 1) * Y_OFFSET;

            GameBoard[row, col] = EMPTY;

            BoardTokens.Add(new Token(row, col, currentPlayer, startY));

            animationTimer.Tag = new Point(row, col);
            animationTimer.Enabled = true;
            isAnimating = true;
            EnableColBtn(false);

        }


        private void AboutBtn_Click(object sender, EventArgs e)
        {
            ab = new AboutBox();
            AboutClicked = true;
            UpdateWelcomeLbl();
            ab.ShowAboutBox();
        }


        private void DatabaseBtn_Click(object sender, EventArgs e)
        {
            db = new DatabaseBox();
            db.USER_NAME_db = USER_NAME.ToLower();
            db.GAME_ID_db = GAME_ID;
            db.OnReplayGameSelected += Db_ReplayGame_ConfirmBtnClicked;
            db.Show();

        }    
        private void Db_ReplayGame_ConfirmBtnClicked(int gameid)
        {
            GAME_ID = gameid;
            GAME_LOADED = db.LoadFromDatabase(USER_NAME, GAME_ID, ref WON, ref DRAW, ref LOST, ref GameDuration, ref StartTime, ref GameBoard, ref BoardTokens);
            GAME_ID_TO_SERVER = GAME_ID;
            if (GAME_LOADED)
            {
                db.GAME_ID_db = GAME_ID;
                this.Invalidate();
                if(WON || DRAW || LOST)
                {
                    GameOver = true;
                    PauseGameBtn.Enabled = false;
                    UpdateWelcomeLbl();
                    UpdateGameLoaded();
                    GameTimer.Stop();
                    RestartGameBtn.Enabled = true;
                    NewGameBtn.Enabled = true;
                    EndGameBtn.Enabled = false;
                    EnableColBtn(false);
                    string durationString = $"{GameDuration.Hours:D2}:{GameDuration.Minutes:D2}:{GameDuration.Seconds:D2}";
                    GameDurationLabel.Text = durationString;
                    this.Invalidate();
                }
                else
                {
                    GameOver = false;
                    PauseGameBtn.Enabled = true;
                    NewGameBtn.Enabled = true;
                    UpdateWelcomeLbl();
                    UpdateGameLoaded();
                    UpdateGameDurationLbl();
                    GameTimer.Start();
                    string durationString = $"{GameDuration.Hours:D2}:{GameDuration.Minutes:D2}:{GameDuration.Seconds:D2}";
                    GameDurationLabel.Text = durationString;
                    RestartGameBtn.Enabled = true;
                    EndGameBtn.Enabled = true;
                    EnableColBtn(true);
                    this.Invalidate();
                }
            }
            else
            {
                WelcomeLabel.Text = "Game Loading Failed";
                this.Invalidate();
            }
        }



        // pause the game and save the game to the database
        private void PauseGameBtn_Click(object sender, EventArgs e)
        {
            if (!GameOver)
            {
                GameOver = true;
                PauseClicked = true;
                GAME_LOADED = false;
                UpdateWelcomeLbl();
                GameTimer.Stop();
                RestartGameBtn.Enabled = false;
                PauseGameBtn.Enabled = false;
                EndGameBtn.Enabled = false;
                EnableColBtn(false);
                GameDuration = DateTime.Now - GameStartTime;
                string durationString = $"{GameDuration.Hours:D2}:{GameDuration.Minutes:D2}:{GameDuration.Seconds:D2}";
                db.SaveToDatabase(USER_NAME, ref GAME_ID, WON, DRAW, LOST, GameDuration, StartTime, GameBoard, BoardTokens);
                UpdateTheServer(USER_NAME, GAME_ID_TO_SERVER, WON, DRAW, LOST, durationString, StartTime);
                db.GAME_ID_db = GAME_ID;
                GAME_ID_TO_SERVER = GAME_ID;
            }
        }

        // restart the game and clear the board
        private void RestartGameBtn_Click(object sender, EventArgs e)
        {
            RestartClicked = true;
            GAME_LOADED = false;
            UpdateWelcomeLbl();
            GameTimer.Stop();
            RestartClicked = false;
            PauseGameBtn.Enabled = false;
            NewGameBtn.Enabled = false;
            RestartGameBtn.Enabled = false;
            EndGameBtn.Enabled = false;
            UpdateGameDurationLbl();
            WelcomeLabel.Text = "The Game Was Restarted, Enjoy " + USER_NAME;
            // clear the game board
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    GameBoard[i, j] = EMPTY;
                }
            }
            // clear the tokens
            BoardTokens.Clear();
            currentPlayer = USER_TOKEN;
            IsUserTurn = true;
            IsComputerTurn = false;
            isAnimating = false;
            //isUpdatingGameDuration = false;  
            UpdateTurnLbl(currentPlayer);
            EnableColBtn(true);
            GameOver = false;
            WON = false;
            DRAW = false;
            LOST = false;
        }


        private void NewGameBtn_Click(object sender, EventArgs e)
        {
            //int AvailableGameIDFromDB = GetAvailableGameIDFromDB(USER_NAME, GAME_ID, WON, DRAW, duration, GameBoard, BoardTokens);
            //GAME_ID = AvailableGameIDFromDB;
            GAME_LOADED = false;
            NewGameClicked = true;
            UpdateWelcomeLbl();
            GameTimer.Stop();
            NewGameClicked = false;
            PauseGameBtn.Enabled = false;
            NewGameBtn.Enabled = false;
            EndGameBtn.Enabled = false;
            GameDuration = DateTime.Now - GameStartTime;
            if (!PauseClicked)
            {
                db.SaveToDatabase(USER_NAME, ref GAME_ID, WON, DRAW, LOST, GameDuration, StartTime, GameBoard, BoardTokens);
                string durationString = $"{GameDuration.Hours:D2}:{GameDuration.Minutes:D2}:{GameDuration.Seconds:D2}";
                UpdateTheServer(USER_NAME, GAME_ID_TO_SERVER, WON, DRAW, LOST, durationString, StartTime);
                db.GAME_ID_db = GAME_ID;
                GAME_ID_TO_SERVER = GAME_ID;
            }
            PauseClicked = false;
            UpdateGameDurationLbl();
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    GameBoard[i, j] = EMPTY;
                }
            }
            BoardTokens.Clear();
            currentPlayer = USER_TOKEN;
            IsUserTurn = true;
            IsComputerTurn = false;
            isAnimating = false;
            //isUpdatingGameDuration = false;
            RestartGameBtn.Enabled = true;
            EnableColBtn(true);
            GameOver = false;
            WON = false;
            DRAW = false;
            LOST = false;
        }


        private void EndGameBtn_Click(object sender, EventArgs e)
        {
            if(!PauseClicked && !GAME_LOADED)
            {
                if (GameOver)
                {
                    DialogResult res = MessageBox.Show("Your game will be saved in the Database.\n" +
                    "You can return to It Any Time you Like and Replay it.", "End Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        GameTimer.Stop();
                        GameDuration = DateTime.Now - GameStartTime;
                        db.SaveToDatabase(USER_NAME, ref GAME_ID, WON, DRAW, LOST, GameDuration, StartTime, GameBoard, BoardTokens);
                        string durationString = $"{GameDuration.Hours:D2}:{GameDuration.Minutes:D2}:{GameDuration.Seconds:D2}";
                        UpdateTheServer(USER_NAME, GAME_ID_TO_SERVER, WON, DRAW, LOST, durationString, StartTime);
                        Application.Exit();
                    }
                }
                else if ((!WON || !DRAW || !LOST) && !GameOver)
                {
                    DialogResult res = MessageBox.Show("Are you sure you want to End the Game?\n" +
                    "Your game will be saved in the Database.\n" +
                    "You can return to It Any Time you Like.", "End Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        GameTimer.Stop();
                        GameDuration = DateTime.Now - GameStartTime;
                        db.SaveToDatabase(USER_NAME, ref GAME_ID, WON, DRAW, LOST, GameDuration, StartTime, GameBoard, BoardTokens);
                        string durationString = $"{GameDuration.Hours:D2}:{GameDuration.Minutes:D2}:{GameDuration.Seconds:D2}";
                        UpdateTheServer(USER_NAME, GAME_ID_TO_SERVER, WON, DRAW, LOST, durationString, StartTime);
                        Application.Exit();
                    }
                }
            }
            else
            {
                if(GAME_LOADED)
                {
                    if (GameOver)
                    {
                        DialogResult res = MessageBox.Show("Your game will be updated in the Database.\n" +
                        "You can return to It Any Time you Like and Replay it.", "End Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (res == DialogResult.Yes)
                        {
                            GameTimer.Stop();
                            GameDuration = DateTime.Now - GameStartTime;
                            db.SaveToDatabase(USER_NAME, ref GAME_ID, WON, DRAW, LOST, GameDuration, StartTime, GameBoard, BoardTokens);
                            string durationString = $"{GameDuration.Hours:D2}:{GameDuration.Minutes:D2}:{GameDuration.Seconds:D2}";
                            UpdateTheServer(USER_NAME, GAME_ID_TO_SERVER, WON, DRAW, LOST, durationString, StartTime);
                            Application.Exit();
                        }
                    }
                    else if ((!WON || !DRAW || !LOST) && !GameOver)
                    {
                        DialogResult res = MessageBox.Show("Are you sure you want to End the Game?\n" +
                        "Your game will be updated in the Database.\n" +
                        "You can return to It Any Time you Like.", "End Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (res == DialogResult.Yes)
                        {
                            GameTimer.Stop();
                            GameDuration = DateTime.Now - GameStartTime;
                            db.SaveToDatabase(USER_NAME, ref GAME_ID, WON, DRAW, LOST, GameDuration, StartTime, GameBoard, BoardTokens);
                            string durationString = $"{GameDuration.Hours:D2}:{GameDuration.Minutes:D2}:{GameDuration.Seconds:D2}";
                            UpdateTheServer(USER_NAME, GAME_ID_TO_SERVER, WON, DRAW, LOST, durationString, StartTime);
                            Application.Exit();
                        }
                    }
                }
                else if(PauseClicked)
                {
                    if (GameOver)
                    {
                        DialogResult res = MessageBox.Show("Your game is already saved in the Database.\n" +
                        "You can return to It Any Time you Like and Replay it.", "End Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (res == DialogResult.Yes)
                        {
                            Application.Exit();
                        }
                    }
                    else if ((!WON || !DRAW || !LOST) && !GameOver)
                    {
                        DialogResult res = MessageBox.Show("Are you sure you want to End the Game?\n" +
                        "Your game is already saved in the Database.\n" +
                        "You can return to It Any Time you Like.", "End Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (res == DialogResult.Yes)
                        {
                            Application.Exit();
                        }
                    }
                }

            }

        


        }

        // paint the game board and the tokens
        private void ConnectFourGame_Paint(object sender, PaintEventArgs e)
        {
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    int circleSize = TOKEN_SIZE + 3;
                    int circleX = START_X + (col + 1) * X_OFFSET;
                    int circleY = START_Y + (row + 1) * Y_OFFSET;

                    Rectangle circleRect = new Rectangle(circleX - circleSize / 2, circleY - circleSize / 2, circleSize, circleSize);
                    e.Graphics.FillEllipse(Brushes.MidnightBlue, circleRect);

                    int player = GameBoard[row, col];
                    if (player != EMPTY)
                    {
                        if (player == USER_TOKEN)
                        {
                            e.Graphics.FillEllipse(Brushes.Red, circleRect);
                        }
                        else
                        {
                            e.Graphics.FillEllipse(Brushes.Yellow, circleRect);
                        }
                    }
                }
            }

            foreach (var token in BoardTokens)
            {
                int circleSize = TOKEN_SIZE;
                int circleX = START_X + (token.Column + 1) * X_OFFSET;
                int circleY = token.Step;
                Rectangle circleRect = new Rectangle(circleX - circleSize / 2, circleY - circleSize / 2, circleSize, circleSize);

                if (token.Player == USER_TOKEN)
                {
                    e.Graphics.FillEllipse(Brushes.Red, circleRect);
                }
                else
                {
                    e.Graphics.FillEllipse(Brushes.Yellow, circleRect);
                }

                DrawCrown(circleX, circleY, e.Graphics);

            }
        }
        private void DrawCrown(int circleX, int circleY, Graphics e)
        {
            int crownWidth = TOKEN_SIZE / 2;
            int crownHeight = TOKEN_SIZE / 4;
            int crownTopHeight = TOKEN_SIZE / 6;
            int crownBottomHeight = crownHeight - crownTopHeight;

            Point[] points = new Point[7];

            // top crown
            points[0] = new Point(circleX - crownWidth / 2, circleY - crownHeight / 2);
            points[1] = new Point(circleX - crownWidth / 4, circleY - crownHeight / 2 + crownTopHeight);
            points[2] = new Point(circleX, circleY - crownHeight / 2);
            points[3] = new Point(circleX + crownWidth / 4, circleY - crownHeight / 2 + crownTopHeight);
            points[4] = new Point(circleX + crownWidth / 2, circleY - crownHeight / 2);


            // bottom crown
            points[5] = new Point(circleX + crownWidth / 4, circleY - crownHeight / 2 + crownBottomHeight);
            points[6] = new Point(circleX - crownWidth / 4, circleY - crownHeight / 2 + crownBottomHeight);

            e.FillPolygon(Brushes.Gold, points);

        }


        // update labels
        private void UpdateTurnLbl(int currentPlayer)
        {

            if (WON == false && DRAW == false && LOST == false)
            {
                string playerName = (currentPlayer == USER_TOKEN) ? "Your Turn" : "Computer's Turn";
                Color playerColor = (currentPlayer == USER_TOKEN) ? Color.Red : Color.Yellow;

                TurnLabel.Text = playerName;
                TurnLabel.ForeColor = playerColor;
            }
        }
        private void UpdateWelcomeLbl()
        {
            if (GAME_ID > 1)
            {
                string str1 = "Hey there " + USER_NAME + "! Welcome Back!";
                string str2 = "Hello " + USER_NAME + ", How are You Today? Ready to Play?";
                string str3 = "Welcome Back " + USER_NAME + "! Ready to Play?";

                int num = rand.Next(1, 4);

                switch (num)
                {
                    case 1:
                        WelcomeLabel.Text = str1;
                        break;
                    case 2:
                        WelcomeLabel.Text = str2;
                        break;
                    case 3:
                        WelcomeLabel.Text = str3;
                        break;
                    default:
                        WelcomeLabel.Text = str1;
                        break;
                }
                this.Invalidate();
            }
            else
            {

                WelcomeLabel.Text = "Welcome to Connect Four Game " + USER_NAME +
                    "!\nIf you are not familiar with the game click on the about button";
                this.Invalidate();
                if (AboutClicked)
                {
                    WelcomeLabel.Text = "Hope it Helped you learn the rules and the basics of the game, Enjoy!";
                }
                this.Invalidate();
            }


            if (RestartClicked)
            {
                RestartClicked = false;
                WelcomeLabel.Text = "The Game Was Restarted, Enjoy " + USER_NAME;

                TurnLabel.Text = "Your Turn";
                this.Invalidate();
            }

            if (PauseClicked)
            {
                //PauseClicked = false;
                WelcomeLabel.Text = "The Game Was Paused, and saved into the Database,\nyou can find it and continue playing";
                this.Invalidate();
            }
            if (NewGameClicked)
            {
                WelcomeLabel.Text = "New Game for you " + USER_NAME;
                TurnLabel.Text = "Your Turn";
                this.Invalidate();
            }

            if(WON)
            {
                WelcomeLabel.Text = "You Won Well Done";
                this.Invalidate();
            }
            else if(DRAW)
            {
                WelcomeLabel.Text = "It is a Draw";
                this.Invalidate();
            }
            else if(LOST)
            {
                WelcomeLabel.Text = "You Lost, Better Luck Next Time";
                this.Invalidate();
            }
        }

        private void UpdateGameLoaded()
        {
            if (GAME_LOADED)
            {
                if (WON || DRAW || LOST)
                {
                    WelcomeLabel.Text = "Game Loaded Successfully, Game #" + GAME_ID + ".\nYou can replay it if you like or just view it alright" + USER_NAME + "?";
                    if (WON)
                    {
                        WelcomeLabel.Text += "\nYou Won this Game!";
                    }
                    else if (DRAW)
                    {
                        WelcomeLabel.Text += "\nIt was a Draw!";

                    }
                    else if (LOST)
                    {
                        WelcomeLabel.Text += "\nYou Lost this Game!";
                    }
                    TurnLabel.Text = "Game Over";
                    TurnLabel.ForeColor = Color.Black;
                    this.Invalidate();
                }
                else
                {
                    WelcomeLabel.Text = "Game Loaded Successfully, Enjoy finishing playing Game #" + GAME_ID + " " + USER_NAME + "!";
                    TurnLabel.Text = "Your Turn";
                    TurnLabel.ForeColor = Color.Red;
                    this.Invalidate();
                }
            }
            else if (!GAME_LOADED)
            {
                WelcomeLabel.Text = "Game did not load successfully, please try again";
                this.Invalidate();
            }
        }

        private void UpdateGameDurationLbl()
        {
            GameTimer = new Timer();
            GameTimer.Interval = 1000;
            GameTimer.Tick += GameTimer_Tick;
        }

        // enable/disable drop buttons
        private void EnableColBtn(bool enable)
        {
            DropBtnCol1.Enabled = enable;
            DropBtnCol2.Enabled = enable;
            DropBtnCol3.Enabled = enable;
            DropBtnCol4.Enabled = enable;
            DropBtnCol5.Enabled = enable;
            DropBtnCol6.Enabled = enable;
            DropBtnCol7.Enabled = enable;
        }

        private async void UpdateTheServer(string username, int gameid, bool w, bool d, bool l, string durationString, DateTime StartTime)
        {
            GameAPIClient gameAPIClient = new GameAPIClient();

            string gameresult = w ? "Won" : (l ? "Lost" : (d ? "Draw" : "Not finished"));

            Game game = new Game
            {
                Username = username,
                GameId = gameid,
                GameDuration = durationString,
                GameResult = gameresult,
                GameStartTime = StartTime,
            };

            bool IsSaveGameToServer = await gameAPIClient.SaveGame(game);
            if (IsSaveGameToServer)
            {
                MessageBox.Show("Game Saved Successfully to the Server");
            }
            else
            {
                MessageBox.Show("Game did not save successfully to the Server");
            }
        }
    }
}
