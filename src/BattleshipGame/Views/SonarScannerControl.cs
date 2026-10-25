using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
    public class SonarScannerControl : Control
    {
        private readonly System.Windows.Forms.Timer _sweepTimer;
        private float _currentAngle = 0f;

        public SonarScannerControl()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true
            );
            BackColor = UITheme.BackgroundDark;
            Size = new Size(160, 160);

            _sweepTimer = new System.Windows.Forms.Timer
            {
                Interval = 30
            };
            _sweepTimer.Tick += (s, e) =>
            {
                _currentAngle = (_currentAngle + 3f) % 360f;
                Invalidate();
            };
            _sweepTimer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _sweepTimer.Stop();
                _sweepTimer.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            UITheme.SetHighQuality(g);

            int size = Math.Min(Width, Height) - 8;
            if (size <= 0) return;

            Rectangle centerRect = new Rectangle((Width - size) / 2, (Height - size) / 2, size, size);
            Point center = new Point(centerRect.X + size / 2, centerRect.Y + size / 2);

            using (Pen ringPen = new Pen(Color.FromArgb(40, UITheme.AccentNeon), 1f))
            {
                g.DrawEllipse(ringPen, centerRect);
                g.DrawEllipse(ringPen, new Rectangle(center.X - size / 3, center.Y - size / 3, size * 2 / 3, size * 2 / 3));
                g.DrawEllipse(ringPen, new Rectangle(center.X - size / 6, center.Y - size / 6, size / 3, size / 3));
            }

            using (Pen axisPen = new Pen(Color.FromArgb(50, UITheme.AccentNeon), 1f))
            {
                g.DrawLine(axisPen, center.X, centerRect.Top, center.X, centerRect.Bottom);
                g.DrawLine(axisPen, centerRect.Left, center.Y, centerRect.Right, center.Y);
            }

            using (GraphicsPath sweepPath = new GraphicsPath())
            {
                sweepPath.AddPie(centerRect, _currentAngle - 40, 40);
                using (PathGradientBrush pgb = new PathGradientBrush(sweepPath))
                {
                    pgb.CenterColor = Color.FromArgb(160, UITheme.AccentNeon);
                    pgb.SurroundColors = new Color[] { Color.FromArgb(0, UITheme.AccentNeon) };
                    g.FillPath(pgb, sweepPath);
                }
            }

            float rad = (float)(_currentAngle * Math.PI / 180.0);
            float endX = center.X + (size / 2f) * (float)Math.Cos(rad);
            float endY = center.Y + (size / 2f) * (float)Math.Sin(rad);

            using (Pen leadPen = new Pen(UITheme.AccentNeon, 2f))
            {
                g.DrawLine(leadPen, center.X, center.Y, endX, endY);
            }

            using (SolidBrush dotBrush = new SolidBrush(UITheme.AccentNeon))
            {
                g.FillEllipse(dotBrush, center.X - 3, center.Y - 3, 6, 6);
            }
        }
    }
}
