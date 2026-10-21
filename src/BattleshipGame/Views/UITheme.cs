using System.Drawing;
using System.Drawing.Drawing2D;

namespace BattleshipGame.Views
{
    /// <summary>
    /// Bảng màu và các tiện ích đồ họa chuẩn Tactical Naval Warfare
    /// </summary>
    public static class UITheme
    {
        // Màu nền chính
        public static readonly Color BackgroundDark = Color.FromArgb(15, 23, 42);      // #0F172A Deep Navy
        public static readonly Color PanelBackground = Color.FromArgb(30, 41, 59);     // #1E293B Dark Slate
        public static readonly Color HeaderBackground = Color.FromArgb(11, 19, 36);    // #0B1324 Extra Deep

        // Màu viền và đường kẻ
        public static readonly Color BorderDefault = Color.FromArgb(51, 65, 85);       // #334155
        public static readonly Color BorderLight = Color.FromArgb(71, 85, 105);        // #475569

        // Màu điểm nhấn Neon & Quân sự
        public static readonly Color PrimaryCyan = Color.FromArgb(14, 165, 233);      // #0EA5E9 Steel Cyan
        public static readonly Color AccentNeon = Color.FromArgb(100, 255, 218);      // #64FFDA Bright Neon
        public static readonly Color SuccessGreen = Color.FromArgb(16, 185, 129);     // #10B981 Emerald Green
        public static readonly Color DangerRed = Color.FromArgb(239, 68, 68);         // #EF4444 Crimson Red
        public static readonly Color WarningAmber = Color.FromArgb(245, 158, 11);     // #F59E0B Amber
        public static readonly Color WarningYellow = Color.FromArgb(234, 179, 8);      // #EAB308 Warning Yellow
        public static readonly Color SunkDarkRed = Color.FromArgb(153, 27, 27);       // #991B1B Dark Red Sunk
        public static readonly Color MissGray = Color.FromArgb(148, 163, 184);        // #94A3B8 Water ripples

        // Màu chữ
        public static readonly Color TextPrimary = Color.FromArgb(241, 245, 249);      // #F1F5F9 Off-white
        public static readonly Color TextSecondary = Color.FromArgb(148, 163, 184);    // #94A3B8 Muted
        public static readonly Color TextMuted = Color.FromArgb(100, 116, 139);        // #64748B Subdued

        // Màu bàn cờ
        public static readonly Color WaterCell = Color.FromArgb(30, 41, 59);          // Nước biển tĩnh
        public static readonly Color WaterCellHover = Color.FromArgb(45, 62, 88);     // Hover ô biển
        public static readonly Color ShipCell = Color.FromArgb(14, 165, 233);         // Ô có tàu
        public static readonly Color PreviewValid = Color.FromArgb(34, 197, 94);      // Xem trước hợp lệ (Green)
        public static readonly Color PreviewInvalid = Color.FromArgb(239, 68, 68);    // Xem trước đè/tràn (Red)

        // Phông chữ chuẩn
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

        /// <summary>
        /// Tạo bo góc mượt mà cho hình chữ nhật
        /// </summary>
        public static GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            int diameter = radius * 2;
            Rectangle arc = new Rectangle(rect.Location, new Size(diameter, diameter));

            // Góc trên trái
            path.AddArc(arc, 180, 90);

            // Góc trên phải
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Góc dưới phải
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Góc dưới trái
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Kích hoạt chế độ vẽ chất lượng cao cho Graphics
        /// </summary>
        public static void SetHighQuality(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        }
    }
}
