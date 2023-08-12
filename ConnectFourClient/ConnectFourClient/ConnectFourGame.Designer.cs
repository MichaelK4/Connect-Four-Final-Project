using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConnectFourClient
{
    partial class ConnectFourGame
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectFourGame));
            this.DropBtnCol1 = new System.Windows.Forms.Button();
            this.DropBtnCol2 = new System.Windows.Forms.Button();
            this.DropBtnCol3 = new System.Windows.Forms.Button();
            this.DropBtnCol4 = new System.Windows.Forms.Button();
            this.DropBtnCol5 = new System.Windows.Forms.Button();
            this.DropBtnCol6 = new System.Windows.Forms.Button();
            this.DropBtnCol7 = new System.Windows.Forms.Button();
            this.EndGameBtn = new System.Windows.Forms.Button();
            this.DatabaseBtn = new System.Windows.Forms.Button();
            this.PauseGameBtn = new System.Windows.Forms.Button();
            this.TurnLabel = new System.Windows.Forms.Label();
            this.WelcomeLabel = new System.Windows.Forms.Label();
            this.RestartGameBtn = new System.Windows.Forms.Button();
            this.AboutBtn = new System.Windows.Forms.Button();
            this.GameDurationLabel = new System.Windows.Forms.Label();
            this.NewGameBtn = new System.Windows.Forms.Button();
            this.BoardPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.BoardPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // DropBtnCol1
            // 
            this.DropBtnCol1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.DropBtnCol1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DropBtnCol1.Location = new System.Drawing.Point(219, 188);
            this.DropBtnCol1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DropBtnCol1.Name = "DropBtnCol1";
            this.DropBtnCol1.Size = new System.Drawing.Size(77, 39);
            this.DropBtnCol1.TabIndex = 1;
            this.DropBtnCol1.Tag = "0";
            this.DropBtnCol1.Text = "Drop";
            this.DropBtnCol1.UseVisualStyleBackColor = false;
            this.DropBtnCol1.Click += new System.EventHandler(this.DropBtnCol_Click);
            // 
            // DropBtnCol2
            // 
            this.DropBtnCol2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.DropBtnCol2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DropBtnCol2.Location = new System.Drawing.Point(337, 188);
            this.DropBtnCol2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DropBtnCol2.Name = "DropBtnCol2";
            this.DropBtnCol2.Size = new System.Drawing.Size(77, 39);
            this.DropBtnCol2.TabIndex = 2;
            this.DropBtnCol2.Tag = "1";
            this.DropBtnCol2.Text = "Drop";
            this.DropBtnCol2.UseVisualStyleBackColor = false;
            this.DropBtnCol2.Click += new System.EventHandler(this.DropBtnCol_Click);
            // 
            // DropBtnCol3
            // 
            this.DropBtnCol3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.DropBtnCol3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DropBtnCol3.Location = new System.Drawing.Point(460, 188);
            this.DropBtnCol3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DropBtnCol3.Name = "DropBtnCol3";
            this.DropBtnCol3.Size = new System.Drawing.Size(77, 39);
            this.DropBtnCol3.TabIndex = 3;
            this.DropBtnCol3.Tag = "2";
            this.DropBtnCol3.Text = "Drop";
            this.DropBtnCol3.UseVisualStyleBackColor = false;
            this.DropBtnCol3.Click += new System.EventHandler(this.DropBtnCol_Click);
            // 
            // DropBtnCol4
            // 
            this.DropBtnCol4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.DropBtnCol4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DropBtnCol4.Location = new System.Drawing.Point(580, 188);
            this.DropBtnCol4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DropBtnCol4.Name = "DropBtnCol4";
            this.DropBtnCol4.Size = new System.Drawing.Size(77, 39);
            this.DropBtnCol4.TabIndex = 4;
            this.DropBtnCol4.Tag = "3";
            this.DropBtnCol4.Text = "Drop";
            this.DropBtnCol4.UseVisualStyleBackColor = false;
            this.DropBtnCol4.Click += new System.EventHandler(this.DropBtnCol_Click);
            // 
            // DropBtnCol5
            // 
            this.DropBtnCol5.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.DropBtnCol5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DropBtnCol5.Location = new System.Drawing.Point(699, 188);
            this.DropBtnCol5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DropBtnCol5.Name = "DropBtnCol5";
            this.DropBtnCol5.Size = new System.Drawing.Size(77, 39);
            this.DropBtnCol5.TabIndex = 5;
            this.DropBtnCol5.Tag = "4";
            this.DropBtnCol5.Text = "Drop";
            this.DropBtnCol5.UseVisualStyleBackColor = false;
            this.DropBtnCol5.Click += new System.EventHandler(this.DropBtnCol_Click);
            // 
            // DropBtnCol6
            // 
            this.DropBtnCol6.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.DropBtnCol6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DropBtnCol6.Location = new System.Drawing.Point(823, 188);
            this.DropBtnCol6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DropBtnCol6.Name = "DropBtnCol6";
            this.DropBtnCol6.Size = new System.Drawing.Size(77, 39);
            this.DropBtnCol6.TabIndex = 6;
            this.DropBtnCol6.Tag = "5";
            this.DropBtnCol6.Text = "Drop";
            this.DropBtnCol6.UseVisualStyleBackColor = false;
            this.DropBtnCol6.Click += new System.EventHandler(this.DropBtnCol_Click);
            // 
            // DropBtnCol7
            // 
            this.DropBtnCol7.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.DropBtnCol7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DropBtnCol7.Location = new System.Drawing.Point(941, 188);
            this.DropBtnCol7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DropBtnCol7.Name = "DropBtnCol7";
            this.DropBtnCol7.Size = new System.Drawing.Size(77, 39);
            this.DropBtnCol7.TabIndex = 7;
            this.DropBtnCol7.Tag = "6";
            this.DropBtnCol7.Text = "Drop";
            this.DropBtnCol7.UseVisualStyleBackColor = false;
            this.DropBtnCol7.Click += new System.EventHandler(this.DropBtnCol_Click);
            // 
            // EndGameBtn
            // 
            this.EndGameBtn.BackColor = System.Drawing.Color.LightGray;
            this.EndGameBtn.Location = new System.Drawing.Point(637, 12);
            this.EndGameBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.EndGameBtn.Name = "EndGameBtn";
            this.EndGameBtn.Size = new System.Drawing.Size(117, 30);
            this.EndGameBtn.TabIndex = 9;
            this.EndGameBtn.Text = "End Game";
            this.EndGameBtn.UseVisualStyleBackColor = false;
            this.EndGameBtn.Click += new System.EventHandler(this.EndGameBtn_Click);
            // 
            // DatabaseBtn
            // 
            this.DatabaseBtn.BackColor = System.Drawing.Color.LightGray;
            this.DatabaseBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DatabaseBtn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.DatabaseBtn.Location = new System.Drawing.Point(21, 12);
            this.DatabaseBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DatabaseBtn.Name = "DatabaseBtn";
            this.DatabaseBtn.Size = new System.Drawing.Size(117, 30);
            this.DatabaseBtn.TabIndex = 10;
            this.DatabaseBtn.Text = "Your Database";
            this.DatabaseBtn.UseVisualStyleBackColor = false;
            this.DatabaseBtn.Click += new System.EventHandler(this.DatabaseBtn_Click);
            // 
            // PauseGameBtn
            // 
            this.PauseGameBtn.BackColor = System.Drawing.Color.LightGray;
            this.PauseGameBtn.Location = new System.Drawing.Point(391, 12);
            this.PauseGameBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PauseGameBtn.Name = "PauseGameBtn";
            this.PauseGameBtn.Size = new System.Drawing.Size(117, 30);
            this.PauseGameBtn.TabIndex = 11;
            this.PauseGameBtn.Text = "Pause Game";
            this.PauseGameBtn.UseVisualStyleBackColor = false;
            this.PauseGameBtn.Click += new System.EventHandler(this.PauseGameBtn_Click);
            // 
            // TurnLabel
            // 
            this.TurnLabel.AutoSize = true;
            this.TurnLabel.BackColor = System.Drawing.Color.Transparent;
            this.TurnLabel.Font = new System.Drawing.Font("Bodoni MT", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TurnLabel.ForeColor = System.Drawing.Color.Black;
            this.TurnLabel.Location = new System.Drawing.Point(984, 12);
            this.TurnLabel.Name = "TurnLabel";
            this.TurnLabel.Size = new System.Drawing.Size(123, 23);
            this.TurnLabel.TabIndex = 12;
            this.TurnLabel.Text = "User\'s Turn";
            // 
            // WelcomeLabel
            // 
            this.WelcomeLabel.AutoSize = true;
            this.WelcomeLabel.BackColor = System.Drawing.Color.Transparent;
            this.WelcomeLabel.Font = new System.Drawing.Font("Bodoni MT", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WelcomeLabel.ForeColor = System.Drawing.Color.Black;
            this.WelcomeLabel.Location = new System.Drawing.Point(16, 59);
            this.WelcomeLabel.Name = "WelcomeLabel";
            this.WelcomeLabel.Size = new System.Drawing.Size(202, 34);
            this.WelcomeLabel.TabIndex = 12;
            this.WelcomeLabel.Text = "Welcome User";
            // 
            // RestartGameBtn
            // 
            this.RestartGameBtn.BackColor = System.Drawing.Color.LightGray;
            this.RestartGameBtn.Location = new System.Drawing.Point(515, 12);
            this.RestartGameBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RestartGameBtn.Name = "RestartGameBtn";
            this.RestartGameBtn.Size = new System.Drawing.Size(117, 30);
            this.RestartGameBtn.TabIndex = 13;
            this.RestartGameBtn.Text = "Restart Game";
            this.RestartGameBtn.UseVisualStyleBackColor = false;
            this.RestartGameBtn.Click += new System.EventHandler(this.RestartGameBtn_Click);
            // 
            // AboutBtn
            // 
            this.AboutBtn.BackColor = System.Drawing.Color.LightGray;
            this.AboutBtn.Location = new System.Drawing.Point(145, 12);
            this.AboutBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.AboutBtn.Name = "AboutBtn";
            this.AboutBtn.Size = new System.Drawing.Size(117, 30);
            this.AboutBtn.TabIndex = 14;
            this.AboutBtn.Text = "About";
            this.AboutBtn.UseVisualStyleBackColor = false;
            this.AboutBtn.Click += new System.EventHandler(this.AboutBtn_Click);
            // 
            // GameDurationLabel
            // 
            this.GameDurationLabel.AutoSize = true;
            this.GameDurationLabel.BackColor = System.Drawing.Color.Transparent;
            this.GameDurationLabel.Font = new System.Drawing.Font("Bodoni MT", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameDurationLabel.ForeColor = System.Drawing.Color.Black;
            this.GameDurationLabel.Location = new System.Drawing.Point(1080, 310);
            this.GameDurationLabel.Name = "GameDurationLabel";
            this.GameDurationLabel.Size = new System.Drawing.Size(68, 23);
            this.GameDurationLabel.TabIndex = 15;
            this.GameDurationLabel.Text = "Timer";
            // 
            // NewGameBtn
            // 
            this.NewGameBtn.BackColor = System.Drawing.Color.LightGray;
            this.NewGameBtn.Location = new System.Drawing.Point(268, 12);
            this.NewGameBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.NewGameBtn.Name = "NewGameBtn";
            this.NewGameBtn.Size = new System.Drawing.Size(117, 30);
            this.NewGameBtn.TabIndex = 16;
            this.NewGameBtn.Text = "New Game";
            this.NewGameBtn.UseVisualStyleBackColor = false;
            this.NewGameBtn.Click += new System.EventHandler(this.NewGameBtn_Click);
            // 
            // BoardPicture
            // 
            this.BoardPicture.BackColor = System.Drawing.Color.Transparent;
            this.BoardPicture.BackgroundImage = global::ConnectFourClient.Properties.Resources.Board;
            this.BoardPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BoardPicture.Location = new System.Drawing.Point(199, 233);
            this.BoardPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BoardPicture.Name = "BoardPicture";
            this.BoardPicture.Size = new System.Drawing.Size(843, 748);
            this.BoardPicture.TabIndex = 8;
            this.BoardPicture.TabStop = false;
            // 
            // ConnectFourGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSlateGray;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1218, 1055);
            this.Controls.Add(this.NewGameBtn);
            this.Controls.Add(this.GameDurationLabel);
            this.Controls.Add(this.AboutBtn);
            this.Controls.Add(this.RestartGameBtn);
            this.Controls.Add(this.PauseGameBtn);
            this.Controls.Add(this.DatabaseBtn);
            this.Controls.Add(this.EndGameBtn);
            this.Controls.Add(this.BoardPicture);
            this.Controls.Add(this.DropBtnCol1);
            this.Controls.Add(this.DropBtnCol2);
            this.Controls.Add(this.DropBtnCol3);
            this.Controls.Add(this.DropBtnCol4);
            this.Controls.Add(this.DropBtnCol5);
            this.Controls.Add(this.DropBtnCol6);
            this.Controls.Add(this.DropBtnCol7);
            this.Controls.Add(this.TurnLabel);
            this.Controls.Add(this.WelcomeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ConnectFourGame";
            this.Text = "Connect Four Game";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.Load += new System.EventHandler(this.ConnectFourGame_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BoardPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button DropBtnCol1;
        private System.Windows.Forms.Button DropBtnCol2;
        private System.Windows.Forms.Button DropBtnCol3;
        private System.Windows.Forms.Button DropBtnCol4;
        private System.Windows.Forms.Button DropBtnCol5;
        private System.Windows.Forms.Button DropBtnCol6;
        private System.Windows.Forms.Button DropBtnCol7;
        private PictureBox BoardPicture;
        private Button EndGameBtn;
        private Button DatabaseBtn;
        private Button PauseGameBtn;
        private Button RestartGameBtn;
        private Button AboutBtn;
        private Label TurnLabel;
        private Label WelcomeLabel;
        private Label GameDurationLabel;
        private Button NewGameBtn;
    }
}

