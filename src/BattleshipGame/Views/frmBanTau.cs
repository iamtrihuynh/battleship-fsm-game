using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BattleshipGame.DataAccess;
using BattleshipGame.Logic;
using BattleshipGame.Models;

namespace BattleshipGame.Views
{
    /// <summary>
    /// Form Trận Đấu chính (frmBanTau) do Thành viên 2 (Frontend & UI/UX Lead) phụ trách.
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
        private int _matchSeconds = 0;
        private int _turnSeconds = 30;
        private bool _giaiDoanXepTau = true;
        private bool _tranDauKetThuc = false;
        private bool _dangXuLyBan = false;

        private enum CheDoKyNang
        {
            KhongCo,
            RadarTrinhSat,
            PhaoKichDienRong
        }
        private CheDoKyNang _kyNangHienTai = CheDoKyNang.KhongCo;
        private bool _daDungSkillRadar = false;
        private bool _daDungSkillBomb = false;

        // Ban co logic
        private readonly cBanCo _banCoTa = new cBanCo(10);
        private readonly cBanCo _banCoDoiThu = new cBanCo(10);
        private readonly HashSet<Point> _cacOBotDaBan = new HashSet<Point>();
        private readonly Random _randBot = new Random();

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
            MinimumSize = new Size(1180, 720);
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
            foreach (Control ctl in pnlPlayerBoard.Controls) ctl.Dispose();
            pnlPlayerBoard.Controls.Clear();
            foreach (Control ctl in pnlRadarBoard.Controls) ctl.Dispose();
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

        #region XỬ LÝ ĐỒ HỌA PHÁT BẮN & MÀN CHE SƯƠNG MÙ (NGÀY 3 - THÀNH VIÊN 2)
        /// <summary>
        /// [NGÀY 3 - TV2] Xử lý đồ họa cho Ô TRƯỢT:
        /// Đổi sang màu xanh sóng nước (#1E3A58), viền sóng (#38BDF8), hiển thị ký tự "•".
        /// </summary>
        public void XuLyDoHoa_OTruot(Button btn)
        {
            if (btn == null) return;
            btn.BackColor = UITheme.MissRippleBackground;
            btn.FlatAppearance.BorderColor = UITheme.MissRippleBorder;
            btn.FlatAppearance.BorderSize = 1;
            btn.ForeColor = UITheme.MissRippleText;
            btn.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
            btn.Text = "•";
            btn.Cursor = Cursors.Default;
        }

        /// <summary>
        /// [NGÀY 3 - TV2] Xử lý đồ họa cho Ô TRÚNG:
        /// Đổi sang màu đỏ rực (#EF4444), hiển thị ký tự "X" màu vàng lửa (#FEF08A).
        /// </summary>
        public void XuLyDoHoa_OTrung(Button btn)
        {
            if (btn == null) return;
            btn.BackColor = UITheme.HitFireBackground;
            btn.FlatAppearance.BorderColor = UITheme.HitFireBorder;
            btn.FlatAppearance.BorderSize = 1;
            btn.ForeColor = UITheme.HitFireText;
            btn.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            btn.Text = "X";
            btn.Cursor = Cursors.Default;
        }

        /// <summary>
        /// [NGÀY 3 - TV2] Xử lý đồ họa khi TÀU CHÌM:
        /// Đổi TOÀN BỘ các ô của con tàu đó sang màu xám tro (#374151), viền đậm 2px màu đỏ sẫm (#991B1B).
        /// </summary>
        public void XuLyDoHoa_TauChim(IEnumerable<Button> danhSachOBiChim)
        {
            if (danhSachOBiChim == null) return;
            foreach (var btn in danhSachOBiChim)
            {
                if (btn == null) continue;
                btn.BackColor = UITheme.SunkAshBackground;
                btn.FlatAppearance.BorderColor = UITheme.SunkThickBorder;
                btn.FlatAppearance.BorderSize = UITheme.SunkBorderThickness; // Viền đậm 2px
                btn.ForeColor = UITheme.SunkAshText;
                btn.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
                btn.Text = "X";
                btn.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Quá tải nhận danh sách tọa độ (X, Y) trên Bàn cờ Radar
        /// </summary>
        public void XuLyDoHoa_TauChim(IEnumerable<Point> danhSachToaDo)
        {
            if (danhSachToaDo == null) return;
            var buttons = new List<Button>();
            foreach (var pt in danhSachToaDo)
            {
                if (pt.Y >= 0 && pt.Y < 10 && pt.X >= 0 && pt.X < 10)
                {
                    var btn = _btnRadarGrid[pt.Y, pt.X];
                    if (btn != null) buttons.Add(btn);
                }
            }
            XuLyDoHoa_TauChim(buttons);
        }

        /// <summary>
        /// Kích hoạt màn che sương mù cho chế độ PvP Hotseat
        /// </summary>
        public void KichHoatManCheSuongMu(int nguoiChoiTiepTheo = 2)
        {
            if (pnlFogOverlay == null) return;
            pnlFogOverlay.Visible = true;
            pnlFogOverlay.BringToFront();
            lblFogTitle.Text = "🌫️ MÀN CHẮN SƯƠNG MÙ CHIẾN THUẬT";
            lblFogMessage.Text = "Để đảm bảo bí mật chiến thuật tuyệt đối giữa hai người chơi tác chiến trên cùng 1 máy tính,\n" +
                                 "toàn bộ hạm đội và radar đã được che kín bởi màn sương mù dày đặc.\n\n" +
                                 $"Xin mời NGƯỜI CHƠI {nguoiChoiTiepTheo} nhận máy và bấm nút bên dưới để mở bàn cờ tác chiến của bạn:";
            btnFogContinue.Text = $"⚓ Người chơi {nguoiChoiTiepTheo}: Sẵn sàng";
            CanChinhGiaoDien();
        }
        #endregion

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
            if (!_giaiDoanXepTau) return;

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

            // Đồng bộ trực tiếp với model logic cBanCo
            int shipIdx = cboSelectShip.SelectedIndex;
            if (shipIdx >= 0 && shipIdx < _banCoTa.DanhSachTau.Count)
            {
                var tauModel = _banCoTa.DanhSachTau[shipIdx];
                _banCoTa.DatTau(tauModel, new Point(col, row), _huongNgang ? HuongTau.Ngang : HuongTau.Doc);
            }

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
            if (!_giaiDoanXepTau) return;

            _banCoTa.XoaTatCaTau();

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

            _daDungSkillRadar = false;
            _daDungSkillBomb = false;
            _kyNangHienTai = CheDoKyNang.KhongCo;
            btnSkillRadar.Enabled = true;
            btnSkillRadar.BackColor = UITheme.PanelBackground;
            btnSkillRadar.ForeColor = UITheme.PrimaryCyan;
            btnSkillCrossBomb.Enabled = true;
            btnSkillCrossBomb.BackColor = UITheme.PanelBackground;
            btnSkillCrossBomb.ForeColor = UITheme.AccentNeon;

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
            if (!_giaiDoanXepTau) return;
            XepTauNgauNhienGiaoDien();
        }

        private void XepTauNgauNhienGiaoDien()
        {
            if (!_giaiDoanXepTau) return;

            // Xoa ban co truoc khi xep
            btnResetBoard_Click(this, EventArgs.Empty);

            bool ok = _banCoTa.TuDongSinhBanCoNgauNhien();
            if (ok)
            {
                for (int r = 0; r < 10; r++)
                {
                    for (int c = 0; c < 10; c++)
                    {
                        _maTranDoiNha[r, c] = _banCoTa.MaTranO[c, r].CoTau ? 1 : 0;
                    }
                }

                for (int i = 0; i < _banCoTa.DanhSachTau.Count && i < _danhSachTauUI.Count; i++)
                {
                    var tauModel = _banCoTa.DanhSachTau[i];
                    var tauUI = _danhSachTauUI[i];
                    tauUI.DaDat = true;
                    tauUI.CacViTri.Clear();
                    tauUI.CacViTri.AddRange(tauModel.DanhSachToaDo);
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
                    "⚡ Đã tự động xếp ngẫu nhiên toàn bộ 5 tàu ngay ngắn trên bàn cờ 10x10!",
                    "Xếp tàu tự động",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void btnStartBattle_Click(object? sender, EventArgs e)
        {
            if (_danhSachTauUI.Count(t => t.DaDat) < 5)
            {
                MessageBox.Show("Vui lòng bố trí đủ 5 tàu trước khi bắt đầu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Đồng bộ an toàn lần cuối giữa UI và model lõi _banCoTa
            for (int i = 0; i < _danhSachTauUI.Count && i < _banCoTa.DanhSachTau.Count; i++)
            {
                var uiShip = _danhSachTauUI[i];
                var modelShip = _banCoTa.DanhSachTau[i];
                if (uiShip.DaDat && uiShip.CacViTri.Count > 0 && !modelShip.DaDatXong)
                {
                    Point goc = uiShip.CacViTri[0];
                    HuongTau huong = (uiShip.CacViTri.Count > 1 && uiShip.CacViTri[1].Y != uiShip.CacViTri[0].Y)
                        ? HuongTau.Doc : HuongTau.Ngang;
                    _banCoTa.DatTau(modelShip, goc, huong);
                }
            }

            _giaiDoanXepTau = false;
            pnlPlacementControls.Enabled = false;
            pnlCombatControls.Enabled = true;

            // Sinh ngau nhien ham doi doi thu
            _banCoDoiThu.TuDongSinhBanCoNgauNhien();

            // Bat nhac nen tran chien
            cAmThanh.PhatNhacNen("bgm_battle.wav");

            // Khoi dong dong ho tran dau
            _turnSeconds = 30;
            tmrMatch.Start();
            tmrTurnCountdown.Start();

            lblTurnIndicator.Text = "LƯỢT: BẠN KHAI HỎA";
            lblTurnIndicator.ForeColor = UITheme.AccentNeon;
            lblTurnMessage.Text = "Click ô trên Radar đối phương để khai hỏa!";
            lblTurnMessage.ForeColor = UITheme.AccentNeon;

            MessageBox.Show(
                "⚔️ Chuyển sang giai đoạn Chiến đấu thành công!\nCác ô trên Radar đối phương đã sẵn sàng tiếp nhận phát bắn.",
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
            if (_giaiDoanXepTau || _tranDauKetThuc) return;
            if (_dangXuLyBan) return;

            int col = pt.X;
            int row = pt.Y;

            if (_kyNangHienTai == CheDoKyNang.RadarTrinhSat)
            {
                ThucHienQuetRadar(col, row);
                return;
            }

            if (_kyNangHienTai == CheDoKyNang.PhaoKichDienRong)
            {
                ThucHienPhaoKich(col, row);
                return;
            }

            if (_maTranRadar[row, col] != 0)
            {
                lblTurnMessage.Text = $"⚠️ Tọa độ {((char)('A' + row))}{col + 1} đã được bắn trước đó! Hãy chọn tọa độ khác.";
                lblTurnMessage.ForeColor = UITheme.DangerRed;
                return;
            }

            _dangXuLyBan = true;
            try
            {
                _tongPhatBan++;
                _turnSeconds = 30; // reset countdown luot

                // Phat tieng dai bac
                cAmThanh.PhatShot();

                // Tiep nhan phat ban thuc su tu ban co doi thu
                KetQuaBan kq = _banCoDoiThu.TiepNhanPhatBan(new Point(col, row), out cTau tauBiBan);

                if (kq == KetQuaBan.Hit)
                {
                    _soPhatTrung++;
                    _maTranRadar[row, col] = 2; // Trúng
                    XuLyDoHoa_OTrung(btn);

                    cAmThanh.PhatHit();
                    string tenTau = tauBiBan != null ? tauBiBan.TenTau : "Chiến hạm";
                    lblTurnMessage.Text = $"💥 BẮN TRÚNG {tenTau}! Lửa cháy bùng lên (X). Bạn được thưởng thêm 1 lượt!";
                    lblTurnMessage.ForeColor = UITheme.HitFireBackground;
                }
                else if (kq == KetQuaBan.Sunk)
                {
                    _soPhatTrung++;
                    _maTranRadar[row, col] = 2;
                    XuLyDoHoa_OTrung(btn);

                    // Cập nhật toàn bộ ô của con tàu đã chìm sang màu tro tàn (#374151) và viền đậm 2px (#991B1B) chuẩn Ngày 3
                    if (tauBiBan != null)
                    {
                        var buttonsTau = new List<Button>();
                        foreach (Point p in tauBiBan.DanhSachToaDo)
                        {
                            if (p.Y >= 0 && p.Y < 10 && p.X >= 0 && p.X < 10)
                            {
                                buttonsTau.Add(_btnRadarGrid[p.Y, p.X]);
                            }
                        }
                        XuLyDoHoa_TauChim(buttonsTau);
                    }

                    cAmThanh.PhatSunk();
                    string tenTau = tauBiBan != null ? tauBiBan.TenTau : "Chiến hạm";
                    lblTurnMessage.Text = $"🔥 BẮN CHÌM {tenTau}! Tro tàn phủ kín. Bạn được bắn tiếp!";
                    lblTurnMessage.ForeColor = UITheme.HitFireBackground;

                    // Kiem tra chien thang
                    if (_banCoDoiThu.TatCaTauDaChim)
                    {
                        XuLyKetThucTran(true);
                        return;
                    }
                }
                else // Miss
                {
                    _maTranRadar[row, col] = 3; // Trượt
                    XuLyDoHoa_OTruot(btn);

                    cAmThanh.PhatMiss();
                    lblTurnMessage.Text = $"🌊 BẮN TRƯỢT tại {((char)('A' + row))}{col + 1}! Sóng nước dâng trào (•). Đổi lượt ngắm bắn.";
                    lblTurnMessage.ForeColor = UITheme.MissRippleText;

                    // May ban tra neu o che do PvE, hoac kich hoat man che suong mu neu PvP
                    if (_cheDoChoi.StartsWith("PvE", StringComparison.OrdinalIgnoreCase))
                    {
                        MayBanTra();
                    }
                    else
                    {
                        KichHoatManCheSuongMu(2);
                    }
                }

                // Cập nhật thống kê
                lblShotCounts.Text = $"Phát bắn: {_tongPhatBan} (Trúng: {_soPhatTrung} • Trượt: {_tongPhatBan - _soPhatTrung})";
                double tiLe = _tongPhatBan > 0 ? (_soPhatTrung * 100.0 / _tongPhatBan) : 0;
                lblAccuracy.Text = $"Độ chính xác: {tiLe:F1}%";
            }
            finally
            {
                _dangXuLyBan = false;
            }
        }

        private void MayBanTra()
        {
            if (_banCoTa.TatCaTauDaChim) return;

            // Tim danh sach o chua ban tren ban co cua nguoi choi
            List<Point> oChuaBan = new List<Point>();
            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 10; c++)
                {
                    Point p = new Point(c, r);
                    if (!_cacOBotDaBan.Contains(p))
                        oChuaBan.Add(p);
                }
            }

            if (oChuaBan.Count == 0) return;

            Point banO = oChuaBan[_randBot.Next(oChuaBan.Count)];
            _cacOBotDaBan.Add(banO);

            KetQuaBan kq = _banCoTa.TiepNhanPhatBan(banO, out cTau tauBiBan);
            var btnO = _btnPlayerGrid[banO.Y, banO.X];

            if (kq == KetQuaBan.Hit)
            {
                XuLyDoHoa_OTrung(btnO);
            }
            else if (kq == KetQuaBan.Sunk)
            {
                XuLyDoHoa_OTrung(btnO);
                if (tauBiBan != null)
                {
                    var buttonsTau = new List<Button>();
                    foreach (Point p in tauBiBan.DanhSachToaDo)
                    {
                        if (p.Y >= 0 && p.Y < 10 && p.X >= 0 && p.X < 10)
                        {
                            buttonsTau.Add(_btnPlayerGrid[p.Y, p.X]);
                        }
                    }
                    XuLyDoHoa_TauChim(buttonsTau);
                }

                if (_banCoTa.TatCaTauDaChim)
                {
                    XuLyKetThucTran(false);
                }
            }
            else
            {
                XuLyDoHoa_OTruot(btnO);
            }
        }

        private void XuLyKetThucTran(bool nguoiChoiThang)
        {
            if (_tranDauKetThuc) return;
            _tranDauKetThuc = true;

            tmrMatch.Stop();
            tmrTurnCountdown.Stop();

            pnlRadarBoard.Enabled = false;
            pnlCombatControls.Enabled = false;

            int soTauConSong = _banCoTa.DanhSachTau.Count(t => !t.DaChim);
            long tinhDiem = 1000L + (soTauConSong * 200L) - (_tongPhatBan * 15L) - (_matchSeconds * 2L);
            int diem = nguoiChoiThang ? (int)Math.Max(0L, Math.Min((long)int.MaxValue, tinhDiem)) : 0;
            string ketQua = nguoiChoiThang ? "Thắng" : "Thua";

            if (nguoiChoiThang)
            {
                cAmThanh.PhatVictory();
                lblTurnIndicator.Text = "🏆 CHIẾN THẮNG CHUNG CUỘC!";
                lblTurnIndicator.ForeColor = UITheme.AccentNeon;
                lblTurnMessage.Text = $"🎉 Chúc mừng! Bạn đã tiêu diệt toàn bộ hạm đội địch! Điểm số: {diem:N0}";
            }
            else
            {
                cAmThanh.PhatDefeat();
                lblTurnIndicator.Text = "☠️ HẠM ĐỘI ĐÃ BỊ TIÊU DIỆT!";
                lblTurnIndicator.ForeColor = UITheme.DangerRed;
                lblTurnMessage.Text = "Hạm đội của bạn đã chìm hoàn toàn. Bạn đã thất bại!";
            }

            // Luu ket qua tran dau vao thanhtichs.txt va kiem tra ket qua
            bool luuOk = cThanhTichDAO.LuuThanhTich("Thuyền Trưởng", diem, _tongPhatBan, _matchSeconds, _cheDoChoi, ketQua);
            string thongBaoLuu = luuOk
                ? "Thành tích đã được lưu vào hệ thống!"
                : $"⚠️ Không thể lưu thành tích ({cThanhTichDAO.LoiGanNhat ?? "File bận"}).";

            MessageBox.Show(
                nguoiChoiThang
                    ? $"🎉 CHIẾN THẮNG XUẤT SẮC!\n\nSố lượt bắn: {_tongPhatBan}\nThời gian: {_matchSeconds / 60:D2}:{_matchSeconds % 60:D2}\nĐiểm số: {diem:N0}\n\n{thongBaoLuu}"
                    : $"☠️ TRẬN ĐẤU KẾT THÚC!\nHạm đội ta đã bị đối phương bắn chìm hoàn toàn.\n\n{thongBaoLuu}",
                nguoiChoiThang ? "Chiến Thắng" : "Thất Bại",
                MessageBoxButtons.OK,
                nguoiChoiThang ? MessageBoxIcon.Information : MessageBoxIcon.Stop
            );
        }
        #endregion

        #region BỘ ĐIỀU KHIỂN & ĐỒNG HỒ
        private void KhoiTaoThongTinChung()
        {
            lblMatchModeInfo.Text = _cheDoChoi switch
            {
                var s when s.StartsWith("PvE") => $"Chế độ: Đấu với Máy ({_cheDoChoi}) • Bàn cờ động 10x10",
                var s when s.StartsWith("PvP") => "Chế độ: Hai người chơi chung 1 máy (Hotseat) • Màn chắn sương mù",
                _ => $"Chế độ: {_cheDoChoi} • Bàn cờ động 10x10"
            };

            CapNhatNutAmThanh();
        }

        private void CapNhatNutAmThanh()
        {
            bool bat = cAmThanh.DaBatAmThanh;
            btnToggleSound.Text = bat ? "🔊 ÂM THANH: BẬT" : "🔇 ÂM THANH: TẮT";
            btnToggleSound.ForeColor = bat ? UITheme.AccentNeon : UITheme.MissGray;
        }

        private void btnToggleSound_Click(object? sender, EventArgs e)
        {
            cAmThanh.BatTatAmThanh();
            CapNhatNutAmThanh();
        }

        private void btnBackToMenu_Click(object? sender, EventArgs e)
        {
            cAmThanh.DungNhacNen();
            tmrMatch.Stop();
            tmrTurnCountdown.Stop();
            Close();
        }

        private void btnSkillRadar_Click(object? sender, EventArgs e)
        {
            if (_giaiDoanXepTau || _tranDauKetThuc) return;

            if (_daDungSkillRadar)
            {
                MessageBox.Show("Kỹ năng Radar trinh sát 3x3 đã được sử dụng trong trận này!", "Kỹ năng đặc biệt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_kyNangHienTai == CheDoKyNang.RadarTrinhSat)
            {
                _kyNangHienTai = CheDoKyNang.KhongCo;
                btnSkillRadar.BackColor = UITheme.PanelBackground;
                btnSkillRadar.ForeColor = UITheme.PrimaryCyan;
                lblTurnMessage.Text = "Đã hủy kích hoạt kỹ năng Radar.";
                lblTurnMessage.ForeColor = UITheme.TextSecondary;
            }
            else
            {
                _kyNangHienTai = CheDoKyNang.RadarTrinhSat;
                btnSkillRadar.BackColor = Color.FromArgb(20, 80, 120);
                btnSkillRadar.ForeColor = Color.White;
                lblTurnMessage.Text = "📡 CHẾ ĐỘ TRINH SÁT RADAR: Hãy click 1 ô trên Radar để quét vùng 3x3 quanh ô đó!";
                lblTurnMessage.ForeColor = UITheme.AccentNeon;
            }
        }

        private void btnSkillCrossBomb_Click(object? sender, EventArgs e)
        {
            if (_giaiDoanXepTau || _tranDauKetThuc) return;

            if (_daDungSkillBomb)
            {
                MessageBox.Show("Kỹ năng Pháo kích diện rộng (+) đã được sử dụng trong trận này!", "Kỹ năng đặc biệt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_kyNangHienTai == CheDoKyNang.PhaoKichDienRong)
            {
                _kyNangHienTai = CheDoKyNang.KhongCo;
                btnSkillCrossBomb.BackColor = UITheme.PanelBackground;
                btnSkillCrossBomb.ForeColor = UITheme.AccentNeon;
                lblTurnMessage.Text = "Đã hủy kích hoạt kỹ năng Pháo kích.";
                lblTurnMessage.ForeColor = UITheme.TextSecondary;
            }
            else
            {
                _kyNangHienTai = CheDoKyNang.PhaoKichDienRong;
                btnSkillCrossBomb.BackColor = Color.FromArgb(120, 30, 30);
                btnSkillCrossBomb.ForeColor = Color.White;
                lblTurnMessage.Text = "💥 CHẾ ĐỘ PHÁO KÍCH DIỆN RỘNG (+): Hãy click 1 ô tâm để nã pháo 5 ô!";
                lblTurnMessage.ForeColor = UITheme.DangerRed;
            }
        }

        private void ThucHienQuetRadar(int centerCol, int centerRow)
        {
            _daDungSkillRadar = true;
            _kyNangHienTai = CheDoKyNang.KhongCo;
            btnSkillRadar.Enabled = false;
            btnSkillRadar.BackColor = UITheme.PanelBackground;
            btnSkillRadar.ForeColor = UITheme.MissGray;

            cAmThanh.PhatRadar();

            int soOTauPhatHien = 0;
            for (int r = centerRow - 1; r <= centerRow + 1; r++)
            {
                for (int c = centerCol - 1; c <= centerCol + 1; c++)
                {
                    if (c >= 0 && c < 10 && r >= 0 && r < 10)
                    {
                        if (_banCoDoiThu.MaTranO[c, r].CoTau)
                        {
                            soOTauPhatHien++;
                        }
                    }
                }
            }

            lblTurnMessage.Text = $"📡 Radar quét khu vực {((char)('A' + centerRow))}{centerCol + 1} (3x3): Phát hiện {soOTauPhatHien} ô chiến hạm địch!";
            lblTurnMessage.ForeColor = soOTauPhatHien > 0 ? UITheme.AccentNeon : UITheme.TextSecondary;

            MessageBox.Show(
                $"📡 KẾT QUẢ TRINH SÁT RADAR 3x3 quanh tọa độ {((char)('A' + centerRow))}{centerCol + 1}:\n\n" +
                $"Số ô chiến hạm địch phát hiện: {soOTauPhatHien} ô.",
                "Radar Trinh Sát",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void ThucHienPhaoKich(int centerCol, int centerRow)
        {
            _daDungSkillBomb = true;
            _kyNangHienTai = CheDoKyNang.KhongCo;
            btnSkillCrossBomb.Enabled = false;
            btnSkillCrossBomb.BackColor = UITheme.PanelBackground;
            btnSkillCrossBomb.ForeColor = UITheme.MissGray;

            cAmThanh.PhatAirstrike();

            Point[] mucTieu = new Point[]
            {
                new Point(centerCol, centerRow),
                new Point(centerCol - 1, centerRow),
                new Point(centerCol + 1, centerRow),
                new Point(centerCol, centerRow - 1),
                new Point(centerCol, centerRow + 1)
            };

            int soPhatTrungPhao = 0;
            foreach (var pt in mucTieu)
            {
                if (pt.X >= 0 && pt.X < 10 && pt.Y >= 0 && pt.Y < 10)
                {
                    if (_maTranRadar[pt.Y, pt.X] == 0)
                    {
                        _tongPhatBan++;
                        KetQuaBan kq = _banCoDoiThu.TiepNhanPhatBan(pt, out cTau tauBiBan);
                        var b = _btnRadarGrid[pt.Y, pt.X];

                        if (kq == KetQuaBan.Hit)
                        {
                            _soPhatTrung++;
                            soPhatTrungPhao++;
                            _maTranRadar[pt.Y, pt.X] = 2;
                            XuLyDoHoa_OTrung(b);
                        }
                        else if (kq == KetQuaBan.Sunk)
                        {
                            _soPhatTrung++;
                            soPhatTrungPhao++;
                            _maTranRadar[pt.Y, pt.X] = 2;
                            XuLyDoHoa_OTrung(b);

                            if (tauBiBan != null)
                            {
                                var buttonsTau = new List<Button>();
                                foreach (Point p in tauBiBan.DanhSachToaDo)
                                {
                                    if (p.Y >= 0 && p.Y < 10 && p.X >= 0 && p.X < 10)
                                    {
                                        buttonsTau.Add(_btnRadarGrid[p.Y, p.X]);
                                    }
                                }
                                XuLyDoHoa_TauChim(buttonsTau);
                            }
                        }
                        else
                        {
                            _maTranRadar[pt.Y, pt.X] = 3;
                            XuLyDoHoa_OTruot(b);
                        }
                    }
                }
            }

            lblShotCounts.Text = $"Phát bắn: {_tongPhatBan} (Trúng: {_soPhatTrung} • Trượt: {_tongPhatBan - _soPhatTrung})";
            double tiLe = _tongPhatBan > 0 ? (_soPhatTrung * 100.0 / _tongPhatBan) : 0;
            lblAccuracy.Text = $"Độ chính xác: {tiLe:F1}%";

            lblTurnMessage.Text = $"💥 Pháo kích diện rộng hoàn tất! Đánh trúng {soPhatTrungPhao} mục tiêu.";
            lblTurnMessage.ForeColor = UITheme.DangerRed;

            if (_banCoDoiThu.TatCaTauDaChim)
            {
                XuLyKetThucTran(true);
            }
            else if (_cheDoChoi.StartsWith("PvE", StringComparison.OrdinalIgnoreCase))
            {
                MayBanTra();
            }
        }

        private void btnPauseGame_Click(object? sender, EventArgs e)
        {
            if (_giaiDoanXepTau || _tranDauKetThuc) return;

            bool dangChay = tmrMatch.Enabled;
            bool tiepTuc = !dangChay;

            tmrMatch.Enabled = tiepTuc;
            tmrTurnCountdown.Enabled = tiepTuc;
            pnlRadarBoard.Enabled = tiepTuc;

            btnPauseGame.Text = tiepTuc ? "⏸ Tạm dừng" : "▶ Tiếp tục";
            lblTurnMessage.Text = tiepTuc
                ? "▶ Trận đấu tiếp tục!"
                : "⏸ Trận đấu đang tạm dừng. Hãy bấm 'Tiếp tục' để chơi tiếp.";
            lblTurnMessage.ForeColor = tiepTuc ? UITheme.AccentNeon : UITheme.WarningYellow;
        }

        private void btnSurrender_Click(object? sender, EventArgs e)
        {
            var res = MessageBox.Show("Đồng chí có chắc chắn muốn đầu hàng?", "Xác nhận đầu hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                cAmThanh.DungNhacNen();
                tmrMatch.Stop();
                tmrTurnCountdown.Stop();
                XuLyKetThucTran(false);
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
            if (_giaiDoanXepTau) return;

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
                lblTurnMessage.Text = "⏰ Hết 30 giây! Bạn bị mất lượt.";
                lblTurnMessage.ForeColor = UITheme.DangerRed;

                if (_cheDoChoi.StartsWith("PvE", StringComparison.OrdinalIgnoreCase))
                {
                    MayBanTra();
                }
                else
                {
                    if (pnlFogOverlay != null)
                    {
                        KichHoatManCheSuongMu(2);
                        lblFogTitle.Text = "⏰ HẾT THỜI GIAN LƯỢT BẮN!";
                        lblFogMessage.Text = "Đã hết 30 giây! Hãy chuyển lượt cho đối thủ tiếp tục.";
                    }
                }
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
