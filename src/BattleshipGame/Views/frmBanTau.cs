using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
    /// <summary>
    /// Form Trận Đấu chính (frmBanTau) do Thành viên 2 (Frontend & UI/UX Lead) phụ trách.
    /// Nhiệm vụ Ngày 1: Tạo giao diện tĩnh chuẩn kích thước, định hình vị trí đặt 2 bàn cờ 10x10,
    /// Dashboard thông số chiến trường và hộp điều khiển chiến thuật theo đúng tài liệu.
    /// </summary>
    public partial class frmBanTau : Form
    {
        private readonly string _cheDoChoi;
        private bool _huongNgang = true;
        private bool _amThanhBat = true;
        private int _matchSeconds = 0;
        private int _turnSeconds = 30;

        public frmBanTau(string cheDoChoi = "PvE")
        {
            InitializeComponent();
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Trận Địa Hải Chiến - Bắn Tàu (Bản Phác Thảo Giao Diện Ngày 1)";

            _cheDoChoi = cheDoChoi;
            KhoiTaoGiaoDienTinh();

            Shown += (s, e) =>
            {
                BringToFront();
                Activate();
                CanChinhGiaoDien();
            };
        }

        private void frmBanTau_Resize(object sender, EventArgs e)
        {
            CanChinhGiaoDien();
        }

        private void CanChinhGiaoDien()
        {
            if (pnlCenterDashboard == null || pnlLeftBoardContainer == null || pnlRightBoardContainer == null) return;

            // Neo nút âm thanh ở thanh trên cùng bên phải
            btnToggleSound.Left = pnlTopBar.Width - btnToggleSound.Width - 20;

            // Tính toán vị trí canh giữa toàn bộ 3 khu vực khi mở to / toàn màn hình
            int totalWidth = pnlLeftBoardContainer.Width + 10 + pnlCenterDashboard.Width + 10 + pnlRightBoardContainer.Width;
            int startX = Math.Max(16, (ClientSize.Width - totalWidth) / 2);

            pnlLeftBoardContainer.Left = startX;
            if (pnlLeftNote != null) pnlLeftNote.Left = startX;

            pnlCenterDashboard.Left = startX + pnlLeftBoardContainer.Width + 10;

            int rightX = pnlCenterDashboard.Right + 10;
            pnlRightBoardContainer.Left = rightX;
            if (pnlRightNote != null) pnlRightNote.Left = rightX;

            // Canh giữa màn che sương mù
            if (pnlFogOverlay != null && pnlFogOverlay.Visible)
            {
                lblFogTitle.Left = (pnlFogOverlay.Width - lblFogTitle.Width) / 2;
                lblFogMessage.Left = (pnlFogOverlay.Width - lblFogMessage.Width) / 2;
                btnFogContinue.Left = (pnlFogOverlay.Width - btnFogContinue.Width) / 2;
            }
        }

        private void KhoiTaoGiaoDienTinh()
        {
            lblMatchModeInfo.Text = _cheDoChoi switch
            {
                "PvE" => "Chế độ: Đấu với Máy (AI) • Bàn cờ quy chuẩn 10x10 • Hạm đội 5 tàu",
                "PvP" => "Chế độ: Hai người chơi chung 1 máy (Hotseat) • Cơ chế màn chắn sương mù chuyển lượt",
                _ => "Chế độ: Tùy biến / Tự do • Bàn cờ quy chuẩn 10x10"
            };

            // Nạp danh sách 5 loại tàu vào kho mẫu giao diện
            cboSelectShip.Items.Clear();
            cboSelectShip.Items.Add("Tàu sân bay (5 ô) - [Đã đặt]");
            cboSelectShip.Items.Add("Thiết giáp hạm (4 ô) - [Đã đặt]");
            cboSelectShip.Items.Add("Tàu ngầm (3 ô) - [Đã đặt]");
            cboSelectShip.Items.Add("Tàu khu trục (3 ô) - [Chưa đặt]");
            cboSelectShip.Items.Add("Tàu tuần tra (2 ô) - [Chưa đặt]");
            cboSelectShip.SelectedIndex = 0;

            CapNhatNutAmThanh();

            // Khởi động đồng hồ giao diện trực quan
            tmrMatch.Start();
            tmrTurnCountdown.Start();
        }

        private void CapNhatNutAmThanh()
        {
            btnToggleSound.Text = _amThanhBat ? "🔊 ÂM THANH: BẬT" : "🔇 ÂM THANH: TẮT";
            btnToggleSound.ForeColor = _amThanhBat ? UITheme.AccentNeon : UITheme.MissGray;
        }

        private void btnToggleSound_Click(object sender, EventArgs e)
        {
            _amThanhBat = !_amThanhBat;
            CapNhatNutAmThanh();
        }

        private void cboSelectShip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSelectShip.SelectedItem != null)
            {
                lblTurnMessage.Text = $"Đang chọn: {cboSelectShip.SelectedItem}";
            }
        }

        private void btnBackToMenu_Click(object sender, EventArgs e)
        {
            tmrMatch.Stop();
            tmrTurnCountdown.Stop();
            Close();
        }

        private void frmBanTau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R || e.KeyCode == Keys.Space)
            {
                DoiHuongTau();
                e.Handled = true;
            }
        }

        private void btnRotateShip_Click(object sender, EventArgs e)
        {
            DoiHuongTau();
        }

        private void DoiHuongTau()
        {
            _huongNgang = !_huongNgang;
            btnRotateShip.Text = $"🔄 Xoay Hướng (R / Space): {(_huongNgang ? "NGANG" : "DỌC")}";
        }

        private void btnAutoPlace_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "⚡ [Giao diện tĩnh Ngày 1 - TV2]: Nút 'Xếp tự động' đã sẵn sàng.\nThuật toán sinh tọa độ ngẫu nhiên sẽ do Thành viên 1 (Backend) tích hợp vào Ngày 2.",
                "Demo Giao diện Xếp Tàu",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnResetBoard_Click(object sender, EventArgs e)
        {
            boardPlayer.XoaBanCo();
            MessageBox.Show(
                "🗑️ [Giao diện tĩnh Ngày 1 - TV2]: Đã làm mới hiển thị bàn cờ.",
                "Đặt lại bàn cờ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnStartBattle_Click(object sender, EventArgs e)
        {
            pnlPlacementControls.Enabled = false;
            pnlCombatControls.Enabled = true;

            lblTurnIndicator.Text = "LƯỢT: BẠN KHAI HỎA";
            lblTurnIndicator.ForeColor = UITheme.AccentNeon;
            lblTurnMessage.Text = "Click ô trên Radar đối phương để khai hỏa!";

            MessageBox.Show(
                "⚔️ [Giao diện Ngày 1 - TV2]: Chuyển sang giao diện chiến đấu thành công!\nCác thuật toán nhận phát bắn và Bot AI sẽ được Thành viên 1 ghép nối vào Ngày 3.",
                "Giai đoạn Chiến đấu",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void boardPlayer_KhiClickO(int x, int y)
        {
            lblTurnMessage.Text = $"Bạn vừa click ô tọa độ bàn cờ hạm đội: {((char)('A' + y))}{x + 1}";
        }

        private void boardRadar_KhiClickO(int x, int y)
        {
            lblTurnMessage.Text = $"Mục tiêu radar đã chọn: {((char)('A' + y))}{x + 1}";
            boardRadar.DatGiaTriO(x, y, 2); // Minh họa phát bắn trúng (đỏ)
        }

        private void btnSkillRadar_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "📡 [Giao diện tĩnh Ngày 1 - TV2]: Kỹ năng Radar trinh sát 3x3 đã định hình vị trí và giao diện.",
                "Kỹ năng đặc biệt",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnSkillCrossBomb_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "💥 [Giao diện tĩnh Ngày 1 - TV2]: Kỹ năng Pháo kích diện rộng (+) 5 ô đã định hình vị trí và giao diện.",
                "Kỹ năng đặc biệt",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnPauseGame_Click(object sender, EventArgs e)
        {
            MessageBox.Show("⏸ [Giao diện tĩnh Ngày 1 - TV2]: Nút Tạm dừng trận đấu.", "Tạm dừng", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSurrender_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("Đồng chí có chắc chắn muốn đầu hàng?", "Xác nhận đầu hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                tmrMatch.Stop();
                tmrTurnCountdown.Stop();
                Close();
            }
        }

        private void btnFogContinue_Click(object sender, EventArgs e)
        {
            pnlFogOverlay.Visible = false;
        }

        private void tmrMatch_Tick(object sender, EventArgs e)
        {
            _matchSeconds++;
            int m = _matchSeconds / 60;
            int s = _matchSeconds % 60;
            lblMatchTimer.Text = $"⏱ Trận: {m:D2}:{s:D2}";
        }

        private void tmrTurnCountdown_Tick(object sender, EventArgs e)
        {
            if (_turnSeconds > 0)
            {
                _turnSeconds--;
                lblTurnTimer.Text = $"⏳ Lượt: {_turnSeconds}s";
                prgTurnTime.Value = Math.Max(0, Math.Min(30, _turnSeconds));
                lblTurnTimer.ForeColor = _turnSeconds <= 10 ? UITheme.DangerRed : UITheme.AccentNeon;
            }
            else
            {
                _turnSeconds = 30; // Reset mẫu cho demo giao diện
            }
        }
    }
}
