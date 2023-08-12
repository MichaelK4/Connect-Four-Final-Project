using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace ConnectFourClient
{
    public partial class LoginForm : Form
    {
        public bool IsLoginSuccessful { get; private set; }

        public  bool exit { get; private set;}
        public string Username { get; private set; }
        public int GameID { get; private set; }

        public bool IsValid { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            MaximizeBox = false;
        }

        private void LogicBtn_Click(object sender, EventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();

            if (!IsValidUsername(username) )
            {
                return;
            }
            Username = UsernameTextBox.Text;
            IsLoginSuccessful = true;
            IsValid = true;
            this.Close();
        }



        public bool IsValidUsername(string username)
        {
            if(username.Contains(" "))
            {
                MessageBox.Show("Please enter A valid username without whitespace.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter A valid username without whitespace.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!Regex.IsMatch(username, @"[a-zA-Z]{4,}"))
            {
                MessageBox.Show("Username should contain at least 4 alphabet letters", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_.]+$"))
            {
                MessageBox.Show("Invalid username input, username can contain only alphabetic, numbers and ('.','_')", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (username.Length > 20)
            {
                MessageBox.Show("Username should be less than 20 characters", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string lowercaseUsername = username.ToLower();

            if (Regex.IsMatch(lowercaseUsername, @"^\d+$"))
            {
                MessageBox.Show("Username should contain at least one alphabet letter", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }


            return true;
        }

        public bool CheckServerForUsername(string username)
        {
            return false;
        }

        private void SignUpBtn_Click(object sender, EventArgs e)
        {
            string url = "https://localhost:7083/Register";
            Process.Start(url);
        }
    }
}
