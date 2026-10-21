using System;
using System.Drawing;
using System.Windows.Forms;
using BattleshipGame.DataAccess;
using BattleshipGame.Logic;

namespace BattleshipGame.Views
{
    /// <summary>
    /// Form Menu chính (frmMenu) do Thành viên 2 (Frontend & UI/UX Lead) phụ trách.
    /// Nhiệm vụ Ngày 1: Thiết kế giao diện Menu chính với các nút:
    /// Chơi với Máy (PvE), Hai người chơi (PvP), Bảng thành tích, Hướng dẫn, Thoát.
    /// </summary>
    public partial class frmMenu : Form
    {
        private bool _amThanhBat = true;

        public frmMenu()
        {
            InitializeComponent();
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bắn Tàu - Tactical Naval Warfare";
            CapNhatNutAmThanh();

            Shown += (s, e) =>
            {
                BringToFront();
                Activate();
                CanChinhGiaoDien();
            };
        }

        private void frmMenu_Resize(object sender, EventArgs e)
        {
            CanChinhGiaoDien();
        }

        private void CanChinhGiaoDien()
        {
            if (pnlButtonContainer == null) return;

            // Nút âm thanh neo ở góc phải trên
            btnSoundToggle.Left = ClientSize.Width - btnSoundToggle.Width - 20;
            btnSoundToggle.Top = 16;

            // Canh giữa SonarScanner
            sonarScanner.Left = (ClientSize.Width - sonarScanner.Width) / 2;
            int availableHeight = ClientSize.Height - 650;
            sonarScanner.Top = Math.Max(15, 20 + availableHeight / 6);

            // Canh giữa Title
            lblGameTitle.Width = ClientSize.Width - 40;
            lblGameTitle.Left = 20;
            lblGameTitle.Top = sonarScanner.Bottom + 10;

            // Canh giữa Subtitle
            lblGameSubtitle.Width = ClientSize.Width - 40;
            lblGameSubtitle.Left = 20;
            lblGameSubtitle.Top = lblGameTitle.Bottom + 5;

            // Canh giữa cụm nút chức năng
            pnlButtonContainer.Left = (ClientSize.Width - pnlButtonContainer.Width) / 2;
            pnlButtonContainer.Top = lblGameSubtitle.Bottom + Math.Max(15, 20 + availableHeight / 6);

            // Canh giữa panel chọn độ khó modal
            pnlDifficultySelection.Left = (ClientSize.Width - pnlDifficultySelection.Width) / 2;
            pnlDifficultySelection.Top = (ClientSize.Height - pnlDifficultySelection.Height) / 2;

            // Footer neo ở đáy và canh giữa
            lblFooter.Width = ClientSize.Width - 40;
            lblFooter.Left = 20;
            lblFooter.Top = ClientSize.Height - 35;
        }

        private void CapNhatNutAmThanh()
        {
            bool bat = cAmThanh.DaBatAmThanh;
            btnSoundToggle.Text = bat ? "🔊 Âm thanh: BẬT" : "🔇 Âm thanh: TẮT";
            btnSoundToggle.ForeColor = bat ? UITheme.AccentNeon : UITheme.MissGray;
            btnSoundToggle.BorderColor = bat ? UITheme.AccentNeon : UITheme.BorderDefault;
        }

        private void btnSoundToggle_Click(object sender, EventArgs e)
        {
            cAmThanh.BatTatAmThanh();
            CapNhatNutAmThanh();
        }

        private void btnPvE_Click(object sender, EventArgs e)
        {
            // Hiển thị panel chọn độ khó AI
            pnlDifficultySelection.Visible = true;
            pnlDifficultySelection.BringToFront();
        }

        private void btnStartPvEEasy_Click(object sender, EventArgs e)
        {
            pnlDifficultySelection.Visible = false;
            MoManHinhTranDau("PvE - Dễ");
        }

        private void btnStartPvEHard_Click(object sender, EventArgs e)
        {
            pnlDifficultySelection.Visible = false;
            MoManHinhTranDau("PvE - Nâng cao");
        }

        private void btnCloseDifficulty_Click(object sender, EventArgs e)
        {
            pnlDifficultySelection.Visible = false;
        }

        private void btnPvP_Click(object sender, EventArgs e)
        {
            MoManHinhTranDau("PvP Hotseat");
        }

        private void btnCustom_Click(object sender, EventArgs e)
        {
            MoManHinhTranDau("Custom");
        }

        private void btnLeaderboard_Click(object sender, EventArgs e)
        {
            try
            {
                using (var f = new frmThanhTich())
                {
                    f.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể mở Bảng thành tích: " + ex.Message,
                    "Bảng Xếp Hạng",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            using (var f = new frmAbout())
            {
                f.ShowDialog(this);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show(
                "Đồng chí Đô đốc có chắc chắn muốn rời khỏi sở chỉ huy và thoát trò chơi?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (res == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void MoManHinhTranDau(string cheDo)
        {
            Hide();
            using (var f = new frmBanTau(cheDo))
            {
                f.ShowDialog(this);
            }
            Show();
            CapNhatNutAmThanh();
        }
    }
}
