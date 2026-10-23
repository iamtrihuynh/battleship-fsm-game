using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
    partial class frmMenu
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblGameTitle;
        private Label lblGameSubtitle;
        private TacticalButton btnSoundToggle;
        private TacticalVolumeSlider sliderVolume;
        private SonarScannerControl sonarScanner;

        private Panel pnlButtonContainer;
        private TacticalButton btnPvE;
        private TacticalButton btnLeaderboard;
        private TacticalButton btnAbout;
        private TacticalButton btnExit;

        // Góc trên bên trái: Tên người chơi và nút đổi tên
        private Label lblCurrentPlayer;
        private TacticalButton btnChangeName;

        private Label lblFooter;

        // Panel chọn độ khó
        private Panel pnlDifficultySelection;
        private Label lblDifficultyTitle;
        private TacticalButton btnStartPvEEasy;
        private TacticalButton btnStartPvEHard;
        private TacticalButton btnCloseDifficulty;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new Panel();
            this.lblGameTitle = new Label();
            this.lblGameSubtitle = new Label();
            this.btnSoundToggle = new TacticalButton();
            this.sliderVolume = new TacticalVolumeSlider();
            this.sonarScanner = new SonarScannerControl();

            this.pnlButtonContainer = new Panel();
            this.btnPvE = new TacticalButton();
            this.btnLeaderboard = new TacticalButton();
            this.btnAbout = new TacticalButton();
            this.btnExit = new TacticalButton();

            this.lblCurrentPlayer = new Label();
            this.btnChangeName = new TacticalButton();

            this.lblFooter = new Label();

            this.pnlDifficultySelection = new Panel();
            this.lblDifficultyTitle = new Label();
            this.btnStartPvEEasy = new TacticalButton();
            this.btnStartPvEHard = new TacticalButton();
            this.btnCloseDifficulty = new TacticalButton();

            this.SuspendLayout();

            // 
            // frmMenu
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = UITheme.BackgroundDark;
            this.ClientSize = new Size(880, 680);
            this.MinimumSize = new Size(880, 680);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.Name = "frmMenu";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Resize += new EventHandler(this.frmMenu_Resize);

            // 
            // lblCurrentPlayer (Tên người chơi hiện tại ở góc trên bên trái)
            // 
            this.lblCurrentPlayer.AutoSize = false;
            this.lblCurrentPlayer.Location = new Point(24, 16);
            this.lblCurrentPlayer.Size = new Size(260, 24);
            this.lblCurrentPlayer.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            this.lblCurrentPlayer.ForeColor = UITheme.AccentNeon;
            this.lblCurrentPlayer.TextAlign = ContentAlignment.MiddleLeft;
            this.lblCurrentPlayer.Text = "👤 Người chơi";
            this.Controls.Add(this.lblCurrentPlayer);

            // 
            // btnChangeName (Nút đổi tên ngay bên dưới tên người chơi)
            // 
            this.btnChangeName.Location = new Point(24, 44);
            this.btnChangeName.Size = new Size(110, 32);
            this.btnChangeName.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            this.btnChangeName.Text = "✏️  Đổi tên";
            this.btnChangeName.CornerRadius = 6;
            this.btnChangeName.BorderColor = UITheme.PrimaryCyan;
            this.btnChangeName.HoverBorderColor = UITheme.AccentNeon;
            this.btnChangeName.HoverTextColor = UITheme.AccentNeon;
            this.btnChangeName.Click += new EventHandler(this.btnChangeName_Click);
            this.Controls.Add(this.btnChangeName);

            // 
            // btnSoundToggle
            // 
            this.btnSoundToggle.Location = new Point(660, 14);
            this.btnSoundToggle.Size = new Size(200, 34);
            this.btnSoundToggle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            this.btnSoundToggle.Text = "🔊 Âm thanh: Bật";
            this.btnSoundToggle.CornerRadius = 6;
            this.btnSoundToggle.Click += new EventHandler(this.btnSoundToggle_Click);
            this.Controls.Add(this.btnSoundToggle);

            // 
            // sliderVolume
            // 
            this.sliderVolume.Location = new Point(660, 52);
            this.sliderVolume.Size = new Size(200, 32);
            this.sliderVolume.ValueChanged += new EventHandler(this.sliderVolume_ValueChanged);
            this.Controls.Add(this.sliderVolume);

            // 
            // sonarScanner
            // 
            this.sonarScanner.Location = new Point(370, 20);
            this.sonarScanner.Size = new Size(140, 140);
            this.Controls.Add(this.sonarScanner);

            // 
            // lblGameTitle
            // 
            this.lblGameTitle.AutoSize = false;
            this.lblGameTitle.Location = new Point(40, 160);
            this.lblGameTitle.Size = new Size(800, 48);
            this.lblGameTitle.Font = new Font("Segoe UI", 26f, FontStyle.Bold);
            this.lblGameTitle.ForeColor = UITheme.AccentNeon;
            this.lblGameTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblGameTitle.Text = "Bắn Tàu";
            this.Controls.Add(this.lblGameTitle);

            // 
            // lblGameSubtitle
            // 
            this.lblGameSubtitle.AutoSize = false;
            this.lblGameSubtitle.Location = new Point(40, 210);
            this.lblGameSubtitle.Size = new Size(800, 24);
            this.lblGameSubtitle.Font = new Font("Segoe UI", 11f, FontStyle.Regular);
            this.lblGameSubtitle.ForeColor = UITheme.PrimaryCyan;
            this.lblGameSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblGameSubtitle.Text = "Chiến thuật hải chiến cùng trí tuệ nhân tạo (AI)";
            this.Controls.Add(this.lblGameSubtitle);

            // 
            // pnlButtonContainer
            // 
            this.pnlButtonContainer.BackColor = UITheme.BackgroundDark;
            this.pnlButtonContainer.Location = new Point(280, 245);
            this.pnlButtonContainer.Size = new Size(320, 240);
            this.Controls.Add(this.pnlButtonContainer);

            // 
            // btnPvE
            // 
            this.btnPvE.Location = new Point(10, 10);
            this.btnPvE.Size = new Size(300, 46);
            this.btnPvE.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            this.btnPvE.Text = "Chơi với máy";
            this.btnPvE.BorderColor = UITheme.PrimaryCyan;
            this.btnPvE.Click += new EventHandler(this.btnPvE_Click);
            this.pnlButtonContainer.Controls.Add(this.btnPvE);

            // 
            // btnLeaderboard
            // 
            this.btnLeaderboard.Location = new Point(10, 68);
            this.btnLeaderboard.Size = new Size(300, 46);
            this.btnLeaderboard.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            this.btnLeaderboard.Text = "Bảng xếp hạng";
            this.btnLeaderboard.BorderColor = UITheme.BorderLight;
            this.btnLeaderboard.Click += new EventHandler(this.btnLeaderboard_Click);
            this.pnlButtonContainer.Controls.Add(this.btnLeaderboard);

            // 
            // btnAbout
            // 
            this.btnAbout.Location = new Point(10, 126);
            this.btnAbout.Size = new Size(300, 46);
            this.btnAbout.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            this.btnAbout.Text = "Hướng dẫn";
            this.btnAbout.BorderColor = UITheme.BorderLight;
            this.btnAbout.Click += new EventHandler(this.btnAbout_Click);
            this.pnlButtonContainer.Controls.Add(this.btnAbout);

            // 
            // btnExit
            // 
            this.btnExit.Location = new Point(10, 184);
            this.btnExit.Size = new Size(300, 46);
            this.btnExit.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            this.btnExit.Text = "Thoát";
            this.btnExit.BorderColor = UITheme.DangerRed;
            this.btnExit.HoverBorderColor = UITheme.DangerRed;
            this.btnExit.HoverTextColor = UITheme.DangerRed;
            this.btnExit.Click += new EventHandler(this.btnExit_Click);
            this.pnlButtonContainer.Controls.Add(this.btnExit);

            // 
            // lblFooter
            // 
            this.lblFooter.AutoSize = false;
            this.lblFooter.Location = new Point(40, 630);
            this.lblFooter.Size = new Size(800, 24);
            this.lblFooter.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.lblFooter.ForeColor = UITheme.TextSecondary;
            this.lblFooter.TextAlign = ContentAlignment.MiddleCenter;
            this.lblFooter.Text = "Nhóm WINDOWN";
            this.Controls.Add(this.lblFooter);

            // 
            // pnlDifficultySelection (Hộp thoại chọn độ khó)
            // 
            this.pnlDifficultySelection.BackColor = UITheme.PanelBackground;
            this.pnlDifficultySelection.BorderStyle = BorderStyle.FixedSingle;
            this.pnlDifficultySelection.Location = new Point(280, 210);
            this.pnlDifficultySelection.Size = new Size(320, 210);
            this.pnlDifficultySelection.Visible = false;
            this.Controls.Add(this.pnlDifficultySelection);

            this.lblDifficultyTitle.AutoSize = false;
            this.lblDifficultyTitle.Location = new Point(20, 16);
            this.lblDifficultyTitle.Size = new Size(280, 28);
            this.lblDifficultyTitle.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            this.lblDifficultyTitle.ForeColor = UITheme.AccentNeon;
            this.lblDifficultyTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblDifficultyTitle.Text = "Chọn cấp độ";
            this.pnlDifficultySelection.Controls.Add(this.lblDifficultyTitle);

            // btnStartPvEEasy
            this.btnStartPvEEasy.Location = new Point(30, 56);
            this.btnStartPvEEasy.Size = new Size(260, 42);
            this.btnStartPvEEasy.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            this.btnStartPvEEasy.Text = "Dễ";
            this.btnStartPvEEasy.BorderColor = UITheme.SuccessGreen;
            this.btnStartPvEEasy.HoverBorderColor = UITheme.SuccessGreen;
            this.btnStartPvEEasy.HoverTextColor = UITheme.SuccessGreen;
            this.btnStartPvEEasy.Click += new EventHandler(this.btnStartPvEEasy_Click);
            this.pnlDifficultySelection.Controls.Add(this.btnStartPvEEasy);

            // btnStartPvEHard
            this.btnStartPvEHard.Location = new Point(30, 106);
            this.btnStartPvEHard.Size = new Size(260, 42);
            this.btnStartPvEHard.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            this.btnStartPvEHard.Text = "Khó";
            this.btnStartPvEHard.BorderColor = UITheme.DangerRed;
            this.btnStartPvEHard.HoverBorderColor = UITheme.DangerRed;
            this.btnStartPvEHard.HoverTextColor = UITheme.DangerRed;
            this.btnStartPvEHard.Click += new EventHandler(this.btnStartPvEHard_Click);
            this.pnlDifficultySelection.Controls.Add(this.btnStartPvEHard);

            // btnCloseDifficulty
            this.btnCloseDifficulty.Location = new Point(90, 158);
            this.btnCloseDifficulty.Size = new Size(140, 36);
            this.btnCloseDifficulty.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.btnCloseDifficulty.Text = "Quay lại";
            this.btnCloseDifficulty.BorderColor = UITheme.BorderDefault;
            this.btnCloseDifficulty.Click += new EventHandler(this.btnCloseDifficulty_Click);
            this.pnlDifficultySelection.Controls.Add(this.btnCloseDifficulty);

            this.ResumeLayout(false);
        }
    }
}
