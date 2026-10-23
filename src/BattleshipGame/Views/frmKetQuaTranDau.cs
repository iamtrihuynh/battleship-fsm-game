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
    /// Thiết kế đồ họa chuyên nghiệp, tích hợp bảng chiến tích, ghi danh Bảng Xếp Hạng và phím điều hướng tác chiến.
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

            int formWidth = 530;
            int formHeight = _thang ? 560 : 470;
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
                Location = new Point(Width - 44, 12),
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

            // Icon biểu tượng kết quả
            var lblIcon = new Label
            {
                Text = _thang ? "🏆" : "💀",
                Font = new Font("Segoe UI Emoji", 34f, FontStyle.Regular),
                Location = new Point(24, 16),
                Size = new Size(62, 64),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(lblIcon);

            // Tiêu đề chính
            var lblMainTitle = new Label
            {
                Text = _thang ? "CHIẾN THẮNG HUY HOÀNG" : "HẠM ĐỘI BỊ TIÊU DIỆT",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = _thang ? UITheme.GoldAccent : UITheme.DangerRed,
                Location = new Point(94, 20),
                Size = new Size(Width - 146, 32),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(lblMainTitle);

            // Tiêu đề phụ
            var lblSubtitle = new Label
            {
                Text = _thang
                    ? "Hạm đội đối phương đã hoàn toàn bị xóa sổ trên biển!"
                    : "Toàn bộ 5 chiến hạm của bạn đã bị đối thủ bắn chìm!",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = _thang ? UITheme.AccentNeon : UITheme.TextSecondary,
                Location = new Point(96, 52),
                Size = new Size(Width - 148, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(lblSubtitle);

            // Thông tin chế độ
            var lblModeBadge = new Label
            {
                Text = $"🎖️ Chế độ: {_cheDoChoi}  •  Đô đốc: {_tenNguoiChoi}",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                ForeColor = UITheme.TextMuted,
                Location = new Point(96, 74),
                Size = new Size(Width - 148, 18),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(lblModeBadge);

            // -------------------------------------------------------------
            // BẢNG THỐNG KÊ CHIẾN TÍCH (STATS CARD)
            // -------------------------------------------------------------
            int cardY = 104;
            int cardH = _thang ? 228 : 250;
            var pnlStatsCard = new Panel
            {
                Location = new Point(24, cardY),
                Size = new Size(Width - 48, cardH),
                BackColor = Color.FromArgb(18, 29, 48)
            };
            pnlStatsCard.Paint += (s, pe) =>
            {
                var g = pe.Graphics;
                UITheme.SetHighQuality(g);
                Color bColor = _thang ? UITheme.PrimaryCyan : Color.FromArgb(90, 40, 50);
                using var p = new Pen(bColor, 1.2f);
                using var path = UITheme.CreateRoundedPath(new RectangleF(0.6f, 0.6f, pnlStatsCard.Width - 1.2f, pnlStatsCard.Height - 1.2f), 10);
                g.DrawPath(p, path);
            };

            // Banner điểm số (Thắng) hoặc Cảnh báo (Thua)
            var pnlScoreRibbon = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(pnlStatsCard.Width - 20, _thang ? 52 : 46),
                BackColor = _thang ? Color.FromArgb(28, 44, 72) : Color.FromArgb(42, 22, 28)
            };
            pnlScoreRibbon.Paint += (s, pe) =>
            {
                var g = pe.Graphics;
                UITheme.SetHighQuality(g);
                Color rbBorder = _thang ? UITheme.GoldAccent : UITheme.DangerRed;
                using var p = new Pen(rbBorder, 1.2f);
                using var path = UITheme.CreateRoundedPath(new RectangleF(0.6f, 0.6f, pnlScoreRibbon.Width - 1.2f, pnlScoreRibbon.Height - 1.2f), 8);
                g.DrawPath(p, path);
            };

            if (_thang)
            {
                var lblScoreTag = new Label
                {
                    Text = "⭐ ĐIỂM SỐ TÁC CHIẾN",
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    ForeColor = UITheme.GoldAccent,
                    Location = new Point(14, 6),
                    Size = new Size(180, 16)
                };
                pnlScoreRibbon.Controls.Add(lblScoreTag);

                var lblScoreValue = new Label
                {
                    Text = $"{_diemSo:N0} ĐIỂM",
                    Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                    ForeColor = UITheme.AccentNeon,
                    Location = new Point(14, 22),
                    Size = new Size(pnlScoreRibbon.Width - 28, 26)
                };
                pnlScoreRibbon.Controls.Add(lblScoreValue);
            }
            else
            {
                var lblDefeatBanner = new Label
                {
                    Text = "⚠️ TRẬN ĐẤU THẤT BẠI - HẠM ĐỘI BỊ ĐẮM",
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = UITheme.DangerRed,
                    Location = new Point(14, 6),
                    Size = new Size(pnlScoreRibbon.Width - 28, 18)
                };
                pnlScoreRibbon.Controls.Add(lblDefeatBanner);

                var lblDefeatSub = new Label
                {
                    Text = "Không đủ điều kiện ghi nhận vào Bảng Xếp Hạng tác chiến.",
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                    ForeColor = UITheme.TextMuted,
                    Location = new Point(14, 24),
                    Size = new Size(pnlScoreRibbon.Width - 28, 16)
                };
                pnlScoreRibbon.Controls.Add(lblDefeatSub);
            }
            pnlStatsCard.Controls.Add(pnlScoreRibbon);

            // 4 Ô thông số chiến thuật (2x2 Grid)
            int gridStartY = _thang ? 70 : 66;
            int tileW = (pnlStatsCard.Width - 28) / 2;
            int tileH = 68;

            double tiLeChinhXac = _tongPhatBan > 0 ? (_soPhatTrung * 100.0 / _tongPhatBan) : 0;
            int soTruot = Math.Max(0, _tongPhatBan - _soPhatTrung);
            string thoiGianStr = $"{_thoiGianGiay / 60:D2}:{_thoiGianGiay % 60:D2}";

            // Tile 1: Phát bắn
            pnlStatsCard.Controls.Add(TaoTileThongSo(
                new Point(10, gridStartY),
                new Size(tileW, tileH),
                "🎯 Tổng phát bắn",
                $"{_tongPhatBan} phát",
                $"Trúng: {_soPhatTrung}  •  Trượt: {soTruot}",
                UITheme.PrimaryCyan
            ));

            // Tile 2: Độ chính xác
            Color accuracyColor = tiLeChinhXac >= 60 ? UITheme.SuccessGreen :
                                  tiLeChinhXac >= 40 ? UITheme.WarningAmber : UITheme.TextSecondary;
            pnlStatsCard.Controls.Add(TaoTileThongSo(
                new Point(18 + tileW, gridStartY),
                new Size(tileW, tileH),
                "⚡ Độ chính xác",
                $"{tiLeChinhXac:F1}%",
                tiLeChinhXac >= 60 ? "Xạ thủ cừ khôi" : "Cần ngắm chuẩn hơn",
                accuracyColor
            ));

            // Tile 3: Thời gian tác chiến
            pnlStatsCard.Controls.Add(TaoTileThongSo(
                new Point(10, gridStartY + tileH + 8),
                new Size(tileW, tileH),
                "⏱️ Thời gian",
                thoiGianStr,
                "Thời gian trận đấu",
                UITheme.AccentNeon
            ));

            // Tile 4: Tàu ta sống sót
            pnlStatsCard.Controls.Add(TaoTileThongSo(
                new Point(18 + tileW, gridStartY + tileH + 8),
                new Size(tileW, tileH),
                "🛡️ Chiến hạm ta",
                $"{_soTauTaCon}/5 sống sót",
                _thang ? $"{_soTauTaCon} tàu toàn mạng" : "Đã bị đắm hoàn toàn",
                _thang ? UITheme.SuccessGreen : UITheme.DangerRed
            ));

            // Lời khuyên tác chiến cho chế độ Thất bại
            if (!_thang)
            {
                var pnlAdvice = new Panel
                {
                    Location = new Point(10, gridStartY + (tileH + 8) * 2),
                    Size = new Size(pnlStatsCard.Width - 20, 36),
                    BackColor = Color.FromArgb(24, 34, 52)
                };
                var lblAdvice = new Label
                {
                    Text = "💡 Lời khuyên: Hãy phân bố tàu so le ở các góc và dùng Radar 3x3 để dò tìm tàu tuần tra địch.",
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                    ForeColor = UITheme.WarningAmber,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(8, 0, 8, 0)
                };
                pnlAdvice.Controls.Add(lblAdvice);
                pnlStatsCard.Controls.Add(pnlAdvice);
            }

            Controls.Add(pnlStatsCard);

            // -------------------------------------------------------------
            // PHẦN NHẬP TÊN GHI DANH BẢNG XẾP HẠNG (CHỈ DÀNH CHO CHIẾN THẮNG)
            // -------------------------------------------------------------
            if (_thang)
            {
                int inputY = cardY + cardH + 12;

                var lblInputPrompt = new Label
                {
                    Text = "🎖️ Ghi danh Đô đốc vào Bảng Xếp Hạng:",
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    ForeColor = UITheme.GoldAccent,
                    Location = new Point(24, inputY),
                    Size = new Size(Width - 48, 20)
                };
                Controls.Add(lblInputPrompt);

                var pnlTxtBox = new Panel
                {
                    Location = new Point(24, inputY + 22),
                    Size = new Size(Width - 48, 38),
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
                    Text = "Thành tích của bạn sẽ được lưu vĩnh viễn trên bảng xếp hạng tác chiến.",
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                    ForeColor = UITheme.TextSecondary,
                    Location = new Point(24, inputY + 64),
                    Size = new Size(Width - 48, 18)
                };
                Controls.Add(_lblSaveStatus);
            }

            // -------------------------------------------------------------
            // CỤM NÚT BẤM ĐIỀU HƯỚNG TÁC CHIẾN (BOTTOM BUTTONS)
            // -------------------------------------------------------------
            int btnY = Height - 60;

            if (_thang)
            {
                _btnSaveRank = new TacticalButton
                {
                    Text = "💾 Lưu & Xem BXH",
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Location = new Point(24, btnY),
                    Size = new Size(160, 44),
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
                    Text = "🔄 Chơi ván mới",
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Location = new Point(194, btnY),
                    Size = new Size(160, 44),
                    CornerRadius = 6,
                    BorderColor = UITheme.AccentNeon,
                    NormalTextColor = UITheme.AccentNeon,
                    HoverBorderColor = Color.White,
                    HoverTextColor = Color.White
                };
                btnPlayAgain.Click += (s, e) =>
                {
                    // Tự động lưu nếu chưa lưu
                    TuDongLuuNeuChuaLuu();
                    UserAction = KetQuaTranAction.ChoiLai;
                    Close();
                };
                Controls.Add(btnPlayAgain);

                var btnMenu = new TacticalButton
                {
                    Text = "🚪 Về Menu",
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Location = new Point(364, btnY),
                    Size = new Size(142, 44),
                    CornerRadius = 6,
                    BorderColor = UITheme.BorderLight,
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
                var btnPlayAgain = new TacticalButton
                {
                    Text = "🔄 Phục thù (Chơi lại)",
                    Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                    Location = new Point(48, btnY),
                    Size = new Size(240, 44),
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
                    Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                    Location = new Point(300, btnY),
                    Size = new Size(182, 44),
                    CornerRadius = 6,
                    BorderColor = UITheme.BorderLight,
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
                BackColor = Color.FromArgb(24, 38, 62)
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
                Location = new Point(8, 6),
                Size = new Size(sz.Width - 16, 16)
            };
            pnl.Controls.Add(lblT);

            var lblV = new Label
            {
                Text = val,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = accent,
                Location = new Point(8, 22),
                Size = new Size(sz.Width - 16, 24)
            };
            pnl.Controls.Add(lblV);

            var lblS = new Label
            {
                Text = sub,
                Font = new Font("Segoe UI", 8f, FontStyle.Regular),
                ForeColor = UITheme.TextMuted,
                Location = new Point(8, 46),
                Size = new Size(sz.Width - 16, 16)
            };
            pnl.Controls.Add(lblS);

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
