using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ConnectFourClient
{
    internal static class Program
    {
        [STAThread]
        static  void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RunApplication().Wait();
        }

        private static async Task RunApplication()
        {
            //DeleteAllGames();
            string username = "";
            bool loginSuccess;
            bool isValid;
            bool isUsernameExistTask;
            bool isUsernameExistInServer = false;
            GameAPIClient gameAPIClient = new GameAPIClient();
            int gameid = 0;
            do
            {
                LoginForm loginForm = new LoginForm();
                Application.Run(loginForm);
                gameid = loginForm.GameID;
                loginSuccess = loginForm.IsLoginSuccessful;
                username = loginForm.Username.Trim();
                isValid = loginForm.IsValid;
                int res = IsUsernameExist(username.ToLower());
                isUsernameExistTask = await gameAPIClient.CheckUsername(username);
                if (!isUsernameExistTask)
                {
                    MessageBox.Show("Username does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    loginSuccess = false;
                    isValid = false;
                }
                
                if (res != -1)
                {
                    gameid = res + 1;
                }
                else
                {
                    gameid = 1;
                }
            } while (!isValid && !loginSuccess && !isUsernameExistTask);


            Application.Run(new ConnectFourGame(username.ToLower(), gameid));
        }


        private static int IsUsernameExist(string username)
        {
            string connStr = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"Put your own path to the .mdf file here\\Database.mdf\";Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT TOP 1 GameID FROM TblConnectFourGames WHERE Username = @Username ORDER BY GameID DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username.ToLower());
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int latestGameID))
                    {
                        return latestGameID;
                    }
                }
            }
            return -1;
        }

    }
}
