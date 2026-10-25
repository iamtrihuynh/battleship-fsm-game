#nullable enable
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
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

            Font msgFont = new Font("Segoe UI", 10f, FontStyle.Regular);
            int contentWidth = 360;
            Size measured = TextRenderer.MeasureText(_customMessage ?? "", msgFont, new Size(contentWidth, 0), TextFormatFlags.WordBreak);
            int msgHeight = Math.Max(36, measured.Height + 10);

            int formWidth = 480;
            int formHeight = 42 + 48 + msgHeight + 20 + 60;
            Size = new Size(formWidth, formHeight);
            BackColor = UITheme.BackgroundDark;

            Color accentColor = _dialogType switch
            {
                TacticalDialogType.Victory => UITheme.GoldAccent,
                TacticalDialogType.Defeat => UITheme.DangerRed,
                TacticalDialogType.Surrender => UITheme.WarningAmber,
                TacticalDialogType.Warning => UITheme.WarningAmber,
                TacticalDialogType.Exit => UITheme.DangerRed,
                TacticalDialogType.Confirm => UITheme.PrimaryCyan,
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
                _ => "ℹ️"
            };

            string headerBadge = _dialogType switch
            {
                TacticalDialogType.Victory => "Chiến thắng",
                TacticalDialogType.Defeat => "Thất bại",
                TacticalDialogType.Surrender => "Xác nhận đầu hàng",
                TacticalDialogType.Warning => "Cảnh báo",
                TacticalDialogType.Exit => "Xác nhận thoát",
                TacticalDialogType.Confirm => "Xác nhận",
                _ => "Thông báo"
            };

            Panel pnlHeader = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(Width, 40),
                BackColor = UITheme.HeaderBackground
            };

            _lblHeader = new Label
            {
                Text = headerBadge,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = accentColor,
                Location = new Point(14, 10),
                AutoSize = true
            };
            pnlHeader.Controls.Add(_lblHeader);

            _btnCloseX = new TacticalButton
            {
                Text = "✕",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(Width - 42, 6),
                Size = new Size(32, 28),
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

            _lblIcon = new Label
            {
                Text = iconText,
                Font = new Font("Segoe UI Emoji", 26f, FontStyle.Regular),
                Location = new Point(16, 52),
                Size = new Size(54, 54),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = accentColor
            };
            Controls.Add(_lblIcon);

            _lblTitle = new Label
            {
                Text = _customTitle,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = accentColor,
                Location = new Point(78, 54),
                Size = new Size(Width - 94, 28),
                AutoEllipsis = true
            };
            Controls.Add(_lblTitle);

            _lblMessage = new Label
            {
                Text = _customMessage,
                Font = msgFont,
                ForeColor = UITheme.TextPrimary,
                Location = new Point(78, 86),
                Size = new Size(Width - 94, msgHeight),
                AutoEllipsis = false
            };
            Controls.Add(_lblMessage);

            Panel pnlButtons = new Panel
            {
                Location = new Point(0, Height - 58),
                Size = new Size(Width, 58),
                BackColor = UITheme.PanelBackground
            };

            if (_buttons == MessageBoxButtons.YesNo)
            {
                string yesText = _dialogType switch
                {
                    TacticalDialogType.Surrender => "Đầu hàng",
                    TacticalDialogType.Exit => "Thoát",
                    _ => "Đồng ý"
                };

                string noText = _dialogType switch
                {
                    TacticalDialogType.Surrender => "Tiếp tục chơi",
                    TacticalDialogType.Exit => "Ở lại",
                    _ => "Hủy"
                };

                int btnW = 140;
                int btnH = 38;
                int gap = 16;
                int startBtnX = (Width - (btnW * 2 + gap)) / 2;

                _btnPrimary = new TacticalButton
                {
                    Text = yesText,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Size = new Size(btnW, btnH),
                    Location = new Point(startBtnX, 10),
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
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Size = new Size(btnW, btnH),
                    Location = new Point(startBtnX + btnW + gap, 10),
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
                    TacticalDialogType.Victory => "Tiếp tục",
                    TacticalDialogType.Defeat => "Đóng",
                    _ => "Đóng"
                };

                int btnW = 130;
                int btnH = 36;
                _btnPrimary = new TacticalButton
                {
                    Text = okText,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Size = new Size(btnW, btnH),
                    Location = new Point((Width - btnW) / 2, 10),
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

            using (Pen linePen = new Pen(UITheme.BorderDefault, 1f))
            {
                g.DrawLine(linePen, 0, 40, Width, 40);
                g.DrawLine(linePen, 0, Height - 58, Width, Height - 58);
            }
        }

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

        public static void ShowWarning(IWin32Window? owner, string title, string message)
        {
            Show(owner, title, message, TacticalDialogType.Warning, MessageBoxButtons.OK);
        }

        public static void ShowInfo(IWin32Window? owner, string title, string message)
        {
            Show(owner, title, message, TacticalDialogType.Info, MessageBoxButtons.OK);
        }

        public static DialogResult ShowConfirm(IWin32Window? owner, string title, string message)
        {
            return Show(owner, title, message, TacticalDialogType.Confirm, MessageBoxButtons.YesNo);
        }

        public static DialogResult ShowSurrenderConfirm(IWin32Window? owner)
        {
            return Show(
                owner,
                "Đầu hàng",
                "Bạn có chắc chắn muốn đầu hàng không?",
                TacticalDialogType.Surrender,
                MessageBoxButtons.YesNo
            );
        }

        public static DialogResult ShowExitConfirm(IWin32Window? owner)
        {
            return Show(
                owner,
                "Thoát trò chơi",
                "Bạn có chắc chắn muốn thoát khỏi trò chơi?",
                TacticalDialogType.Exit,
                MessageBoxButtons.YesNo
            );
        }

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

        public static void ShowDefeat(IWin32Window? owner, string message)
        {
            Show(
                owner,
                "Thất bại",
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
    }
}
