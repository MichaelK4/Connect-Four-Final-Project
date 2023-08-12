using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace ConnectFourClient
{
    public partial class DatabaseBox : Form
    {
        public event Action<int> OnReplayGameSelected;

        public string USER_NAME_db { get; set; }
        public int GAME_ID_db { get; set; }
        public int GAME_ID_REPLAY { get; set; }

        public bool REPLAY { get; set; }    

        private int ROWS = 6;
        private int COLUMNS = 7;

        private const string connStr = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"Put your own path to the .mdf file here\\Database.mdf\";Integrated Security=True";
        private DataTable tbl;
        public DatabaseBox()
        {
            InitializeComponent();
            tbl = new DataTable();
            MaximizeBox = false;
        }

        private void DatabaseBox_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'databaseDataSet.TblConnectFourGames' table. You can move, or remove it, as needed.
            this.tblConnectFourGamesTableAdapter.Fill(this.databaseDataSet.TblConnectFourGames);
            tbl = AllGames();
            dataGridView1.DataSource = tbl;
        }



        private void AllGamesBtn_Click(object sender, EventArgs e)
        {
            tbl = AllGames();
            dataGridView1.DataSource = tbl;
        }

        private void UnfinishedGamesBtn_Click(object sender, EventArgs e)
        {
            tbl = UnfinishedGames();
            dataGridView1.DataSource = tbl;
        }

        private void FinishedGamesBtn_Click(object sender, EventArgs e)
        {
            tbl = FinishedGames();
            dataGridView1.DataSource = tbl;
        }

        public bool SaveToDatabase(string username, ref int gameid, bool w, bool d, bool l , TimeSpan duration, DateTime StartTime, int[,]board, List<Token> tokens)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open(); //TblAllUsersLocal
                    string query = "IF EXISTS (SELECT 1 FROM TblConnectFourGames " +
                        "WHERE Username = @Username AND GameID = @GameID) " +
                        "BEGIN " +
                        "   UPDATE TblConnectFourGames " +
                        "   SET Won = @Won, Draw = @Draw,Lost = @Lost ,Duration = @Duration, StartTime = @StartTime, GameBoard = @GameBoard, BoardTokens = @BoardTokens " +
                        "   WHERE Username = @Username AND GameID = @GameID " +
                        "END " +
                        "ELSE " +
                        "BEGIN " +
                        "   INSERT INTO TblConnectFourGames (Username, GameID, Won, Draw, Lost,Duration, StartTime, GameBoard , BoardTokens) " +
                        "   VALUES (@Username, @GameID, @Won, @Draw, @Lost, @Duration, @StartTime, @GameBoard , @BoardTokens) " +
                        "END";
                    using(SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int won = 0;
                        int draw = 0;
                        int lost = 0;
                        if (w)
                            won = 1;
                        else
                            won = 0;
                        if(d)
                            draw = 1;
                        else
                            draw = 0;
                        if(l)
                            lost = 1;
                        else
                            lost = 0;

                        string durationString = $"{duration.Hours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}";


                        cmd.Parameters.AddWithValue("@Username", username.ToLower());
                        cmd.Parameters.AddWithValue("@GameID", gameid);
                        cmd.Parameters.AddWithValue("@Won", won);
                        cmd.Parameters.AddWithValue("@Draw", draw);
                        cmd.Parameters.AddWithValue("@Lost", lost);
                        cmd.Parameters.AddWithValue("@Duration", durationString);
                        cmd.Parameters.AddWithValue("@StartTime", StartTime);
                        cmd.Parameters.AddWithValue("@GameBoard", GameBoardToStrJson(board));
                        cmd.Parameters.AddWithValue("@BoardTokens", BoardTokensToStrJSON(tokens));
                        
                        cmd.ExecuteNonQuery();
                       
                        gameid = GetLatestGameID(username.ToLower());

                        gameid++;


                        return true;
                    }
                }
            }
            catch (Exception e)
            {

                MessageBox.Show("Error saving game to database: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private int GetLatestGameID(string username)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT TOP 1 GameID FROM TblConnectFourGames WHERE Username = @Username ORDER BY GameID DESC";
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username.ToLower());
                    object result = cmd.ExecuteScalar();
                    if(result != null && int.TryParse(result.ToString(), out int latestGameID))
                    {
                        return latestGameID;
                    }
                }
            }
            return -1;
        }

        private static string GameBoardToStrJson(int[,] board)
        {
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);

            int[] singleArr = new int[rows * cols];

            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    singleArr[i * cols + j] = board[i, j];
                }
            }
            string json = JsonConvert.SerializeObject(singleArr);
            return json;
        }

        private static string BoardTokensToStrJSON(List<Token> tokens)
        {
            string json = JsonConvert.SerializeObject(tokens);
            return json;
        }


        public bool LoadFromDatabase(string username, int gameid, ref bool w, ref bool d, ref bool l, ref TimeSpan duration, ref DateTime StartTime, ref int[,] loadBoard, ref List<Token> loadTokens)
        {
            try 
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = "SELECT " +
                        "Username, " +
                        "GameID, " +
                        "Won, " +
                        "Draw, " +
                        "Lost, " +
                        "Duration, " +
                        "StartTime ," +
                        "GameBoard, " +
                        "BoardTokens " +
                        "FROM TblConnectFourGames WHERE Username = @Username AND GameID = @GameID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username.ToLower());
                        cmd.Parameters.AddWithValue("@GameID", gameid);


                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                w = Convert.ToBoolean(reader["Won"]);
                                d = Convert.ToBoolean(reader["Draw"]);
                                l = Convert.ToBoolean(reader["Lost"]);
                                string durationString = reader["Duration"].ToString();
                                duration = TimeSpan.Parse(durationString);
                                StartTime = Convert.ToDateTime(reader["StartTime"]);
                                int[] Single = JsonConvert.DeserializeObject<int[]>(reader["GameBoard"].ToString());
                                loadTokens = JsonConvert.DeserializeObject<List<Token>>(reader["BoardTokens"].ToString());


                                for (int i = 0; i < ROWS; i++)
                                {
                                    for (int j = 0; j < COLUMNS; j++)
                                    {
                                      
                                        loadBoard[i, j] = Single[i * COLUMNS + j];
                                    }
                                }
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("Game data not found in the database for this user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading game from database: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private DataTable AllGames()
        {
            tbl = new DataTable();
            ReplayGame.Text = "Replay Game";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT " +
                    "Username, " +
                    "GameID, " +
                    "CASE WHEN Won = 1 THEN 'Yes' ELSE 'No' END AS Won, " +
                    "CASE WHEN Draw = 1 THEN 'Yes' ELSE 'No' END AS Draw, " +
                    "CASE WHEN Lost = 1 THEN 'Yes' ELSE 'No' END AS Lost, " +
                    "Duration, " +
                    "StartTime " +
                    "FROM TblConnectFourGames WHERE Username = @Username";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", USER_NAME_db.ToLower());
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(tbl);
                    }
                }
            }

            return tbl;
        }


        private DataTable UnfinishedGames()
        {
            tbl = new DataTable();
            ReplayGame.Text = "Finish Game";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT " +
                    "Username, " +
                    "GameID, " +
                    "CASE WHEN Won = 1 THEN 'Yes' ELSE 'No' END AS Won, " +
                    "CASE WHEN Draw = 1 THEN 'Yes' ELSE 'No' END AS Draw, " +
                    "CASE WHEN Lost = 1 THEN 'Yes' ELSE 'No' END AS Lost, " +
                    "Duration, " +
                    "StartTime " +
                    "FROM TblConnectFourGames WHERE Won = 0 AND Draw = 0 AND Lost = 0 AND Username = @Username";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", USER_NAME_db.ToLower());
                    //cmd.Parameters.AddWithValue("@GameID", GAME_ID_db);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(tbl);
                    }
                }
            }

            return tbl;
        }

        private DataTable FinishedGames()
        {
            tbl = new DataTable();
            ReplayGame.Text = "Replay Game";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT " +
                    "Username, " +
                    "GameID, " +
                    "CASE WHEN Won = 1 THEN 'Yes' ELSE 'No' END AS Won, " +
                    "CASE WHEN Draw = 1 THEN 'Yes' ELSE 'No' END AS Draw, " +
                    "CASE WHEN Lost = 1 THEN 'Yes' ELSE 'No' END AS Lost, " +
                    "Duration, " +
                    "StartTime " +
                    "FROM TblConnectFourGames WHERE (Won = 1 OR Draw = 1 OR Lost = 1) AND Username = @Username";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", USER_NAME_db.ToLower());
                    cmd.Parameters.AddWithValue("@GameID", GAME_ID_db);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(tbl);
                    }
                }
            }

            return tbl;
        }



        private DataTable AllUsers()
        {
            tbl = new DataTable();
            ReplayGame.Text = "Replay Game";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT " +
                    "Username, " +
                    "GameID, " +
                    "CASE WHEN Won = 1 THEN 'Yes' ELSE 'No' END AS Won, " +
                    "CASE WHEN Draw = 1 THEN 'Yes' ELSE 'No' END AS Draw, " +
                    "CASE WHEN Lost = 1 THEN 'Yes' ELSE 'No' END AS Lost, " +
                    "Duration, " +
                    "StartTime " +
                    "FROM TblConnectFourGames";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", USER_NAME_db.ToLower());
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(tbl);
                    }
                }
            }

            return tbl;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tbl = AllUsers();
            dataGridView1.DataSource = tbl;
        }

        private void ReplayGame_Click(object sender, EventArgs e)
        {

            int selctedGameID = PromptForGameID();

            if(selctedGameID != -1)
            {
                this.Close();
                OnReplayGameSelected?.Invoke(selctedGameID);
            }
        }


        private int PromptForGameID()
        {
            int gameid = -1;
            bool valid = false;


            while(!valid)
            {
                string input = PromptInputBox("Enter Game ID to Replay:", "Replay Game");


                if(input == "Exit" || input == "Cancel")
                {
                    valid = true;
                    gameid = -1;
                    MessageBox.Show("Replay Game Cancelled", "Replay Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if(string.IsNullOrWhiteSpace(input))
                {
                    MessageBox.Show("Please enter a valid Game ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if(!int.TryParse(input, out gameid))
                {
                    MessageBox.Show("Please enter a valid Game ID. Should be an Integer", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if((input != "Exit" && input != "Cancel"))
                    if (gameid <= 0 && (input != "Exit" || input != "Cancel"))
                    {
                        MessageBox.Show("Please enter a valid Game ID. Should be a positive Integer", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                       valid = true;
                    }
            }
            return gameid;
        }


        private string PromptInputBox(string prompt, string title)
        {
            Form promptForm = new Form();
            promptForm.Width = 300;
            promptForm.Height = 150;
            promptForm.Text = title;
            promptForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            promptForm.StartPosition = FormStartPosition.CenterScreen;

            Label promptLabel = new Label() { Left = 20, Top = 20, Text = prompt };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 250 };
            Button okButton = new Button() { Text = "OK", Left = 100, Top = 80, Width = 70 };
            Button cancelButton = new Button() { Text = "Cancel", Left = 180, Top = 80, Width = 70 };

            okButton.Click += (sender, e) => { promptForm.DialogResult = DialogResult.OK; };
            cancelButton.Click += (sender, e) => { promptForm.DialogResult = DialogResult.Cancel; };

            promptForm.Controls.Add(promptLabel);
            promptForm.Controls.Add(textBox);
            promptForm.Controls.Add(okButton);
            promptForm.Controls.Add(cancelButton);

            DialogResult dialogResult = promptForm.ShowDialog();

            promptForm.FormClosing += (sender, e) =>
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    promptForm.DialogResult = DialogResult.Abort;
                }
            };


            if (dialogResult == DialogResult.OK)
            {
                return textBox.Text;
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                return "Cancel";
            }
            else if (dialogResult == DialogResult.Abort)
            {
                return "Exit";
            }

            return null;
        }

    }



}
