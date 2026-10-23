#nullable enable
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BattleshipGame.DataAccess;

namespace BattleshipGame.Views
{
    public enum KetQuaTranAction
    {
        Dong,
        ChoiLai,
        VeMenu
    }

    /// <summary>
    /// Hộp thoại hiển thị kết quả trận đấu (Chiến thắng / Thất bại) phong cách Hải quân hiện đại.
    /// Thiết kế đối xứng, cân đối trung tâm, đường nét sắc sảo và thanh lịch.
    /// </summary>
    public class frmKetQuaTranDau : Form
    {
        private readonly bool _thang;
        private string _tenNguoiChoi;
        private readonly int _diemSo;
        private readonly int _tongPhatBan;
        private readonly int _soPhatTrung;
        private readonly int _thoiGianGiay;
        private readonly int _soTauTaCon;
        private readonly string _cheDoChoi;

        private bool _daLuuThanhTich = false;
        public KetQuaTranAction UserAction { get; private set; } = KetQuaTranAction.Dong;
        public string TenNguoiChoiMoi => _tenNguoiChoi;

        private TextBox? _txtName;
        private Label? _lblSaveStatus;
        private TacticalButton? _btnSaveRank;

        public frmKetQuaTranDau(
            bool thang,
            string tenNguoiChoi,
            int diemSo,
            int tongPhatBan,
            int soPhatTrung,
            int thoiGianGiay,
            int soTauTaCon,
            string cheDoChoi)
        {
            _thang = thang;
            _tenNguoiChoi = string.IsNullOrWhiteSpace(tenNguoiChoi) ? "Đô đốc" : tenNguoiChoi.Trim();
            _diemSo = diemSo;
            _tongPhatBan = tongPhatBan;
            _soPhatTrung = soPhatTrung;
            _thoiGianGiay = thoiGianGiay;
            _soTauTaCon = soTauTaCon;
            _cheDoChoi = cheDoChoi;

            InitializeForm();
        }

        private void InitializeForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            DoubleBuffered = true;
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint,
                true
            );

            int formWidth = 520;
            int formHeight = _thang ? 520 : 420;
            Size = new Size(formWidth, formHeight);
            BackColor = UITheme.HeaderBackground;
            ForeColor = UITheme.TextPrimary;

            // Bo tròn 4 góc cửa sổ
            Region = new Region(UITheme.CreateRoundedPath(new Rectangle(0, 0, Width, Height), 16));

            // Nút đóng '✕' góc trên phải
            var btnCloseX = new TacticalButton
            {
                Text = "✕",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(Width - 42, 10),
                Size = new Size(32, 28),
                CornerRadius = 4,
                BaseFillColor = Color.Transparent,
                BorderColor = Color.Transparent,
                HoverFillColor = Color.FromArgb(80, 239, 68, 68),
                HoverTextColor = Color.White,
                NormalTextColor = UITheme.TextMuted
            };
            btnCloseX.Click += (s, e) => { UserAction = KetQuaTranAction.Dong; Close(); };
            Controls.Add(btnCloseX);

            int contentWidth = 440;
            int contentX = (Width - contentWidth) / 2; // Canh lề 40px đều hai bên

            // 1. Icon trung tâm
            var lblIcon = new Label
            {
                Text = _thang ? "🏆" : "💀",
                Font = new Font("Segoe UI Emoji", 32f, FontStyle.Regular),
                Location = new Point((Width - 80) / 2, 16),
                Size = new Size(80, 48),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(lblIcon);

            // 2. Tiêu đề chính căn giữa
            var lblMainTitle = new Label
            {
                Text = _thang ? "CHIẾN THẮNG" : "THẤT BẠI",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = _thang ? UITheme.GoldAccent : UITheme.DangerRed,
                Location = new Point(contentX, 66),
                Size = new Size(contentWidth, 32),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(lblMainTitle);

            // 3. Phụ đề căn giữa
            var lblSubtitle = new Label
            {
                Text = _thang
                    ? "Đô đốc đã đánh chìm toàn bộ hạm đội đối phương!"
                    : "Toàn bộ 5 chiến hạm của bạn đã bị bắn chìm!",
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = _thang ? UITheme.AccentNeon : UITheme.TextSecondary,
                Location = new Point(contentX, 98),
                Size = new Size(contentWidth, 22),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(lblSubtitle);

            int currentY = 126;

            // 4. Khối điểm số (Chỉ dành cho Chiến thắng) hoặc Khối chế độ (Thất bại)
            if (_thang)
            {
                var pnlScorePlate = new Panel
                {
                    Location = new Point(contentX, currentY),
                    Size = new Size(contentWidth, 54),
                    BackColor = Color.FromArgb(22, 36, 60)
                };
                pnlScorePlate.Paint += (s, pe) =>
                {
                    var g = pe.Graphics;
                    UITheme.SetHighQuality(g);
                    using var pen = new Pen(UITheme.GoldAccent, 1.2f);
                    using var path = UITheme.CreateRoundedPath(new RectangleF(0.6f, 0.6f, pnlScorePlate.Width - 1.2f, pnlScorePlate.Height - 1.2f), 8);
                    g.DrawPath(pen, path);
                };

                var lblScoreTag = new Label
                {
                    Text = "⭐ ĐIỂM SỐ TÁC CHIẾN",
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    ForeColor = UITheme.GoldAccent,
                    Location = new Point(0, 5),
                    Size = new Size(contentWidth, 16),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pnlScorePlate.Controls.Add(lblScoreTag);

                var lblScoreValue = new Label
                {
                    Text = $"{_diemSo:N0} ĐIỂM",
                    Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                    ForeColor = UITheme.AccentNeon,
                    Location = new Point(0, 22),
                    Size = new Size(contentWidth, 26),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pnlScorePlate.Controls.Add(lblScoreValue);

                Controls.Add(pnlScorePlate);
                currentY += 62;
            }
            else
            {
                var pnlModePlate = new Panel
                {
                    Location = new Point(contentX, currentY),
                    Size = new Size(contentWidth, 34),
                    BackColor = Color.FromArgb(26, 20, 30)
                };
                pnlModePlate.Paint += (s, pe) =>
                {
                    var g = pe.Graphics;
                    UITheme.SetHighQuality(g);
                    using var pen = new Pen(Color.FromArgb(85, 35, 45), 1f);
                    using var path = UITheme.CreateRoundedPath(new RectangleF(0.5f, 0.5f, pnlModePlate.Width - 1f, pnlModePlate.Height - 1f), 6);
                    g.DrawPath(pen, path);
                };

                var lblModeText = new Label
                {
                    Text = $"🎖️ Chế độ: {_cheDoChoi}  •  Đô đốc: {_tenNguoiChoi}",
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(220, 120, 130),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pnlModePlate.Controls.Add(lblModeText);
                Controls.Add(pnlModePlate);
                currentY += 44;
            }

            // 5. Lưới 4 ô thông số chiến thuật (2x2 Grid) cân đối
            int tileW = (contentWidth - 10) / 2; // 215px
            int tileH = _thang ? 56 : 62;

            double tiLeChinhXac = _tongPhatBan > 0 ? (_soPhatTrung * 100.0 / _tongPhatBan) : 0;
            int soTruot = Math.Max(0, _tongPhatBan - _soPhatTrung);
            string thoiGianStr = $"{_thoiGianGiay / 60:D2}:{_thoiGianGiay % 60:D2}";

            // Tile 1: Khai hỏa
            Controls.Add(TaoTileThongSo(
                new Point(contentX, currentY),
                new Size(tileW, tileH),
                "🎯 Khai hỏa",
                $"{_tongPhatBan} phát",
                $"Trúng {_soPhatTrung} • Trượt {soTruot}",
                UITheme.PrimaryCyan
            ));

            // Tile 2: Độ chính xác
            Color accColor = tiLeChinhXac >= 60 ? UITheme.SuccessGreen :
                             tiLeChinhXac >= 40 ? UITheme.WarningAmber : UITheme.TextSecondary;
            Controls.Add(TaoTileThongSo(
                new Point(contentX + tileW + 10, currentY),
                new Size(tileW, tileH),
                "⚡ Độ chính xác",
                $"{tiLeChinhXac:F1}%",
                tiLeChinhXac >= 60 ? "Xạ thủ cừ khôi" : "Hiệu suất bắn",
                accColor
            ));

            // Tile 3: Thời gian
            Controls.Add(TaoTileThongSo(
                new Point(contentX, currentY + tileH + 8),
                new Size(tileW, tileH),
                "⏱️ Thời gian",
                thoiGianStr,
                "Thời gian tác chiến",
                UITheme.AccentNeon
            ));

            // Tile 4: Tàu ta sống sót
            Controls.Add(TaoTileThongSo(
                new Point(contentX + tileW + 10, currentY + tileH + 8),
                new Size(tileW, tileH),
                "🛡️ Chiến hạm ta",
                $"{_soTauTaCon}/5 tàu",
                _thang ? $"{_soTauTaCon} tàu sống sót" : "Đã bị bắn chìm",
                _thang ? UITheme.SuccessGreen : UITheme.DangerRed
            ));

            currentY += (tileH * 2) + 16;

            // 6. Nhập tên ghi danh Bảng Xếp Hạng (CHỈ KHI CHIẾN THẮNG)
            if (_thang)
            {
                var lblInputPrompt = new Label
                {
                    Text = "Tên chỉ huy ghi danh Bảng Xếp Hạng:",
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    ForeColor = UITheme.GoldAccent,
                    Location = new Point(contentX, currentY),
                    Size = new Size(contentWidth, 18),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                Controls.Add(lblInputPrompt);

                var pnlTxtBox = new Panel
                {
                    Location = new Point(contentX, currentY + 22),
                    Size = new Size(contentWidth, 38),
                    BackColor = Color.FromArgb(20, 32, 52)
                };
                pnlTxtBox.Paint += (s, pe) =>
                {
                    var g = pe.Graphics;
                    UITheme.SetHighQuality(g);
                    using var p = new Pen(UITheme.PrimaryCyan, 1.5f);
                    using var path = UITheme.CreateRoundedPath(new RectangleF(0.75f, 0.75f, pnlTxtBox.Width - 1.5f, pnlTxtBox.Height - 1.5f), 6);
                    g.DrawPath(p, path);
                };

                _txtName = new TextBox
                {
                    Text = _tenNguoiChoi,
                    Location = new Point(10, 8),
                    Size = new Size(pnlTxtBox.Width - 20, 22),
                    Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                    BackColor = Color.FromArgb(20, 32, 52),
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    MaxLength = 30
                };
                _txtName.SelectAll();
                pnlTxtBox.Controls.Add(_txtName);
                Controls.Add(pnlTxtBox);

                _lblSaveStatus = new Label
                {
                    Text = "Thành tích sẽ được lưu vào Bảng Xếp Hạng tác chiến.",
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                    ForeColor = UITheme.TextSecondary,
                    Location = new Point(contentX, currentY + 62),
                    Size = new Size(contentWidth, 16),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                Controls.Add(_lblSaveStatus);
            }

            // 7. Cụm nút bấm điều hướng (Bottom Buttons) cân đối, đối xứng
            if (_thang)
            {
                int btnW = 140;
                int btnGap = 10;
                int startBtnX = contentX; // 40px

                _btnSaveRank = new TacticalButton
                {
                    Text = "💾 Lưu & Xem BXH",
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Location = new Point(startBtnX, Height - 58),
                    Size = new Size(btnW, 42),
                    CornerRadius = 6,
                    BorderColor = UITheme.GoldAccent,
                    NormalTextColor = UITheme.GoldAccent,
                    HoverBorderColor = Color.White,
                    HoverTextColor = Color.White
                };
                _btnSaveRank.Click += BtnSaveRank_Click;
                Controls.Add(_btnSaveRank);

                var btnPlayAgain = new TacticalButton
                {
                    Text = "🔄 Chơi lại",
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Location = new Point(startBtnX + btnW + btnGap, Height - 58),
                    Size = new Size(btnW, 42),
                    CornerRadius = 6,
                    BorderColor = UITheme.PrimaryCyan,
                    NormalTextColor = UITheme.PrimaryCyan,
                    HoverBorderColor = Color.White,
                    HoverTextColor = Color.White
                };
                btnPlayAgain.Click += (s, e) =>
                {
                    TuDongLuuNeuChuaLuu();
                    UserAction = KetQuaTranAction.ChoiLai;
                    Close();
                };
                Controls.Add(btnPlayAgain);

                var btnMenu = new TacticalButton
                {
                    Text = "🚪 Về Menu",
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Location = new Point(startBtnX + (btnW + btnGap) * 2, Height - 58),
                    Size = new Size(btnW, 42),
                    CornerRadius = 6,
                    BorderColor = UITheme.BorderDefault,
                    NormalTextColor = UITheme.TextSecondary,
                    HoverBorderColor = Color.White,
                    HoverTextColor = Color.White
                };
                btnMenu.Click += (s, e) =>
                {
                    TuDongLuuNeuChuaLuu();
                    UserAction = KetQuaTranAction.VeMenu;
                    Close();
                };
                Controls.Add(btnMenu);

                AcceptButton = _btnSaveRank;
            }
            else
            {
                int btnW = 210;
                int btnGap = 20;
                int startBtnX = contentX; // 40px

                var btnPlayAgain = new TacticalButton
                {
                    Text = "🔄 Phục thù (Chơi lại)",
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    Location = new Point(startBtnX, Height - 60),
                    Size = new Size(btnW, 44),
                    CornerRadius = 6,
                    BorderColor = UITheme.AccentNeon,
                    NormalTextColor = UITheme.AccentNeon,
                    HoverBorderColor = Color.White,
                    HoverTextColor = Color.White
                };
                btnPlayAgain.Click += (s, e) =>
                {
                    UserAction = KetQuaTranAction.ChoiLai;
                    Close();
                };
                Controls.Add(btnPlayAgain);

                var btnMenu = new TacticalButton
                {
                    Text = "🚪 Về Menu chính",
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    Location = new Point(startBtnX + btnW + btnGap, Height - 60),
                    Size = new Size(btnW, 44),
                    CornerRadius = 6,
                    BorderColor = UITheme.BorderDefault,
                    NormalTextColor = UITheme.TextSecondary,
                    HoverBorderColor = Color.White,
                    HoverTextColor = Color.White
                };
                btnMenu.Click += (s, e) =>
                {
                    UserAction = KetQuaTranAction.VeMenu;
                    Close();
                };
                Controls.Add(btnMenu);

                AcceptButton = btnPlayAgain;
            }

            KeyPreview = true;
            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    UserAction = KetQuaTranAction.Dong;
                    Close();
                }
            };
        }

        private Panel TaoTileThongSo(Point loc, Size sz, string title, string val, string sub, Color accent)
        {
            var pnl = new Panel
            {
                Location = loc,
                Size = sz,
                BackColor = Color.FromArgb(20, 32, 52)
            };
            pnl.Paint += (s, pe) =>
            {
                var g = pe.Graphics;
                UITheme.SetHighQuality(g);
                using var pen = new Pen(Color.FromArgb(45, 65, 95), 1f);
                using var path = UITheme.CreateRoundedPath(new RectangleF(0.5f, 0.5f, pnl.Width - 1f, pnl.Height - 1f), 6);
                g.DrawPath(pen, path);
            };

            var lblT = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = UITheme.TextSecondary,
                Location = new Point(10, 6),
                Size = new Size(sz.Width - 20, 16),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnl.Controls.Add(lblT);

            var lblV = new Label
            {
                Text = val,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = accent,
                Location = new Point(10, 22),
                Size = new Size(sz.Width - 20, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnl.Controls.Add(lblV);

            if (sz.Height >= 58 && !string.IsNullOrEmpty(sub))
            {
                var lblS = new Label
                {
                    Text = sub,
                    Font = new Font("Segoe UI", 8f, FontStyle.Regular),
                    ForeColor = UITheme.TextMuted,
                    Location = new Point(10, 42),
                    Size = new Size(sz.Width - 20, 14),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                pnl.Controls.Add(lblS);
            }

            return pnl;
        }

        private void TuDongLuuNeuChuaLuu()
        {
            if (!_thang || _daLuuThanhTich) return;
            string ten = _txtName != null && !string.IsNullOrWhiteSpace(_txtName.Text)
                ? _txtName.Text.Trim()
                : _tenNguoiChoi;
            _tenNguoiChoi = ten;
            cThanhTichDAO.LuuThanhTich(ten, _diemSo, _tongPhatBan, _thoiGianGiay, _cheDoChoi, "Thắng");
            _daLuuThanhTich = true;
        }

        private void BtnSaveRank_Click(object? sender, EventArgs e)
        {
            if (!_daLuuThanhTich)
            {
                string ten = _txtName != null && !string.IsNullOrWhiteSpace(_txtName.Text)
                    ? _txtName.Text.Trim()
                    : _tenNguoiChoi;
                _tenNguoiChoi = ten;

                bool ok = cThanhTichDAO.LuuThanhTich(ten, _diemSo, _tongPhatBan, _thoiGianGiay, _cheDoChoi, "Thắng");
                _daLuuThanhTich = true;

                if (ok && _lblSaveStatus != null)
                {
                    _lblSaveStatus.Text = "✅ Đã ghi danh thành công vào Bảng Xếp Hạng!";
                    _lblSaveStatus.ForeColor = UITheme.SuccessGreen;
                }
                if (_btnSaveRank != null)
                {
                    _btnSaveRank.Text = "📊 Xem Bảng Xếp Hạng";
                    _btnSaveRank.BorderColor = UITheme.SuccessGreen;
                    _btnSaveRank.NormalTextColor = UITheme.SuccessGreen;
                }
            }

            try
            {
                using var f = new frmThanhTich();
                f.ShowDialog(this);
            }
            catch { }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            UITheme.SetHighQuality(g);

            // 1. Phủ dải màu gradient trên đầu
            Color topGlowColor = _thang ? Color.FromArgb(40, 251, 191, 36) : Color.FromArgb(50, 239, 68, 68);
            using (var headerGrad = new LinearGradientBrush(
                new Point(0, 0),
                new Point(0, 110),
                topGlowColor,
                Color.Transparent))
            {
                g.FillRectangle(headerGrad, 0, 0, Width, 110);
            }

            // 2. Vẽ viền bo tròn phát sáng toàn bộ form
            Color borderColor = _thang ? UITheme.GoldAccent : UITheme.DangerRed;
            using (var borderPen = new Pen(borderColor, 1.8f))
            {
                using (var path = UITheme.CreateRoundedPath(new RectangleF(0.9f, 0.9f, Width - 1.8f, Height - 1.8f), 16))
                {
                    g.DrawPath(borderPen, path);
                }
            }
        }

        /// <summary>
        /// Phương thức tĩnh hiển thị hộp thoại kết quả và trả về hành động của người chơi
        /// </summary>
        public static KetQuaTranAction HienThi(
            IWin32Window owner,
            bool thang,
            string tenNguoiChoi,
            int diemSo,
            int tongPhatBan,
            int soPhatTrung,
            int thoiGianGiay,
            int soTauTaCon,
            string cheDoChoi,
            out string tenMoi)
        {
            using var dlg = new frmKetQuaTranDau(
                thang,
                tenNguoiChoi,
                diemSo,
                tongPhatBan,
                soPhatTrung,
                thoiGianGiay,
                soTauTaCon,
                cheDoChoi
            );
            dlg.ShowDialog(owner);
            tenMoi = dlg.TenNguoiChoiMoi;
            return dlg.UserAction;
        }
    }
}
