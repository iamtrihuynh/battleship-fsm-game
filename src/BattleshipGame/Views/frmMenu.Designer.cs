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
        private SonarScannerControl sonarScanner;

        private Panel pnlButtonContainer;
        private TacticalButton btnPvE;
        private TacticalButton btnPvP;
        private TacticalButton btnCustom;
        private TacticalButton btnLeaderboard;
        private TacticalButton btnAbout;
        private TacticalButton btnExit;

        private Label lblFooter;

        // Panel chọn độ khó
        private Panel pnlDifficultySelection;
        private Label lblDifficultyTitle;
        private Label lblDifficultyDesc;
        private TacticalButton btnStartPvEEasy;
        private TacticalButton btnStartPvEHard;
        private TacticalButton btnCloseDifficulty;
        private Label lblPlayerName;
        private TextBox txtPlayerName;

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
            this.sonarScanner = new SonarScannerControl();

            this.pnlButtonContainer = new Panel();
            this.btnPvE = new TacticalButton();
            this.btnPvP = new TacticalButton();
            this.btnCustom = new TacticalButton();
            this.btnLeaderboard = new TacticalButton();
            this.btnAbout = new TacticalButton();
            this.btnExit = new TacticalButton();

            this.lblFooter = new Label();

            this.pnlDifficultySelection = new Panel();
            this.lblDifficultyTitle = new Label();
            this.lblDifficultyDesc = new Label();
            this.btnStartPvEEasy = new TacticalButton();
            this.btnStartPvEHard = new TacticalButton();
            this.btnCloseDifficulty = new TacticalButton();
            this.lblPlayerName = new Label();
            this.txtPlayerName = new TextBox();

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
            // btnSoundToggle
            // 
            this.btnSoundToggle.Location = new Point(680, 16);
            this.btnSoundToggle.Size = new Size(180, 38);
            this.btnSoundToggle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.btnSoundToggle.Text = "🔊 Âm thanh: BẬT";
            this.btnSoundToggle.CornerRadius = 6;
            this.btnSoundToggle.Click += new EventHandler(this.btnSoundToggle_Click);
            this.Controls.Add(this.btnSoundToggle);

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
            this.lblGameTitle.Text = "BẮN TÀU - CHIẾN HẠM TÁC CHIẾN";
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
            this.lblGameSubtitle.Text = "★ HỆ THỐNG PHÒNG THỦ & TÁC CHIẾN HẢI QUÂN HIỆN ĐẠI ★";
            this.Controls.Add(this.lblGameSubtitle);

            // 
            // pnlButtonContainer
            // 
            this.pnlButtonContainer.BackColor = UITheme.BackgroundDark;
            this.pnlButtonContainer.Location = new Point(270, 245);
            this.pnlButtonContainer.Size = new Size(340, 360);
            this.Controls.Add(this.pnlButtonContainer);

            // 
            // btnPvE
            // 
            this.btnPvE.Location = new Point(20, 10);
            this.btnPvE.Size = new Size(300, 48);
            this.btnPvE.Text = "🤖  CHƠI VỚI MÁY (PvE)";
            this.btnPvE.BorderColor = UITheme.PrimaryCyan;
            this.btnPvE.Click += new EventHandler(this.btnPvE_Click);
            this.pnlButtonContainer.Controls.Add(this.btnPvE);

            // 
            // btnPvP
            // 
            this.btnPvP.Location = new Point(20, 68);
            this.btnPvP.Size = new Size(300, 48);
            this.btnPvP.Text = "👥  HAI NGƯỜI CHƠI (PvP)";
            this.btnPvP.BorderColor = UITheme.PrimaryCyan;
            this.btnPvP.Click += new EventHandler(this.btnPvP_Click);
            this.pnlButtonContainer.Controls.Add(this.btnPvP);

            // 
            // btnCustom
            // 
            this.btnCustom.Location = new Point(20, 126);
            this.btnCustom.Size = new Size(300, 48);
            this.btnCustom.Text = "⚙️  CHẾ ĐỘ TÙY BIẾN (CUSTOM)";
            this.btnCustom.BorderColor = UITheme.BorderLight;
            this.btnCustom.Click += new EventHandler(this.btnCustom_Click);
            this.pnlButtonContainer.Controls.Add(this.btnCustom);

            // 
            // btnLeaderboard
            // 
            this.btnLeaderboard.Location = new Point(20, 184);
            this.btnLeaderboard.Size = new Size(300, 48);
            this.btnLeaderboard.Text = "🏆  BẢNG XẾP HẠNG";
            this.btnLeaderboard.BorderColor = UITheme.BorderLight;
            this.btnLeaderboard.Click += new EventHandler(this.btnLeaderboard_Click);
            this.pnlButtonContainer.Controls.Add(this.btnLeaderboard);

            // 
            // btnAbout
            // 
            this.btnAbout.Location = new Point(20, 242);
            this.btnAbout.Size = new Size(300, 48);
            this.btnAbout.Text = "ℹ️  HƯỚNG DẪN & GIỚI THIỆU";
            this.btnAbout.BorderColor = UITheme.BorderLight;
            this.btnAbout.Click += new EventHandler(this.btnAbout_Click);
            this.pnlButtonContainer.Controls.Add(this.btnAbout);

            // 
            // btnExit
            // 
            this.btnExit.Location = new Point(20, 300);
            this.btnExit.Size = new Size(300, 48);
            this.btnExit.Text = "🚪  THOÁT TRÒ CHƠI";
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
            this.lblFooter.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            this.lblFooter.ForeColor = UITheme.PrimaryCyan;
            this.lblFooter.TextAlign = ContentAlignment.MiddleCenter;
            this.lblFooter.Text = "Bắn Tàu - Nhóm WINDOWN";
            this.Controls.Add(this.lblFooter);

            // 
            // pnlDifficultySelection (Hộp thoại chọn độ khó dạng modal)
            // 
            this.pnlDifficultySelection.BackColor = UITheme.PanelBackground;
            this.pnlDifficultySelection.BorderStyle = BorderStyle.FixedSingle;
            this.pnlDifficultySelection.Location = new Point(190, 160);
            this.pnlDifficultySelection.Size = new Size(500, 360);
            this.pnlDifficultySelection.Visible = false;
            this.Controls.Add(this.pnlDifficultySelection);

            this.lblDifficultyTitle.AutoSize = false;
            this.lblDifficultyTitle.Location = new Point(20, 16);
            this.lblDifficultyTitle.Size = new Size(460, 30);
            this.lblDifficultyTitle.Font = new Font("Segoe UI", 15f, FontStyle.Bold);
            this.lblDifficultyTitle.ForeColor = UITheme.AccentNeon;
            this.lblDifficultyTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblDifficultyTitle.Text = "CHỌN CẤP ĐỘ MÁY ĐỐI THỦ (AI)";
            this.pnlDifficultySelection.Controls.Add(this.lblDifficultyTitle);

            this.lblDifficultyDesc.AutoSize = false;
            this.lblDifficultyDesc.Location = new Point(25, 48);
            this.lblDifficultyDesc.Size = new Size(450, 30);
            this.lblDifficultyDesc.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            this.lblDifficultyDesc.ForeColor = UITheme.TextSecondary;
            this.lblDifficultyDesc.TextAlign = ContentAlignment.MiddleCenter;
            this.lblDifficultyDesc.Text = "Nhập tên Đô đốc để lưu điểm vào Bảng Xếp Hạng khi kết thúc ván đấu:";
            this.pnlDifficultySelection.Controls.Add(this.lblDifficultyDesc);

            // lblPlayerName
            this.lblPlayerName.AutoSize = false;
            this.lblPlayerName.Location = new Point(50, 82);
            this.lblPlayerName.Size = new Size(400, 18);
            this.lblPlayerName.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            this.lblPlayerName.ForeColor = UITheme.PrimaryCyan;
            this.lblPlayerName.TextAlign = ContentAlignment.MiddleLeft;
            this.lblPlayerName.Text = "👤 TÊN ĐÔ ĐỐC / NGƯỜI CHƠI:";
            this.pnlDifficultySelection.Controls.Add(this.lblPlayerName);

            // txtPlayerName
            this.txtPlayerName.Location = new Point(50, 102);
            this.txtPlayerName.Size = new Size(400, 28);
            this.txtPlayerName.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            this.txtPlayerName.BackColor = Color.FromArgb(15, 23, 42);
            this.txtPlayerName.ForeColor = Color.White;
            this.txtPlayerName.BorderStyle = BorderStyle.FixedSingle;
            this.txtPlayerName.MaxLength = 40;
            this.txtPlayerName.Text = "Thuyền Trưởng";
            this.pnlDifficultySelection.Controls.Add(this.txtPlayerName);

            // btnStartPvEEasy
            this.btnStartPvEEasy.Location = new Point(50, 142);
            this.btnStartPvEEasy.Size = new Size(400, 48);
            this.btnStartPvEEasy.Text = "🟢 CẤP ĐỘ DỄ (Bắn ngẫu nhiên)";
            this.btnStartPvEEasy.BorderColor = UITheme.SuccessGreen;
            this.btnStartPvEEasy.HoverBorderColor = UITheme.SuccessGreen;
            this.btnStartPvEEasy.HoverTextColor = UITheme.SuccessGreen;
            this.btnStartPvEEasy.Click += new EventHandler(this.btnStartPvEEasy_Click);
            this.pnlDifficultySelection.Controls.Add(this.btnStartPvEEasy);

            // btnStartPvEHard
            this.btnStartPvEHard.Location = new Point(50, 200);
            this.btnStartPvEHard.Size = new Size(400, 48);
            this.btnStartPvEHard.Text = "🔴 CẤP ĐỘ KHÓ (FSM Hunt & Target)";
            this.btnStartPvEHard.BorderColor = UITheme.DangerRed;
            this.btnStartPvEHard.HoverBorderColor = UITheme.DangerRed;
            this.btnStartPvEHard.HoverTextColor = UITheme.DangerRed;
            this.btnStartPvEHard.Click += new EventHandler(this.btnStartPvEHard_Click);
            this.pnlDifficultySelection.Controls.Add(this.btnStartPvEHard);

            // btnCloseDifficulty
            this.btnCloseDifficulty.Location = new Point(140, 260);
            this.btnCloseDifficulty.Size = new Size(220, 38);
            this.btnCloseDifficulty.Text = "Quay Lại Menu";
            this.btnCloseDifficulty.BorderColor = UITheme.BorderDefault;
            this.btnCloseDifficulty.Click += new EventHandler(this.btnCloseDifficulty_Click);
            this.pnlDifficultySelection.Controls.Add(this.btnCloseDifficulty);

            this.ResumeLayout(false);
        }
    }
}
