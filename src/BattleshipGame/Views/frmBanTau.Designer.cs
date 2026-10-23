using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
    partial class frmBanTau
    {
        private System.ComponentModel.IContainer components = null;

        // Header controls
        private Panel pnlTopBar;
        private Label lblMatchModeInfo;
        private TacticalButton btnBackToMenu;
        private TacticalButton btnToggleSound;

        // Khu vực chính 3 cột
        private Panel pnlLeftBoardContainer;
        private Label lblLeftBoardTitle;
        private Panel pnlPlayerBoard;

        private Panel pnlCenterDashboard;
        private Label lblTurnIndicator;
        private Label lblTurnMessage;
        private Panel pnlTimers;
        private Label lblMatchTimer;
        private Label lblTurnTimer;
        private ProgressBar prgTurnTime;

        // Bảng thống kê
        private Panel pnlStats;
        private Label lblStatsTitle;
        private Label lblPlayerFleetStatus;
        private Label lblEnemyFleetStatus;
        private Label lblAccuracy;
        private Label lblShotCounts;

        // Kho tàu & điều khiển xếp tàu
        private Panel pnlPlacementControls;
        private Label lblPlacementTitle;
        private ComboBox cboSelectShip;
        private TacticalButton btnRotateShip;
        private TacticalButton btnAutoPlace;
        private TacticalButton btnResetBoard;
        private TacticalButton btnStartBattle;

        // Kỹ năng đặc biệt & Điều khiển trận đấu
        private Panel pnlCombatControls;
        private Label lblCombatSkillsTitle;
        private TacticalButton btnSkillRadar;
        private TacticalButton btnSkillCrossBomb;
        private TacticalButton btnPauseGame;
        private TacticalButton btnSurrender;

        // Bàn cờ bên phải
        private Panel pnlRightBoardContainer;
        private Label lblRightBoardTitle;
        private Panel pnlRadarBoard;

        // Timers
        private System.Windows.Forms.Timer tmrMatch;
        private System.Windows.Forms.Timer tmrTurnCountdown;

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
            this.components = new System.ComponentModel.Container();

            this.pnlTopBar = new Panel();
            this.lblMatchModeInfo = new Label();
            this.btnBackToMenu = new TacticalButton();
            this.btnToggleSound = new TacticalButton();

            this.pnlLeftBoardContainer = new Panel();
            this.lblLeftBoardTitle = new Label();
            this.pnlPlayerBoard = new Panel();

            this.pnlCenterDashboard = new Panel();
            this.lblTurnIndicator = new Label();
            this.lblTurnMessage = new Label();

            this.pnlTimers = new Panel();
            this.lblMatchTimer = new Label();
            this.lblTurnTimer = new Label();
            this.prgTurnTime = new ProgressBar();

            this.pnlStats = new Panel();
            this.lblStatsTitle = new Label();
            this.lblPlayerFleetStatus = new Label();
            this.lblEnemyFleetStatus = new Label();
            this.lblAccuracy = new Label();
            this.lblShotCounts = new Label();

            this.pnlPlacementControls = new Panel();
            this.lblPlacementTitle = new Label();
            this.cboSelectShip = new ComboBox();
            this.btnRotateShip = new TacticalButton();
            this.btnAutoPlace = new TacticalButton();
            this.btnResetBoard = new TacticalButton();
            this.btnStartBattle = new TacticalButton();

            this.pnlCombatControls = new Panel();
            this.lblCombatSkillsTitle = new Label();
            this.btnSkillRadar = new TacticalButton();
            this.btnSkillCrossBomb = new TacticalButton();
            this.btnPauseGame = new TacticalButton();
            this.btnSurrender = new TacticalButton();

            this.pnlRightBoardContainer = new Panel();
            this.lblRightBoardTitle = new Label();
            this.pnlRadarBoard = new Panel();

            this.tmrMatch = new System.Windows.Forms.Timer(this.components);
            this.tmrTurnCountdown = new System.Windows.Forms.Timer(this.components);

            this.SuspendLayout();

            // 
            // frmBanTau
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = UITheme.BackgroundDark;
            this.ClientSize = new Size(1320, 640);
            this.MinimumSize = new Size(1320, 640);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.Name = "frmBanTau";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(this.frmBanTau_KeyDown);
            this.Resize += new EventHandler(this.frmBanTau_Resize);
            this.Load += new EventHandler(this.frmBanTau_Load);

            // 
            // pnlTopBar
            // 
            this.pnlTopBar.BackColor = UITheme.HeaderBackground;
            this.pnlTopBar.Dock = DockStyle.Top;
            this.pnlTopBar.Height = 56;
            this.pnlTopBar.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(this.pnlTopBar);

            // 
            // btnBackToMenu
            // 
            this.btnBackToMenu.Location = new Point(14, 9);
            this.btnBackToMenu.Size = new Size(120, 38);
            this.btnBackToMenu.Text = "⬅ Menu";
            this.btnBackToMenu.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.btnBackToMenu.CornerRadius = 6;
            this.btnBackToMenu.Click += new EventHandler(this.btnBackToMenu_Click);
            this.pnlTopBar.Controls.Add(this.btnBackToMenu);

            // 
            // lblMatchModeInfo (Thông tin trận đấu, chữ xanh lá viết đậm)
            // 
            this.lblMatchModeInfo.AutoSize = false;
            this.lblMatchModeInfo.Location = new Point(148, 14);
            this.lblMatchModeInfo.Size = new Size(820, 26);
            this.lblMatchModeInfo.Font = new Font("Segoe UI", 11.5f, FontStyle.Bold);
            this.lblMatchModeInfo.ForeColor = UITheme.SuccessGreen;
            this.lblMatchModeInfo.TextAlign = ContentAlignment.MiddleLeft;
            this.lblMatchModeInfo.Text = "Chế độ: Đấu với máy (AI)";
            this.pnlTopBar.Controls.Add(this.lblMatchModeInfo);

            // 
            // btnToggleSound
            // 
            this.btnToggleSound.Location = new Point(1150, 9);
            this.btnToggleSound.Size = new Size(150, 38);
            this.btnToggleSound.Text = "🔊 Âm thanh";
            this.btnToggleSound.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.btnToggleSound.CornerRadius = 6;
            this.btnToggleSound.Click += new EventHandler(this.btnToggleSound_Click);
            this.pnlTopBar.Controls.Add(this.btnToggleSound);

            // 
            // pnlLeftBoardContainer (Bàn cờ Đội nhà)
            // 
            this.pnlLeftBoardContainer.BackColor = UITheme.PanelBackground;
            this.pnlLeftBoardContainer.BorderStyle = BorderStyle.FixedSingle;
            this.pnlLeftBoardContainer.Location = new Point(16, 70);
            this.pnlLeftBoardContainer.Size = new Size(424, 550);
            this.Controls.Add(this.pnlLeftBoardContainer);

            this.lblLeftBoardTitle.AutoSize = false;
            this.lblLeftBoardTitle.Location = new Point(10, 8);
            this.lblLeftBoardTitle.Size = new Size(404, 24);
            this.lblLeftBoardTitle.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            this.lblLeftBoardTitle.ForeColor = UITheme.PrimaryCyan;
            this.lblLeftBoardTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblLeftBoardTitle.Text = "🛡️ Hạm đội của bạn";
            this.pnlLeftBoardContainer.Controls.Add(this.lblLeftBoardTitle);

            this.pnlPlayerBoard.Location = new Point(12, 38);
            this.pnlPlayerBoard.Size = new Size(400, 498);
            this.pnlPlayerBoard.BackColor = UITheme.HeaderBackground;
            this.pnlPlayerBoard.BorderStyle = BorderStyle.FixedSingle;
            this.pnlLeftBoardContainer.Controls.Add(this.pnlPlayerBoard);

            // 
            // pnlRightBoardContainer (Bàn cờ Radar Đối phương)
            // 
            this.pnlRightBoardContainer.BackColor = UITheme.PanelBackground;
            this.pnlRightBoardContainer.BorderStyle = BorderStyle.FixedSingle;
            this.pnlRightBoardContainer.Location = new Point(880, 70);
            this.pnlRightBoardContainer.Size = new Size(424, 550);
            this.Controls.Add(this.pnlRightBoardContainer);

            this.lblRightBoardTitle.AutoSize = false;
            this.lblRightBoardTitle.Location = new Point(10, 8);
            this.lblRightBoardTitle.Size = new Size(404, 24);
            this.lblRightBoardTitle.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            this.lblRightBoardTitle.ForeColor = UITheme.AccentNeon;
            this.lblRightBoardTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblRightBoardTitle.Text = "🎯 Radar đối thủ";
            this.pnlRightBoardContainer.Controls.Add(this.lblRightBoardTitle);

            this.pnlRadarBoard.Location = new Point(12, 38);
            this.pnlRadarBoard.Size = new Size(400, 498);
            this.pnlRadarBoard.BackColor = UITheme.HeaderBackground;
            this.pnlRadarBoard.BorderStyle = BorderStyle.FixedSingle;
            this.pnlRightBoardContainer.Controls.Add(this.pnlRadarBoard);

            // 
            // pnlCenterDashboard (Trung tâm điều khiển ở giữa)
            // 
            this.pnlCenterDashboard.BackColor = UITheme.PanelBackground;
            this.pnlCenterDashboard.BorderStyle = BorderStyle.FixedSingle;
            this.pnlCenterDashboard.Location = new Point(454, 70);
            this.pnlCenterDashboard.Size = new Size(412, 550);
            this.Controls.Add(this.pnlCenterDashboard);

            // Chỉ báo lượt
            this.lblTurnIndicator.AutoSize = false;
            this.lblTurnIndicator.Location = new Point(10, 8);
            this.lblTurnIndicator.Size = new Size(392, 26);
            this.lblTurnIndicator.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            this.lblTurnIndicator.ForeColor = UITheme.AccentNeon;
            this.lblTurnIndicator.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTurnIndicator.Text = "Bố trí hạm đội";
            this.pnlCenterDashboard.Controls.Add(this.lblTurnIndicator);

            this.lblTurnMessage.AutoSize = false;
            this.lblTurnMessage.Location = new Point(10, 36);
            this.lblTurnMessage.Size = new Size(392, 22);
            this.lblTurnMessage.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.lblTurnMessage.ForeColor = UITheme.TextSecondary;
            this.lblTurnMessage.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTurnMessage.Text = "Chọn tàu và click vị trí trên bàn cờ để đặt";
            this.pnlCenterDashboard.Controls.Add(this.lblTurnMessage);

            // Bộ đồng hồ
            this.pnlTimers.BackColor = UITheme.HeaderBackground;
            this.pnlTimers.BorderStyle = BorderStyle.FixedSingle;
            this.pnlTimers.Location = new Point(14, 64);
            this.pnlTimers.Size = new Size(384, 58);
            this.pnlCenterDashboard.Controls.Add(this.pnlTimers);

            this.lblMatchTimer.AutoSize = false;
            this.lblMatchTimer.Location = new Point(12, 6);
            this.lblMatchTimer.Size = new Size(170, 20);
            this.lblMatchTimer.Font = new Font("Consolas", 11f, FontStyle.Bold);
            this.lblMatchTimer.ForeColor = UITheme.TextPrimary;
            this.lblMatchTimer.Text = "⏱ Trận: 00:00";
            this.pnlTimers.Controls.Add(this.lblMatchTimer);

            this.lblTurnTimer.AutoSize = false;
            this.lblTurnTimer.Location = new Point(200, 6);
            this.lblTurnTimer.Size = new Size(170, 20);
            this.lblTurnTimer.Font = new Font("Consolas", 11f, FontStyle.Bold);
            this.lblTurnTimer.ForeColor = UITheme.AccentNeon;
            this.lblTurnTimer.TextAlign = ContentAlignment.TopRight;
            this.lblTurnTimer.Text = "⏳ Lượt: 30s";
            this.pnlTimers.Controls.Add(this.lblTurnTimer);

            this.prgTurnTime.Location = new Point(12, 30);
            this.prgTurnTime.Size = new Size(360, 18);
            this.prgTurnTime.Maximum = 30;
            this.prgTurnTime.Value = 30;
            this.pnlTimers.Controls.Add(this.prgTurnTime);

            // Thống kê trận đấu
            this.pnlStats.BackColor = UITheme.HeaderBackground;
            this.pnlStats.BorderStyle = BorderStyle.FixedSingle;
            this.pnlStats.Location = new Point(14, 128);
            this.pnlStats.Size = new Size(384, 118);
            this.pnlCenterDashboard.Controls.Add(this.pnlStats);

            this.lblStatsTitle.AutoSize = false;
            this.lblStatsTitle.Location = new Point(10, 6);
            this.lblStatsTitle.Size = new Size(364, 18);
            this.lblStatsTitle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.lblStatsTitle.ForeColor = UITheme.AccentNeon;
            this.lblStatsTitle.Text = "📊 Thống kê tác chiến";
            this.pnlStats.Controls.Add(this.lblStatsTitle);

            this.lblPlayerFleetStatus.AutoSize = false;
            this.lblPlayerFleetStatus.Location = new Point(10, 28);
            this.lblPlayerFleetStatus.Size = new Size(180, 20);
            this.lblPlayerFleetStatus.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            this.lblPlayerFleetStatus.ForeColor = UITheme.PrimaryCyan;
            this.lblPlayerFleetStatus.Text = "🛡️ Bạn: 5/5 tàu (17 ô)";
            this.pnlStats.Controls.Add(this.lblPlayerFleetStatus);

            this.lblEnemyFleetStatus.AutoSize = false;
            this.lblEnemyFleetStatus.Location = new Point(194, 28);
            this.lblEnemyFleetStatus.Size = new Size(180, 20);
            this.lblEnemyFleetStatus.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            this.lblEnemyFleetStatus.ForeColor = UITheme.WarningAmber;
            this.lblEnemyFleetStatus.Text = "🤖 Máy: 5/5 tàu (17 ô)";
            this.pnlStats.Controls.Add(this.lblEnemyFleetStatus);

            this.lblShotCounts.AutoSize = false;
            this.lblShotCounts.Location = new Point(10, 56);
            this.lblShotCounts.Size = new Size(364, 20);
            this.lblShotCounts.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            this.lblShotCounts.ForeColor = UITheme.TextSecondary;
            this.lblShotCounts.Text = "🎯 Khai hỏa: 0  |  Trúng: 0  |  Trượt: 0";
            this.pnlStats.Controls.Add(this.lblShotCounts);

            this.lblAccuracy.AutoSize = false;
            this.lblAccuracy.Location = new Point(10, 82);
            this.lblAccuracy.Size = new Size(364, 24);
            this.lblAccuracy.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            this.lblAccuracy.ForeColor = UITheme.AccentNeon;
            this.lblAccuracy.Text = "⚡ Độ chính xác: 0.0%";
            this.pnlStats.Controls.Add(this.lblAccuracy);

            // Panel Bố trí hạm đội (Kho tàu)
            this.pnlPlacementControls.BackColor = UITheme.PanelBackground;
            this.pnlPlacementControls.Location = new Point(14, 254);
            this.pnlPlacementControls.Size = new Size(384, 284);
            this.pnlCenterDashboard.Controls.Add(this.pnlPlacementControls);

            this.lblPlacementTitle.AutoSize = false;
            this.lblPlacementTitle.Location = new Point(10, 8);
            this.lblPlacementTitle.Size = new Size(364, 20);
            this.lblPlacementTitle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.lblPlacementTitle.ForeColor = UITheme.PrimaryCyan;
            this.lblPlacementTitle.Text = "⚓ Chọn tàu để đặt:";
            this.pnlPlacementControls.Controls.Add(this.lblPlacementTitle);

            this.cboSelectShip.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboSelectShip.BackColor = UITheme.HeaderBackground;
            this.cboSelectShip.ForeColor = UITheme.TextPrimary;
            this.cboSelectShip.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.cboSelectShip.Location = new Point(10, 32);
            this.cboSelectShip.Size = new Size(364, 28);
            this.cboSelectShip.SelectedIndexChanged += new EventHandler(this.cboSelectShip_SelectedIndexChanged);
            this.pnlPlacementControls.Controls.Add(this.cboSelectShip);

            this.btnRotateShip.Location = new Point(10, 72);
            this.btnRotateShip.Size = new Size(364, 42);
            this.btnRotateShip.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.btnRotateShip.Text = "🔄 Xoay hướng (R): Ngang";
            this.btnRotateShip.Click += new EventHandler(this.btnRotateShip_Click);
            this.pnlPlacementControls.Controls.Add(this.btnRotateShip);

            this.btnAutoPlace.Location = new Point(10, 126);
            this.btnAutoPlace.Size = new Size(174, 44);
            this.btnAutoPlace.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.btnAutoPlace.Text = "⚡ Xếp tự động";
            this.btnAutoPlace.Click += new EventHandler(this.btnAutoPlace_Click);
            this.pnlPlacementControls.Controls.Add(this.btnAutoPlace);

            this.btnResetBoard.Location = new Point(200, 126);
            this.btnResetBoard.Size = new Size(174, 44);
            this.btnResetBoard.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.btnResetBoard.Text = "🗑️ Đặt lại";
            this.btnResetBoard.BorderColor = UITheme.DangerRed;
            this.btnResetBoard.Click += new EventHandler(this.btnResetBoard_Click);
            this.pnlPlacementControls.Controls.Add(this.btnResetBoard);

            this.btnStartBattle.Location = new Point(10, 184);
            this.btnStartBattle.Size = new Size(364, 52);
            this.btnStartBattle.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            this.btnStartBattle.Text = "⚔️ Vào trận";
            this.btnStartBattle.BorderColor = UITheme.AccentNeon;
            this.btnStartBattle.HoverBorderColor = UITheme.AccentNeon;
            this.btnStartBattle.HoverTextColor = UITheme.AccentNeon;
            this.btnStartBattle.Click += new EventHandler(this.btnStartBattle_Click);
            this.pnlPlacementControls.Controls.Add(this.btnStartBattle);

            // Panel Chiến đấu (Kỹ năng & Tạm dừng)
            this.pnlCombatControls.BackColor = UITheme.PanelBackground;
            this.pnlCombatControls.Location = new Point(14, 254);
            this.pnlCombatControls.Size = new Size(384, 284);
            this.pnlCombatControls.Visible = false;
            this.pnlCenterDashboard.Controls.Add(this.pnlCombatControls);

            this.lblCombatSkillsTitle.AutoSize = false;
            this.lblCombatSkillsTitle.Location = new Point(10, 8);
            this.lblCombatSkillsTitle.Size = new Size(364, 20);
            this.lblCombatSkillsTitle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.lblCombatSkillsTitle.ForeColor = UITheme.AccentNeon;
            this.lblCombatSkillsTitle.Text = "⚡ Kỹ năng (1 lần/trận):";
            this.pnlCombatControls.Controls.Add(this.lblCombatSkillsTitle);

            this.btnSkillRadar.Location = new Point(10, 32);
            this.btnSkillRadar.Size = new Size(364, 46);
            this.btnSkillRadar.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.btnSkillRadar.Text = "📡 Radar trinh sát (3x3)";
            this.btnSkillRadar.Click += new EventHandler(this.btnSkillRadar_Click);
            this.pnlCombatControls.Controls.Add(this.btnSkillRadar);

            this.btnSkillCrossBomb.Location = new Point(10, 88);
            this.btnSkillCrossBomb.Size = new Size(364, 46);
            this.btnSkillCrossBomb.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.btnSkillCrossBomb.Text = "💥 Pháo kích diện rộng (+)";
            this.btnSkillCrossBomb.Click += new EventHandler(this.btnSkillCrossBomb_Click);
            this.pnlCombatControls.Controls.Add(this.btnSkillCrossBomb);

            this.btnPauseGame.Location = new Point(10, 154);
            this.btnPauseGame.Size = new Size(174, 48);
            this.btnPauseGame.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.btnPauseGame.Text = "⏸ Tạm dừng";
            this.btnPauseGame.Click += new EventHandler(this.btnPauseGame_Click);
            this.pnlCombatControls.Controls.Add(this.btnPauseGame);

            this.btnSurrender.Location = new Point(200, 154);
            this.btnSurrender.Size = new Size(174, 48);
            this.btnSurrender.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.btnSurrender.Text = "🏳 Đầu hàng";
            this.btnSurrender.BorderColor = UITheme.DangerRed;
            this.btnSurrender.Click += new EventHandler(this.btnSurrender_Click);
            this.pnlCombatControls.Controls.Add(this.btnSurrender);

            // 
            // Timers configuration
            // 
            this.tmrMatch.Interval = 1000;
            this.tmrMatch.Tick += new EventHandler(this.tmrMatch_Tick);

            this.tmrTurnCountdown.Interval = 1000;
            this.tmrTurnCountdown.Tick += new EventHandler(this.tmrTurnCountdown_Tick);

            this.ResumeLayout(false);
        }
    }
}
