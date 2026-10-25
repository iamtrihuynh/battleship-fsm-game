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

            int formWidth = 500;
            int formHeight = _thang ? 486 : 386;
            Size = new Size(formWidth, formHeight);
            BackColor = UITheme.BackgroundDark;
            ForeColor = UITheme.TextPrimary;

            Region = new Region(UITheme.CreateRoundedPath(new Rectangle(0, 0, Width, Height), 14));

            var btnCloseX = new TacticalButton
            {
                Text = "✕",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(Width - 40, 10),
                Size = new Size(30, 26),
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
            int contentX = (Width - contentWidth) / 2;

            var lblIcon = new Label
            {
                Text = _thang ? "🏆" : "💀",
                Font = new Font("Segoe UI Emoji", 28f, FontStyle.Regular),
                Location = new Point((Width - 60) / 2, 14),
                Size = new Size(60, 42),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(lblIcon);

            var lblMainTitle = new Label
            {
                Text = _thang ? "CHIẾN THẮNG" : "THẤT BẠI",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = _thang ? UITheme.GoldAccent : UITheme.DangerRed,
                Location = new Point(contentX, 58),
                Size = new Size(contentWidth, 28),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(lblMainTitle);

            var lblSubtitle = new Label
            {
                Text = _thang
                    ? "Bạn đã tiêu diệt toàn bộ hạm đội đối phương"
                    : "Toàn bộ 5 chiến hạm của bạn đã bị bắn chìm",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = _thang ? UITheme.AccentNeon : UITheme.TextSecondary,
                Location = new Point(contentX, 86),
                Size = new Size(contentWidth, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(lblSubtitle);

            int tableY = 114;
            int tableH = 175;

            var pnlTable = new Panel
            {
                Location = new Point(contentX, tableY),
                Size = new Size(contentWidth, tableH),
                BackColor = UITheme.PanelBackground
            };
            pnlTable.Paint += (s, pe) =>
            {
                var g = pe.Graphics;
                UITheme.SetHighQuality(g);

                using var p = new Pen(UITheme.BorderDefault, 1f);
                using var path = UITheme.CreateRoundedPath(new RectangleF(0.5f, 0.5f, pnlTable.Width - 1f, pnlTable.Height - 1f), 8);
                g.DrawPath(p, path);

                using var linePen = new Pen(Color.FromArgb(48, 64, 88), 1f);
                for (int i = 1; i <= 4; i++)
                {
                    int lineY = i * 35;
                    g.DrawLine(linePen, 12, lineY, pnlTable.Width - 12, lineY);
                }
            };

            double tiLeChinhXac = _tongPhatBan > 0 ? (_soPhatTrung * 100.0 / _tongPhatBan) : 0;
            int soTruot = Math.Max(0, _tongPhatBan - _soPhatTrung);
            string thoiGianStr = $"{_thoiGianGiay / 60:D2}:{_thoiGianGiay % 60:D2}";

            if (_thang)
            {
                ThemHangThongSo(pnlTable, 0, "⭐ Điểm số", $"{_diemSo:N0} điểm", UITheme.GoldAccent, true);
                ThemHangThongSo(pnlTable, 1, "🎯 Khai hỏa", $"{_tongPhatBan} phát ({_soPhatTrung} trúng - {soTruot} trượt)", UITheme.TextPrimary, false);
                ThemHangThongSo(pnlTable, 2, "⚡ Độ chính xác", $"{tiLeChinhXac:F1}%", UITheme.AccentNeon, true);
                ThemHangThongSo(pnlTable, 3, "⏱️ Thời gian", thoiGianStr, UITheme.TextPrimary, false);
                ThemHangThongSo(pnlTable, 4, "🛡️ Tàu còn lại", $"{_soTauTaCon} / 5 tàu sống sót", UITheme.SuccessGreen, true);
            }
            else
            {
                ThemHangThongSo(pnlTable, 0, "🎖️ Chế độ chơi", _cheDoChoi, UITheme.TextPrimary, false);
                ThemHangThongSo(pnlTable, 1, "🎯 Khai hỏa", $"{_tongPhatBan} phát ({_soPhatTrung} trúng - {soTruot} trượt)", UITheme.TextPrimary, false);
                ThemHangThongSo(pnlTable, 2, "⚡ Độ chính xác", $"{tiLeChinhXac:F1}%", UITheme.AccentNeon, true);
                ThemHangThongSo(pnlTable, 3, "⏱️ Thời gian", thoiGianStr, UITheme.TextPrimary, false);
                ThemHangThongSo(pnlTable, 4, "🛡️ Tàu còn lại", "0 / 5 tàu (Đã đắm)", UITheme.DangerRed, true);
            }

            Controls.Add(pnlTable);

            if (_thang)
            {
                int inputY = tableY + tableH + 12;

                var lblInputPrompt = new Label
                {
                    Text = "Tên ghi danh Bảng Xếp Hạng:",
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    ForeColor = UITheme.TextPrimary,
                    Location = new Point(contentX, inputY),
                    Size = new Size(contentWidth, 18),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                Controls.Add(lblInputPrompt);

                var pnlTxtBox = new Panel
                {
                    Location = new Point(contentX, inputY + 22),
                    Size = new Size(contentWidth, 36),
                    BackColor = UITheme.HeaderBackground
                };
                pnlTxtBox.Paint += (s, pe) =>
                {
                    var g = pe.Graphics;
                    UITheme.SetHighQuality(g);
                    using var p = new Pen(UITheme.PrimaryCyan, 1.2f);
                    using var path = UITheme.CreateRoundedPath(new RectangleF(0.6f, 0.6f, pnlTxtBox.Width - 1.2f, pnlTxtBox.Height - 1.2f), 6);
                    g.DrawPath(p, path);
                };

                _txtName = new TextBox
                {
                    Text = _tenNguoiChoi,
                    Location = new Point(10, 8),
                    Size = new Size(pnlTxtBox.Width - 20, 20),
                    Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                    BackColor = UITheme.HeaderBackground,
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
                    Location = new Point(contentX, inputY + 60),
                    Size = new Size(contentWidth, 16),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                Controls.Add(_lblSaveStatus);
            }

            int btnY = Height - 56;

            if (_thang)
            {
                int btnW = 138;
                int btnGap = 13;
                int startBtnX = contentX;

                _btnSaveRank = new TacticalButton
                {
                    Text = "🏆 Bảng Xếp Hạng",
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    Location = new Point(startBtnX, btnY),
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
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    Location = new Point(startBtnX + btnW + btnGap, btnY),
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
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    Location = new Point(startBtnX + (btnW + btnGap) * 2, btnY),
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
                int startBtnX = contentX;

                var btnPlayAgain = new TacticalButton
                {
                    Text = "🔄 Chơi lại",
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    Location = new Point(startBtnX, btnY),
                    Size = new Size(btnW, 42),
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
                    Text = "🚪 Về Menu",
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    Location = new Point(startBtnX + btnW + btnGap, btnY),
                    Size = new Size(btnW, 42),
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

        private void ThemHangThongSo(Panel parent, int rowIndex, string label, string value, Color valColor, bool boldVal)
        {
            int rowY = rowIndex * 35;

            var lblKey = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = UITheme.TextSecondary,
                Location = new Point(14, rowY + 7),
                Size = new Size(160, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            parent.Controls.Add(lblKey);

            var lblVal = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 10f, boldVal ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = valColor,
                Location = new Point(180, rowY + 7),
                Size = new Size(parent.Width - 194, 20),
                TextAlign = ContentAlignment.MiddleRight
            };
            parent.Controls.Add(lblVal);
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            BattleshipGame.Logic.cAmThanh.DungSFX();
            base.OnFormClosing(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            UITheme.SetHighQuality(g);

            Color borderColor = _thang ? UITheme.GoldAccent : UITheme.DangerRed;
            using var borderPen = new Pen(borderColor, 1.8f);
            using var path = UITheme.CreateRoundedPath(new RectangleF(0.9f, 0.9f, Width - 1.8f, Height - 1.8f), 14);
            g.DrawPath(borderPen, path);
        }

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
