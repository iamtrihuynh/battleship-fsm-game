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

        private string _tenNguoiChoi = "Người chơi";

        public frmMenu()
        {
            InitializeComponent();
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bắn Tàu";
            CapNhatNutAmThanh();
            CapNhatHienThiTenNguoiChoi();

            // Đăng ký nhận sự kiện thay đổi âm lượng toàn cục
            TacticalVolumeSlider.GlobalVolumeChanged += (s, e) =>
            {
                if (!IsDisposed && IsHandleCreated)
                {
                    BeginInvoke(new Action(CapNhatNutAmThanh));
                }
            };

            Shown += (s, e) =>
            {
                BringToFront();
                Activate();
                CanChinhGiaoDien();
                cAmThanh.PhatNhacNen("bgm_battle.wav");
            };
        }

        private void frmMenu_Resize(object sender, EventArgs e)
        {
            CanChinhGiaoDien();
        }

        private void CanChinhGiaoDien()
        {
            if (pnlButtonContainer == null) return;

            // Cụm điều khiển âm lượng neo ở góc phải trên
            if (sliderVolume != null)
            {
                sliderVolume.Left = ClientSize.Width - sliderVolume.Width - 20;
                sliderVolume.Top = 52;
                btnSoundToggle.Left = ClientSize.Width - btnSoundToggle.Width - 20;
                btnSoundToggle.Top = 14;
            }
            else
            {
                btnSoundToggle.Left = ClientSize.Width - btnSoundToggle.Width - 20;
                btnSoundToggle.Top = 16;
            }

            // Neo cụm người chơi ở góc trái trên
            if (lblCurrentPlayer != null && btnChangeName != null)
            {
                lblCurrentPlayer.Location = new Point(24, 16);
                btnChangeName.Location = new Point(24, lblCurrentPlayer.Bottom + 14);
            }

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
            try
            {
                bool bat = sliderVolume != null ? (!sliderVolume.IsMuted && sliderVolume.Value > 0) : cAmThanh.DaBatAmThanh;
                btnSoundToggle.Text = bat ? "🔊 Âm thanh: Bật" : "🔇 Âm thanh: Tắt";
                btnSoundToggle.ForeColor = bat ? UITheme.AccentNeon : UITheme.MissGray;
                btnSoundToggle.BorderColor = bat ? UITheme.AccentNeon : UITheme.BorderDefault;
                btnSoundToggle.Invalidate();
            }
            catch { }
        }

        private void btnSoundToggle_Click(object sender, EventArgs e)
        {
            try
            {
                TacticalVolumeSlider.ToggleGlobalMute(this);
                CapNhatNutAmThanh();
            }
            catch { }
        }

        private void sliderVolume_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CapNhatNutAmThanh();
            }
            catch { }
        }

        private void CapNhatHienThiTenNguoiChoi()
        {
            if (lblCurrentPlayer != null)
            {
                lblCurrentPlayer.Text = $"👤 Người chơi: {_tenNguoiChoi}";
            }
        }

        private void btnChangeName_Click(object sender, EventArgs e)
        {
            using (Form prompt = new Form())
            {
                prompt.Width = 380;
                prompt.Height = 180;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.Text = "Đổi tên";
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.BackColor = UITheme.BackgroundDark;
                prompt.ForeColor = Color.White;
                prompt.MaximizeBox = false;
                prompt.MinimizeBox = false;

                Label lblPrompt = new Label()
                {
                    Left = 24,
                    Top = 18,
                    Width = 330,
                    Height = 22,
                    Text = "Nhập tên mới:",
                    ForeColor = UITheme.PrimaryCyan,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
                };

                TextBox txtInput = new TextBox()
                {
                    Left = 24,
                    Top = 46,
                    Width = 320,
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
                    Left = 130,
                    Width = 100,
                    Top = 90,
                    Height = 36,
                    BorderColor = UITheme.PrimaryCyan,
                    DialogResult = DialogResult.OK
                };

                TacticalButton btnHuy = new TacticalButton()
                {
                    Text = "Hủy",
                    Left = 244,
                    Width = 100,
                    Top = 90,
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
                TacticalMessageBox.ShowWarning(
                    this,
                    "Lỗi bảng xếp hạng",
                    "Không thể mở bảng xếp hạng: " + ex.Message
                );
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            try
            {
                using (var f = new frmAbout())
                {
                    f.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                TacticalMessageBox.ShowWarning(
                    this,
                    "Lỗi hướng dẫn",
                    "Không thể mở hướng dẫn: " + ex.Message
                );
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                var res = TacticalMessageBox.ShowExitConfirm(this);
                if (res == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            catch
            {
                Application.Exit();
            }
        }

        private void MoManHinhTranDau(string cheDo, string tenNguoiChoi = "Người chơi")
        {
            try
            {
                Hide();
                using (var f = new frmBanTau(cheDo, tenNguoiChoi))
                {
                    f.ShowDialog(this);
                }
                Show();
                cAmThanh.PhatNhacNen("bgm_battle.wav");
                CapNhatNutAmThanh();
            }
            catch (Exception ex)
            {
                Show();
                TacticalMessageBox.ShowWarning(
                    this,
                    "Lỗi khởi động",
                    "Đã xảy ra sự cố khi tải trận đấu: " + ex.Message
                );
            }
        }
    }
}
