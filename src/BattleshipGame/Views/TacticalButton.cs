using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
    /// <summary>
    /// Nút bấm phong cách quân sự Hải Quân hiện đại với hiệu ứng phát sáng Neon khi hover
    /// </summary>
    public class TacticalButton : Button
    {
        private bool _isHovered = false;
        private bool _isPressed = false;

        [Category("Tactical Style")]
        public Color BorderColor { get; set; } = UITheme.BorderLight;

        [Category("Tactical Style")]
        public Color HoverBorderColor { get; set; } = UITheme.AccentNeon;

        [Category("Tactical Style")]
        public Color HoverTextColor { get; set; } = UITheme.AccentNeon;

        [Category("Tactical Style")]
        public Color NormalTextColor { get; set; } = UITheme.TextPrimary;

        [Category("Tactical Style")]
        public Color BaseFillColor { get; set; } = UITheme.PanelBackground;

        [Category("Tactical Style")]
        public Color HoverFillColor { get; set; } = Color.FromArgb(40, 56, 80);

        [Category("Tactical Style")]
        public int CornerRadius { get; set; } = 8;

        public TacticalButton()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true
            );
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = UITheme.PanelBackground;
            ForeColor = UITheme.TextPrimary;
            Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            Cursor = Cursors.Hand;
            Size = new Size(240, 48);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            _isPressed = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            if (mevent.Button == MouseButtons.Left)
            {
                _isPressed = true;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            _isPressed = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            UITheme.SetHighQuality(g);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            Color fillColor = !Enabled ? Color.FromArgb(20, 28, 42) :
                              _isPressed ? Color.FromArgb(24, 38, 58) :
                              _isHovered ? HoverFillColor : BaseFillColor;

            Color borderColor = !Enabled ? UITheme.BorderDefault :
                                _isHovered ? HoverBorderColor : BorderColor;

            Color textColor = !Enabled ? UITheme.TextMuted :
                              _isHovered ? HoverTextColor : NormalTextColor;

            using (GraphicsPath path = UITheme.CreateRoundedPath(rect, CornerRadius))
            {
                // Vẽ nền
                using (SolidBrush brush = new SolidBrush(fillColor))
                {
                    g.FillPath(brush, path);
                }

                // Vẽ viền sắc nét, nổi bật rõ ràng
                using (Pen pen = new Pen(borderColor, _isHovered ? 2.2f : 1.8f))
                {
                    g.DrawPath(pen, path);
                }

                // Vẽ hiệu ứng viền phát sáng nhẹ khi hover
                if (_isHovered && Enabled)
                {
                    using (Pen glowPen = new Pen(Color.FromArgb(60, HoverBorderColor), 3.5f))
                    {
                        Rectangle glowRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
                        using (GraphicsPath glowPath = UITheme.CreateRoundedPath(glowRect, Math.Max(2, CornerRadius - 1)))
                        {
                            g.DrawPath(glowPen, glowPath);
                        }
                    }
                }
            }

            // Vẽ Text căn giữa
            TextRenderer.DrawText(
                g,
                Text,
                Font,
                rect,
                textColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis
            );
        }
    }
}
