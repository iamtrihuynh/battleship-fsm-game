using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
    partial class frmAbout
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlTopContainer;

        private Panel pnlHeader;
        private Label lblHeaderTitle;
        private TacticalButton btnClose;

        private Panel pnlTabBar;
        private TacticalButton btnTabRules;
        private TacticalButton btnTabShips;
        private TacticalButton btnTabTactics;
        private TacticalButton btnTabTeam;

        private Panel pnlContentContainer;

        private Panel pnlRulesView;
        private Panel pnlShipsView;
        private Panel pnlTacticsView;
        private Panel pnlTeamView;

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
            this.pnlTopContainer = new Panel();
            this.pnlHeader = new Panel();
            this.lblHeaderTitle = new Label();
            this.btnClose = new TacticalButton();

            this.pnlTabBar = new Panel();
            this.btnTabRules = new TacticalButton();
            this.btnTabShips = new TacticalButton();
            this.btnTabTactics = new TacticalButton();
            this.btnTabTeam = new TacticalButton();

            this.pnlContentContainer = new Panel();
            this.pnlRulesView = new Panel();
            this.pnlShipsView = new Panel();
            this.pnlTacticsView = new Panel();
            this.pnlTeamView = new Panel();

            this.pnlTopContainer.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlTabBar.SuspendLayout();
            this.pnlContentContainer.SuspendLayout();
            this.SuspendLayout();

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = UITheme.BackgroundDark;
            this.ClientSize = new Size(1060, 720);
            this.MinimumSize = new Size(1000, 680);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmAbout";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Hướng dẫn tác chiến hải quân";

            this.pnlTopContainer.BackColor = UITheme.HeaderBackground;
            this.pnlTopContainer.Dock = DockStyle.Top;
            this.pnlTopContainer.Height = 112;
            this.pnlTopContainer.BorderStyle = BorderStyle.FixedSingle;
            this.pnlTopContainer.Controls.Add(this.pnlTabBar);
            this.pnlTopContainer.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlTopContainer);

            this.pnlHeader.BackColor = UITheme.HeaderBackground;
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 54;
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Controls.Add(this.lblHeaderTitle);

            this.lblHeaderTitle.AutoSize = false;
            this.lblHeaderTitle.Location = new Point(24, 11);
            this.lblHeaderTitle.Size = new Size(600, 32);
            this.lblHeaderTitle.Font = new Font("Segoe UI", 15f, FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = UITheme.AccentNeon;
            this.lblHeaderTitle.Text = "📖 Hướng dẫn tác chiến hải quân";
            this.lblHeaderTitle.TextAlign = ContentAlignment.MiddleLeft;

            this.btnClose.Location = new Point(930, 9);
            this.btnClose.Size = new Size(106, 36);
            this.btnClose.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.btnClose.Text = "✕  Đóng";
            this.btnClose.BaseFillColor = Color.FromArgb(30, 41, 59);
            this.btnClose.BorderColor = UITheme.DangerRed;
            this.btnClose.NormalTextColor = Color.FromArgb(248, 113, 113);
            this.btnClose.HoverFillColor = Color.FromArgb(220, 38, 38);
            this.btnClose.HoverTextColor = Color.White;
            this.btnClose.CornerRadius = 6;
            this.btnClose.Click += (s, e) => this.Close();

            this.pnlTabBar.BackColor = Color.FromArgb(16, 26, 46);
            this.pnlTabBar.Dock = DockStyle.Fill;
            this.pnlTabBar.Controls.Add(this.btnTabRules);
            this.pnlTabBar.Controls.Add(this.btnTabShips);
            this.pnlTabBar.Controls.Add(this.btnTabTactics);
            this.pnlTabBar.Controls.Add(this.btnTabTeam);

            this.btnTabRules.Location = new Point(57, 8);
            this.btnTabRules.Size = new Size(226, 40);
            this.btnTabRules.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            this.btnTabRules.Text = "🎯  Luật chơi";
            this.btnTabRules.CornerRadius = 6;
            this.btnTabRules.Click += new EventHandler(this.btnTabRules_Click);

            this.btnTabShips.Location = new Point(297, 8);
            this.btnTabShips.Size = new Size(226, 40);
            this.btnTabShips.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            this.btnTabShips.Text = "🚢  Thông tin hạm đội";
            this.btnTabShips.CornerRadius = 6;
            this.btnTabShips.Click += new EventHandler(this.btnTabShips_Click);

            this.btnTabTactics.Location = new Point(537, 8);
            this.btnTabTactics.Size = new Size(226, 40);
            this.btnTabTactics.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            this.btnTabTactics.Text = "⚡  Mẹo chiến thuật";
            this.btnTabTactics.CornerRadius = 6;
            this.btnTabTactics.Click += new EventHandler(this.btnTabTactics_Click);

            this.btnTabTeam.Location = new Point(777, 8);
            this.btnTabTeam.Size = new Size(226, 40);
            this.btnTabTeam.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            this.btnTabTeam.Text = "👥  Thành viên nhóm";
            this.btnTabTeam.CornerRadius = 6;
            this.btnTabTeam.Click += new EventHandler(this.btnTabTeam_Click);

            this.pnlContentContainer.BackColor = UITheme.PanelBackground;
            this.pnlContentContainer.Dock = DockStyle.Fill;
            this.pnlContentContainer.Padding = new Padding(16);
            this.pnlContentContainer.Controls.Add(this.pnlRulesView);
            this.pnlContentContainer.Controls.Add(this.pnlShipsView);
            this.pnlContentContainer.Controls.Add(this.pnlTacticsView);
            this.pnlContentContainer.Controls.Add(this.pnlTeamView);
            this.Controls.Add(this.pnlContentContainer);

            this.pnlRulesView.Dock = DockStyle.Fill;
            this.pnlRulesView.BackColor = UITheme.PanelBackground;
            this.pnlRulesView.AutoScroll = true;
            this.pnlRulesView.Paint += new PaintEventHandler(this.pnlRulesView_Paint);

            this.pnlShipsView.Dock = DockStyle.Fill;
            this.pnlShipsView.BackColor = UITheme.PanelBackground;
            this.pnlShipsView.AutoScroll = true;
            this.pnlShipsView.Visible = false;
            this.pnlShipsView.Paint += new PaintEventHandler(this.pnlShipsView_Paint);

            this.pnlTacticsView.Dock = DockStyle.Fill;
            this.pnlTacticsView.BackColor = UITheme.PanelBackground;
            this.pnlTacticsView.AutoScroll = true;
            this.pnlTacticsView.Visible = false;
            this.pnlTacticsView.Paint += new PaintEventHandler(this.pnlTacticsView_Paint);

            this.pnlTeamView.Dock = DockStyle.Fill;
            this.pnlTeamView.BackColor = UITheme.PanelBackground;
            this.pnlTeamView.AutoScroll = true;
            this.pnlTeamView.Visible = false;
            this.pnlTeamView.Paint += new PaintEventHandler(this.pnlTeamView_Paint);

            this.pnlHeader.SendToBack();
            this.pnlTabBar.BringToFront();
            this.pnlTopContainer.SendToBack();
            this.pnlContentContainer.BringToFront();

            this.pnlTopContainer.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlTabBar.ResumeLayout(false);
            this.pnlContentContainer.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
