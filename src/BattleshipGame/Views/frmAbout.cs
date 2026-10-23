using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
    /// <summary>
    /// [NGÀY 4 - THÀNH VIÊN 2: TRẦN TRỌNG HƯNG - FRONTEND & UI/UX LEAD]
    /// Form Hướng Dẫn Và Giới Thiệu (frmAbout) phong cách Hải quân hiện đại (Dark Theme):
    /// Bấm vào mục nào sẽ hiển thị tức thời thông tin phong phú của mục đó:
    /// 1. 🎯 Luật chơi (có minh họa đồ họa vector trực quan 3 phát bắn Trượt/Trúng/Chìm)
    /// 2. 🚢 Thông tin hạm đội (Đồ họa 5 loại chiến hạm với đầy đủ kích thước, HP, vai trò)
    /// 3. ⚡ Mẹo chiến thuật (Bàn cờ Parity so le chẵn lẻ, Target Mode, Bố trí phòng thủ)
    /// 4. 👥 Thành viên nhóm (Thẻ thông tin 3 thành viên kèm MSSV và phân công WBS chi tiết)
    /// </summary>
    public partial class frmAbout : Form
    {
        private int _currentTabIndex = 0; // 0: Rules, 1: Ships, 2: Tactics, 3: Team

        public frmAbout()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            // Bật double buffering và cấu hình vùng cuộn cho 4 Panel nội dung
            EnableDoubleBuffering(pnlRulesView);
            EnableDoubleBuffering(pnlShipsView);
            EnableDoubleBuffering(pnlTacticsView);
            EnableDoubleBuffering(pnlTeamView);

            pnlRulesView.AutoScrollMinSize = new Size(1000, 600);
            pnlShipsView.AutoScrollMinSize = new Size(1000, 680);
            pnlTacticsView.AutoScrollMinSize = new Size(1000, 560);
            pnlTeamView.AutoScrollMinSize = new Size(1000, 600);

            pnlRulesView.Resize += (s, e) => pnlRulesView.Invalidate();
            pnlShipsView.Resize += (s, e) => pnlShipsView.Invalidate();
            pnlTacticsView.Resize += (s, e) => pnlTacticsView.Invalidate();
            pnlTeamView.Resize += (s, e) => pnlTeamView.Invalidate();

            // Đảm bảo nút đóng luôn hiển thị đúng góc trên bên phải khi thay đổi kích thước
            pnlHeader.Resize += (s, e) => {
                btnClose.Location = new Point(pnlHeader.ClientSize.Width - btnClose.Width - 16, 8);
            };
            this.KeyPreview = true;
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };
            this.CancelButton = btnClose;

            // Mặc định chọn Tab 1: Luật chơi có minh họa
            ChonMucHienThi(0);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            btnClose.Location = new Point(pnlHeader.ClientSize.Width - btnClose.Width - 16, 8);
            btnClose.BringToFront();
        }

        private static void EnableDoubleBuffering(Control c)
        {
            try
            {
                typeof(Control).InvokeMember(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                    null,
                    c,
                    new object[] { true }
                );
            }
            catch { }
        }

        #region ĐIỀU HƯỚNG DANH MỤC (TAB NAVIGATION)
        private void btnTabRules_Click(object sender, EventArgs e) => ChonMucHienThi(0);
        private void btnTabShips_Click(object sender, EventArgs e) => ChonMucHienThi(1);
        private void btnTabTactics_Click(object sender, EventArgs e) => ChonMucHienThi(2);
        private void btnTabTeam_Click(object sender, EventArgs e) => ChonMucHienThi(3);

        /// <summary>
        /// Kích hoạt danh mục được chọn, đổi màu nút nổi bật và hiển thị panel tương ứng
        /// </summary>
        private void ChonMucHienThi(int index)
        {
            _currentTabIndex = index;

            // Đổi giao diện nút bấm
            CapNhatTrangThaiNut(btnTabRules, index == 0, UITheme.AccentNeon);
            CapNhatTrangThaiNut(btnTabShips, index == 1, UITheme.PrimaryCyan);
            CapNhatTrangThaiNut(btnTabTactics, index == 2, UITheme.WarningAmber);
            CapNhatTrangThaiNut(btnTabTeam, index == 3, Color.FromArgb(250, 204, 21));

            // Hiển thị panel tương ứng
            pnlRulesView.Visible = (index == 0);
            pnlShipsView.Visible = (index == 1);
            pnlTacticsView.Visible = (index == 2);
            pnlTeamView.Visible = (index == 3);

            // Refresh vẽ lại
            switch (index)
            {
                case 0: pnlRulesView.Invalidate(); break;
                case 1: pnlShipsView.Invalidate(); break;
                case 2: pnlTacticsView.Invalidate(); break;
                case 3: pnlTeamView.Invalidate(); break;
            }
        }

        private void CapNhatTrangThaiNut(TacticalButton btn, bool isActive, Color activeColor)
        {
            if (isActive)
            {
                btn.BaseFillColor = Color.FromArgb(28, 44, 72);
                btn.BorderColor = activeColor;
                btn.NormalTextColor = activeColor;
            }
            else
            {
                btn.BaseFillColor = Color.FromArgb(16, 26, 46);
                btn.BorderColor = UITheme.BorderDefault;
                btn.NormalTextColor = UITheme.TextSecondary;
            }
            btn.Invalidate();
        }
        #endregion

        #region MỤC 1: LUẬT CHƠI (CÓ MINH HỌA ĐỒ HỌA TRỰC QUAN)
        private void pnlRulesView_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TranslateTransform(pnlRulesView.AutoScrollPosition.X, pnlRulesView.AutoScrollPosition.Y);
            UITheme.SetHighQuality(g);

            int viewWidth = Math.Max(980, pnlRulesView.ClientSize.Width - 36);
            int cardW = (viewWidth - 32) / 3;
            int cardH = 250;
            int topY = 16;

            // 1. MINH HỌA 3 PHÁT BẮN (TRƯỢT, TRÚNG, CHÌM)
            Rectangle rectMiss = new Rectangle(16, topY, cardW, cardH);
            Rectangle rectHit = new Rectangle(rectMiss.Right + 16, topY, cardW, cardH);
            Rectangle rectSunk = new Rectangle(rectHit.Right + 16, topY, cardW, cardH);

            VeCardMinhHoaPhatBan(
                g,
                rectMiss,
                "🌊 Phát bắn trượt (Miss)",
                "•",
                UITheme.MissRippleBackground,
                UITheme.MissRippleBorder,
                UITheme.MissRippleText,
                "⛔  Mất lượt",
                Color.FromArgb(148, 163, 184),
                "• Bắn vào ô nước biển trống (không có tàu).\n" +
                "• Giao diện đổi thành ô sóng nước xanh lam kèm ký tự '•'.\n" +
                "• Mất quyền bắn, lượt ngay lập tức chuyển sang cho đối thủ."
            );

            VeCardMinhHoaPhatBan(
                g,
                rectHit,
                "💥 Phát bắn trúng (Hit)",
                "X",
                UITheme.HitFireBackground,
                UITheme.HitFireBorder,
                UITheme.HitFireText,
                "🔥  Thưởng lượt (+1)",
                UITheme.AccentNeon,
                "• Bắn trúng 1 ô bộ phận của chiến hạm đối phương.\n" +
                "• Giao diện đổi sang màu đỏ rực lửa kèm ký tự 'X'.\n" +
                "• Được thưởng bắn tiếp một lượt nữa!"
            );

            VeCardMinhHoaTauChim(
                g,
                rectSunk,
                "☠️ Bắn chìm tàu (Sunk)",
                "🏆  Hạ gục tàu",
                UITheme.DangerRed,
                "• Bắn trúng ô cuối cùng của một chiến hạm.\n" +
                "• Toàn bộ các ô của tàu đổi sang xám tro, viền đậm 'X'.\n" +
                "• Còi hụ báo hiệu tàu chìm và được bắn tiếp lượt mới!"
            );

            // 2. CARD QUY TẮC CHIẾN TRƯỜNG & ĐIỀU KIỆN THẮNG
            int ruleTop = rectMiss.Bottom + 16;
            Rectangle rectRuleBox = new Rectangle(16, ruleTop, rectSunk.Right - 16, 285);

            using (GraphicsPath path = UITheme.CreateRoundedPath(rectRuleBox, 8))
            {
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(15, 23, 42))) g.FillPath(bg, path);
                using (Pen pen = new Pen(UITheme.PrimaryCyan, 1.2f)) g.DrawPath(pen, path);
            }

            using (Font titleFont = new Font("Segoe UI", 12f, FontStyle.Bold))
            {
                Rectangle tRect = new Rectangle(rectRuleBox.X + 16, rectRuleBox.Y + 12, rectRuleBox.Width - 32, 26);
                TextRenderer.DrawText(g, "🎯 Quy tắc tác chiến & Điều kiện toàn thắng", titleFont, tRect, UITheme.AccentNeon, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
            }

            var rules = new (string Icon, string Title, string Desc)[]
            {
                ("🗺️", "Vùng biển quy chuẩn", "Mỗi bên sở hữu bàn cờ 10x10 = 100 ô tọa độ (Hàng A - J, Cột 1 - 10). Bố trí bí mật hạm đội, không chồng lấn và không vượt biên."),
                ("⚓", "Hạm đội tham chiến", "Gồm 5 chiến hạm từ 2 đến 5 ô (Tàu sân bay: 5 ô, Thiết giáp hạm: 4 ô, Tàu ngầm: 3 ô, Tàu khu trục: 3 ô, Tàu tuần tra: 2 ô). Tổng cộng 17 ô máu."),
                ("⏳", "Đồng hồ đếm ngược 30s", "Mỗi lượt khai hỏa có tối đa 30 giây để ngắm bắn. Nếu hết 30 giây mà không bắn, hệ thống sẽ tự động tính là mất lượt và chuyển quyền bắn."),
                ("🏆", "Điều kiện chiến thắng", "Bên nào tiêu diệt toàn bộ 5 chiến hạm (bắn chìm đủ 17/17 ô tàu) của đối phương trước sẽ giành chiến thắng tuyệt đối chung cuộc!")
            };

            int startY = rectRuleBox.Y + 46;
            for (int i = 0; i < rules.Length; i++)
            {
                var (icon, rTitle, rDesc) = rules[i];
                Rectangle lineRect = new Rectangle(rectRuleBox.X + 16, startY + i * 56, rectRuleBox.Width - 32, 50);

                using (Font iFont = new Font("Segoe UI", 14f))
                using (Font hFont = new Font("Segoe UI", 10.5f, FontStyle.Bold))
                using (Font dFont = new Font("Segoe UI", 9.5f, FontStyle.Regular))
                {
                    TextRenderer.DrawText(g, icon, iFont, new Rectangle(lineRect.X, lineRect.Y, 32, 32), Color.White, TextFormatFlags.Left);
                    TextRenderer.DrawText(g, rTitle, hFont, new Rectangle(lineRect.X + 36, lineRect.Y, lineRect.Width - 36, 20), UITheme.PrimaryCyan, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
                    TextRenderer.DrawText(g, rDesc, dFont, new Rectangle(lineRect.X + 36, lineRect.Y + 22, lineRect.Width - 36, 28), UITheme.TextPrimary, TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
                }
            }
        }

        private void VeCardMinhHoaPhatBan(
            Graphics g,
            Rectangle rect,
            string title,
            string symbol,
            Color cellBg,
            Color cellBorder,
            Color symbolColor,
            string badgeText,
            Color badgeColor,
            string description)
        {
            using (GraphicsPath path = UITheme.CreateRoundedPath(rect, 8))
            {
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(15, 23, 42))) g.FillPath(bg, path);
                using (Pen pen = new Pen(cellBorder, 1.2f)) g.DrawPath(pen, path);
            }

            // Tiêu đề thẻ
            using (Font titleFont = new Font("Segoe UI", 10.5f, FontStyle.Bold))
            {
                Rectangle tRect = new Rectangle(rect.X + 12, rect.Y + 10, rect.Width - 24, 24);
                TextRenderer.DrawText(g, title, titleFont, tRect, cellBorder, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
            }

            // Ô cờ minh họa
            Rectangle sampleCell = new Rectangle(rect.X + 16, rect.Y + 40, 52, 52);
            using (SolidBrush brush = new SolidBrush(cellBg)) g.FillRectangle(brush, sampleCell);
            using (Pen pen = new Pen(cellBorder, 1.5f)) g.DrawRectangle(pen, sampleCell);
            using (Font symFont = new Font("Segoe UI", symbol == "•" ? 22f : 16f, FontStyle.Bold))
            {
                TextRenderer.DrawText(g, symbol, symFont, sampleCell, symbolColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            // Badge trạng thái
            int badgeW = Math.Max(120, rect.Right - sampleCell.Right - 26);
            Rectangle badgeRect = new Rectangle(sampleCell.Right + 12, rect.Y + 44, badgeW, 28);
            using (GraphicsPath bPath = UITheme.CreateRoundedPath(badgeRect, 4))
            {
                using (SolidBrush bBg = new SolidBrush(Color.FromArgb(24, 38, 60))) g.FillPath(bBg, bPath);
                using (Pen bPen = new Pen(badgeColor, 1f)) g.DrawPath(bPen, bPath);
            }
            using (Font bFont = new Font("Segoe UI", 9f, FontStyle.Bold))
            {
                TextRenderer.DrawText(g, badgeText, bFont, badgeRect, badgeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }

            // Mô tả
            Rectangle descRect = new Rectangle(rect.X + 16, sampleCell.Bottom + 12, rect.Width - 32, 130);
            using (Font descFont = new Font("Segoe UI", 9.25f, FontStyle.Regular))
            {
                TextRenderer.DrawText(g, description, descFont, descRect, UITheme.TextPrimary, TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
            }
        }

        private void VeCardMinhHoaTauChim(Graphics g, Rectangle rect, string title, string badgeText, Color badgeColor, string description)
        {
            using (GraphicsPath path = UITheme.CreateRoundedPath(rect, 8))
            {
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(15, 23, 42))) g.FillPath(bg, path);
                using (Pen pen = new Pen(UITheme.DangerRed, 1.5f)) g.DrawPath(pen, path);
            }

            using (Font titleFont = new Font("Segoe UI", 10.5f, FontStyle.Bold))
            {
                Rectangle tRect = new Rectangle(rect.X + 14, rect.Y + 10, rect.Width - 28, 24);
                TextRenderer.DrawText(g, title, titleFont, tRect, UITheme.DangerRed, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
            }

            // Vẽ 3 ô tàu chìm liền nhau
            int startX = rect.X + 16;
            int startY = rect.Y + 44;
            int cellSize = 28;
            for (int i = 0; i < 3; i++)
            {
                Rectangle cell = new Rectangle(startX + i * (cellSize + 4), startY, cellSize, cellSize);
                using (SolidBrush brush = new SolidBrush(UITheme.SunkAshBackground)) g.FillRectangle(brush, cell);
                using (Pen pen = new Pen(UITheme.SunkThickBorder, 2f)) g.DrawRectangle(pen, cell); // Viền đậm 2px
                using (Font symFont = new Font("Segoe UI", 11f, FontStyle.Bold))
                {
                    TextRenderer.DrawText(g, "X", symFont, cell, UITheme.SunkAshText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }

            // Badge (được giới hạn hoàn toàn bên trong thẻ, không tràn ra ngoài)
            int badgeX = startX + 3 * (cellSize + 4) + 8;
            int badgeW = Math.Max(100, rect.Right - badgeX - 16);
            Rectangle badgeRect = new Rectangle(badgeX, startY + 2, badgeW, 28);
            using (GraphicsPath bPath = UITheme.CreateRoundedPath(badgeRect, 4))
            {
                using (SolidBrush bBg = new SolidBrush(Color.FromArgb(40, 20, 24))) g.FillPath(bBg, bPath);
                using (Pen bPen = new Pen(badgeColor, 1f)) g.DrawPath(bPen, bPath);
            }
            using (Font bFont = new Font("Segoe UI", 8.5f, FontStyle.Bold))
            {
                TextRenderer.DrawText(g, badgeText, bFont, badgeRect, badgeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }

            // Mô tả
            Rectangle descRect = new Rectangle(rect.X + 16, startY + cellSize + 16, rect.Width - 32, 130);
            using (Font descFont = new Font("Segoe UI", 9.25f, FontStyle.Regular))
            {
                TextRenderer.DrawText(g, description, descFont, descRect, UITheme.TextPrimary, TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
            }
        }
        #endregion

        #region MỤC 2: THÔNG TIN HẠM ĐỘI (5 CHIẾN HẠM CÓ ĐỒ HỌA VECTOR)
        private void pnlShipsView_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TranslateTransform(pnlShipsView.AutoScrollPosition.X, pnlShipsView.AutoScrollPosition.Y);
            UITheme.SetHighQuality(g);

            var danhSachTau = new (string TenVN, string TenEN, int Size, string VaiTro, string Icon, Color AccentColor)[]
            {
                ("Tàu sân bay", "Aircraft Carrier", 5, "Soái hạm nổi đầu não của hạm đội, chở tiêm kích phòng không. Kích thước 5 ô lớn nhất, là mục tiêu săn lùng hàng đầu.", "✈️", Color.FromArgb(56, 189, 248)),
                ("Thiết giáp hạm", "Battleship", 4, "Tàu pháo bọc thép hạng nặng, sở hữu đại bác hạm đội công phá tầm xa cực kỳ uy lực.", "💣", Color.FromArgb(245, 158, 11)),
                ("Tàu ngầm", "Submarine", 3, "Sát thủ lặn sâu tàng hình dưới lòng đại dương, di chuyển lẩn khuất, khó phán đoán tọa độ.", "🌊", Color.FromArgb(16, 185, 129)),
                ("Tàu khu trục", "Destroyer", 3, "Tàu hộ tống cơ động cao, bệ phóng tên lửa hành trình, chuyên săn lùng tàu ngầm và bảo vệ hạm đội.", "🚀", Color.FromArgb(168, 85, 247)),
                ("Tàu tuần tra", "Patrol Boat", 2, "Trinh sát cao tốc nhỏ nhất (chỉ 2 ô). Cực kỳ khó phát hiện trên bàn cờ 100 ô, là chìa khóa quyết định thắng bại!", "⚡", Color.FromArgb(244, 63, 94))
            };

            int startY = 16;
            int cardW = Math.Max(980, pnlShipsView.ClientSize.Width - 36);
            int cardH = 116;

            for (int i = 0; i < danhSachTau.Length; i++)
            {
                var (tenVN, tenEN, size, vaiTro, icon, accentColor) = danhSachTau[i];
                Rectangle cardRect = new Rectangle(16, startY + i * (cardH + 14), cardW, cardH);

                // Khung card
                using (GraphicsPath path = UITheme.CreateRoundedPath(cardRect, 8))
                {
                    using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(15, 23, 42))) g.FillPath(bgBrush, path);
                    using (Pen borderPen = new Pen(accentColor, 1.2f)) g.DrawPath(borderPen, path);
                }

                // Tiêu đề & NATO code
                using (Font nameFont = new Font("Segoe UI", 12f, FontStyle.Bold))
                using (Font enFont = new Font("Segoe UI", 9.5f, FontStyle.Regular))
                {
                    Rectangle nameRect = new Rectangle(cardRect.X + 16, cardRect.Y + 12, 300, 26);
                    TextRenderer.DrawText(g, $"{icon}  {tenVN} ({size} ô)", nameFont, nameRect, accentColor, TextFormatFlags.Left | TextFormatFlags.NoPrefix);

                    Rectangle enRect = new Rectangle(cardRect.X + 18, cardRect.Y + 38, 300, 20);
                    TextRenderer.DrawText(g, $"Mã hiệu: {tenEN} • Độ bền: {size}/{size} HP", enFont, enRect, UITheme.TextSecondary, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
                }

                // Vẽ các ô mô phỏng thân tàu
                int cellStartX = cardRect.X + 320;
                int cellY = cardRect.Y + 14;
                int cellSize = 32;

                for (int s = 0; s < size; s++)
                {
                    Rectangle segRect = new Rectangle(cellStartX + s * (cellSize + 4), cellY, cellSize, cellSize);
                    using (SolidBrush segBrush = new SolidBrush(UITheme.ShipCell)) g.FillRectangle(segBrush, segRect);
                    using (Pen segPen = new Pen(accentColor, 1.2f)) g.DrawRectangle(segPen, segRect);
                    using (Font sFont = new Font("Segoe UI", 9.5f, FontStyle.Bold))
                    {
                        TextRenderer.DrawText(g, "⚓", sFont, segRect, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    }
                }

                // Mô tả vai trò
                Rectangle roleRect = new Rectangle(cellStartX, cardRect.Y + 54, cardRect.Width - 340, 50);
                using (Font roleFont = new Font("Segoe UI", 9.25f, FontStyle.Regular))
                {
                    TextRenderer.DrawText(g, vaiTro, roleFont, roleRect, UITheme.TextPrimary, TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
                }
            }
        }
        #endregion

        #region MỤC 3: MẸO CHIẾN THUẬT (PARITY SO LE, TARGET MODE, PHÒNG THỦ)
        private void pnlTacticsView_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TranslateTransform(pnlTacticsView.AutoScrollPosition.X, pnlTacticsView.AutoScrollPosition.Y);
            UITheme.SetHighQuality(g);

            int viewWidth = Math.Max(980, pnlTacticsView.ClientSize.Width - 36);
            int cardW = (viewWidth - 32) / 3;
            int cardH = 240;
            int topY = 16;

            // 1. SĂN SO LE (PARITY HUNTING)
            Rectangle rectParity = new Rectangle(16, topY, cardW, cardH);
            VeCardParity(g, rectParity);

            // 2. BÁM ĐUỔI 4 HƯỚNG (TARGET MODE)
            Rectangle rectTarget = new Rectangle(rectParity.Right + 16, topY, cardW, cardH);
            VeCardTargetMode(g, rectTarget);

            // 3. BỐ TRÍ PHÒNG THỦ (DEFENSE STRATEGY)
            Rectangle rectDefense = new Rectangle(rectTarget.Right + 16, topY, cardW, cardH);
            VeCardDefenseStrategy(g, rectDefense);

            // 4. KỸ NĂNG ĐẶC BIỆT DƯỚI CÙNG
            int skillTop = rectParity.Bottom + 16;
            Rectangle rectSkillBox = new Rectangle(16, skillTop, rectDefense.Right - 16, 220);

            using (GraphicsPath path = UITheme.CreateRoundedPath(rectSkillBox, 8))
            {
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(15, 23, 42))) g.FillPath(bg, path);
                using (Pen pen = new Pen(UITheme.WarningAmber, 1.2f)) g.DrawPath(pen, path);
            }

            using (Font titleFont = new Font("Segoe UI", 12f, FontStyle.Bold))
            {
                Rectangle tRect = new Rectangle(rectSkillBox.X + 16, rectSkillBox.Y + 12, rectSkillBox.Width - 32, 26);
                TextRenderer.DrawText(g, "📡 Mẹo sử dụng kỹ năng tác chiến đặc biệt", titleFont, tRect, UITheme.WarningAmber, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
            }

            var skills = new (string Icon, string Name, string Desc)[]
            {
                ("📡", "Radar trinh sát (3x3)", "Quét một vùng biển 3x3 ô nghi vấn. Hệ thống thông báo 'Phát hiện có dấu vết tàu địch' hoặc 'Vùng biển an toàn' mà không tiết lộ ô chính xác. Rất hữu ích khi cần xác định tàu tuần tra 2 ô đang trốn ở góc nào!"),
                ("💥", "Pháo kích diện rộng (+)", "Khai hỏa đồng loạt 5 quả đại bác theo hình dấu cộng (+) cùng một lúc. Giúp càn quét cực nhanh các vùng biển trống và gia tăng xác suất bắn trúng các tàu cỡ lớn (4-5 ô)!")
            };

            for (int i = 0; i < skills.Length; i++)
            {
                var (icon, sName, sDesc) = skills[i];
                Rectangle sRect = new Rectangle(rectSkillBox.X + 16, rectSkillBox.Y + 46 + i * 82, rectSkillBox.Width - 32, 74);

                using (Font iFont = new Font("Segoe UI", 16f))
                using (Font hFont = new Font("Segoe UI", 10.5f, FontStyle.Bold))
                using (Font dFont = new Font("Segoe UI", 9f, FontStyle.Regular))
                {
                    TextRenderer.DrawText(g, icon, iFont, new Rectangle(sRect.X, sRect.Y, 36, 36), Color.White, TextFormatFlags.Left);
                    TextRenderer.DrawText(g, sName, hFont, new Rectangle(sRect.X + 44, sRect.Y, sRect.Width - 44, 22), UITheme.AccentNeon, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
                    TextRenderer.DrawText(g, sDesc, dFont, new Rectangle(sRect.X + 44, sRect.Y + 24, sRect.Width - 44, 46), UITheme.TextPrimary, TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
                }
            }
        }

        private void VeCardParity(Graphics g, Rectangle rect)
        {
            using (GraphicsPath path = UITheme.CreateRoundedPath(rect, 8))
            {
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(15, 23, 42))) g.FillPath(bg, path);
                using (Pen pen = new Pen(UITheme.AccentNeon, 1.2f)) g.DrawPath(pen, path);
            }

            using (Font titleFont = new Font("Segoe UI", 10.5f, FontStyle.Bold))
            {
                Rectangle tRect = new Rectangle(rect.X + 14, rect.Y + 10, rect.Width - 28, 24);
                TextRenderer.DrawText(g, "🏁 Bắn so le chẵn lẻ (Parity)", titleFont, tRect, UITheme.AccentNeon, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
            }

            // Vẽ bàn cờ vua mini 4x4
            int startX = rect.X + 16;
            int startY = rect.Y + 40;
            int sz = 24;

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    Rectangle cRect = new Rectangle(startX + c * sz, startY + r * sz, sz, sz);
                    bool isParity = (r + c) % 2 == 0;
                    Color color = isParity ? UITheme.SuccessGreen : Color.FromArgb(30, 41, 59);

                    using (SolidBrush b = new SolidBrush(color)) g.FillRectangle(b, cRect);
                    using (Pen p = new Pen(UITheme.BorderDefault, 1f)) g.DrawRectangle(p, cRect);

                    if (isParity)
                    {
                        using (Font pf = new Font("Segoe UI", 8f, FontStyle.Bold))
                        {
                            TextRenderer.DrawText(g, "🎯", pf, cRect, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                        }
                    }
                }
            }

            Rectangle dRect = new Rectangle(startX + 4 * sz + 12, startY, rect.Width - (4 * sz) - 36, 175);
            using (Font df = new Font("Segoe UI", 9f, FontStyle.Regular))
            {
                TextRenderer.DrawText(
                    g,
                    "Nguyên lý toán học:\n" +
                    "• Tàu nhỏ nhất dài 2 ô bắt buộc phải đè lên ít nhất một ô chẵn lẻ ((X+Y)%2 == 0).\n" +
                    "• Bắn so le giúp loại trừ 50% số phát bắn vu vơ, tăng tốc độ tìm tàu đối phương!",
                    df,
                    dRect,
                    UITheme.TextPrimary,
                    TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix
                );
            }
        }

        private void VeCardTargetMode(Graphics g, Rectangle rect)
        {
            using (GraphicsPath path = UITheme.CreateRoundedPath(rect, 8))
            {
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(15, 23, 42))) g.FillPath(bg, path);
                using (Pen pen = new Pen(UITheme.HitFireBackground, 1.2f)) g.DrawPath(pen, path);
            }

            using (Font titleFont = new Font("Segoe UI", 10.5f, FontStyle.Bold))
            {
                Rectangle tRect = new Rectangle(rect.X + 14, rect.Y + 10, rect.Width - 28, 24);
                TextRenderer.DrawText(g, "🎯 Bám đuổi 4 hướng (Target)", titleFont, tRect, UITheme.HitFireBackground, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
            }

            // Vẽ chữ thập 3x3 với tâm là HIT
            int startX = rect.X + 16;
            int startY = rect.Y + 40;
            int sz = 30;

            Rectangle centerCell = new Rectangle(startX + sz, startY + sz, sz, sz);
            using (SolidBrush b = new SolidBrush(UITheme.HitFireBackground)) g.FillRectangle(b, centerCell);
            using (Pen p = new Pen(UITheme.HitFireBorder, 1.5f)) g.DrawRectangle(p, centerCell);
            using (Font sf = new Font("Segoe UI", 11f, FontStyle.Bold))
            {
                TextRenderer.DrawText(g, "X", sf, centerCell, UITheme.HitFireText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            var dirs = new (int dx, int dy, string icon)[]
            {
                (1, 0, "↑"),
                (1, 2, "↓"),
                (0, 1, "←"),
                (2, 1, "→")
            };

            foreach (var (dx, dy, icon) in dirs)
            {
                Rectangle cell = new Rectangle(startX + dx * sz, startY + dy * sz, sz, sz);
                using (SolidBrush b = new SolidBrush(Color.FromArgb(30, 41, 59))) g.FillRectangle(b, cell);
                using (Pen p = new Pen(UITheme.AccentNeon, 1.2f)) g.DrawRectangle(p, cell);
                using (Font af = new Font("Segoe UI", 11f, FontStyle.Bold))
                {
                    TextRenderer.DrawText(g, icon, af, cell, UITheme.AccentNeon, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }

            Rectangle dRect = new Rectangle(startX + 3 * sz + 12, startY, rect.Width - (3 * sz) - 36, 175);
            using (Font df = new Font("Segoe UI", 9f, FontStyle.Regular))
            {
                TextRenderer.DrawText(
                    g,
                    "Ngay khi bắn trúng ô X:\n" +
                    "• Đẩy 4 ô kề (↑ ↓ ← →) vào danh sách ưu tiên ngắm bắn.\n" +
                    "• Khi trúng phát thứ 2: Xác định trục tàu (dọc/ngang) và dồn hỏa lực tiêu diệt!",
                    df,
                    dRect,
                    UITheme.TextPrimary,
                    TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix
                );
            }
        }

        private void VeCardDefenseStrategy(Graphics g, Rectangle rect)
        {
            using (GraphicsPath path = UITheme.CreateRoundedPath(rect, 8))
            {
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(15, 23, 42))) g.FillPath(bg, path);
                using (Pen pen = new Pen(UITheme.PrimaryCyan, 1.2f)) g.DrawPath(pen, path);
            }

            using (Font titleFont = new Font("Segoe UI", 10.5f, FontStyle.Bold))
            {
                Rectangle tRect = new Rectangle(rect.X + 14, rect.Y + 10, rect.Width - 28, 24);
                TextRenderer.DrawText(g, "🛡️ Bố trí phòng thủ", titleFont, tRect, UITheme.PrimaryCyan, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
            }

            Rectangle iconBox = new Rectangle(rect.X + 16, rect.Y + 45, 60, 60);
            using (SolidBrush b = new SolidBrush(Color.FromArgb(10, 17, 32))) g.FillRectangle(b, iconBox);
            using (Pen p = new Pen(UITheme.PrimaryCyan, 1.5f)) g.DrawRectangle(p, iconBox);
            using (Font ifont = new Font("Segoe UI", 26f))
            {
                TextRenderer.DrawText(g, "🛡️", ifont, iconBox, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            Rectangle dRect = new Rectangle(iconBox.Right + 12, rect.Y + 40, rect.Width - iconBox.Width - 36, 175);
            using (Font df = new Font("Segoe UI", 9f, FontStyle.Regular))
            {
                TextRenderer.DrawText(
                    g,
                    "Chiến thuật xếp đội hình:\n" +
                    "• Phân tán các tàu, không đặt sát dính liền nhau để tránh bị pháo kích trúng nhiều tàu.\n" +
                    "• Đặt xen kẽ tàu dọc và ngang, tận dụng mép bàn cờ để bẻ gãy phán đoán của máy!\n" +
                    "• Giấu tàu tuần tra 2 ô ở vị trí bất ngờ nhất.",
                    df,
                    dRect,
                    UITheme.TextPrimary,
                    TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix
                );
            }
        }
        #endregion

        #region MỤC 4: THÀNH VIÊN NHÓM (3 THẺ THÀNH VIÊN & PHÂN CÔNG WBS)
        private void pnlTeamView_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TranslateTransform(pnlTeamView.AutoScrollPosition.X, pnlTeamView.AutoScrollPosition.Y);
            UITheme.SetHighQuality(g);

            var danhSachThanhVien = new (string VaiTro, string HoTen, string MSSV, string NhiemVuChinh, Color AccentColor, string Icon)[]
            {
                (
                    "Thành viên 1 - Trưởng nhóm / Backend & AI Lead",
                    "Huỳnh Minh Trí",
                    "51.01.104.105",
                    "• Thiết kế kiến trúc Model: cO.cs, cTau.cs, cBanCo.cs.\n• Thuật toán kiểm tra va chạm và xếp tàu tự động hợp lệ.\n• Thuật toán trí tuệ nhân tạo Bot AI (Hunt Mode Parity & Target Mode bám đuổi).\n• Điều phối ván đấu cQuanLyVanDau.cs và công thức tính điểm.",
                    UITheme.AccentNeon,
                    "👑"
                ),
                (
                    "Thành viên 2 - Frontend & UI/UX Lead",
                    "Trần Trọng Hưng",
                    "51.01.104.132",
                    "• Thiết kế toàn bộ giao diện WinForms chuẩn phong cách Hải quân hiện đại.\n• Xử lý đồ họa 2 bàn cờ, hiệu ứng bóng mờ (Preview Shadow) khi bố trí tàu.\n• Đồ họa phát bắn (Sóng nước •, Lửa đỏ X, Tro tàn chìm tàu viền đậm 2px).\n• Thiết kế giao diện Hướng dẫn tác chiến trực quan, tối ưu trải nghiệm người dùng.",
                    UITheme.PrimaryCyan,
                    "🎨"
                ),
                (
                    "Thành viên 3 - System I/O, Audio & QA/Tester",
                    "Trần Vũ Hòa Phát",
                    "51.01.104.072",
                    "• Tích hợp hệ thống âm thanh đa luồng: Nhạc nền mciSendString và SFX SoundPlayer.\n• Quản lý lưu trữ/đọc bảng xếp hạng thanhtichs.txt qua cThanhTichDAO.cs.\n• Cơ chế bẫy lỗi toàn diện phòng chống ngoại lệ.\n• Soạn thảo bộ kịch bản kiểm thử, tài liệu báo cáo đồ án và video demo.",
                    UITheme.WarningAmber,
                    "🔊"
                )
            };

            int startY = 16;
            int cardW = Math.Max(980, pnlTeamView.ClientSize.Width - 36);
            int cardH = 158;

            for (int i = 0; i < danhSachThanhVien.Length; i++)
            {
                var (vaiTro, hoTen, mssv, nhiemVu, accentColor, icon) = danhSachThanhVien[i];
                Rectangle cardRect = new Rectangle(16, startY + i * (cardH + 16), cardW, cardH);

                // Khung card
                using (GraphicsPath path = UITheme.CreateRoundedPath(cardRect, 8))
                {
                    using (SolidBrush bg = new SolidBrush(Color.FromArgb(15, 23, 42))) g.FillPath(bg, path);
                    using (Pen pen = new Pen(accentColor, 1.2f)) g.DrawPath(pen, path);
                }

                // Tiêu đề, Tên và MSSV
                using (Font roleFont = new Font("Segoe UI", 9.5f, FontStyle.Bold))
                using (Font nameFont = new Font("Segoe UI", 13.5f, FontStyle.Bold))
                using (Font mssvFont = new Font("Consolas", 11.5f, FontStyle.Bold))
                {
                    Rectangle roleRect = new Rectangle(cardRect.X + 16, cardRect.Y + 12, 380, 22);
                    TextRenderer.DrawText(g, $"{icon}  {vaiTro}", roleFont, roleRect, accentColor, TextFormatFlags.Left | TextFormatFlags.NoPrefix);

                    Rectangle nameRect = new Rectangle(cardRect.X + 16, cardRect.Y + 38, 380, 28);
                    TextRenderer.DrawText(g, hoTen, nameFont, nameRect, Color.White, TextFormatFlags.Left | TextFormatFlags.NoPrefix);

                    Rectangle mssvRect = new Rectangle(cardRect.X + 16, cardRect.Y + 68, 380, 24);
                    TextRenderer.DrawText(g, $"MSSV: {mssv}", mssvFont, mssvRect, UITheme.GoldAccent, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
                }

                // Nhiệm vụ WBS
                int taskStartX = cardRect.X + 410;
                Rectangle taskRect = new Rectangle(taskStartX, cardRect.Y + 12, cardRect.Width - 426, cardRect.Height - 20);
                using (Font taskFont = new Font("Segoe UI", 9.25f, FontStyle.Regular))
                {
                    TextRenderer.DrawText(g, $"Phân công nhiệm vụ chi tiết (WBS):\n{nhiemVu}", taskFont, taskRect, UITheme.TextPrimary, TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
                }
            }
        }
        #endregion
    }
}
