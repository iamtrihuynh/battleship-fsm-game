#nullable enable
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
    /// <summary>
    /// [NGÀY 6 - THÀNH VIÊN 2: TRẦN TRỌNG HƯNG - FRONTEND & UI/UX LEAD]
    /// Hộp thoại thông báo tùy biến phong cách Hải quân hiện đại (Tactical Naval Dark Theme).
    /// Thay thế hoàn toàn MessageBox.Show cổ điển, mang lại trải nghiệm thị giác quân sự đồng bộ:
    /// - Hộp thoại Chiến Thắng Hải Quân (Victory): Cúp vàng 🏆, viền Neon/Gold rực rỡ.
    /// - Hộp thoại Thất Bại / Tổn Thất (Defeat): Xương sọ 💀, viền đỏ thẫm báo động.
    /// - Hộp thoại Đầu Hàng / Rút Quân (Surrender): Cờ trắng 🏳️, cảnh báo Đô đốc cân nhắc chiến thuật.
    /// - Hộp thoại Cảnh Báo Tác Chiến (Warning): Radar ⚠️, nhắc nhở quy tắc khai hỏa.
    /// - Hộp thoại Xác Nhận Thoát (Confirm Exit): Biểu tượng mỏ neo ⚓ và sở chỉ huy.
    /// </summary>
    public enum TacticalDialogType
    {
        Info,
        Warning,
        Confirm,
        Victory,
        Defeat,
        Surrender,
        Exit
    }

    public class TacticalMessageBox : Form
    {
        private readonly TacticalDialogType _dialogType;
        private readonly string _customTitle;
        private readonly string _customMessage;
        private readonly MessageBoxButtons _buttons;

        private TacticalButton? _btnPrimary;
        private TacticalButton? _btnSecondary;
        private Label? _lblHeader;
        private Label? _lblTitle;
        private Label? _lblMessage;
        private Label? _lblIcon;
        private TacticalButton? _btnCloseX;

        public TacticalMessageBox(
            string title,
            string message,
            TacticalDialogType type = TacticalDialogType.Info,
            MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            _customTitle = title;
            _customMessage = message;
            _dialogType = type;
            _buttons = buttons;

            InitializeForm();
        }

        private void InitializeForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            int lineCount = (_customMessage != null) ? _customMessage.Split('\n').Length : 1;
            int extraHeight = Math.Max(0, (lineCount - 5) * 20);
            Size = new Size(520, 290 + extraHeight);
            BackColor = UITheme.BackgroundDark;

            // Màu chủ đạo theo loại hộp thoại
            Color accentColor = _dialogType switch
            {
                TacticalDialogType.Victory => UITheme.GoldAccent,
                TacticalDialogType.Defeat => UITheme.DangerRed,
                TacticalDialogType.Surrender => UITheme.WarningAmber,
                TacticalDialogType.Warning => UITheme.WarningAmber,
                TacticalDialogType.Exit => UITheme.PrimaryCyan,
                TacticalDialogType.Confirm => UITheme.AccentNeon,
                _ => UITheme.AccentNeon
            };

            string iconText = _dialogType switch
            {
                TacticalDialogType.Victory => "🏆",
                TacticalDialogType.Defeat => "💀",
                TacticalDialogType.Surrender => "🏳️",
                TacticalDialogType.Warning => "⚠️",
                TacticalDialogType.Exit => "🚪",
                TacticalDialogType.Confirm => "⚓",
                _ => "📡"
            };

            string headerBadge = _dialogType switch
            {
                TacticalDialogType.Victory => "BÁO CÁO CHIẾN CÔNG HẢI QUÂN",
                TacticalDialogType.Defeat => "BÁO CÁO TỔN THẤT HẠM ĐỘI",
                TacticalDialogType.Surrender => "MỆNH LỆNH RÚT QUÂN HẢI QUÂN",
                TacticalDialogType.Warning => "CẢNH BÁO TÁC CHIẾN CHIẾN HẠM",
                TacticalDialogType.Exit => "SỞ CHỈ HUY QUÂN SỰ",
                TacticalDialogType.Confirm => "XÁC NHẬN LỆNH ĐÔ ĐỐC",
                _ => "THÔNG ĐIỆP TỔNG HÀNH DINH"
            };

            // 1. Header Bar
            Panel pnlHeader = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(Width, 42),
                BackColor = UITheme.HeaderBackground
            };

            _lblHeader = new Label
            {
                Text = $"⚓  {headerBadge}",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = accentColor,
                Location = new Point(14, 11),
                AutoSize = true
            };
            pnlHeader.Controls.Add(_lblHeader);

            _btnCloseX = new TacticalButton
            {
                Text = "✕",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Location = new Point(Width - 46, 7),
                Size = new Size(34, 28),
                CornerRadius = 4,
                BaseFillColor = Color.Transparent,
                BorderColor = Color.Transparent,
                HoverFillColor = Color.FromArgb(80, 239, 68, 68),
                HoverTextColor = Color.White,
                NormalTextColor = UITheme.TextMuted
            };
            _btnCloseX.Click += (s, e) =>
            {
                DialogResult = (_buttons == MessageBoxButtons.YesNo || _buttons == MessageBoxButtons.YesNoCancel)
                    ? DialogResult.No
                    : DialogResult.Cancel;
                Close();
            };
            pnlHeader.Controls.Add(_btnCloseX);
            Controls.Add(pnlHeader);

            // 2. Icon lớn
            _lblIcon = new Label
            {
                Text = iconText,
                Font = new Font("Segoe UI Emoji", 34f, FontStyle.Regular),
                Location = new Point(20, 56),
                Size = new Size(64, 64),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = accentColor
            };
            Controls.Add(_lblIcon);

            // 3. Tiêu đề
            _lblTitle = new Label
            {
                Text = _customTitle,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = accentColor,
                Location = new Point(94, 58),
                Size = new Size(Width - 110, 32),
                AutoEllipsis = true
            };
            Controls.Add(_lblTitle);

            // 4. Nội dung tin nhắn
            _lblMessage = new Label
            {
                Text = _customMessage,
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = UITheme.TextPrimary,
                Location = new Point(96, 96),
                Size = new Size(Width - 116, 110 + extraHeight),
                AutoEllipsis = false
            };
            Controls.Add(_lblMessage);

            // 5. Cụm nút bấm
            Panel pnlButtons = new Panel
            {
                Location = new Point(0, Height - 66),
                Size = new Size(Width, 66),
                BackColor = UITheme.PanelBackground
            };

            if (_buttons == MessageBoxButtons.YesNo)
            {
                string yesText = _dialogType switch
                {
                    TacticalDialogType.Surrender => "🏳️ ĐẦU HÀNG",
                    TacticalDialogType.Exit => "🚪 THOÁT GAME",
                    _ => "✓ ĐỒNG Ý"
                };

                string noText = _dialogType switch
                {
                    TacticalDialogType.Surrender => "⚔️ CHIẾN ĐẤU TIẾP",
                    TacticalDialogType.Exit => "⚓ Ở LẠI SỞ CHỈ HUY",
                    _ => "✕ HỦY BỎ"
                };

                _btnPrimary = new TacticalButton
                {
                    Text = yesText,
                    Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                    Size = new Size(180, 42),
                    Location = new Point(Width - 390, 12),
                    CornerRadius = 6,
                    BaseFillColor = Color.FromArgb(24, 38, 58),
                    BorderColor = accentColor,
                    NormalTextColor = accentColor,
                    HoverBorderColor = accentColor,
                    HoverTextColor = Color.White,
                    HoverFillColor = Color.FromArgb(40, accentColor)
                };
                _btnPrimary.Click += (s, e) => { DialogResult = DialogResult.Yes; Close(); };

                _btnSecondary = new TacticalButton
                {
                    Text = noText,
                    Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                    Size = new Size(180, 42),
                    Location = new Point(Width - 198, 12),
                    CornerRadius = 6,
                    BaseFillColor = Color.FromArgb(20, 28, 42),
                    BorderColor = UITheme.BorderDefault,
                    NormalTextColor = UITheme.TextSecondary,
                    HoverBorderColor = UITheme.AccentNeon,
                    HoverTextColor = UITheme.AccentNeon
                };
                _btnSecondary.Click += (s, e) => { DialogResult = DialogResult.No; Close(); };

                pnlButtons.Controls.Add(_btnPrimary);
                pnlButtons.Controls.Add(_btnSecondary);
                AcceptButton = _btnPrimary;
                CancelButton = _btnSecondary;
            }
            else
            {
                string okText = _dialogType switch
                {
                    TacticalDialogType.Victory => "🏆 TIẾP TỤC",
                    TacticalDialogType.Defeat => "🔄 ĐÃ RÕ",
                    _ => "⚓ TIẾP TỤC"
                };

                _btnPrimary = new TacticalButton
                {
                    Text = okText,
                    Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                    Size = new Size(180, 42),
                    Location = new Point((Width - 180) / 2, 12),
                    CornerRadius = 6,
                    BaseFillColor = Color.FromArgb(24, 38, 58),
                    BorderColor = accentColor,
                    NormalTextColor = accentColor,
                    HoverBorderColor = accentColor,
                    HoverTextColor = Color.White,
                    HoverFillColor = Color.FromArgb(40, accentColor)
                };
                _btnPrimary.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };

                pnlButtons.Controls.Add(_btnPrimary);
                AcceptButton = _btnPrimary;
            }

            Controls.Add(pnlButtons);

            // Hỗ trợ phím tắt
            KeyPreview = true;
            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    DialogResult = (_buttons == MessageBoxButtons.YesNo) ? DialogResult.No : DialogResult.Cancel;
                    Close();
                }
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            UITheme.SetHighQuality(g);

            // Vẽ viền bo góc sắc nét màu theo chủ đề
            Color borderColor = _dialogType switch
            {
                TacticalDialogType.Victory => UITheme.GoldAccent,
                TacticalDialogType.Defeat => UITheme.DangerRed,
                TacticalDialogType.Surrender => UITheme.WarningAmber,
                TacticalDialogType.Warning => UITheme.WarningAmber,
                _ => UITheme.PrimaryCyan
            };

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (Pen pen = new Pen(borderColor, 1.8f))
            {
                g.DrawRectangle(pen, rect);
            }

            // Đường phân cách dưới Header
            using (Pen linePen = new Pen(UITheme.BorderDefault, 1f))
            {
                g.DrawLine(linePen, 0, 42, Width, 42);
                g.DrawLine(linePen, 0, Height - 66, Width, Height - 66);
            }
        }

        #region PHƯƠNG THỨC TIỆN ÍCH TĨNH (STATIC HELPER METHODS)
        /// <summary>
        /// Hiển thị hộp thoại phong cách hải chiến tổng quát
        /// </summary>
        public static DialogResult Show(
            IWin32Window? owner,
            string title,
            string message,
            TacticalDialogType type = TacticalDialogType.Info,
            MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            using var dlg = new TacticalMessageBox(title, message, type, buttons);
            return owner != null ? dlg.ShowDialog(owner) : dlg.ShowDialog();
        }

        /// <summary>
        /// Hộp thoại cảnh báo tác chiến
        /// </summary>
        public static void ShowWarning(IWin32Window? owner, string title, string message)
        {
            Show(owner, title, message, TacticalDialogType.Warning, MessageBoxButtons.OK);
        }

        /// <summary>
        /// Hộp thoại thông tin tác chiến
        /// </summary>
        public static void ShowInfo(IWin32Window? owner, string title, string message)
        {
            Show(owner, title, message, TacticalDialogType.Info, MessageBoxButtons.OK);
        }

        /// <summary>
        /// Hộp thoại xác nhận mệnh lệnh hải quân (Yes/No)
        /// </summary>
        public static DialogResult ShowConfirm(IWin32Window? owner, string title, string message)
        {
            return Show(owner, title, message, TacticalDialogType.Confirm, MessageBoxButtons.YesNo);
        }

        /// <summary>
        /// Hộp thoại Đô đốc xác nhận đầu hàng (Surrender Confirmation)
        /// </summary>
        public static DialogResult ShowSurrenderConfirm(IWin32Window? owner)
        {
            return Show(
                owner,
                "LỆNH ĐẦU HÀNG CHIẾN HẠM",
                "Đồng chí Đô đốc có chắc chắn muốn phát tín hiệu cờ trắng đầu hàng và rút toàn bộ hạm đội về căn cứ?\n\n" +
                "⚠️ Thao tác này sẽ kết thúc ván đấu ngay lập tức và tính phần thắng cho đối phương!",
                TacticalDialogType.Surrender,
                MessageBoxButtons.YesNo
            );
        }

        /// <summary>
        /// Hộp thoại xác nhận rời khỏi Sở chỉ huy (Exit Confirmation)
        /// </summary>
        public static DialogResult ShowExitConfirm(IWin32Window? owner)
        {
            return Show(
                owner,
                "RỜI KHỎI SỞ CHỈ HUY",
                "Đồng chí Đô đốc có chắc chắn muốn đóng sở chỉ huy và thoát khỏi trò chơi Bắn Tàu?",
                TacticalDialogType.Exit,
                MessageBoxButtons.YesNo
            );
        }

        /// <summary>
        /// Hộp thoại Chiến Thắng (Victory)
        /// </summary>
        public static DialogResult ShowVictory(IWin32Window? owner, string title, string message, MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            return Show(
                owner,
                title,
                message,
                TacticalDialogType.Victory,
                buttons
            );
        }

        /// <summary>
        /// Hộp thoại Thất Bại (Defeat)
        /// </summary>
        public static void ShowDefeat(IWin32Window? owner, string message)
        {
            Show(
                owner,
                "HẠM ĐỘI BỊ TIÊU DIỆT",
                message,
                TacticalDialogType.Defeat,
                MessageBoxButtons.OK
            );
        }

        public static void ShowDefeat(IWin32Window? owner, string title, string message)
        {
            Show(
                owner,
                title,
                message,
                TacticalDialogType.Defeat,
                MessageBoxButtons.OK
            );
        }
        #endregion
    }
}
