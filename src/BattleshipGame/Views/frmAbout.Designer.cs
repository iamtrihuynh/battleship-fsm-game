using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
    partial class frmAbout
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblHeaderTitle;
        private TacticalButton btnClose;
        private TabControl tabAbout;
        private TabPage tabRules;
        private TabPage tabShips;
        private TabPage tabTactics;
        private TabPage tabTeam;

        private RichTextBox rtbRules;
        private RichTextBox rtbShips;
        private RichTextBox rtbTactics;
        private RichTextBox rtbTeam;

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
            this.lblHeaderTitle = new Label();
            this.btnClose = new TacticalButton();

            this.tabAbout = new TabControl();
            this.tabRules = new TabPage();
            this.tabShips = new TabPage();
            this.tabTactics = new TabPage();
            this.tabTeam = new TabPage();

            this.rtbRules = new RichTextBox();
            this.rtbShips = new RichTextBox();
            this.rtbTactics = new RichTextBox();
            this.rtbTeam = new RichTextBox();

            this.SuspendLayout();

            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = UITheme.BackgroundDark;
            this.ClientSize = new Size(820, 580);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmAbout";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Hướng Dẫn & Giới Thiệu - Bắn Tàu";

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = UITheme.HeaderBackground;
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 60;
            this.pnlHeader.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(this.pnlHeader);

            this.lblHeaderTitle.AutoSize = false;
            this.lblHeaderTitle.Location = new Point(20, 14);
            this.lblHeaderTitle.Size = new Size(600, 32);
            this.lblHeaderTitle.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = UITheme.AccentNeon;
            this.lblHeaderTitle.Text = "📖 HƯỚNG DẪN CHIẾN THUẬT & THÔNG TIN DỰ ÁN";
            this.pnlHeader.Controls.Add(this.lblHeaderTitle);

            this.btnClose.Location = new Point(680, 12);
            this.btnClose.Size = new Size(120, 36);
            this.btnClose.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            this.btnClose.Text = "Đóng";
            this.btnClose.CornerRadius = 6;
            this.btnClose.Click += (s, e) => this.Close();
            this.pnlHeader.Controls.Add(this.btnClose);

            // 
            // tabAbout
            // 
            this.tabAbout.Location = new Point(16, 76);
            this.tabAbout.Size = new Size(788, 484);
            this.tabAbout.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            this.Controls.Add(this.tabAbout);

            this.tabAbout.Controls.Add(this.tabRules);
            this.tabAbout.Controls.Add(this.tabShips);
            this.tabAbout.Controls.Add(this.tabTactics);
            this.tabAbout.Controls.Add(this.tabTeam);

            // 
            // tabRules
            // 
            this.tabRules.BackColor = UITheme.PanelBackground;
            this.tabRules.Text = "  Luật Chơi  ";
            this.rtbRules.Dock = DockStyle.Fill;
            this.rtbRules.ReadOnly = true;
            this.rtbRules.BackColor = UITheme.PanelBackground;
            this.rtbRules.ForeColor = UITheme.TextPrimary;
            this.rtbRules.Font = new Font("Segoe UI", 11f, FontStyle.Regular);
            this.rtbRules.BorderStyle = BorderStyle.None;
            this.tabRules.Controls.Add(this.rtbRules);

            // 
            // tabShips
            // 
            this.tabShips.BackColor = UITheme.PanelBackground;
            this.tabShips.Text = "  Thông Số Hạm Đội  ";
            this.rtbShips.Dock = DockStyle.Fill;
            this.rtbShips.ReadOnly = true;
            this.rtbShips.BackColor = UITheme.PanelBackground;
            this.rtbShips.ForeColor = UITheme.TextPrimary;
            this.rtbShips.Font = new Font("Segoe UI", 11f, FontStyle.Regular);
            this.rtbShips.BorderStyle = BorderStyle.None;
            this.tabShips.Controls.Add(this.rtbShips);

            // 
            // tabTactics
            // 
            this.tabTactics.BackColor = UITheme.PanelBackground;
            this.tabTactics.Text = "  Mẹo Chiến Thuật  ";
            this.rtbTactics.Dock = DockStyle.Fill;
            this.rtbTactics.ReadOnly = true;
            this.rtbTactics.BackColor = UITheme.PanelBackground;
            this.rtbTactics.ForeColor = UITheme.TextPrimary;
            this.rtbTactics.Font = new Font("Segoe UI", 11f, FontStyle.Regular);
            this.rtbTactics.BorderStyle = BorderStyle.None;
            this.tabTactics.Controls.Add(this.rtbTactics);

            // 
            // tabTeam
            // 
            this.tabTeam.BackColor = UITheme.PanelBackground;
            this.tabTeam.Text = "  Thành Viên Nhóm  ";
            this.rtbTeam.Dock = DockStyle.Fill;
            this.rtbTeam.ReadOnly = true;
            this.rtbTeam.BackColor = UITheme.PanelBackground;
            this.rtbTeam.ForeColor = UITheme.TextPrimary;
            this.rtbTeam.Font = new Font("Segoe UI", 11f, FontStyle.Regular);
            this.rtbTeam.BorderStyle = BorderStyle.None;
            this.tabTeam.Controls.Add(this.rtbTeam);

            this.ResumeLayout(false);
        }
    }
}
