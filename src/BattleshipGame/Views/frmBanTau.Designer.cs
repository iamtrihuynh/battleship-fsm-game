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
        private Label lblMatchTitle;
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

        // Màn che sương mù chuyển lượt PvP Hotseat
        private Panel pnlFogOverlay;
        private Label lblFogTitle;
        private Label lblFogMessage;
        private TacticalButton btnFogContinue;

        // Bảng chú thích hai bên
        private Panel pnlLeftNote;
        private Panel pnlRightNote;

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
            this.lblMatchTitle = new Label();
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

            this.pnlFogOverlay = new Panel();
            this.lblFogTitle = new Label();
            this.lblFogMessage = new Label();
            this.btnFogContinue = new TacticalButton();

            this.tmrMatch = new System.Windows.Forms.Timer(this.components);
            this.tmrTurnCountdown = new System.Windows.Forms.Timer(this.components);

            this.SuspendLayout();

            // 
            // frmBanTau
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = UITheme.BackgroundDark;
            this.ClientSize = new Size(1260, 780);
            this.MinimumSize = new Size(1260, 780);
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
            this.pnlTopBar.Height = 65;
            this.pnlTopBar.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(this.pnlTopBar);

            // 
            // btnBackToMenu
            // 
            this.btnBackToMenu.Location = new Point(14, 12);
            this.btnBackToMenu.Size = new Size(130, 38);
            this.btnBackToMenu.Text = "⬅ MENU";
            this.btnBackToMenu.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.btnBackToMenu.CornerRadius = 6;
            this.btnBackToMenu.Click += new EventHandler(this.btnBackToMenu_Click);
            this.pnlTopBar.Controls.Add(this.btnBackToMenu);

            // 
            // lblMatchTitle
            // 
            this.lblMatchTitle.AutoSize = false;
            this.lblMatchTitle.Location = new Point(160, 8);
            this.lblMatchTitle.Size = new Size(720, 28);
            this.lblMatchTitle.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
            this.lblMatchTitle.ForeColor = UITheme.AccentNeon;
            this.lblMatchTitle.Text = "HẢI CHIẾN CHIẾN THUẬT - TRẬN ĐỊA BẮN TÀU";
            this.pnlTopBar.Controls.Add(this.lblMatchTitle);

            // 
            // lblMatchModeInfo
            // 
            this.lblMatchModeInfo.AutoSize = false;
            this.lblMatchModeInfo.Location = new Point(162, 36);
            this.lblMatchModeInfo.Size = new Size(720, 20);
            this.lblMatchModeInfo.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.lblMatchModeInfo.ForeColor = UITheme.PrimaryCyan;
            this.lblMatchModeInfo.Text = "Chế độ: Đấu với Máy (AI Nâng cao) • Quy chuẩn hạm đội 5 tàu";
            this.pnlTopBar.Controls.Add(this.lblMatchModeInfo);

            // 
            // btnToggleSound
            // 
            this.btnToggleSound.Location = new Point(1090, 12);
            this.btnToggleSound.Size = new Size(150, 38);
            this.btnToggleSound.Text = "🔊 ÂM THANH";
            this.btnToggleSound.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.btnToggleSound.CornerRadius = 6;
            this.btnToggleSound.Click += new EventHandler(this.btnToggleSound_Click);
            this.pnlTopBar.Controls.Add(this.btnToggleSound);

            // 
            // pnlLeftBoardContainer (Bàn cờ Đội nhà)
            // 
            this.pnlLeftBoardContainer.BackColor = UITheme.PanelBackground;
            this.pnlLeftBoardContainer.BorderStyle = BorderStyle.FixedSingle;
            this.pnlLeftBoardContainer.Location = new Point(16, 78);
            this.pnlLeftBoardContainer.Size = new Size(420, 480);
            this.Controls.Add(this.pnlLeftBoardContainer);

            this.lblLeftBoardTitle.AutoSize = false;
            this.lblLeftBoardTitle.Location = new Point(10, 8);
            this.lblLeftBoardTitle.Size = new Size(400, 24);
            this.lblLeftBoardTitle.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            this.lblLeftBoardTitle.ForeColor = UITheme.PrimaryCyan;
            this.lblLeftBoardTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblLeftBoardTitle.Text = "🛡️ VÙNG BIỂN CỦA BẠN (HẠM ĐỘI)";
            this.pnlLeftBoardContainer.Controls.Add(this.lblLeftBoardTitle);

            this.pnlPlayerBoard.Location = new Point(10, 36);
            this.pnlPlayerBoard.Size = new Size(400, 434);
            this.pnlPlayerBoard.BackColor = UITheme.HeaderBackground;
            this.pnlPlayerBoard.BorderStyle = BorderStyle.FixedSingle;
            this.pnlLeftBoardContainer.Controls.Add(this.pnlPlayerBoard);

            // 
            // pnlRightBoardContainer (Bàn cờ Radar Đối phương)
            // 
            this.pnlRightBoardContainer.BackColor = UITheme.PanelBackground;
            this.pnlRightBoardContainer.BorderStyle = BorderStyle.FixedSingle;
            this.pnlRightBoardContainer.Location = new Point(824, 78);
            this.pnlRightBoardContainer.Size = new Size(420, 480);
            this.Controls.Add(this.pnlRightBoardContainer);

            this.lblRightBoardTitle.AutoSize = false;
            this.lblRightBoardTitle.Location = new Point(10, 8);
            this.lblRightBoardTitle.Size = new Size(400, 24);
            this.lblRightBoardTitle.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            this.lblRightBoardTitle.ForeColor = UITheme.AccentNeon;
            this.lblRightBoardTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblRightBoardTitle.Text = "🎯 RADAR KHAI HỎA (ĐỐI PHƯƠNG)";
            this.pnlRightBoardContainer.Controls.Add(this.lblRightBoardTitle);

            this.pnlRadarBoard.Location = new Point(10, 36);
            this.pnlRadarBoard.Size = new Size(400, 434);
            this.pnlRadarBoard.BackColor = UITheme.HeaderBackground;
            this.pnlRadarBoard.BorderStyle = BorderStyle.FixedSingle;
            this.pnlRightBoardContainer.Controls.Add(this.pnlRadarBoard);

            // 
            // pnlCenterDashboard (Trung tâm điều khiển ở giữa)
            // 
            this.pnlCenterDashboard.BackColor = UITheme.PanelBackground;
            this.pnlCenterDashboard.BorderStyle = BorderStyle.FixedSingle;
            this.pnlCenterDashboard.Location = new Point(446, 78);
            this.pnlCenterDashboard.Size = new Size(368, 688);
            this.Controls.Add(this.pnlCenterDashboard);

            // Chỉ báo lượt
            this.lblTurnIndicator.AutoSize = false;
            this.lblTurnIndicator.Location = new Point(10, 10);
            this.lblTurnIndicator.Size = new Size(346, 32);
            this.lblTurnIndicator.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            this.lblTurnIndicator.ForeColor = UITheme.AccentNeon;
            this.lblTurnIndicator.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTurnIndicator.Text = "GIAI ĐOẠN: BỐ TRÍ HẠM ĐỘI";
            this.pnlCenterDashboard.Controls.Add(this.lblTurnIndicator);

            this.lblTurnMessage.AutoSize = false;
            this.lblTurnMessage.Location = new Point(10, 44);
            this.lblTurnMessage.Size = new Size(346, 20);
            this.lblTurnMessage.Font = new Font("Segoe UI", 9f, FontStyle.Italic);
            this.lblTurnMessage.ForeColor = UITheme.TextSecondary;
            this.lblTurnMessage.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTurnMessage.Text = "Chọn tàu và click vị trí đặt trên bàn cờ của bạn";
            this.pnlCenterDashboard.Controls.Add(this.lblTurnMessage);

            // Bộ đồng hồ
            this.pnlTimers.BackColor = UITheme.HeaderBackground;
            this.pnlTimers.BorderStyle = BorderStyle.FixedSingle;
            this.pnlTimers.Location = new Point(12, 70);
            this.pnlTimers.Size = new Size(342, 72);
            this.pnlCenterDashboard.Controls.Add(this.pnlTimers);

            this.lblMatchTimer.AutoSize = false;
            this.lblMatchTimer.Location = new Point(10, 8);
            this.lblMatchTimer.Size = new Size(150, 24);
            this.lblMatchTimer.Font = new Font("Consolas", 11f, FontStyle.Bold);
            this.lblMatchTimer.ForeColor = UITheme.TextPrimary;
            this.lblMatchTimer.Text = "⏱ Trận: 00:00";
            this.pnlTimers.Controls.Add(this.lblMatchTimer);

            this.lblTurnTimer.AutoSize = false;
            this.lblTurnTimer.Location = new Point(180, 8);
            this.lblTurnTimer.Size = new Size(150, 24);
            this.lblTurnTimer.Font = new Font("Consolas", 11f, FontStyle.Bold);
            this.lblTurnTimer.ForeColor = UITheme.AccentNeon;
            this.lblTurnTimer.TextAlign = ContentAlignment.TopRight;
            this.lblTurnTimer.Text = "⏳ Lượt: 30s";
            this.pnlTimers.Controls.Add(this.lblTurnTimer);

            this.prgTurnTime.Location = new Point(10, 42);
            this.prgTurnTime.Size = new Size(320, 18);
            this.prgTurnTime.Maximum = 30;
            this.prgTurnTime.Value = 30;
            this.pnlTimers.Controls.Add(this.prgTurnTime);

            // Thống kê trận đấu
            this.pnlStats.BackColor = UITheme.HeaderBackground;
            this.pnlStats.BorderStyle = BorderStyle.FixedSingle;
            this.pnlStats.Location = new Point(12, 150);
            this.pnlStats.Size = new Size(342, 100);
            this.pnlCenterDashboard.Controls.Add(this.pnlStats);

            this.lblStatsTitle.AutoSize = false;
            this.lblStatsTitle.Location = new Point(8, 6);
            this.lblStatsTitle.Size = new Size(324, 18);
            this.lblStatsTitle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            this.lblStatsTitle.ForeColor = UITheme.PrimaryCyan;
            this.lblStatsTitle.Text = "📊 THỐNG KÊ CHIẾN TRƯỜNG";
            this.pnlStats.Controls.Add(this.lblStatsTitle);

            this.lblPlayerFleetStatus.AutoSize = false;
            this.lblPlayerFleetStatus.Location = new Point(8, 28);
            this.lblPlayerFleetStatus.Size = new Size(160, 20);
            this.lblPlayerFleetStatus.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            this.lblPlayerFleetStatus.ForeColor = UITheme.TextPrimary;
            this.lblPlayerFleetStatus.Text = "Tàu bạn: 5/5";
            this.pnlStats.Controls.Add(this.lblPlayerFleetStatus);

            this.lblEnemyFleetStatus.AutoSize = false;
            this.lblEnemyFleetStatus.Location = new Point(170, 28);
            this.lblEnemyFleetStatus.Size = new Size(160, 20);
            this.lblEnemyFleetStatus.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            this.lblEnemyFleetStatus.ForeColor = UITheme.TextPrimary;
            this.lblEnemyFleetStatus.Text = "Tàu địch: 5/5";
            this.pnlStats.Controls.Add(this.lblEnemyFleetStatus);

            this.lblShotCounts.AutoSize = false;
            this.lblShotCounts.Location = new Point(8, 52);
            this.lblShotCounts.Size = new Size(320, 20);
            this.lblShotCounts.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            this.lblShotCounts.ForeColor = UITheme.TextSecondary;
            this.lblShotCounts.Text = "Số phát bắn: 0 • Trúng: 0";
            this.pnlStats.Controls.Add(this.lblShotCounts);

            this.lblAccuracy.AutoSize = false;
            this.lblAccuracy.Location = new Point(8, 74);
            this.lblAccuracy.Size = new Size(320, 20);
            this.lblAccuracy.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            this.lblAccuracy.ForeColor = UITheme.AccentNeon;
            this.lblAccuracy.Text = "Độ chính xác: 0.0%";
            this.pnlStats.Controls.Add(this.lblAccuracy);

            // Panel Bố trí hạm đội (Kho tàu)
            this.pnlPlacementControls.BackColor = UITheme.PanelBackground;
            this.pnlPlacementControls.Location = new Point(12, 258);
            this.pnlPlacementControls.Size = new Size(342, 230);
            this.pnlCenterDashboard.Controls.Add(this.pnlPlacementControls);

            this.lblPlacementTitle.AutoSize = false;
            this.lblPlacementTitle.Location = new Point(4, 4);
            this.lblPlacementTitle.Size = new Size(330, 20);
            this.lblPlacementTitle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.lblPlacementTitle.ForeColor = UITheme.PrimaryCyan;
            this.lblPlacementTitle.Text = "⚓ KHO TÀU CHIẾN HẠM:";
            this.pnlPlacementControls.Controls.Add(this.lblPlacementTitle);

            this.cboSelectShip.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboSelectShip.BackColor = UITheme.HeaderBackground;
            this.cboSelectShip.ForeColor = UITheme.TextPrimary;
            this.cboSelectShip.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.cboSelectShip.Location = new Point(6, 28);
            this.cboSelectShip.Size = new Size(330, 26);
            this.cboSelectShip.SelectedIndexChanged += new EventHandler(this.cboSelectShip_SelectedIndexChanged);
            this.pnlPlacementControls.Controls.Add(this.cboSelectShip);

            this.btnRotateShip.Location = new Point(6, 62);
            this.btnRotateShip.Size = new Size(330, 36);
            this.btnRotateShip.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            this.btnRotateShip.Text = "🔄 Xoay Hướng (Phím R / Space): NGANG";
            this.btnRotateShip.Click += new EventHandler(this.btnRotateShip_Click);
            this.pnlPlacementControls.Controls.Add(this.btnRotateShip);

            this.btnAutoPlace.Location = new Point(6, 104);
            this.btnAutoPlace.Size = new Size(160, 36);
            this.btnAutoPlace.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            this.btnAutoPlace.Text = "⚡ Xếp Tự Động";
            this.btnAutoPlace.Click += new EventHandler(this.btnAutoPlace_Click);
            this.pnlPlacementControls.Controls.Add(this.btnAutoPlace);

            this.btnResetBoard.Location = new Point(176, 104);
            this.btnResetBoard.Size = new Size(160, 36);
            this.btnResetBoard.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            this.btnResetBoard.Text = "🗑️ Đặt Lại";
            this.btnResetBoard.BorderColor = UITheme.DangerRed;
            this.btnResetBoard.Click += new EventHandler(this.btnResetBoard_Click);
            this.pnlPlacementControls.Controls.Add(this.btnResetBoard);

            this.btnStartBattle.Location = new Point(6, 150);
            this.btnStartBattle.Size = new Size(330, 48);
            this.btnStartBattle.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            this.btnStartBattle.Text = "⚔️ BẮT ĐẦU CHIẾN ĐẤU";
            this.btnStartBattle.BorderColor = UITheme.AccentNeon;
            this.btnStartBattle.HoverBorderColor = UITheme.AccentNeon;
            this.btnStartBattle.HoverTextColor = UITheme.AccentNeon;
            this.btnStartBattle.Click += new EventHandler(this.btnStartBattle_Click);
            this.pnlPlacementControls.Controls.Add(this.btnStartBattle);

            // Panel Chiến đấu (Kỹ năng & Tạm dừng)
            this.pnlCombatControls.BackColor = UITheme.PanelBackground;
            this.pnlCombatControls.Location = new Point(12, 495);
            this.pnlCombatControls.Size = new Size(342, 180);
            this.pnlCenterDashboard.Controls.Add(this.pnlCombatControls);

            this.lblCombatSkillsTitle.AutoSize = false;
            this.lblCombatSkillsTitle.Location = new Point(4, 4);
            this.lblCombatSkillsTitle.Size = new Size(330, 20);
            this.lblCombatSkillsTitle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.lblCombatSkillsTitle.ForeColor = UITheme.AccentNeon;
            this.lblCombatSkillsTitle.Text = "✨ KỸ NĂNG ĐẶC BIỆT (1 LẦN/VÁN):";
            this.pnlCombatControls.Controls.Add(this.lblCombatSkillsTitle);

            this.btnSkillRadar.Location = new Point(6, 26);
            this.btnSkillRadar.Size = new Size(330, 36);
            this.btnSkillRadar.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            this.btnSkillRadar.Text = "📡 Radar Trinh Sát 3x3 [Khả dụng]";
            this.btnSkillRadar.Click += new EventHandler(this.btnSkillRadar_Click);
            this.pnlCombatControls.Controls.Add(this.btnSkillRadar);

            this.btnSkillCrossBomb.Location = new Point(6, 68);
            this.btnSkillCrossBomb.Size = new Size(330, 36);
            this.btnSkillCrossBomb.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            this.btnSkillCrossBomb.Text = "💥 Pháo Kích Dấu (+) 5 Ô [Khả dụng]";
            this.btnSkillCrossBomb.Click += new EventHandler(this.btnSkillCrossBomb_Click);
            this.pnlCombatControls.Controls.Add(this.btnSkillCrossBomb);

            this.btnPauseGame.Location = new Point(6, 114);
            this.btnPauseGame.Size = new Size(160, 36);
            this.btnPauseGame.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            this.btnPauseGame.Text = "⏸ Tạm Dừng";
            this.btnPauseGame.Click += new EventHandler(this.btnPauseGame_Click);
            this.pnlCombatControls.Controls.Add(this.btnPauseGame);

            this.btnSurrender.Location = new Point(176, 114);
            this.btnSurrender.Size = new Size(160, 36);
            this.btnSurrender.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            this.btnSurrender.Text = "🏳 Đầu Hàng";
            this.btnSurrender.BorderColor = UITheme.DangerRed;
            this.btnSurrender.Click += new EventHandler(this.btnSurrender_Click);
            this.pnlCombatControls.Controls.Add(this.btnSurrender);

            // 
            // Panel Bảng hướng dẫn phụ bên dưới 2 bàn cờ
            // 
            this.pnlLeftNote = new Panel();
            this.pnlLeftNote.BackColor = UITheme.PanelBackground;
            this.pnlLeftNote.BorderStyle = BorderStyle.FixedSingle;
            this.pnlLeftNote.Location = new Point(16, 566);
            this.pnlLeftNote.Size = new Size(420, 200);
            this.Controls.Add(this.pnlLeftNote);

            Label lblLeftNote = new Label();
            lblLeftNote.AutoSize = false;
            lblLeftNote.Dock = DockStyle.Fill;
            lblLeftNote.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            lblLeftNote.ForeColor = UITheme.TextSecondary;
            lblLeftNote.Padding = new Padding(12);
            lblLeftNote.Text = "📌 QUY TẮC BÀN CỜ HẠM ĐỘI:\n" +
                               "• Tàu sân bay (5 ô) • Thiết giáp hạm (4 ô)\n" +
                               "• Tàu ngầm (3 ô) • Tàu khu trục (3 ô) • Tàu tuần tra (2 ô)\n" +
                               "• Rê chuột để xem bóng tàu (Xanh = Hợp lệ, Đỏ = Va chạm).\n" +
                               "• Nhấn phím R hoặc Space để xoay chiều Ngang/Dọc.\n" +
                               "• Phải xếp đủ 5 tàu trước khi bấm 'Bắt đầu chiến đấu'.";
            this.pnlLeftNote.Controls.Add(lblLeftNote);

            this.pnlRightNote = new Panel();
            this.pnlRightNote.BackColor = UITheme.PanelBackground;
            this.pnlRightNote.BorderStyle = BorderStyle.FixedSingle;
            this.pnlRightNote.Location = new Point(824, 566);
            this.pnlRightNote.Size = new Size(420, 200);
            this.Controls.Add(this.pnlRightNote);

            Label lblRightNote = new Label();
            lblRightNote.AutoSize = false;
            lblRightNote.Dock = DockStyle.Fill;
            lblRightNote.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            lblRightNote.ForeColor = UITheme.TextSecondary;
            lblRightNote.Padding = new Padding(12);
            lblRightNote.Text = "🎯 QUY TẮC RADAR KHAI HỎA:\n" +
                                "• Click vào tọa độ chưa bắn để phóng hỏa lực.\n" +
                                "• Trúng (Hit): Lửa đỏ rực -> Được bắn tiếp 1 lượt (Thưởng lượt)!\n" +
                                "• Trượt (Miss): Gợn sóng xám -> Mất lượt, quyền bắn đổi sang đối thủ.\n" +
                                "• Chìm (Sunk): Tàu địch bị bắn hạ hoàn toàn -> Còi hụ vang dội.\n" +
                                "• Tiêu diệt toàn bộ 17 ô tàu đối phương để giành chiến thắng!";
            pnlRightNote.Controls.Add(lblRightNote);

            // 
            // Màn che sương mù (PvP Hotseat Overlay)
            // 
            this.pnlFogOverlay.BackColor = UITheme.FogOverlayBackground;
            this.pnlFogOverlay.Dock = DockStyle.Fill;
            this.pnlFogOverlay.Visible = false;
            this.Controls.Add(this.pnlFogOverlay);

            this.lblFogTitle.AutoSize = false;
            this.lblFogTitle.Location = new Point(230, 220);
            this.lblFogTitle.Size = new Size(800, 48);
            this.lblFogTitle.Font = new Font("Segoe UI", 24f, FontStyle.Bold);
            this.lblFogTitle.ForeColor = UITheme.AccentNeon;
            this.lblFogTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblFogTitle.Text = "🌫️ MÀN CHE CHIẾN THUẬT SƯƠNG MÙ";
            this.pnlFogOverlay.Controls.Add(this.lblFogTitle);

            this.lblFogMessage.AutoSize = false;
            this.lblFogMessage.Location = new Point(230, 280);
            this.lblFogMessage.Size = new Size(800, 60);
            this.lblFogMessage.Font = new Font("Segoe UI", 13f, FontStyle.Regular);
            this.lblFogMessage.ForeColor = UITheme.TextPrimary;
            this.lblFogMessage.TextAlign = ContentAlignment.MiddleCenter;
            this.lblFogMessage.Text = "Đã hết lượt bắn của người chơi!\nVui lòng nhận máy và bấm 'Tiếp Tục' để mở bàn cờ tác chiến của bạn.";
            this.pnlFogOverlay.Controls.Add(this.lblFogMessage);

            this.btnFogContinue.Location = new Point(460, 370);
            this.btnFogContinue.Size = new Size(340, 56);
            this.btnFogContinue.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            this.btnFogContinue.Text = "⚓ Người chơi 2: Sẵn sàng";
            this.btnFogContinue.BorderColor = UITheme.AccentNeon;
            this.btnFogContinue.HoverBorderColor = UITheme.AccentNeon;
            this.btnFogContinue.HoverTextColor = UITheme.AccentNeon;
            this.btnFogContinue.Click += new EventHandler(this.btnFogContinue_Click);
            this.pnlFogOverlay.Controls.Add(this.btnFogContinue);

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
