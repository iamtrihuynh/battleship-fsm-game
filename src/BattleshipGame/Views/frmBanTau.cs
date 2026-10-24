#nullable enable
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
        private string _tenNguoiChoi = "Người chơi";
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
        private readonly cMayBanAI _botAI;
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
        private TacticalVolumeSlider? _sliderVolumeInMatch;
        private int _tongPhatBan = 0;
        private int _soPhatTrung = 0;

        // Danh sách 5 chiến hạm quy chuẩn theo tài liệu đề tài
        private readonly List<ThongTinTauUI> _danhSachTauUI = new List<ThongTinTauUI>
        {
            new ThongTinTauUI { TenTau = "Tàu sân bay (5 ô)", DoDai = 5 },
            new ThongTinTauUI { TenTau = "Thiết giáp hạm (4 ô)", DoDai = 4 },
            new ThongTinTauUI { TenTau = "Tàu khu trục (3 ô)", DoDai = 3 },
            new ThongTinTauUI { TenTau = "Tàu ngầm (3 ô)", DoDai = 3 },
            new ThongTinTauUI { TenTau = "Tàu tuần tra (2 ô)", DoDai = 2 }
        };

        public frmBanTau(string cheDoChoi = "PvE - Dễ", string tenNguoiChoi = "Người chơi")
        {
            InitializeComponent();
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1320, 640);
            MinimumSize = new Size(1320, 640);
            Text = "Bắn Tàu";

            _cheDoChoi = cheDoChoi;
            _tenNguoiChoi = string.IsNullOrWhiteSpace(tenNguoiChoi) ? "Người chơi" : tenNguoiChoi.Trim();
            _botAI = new cMayBanAI(_cheDoChoi.IndexOf("Dễ", StringComparison.OrdinalIgnoreCase) >= 0 ? DoKhoAI.De : DoKhoAI.Kho, 10);
            KhoiTaoThongTinChung();

            _sliderVolumeInMatch = new TacticalVolumeSlider
            {
                Size = new Size(160, 34),
                Top = 11
            };
            pnlTopBar.Controls.Add(_sliderVolumeInMatch);

            TacticalVolumeSlider.GlobalVolumeChanged += OnGlobalVolumeChanged;

            Shown += (s, e) =>
            {
                BringToFront();
                Activate();
                CanChinhGiaoDien();
                if (cAmThanh.DaBatAmThanh && TacticalVolumeSlider.IsSoundActive)
                {
                    cAmThanh.PhatNhacNen("bgm_battle.wav");
                }
            };
        }

        /// <summary>
        /// Sự kiện Form_Load: Gọi hàm sinh động 10x10=100 Button cho 2 bàn cờ theo đúng yêu cầu Ngày 2.
        /// </summary>
        private void frmBanTau_Load(object? sender, EventArgs e)
        {
            SinhBanCoDong();
            CapNhatGiaoDienKhoTau();
            CapNhatThongKeTranDau();
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

        public void XuLyDoHoa_ONuoc(Button btn)
        {
            if (btn == null) return;
            btn.BackColor = UITheme.WaterCell;
            btn.FlatAppearance.BorderColor = UITheme.BorderDefault;
            btn.FlatAppearance.BorderSize = 1;
            btn.ForeColor = Color.White;
            btn.Text = "";
            btn.Cursor = Cursors.Hand;
        }

        public void XuLyDoHoa_OTau(Button btn)
        {
            if (btn == null) return;
            btn.BackColor = UITheme.ShipCell;
            btn.FlatAppearance.BorderColor = UITheme.PrimaryCyan;
            btn.FlatAppearance.BorderSize = 1;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btn.Text = "⚓";
            btn.Cursor = Cursors.Default;
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
            if (!_giaiDoanXepTau) return;
            DoiHuongTau();
            if (_oDangHover.X >= 0 && _oDangHover.Y >= 0)
            {
                HienThiPreviewShadow(_oDangHover.X, _oDangHover.Y);
            }
        }

        private void DoiHuongTau()
        {
            _huongNgang = !_huongNgang;
            btnRotateShip.Text = $"🔄 Xoay hướng (R): {(_huongNgang ? "Ngang" : "Dọc")}";
            btnRotateShip.ForeColor = _huongNgang ? UITheme.PrimaryCyan : UITheme.AccentNeon;
        }
        #endregion

        #region THAO TÁC ĐẶT TÀU & ĐIỀU KHIỂN KHO TÀU
        private void btnPlayerCell_Click(object? sender, EventArgs e)
        {
            if (!_giaiDoanXepTau) return;
            if (sender is not Button btn || btn.Tag is not Point pt) return;

            try
            {
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
            cBanCo banCoNha = _banCoTa;
            if (shipIdx >= 0 && shipIdx < banCoNha.DanhSachTau.Count)
            {
                var tauModel = banCoNha.DanhSachTau[shipIdx];
                banCoNha.DatTau(tauModel, new Point(col, row), _huongNgang ? HuongTau.Ngang : HuongTau.Doc);
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
            CapNhatThongKeTranDau();

            if (soTauDaDat == 5)
            {
                btnStartBattle.Enabled = true;
                btnStartBattle.BackColor = UITheme.AccentNeon;
                btnStartBattle.ForeColor = UITheme.HeaderBackground;
                lblTurnIndicator.Text = "Hạm đội sẵn sàng";
                lblTurnIndicator.ForeColor = UITheme.AccentNeon;
                lblTurnMessage.Text = "Đã xếp đủ 5 tàu. Nhấn 'Vào trận' để bắt đầu!";
                lblTurnMessage.ForeColor = UITheme.AccentNeon;
            }
            else
            {
                lblTurnMessage.Text = $"Đã đặt {tauDangChon.TenTau}. Tiến độ: {soTauDaDat}/5 tàu.";
                lblTurnMessage.ForeColor = UITheme.SuccessGreen;
            }
            }
            catch (Exception ex)
            {
                TacticalMessageBox.ShowWarning(this, "Lỗi đặt tàu", "Đã xảy ra sự cố khi đặt tàu: " + ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật hiển thị trạng thái hạm đội và số liệu bắn trên bảng thống kê tác chiến.
        /// </summary>
        private void CapNhatThongKeTranDau()
        {
            if (lblPlayerFleetStatus == null || lblEnemyFleetStatus == null || lblShotCounts == null || lblAccuracy == null) return;

            if (_giaiDoanXepTau)
            {
                int soTauDaDat = _danhSachTauUI.Count(t => t.DaDat);
                int tongSoO = _danhSachTauUI.Where(t => t.DaDat).Sum(t => t.DoDai);
                lblPlayerFleetStatus.Text = $"🛡️ Bạn: {soTauDaDat}/5 tàu ({tongSoO} ô)";
                lblEnemyFleetStatus.Text = "🤖 Máy: 5/5 tàu (17 ô)";
                lblShotCounts.Text = "🎯 Khai hỏa: 0  |  Trúng: 0  |  Trượt: 0";
                lblAccuracy.Text = "⚡ Độ chính xác: 0.0%";
                return;
            }

            int tauTaCon = _banCoTa.DanhSachTau.Count(t => !t.DaChim);
            int mauTaCon = _banCoTa.DanhSachTau.Sum(t => t.SoOConLai);
            int tauMayCon = _banCoDoiThu.DanhSachTau.Count(t => !t.DaChim);
            int mauMayCon = _banCoDoiThu.DanhSachTau.Sum(t => t.SoOConLai);

            lblPlayerFleetStatus.Text = $"🛡️ Bạn: {tauTaCon}/5 tàu ({mauTaCon} ô)";
            lblEnemyFleetStatus.Text = $"🤖 Máy: {tauMayCon}/5 tàu ({mauMayCon} ô)";

            int soTruot = Math.Max(0, _tongPhatBan - _soPhatTrung);
            lblShotCounts.Text = $"🎯 Bắn: {_tongPhatBan} phát  |  Trúng: {_soPhatTrung}  |  Trượt: {soTruot}";

            double tiLe = _tongPhatBan > 0 ? (_soPhatTrung * 100.0 / _tongPhatBan) : 0;
            lblAccuracy.Text = $"⚡ Độ chính xác: {tiLe:F1}%";
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

            cBanCo banCoNha = _banCoTa;
            banCoNha.XoaTatCaTau();

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
            btnSkillRadar.Text = "📡 Radar trinh sát (3x3)";
            btnSkillCrossBomb.Enabled = true;
            btnSkillCrossBomb.BackColor = UITheme.PanelBackground;
            btnSkillCrossBomb.ForeColor = UITheme.AccentNeon;
            btnSkillCrossBomb.Text = "💥 Pháo kích diện rộng (+)";

            _huongNgang = true;
            btnRotateShip.Text = "🔄 Xoay hướng (R): Ngang";
            btnRotateShip.ForeColor = UITheme.PrimaryCyan;

            CapNhatThongKeTranDau();
            lblTurnIndicator.Text = "Bố trí hạm đội";
            lblTurnIndicator.ForeColor = UITheme.PrimaryCyan;
            lblTurnMessage.Text = "Đã thu hồi hạm đội. Nhấn chuột để đặt tàu hoặc xếp tự động.";
            lblTurnMessage.ForeColor = UITheme.TextSecondary;
        }

        /// <summary>
        /// Nút Xếp tự động: Sinh tự động 5 tàu hợp lệ trên bàn cờ.
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

            cBanCo banCoNha = _banCoTa;
            bool ok = banCoNha.TuDongSinhBanCoNgauNhien();
            if (ok)
            {
                for (int r = 0; r < 10; r++)
                {
                    for (int c = 0; c < 10; c++)
                    {
                        _maTranDoiNha[r, c] = banCoNha.MaTranO[c, r].CoTau ? 1 : 0;
                    }
                }

                for (int i = 0; i < banCoNha.DanhSachTau.Count && i < _danhSachTauUI.Count; i++)
                {
                    var tauModel = banCoNha.DanhSachTau[i];
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

                CapNhatThongKeTranDau();
                lblTurnIndicator.Text = "Hạm đội sẵn sàng";
                lblTurnIndicator.ForeColor = UITheme.AccentNeon;
                lblTurnMessage.Text = "Đã xếp tự động 5 tàu. Nhấn 'Vào trận' để bắt đầu!";
                lblTurnMessage.ForeColor = UITheme.AccentNeon;
            }
        }

        private void btnStartBattle_Click(object? sender, EventArgs e)
        {
            if (_danhSachTauUI.Count(t => t.DaDat) < 5)
            {
                TacticalMessageBox.ShowWarning(
                    this,
                    "Chưa đủ tàu",
                    "Vui lòng bố trí đủ 5 tàu trước khi vào trận!"
                );
                return;
            }

            cBanCo banCoNha = _banCoTa;

            // Đồng bộ an toàn lần cuối giữa UI và model lõi
            for (int i = 0; i < _danhSachTauUI.Count && i < banCoNha.DanhSachTau.Count; i++)
            {
                var uiShip = _danhSachTauUI[i];
                var modelShip = banCoNha.DanhSachTau[i];
                if (uiShip.DaDat && uiShip.CacViTri.Count > 0 && !modelShip.DaDatXong)
                {
                    Point goc = uiShip.CacViTri[0];
                    HuongTau huong = (uiShip.CacViTri.Count > 1 && uiShip.CacViTri[1].Y != uiShip.CacViTri[0].Y)
                        ? HuongTau.Doc : HuongTau.Ngang;
                    banCoNha.DatTau(modelShip, goc, huong);
                }
            }

            _giaiDoanXepTau = false;
            pnlPlacementControls.Visible = false;
            pnlCombatControls.Visible = true;
            pnlCombatControls.Enabled = true;

            // Sinh ngẫu nhiên hạm đội đối thủ (Bot)
            _banCoDoiThu.TuDongSinhBanCoNgauNhien();
            _botAI.KhoiTao();
            _cacOBotDaBan.Clear();

            // Cập nhật thống kê khi bắt đầu trận chiến
            CapNhatThongKeTranDau();

            // Reset các kỹ năng tác chiến về sẵn sàng
            _daDungSkillRadar = false;
            _daDungSkillBomb = false;
            _kyNangHienTai = CheDoKyNang.KhongCo;
            btnSkillRadar.Enabled = true;
            btnSkillRadar.BaseFillColor = UITheme.PanelBackground;
            btnSkillRadar.BorderColor = UITheme.BorderLight;
            btnSkillRadar.NormalTextColor = UITheme.PrimaryCyan;
            btnSkillRadar.Text = "📡 Radar trinh sát (3x3)";
            btnSkillRadar.Invalidate();

            btnSkillCrossBomb.Enabled = true;
            btnSkillCrossBomb.BaseFillColor = UITheme.PanelBackground;
            btnSkillCrossBomb.BorderColor = UITheme.BorderLight;
            btnSkillCrossBomb.NormalTextColor = UITheme.AccentNeon;
            btnSkillCrossBomb.Text = "💥 Pháo kích diện rộng (+)";
            btnSkillCrossBomb.Invalidate();

            // Nhạc nền trận chiến: Tự động điều chỉnh về mức nền êm ái (Tactical Ambient ~25%) để không át tiếng đạn pháo
            if (TacticalVolumeSlider.GlobalVolume > 30)
            {
                TacticalVolumeSlider.SetGlobalState(25, false, this);
            }
            cAmThanh.PhatNhacNen("bgm_battle.wav");

            // Khởi động đồng hồ trận đấu
            _turnSeconds = 30;
            tmrMatch.Start();
            tmrTurnCountdown.Start();

            lblTurnIndicator.Text = "Lượt của bạn";
            lblTurnIndicator.ForeColor = UITheme.AccentNeon;
            lblTurnMessage.Text = "Nhấn ô trên radar đối thủ để khai hỏa!";
            lblTurnMessage.ForeColor = UITheme.AccentNeon;
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
                lblTurnMessage.Text = $"Tọa độ ngắm bắn: {((char)('A' + row))}{col + 1}";
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
                lblTurnMessage.Text = $"Tọa độ {((char)('A' + row))}{col + 1} đã được bắn! Hãy chọn tọa độ khác.";
                lblTurnMessage.ForeColor = UITheme.DangerRed;
                return;
            }

            _dangXuLyBan = true;
            try
            {
                _tongPhatBan++;
                _turnSeconds = 30; // reset countdown lượt

                // Phát tiếng đại bác
                cAmThanh.PhatShot();

                // Tiếp nhận phát bắn từ bàn cờ đối thủ
                KetQuaBan kq = _banCoDoiThu.TiepNhanPhatBan(new Point(col, row), out cTau tauBiBan);

                if (kq == KetQuaBan.Hit)
                {
                    _soPhatTrung++;
                    _maTranRadar[row, col] = 2; // Trúng
                    XuLyDoHoa_OTrung(btn);

                    cAmThanh.PhatHit();
                    string tenTau = tauBiBan != null ? tauBiBan.TenTau : "Chiến hạm";
                    lblTurnMessage.Text = $"Bắn trúng {tenTau}! Bạn được thưởng thêm 1 lượt!";
                    lblTurnMessage.ForeColor = UITheme.HitFireBackground;
                }
                else if (kq == KetQuaBan.Sunk)
                {
                    _soPhatTrung++;
                    _maTranRadar[row, col] = 2;
                    XuLyDoHoa_OTrung(btn);

                    // Cập nhật toàn bộ ô của con tàu đã chìm
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
                    lblTurnMessage.Text = $"Bắn chìm {tenTau}! Bạn được bắn tiếp!";
                    lblTurnMessage.ForeColor = UITheme.HitFireBackground;

                    // Kiểm tra chiến thắng
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
                    lblTurnMessage.Text = $"Bắn trượt tại {((char)('A' + row))}{col + 1}! Đổi lượt cho máy.";
                    lblTurnMessage.ForeColor = UITheme.MissRippleText;

                    MayBanTra();
                }

                // Cập nhật thống kê
                CapNhatThongKeTranDau();
            }
            catch (Exception ex)
            {
                TacticalMessageBox.ShowWarning(this, "Lỗi phát bắn", "Đã xảy ra sự cố trong quá trình xử lý phát bắn: " + ex.Message);
            }
            finally
            {
                _dangXuLyBan = false;
            }
        }

        private void MayBanTra()
        {
            if (_banCoTa.TatCaTauDaChim || _tranDauKetThuc) return;

            const int maxRetries = 20;
            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                Point banO = _botAI.ChonToaDoBan();
                if (banO.X < 0 || banO.Y < 0 || banO.X >= 10 || banO.Y >= 10) return;

                _cacOBotDaBan.Add(banO);

                KetQuaBan kq = _banCoTa.TiepNhanPhatBan(banO, out cTau? tauBiBan);
                _botAI.CapNhatKetQua(banO, kq, tauBiBan);
                var btnO = _btnPlayerGrid[banO.Y, banO.X];
                CapNhatThongKeTranDau();

                if (kq == KetQuaBan.Hit || kq == KetQuaBan.Sunk)
                {
                    XuLyDoHoa_OTrung(btnO);
                    if (kq == KetQuaBan.Sunk && tauBiBan != null)
                    {
                        cAmThanh.PhatSunk();
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
                    else
                    {
                        cAmThanh.PhatHit();
                    }

                    if (_banCoTa.TatCaTauDaChim)
                    {
                        XuLyKetThucTran(false);
                        return;
                    }

                    MayBanTra();
                    return;
                }
                else if (kq == KetQuaBan.Miss)
                {
                    XuLyDoHoa_OTruot(btnO);
                    cAmThanh.PhatMiss();
                    return;
                }
            }
        }

        /// <summary>
        /// Khởi động lại trận đấu (Chơi lại ván mới) sau khi kết thúc ván đấu
        /// </summary>
        public void KhoiDongLaiTranDau()
        {
            tmrMatch.Stop();
            tmrTurnCountdown.Stop();
            _matchSeconds = 0;
            _turnSeconds = 30;
            lblMatchTimer.Text = "⏱ Trận: 00:00";
            lblTurnTimer.Text = "⏳ Lượt: 30s";
            prgTurnTime.Value = 30;

            _tranDauKetThuc = false;
            _dangXuLyBan = false;
            _giaiDoanXepTau = true;

            _tongPhatBan = 0;
            _soPhatTrung = 0;

            pnlCombatControls.Visible = false;
            pnlPlacementControls.Visible = true;
            pnlPlacementControls.Enabled = true;
            pnlRadarBoard.Enabled = true;

            // Reset trạng thái kỹ năng chiến thuật
            _daDungSkillRadar = false;
            _daDungSkillBomb = false;
            _kyNangHienTai = CheDoKyNang.KhongCo;
            btnSkillRadar.Enabled = true;
            btnSkillRadar.BaseFillColor = UITheme.PanelBackground;
            btnSkillRadar.BorderColor = UITheme.BorderLight;
            btnSkillRadar.NormalTextColor = UITheme.PrimaryCyan;
            btnSkillRadar.Text = "📡 Radar trinh sát (3x3)";
            btnSkillRadar.Invalidate();

            btnSkillCrossBomb.Enabled = true;
            btnSkillCrossBomb.BaseFillColor = UITheme.PanelBackground;
            btnSkillCrossBomb.BorderColor = UITheme.BorderLight;
            btnSkillCrossBomb.NormalTextColor = UITheme.AccentNeon;
            btnSkillCrossBomb.Text = "💥 Pháo kích diện rộng (+)";
            btnSkillCrossBomb.Invalidate();

            _banCoTa.XoaTatCaTau();
            _banCoDoiThu.XoaTatCaTau();
            _botAI.KhoiTao();
            _cacOBotDaBan.Clear();

            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 10; c++)
                {
                    _maTranDoiNha[r, c] = 0;
                    _maTranRadar[r, c] = 0;
                }
            }

            SinhBanCoDong();
            CapNhatGiaoDienKhoTau();

            btnResetBoard_Click(this, EventArgs.Empty);

            cAmThanh.DungNhacNen();
            cAmThanh.PhatNhacNen("bgm_battle.wav");
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
            int diem = nguoiChoiThang ? cThanhTichDAO.TinhDiem(true, soTauConSong, _tongPhatBan, _matchSeconds) : 0;

            if (nguoiChoiThang)
            {
                cAmThanh.PhatVictory();
                lblTurnIndicator.Text = "Chiến thắng!";
                lblTurnIndicator.ForeColor = UITheme.AccentNeon;
                lblTurnMessage.Text = $"Chúc mừng {_tenNguoiChoi}! Bạn đã tiêu diệt toàn bộ hạm đội địch!";
            }
            else
            {
                cAmThanh.PhatDefeat();
                lblTurnIndicator.Text = "Hạm đội bị tiêu diệt!";
                lblTurnIndicator.ForeColor = UITheme.DangerRed;
                lblTurnMessage.Text = $"Hạm đội của {_tenNguoiChoi} đã chìm hoàn toàn. Bạn đã thất bại!";
            }

            CapNhatThongKeTranDau();

            // Hiển thị giao diện kết quả trận đấu đồ họa cao cấp
            var action = frmKetQuaTranDau.HienThi(
                this,
                nguoiChoiThang,
                _tenNguoiChoi,
                diem,
                _tongPhatBan,
                _soPhatTrung,
                _matchSeconds,
                soTauConSong,
                _cheDoChoi,
                out string tenMoi
            );

            if (!string.IsNullOrWhiteSpace(tenMoi))
            {
                _tenNguoiChoi = tenMoi;
            }

            if (action == KetQuaTranAction.ChoiLai)
            {
                KhoiDongLaiTranDau();
            }
            else if (action == KetQuaTranAction.VeMenu)
            {
                cAmThanh.DungNhacNen();
                Close();
            }
        }
        #endregion

        #region BỘ ĐIỀU KHIỂN & ĐỒNG HỒ
        private void KhoiTaoThongTinChung()
        {
            lblMatchModeInfo.Text = $"Chế độ: Đấu với máy ({_cheDoChoi}) • Đô đốc: {_tenNguoiChoi} • Bàn cờ 10x10";
            lblLeftBoardTitle.Text = $"Vùng biển của {_tenNguoiChoi} (Hạm đội ta)";
            lblRightBoardTitle.Text = "Radar tác chiến (Vùng biển quân địch)";
            btnStartBattle.Text = "Vào trận";
            CapNhatNutAmThanh();
        }

        private void CapNhatNutAmThanh()
        {
            bool bat = cAmThanh.DaBatAmThanh && TacticalVolumeSlider.IsSoundActive;
            btnToggleSound.Text = bat ? "🔊 Âm thanh" : "🔇 Tắt tiếng";
            btnToggleSound.ForeColor = bat ? UITheme.AccentNeon : UITheme.MissGray;
            btnToggleSound.NormalTextColor = bat ? UITheme.AccentNeon : UITheme.MissGray;
            btnToggleSound.BorderColor = bat ? UITheme.AccentNeon : UITheme.BorderDefault;
            btnToggleSound.BaseFillColor = Color.FromArgb(15, 23, 42);
            btnToggleSound.Invalidate();
        }

        private void OnGlobalVolumeChanged(object? sender, EventArgs e)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(CapNhatNutAmThanh));
                }
                else
                {
                    CapNhatNutAmThanh();
                }
            }
            catch { }
        }

        private void btnToggleSound_Click(object? sender, EventArgs e)
        {
            try
            {
                TacticalVolumeSlider.ToggleGlobalMute(this);
                CapNhatNutAmThanh();
            }
            catch { }
        }

        private void btnBackToMenu_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void btnSkillRadar_Click(object? sender, EventArgs e)
        {
            if (_giaiDoanXepTau || _tranDauKetThuc) return;

            if (_daDungSkillRadar)
            {
                TacticalMessageBox.ShowWarning(this, "Kỹ năng tác chiến", "Kỹ năng Radar trinh sát 3x3 đã được sử dụng trong trận này!");
                return;
            }

            if (_kyNangHienTai == CheDoKyNang.RadarTrinhSat)
            {
                _kyNangHienTai = CheDoKyNang.KhongCo;
                btnSkillRadar.BaseFillColor = UITheme.PanelBackground;
                btnSkillRadar.BorderColor = UITheme.BorderLight;
                btnSkillRadar.NormalTextColor = UITheme.PrimaryCyan;
                btnSkillRadar.Text = "📡 Radar trinh sát (3x3)";
                btnSkillRadar.Invalidate();

                lblTurnMessage.Text = "Đã hủy kỹ năng. Đến lượt bạn khai hỏa.";
                lblTurnMessage.ForeColor = UITheme.TextSecondary;
            }
            else
            {
                _kyNangHienTai = CheDoKyNang.RadarTrinhSat;
                btnSkillRadar.BaseFillColor = Color.FromArgb(20, 55, 80);
                btnSkillRadar.BorderColor = UITheme.PrimaryCyan;
                btnSkillRadar.NormalTextColor = Color.FromArgb(224, 242, 254);
                btnSkillRadar.Text = "📡 [BẬT] Radar (3x3)";
                btnSkillRadar.Invalidate();

                if (!_daDungSkillBomb)
                {
                    btnSkillCrossBomb.BaseFillColor = UITheme.PanelBackground;
                    btnSkillCrossBomb.BorderColor = UITheme.BorderLight;
                    btnSkillCrossBomb.NormalTextColor = UITheme.AccentNeon;
                    btnSkillCrossBomb.Text = "💥 Pháo kích diện rộng (+)";
                    btnSkillCrossBomb.Invalidate();
                }

                lblTurnMessage.Text = "📡 Chế độ Radar 3x3: Nhấn 1 ô trên bàn cờ đối thủ để quét.";
                lblTurnMessage.ForeColor = UITheme.AccentNeon;
            }
        }

        private void btnSkillCrossBomb_Click(object? sender, EventArgs e)
        {
            if (_giaiDoanXepTau || _tranDauKetThuc) return;

            if (_daDungSkillBomb)
            {
                TacticalMessageBox.ShowWarning(this, "Kỹ năng tác chiến", "Kỹ năng Pháo kích diện rộng (+) đã được sử dụng trong trận này!");
                return;
            }

            if (_kyNangHienTai == CheDoKyNang.PhaoKichDienRong)
            {
                _kyNangHienTai = CheDoKyNang.KhongCo;
                btnSkillCrossBomb.BaseFillColor = UITheme.PanelBackground;
                btnSkillCrossBomb.BorderColor = UITheme.BorderLight;
                btnSkillCrossBomb.NormalTextColor = UITheme.AccentNeon;
                btnSkillCrossBomb.Text = "💥 Pháo kích diện rộng (+)";
                btnSkillCrossBomb.Invalidate();

                lblTurnMessage.Text = "Đã hủy kỹ năng. Đến lượt bạn khai hỏa.";
                lblTurnMessage.ForeColor = UITheme.TextSecondary;
            }
            else
            {
                _kyNangHienTai = CheDoKyNang.PhaoKichDienRong;
                btnSkillCrossBomb.BaseFillColor = Color.FromArgb(85, 28, 28);
                btnSkillCrossBomb.BorderColor = UITheme.DangerRed;
                btnSkillCrossBomb.NormalTextColor = Color.FromArgb(254, 226, 226);
                btnSkillCrossBomb.Text = "💥 [BẬT] Pháo kích (+)";
                btnSkillCrossBomb.Invalidate();

                if (!_daDungSkillRadar)
                {
                    btnSkillRadar.BaseFillColor = UITheme.PanelBackground;
                    btnSkillRadar.BorderColor = UITheme.BorderLight;
                    btnSkillRadar.NormalTextColor = UITheme.PrimaryCyan;
                    btnSkillRadar.Text = "📡 Radar trinh sát (3x3)";
                    btnSkillRadar.Invalidate();
                }

                lblTurnMessage.Text = "💥 Chế độ Pháo kích (+): Nhấn 1 ô tâm để bắn 5 ô chữ thập.";
                lblTurnMessage.ForeColor = UITheme.DangerRed;
            }
        }

        private void ThucHienQuetRadar(int centerCol, int centerRow)
        {
            _dangXuLyBan = true;
            try
            {
                _daDungSkillRadar = true;
                _kyNangHienTai = CheDoKyNang.KhongCo;
                btnSkillRadar.Enabled = false;
                btnSkillRadar.BaseFillColor = UITheme.PanelBackground;
                btnSkillRadar.BorderColor = UITheme.BorderDefault;
                btnSkillRadar.NormalTextColor = UITheme.MissGray;
                btnSkillRadar.Text = "📡 Radar (Đã dùng)";
                btnSkillRadar.Invalidate();

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

                _tongPhatBan++;
                _turnSeconds = 30;
                CapNhatThongKeTranDau();

                lblTurnMessage.Text = $"Radar quét khu vực {((char)('A' + centerRow))}{centerCol + 1}: Phát hiện {soOTauPhatHien} ô tàu địch. Đổi lượt cho máy.";
                lblTurnMessage.ForeColor = soOTauPhatHien > 0 ? UITheme.AccentNeon : UITheme.TextSecondary;

                TacticalMessageBox.ShowInfo(
                    this,
                    "Radar trinh sát 3x3",
                    $"Kết quả trinh sát quanh {((char)('A' + centerRow))}{centerCol + 1}:\n\n" +
                    $"Số ô tàu phát hiện: {soOTauPhatHien} ô.\n\n" +
                    "Đổi lượt cho máy."
                );

                MayBanTra();
            }
            finally
            {
                _dangXuLyBan = false;
            }
        }

        private void ThucHienPhaoKich(int centerCol, int centerRow)
        {
            Point[] mucTieu = new Point[]
            {
                new Point(centerCol, centerRow),
                new Point(centerCol - 1, centerRow),
                new Point(centerCol + 1, centerRow),
                new Point(centerCol, centerRow - 1),
                new Point(centerCol, centerRow + 1)
            };

            bool coOMucTieuHopLe = mucTieu.Any(pt => pt.X >= 0 && pt.X < 10 && pt.Y >= 0 && pt.Y < 10 && _maTranRadar[pt.Y, pt.X] == 0);
            if (!coOMucTieuHopLe)
            {
                lblTurnMessage.Text = "Toàn bộ ô quanh tọa độ này đều đã bắn! Hãy chọn tọa độ khác.";
                lblTurnMessage.ForeColor = UITheme.DangerRed;
                TacticalMessageBox.ShowWarning(
                    this,
                    "Pháo kích diện rộng",
                    "Toàn bộ các ô trong khu vực này đều đã được bắn trước đó!\nVui lòng chọn tọa độ khác."
                );
                return;
            }

            _dangXuLyBan = true;
            try
            {
                _daDungSkillBomb = true;
                _kyNangHienTai = CheDoKyNang.KhongCo;
                _turnSeconds = 30;
                btnSkillCrossBomb.Enabled = false;
                btnSkillCrossBomb.BaseFillColor = UITheme.PanelBackground;
                btnSkillCrossBomb.BorderColor = UITheme.BorderDefault;
                btnSkillCrossBomb.NormalTextColor = UITheme.MissGray;
                btnSkillCrossBomb.Text = "💥 Pháo kích (Đã dùng)";
                btnSkillCrossBomb.Invalidate();

                cAmThanh.PhatAirstrike();

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

                CapNhatThongKeTranDau();

                lblTurnMessage.Text = $"Pháo kích hoàn tất! Trúng {soPhatTrungPhao} mục tiêu.";
                lblTurnMessage.ForeColor = UITheme.DangerRed;

                if (_banCoDoiThu.TatCaTauDaChim)
                {
                    XuLyKetThucTran(true);
                }
                else
                {
                    MayBanTra();
                }
            }
            finally
            {
                _dangXuLyBan = false;
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
            if (_giaiDoanXepTau || _tranDauKetThuc) return;

            bool wasTurnTimerRunning = tmrTurnCountdown.Enabled;
            bool wasMatchTimerRunning = tmrMatch.Enabled;
            tmrTurnCountdown.Stop();
            tmrMatch.Stop();

            var res = TacticalMessageBox.ShowSurrenderConfirm(this);
            if (res == DialogResult.Yes)
            {
                cAmThanh.DungNhacNen();
                XuLyKetThucTran(false);
                Close();
            }
            else
            {
                if (wasTurnTimerRunning && !_tranDauKetThuc) tmrTurnCountdown.Start();
                if (wasMatchTimerRunning && !_tranDauKetThuc) tmrMatch.Start();
            }
        }

        private void tmrMatch_Tick(object? sender, EventArgs e)
        {
            try
            {
                _matchSeconds++;
                int m = _matchSeconds / 60;
                int s = _matchSeconds % 60;
                lblMatchTimer.Text = $"⏱ Trận: {m:D2}:{s:D2}";
            }
            catch { }
        }

        private void tmrTurnCountdown_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (_giaiDoanXepTau || _tranDauKetThuc || _dangXuLyBan) return;

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
                    _kyNangHienTai = CheDoKyNang.KhongCo;
                    lblTurnMessage.Text = "Hết 30 giây! Bạn bị mất lượt.";
                    lblTurnMessage.ForeColor = UITheme.DangerRed;

                    MayBanTra();
                }
            }
            catch
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

            // Neo cụm điều khiển âm lượng ở góc phải thanh trên cùng, căn giữa theo chiều dọc
            btnToggleSound.Height = 34;
            btnToggleSound.Width = 110;
            btnToggleSound.Top = 11;

            if (_sliderVolumeInMatch != null)
            {
                _sliderVolumeInMatch.Height = 34;
                _sliderVolumeInMatch.Top = 11;
                _sliderVolumeInMatch.Left = pnlTopBar.Width - _sliderVolumeInMatch.Width - 16;
                btnToggleSound.Left = _sliderVolumeInMatch.Left - btnToggleSound.Width - 10;
            }
            else
            {
                btnToggleSound.Left = pnlTopBar.Width - btnToggleSound.Width - 20;
            }

            // Canh giữa 3 khu vực khi mở to / toàn màn hình
            int totalWidth = pnlLeftBoardContainer.Width + 14 + pnlCenterDashboard.Width + 14 + pnlRightBoardContainer.Width;
            int startX = Math.Max(16, (ClientSize.Width - totalWidth) / 2);

            pnlLeftBoardContainer.Left = startX;
            pnlCenterDashboard.Left = startX + pnlLeftBoardContainer.Width + 14;
            pnlRightBoardContainer.Left = pnlCenterDashboard.Right + 14;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_giaiDoanXepTau && !_tranDauKetThuc && e.CloseReason == CloseReason.UserClosing)
            {
                bool wasTurnRunning = tmrTurnCountdown?.Enabled ?? false;
                bool wasMatchRunning = tmrMatch?.Enabled ?? false;
                tmrTurnCountdown?.Stop();
                tmrMatch?.Stop();

                var res = TacticalMessageBox.ShowConfirm(
                    this,
                    "TẠM DỪNG / THOÁT TRẬN ĐẤU",
                    "Trận đấu đang diễn ra. Bạn có chắc chắn muốn thoát về Menu chính không?"
                );
                if (res != DialogResult.Yes)
                {
                    e.Cancel = true;
                    if (wasTurnRunning && !_tranDauKetThuc) tmrTurnCountdown?.Start();
                    if (wasMatchRunning && !_tranDauKetThuc) tmrMatch?.Start();
                    return;
                }
            }

            try
            {
                TacticalVolumeSlider.GlobalVolumeChanged -= OnGlobalVolumeChanged;
                tmrMatch?.Stop();
                tmrTurnCountdown?.Stop();
                cAmThanh.DungSFX();
                cAmThanh.DungNhacNen();
            }
            catch { }
            base.OnFormClosing(e);
        }
        #endregion
    }
}
