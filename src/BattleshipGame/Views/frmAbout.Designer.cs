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
        private PictureBox picAboutLogo;
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
            pnlTopContainer = new Panel();
            pnlTabBar = new Panel();
            btnTabRules = new TacticalButton();
            btnTabShips = new TacticalButton();
            btnTabTactics = new TacticalButton();
            btnTabTeam = new TacticalButton();
            pnlHeader = new Panel();
            btnClose = new TacticalButton();
            lblHeaderTitle = new Label();
            picAboutLogo = new PictureBox();
            pnlContentContainer = new Panel();
            pnlRulesView = new Panel();
            pnlShipsView = new Panel();
            pnlTacticsView = new Panel();
            pnlTeamView = new Panel();
            pnlTopContainer.SuspendLayout();
            pnlTabBar.SuspendLayout();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picAboutLogo).BeginInit();
            pnlContentContainer.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTopContainer
            // 
            pnlTopContainer.BackColor = Color.FromArgb(11, 19, 36);
            pnlTopContainer.BorderStyle = BorderStyle.FixedSingle;
            pnlTopContainer.Controls.Add(pnlTabBar);
            pnlTopContainer.Controls.Add(pnlHeader);
            pnlTopContainer.Dock = DockStyle.Top;
            pnlTopContainer.Location = new Point(0, 0);
            pnlTopContainer.Margin = new Padding(3, 4, 3, 4);
            pnlTopContainer.Name = "pnlTopContainer";
            pnlTopContainer.Size = new Size(1211, 149);
            pnlTopContainer.TabIndex = 0;
            // 
            // pnlTabBar
            // 
            pnlTabBar.BackColor = Color.FromArgb(16, 26, 46);
            pnlTabBar.Controls.Add(btnTabRules);
            pnlTabBar.Controls.Add(btnTabShips);
            pnlTabBar.Controls.Add(btnTabTactics);
            pnlTabBar.Controls.Add(btnTabTeam);
            pnlTabBar.Dock = DockStyle.Fill;
            pnlTabBar.Location = new Point(0, 72);
            pnlTabBar.Margin = new Padding(3, 4, 3, 4);
            pnlTabBar.Name = "pnlTabBar";
            pnlTabBar.Size = new Size(1209, 75);
            pnlTabBar.TabIndex = 0;
            // 
            // btnTabRules
            // 
            btnTabRules.BackColor = Color.FromArgb(30, 41, 59);
            btnTabRules.BaseFillColor = Color.FromArgb(30, 41, 59);
            btnTabRules.BorderColor = Color.FromArgb(100, 128, 165);
            btnTabRules.CornerRadius = 6;
            btnTabRules.FlatStyle = FlatStyle.Flat;
            btnTabRules.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnTabRules.ForeColor = Color.FromArgb(241, 245, 249);
            btnTabRules.HoverBorderColor = Color.FromArgb(100, 255, 218);
            btnTabRules.HoverFillColor = Color.FromArgb(40, 56, 80);
            btnTabRules.HoverTextColor = Color.FromArgb(100, 255, 218);
            btnTabRules.Location = new Point(65, 11);
            btnTabRules.Margin = new Padding(3, 4, 3, 4);
            btnTabRules.Name = "btnTabRules";
            btnTabRules.NormalTextColor = Color.FromArgb(241, 245, 249);
            btnTabRules.Size = new Size(258, 53);
            btnTabRules.TabIndex = 0;
            btnTabRules.Text = "🎯  Luật chơi";
            btnTabRules.UseVisualStyleBackColor = false;
            btnTabRules.Click += btnTabRules_Click;
            // 
            // btnTabShips
            // 
            btnTabShips.BackColor = Color.FromArgb(30, 41, 59);
            btnTabShips.BaseFillColor = Color.FromArgb(30, 41, 59);
            btnTabShips.BorderColor = Color.FromArgb(100, 128, 165);
            btnTabShips.CornerRadius = 6;
            btnTabShips.FlatStyle = FlatStyle.Flat;
            btnTabShips.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnTabShips.ForeColor = Color.FromArgb(241, 245, 249);
            btnTabShips.HoverBorderColor = Color.FromArgb(100, 255, 218);
            btnTabShips.HoverFillColor = Color.FromArgb(40, 56, 80);
            btnTabShips.HoverTextColor = Color.FromArgb(100, 255, 218);
            btnTabShips.Location = new Point(339, 11);
            btnTabShips.Margin = new Padding(3, 4, 3, 4);
            btnTabShips.Name = "btnTabShips";
            btnTabShips.NormalTextColor = Color.FromArgb(241, 245, 249);
            btnTabShips.Size = new Size(258, 53);
            btnTabShips.TabIndex = 1;
            btnTabShips.Text = "🚢  Thông tin hạm đội";
            btnTabShips.UseVisualStyleBackColor = false;
            btnTabShips.Click += btnTabShips_Click;
            // 
            // btnTabTactics
            // 
            btnTabTactics.BackColor = Color.FromArgb(30, 41, 59);
            btnTabTactics.BaseFillColor = Color.FromArgb(30, 41, 59);
            btnTabTactics.BorderColor = Color.FromArgb(100, 128, 165);
            btnTabTactics.CornerRadius = 6;
            btnTabTactics.FlatStyle = FlatStyle.Flat;
            btnTabTactics.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnTabTactics.ForeColor = Color.FromArgb(241, 245, 249);
            btnTabTactics.HoverBorderColor = Color.FromArgb(100, 255, 218);
            btnTabTactics.HoverFillColor = Color.FromArgb(40, 56, 80);
            btnTabTactics.HoverTextColor = Color.FromArgb(100, 255, 218);
            btnTabTactics.Location = new Point(614, 11);
            btnTabTactics.Margin = new Padding(3, 4, 3, 4);
            btnTabTactics.Name = "btnTabTactics";
            btnTabTactics.NormalTextColor = Color.FromArgb(241, 245, 249);
            btnTabTactics.Size = new Size(258, 53);
            btnTabTactics.TabIndex = 2;
            btnTabTactics.Text = "⚡  Mẹo chiến thuật";
            btnTabTactics.UseVisualStyleBackColor = false;
            btnTabTactics.Click += btnTabTactics_Click;
            // 
            // btnTabTeam
            // 
            btnTabTeam.BackColor = Color.FromArgb(30, 41, 59);
            btnTabTeam.BaseFillColor = Color.FromArgb(30, 41, 59);
            btnTabTeam.BorderColor = Color.FromArgb(100, 128, 165);
            btnTabTeam.CornerRadius = 6;
            btnTabTeam.FlatStyle = FlatStyle.Flat;
            btnTabTeam.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnTabTeam.ForeColor = Color.FromArgb(241, 245, 249);
            btnTabTeam.HoverBorderColor = Color.FromArgb(100, 255, 218);
            btnTabTeam.HoverFillColor = Color.FromArgb(40, 56, 80);
            btnTabTeam.HoverTextColor = Color.FromArgb(100, 255, 218);
            btnTabTeam.Location = new Point(888, 11);
            btnTabTeam.Margin = new Padding(3, 4, 3, 4);
            btnTabTeam.Name = "btnTabTeam";
            btnTabTeam.NormalTextColor = Color.FromArgb(241, 245, 249);
            btnTabTeam.Size = new Size(258, 53);
            btnTabTeam.TabIndex = 3;
            btnTabTeam.Text = "👥  Thành viên nhóm";
            btnTabTeam.UseVisualStyleBackColor = false;
            btnTabTeam.Click += btnTabTeam_Click;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(11, 19, 36);
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Controls.Add(picAboutLogo);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(3, 4, 3, 4);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1209, 72);
            pnlHeader.TabIndex = 1;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(30, 41, 59);
            btnClose.BaseFillColor = Color.FromArgb(30, 41, 59);
            btnClose.BorderColor = Color.FromArgb(239, 68, 68);
            btnClose.CornerRadius = 6;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnClose.ForeColor = Color.FromArgb(241, 245, 249);
            btnClose.HoverBorderColor = Color.FromArgb(100, 255, 218);
            btnClose.HoverFillColor = Color.FromArgb(220, 38, 38);
            btnClose.HoverTextColor = Color.White;
            btnClose.Location = new Point(1063, 12);
            btnClose.Margin = new Padding(3, 4, 3, 4);
            btnClose.Name = "btnClose";
            btnClose.NormalTextColor = Color.FromArgb(248, 113, 113);
            btnClose.Size = new Size(121, 48);
            btnClose.TabIndex = 0;
            btnClose.Text = "✕  Đóng";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.FromArgb(100, 255, 218);
            lblHeaderTitle.Location = new Point(263, 15);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(686, 43);
            lblHeaderTitle.TabIndex = 1;
            lblHeaderTitle.Text = "📖 Hướng dẫn tác chiến hải quân";
            lblHeaderTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picAboutLogo
            // 
            picAboutLogo.BackColor = Color.Transparent;
            picAboutLogo.Location = new Point(18, 7);
            picAboutLogo.Margin = new Padding(3, 4, 3, 4);
            picAboutLogo.Name = "picAboutLogo";
            picAboutLogo.Size = new Size(234, 59);
            picAboutLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picAboutLogo.TabIndex = 2;
            picAboutLogo.TabStop = false;
            // 
            // pnlContentContainer
            // 
            pnlContentContainer.BackColor = Color.FromArgb(30, 41, 59);
            pnlContentContainer.Controls.Add(pnlRulesView);
            pnlContentContainer.Controls.Add(pnlShipsView);
            pnlContentContainer.Controls.Add(pnlTacticsView);
            pnlContentContainer.Controls.Add(pnlTeamView);
            pnlContentContainer.Dock = DockStyle.Fill;
            pnlContentContainer.Location = new Point(0, 0);
            pnlContentContainer.Margin = new Padding(3, 4, 3, 4);
            pnlContentContainer.Name = "pnlContentContainer";
            pnlContentContainer.Padding = new Padding(18, 21, 18, 21);
            pnlContentContainer.Size = new Size(1211, 960);
            pnlContentContainer.TabIndex = 1;
            // 
            // pnlRulesView
            // 
            pnlRulesView.AutoScroll = true;
            pnlRulesView.BackColor = Color.FromArgb(30, 41, 59);
            pnlRulesView.Dock = DockStyle.Fill;
            pnlRulesView.Location = new Point(18, 21);
            pnlRulesView.Margin = new Padding(3, 4, 3, 4);
            pnlRulesView.Name = "pnlRulesView";
            pnlRulesView.Size = new Size(1175, 918);
            pnlRulesView.TabIndex = 0;
            pnlRulesView.Paint += pnlRulesView_Paint;
            // 
            // pnlShipsView
            // 
            pnlShipsView.AutoScroll = true;
            pnlShipsView.BackColor = Color.FromArgb(30, 41, 59);
            pnlShipsView.Dock = DockStyle.Fill;
            pnlShipsView.Location = new Point(18, 21);
            pnlShipsView.Margin = new Padding(3, 4, 3, 4);
            pnlShipsView.Name = "pnlShipsView";
            pnlShipsView.Size = new Size(1175, 918);
            pnlShipsView.TabIndex = 1;
            pnlShipsView.Visible = false;
            pnlShipsView.Paint += pnlShipsView_Paint;
            // 
            // pnlTacticsView
            // 
            pnlTacticsView.AutoScroll = true;
            pnlTacticsView.BackColor = Color.FromArgb(30, 41, 59);
            pnlTacticsView.Dock = DockStyle.Fill;
            pnlTacticsView.Location = new Point(18, 21);
            pnlTacticsView.Margin = new Padding(3, 4, 3, 4);
            pnlTacticsView.Name = "pnlTacticsView";
            pnlTacticsView.Size = new Size(1175, 918);
            pnlTacticsView.TabIndex = 2;
            pnlTacticsView.Visible = false;
            pnlTacticsView.Paint += pnlTacticsView_Paint;
            // 
            // pnlTeamView
            // 
            pnlTeamView.AutoScroll = true;
            pnlTeamView.BackColor = Color.FromArgb(30, 41, 59);
            pnlTeamView.Dock = DockStyle.Fill;
            pnlTeamView.Location = new Point(18, 21);
            pnlTeamView.Margin = new Padding(3, 4, 3, 4);
            pnlTeamView.Name = "pnlTeamView";
            pnlTeamView.Size = new Size(1175, 918);
            pnlTeamView.TabIndex = 3;
            pnlTeamView.Visible = false;
            pnlTeamView.Paint += pnlTeamView_Paint;
            // 
            // frmAbout
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(15, 23, 42);
            ClientSize = new Size(1211, 960);
            Controls.Add(pnlContentContainer);
            Controls.Add(pnlTopContainer);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimumSize = new Size(1140, 891);
            Name = "frmAbout";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Hướng dẫn tác chiến hải quân";
            pnlTopContainer.ResumeLayout(false);
            pnlTabBar.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picAboutLogo).EndInit();
            pnlContentContainer.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
