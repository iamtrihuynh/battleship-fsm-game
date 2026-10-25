using System.Drawing;
using System.Drawing.Drawing2D;

namespace BattleshipGame.Views
{
    public static class UITheme
    {
        public static readonly Color BackgroundDark = Color.FromArgb(15, 23, 42);
        public static readonly Color PanelBackground = Color.FromArgb(30, 41, 59);
        public static readonly Color HeaderBackground = Color.FromArgb(11, 19, 36);

        public static readonly Color BorderDefault = Color.FromArgb(70, 88, 115);
        public static readonly Color BorderLight = Color.FromArgb(100, 128, 165);

        public static readonly Color PrimaryCyan = Color.FromArgb(14, 165, 233);
        public static readonly Color AccentNeon = Color.FromArgb(100, 255, 218);
        public static readonly Color SuccessGreen = Color.FromArgb(16, 185, 129);
        public static readonly Color DangerRed = Color.FromArgb(239, 68, 68);
        public static readonly Color WarningAmber = Color.FromArgb(245, 158, 11);
        public static readonly Color WarningYellow = Color.FromArgb(234, 179, 8);
        public static readonly Color SunkDarkRed = Color.FromArgb(153, 27, 27);
        public static readonly Color MissGray = Color.FromArgb(148, 163, 184);
        public static readonly Color GoldAccent = Color.FromArgb(250, 204, 21);

        public static readonly Color MissRippleBackground = Color.FromArgb(30, 58, 88);
        public static readonly Color MissRippleBorder = Color.FromArgb(56, 189, 248);
        public static readonly Color MissRippleText = Color.FromArgb(147, 197, 253);
        public static readonly Color HitFireBackground = Color.FromArgb(239, 68, 68);
        public static readonly Color HitFireBorder = Color.FromArgb(252, 165, 165);
        public static readonly Color HitFireText = Color.FromArgb(254, 240, 138);
        public static readonly Color SunkAshBackground = Color.FromArgb(55, 65, 81);
        public static readonly Color SunkThickBorder = Color.FromArgb(153, 27, 27);
        public static readonly int SunkBorderThickness = 2;
        public static readonly Color SunkAshText = Color.FromArgb(226, 232, 240);
        public static readonly Color FogOverlayBackground = Color.FromArgb(240, 10, 17, 32);

        public static readonly Color TextPrimary = Color.FromArgb(241, 245, 249);
        public static readonly Color TextSecondary = Color.FromArgb(148, 163, 184);
        public static readonly Color TextMuted = Color.FromArgb(100, 116, 139);

        public static readonly Color WaterCell = Color.FromArgb(30, 41, 59);
        public static readonly Color WaterCellHover = Color.FromArgb(45, 62, 88);
        public static readonly Color ShipCell = Color.FromArgb(14, 165, 233);
        public static readonly Color PreviewValid = Color.FromArgb(34, 197, 94);
        public static readonly Color PreviewInvalid = Color.FromArgb(239, 68, 68);

        public static Font GetHeaderFont(float size = 18f, FontStyle style = FontStyle.Bold)
        {
            return new Font("Segoe UI", size, style);
        }

        public static Font GetBodyFont(float size = 10f, FontStyle style = FontStyle.Regular)
        {
            return new Font("Segoe UI", size, style);
        }

        public static Font GetMonoFont(float size = 10f, FontStyle style = FontStyle.Bold)
        {
            return new Font("Consolas", size, style);
        }

        public static GraphicsPath CreateRoundedPath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            float diameter = radius * 2f;
            if (diameter > rect.Width) diameter = rect.Width;
            if (diameter > rect.Height) diameter = rect.Height;

            RectangleF arc = new RectangleF(rect.Location, new SizeF(diameter, diameter));

            path.AddArc(arc, 180, 90);

            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);

            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        public static GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
        {
            return CreateRoundedPath(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), radius);
        }

        public static void SetHighQuality(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.Default;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        }
    }
}