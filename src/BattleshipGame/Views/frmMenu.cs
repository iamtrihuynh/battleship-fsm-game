#nullable enable
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BattleshipGame.DataAccess;
using BattleshipGame.Logic;

namespace BattleshipGame.Views
{
    public partial class frmMenu : Form
    {

        private string _tenNguoiChoi = "Người chơi";
        private bool _heThongBiKhoa = false;

        public frmMenu()
        {
            InitializeComponent();
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bắn Tàu";
            CapNhatNutAmThanh();
            CapNhatHienThiTenNguoiChoi();

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
                cAmThanh.DungSFX();

                if (!KiemTraThuMucVaFileQuanTrong(out string thongBaoLoi))
                {
                    TacticalMessageBox.Show(
                        this,
                        "LỖI HỆ THỐNG - THIẾU TỆP/THƯ MỤC QUAN TRỌNG",
                        thongBaoLoi,
                        TacticalDialogType.Warning,
                        MessageBoxButtons.OK
                    );
                    KhoaGiaoDienKhongChoVaoGame();
                    return;
                }

                bool amThanhSanSang = KiemTraTaiNguyenVaCanhBao();

                if (amThanhSanSang)
                {
                    cAmThanh.PhatNhacNen("bgm_battle.wav");
                }
            };
        }

        private void frmMenu_Resize(object sender, EventArgs e)
        {
            CanChinhGiaoDien();
        }

        private void CanChinhGiaoDien()
        {
            if (pnlButtonContainer == null) return;

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

            if (lblCurrentPlayer != null && btnChangeName != null)
            {
                lblCurrentPlayer.Location = new Point(24, 16);
                btnChangeName.Location = new Point(24, lblCurrentPlayer.Bottom + 14);
            }

            sonarScanner.Left = (ClientSize.Width - sonarScanner.Width) / 2;
            int availableHeight = ClientSize.Height - 650;
            sonarScanner.Top = Math.Max(15, 20 + availableHeight / 6);

            lblGameTitle.Width = ClientSize.Width - 40;
            lblGameTitle.Left = 20;
            lblGameTitle.Top = sonarScanner.Bottom + 10;

            lblGameSubtitle.Width = ClientSize.Width - 40;
            lblGameSubtitle.Left = 20;
            lblGameSubtitle.Top = lblGameTitle.Bottom + 5;

            pnlButtonContainer.Left = (ClientSize.Width - pnlButtonContainer.Width) / 2;
            pnlButtonContainer.Top = lblGameSubtitle.Bottom + Math.Max(15, 20 + availableHeight / 6);

            pnlDifficultySelection.Left = (ClientSize.Width - pnlDifficultySelection.Width) / 2;
            pnlDifficultySelection.Top = (ClientSize.Height - pnlDifficultySelection.Height) / 2;

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
            if (_heThongBiKhoa || !KiemTraThuMucVaFileQuanTrong(out string thongBaoLoi))
            {
                TacticalMessageBox.Show(
                    this,
                    "KHÔNG CHO PHÉP VÀO GAME",
                    "Không tìm thấy thư mục/tệp tin quan trọng (Logic,...). Trò chơi không thể khởi động!",
                    TacticalDialogType.Warning,
                    MessageBoxButtons.OK
                );
                KhoaGiaoDienKhongChoVaoGame();
                return;
            }

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
            MoManHinhTranDau("PvE - Khó", _tenNguoiChoi);
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
            if (_heThongBiKhoa || !KiemTraThuMucVaFileQuanTrong(out string thongBaoLoi))
            {
                TacticalMessageBox.Show(
                    this,
                    "KHÔNG CHO PHÉP VÀO GAME",
                    "Không tìm thấy thư mục/tệp tin quan trọng (Logic,...). Trò chơi không cho phép vào game!",
                    TacticalDialogType.Warning,
                    MessageBoxButtons.OK
                );
                KhoaGiaoDienKhongChoVaoGame();
                return;
            }

            try
            {
                Hide();
                using (var f = new frmBanTau(cheDo, tenNguoiChoi))
                {
                    f.ShowDialog(this);
                }
                Show();
                cAmThanh.DungSFX();
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

        private bool KiemTraTaiNguyenVaCanhBao()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(
                    Path.DirectorySeparatorChar, 
                    Path.AltDirectorySeparatorChar
                );

                string? repoRoot = TimThuMucGocRepo(baseDir);

                bool coBg = KiemTraThuMucAudio(baseDir, repoRoot);
                bool coImg = KiemTraThuMucHinhAnh(baseDir, repoRoot);

                if (!coBg && !coImg)
                {
                    cAmThanh.ThuMucAudio = "";
                    cAmThanh.DungNhacNen();
                    TacticalMessageBox.ShowWarning(
                        this,
                        "Bẫy lỗi tài nguyên (bg/ & img/)",
                        "Không tìm thấy cả thư mục âm thanh bg/ và thư mục hình ảnh img/!\n\nHệ thống đã kích hoạt cơ chế bẫy lỗi (try...catch) an toàn:\n• Âm thanh: Tự động chuyển sang âm thanh dự phòng của hệ điều hành.\n• Giao diện: Tự động chuyển sang giao diện đồ họa vector dự phòng."
                    );
                    return false;
                }
                else if (!coBg)
                {
                    cAmThanh.ThuMucAudio = "";
                    cAmThanh.DungNhacNen();
                    TacticalMessageBox.ShowWarning(
                        this,
                        "Bẫy lỗi tài nguyên (bg/)",
                        "Không tìm thấy thư mục âm thanh bg/! Hệ thống tự động kích hoạt bẫy lỗi và chuyển sang âm thanh dự phòng của hệ điều hành."
                    );
                    return false;
                }
                else if (!coImg)
                {
                    TacticalMessageBox.ShowWarning(
                        this,
                        "Bẫy lỗi tài nguyên (img/)",
                        "Không tìm thấy thư mục hình ảnh img/! Hệ thống kích hoạt cơ chế bẫy lỗi và chuyển sang giao diện đồ họa dự phòng an toàn."
                    );
                    return true;
                }

                return true;
            }
            catch (Exception ex)
            {
                TacticalMessageBox.ShowWarning(
                    this,
                    "Bẫy lỗi tài nguyên",
                    "Đã xảy ra sự cố khi kiểm tra tài nguyên: " + ex.Message
                );
                return false;
            }
        }

        private static string? TimThuMucGocRepo(string baseDir)
        {
            try
            {
                string p = baseDir;
                for (int i = 0; i < 6; i++)
                {
                    if (File.Exists(Path.Combine(p, "BattleshipGame.sln")) ||
                        Directory.Exists(Path.Combine(p, ".git")))
                    {
                        return p;
                    }
                    string? parent = Path.GetDirectoryName(p);
                    if (string.IsNullOrEmpty(parent) || parent == p) break;
                    p = parent;
                }
            }
            catch { }
            return null;
        }

        private static bool KiemTraThuMucAudio(string baseDir, string? repoRoot)
        {
            try
            {
                string localBg = Path.Combine(baseDir, "bg");
                bool localExists = Directory.Exists(localBg) && CoTepTin(localBg, "*.wav");

                if (!string.IsNullOrEmpty(repoRoot))
                {
                    string rootBg = Path.Combine(repoRoot, "bg");
                    bool rootExists = Directory.Exists(rootBg) && CoTepTin(rootBg, "*.wav");

                    if (!rootExists || !localExists)
                    {
                        return false;
                    }

                    cAmThanh.ThuMucAudio = localBg;
                    return true;
                }

                if (localExists)
                {
                    cAmThanh.ThuMucAudio = localBg;
                    return true;
                }
            }
            catch { }

            return false;
        }

        private static bool KiemTraThuMucHinhAnh(string baseDir, string? repoRoot)
        {
            try
            {
                string localImg = Path.Combine(baseDir, "img");
                bool localExists = Directory.Exists(localImg) && CoTepTin(localImg, "*.*");

                if (!string.IsNullOrEmpty(repoRoot))
                {
                    string rootImg = Path.Combine(repoRoot, "img");
                    bool rootExists = Directory.Exists(rootImg) && CoTepTin(rootImg, "*.*");

                    if (!rootExists || !localExists)
                    {
                        return false;
                    }

                    return true;
                }

                if (localExists)
                {
                    return true;
                }
            }
            catch { }

            return false;
        }

        private static bool CoTepTin(string dir, string searchPattern)
        {
            try
            {
                if (!Directory.Exists(dir)) return false;
                string[] files = Directory.GetFiles(dir, searchPattern);
                return files != null && files.Length > 0;
            }
            catch
            {
                return false;
            }
        }

        private void KhoaGiaoDienKhongChoVaoGame()
        {
            _heThongBiKhoa = true;
            btnPvE.Enabled = false;
            btnStartPvEEasy.Enabled = false;
            btnStartPvEHard.Enabled = false;
            pnlDifficultySelection.Visible = false;
            lblGameSubtitle.Text = "⛔ ĐÃ KHÓA: Thiếu thư mục/tệp quan trọng (Logic,...) - Không thể vào game!";
            lblGameSubtitle.ForeColor = UITheme.DangerRed;
            btnPvE.Text = "🔒 Đã khóa (Thiếu file/folder)";
            btnPvE.NormalTextColor = UITheme.MissGray;
            btnPvE.BorderColor = UITheme.DangerRed;
            btnPvE.Invalidate();
        }

        private static string? TimThuMucDuAn(string baseDir)
        {
            try
            {
                string p = baseDir;
                for (int i = 0; i < 6; i++)
                {
                    if (File.Exists(Path.Combine(p, "BattleshipGame.csproj")))
                    {
                        return p;
                    }
                    string? parent = Path.GetDirectoryName(p);
                    if (string.IsNullOrEmpty(parent) || parent == p) break;
                    p = parent;
                }
            }
            catch { }
            return null;
        }

        private static bool KiemTraThuMucVaFileQuanTrong(out string thongBaoLoi, string? customBaseDir = null)
        {
            thongBaoLoi = "";
            try
            {
                string baseDir = (customBaseDir ?? AppDomain.CurrentDomain.BaseDirectory).TrimEnd(
                    Path.DirectorySeparatorChar, 
                    Path.AltDirectorySeparatorChar
                );

                string? repoRoot = TimThuMucGocRepo(baseDir);
                string? projectDir = TimThuMucDuAn(baseDir);

                var thieu = new System.Collections.Generic.List<string>();

                string[] dsThuMucQuanTrong = new string[] { "Logic", "Models", "DataAccess", "Views" };

                foreach (string folderName in dsThuMucQuanTrong)
                {
                    bool coThuMuc = false;

                    if (!string.IsNullOrEmpty(projectDir))
                    {
                        string pathProj = Path.Combine(projectDir, folderName);
                        if (Directory.Exists(pathProj) && CoTepTin(pathProj, "*.cs"))
                        {
                            coThuMuc = true;
                        }
                    }
                    else
                    {
                        string pathBase = Path.Combine(baseDir, folderName);
                        if (Directory.Exists(pathBase))
                        {
                            coThuMuc = true;
                        }
                    }

                    if (!coThuMuc)
                    {
                        thieu.Add($"• Thư mục quan trọng: {folderName}/");
                    }
                }

                bool coFileThanhTich = false;
                if (File.Exists(Path.Combine(baseDir, "thanhtichs.txt")))
                {
                    coFileThanhTich = true;
                }
                else if (!string.IsNullOrEmpty(repoRoot) && File.Exists(Path.Combine(repoRoot, "thanhtichs.txt")))
                {
                    coFileThanhTich = true;
                }
                else if (!string.IsNullOrEmpty(projectDir) && File.Exists(Path.Combine(projectDir, "thanhtichs.txt")))
                {
                    coFileThanhTich = true;
                }

                if (!coFileThanhTich)
                {
                    thieu.Add("• Tệp tin dữ liệu: thanhtichs.txt");
                }

                string dllPath = Path.Combine(baseDir, "BattleshipGame.dll");
                if (!File.Exists(dllPath))
                {
                    thieu.Add("• Tệp tin thư viện: BattleshipGame.dll");
                }

                if (thieu.Count > 0)
                {
                    var sb = new System.Text.StringBuilder();
                    sb.AppendLine("Hệ thống phát hiện thiếu các tệp tin / thư mục quan trọng để vận hành trò chơi:\n");
                    foreach (string item in thieu)
                    {
                        sb.AppendLine(item);
                    }
                    sb.AppendLine("\n⛔ Để đảm bảo an toàn, trò chơi KHÔNG CHO PHÉP VÀO GAME.");
                    sb.AppendLine("Vui lòng khôi phục các tệp tin/thư mục trên trước khi khởi động lại!");
                    thongBaoLoi = sb.ToString();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                thongBaoLoi = "Lỗi khi kiểm tra tính toàn vẹn hệ thống: " + ex.Message;
                return false;
            }
        }
    }
}
