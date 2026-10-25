using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
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

            if (Width <= 4 || Height <= 4) return;

            if (Parent != null)
            {
                using (SolidBrush parentBrush = new SolidBrush(Parent.BackColor))
                {
                    g.FillRectangle(parentBrush, ClientRectangle);
                }
            }

            float strokeWidth = _isHovered ? 2.0f : 1.5f;
            float halfStroke = strokeWidth / 2f;
            RectangleF borderRect = new RectangleF(
                halfStroke,
                halfStroke,
                Width - strokeWidth,
                Height - strokeWidth
            );

            Color fillColor = !Enabled ? Color.FromArgb(16, 24, 38) :
                              _isPressed ? Color.FromArgb(20, 32, 50) :
                              _isHovered ? HoverFillColor : BaseFillColor;

            Color borderColor = !Enabled ? Color.FromArgb(45, 60, 80) :
                                _isHovered ? HoverBorderColor : BorderColor;

            Color textColor = !Enabled ? UITheme.TextMuted :
                              _isHovered ? HoverTextColor : NormalTextColor;

            using (GraphicsPath path = UITheme.CreateRoundedPath(borderRect, CornerRadius))
            {
                using (SolidBrush brush = new SolidBrush(fillColor))
                {
                    g.FillPath(brush, path);
                }

                if (Enabled && borderRect.Height > 10)
                {
                    using (LinearGradientBrush sheen = new LinearGradientBrush(
                        new PointF(0, borderRect.Top),
                        new PointF(0, borderRect.Top + borderRect.Height * 0.45f),
                        Color.FromArgb(_isHovered ? 30 : 14, Color.White),
                        Color.FromArgb(0, Color.White)))
                    {
                        g.FillPath(sheen, path);
                    }
                }

                using (Pen pen = new Pen(borderColor, strokeWidth))
                {
                    pen.LineJoin = LineJoin.Round;
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    g.DrawPath(pen, path);
                }
            }

            Rectangle textRect = new Rectangle(0, 0, Width, Height);
            if (_isPressed)
            {
                textRect.Offset(0, 1);
            }

            TextRenderer.DrawText(
                g,
                Text,
                Font,
                textRect,
                textColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix
            );
        }
    }
}
