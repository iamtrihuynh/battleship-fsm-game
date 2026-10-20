using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
    /// <summary>
    /// Form Trận Đấu chính (frmBanTau) do Thành viên 2 (Frontend & UI/UX Lead) phụ trách.
    /// Nhiệm vụ Ngày 2:
    /// 1. Viết hàm sinh động 10x10 = 100 Button cho 2 bàn cờ (Bàn cờ Đội nhà & Bàn cờ Tác chiến) tại sự kiện Form_Load.
    /// 2. Gắn sự kiện MouseEnter, MouseLeave để hiển thị bóng mờ (Preview) khi xếp tàu (Hợp lệ = Xanh lá, Không hợp lệ = Đỏ).
    /// 3. Lập trình phím tắt Space hoặc R để đổi chiều Dọc <-> Ngang khi rê chuột.
    /// </summary>
    public partial class frmBanTau : Form
    {
        private class ThongTinTauUI
        {
            public string TenTau { get; set; } = "";
            public int DoDai { get; set; }
            public bool DaDat { get; set; }
            public List<Point> CacViTri { get; } = new List<Point>();
        }

        private readonly string _cheDoChoi;
        private bool _huongNgang = true; // true: Ngang (Horizontal), false: Dọc (Vertical)
        private bool _amThanhBat = true;
        private int _matchSeconds = 0;
        private int _turnSeconds = 30;
        private bool _giaiDoanXepTau = true;

        // Ma trận trạng thái giao diện (0: Nước, 1: Tàu đã đặt)
        private readonly int[,] _maTranDoiNha = new int[10, 10];
        // Ma trận radar (0: Chưa bắn, 2: Trúng, 3: Trượt)
        private readonly int[,] _maTranRadar = new int[10, 10];

        // Mảng 2 chiều chứa 100 Button giao diện cho mỗi bàn cờ
        private readonly Button[,] _btnPlayerGrid = new Button[10, 10];
        private readonly Button[,] _btnRadarGrid = new Button[10, 10];

        private Point _oDangHover = new Point(-1, -1);
        private int _tongPhatBan = 0;
        private int _soPhatTrung = 0;

        // Danh sách 5 chiến hạm quy chuẩn theo tài liệu đề tài
        private readonly List<ThongTinTauUI> _danhSachTauUI = new List<ThongTinTauUI>
        {
            new ThongTinTauUI { TenTau = "Tàu sân bay (5 ô)", DoDai = 5 },
            new ThongTinTauUI { TenTau = "Thiết giáp hạm (4 ô)", DoDai = 4 },
            new ThongTinTauUI { TenTau = "Tàu ngầm (3 ô)", DoDai = 3 },
            new ThongTinTauUI { TenTau = "Tàu khu trục (3 ô)", DoDai = 3 },
            new ThongTinTauUI { TenTau = "Tàu tuần tra (2 ô)", DoDai = 2 }
        };

        public frmBanTau(string cheDoChoi = "PvE")
        {
            InitializeComponent();
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Trận Địa Hải Chiến - Bắn Tàu (Giao Diện Bàn Cờ Động Ngày 2 - TV2)";

            _cheDoChoi = cheDoChoi;
            KhoiTaoThongTinChung();

            Shown += (s, e) =>
            {
                BringToFront();
                Activate();
                CanChinhGiaoDien();
            };
        }

        /// <summary>
        /// Sự kiện Form_Load: Gọi hàm sinh động 10x10=100 Button cho 2 bàn cờ theo đúng yêu cầu Ngày 2.
        /// </summary>
        private void frmBanTau_Load(object? sender, EventArgs e)
        {
            SinhBanCoDong();
            CapNhatGiaoDienKhoTau();
        }

        /// <summary>
        /// Hàm sinh động 10x10 = 100 Button cho Bàn cờ Đội nhà & Bàn cờ Tác chiến kèm hệ tọa độ A-J, 1-10.
        /// </summary>
        private void SinhBanCoDong()
        {
            pnlPlayerBoard.Controls.Clear();
            pnlRadarBoard.Controls.Clear();

            int startX = 32;
            int startY = 30;
            int cellSize = 36;
            string[] hangNhan = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

            // -------------------------------------------------------------
            // 1. SINH ĐỘNG BÀN CỜ ĐỘI NHÀ (10x10 = 100 BUTTON)
            // -------------------------------------------------------------
            for (int c = 0; c < 10; c++)
            {
                Label lblCol = new Label
                {
                    Text = (c + 1).ToString(),
                    Location = new Point(startX + c * cellSize, 6),
                    Size = new Size(cellSize, 22),
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    ForeColor = UITheme.PrimaryCyan,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pnlPlayerBoard.Controls.Add(lblCol);
            }

            for (int r = 0; r < 10; r++)
            {
                Label lblRow = new Label
                {
                    Text = hangNhan[r],
                    Location = new Point(6, startY + r * cellSize),
                    Size = new Size(24, cellSize),
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    ForeColor = UITheme.PrimaryCyan,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pnlPlayerBoard.Controls.Add(lblRow);

                for (int c = 0; c < 10; c++)
                {
                    Button btn = new Button
                    {
                        Name = $"btnPlayer_{r}_{c}",
                        Location = new Point(startX + c * cellSize, startY + r * cellSize),
                        Size = new Size(cellSize, cellSize),
                        FlatStyle = FlatStyle.Flat,
                        BackColor = UITheme.WaterCell,
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                        Cursor = Cursors.Hand,
                        Margin = Padding.Empty,
                        Padding = Padding.Empty,
                        Tag = new Point(c, r) // X: Cột (0..9), Y: Hàng (0..9)
                    };
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = UITheme.BorderDefault;

                    // Gắn sự kiện MouseEnter, MouseLeave để hiển thị bóng mờ (Preview) khi xếp tàu
                    btn.MouseEnter += btnPlayerCell_MouseEnter;
                    btn.MouseLeave += btnPlayerCell_MouseLeave;
                    btn.Click += btnPlayerCell_Click;

                    _btnPlayerGrid[r, c] = btn;
                    pnlPlayerBoard.Controls.Add(btn);
                }
            }

            // -------------------------------------------------------------
            // 2. SINH ĐỘNG BÀN CỜ TÁC CHIẾN / RADAR (10x10 = 100 BUTTON)
            // -------------------------------------------------------------
            for (int c = 0; c < 10; c++)
            {
                Label lblCol = new Label
                {
                    Text = (c + 1).ToString(),
                    Location = new Point(startX + c * cellSize, 6),
                    Size = new Size(cellSize, 22),
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    ForeColor = UITheme.AccentNeon,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pnlRadarBoard.Controls.Add(lblCol);
            }

            for (int r = 0; r < 10; r++)
            {
                Label lblRow = new Label
                {
                    Text = hangNhan[r],
                    Location = new Point(6, startY + r * cellSize),
                    Size = new Size(24, cellSize),
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    ForeColor = UITheme.AccentNeon,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pnlRadarBoard.Controls.Add(lblRow);

                for (int c = 0; c < 10; c++)
                {
                    Button btn = new Button
                    {
                        Name = $"btnRadar_{r}_{c}",
                        Location = new Point(startX + c * cellSize, startY + r * cellSize),
                        Size = new Size(cellSize, cellSize),
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.FromArgb(15, 23, 42),
                        ForeColor = UITheme.AccentNeon,
                        Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                        Cursor = Cursors.Cross,
                        Margin = Padding.Empty,
                        Padding = Padding.Empty,
                        Tag = new Point(c, r)
                    };
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = UITheme.BorderDefault;

                    btn.MouseEnter += btnRadarCell_MouseEnter;
                    btn.MouseLeave += btnRadarCell_MouseLeave;
                    btn.Click += btnRadarCell_Click;

                    _btnRadarGrid[r, c] = btn;
                    pnlRadarBoard.Controls.Add(btn);
                }
            }
        }

        #region XỬ LÝ BÓNG MỜ (PREVIEW SHADOW) KHI XẾP TÀU
        private void btnPlayerCell_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Point pt)
            {
                _oDangHover = pt;
                HienThiPreviewShadow(pt.X, pt.Y);
            }
        }

        private void btnPlayerCell_MouseLeave(object? sender, EventArgs e)
        {
            LamMoiMauBanCoDoiNha();
            _oDangHover = new Point(-1, -1);
        }

        /// <summary>
        /// Hiển thị bóng mờ (Preview) khi xếp tàu:
        /// - Vị trí Hợp lệ: Tô màu Xanh lá (#10B981) kèm biểu tượng ⚓.
        /// - Vị trí Không hợp lệ (tràn mép bàn cờ, đè lên tàu đã xếp): Tô màu Đỏ (#EF4444) kèm biểu tượng ✕.
        /// </summary>
        private void HienThiPreviewShadow(int col, int row)
        {
            if (!_giaiDoanXepTau) return;
            if (cboSelectShip.SelectedIndex < 0 || cboSelectShip.SelectedIndex >= _danhSachTauUI.Count) return;

            var tauDangChon = _danhSachTauUI[cboSelectShip.SelectedIndex];
            int doDai = tauDangChon.DoDai;

            // Làm mới các ô trên bàn cờ về màu gốc trước khi vẽ bóng mờ mới
            LamMoiMauBanCoDoiNha();

            bool hopLe = true;
            List<Point> toaDoChiemCho = new List<Point>();
            List<Point> toaDoTrongLuoi = new List<Point>();

            for (int i = 0; i < doDai; i++)
            {
                int c = _huongNgang ? (col + i) : col;
                int r = _huongNgang ? row : (row + i);
                toaDoChiemCho.Add(new Point(c, r));

                if (c < 0 || c >= 10 || r < 0 || r >= 10)
                {
                    hopLe = false; // Tràn mép bàn cờ
                }
                else
                {
                    toaDoTrongLuoi.Add(new Point(c, r));
                    // Kiểm tra va chạm với tàu khác đã đặt (bỏ qua nếu chính tàu này đang được xếp lại)
                    if (_maTranDoiNha[r, c] == 1 && !tauDangChon.CacViTri.Contains(new Point(c, r)))
                    {
                        hopLe = false; // Đè lên tàu đã xếp
                    }
                }
            }

            if (hopLe)
            {
                // Hợp lệ: Xanh lá
                foreach (var p in toaDoTrongLuoi)
                {
                    var btn = _btnPlayerGrid[p.Y, p.X];
                    btn.BackColor = Color.FromArgb(16, 185, 129); // Success Green
                    btn.FlatAppearance.BorderColor = Color.FromArgb(52, 211, 153);
                    btn.ForeColor = Color.White;
                    btn.Text = "⚓";
                }
                lblTurnMessage.Text = $"✅ Vị trí HỢP LỆ: {((char)('A' + row))}{col + 1} • {(_huongNgang ? "Ngang" : "Dọc")} • {tauDangChon.TenTau} [Click để đặt]";
                lblTurnMessage.ForeColor = UITheme.SuccessGreen;
            }
            else
            {
                // Không hợp lệ: Đỏ
                foreach (var p in toaDoTrongLuoi)
                {
                    var btn = _btnPlayerGrid[p.Y, p.X];
                    btn.BackColor = Color.FromArgb(239, 68, 68); // Danger Red
                    btn.FlatAppearance.BorderColor = Color.FromArgb(248, 113, 113);
                    btn.ForeColor = Color.White;
                    btn.Text = "✕";
                }
                lblTurnMessage.Text = $"❌ Vị trí KHÔNG HỢP LỆ! (Chạm mép bàn cờ hoặc đè lên tàu khác)";
                lblTurnMessage.ForeColor = UITheme.DangerRed;
            }
        }

        /// <summary>
        /// Khôi phục màu sắc các ô trên bàn cờ Đội nhà theo trạng thái thực tế trong ma trận.
        /// </summary>
        private void LamMoiMauBanCoDoiNha()
        {
            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 10; c++)
                {
                    var btn = _btnPlayerGrid[r, c];
                    if (btn == null) continue;

                    if (_maTranDoiNha[r, c] == 1)
                    {
                        btn.BackColor = UITheme.ShipCell; // Màu cyan tàu chiến
                        btn.FlatAppearance.BorderColor = UITheme.PrimaryCyan;
                        btn.ForeColor = Color.White;
                        btn.Text = "🚢";
                    }
                    else
                    {
                        btn.BackColor = UITheme.WaterCell;
                        btn.FlatAppearance.BorderColor = UITheme.BorderDefault;
                        btn.ForeColor = Color.White;
                        btn.Text = "";
                    }
                }
            }
        }
        #endregion

        #region PHÍM TẮT XOAY HƯỚNG TÀU (SPACE / R)
        /// <summary>
        /// Lập trình phím tắt Space hoặc R để đổi chiều Dọc <-> Ngang khi rê chuột.
        /// Khi đổi chiều, bóng mờ (Preview) tại vị trí con trỏ lập tức được cập nhật theo hướng mới.
        /// </summary>
        private void frmBanTau_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R || e.KeyCode == Keys.Space)
            {
                DoiHuongTau();
                // Cập nhật ngay lập tức bóng mờ nếu đang rê chuột trên bàn cờ
                if (_oDangHover.X >= 0 && _oDangHover.Y >= 0)
                {
                    HienThiPreviewShadow(_oDangHover.X, _oDangHover.Y);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnRotateShip_Click(object? sender, EventArgs e)
        {
            DoiHuongTau();
            if (_oDangHover.X >= 0 && _oDangHover.Y >= 0)
            {
                HienThiPreviewShadow(_oDangHover.X, _oDangHover.Y);
            }
        }

        private void DoiHuongTau()
        {
            _huongNgang = !_huongNgang;
            btnRotateShip.Text = $"🔄 Xoay Hướng (R / Space): {(_huongNgang ? "NGANG" : "DỌC")}";
            btnRotateShip.ForeColor = _huongNgang ? UITheme.PrimaryCyan : UITheme.AccentNeon;
        }
        #endregion

        #region THAO TÁC ĐẶT TÀU & ĐIỀU KHIỂN KHO TÀU
        private void btnPlayerCell_Click(object? sender, EventArgs e)
        {
            if (!_giaiDoanXepTau) return;
            if (sender is not Button btn || btn.Tag is not Point pt) return;

            int col = pt.X;
            int row = pt.Y;

            if (cboSelectShip.SelectedIndex < 0 || cboSelectShip.SelectedIndex >= _danhSachTauUI.Count) return;
            var tauDangChon = _danhSachTauUI[cboSelectShip.SelectedIndex];
            int doDai = tauDangChon.DoDai;

            // Kiểm tra tính hợp lệ của vị trí đặt
            bool hopLe = true;
            List<Point> viTriMoi = new List<Point>();
            for (int i = 0; i < doDai; i++)
            {
                int c = _huongNgang ? (col + i) : col;
                int r = _huongNgang ? row : (row + i);

                if (c < 0 || c >= 10 || r < 0 || r >= 10)
                {
                    hopLe = false;
                    break;
                }
                if (_maTranDoiNha[r, c] == 1 && !tauDangChon.CacViTri.Contains(new Point(c, r)))
                {
                    hopLe = false;
                    break;
                }
                viTriMoi.Add(new Point(c, r));
            }

            if (!hopLe)
            {
                lblTurnMessage.Text = "⚠️ Không thể đặt tàu tại đây! Hãy chọn vị trí hiển thị bóng xanh.";
                lblTurnMessage.ForeColor = UITheme.DangerRed;
                return;
            }

            // Nếu tàu này từng đặt ở vị trí cũ, xóa vị trí cũ trên ma trận
            if (tauDangChon.DaDat)
            {
                foreach (var oldP in tauDangChon.CacViTri)
                {
                    _maTranDoiNha[oldP.Y, oldP.X] = 0;
                }
                tauDangChon.CacViTri.Clear();
            }

            // Ghi nhận vị trí mới
            foreach (var p in viTriMoi)
            {
                _maTranDoiNha[p.Y, p.X] = 1;
                tauDangChon.CacViTri.Add(p);
            }
            tauDangChon.DaDat = true;

            LamMoiMauBanCoDoiNha();
            CapNhatGiaoDienKhoTau();

            // Tự động chuyển sang tàu tiếp theo chưa được xếp
            int nextUnplaced = _danhSachTauUI.FindIndex(t => !t.DaDat);
            if (nextUnplaced >= 0)
            {
                cboSelectShip.SelectedIndex = nextUnplaced;
            }

            // Kiểm tra số lượng tàu đã hoàn tất
            int soTauDaDat = _danhSachTauUI.Count(t => t.DaDat);
            int tongSoO = _danhSachTauUI.Where(t => t.DaDat).Sum(t => t.DoDai);
            lblPlayerFleetStatus.Text = $"Hạm đội ta: {soTauDaDat}/5 tàu ({tongSoO} ô)";

            if (soTauDaDat == 5)
            {
                btnStartBattle.Enabled = true;
                btnStartBattle.BackColor = UITheme.AccentNeon;
                btnStartBattle.ForeColor = UITheme.HeaderBackground;
                lblTurnIndicator.Text = "HẠM ĐỘI ĐÃ SẴN SÀNG!";
                lblTurnIndicator.ForeColor = UITheme.AccentNeon;
                lblTurnMessage.Text = "🎉 Đã xếp đủ 5/5 tàu! Hãy bấm 'Bắt đầu chiến đấu' để khai hỏa!";
                lblTurnMessage.ForeColor = UITheme.AccentNeon;
            }
            else
            {
                lblTurnMessage.Text = $"Đã đặt thành công {tauDangChon.TenTau}! Tiến độ: {soTauDaDat}/5 tàu.";
                lblTurnMessage.ForeColor = UITheme.SuccessGreen;
            }
        }

        private void CapNhatGiaoDienKhoTau()
        {
            int currentIndex = cboSelectShip.SelectedIndex;
            cboSelectShip.Items.Clear();
            foreach (var tau in _danhSachTauUI)
            {
                string status = tau.DaDat ? "[Đã đặt]" : "[Chưa đặt]";
                cboSelectShip.Items.Add($"{tau.TenTau} - {status}");
            }
            if (currentIndex >= 0 && currentIndex < cboSelectShip.Items.Count)
            {
                cboSelectShip.SelectedIndex = currentIndex;
            }
            else if (cboSelectShip.Items.Count > 0)
            {
                cboSelectShip.SelectedIndex = 0;
            }
        }

        private void cboSelectShip_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboSelectShip.SelectedItem != null)
            {
                lblTurnMessage.Text = $"Đang chọn: {cboSelectShip.SelectedItem}";
                lblTurnMessage.ForeColor = UITheme.TextSecondary;
            }
        }

        private void btnResetBoard_Click(object? sender, EventArgs e)
        {
            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 10; c++)
                {
                    _maTranDoiNha[r, c] = 0;
                }
            }

            foreach (var tau in _danhSachTauUI)
            {
                tau.DaDat = false;
                tau.CacViTri.Clear();
            }

            LamMoiMauBanCoDoiNha();
            CapNhatGiaoDienKhoTau();
            cboSelectShip.SelectedIndex = 0;

            btnStartBattle.Enabled = false;
            btnStartBattle.BackColor = UITheme.PanelBackground;
            btnStartBattle.ForeColor = UITheme.MissGray;
            lblPlayerFleetStatus.Text = "Hạm đội ta: 0/5 tàu (0 ô)";
            lblTurnIndicator.Text = "GIAI ĐOẠN: BỐ TRÍ HẠM ĐỘI";
            lblTurnIndicator.ForeColor = UITheme.PrimaryCyan;
            lblTurnMessage.Text = "Đã thu hồi toàn bộ hạm đội về kho. Hãy click đặt tàu hoặc bấm 'Xếp ngẫu nhiên'.";
            lblTurnMessage.ForeColor = UITheme.TextSecondary;
        }

        /// <summary>
        /// Nút Xếp ngẫu nhiên (Auto Place): Hỗ trợ người chơi sinh tự động 5 tàu ngay ngắn trên bàn cờ.
        /// </summary>
        private void btnAutoPlace_Click(object? sender, EventArgs e)
        {
            XepTauNgauNhienGiaoDien();
        }

        private void XepTauNgauNhienGiaoDien()
        {
            // Làm sạch bàn cờ
            btnResetBoard_Click(this, EventArgs.Empty);

            Random rng = new Random();
            foreach (var tau in _danhSachTauUI)
            {
                bool daXep = false;
                int lanThu = 0;

                while (!daXep && lanThu < 1000)
                {
                    lanThu++;
                    bool huong = rng.Next(2) == 0;
                    int startC = huong ? rng.Next(10 - tau.DoDai + 1) : rng.Next(10);
                    int startR = huong ? rng.Next(10) : rng.Next(10 - tau.DoDai + 1);

                    bool hopLe = true;
                    for (int i = 0; i < tau.DoDai; i++)
                    {
                        int c = huong ? (startC + i) : startC;
                        int r = huong ? startR : (startR + i);
                        if (_maTranDoiNha[r, c] == 1)
                        {
                            hopLe = false;
                            break;
                        }
                    }

                    if (hopLe)
                    {
                        for (int i = 0; i < tau.DoDai; i++)
                        {
                            int c = huong ? (startC + i) : startC;
                            int r = huong ? startR : (startR + i);
                            _maTranDoiNha[r, c] = 1;
                            tau.CacViTri.Add(new Point(c, r));
                        }
                        tau.DaDat = true;
                        daXep = true;
                    }
                }
            }

            LamMoiMauBanCoDoiNha();
            CapNhatGiaoDienKhoTau();

            btnStartBattle.Enabled = true;
            btnStartBattle.BackColor = UITheme.AccentNeon;
            btnStartBattle.ForeColor = UITheme.HeaderBackground;
            lblPlayerFleetStatus.Text = "Hạm đội ta: 5/5 tàu (17 ô)";
            lblTurnIndicator.Text = "HẠM ĐỘI ĐÃ SẴN SÀNG!";
            lblTurnIndicator.ForeColor = UITheme.AccentNeon;
            lblTurnMessage.Text = "⚡ Đã xếp ngẫu nhiên 5/5 chiến hạm ngay ngắn trên bàn cờ!";
            lblTurnMessage.ForeColor = UITheme.AccentNeon;

            MessageBox.Show(
                "⚡ [Ngày 2 - Giao diện TV2]: Đã tự động xếp ngẫu nhiên toàn bộ 5 tàu ngay ngắn trên bàn cờ 10x10!",
                "Xếp tàu tự động",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnStartBattle_Click(object? sender, EventArgs e)
        {
            _giaiDoanXepTau = false;
            pnlPlacementControls.Enabled = false;
            pnlCombatControls.Enabled = true;

            lblTurnIndicator.Text = "LƯỢT: BẠN KHAI HỎA";
            lblTurnIndicator.ForeColor = UITheme.AccentNeon;
            lblTurnMessage.Text = "Click ô trên Radar đối phương để khai hỏa!";
            lblTurnMessage.ForeColor = UITheme.AccentNeon;

            MessageBox.Show(
                "⚔️ [Giao diện Ngày 2 - TV2]: Chuyển sang giai đoạn Chiến đấu thành công!\nCác ô trên Radar đối phương đã sẵn sàng tiếp nhận phát bắn.",
                "Giai đoạn Chiến đấu",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        #endregion

        #region TƯƠNG TÁC BÀN CỜ RADAR ĐỐI PHƯƠNG
        private void btnRadarCell_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Point pt)
            {
                int col = pt.X;
                int row = pt.Y;
                if (_maTranRadar[row, col] == 0)
                {
                    btn.FlatAppearance.BorderColor = UITheme.AccentNeon;
                    btn.BackColor = Color.FromArgb(20, 35, 60);
                }
                lblTurnMessage.Text = $"🎯 Tọa độ ngắm bắn Radar: {((char)('A' + row))}{col + 1}";
                lblTurnMessage.ForeColor = UITheme.AccentNeon;
            }
        }

        private void btnRadarCell_MouseLeave(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Point pt)
            {
                int col = pt.X;
                int row = pt.Y;
                if (_maTranRadar[row, col] == 0)
                {
                    btn.FlatAppearance.BorderColor = UITheme.BorderDefault;
                    btn.BackColor = Color.FromArgb(15, 23, 42);
                }
            }
        }

        private void btnRadarCell_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not Point pt) return;
            int col = pt.X;
            int row = pt.Y;

            if (_giaiDoanXepTau)
            {
                MessageBox.Show(
                    "⚠️ Vui lòng hoàn thành xếp đủ 5 tàu trên bàn cờ của bạn và bấm 'BẮT ĐẦU CHIẾN ĐẤU' trước khi khai hỏa!",
                    "Chưa bắt đầu trận đấu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (_maTranRadar[row, col] != 0)
            {
                lblTurnMessage.Text = $"⚠️ Tọa độ {((char)('A' + row))}{col + 1} đã được bắn trước đó! Hãy chọn tọa độ khác.";
                lblTurnMessage.ForeColor = UITheme.DangerRed;
                return;
            }

            _tongPhatBan++;
            // Mô phỏng trực quan hiệu ứng phát bắn trên giao diện (sẽ được tích hợp logic AI ở Ngày 3)
            bool trung = (_tongPhatBan % 3 == 0);
            if (trung)
            {
                _soPhatTrung++;
                _maTranRadar[row, col] = 2; // Trúng (Lửa đỏ)
                btn.BackColor = UITheme.DangerRed;
                btn.FlatAppearance.BorderColor = Color.FromArgb(248, 113, 113);
                btn.ForeColor = Color.Yellow;
                btn.Text = "✕";
                lblTurnMessage.Text = $"💥 BẮN TRÚNG MỤC TIÊU tại {((char)('A' + row))}{col + 1}! Bạn được thưởng thêm 1 lượt bắn!";
                lblTurnMessage.ForeColor = UITheme.DangerRed;
            }
            else
            {
                _maTranRadar[row, col] = 3; // Trượt (Sóng nước)
                btn.BackColor = Color.FromArgb(45, 58, 76);
                btn.FlatAppearance.BorderColor = UITheme.BorderDefault;
                btn.ForeColor = UITheme.MissGray;
                btn.Text = "•";
                lblTurnMessage.Text = $"🌊 BẮN TRƯỢT tại {((char)('A' + row))}{col + 1}! Đổi lượt ngắm bắn.";
                lblTurnMessage.ForeColor = UITheme.TextSecondary;
            }

            // Cập nhật thống kê
            lblShotCounts.Text = $"Phát bắn: {_tongPhatBan} (Trúng: {_soPhatTrung} • Trượt: {_tongPhatBan - _soPhatTrung})";
            double tiLe = _tongPhatBan > 0 ? (_soPhatTrung * 100.0 / _tongPhatBan) : 0;
            lblAccuracy.Text = $"Độ chính xác: {tiLe:F1}%";
        }
        #endregion

        #region BỘ ĐIỀU KHIỂN & ĐỒNG HỒ
        private void KhoiTaoThongTinChung()
        {
            lblMatchModeInfo.Text = _cheDoChoi switch
            {
                "PvE" => "Chế độ: Đấu với Máy (AI) • Bàn cờ động 10x10 • Hạm đội 5 tàu",
                "PvP" => "Chế độ: Hai người chơi chung 1 máy (Hotseat) • Cơ chế màn chắn sương mù chuyển lượt",
                _ => "Chế độ: Tùy biến / Tự do • Bàn cờ động 10x10"
            };

            CapNhatNutAmThanh();

            tmrMatch.Start();
            tmrTurnCountdown.Start();
        }

        private void CapNhatNutAmThanh()
        {
            btnToggleSound.Text = _amThanhBat ? "🔊 ÂM THANH: BẬT" : "🔇 ÂM THANH: TẮT";
            btnToggleSound.ForeColor = _amThanhBat ? UITheme.AccentNeon : UITheme.MissGray;
        }

        private void btnToggleSound_Click(object? sender, EventArgs e)
        {
            _amThanhBat = !_amThanhBat;
            CapNhatNutAmThanh();
        }

        private void btnBackToMenu_Click(object? sender, EventArgs e)
        {
            tmrMatch.Stop();
            tmrTurnCountdown.Stop();
            Close();
        }

        private void btnSkillRadar_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "📡 [Giao diện Ngày 2 - TV2]: Kỹ năng Radar trinh sát 3x3 đã sẵn sàng kết nối.",
                "Kỹ năng đặc biệt",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnSkillCrossBomb_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "💥 [Giao diện Ngày 2 - TV2]: Kỹ năng Pháo kích diện rộng (+) 5 ô đã sẵn sàng kết nối.",
                "Kỹ năng đặc biệt",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnPauseGame_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("⏸ [Giao diện Ngày 2 - TV2]: Tạm dừng trận đấu.", "Tạm dừng", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSurrender_Click(object? sender, EventArgs e)
        {
            var res = MessageBox.Show("Đồng chí có chắc chắn muốn đầu hàng?", "Xác nhận đầu hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                tmrMatch.Stop();
                tmrTurnCountdown.Stop();
                Close();
            }
        }

        private void btnFogContinue_Click(object? sender, EventArgs e)
        {
            pnlFogOverlay.Visible = false;
        }

        private void tmrMatch_Tick(object? sender, EventArgs e)
        {
            _matchSeconds++;
            int m = _matchSeconds / 60;
            int s = _matchSeconds % 60;
            lblMatchTimer.Text = $"⏱ Trận: {m:D2}:{s:D2}";
        }

        private void tmrTurnCountdown_Tick(object? sender, EventArgs e)
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
                _turnSeconds = 30;
            }
        }

        private void frmBanTau_Resize(object? sender, EventArgs e)
        {
            CanChinhGiaoDien();
        }

        private void CanChinhGiaoDien()
        {
            if (pnlCenterDashboard == null || pnlLeftBoardContainer == null || pnlRightBoardContainer == null) return;

            // Neo nút âm thanh ở thanh trên cùng bên phải
            btnToggleSound.Left = pnlTopBar.Width - btnToggleSound.Width - 20;

            // Canh giữa 3 khu vực khi mở to / toàn màn hình
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
        #endregion
    }
}
