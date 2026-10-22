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

        private string _tenNguoiChoi = "Thuyền Trưởng";

        public frmMenu()
        {
            InitializeComponent();
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bắn Tàu - Tactical Naval Warfare";
            CapNhatNutAmThanh();
            CapNhatHienThiTenNguoiChoi();

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

        private void CapNhatHienThiTenNguoiChoi()
        {
            if (lblCurrentPlayer != null)
            {
                lblCurrentPlayer.Text = $"👤 Đô đốc: {_tenNguoiChoi}";
            }
        }

        private void btnChangeName_Click(object sender, EventArgs e)
        {
            using (Form prompt = new Form())
            {
                prompt.Width = 400;
                prompt.Height = 190;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.Text = "Đổi Tên Đô Đốc";
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.BackColor = UITheme.BackgroundDark;
                prompt.ForeColor = Color.White;
                prompt.MaximizeBox = false;
                prompt.MinimizeBox = false;

                Label lblPrompt = new Label()
                {
                    Left = 24,
                    Top = 18,
                    Width = 340,
                    Height = 22,
                    Text = "Nhập tên Đô đốc mới (dùng cho chế độ Đấu Máy):",
                    ForeColor = UITheme.PrimaryCyan,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
                };

                TextBox txtInput = new TextBox()
                {
                    Left = 24,
                    Top = 48,
                    Width = 336,
                    Text = _tenNguoiChoi,
                    MaxLength = 30,
                    BackColor = Color.FromArgb(15, 23, 42),
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new Font("Segoe UI", 11f, FontStyle.Bold)
                };

                TacticalButton btnXacNhan = new TacticalButton()
                {
                    Text = "Xác nhận",
                    Left = 140,
                    Width = 105,
                    Top = 95,
                    Height = 36,
                    BorderColor = UITheme.PrimaryCyan,
                    DialogResult = DialogResult.OK
                };

                TacticalButton btnHuy = new TacticalButton()
                {
                    Text = "Hủy",
                    Left = 255,
                    Width = 105,
                    Top = 95,
                    Height = 36,
                    BorderColor = UITheme.BorderLight,
                    DialogResult = DialogResult.Cancel
                };

                prompt.Controls.Add(lblPrompt);
                prompt.Controls.Add(txtInput);
                prompt.Controls.Add(btnXacNhan);
                prompt.Controls.Add(btnHuy);
                prompt.AcceptButton = btnXacNhan;
                prompt.CancelButton = btnHuy;

                prompt.Shown += (s, ev) =>
                {
                    txtInput.Focus();
                    txtInput.SelectAll();
                };

                if (prompt.ShowDialog(this) == DialogResult.OK)
                {
                    if (!string.IsNullOrWhiteSpace(txtInput.Text))
                    {
                        _tenNguoiChoi = txtInput.Text.Trim();
                        CapNhatHienThiTenNguoiChoi();
                    }
                }
            }
        }

        private void btnPvE_Click(object sender, EventArgs e)
        {
            // Hiển thị panel chọn độ khó AI
            lblDifficultyDesc.Text = $"Đô đốc hiện tại: {_tenNguoiChoi}\nHãy chọn cấp độ để sẵn sàng vào trận!";
            pnlDifficultySelection.Visible = true;
            pnlDifficultySelection.BringToFront();
        }

        private void btnStartPvEEasy_Click(object sender, EventArgs e)
        {
            pnlDifficultySelection.Visible = false;
            MoManHinhTranDau("PvE - Dễ", _tenNguoiChoi);
        }

        private void btnStartPvEHard_Click(object sender, EventArgs e)
        {
            pnlDifficultySelection.Visible = false;
            MoManHinhTranDau("PvE - Nâng cao", _tenNguoiChoi);
        }

        private void btnCloseDifficulty_Click(object sender, EventArgs e)
        {
            pnlDifficultySelection.Visible = false;
        }

        private void btnPvP_Click(object sender, EventArgs e)
        {
            MoManHinhTranDau("PvP Hotseat");
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

        private void MoManHinhTranDau(string cheDo, string tenNguoiChoi = "Thuyền Trưởng")
        {
            Hide();
            using (var f = new frmBanTau(cheDo, tenNguoiChoi))
            {
                f.ShowDialog(this);
            }
            Show();
            CapNhatNutAmThanh();
        }
    }
}
