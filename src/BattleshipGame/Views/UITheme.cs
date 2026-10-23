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
        public static readonly Color BorderDefault = Color.FromArgb(70, 88, 115);       // #465873 Rõ ràng, sắc nét
        public static readonly Color BorderLight = Color.FromArgb(100, 128, 165);        // #6480A5 Tương phản cao, nổi bật

        // Màu điểm nhấn Neon & Quân sự
        public static readonly Color PrimaryCyan = Color.FromArgb(14, 165, 233);      // #0EA5E9 Steel Cyan
        public static readonly Color AccentNeon = Color.FromArgb(100, 255, 218);      // #64FFDA Bright Neon
        public static readonly Color SuccessGreen = Color.FromArgb(16, 185, 129);     // #10B981 Emerald Green
        public static readonly Color DangerRed = Color.FromArgb(239, 68, 68);         // #EF4444 Crimson Red
        public static readonly Color WarningAmber = Color.FromArgb(245, 158, 11);     // #F59E0B Amber
        public static readonly Color WarningYellow = Color.FromArgb(234, 179, 8);      // #EAB308 Warning Yellow
        public static readonly Color SunkDarkRed = Color.FromArgb(153, 27, 27);       // #991B1B Dark Red Sunk
        public static readonly Color MissGray = Color.FromArgb(148, 163, 184);        // #94A3B8 Water ripples
        public static readonly Color GoldAccent = Color.FromArgb(250, 204, 21);       // #FACC15 Trophy Gold

        // Màu xử lý đồ họa phát bắn (Ngày 3 - TV2)
        public static readonly Color MissRippleBackground = Color.FromArgb(30, 58, 88);       // #1E3A58 Gợn sóng nước biển
        public static readonly Color MissRippleBorder = Color.FromArgb(56, 189, 248);         // #38BDF8 Viền sóng nhạt
        public static readonly Color MissRippleText = Color.FromArgb(147, 197, 253);           // #93C5FD Ký tự sóng •
        public static readonly Color HitFireBackground = Color.FromArgb(239, 68, 68);         // #EF4444 Lửa cháy đỏ rực
        public static readonly Color HitFireBorder = Color.FromArgb(252, 165, 165);            // #FCA5A5 Viền lửa cam rực
        public static readonly Color HitFireText = Color.FromArgb(254, 240, 138);             // #FEF08A Ký tự X vàng lửa
        public static readonly Color SunkAshBackground = Color.FromArgb(55, 65, 81);          // #374151 Tro tàn tối sẫm
        public static readonly Color SunkThickBorder = Color.FromArgb(153, 27, 27);           // #991B1B Viền đậm 2px đỏ thẫm
        public static readonly int SunkBorderThickness = 2;                                   // Độ dày viền tàu chìm: 2px
        public static readonly Color SunkAshText = Color.FromArgb(226, 232, 240);             // #E2E8F0 Ký tự X màu tro tàn
        public static readonly Color FogOverlayBackground = Color.FromArgb(240, 10, 17, 32);   // Màn che sương mù bán trong suốt

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
        /// Tạo bo góc mượt mà cho hình chữ nhật dạng số thực (RectangleF)
        /// </summary>
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
        /// Tạo bo góc mượt mà cho hình chữ nhật nguyên (Rectangle)
        /// </summary>
        public static GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
        {
            return CreateRoundedPath(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), radius);
        }

        /// <summary>
        /// Kích hoạt chế độ vẽ chất lượng cao sắc nét cho Graphics
        /// </summary>
        public static void SetHighQuality(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.Default;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        }
    }
}