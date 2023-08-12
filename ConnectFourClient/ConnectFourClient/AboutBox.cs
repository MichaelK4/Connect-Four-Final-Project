using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectFourClient
{
    partial class AboutBox : Form
    {
        public delegate void AboutBoxDelegate(TextBox d);
        public AboutBox()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.textBoxDescription.Text = AssemblyDescription;
        }


        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        public void ShowAboutBox()
        {

            this.Text = "About Box";

            this.labelProductName.Text = "Connect Four Game";
            this.labelProductName.Font = new Font("Arial", 10, FontStyle.Bold);

            this.labelVersion.Text = String.Format("Version {0}", "3.1.0.7");
            this.labelVersion.Font = new Font("Arial", 10);

            this.labelCopyright.Text = "Michael and Itay";
            this.labelCopyright.Font = new Font("Arial", 10);

            this.textBoxDescription.Text = "Connect Four is a two-player game in which players choose a color and take turns placing colored tokens or discs on a board with seven columns and six rows.\r\n" +
                "The goal is to form a line of four tokens or discs of the same color horizontally, vertically, or diagonally.\r\n" +
                "The game is also known by other names, such as Connect Four, Four in a Row, or Captain’s Mistress.\r\n" +
                "The game was first sold by Milton Bradley in 1974.\r\n\r\n" +
                "The End Button - Will Exit the game saving the process to the Database.\r\n" +
                "The Pause Button - This will pause the game, you can return to the game whenever you like, to move on to the next one,\n " +
                "the game either way will be saved to the database.\r\n" +
                "The Your Database Button - Will show you all your games, the time played and when it was played on each game if you won or not or got a draw,\r\n" +
                "it will also show you if a game was unfinished (Paused).\r\n" +
                "The Restart Button - This allows you to restart the current game from the beginning.\r\n" +
                "The Drop Buttons  - Above each column will let you drop a token inside this column, and the computer will respond to your turn.\r\n\r\n" +
                "After the game ends it will be saved to the database and you can decide if you want to play again or return later.\r\n";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.BackColor = Color.White;
            this.textBoxDescription.ForeColor = Color.Black;
            this.textBoxDescription.Font = new Font("Arial", 10, FontStyle.Bold);
            ShowDialog();
        }

        private void labelCopyright_Click(object sender, EventArgs e)
        {

        }
    }
}
