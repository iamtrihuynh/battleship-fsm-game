using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
    public partial class frmAbout : Form
    {
        private int _currentTabIndex = 0;

        public frmAbout()
        {
            InitializeComponent();
            pnlTopContainer.SendToBack();
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

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

            pnlRulesView.Scroll += (s, e) => pnlRulesView.Invalidate();
            pnlShipsView.Scroll += (s, e) => pnlShipsView.Invalidate();
            pnlTacticsView.Scroll += (s, e) => pnlTacticsView.Invalidate();
            pnlTeamView.Scroll += (s, e) => pnlTeamView.Invalidate();

            pnlRulesView.MouseWheel += (s, e) => pnlRulesView.Invalidate();
            pnlShipsView.MouseWheel += (s, e) => pnlShipsView.Invalidate();
            pnlTacticsView.MouseWheel += (s, e) => pnlTacticsView.Invalidate();
            pnlTeamView.MouseWheel += (s, e) => pnlTeamView.Invalidate();

            pnlHeader.Resize += (s, e) => {
                btnClose.Location = new Point(pnlHeader.ClientSize.Width - btnClose.Width - 16, 8);
                lblHeaderTitle.Location = new Point((pnlHeader.ClientSize.Width - lblHeaderTitle.Width) / 2, (pnlHeader.ClientSize.Height - lblHeaderTitle.Height) / 2);
            };
            this.KeyPreview = true;
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };
            this.CancelButton = btnClose;

            KhoiTaoGiaoDienRulesView();
            KhoiTaoGiaoDienShipsView();
            KhoiTaoGiaoDienTacticsView();
            KhoiTaoGiaoDienTeamView();

            ChonMucHienThi(0);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            pnlTopContainer.SendToBack();
            btnClose.Location = new Point(pnlHeader.ClientSize.Width - btnClose.Width - 16, 8);
            btnClose.BringToFront();
            lblHeaderTitle.Location = new Point((pnlHeader.ClientSize.Width - lblHeaderTitle.Width) / 2, (pnlHeader.ClientSize.Height - lblHeaderTitle.Height) / 2);
            lblHeaderTitle.TextAlign = ContentAlignment.MiddleCenter;
            KiemTraVaNapLogo();
        }

        private void KiemTraVaNapLogo()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);
                string imgDir = System.IO.Path.Combine(baseDir, "img");
                if (!System.IO.Directory.Exists(imgDir))
                {
                    TacticalMessageBox.ShowWarning(
                        this,
                        "Bẫy lỗi tài nguyên (img/)",
                        "Không tìm thấy thư mục hình ảnh img/! Hệ thống kích hoạt cơ chế bẫy lỗi và sử dụng logo vector dự phòng an toàn."
                    );
                    return;
                }

                string logoPath = System.IO.Path.Combine(imgDir, "logo.png");
                if (!System.IO.File.Exists(logoPath))
                {
                    TacticalMessageBox.ShowWarning(
                        this,
                        "Bẫy lỗi tài nguyên (img/)",
                        "Không tìm thấy file logo.png trong thư mục img/! Hệ thống kích hoạt cơ chế bẫy lỗi và chuyển sang giao diện dự phòng."
                    );
                    return;
                }

                if (picAboutLogo != null)
                {
                    using (var fs = new System.IO.FileStream(logoPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        picAboutLogo.Image = Image.FromStream(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                TacticalMessageBox.ShowWarning(
                    this,
                    "Ngoại lệ tài nguyên ảnh",
                    "Đã xảy ra sự cố khi nạp ảnh img: " + ex.Message
                );
            }
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
        private void btnClose_Click(object sender, EventArgs e) => this.Close();
        private void btnTabShips_Click(object sender, EventArgs e) => ChonMucHienThi(1);
        private void btnTabTactics_Click(object sender, EventArgs e) => ChonMucHienThi(2);
        private void btnTabTeam_Click(object sender, EventArgs e) => ChonMucHienThi(3);

        private void ChonMucHienThi(int index)
        {
            _currentTabIndex = index;

            CapNhatTrangThaiNut(btnTabRules, index == 0, UITheme.AccentNeon);
            CapNhatTrangThaiNut(btnTabShips, index == 1, UITheme.PrimaryCyan);
            CapNhatTrangThaiNut(btnTabTactics, index == 2, UITheme.WarningAmber);
            CapNhatTrangThaiNut(btnTabTeam, index == 3, Color.FromArgb(250, 204, 21));

            pnlRulesView.Visible = (index == 0);
            pnlShipsView.Visible = (index == 1);
            pnlTacticsView.Visible = (index == 2);
            pnlTeamView.Visible = (index == 3);

            switch (index)
            {
                case 0:
                    foreach (Control c in pnlRulesView.Controls) c.Invalidate();
                    break;
                case 1:
                    foreach (Control c in pnlShipsView.Controls) c.Invalidate();
                    break;
                case 2:
                    foreach (Control c in pnlTacticsView.Controls) c.Invalidate();
                    break;
                case 3:
                    foreach (Control c in pnlTeamView.Controls) c.Invalidate();
                    break;
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
        
        private void pnlShipsView_Paint(object sender, PaintEventArgs e)
        {
        }
        #endregion

        #region MỤC 1: LUẬT CHƠI (SỬ DỤNG CHILD PANELS CHỐNG GHOSTING KHI CUỘN)
        private void KhoiTaoGiaoDienRulesView()
        {
            pnlRulesView.Controls.Clear();
            pnlRulesView.AutoScroll = true;
            pnlRulesView.BackColor = UITheme.PanelBackground;

            int viewWidth = Math.Max(980, pnlRulesView.ClientSize.Width - 36);
            int cardW = (viewWidth - 32) / 3;
            int cardH = 250;
            int topY = 16;

            Panel pnlMiss = new Panel { Location = new Point(16, topY), Size = new Size(cardW, cardH), BackColor = Color.FromArgb(15, 23, 42) };
            EnableDoubleBuffering(pnlMiss);
            pnlMiss.Paint += (s, e) =>
            {
                UITheme.SetHighQuality(e.Graphics);
                VeCardMinhHoaPhatBan(
                    e.Graphics,
                    new Rectangle(0, 0, pnlMiss.Width - 1, pnlMiss.Height - 1),
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
            };
            pnlRulesView.Controls.Add(pnlMiss);

            Panel pnlHit = new Panel { Location = new Point(16 + cardW + 16, topY), Size = new Size(cardW, cardH), BackColor = Color.FromArgb(15, 23, 42) };
            EnableDoubleBuffering(pnlHit);
            pnlHit.Paint += (s, e) =>
            {
                UITheme.SetHighQuality(e.Graphics);
                VeCardMinhHoaPhatBan(
                    e.Graphics,
                    new Rectangle(0, 0, pnlHit.Width - 1, pnlHit.Height - 1),
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
            };
            pnlRulesView.Controls.Add(pnlHit);

            Panel pnlSunk = new Panel { Location = new Point(16 + 2 * (cardW + 16), topY), Size = new Size(cardW, cardH), BackColor = Color.FromArgb(15, 23, 42) };
            EnableDoubleBuffering(pnlSunk);
            pnlSunk.Paint += (s, e) =>
            {
                UITheme.SetHighQuality(e.Graphics);
                VeCardMinhHoaTauChim(
                    e.Graphics,
                    new Rectangle(0, 0, pnlSunk.Width - 1, pnlSunk.Height - 1),
                    "☠️ Bắn chìm tàu (Sunk)",
                    "🏆  Hạ gục tàu",
                    UITheme.DangerRed,
                    "• Bắn trúng ô cuối cùng của một chiến hạm.\n" +
                    "• Toàn bộ các ô của tàu đổi sang xám tro, viền đậm 'X'.\n" +
                    "• Còi hụ báo hiệu tàu chìm và được bắn tiếp lượt mới!"
                );
            };
            pnlRulesView.Controls.Add(pnlSunk);

            int ruleTop = topY + cardH + 16;
            Panel pnlRuleBox = new Panel { Location = new Point(16, ruleTop), Size = new Size(viewWidth, 285), BackColor = Color.FromArgb(15, 23, 42) };
            EnableDoubleBuffering(pnlRuleBox);
            pnlRuleBox.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                UITheme.SetHighQuality(g);
                Rectangle rectRuleBox = new Rectangle(0, 0, pnlRuleBox.Width - 1, pnlRuleBox.Height - 1);

                using (GraphicsPath path = UITheme.CreateRoundedPath(rectRuleBox, 8))
                {
                    using (SolidBrush bg = new SolidBrush(Color.FromArgb(15, 23, 42))) g.FillPath(bg, path);
                    using (Pen pen = new Pen(UITheme.PrimaryCyan, 1.2f)) g.DrawPath(pen, path);
                }

                using (Font titleFont = new Font("Segoe UI", 12f, FontStyle.Bold))
                {
                    Rectangle tRect = new Rectangle(16, 12, rectRuleBox.Width - 32, 26);
                    TextRenderer.DrawText(g, "🎯 Quy tắc tác chiến & Điều kiện toàn thắng", titleFont, tRect, UITheme.AccentNeon, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
                }

                var rules = new (string Icon, string Title, string Desc)[]
                {
                    ("🗺️", "Vùng biển quy chuẩn", "Mỗi bên sở hữu bàn cờ 10x10 = 100 ô tọa độ (Hàng A - J, Cột 1 - 10). Bố trí bí mật hạm đội, không chồng lấn và không vượt biên."),
                    ("⚓", "Hạm đội tham chiến", "Gồm 5 chiến hạm từ 2 đến 5 ô (Tàu sân bay: 5 ô, Thiết giáp hạm: 4 ô, Tàu ngầm: 3 ô, Tàu khu trục: 3 ô, Tàu tuần tra: 2 ô). Tổng cộng 17 ô máu."),
                    ("⏳", "Đồng hồ đếm ngược 30s", "Mỗi lượt khai hỏa có tối đa 30 giây để ngắm bắn. Nếu hết 30 giây mà không bắn, hệ thống sẽ tự động tính là mất lượt và chuyển quyền bắn."),
                    ("🏆", "Điều kiện chiến thắng", "Bên nào tiêu diệt toàn bộ 5 chiến hạm (bắn chìm đủ 17/17 ô tàu) của đối phương trước sẽ giành chiến thắng tuyệt đối chung cuộc!")
                };

                int startY = 46;
                for (int i = 0; i < rules.Length; i++)
                {
                    var (icon, rTitle, rDesc) = rules[i];
                    Rectangle lineRect = new Rectangle(16, startY + i * 56, rectRuleBox.Width - 32, 50);

                    using (Font iFont = new Font("Segoe UI", 14f))
                    using (Font hFont = new Font("Segoe UI", 10.5f, FontStyle.Bold))
                    using (Font dFont = new Font("Segoe UI", 9.5f, FontStyle.Regular))
                    {
                        TextRenderer.DrawText(g, icon, iFont, new Rectangle(lineRect.X, lineRect.Y, 32, 32), Color.White, TextFormatFlags.Left);
                        TextRenderer.DrawText(g, rTitle, hFont, new Rectangle(lineRect.X + 36, lineRect.Y, lineRect.Width - 36, 20), UITheme.PrimaryCyan, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
                        TextRenderer.DrawText(g, rDesc, dFont, new Rectangle(lineRect.X + 36, lineRect.Y + 22, lineRect.Width - 36, 28), UITheme.TextPrimary, TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
                    }
                }
            };
            pnlRulesView.Controls.Add(pnlRuleBox);

            pnlRulesView.Resize += (s, e) =>
            {
                int vw = Math.Max(980, pnlRulesView.ClientSize.Width - 36);
                int cw = (vw - 32) / 3;
                pnlMiss.Size = new Size(cw, cardH);
                pnlHit.Location = new Point(16 + cw + 16, topY);
                pnlHit.Size = new Size(cw, cardH);
                pnlSunk.Location = new Point(16 + 2 * (cw + 16), topY);
                pnlSunk.Size = new Size(cw, cardH);
                pnlRuleBox.Size = new Size(vw, 285);
                pnlMiss.Invalidate();
                pnlHit.Invalidate();
                pnlSunk.Invalidate();
                pnlRuleBox.Invalidate();
            };
        }

        private void pnlRulesView_Paint(object sender, PaintEventArgs e) { }

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

            using (Font titleFont = new Font("Segoe UI", 10.5f, FontStyle.Bold))
            {
                Rectangle tRect = new Rectangle(rect.X + 12, rect.Y + 10, rect.Width - 24, 24);
                TextRenderer.DrawText(g, title, titleFont, tRect, cellBorder, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
            }

            Rectangle sampleCell = new Rectangle(rect.X + 16, rect.Y + 40, 52, 52);
            using (SolidBrush brush = new SolidBrush(cellBg)) g.FillRectangle(brush, sampleCell);
            using (Pen pen = new Pen(cellBorder, 1.5f)) g.DrawRectangle(pen, sampleCell);
            using (Font symFont = new Font("Segoe UI", symbol == "•" ? 22f : 16f, FontStyle.Bold))
            {
                TextRenderer.DrawText(g, symbol, symFont, sampleCell, symbolColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

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

            int startX = rect.X + 16;
            int startY = rect.Y + 44;
            int cellSize = 28;
            for (int i = 0; i < 3; i++)
            {
                Rectangle cell = new Rectangle(startX + i * (cellSize + 4), startY, cellSize, cellSize);
                using (SolidBrush brush = new SolidBrush(UITheme.SunkAshBackground)) g.FillRectangle(brush, cell);
                using (Pen pen = new Pen(UITheme.SunkThickBorder, 2f)) g.DrawRectangle(pen, cell);
                using (Font symFont = new Font("Segoe UI", 11f, FontStyle.Bold))
                {
                    TextRenderer.DrawText(g, "X", symFont, cell, UITheme.SunkAshText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }

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

            Rectangle descRect = new Rectangle(rect.X + 16, startY + cellSize + 16, rect.Width - 32, 130);
            using (Font descFont = new Font("Segoe UI", 9.25f, FontStyle.Regular))
            {
                TextRenderer.DrawText(g, description, descFont, descRect, UITheme.TextPrimary, TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
            }
        }
        #endregion

        #region MỤC 2: THÔNG TIN HẠM ĐỘI (5 CHIẾN HẠM SỬ DỤNG CHILD PANELS CHỐNG GHOSTING)
        private void KhoiTaoGiaoDienShipsView()
        {
            pnlShipsView.Controls.Clear();
            pnlShipsView.AutoScroll = true;
            pnlShipsView.BackColor = UITheme.PanelBackground;

            var danhSachTau = new (string TenVN, string TenEN, int Size, string VaiTro, string Icon, Color AccentColor)[]
            {
                ("Tàu sân bay", "Aircraft Carrier", 5, "Soái hạm nổi đầu não của hạm đội, chở tiêm kích phòng không. Kích thước 5 ô lớn nhất, là mục tiêu săn lùng hàng đầu.", "✈️", Color.FromArgb(56, 189, 248)),
                ("Thiết giáp hạm", "Battleship", 4, "Tàu pháo bọc thép hạng nặng, sở hữu đại bác hạm đội công phá tầm xa cực kỳ uy lực.", "💣", Color.FromArgb(245, 158, 11)),
                ("Tàu ngầm", "Submarine", 3, "Sát thủ lặn sâu tàng hình dưới lòng đại dương, di chuyển lẩn khuất, khó phán đoán tọa độ.", "🌊", Color.FromArgb(16, 185, 129)),
                ("Tàu khu trục", "Destroyer", 3, "Tàu hộ tống cơ động cao, bệ phóng tên lửa hành trình, chuyên săn lùng tàu ngầm và bảo vệ hạm đội.", "🚀", Color.FromArgb(168, 85, 247)),
                ("Tàu tuần tra", "Patrol Boat", 2, "Trinh sát cao tốc nhỏ nhất (chỉ 2 ô). Cực kỳ khó phát hiện trên bàn cờ 100 ô, là chìa khóa quyết định thắng bại!", "⚡", Color.FromArgb(244, 63, 94))
            };

            int startY = 16;
            int cardH = 116;

            for (int i = 0; i < danhSachTau.Length; i++)
            {
                var item = danhSachTau[i];
                Panel pnlCard = new Panel
                {
                    Location = new Point(16, startY + i * (cardH + 14)),
                    Size = new Size(Math.Max(980, pnlShipsView.ClientSize.Width - 36), cardH),
                    BackColor = Color.FromArgb(15, 23, 42),
                    Tag = item
                };

                EnableDoubleBuffering(pnlCard);

                pnlCard.Paint += (s, e) =>
                {
                    Control ctl = (Control)s!;
                    Graphics g = e.Graphics;
                    UITheme.SetHighQuality(g);

                    var (tenVN, tenEN, size, vaiTro, icon, accentColor) = ((string, string, int, string, string, Color))ctl.Tag!;
                    Rectangle cardRect = new Rectangle(0, 0, ctl.Width - 1, ctl.Height - 1);

                    using (GraphicsPath path = UITheme.CreateRoundedPath(cardRect, 8))
                    {
                        using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(15, 23, 42))) g.FillPath(bgBrush, path);
                        using (Pen borderPen = new Pen(accentColor, 1.2f)) g.DrawPath(borderPen, path);
                    }

                    using (Font nameFont = new Font("Segoe UI", 12f, FontStyle.Bold))
                    using (Font enFont = new Font("Segoe UI", 9.5f, FontStyle.Regular))
                    {
                        Rectangle nameRect = new Rectangle(16, 12, 300, 26);
                        TextRenderer.DrawText(g, $"{icon}  {tenVN} ({size} ô)", nameFont, nameRect, accentColor, TextFormatFlags.Left | TextFormatFlags.NoPrefix);

                        Rectangle enRect = new Rectangle(18, 38, 300, 20);
                        TextRenderer.DrawText(g, $"Mã hiệu: {tenEN} • Độ bền: {size}/{size} HP", enFont, enRect, UITheme.TextSecondary, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
                    }

                    int cellStartX = 320;
                    int cellY = 14;
                    int cellSize = 32;

                    for (int seg = 0; seg < size; seg++)
                    {
                        Rectangle segRect = new Rectangle(cellStartX + seg * (cellSize + 4), cellY, cellSize, cellSize);
                        using (SolidBrush segBrush = new SolidBrush(UITheme.ShipCell)) g.FillRectangle(segBrush, segRect);
                        using (Pen segPen = new Pen(accentColor, 1.2f)) g.DrawRectangle(segPen, segRect);
                        using (Font sFont = new Font("Segoe UI", 9.5f, FontStyle.Bold))
                        {
                            TextRenderer.DrawText(g, "⚓", sFont, segRect, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                        }
                    }

                    Rectangle roleRect = new Rectangle(cellStartX, 54, ctl.Width - cellStartX - 20, 50);
                    using (Font roleFont = new Font("Segoe UI", 9.25f, FontStyle.Regular))
                    {
                        TextRenderer.DrawText(g, vaiTro, roleFont, roleRect, UITheme.TextPrimary, TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
                    }
                };

                pnlShipsView.Controls.Add(pnlCard);
            }

            pnlShipsView.Resize += (s, e) =>
            {
                int w = Math.Max(980, pnlShipsView.ClientSize.Width - 36);
                foreach (Control ctl in pnlShipsView.Controls)
                {
                    if (ctl.Width != w)
                    {
                        ctl.Width = w;
                        ctl.Invalidate();
                    }
                }
            };
        }
        #endregion

        #region MỤC 3: MẸO CHIẾN THUẬT (SỬ DỤNG CHILD PANELS CHỐNG GHOSTING KHI CUỘN)
        private void KhoiTaoGiaoDienTacticsView()
        {
            pnlTacticsView.Controls.Clear();
            pnlTacticsView.AutoScroll = true;
            pnlTacticsView.BackColor = UITheme.PanelBackground;

            int viewWidth = Math.Max(980, pnlTacticsView.ClientSize.Width - 36);
            int cardW = (viewWidth - 32) / 3;
            int cardH = 240;
            int topY = 16;

            Panel pnlParity = new Panel { Location = new Point(16, topY), Size = new Size(cardW, cardH), BackColor = Color.FromArgb(15, 23, 42) };
            EnableDoubleBuffering(pnlParity);
            pnlParity.Paint += (s, e) => {
                UITheme.SetHighQuality(e.Graphics);
                VeCardParity(e.Graphics, new Rectangle(0, 0, pnlParity.Width - 1, pnlParity.Height - 1));
            };
            pnlTacticsView.Controls.Add(pnlParity);

            Panel pnlTarget = new Panel { Location = new Point(16 + cardW + 16, topY), Size = new Size(cardW, cardH), BackColor = Color.FromArgb(15, 23, 42) };
            EnableDoubleBuffering(pnlTarget);
            pnlTarget.Paint += (s, e) => {
                UITheme.SetHighQuality(e.Graphics);
                VeCardTargetMode(e.Graphics, new Rectangle(0, 0, pnlTarget.Width - 1, pnlTarget.Height - 1));
            };
            pnlTacticsView.Controls.Add(pnlTarget);

            Panel pnlDefense = new Panel { Location = new Point(16 + 2 * (cardW + 16), topY), Size = new Size(cardW, cardH), BackColor = Color.FromArgb(15, 23, 42) };
            EnableDoubleBuffering(pnlDefense);
            pnlDefense.Paint += (s, e) => {
                UITheme.SetHighQuality(e.Graphics);
                VeCardDefenseStrategy(e.Graphics, new Rectangle(0, 0, pnlDefense.Width - 1, pnlDefense.Height - 1));
            };
            pnlTacticsView.Controls.Add(pnlDefense);

            int skillTop = topY + cardH + 16;
            Panel pnlSkillBox = new Panel { Location = new Point(16, skillTop), Size = new Size(viewWidth, 220), BackColor = Color.FromArgb(15, 23, 42) };
            EnableDoubleBuffering(pnlSkillBox);
            pnlSkillBox.Paint += (s, e) => {
                Graphics g = e.Graphics;
                UITheme.SetHighQuality(g);
                Rectangle rectSkillBox = new Rectangle(0, 0, pnlSkillBox.Width - 1, pnlSkillBox.Height - 1);

                using (GraphicsPath path = UITheme.CreateRoundedPath(rectSkillBox, 8))
                {
                    using (SolidBrush bg = new SolidBrush(Color.FromArgb(15, 23, 42))) g.FillPath(bg, path);
                    using (Pen pen = new Pen(UITheme.WarningAmber, 1.2f)) g.DrawPath(pen, path);
                }

                using (Font titleFont = new Font("Segoe UI", 12f, FontStyle.Bold))
                {
                    Rectangle tRect = new Rectangle(16, 12, rectSkillBox.Width - 32, 26);
                    TextRenderer.DrawText(g, "📡 Mẹo sử dụng kỹ năng tác chiến", titleFont, tRect, UITheme.WarningAmber, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
                }

                var skills = new (string Icon, string Name, string Desc)[]
                {
                    ("📡", "Radar trinh sát (3x3)", "Quét một vùng biển 3x3 ô nghi vấn. Hệ thống thông báo 'Phát hiện có dấu vết tàu địch' hoặc 'Vùng biển an toàn' mà không tiết lộ ô chính xác. Rất hữu ích khi cần xác định tàu tuần tra 2 ô đang trốn ở góc nào!"),
                    ("💥", "Pháo kích diện rộng (+)", "Khai hỏa đồng loạt 5 quả đại bác theo hình dấu cộng (+) cùng một lúc. Giúp càn quét cực nhanh các vùng biển trống và gia tăng xác suất bắn trúng các tàu cỡ lớn (4-5 ô)!")
                };

                for (int i = 0; i < skills.Length; i++)
                {
                    var (icon, sName, sDesc) = skills[i];
                    Rectangle sRect = new Rectangle(16, 46 + i * 82, rectSkillBox.Width - 32, 74);

                    using (Font iFont = new Font("Segoe UI", 16f))
                    using (Font hFont = new Font("Segoe UI", 10.5f, FontStyle.Bold))
                    using (Font dFont = new Font("Segoe UI", 9f, FontStyle.Regular))
                    {
                        TextRenderer.DrawText(g, icon, iFont, new Rectangle(sRect.X, sRect.Y, 36, 36), Color.White, TextFormatFlags.Left);
                        TextRenderer.DrawText(g, sName, hFont, new Rectangle(sRect.X + 44, sRect.Y, sRect.Width - 44, 22), UITheme.AccentNeon, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
                        TextRenderer.DrawText(g, sDesc, dFont, new Rectangle(sRect.X + 44, sRect.Y + 24, sRect.Width - 44, 46), UITheme.TextPrimary, TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
                    }
                }
            };
            pnlTacticsView.Controls.Add(pnlSkillBox);

            pnlTacticsView.Resize += (s, e) => {
                int vw = Math.Max(980, pnlTacticsView.ClientSize.Width - 36);
                int cw = (vw - 32) / 3;
                pnlParity.Size = new Size(cw, cardH);
                pnlTarget.Location = new Point(16 + cw + 16, topY);
                pnlTarget.Size = new Size(cw, cardH);
                pnlDefense.Location = new Point(16 + 2 * (cw + 16), topY);
                pnlDefense.Size = new Size(cw, cardH);
                pnlSkillBox.Size = new Size(vw, 220);
                pnlParity.Invalidate();
                pnlTarget.Invalidate();
                pnlDefense.Invalidate();
                pnlSkillBox.Invalidate();
            };
        }

        private void pnlTacticsView_Paint(object sender, PaintEventArgs e) { }

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

        #region MỤC 4: THÀNH VIÊN NHÓM (SỬ DỤNG CHILD PANELS CHỐNG GHOSTING KHI CUỘN)
        private void KhoiTaoGiaoDienTeamView()
        {
            pnlTeamView.Controls.Clear();
            pnlTeamView.AutoScroll = true;
            pnlTeamView.BackColor = UITheme.PanelBackground;

            var danhSachThanhVien = new (string VaiTro, string HoTen, string MSSV, string NhiemVuChinh, Color AccentColor, string Icon)[]
            {
                (
                    "Trưởng nhóm / Backend & Game Logic",
                    "Huỳnh Minh Trí",
                    "51.01.104.105",
                    "• Thiết kế kiến trúc mô hình dữ liệu (Models) và bàn cờ 10x10.\n\n• Lập trình thuật toán Bot bắn tự động FSM (Hunt & Target Mode).\n\n• Xử lý kiểm tra va chạm xếp tàu và điều phối luồng ván đấu.",
                    UITheme.AccentNeon,
                    "👑"
                ),
                (
                    "Frontend & UI/UX Lead",
                    "Trần Trọng Hưng",
                    "51.01.104.132",
                    "• Thiết kế toàn bộ giao diện WinForms phong cách hải quân.\n\n• Lập trình đồ họa bàn cờ, hiệu ứng đặt tàu và phát bắn.\n\n• Xây dựng màn hình Hướng dẫn và tối ưu trải nghiệm người dùng.",
                    UITheme.PrimaryCyan,
                    "🎨"
                ),
                (
                    "System I/O, Audio & QA/Tester",
                    "Trần Vũ Hòa Phát",
                    "51.01.104.072",
                    "• Tích hợp hệ thống âm thanh (nhạc nền và hiệu ứng chiến đấu).\n\n• Đọc/ghi dữ liệu bảng xếp hạng thanhtichs.txt qua System.IO.\n\n• Xây dựng cơ chế bẫy lỗi try...catch và thực hiện video demo.",
                    UITheme.WarningAmber,
                    "🔊"
                )
            };

            int startY = 16;
            int cardH = 176;

            for (int i = 0; i < danhSachThanhVien.Length; i++)
            {
                var tv = danhSachThanhVien[i];
                Panel pnlCard = new Panel
                {
                    Location = new Point(16, startY + i * (cardH + 16)),
                    Size = new Size(Math.Max(980, pnlTeamView.ClientSize.Width - 36), cardH),
                    BackColor = Color.FromArgb(15, 23, 42),
                    Tag = tv
                };
                EnableDoubleBuffering(pnlCard);

                pnlCard.Paint += (s, e) =>
                {
                    Control ctl = (Control)s!;
                    Graphics g = e.Graphics;
                    UITheme.SetHighQuality(g);

                    var (vaiTro, hoTen, mssv, nhiemVu, accentColor, icon) = ((string, string, string, string, Color, string))ctl.Tag!;
                    Rectangle cardRect = new Rectangle(0, 0, ctl.Width - 1, ctl.Height - 1);

                    using (GraphicsPath path = UITheme.CreateRoundedPath(cardRect, 8))
                    {
                        using (SolidBrush bg = new SolidBrush(Color.FromArgb(15, 23, 42))) g.FillPath(bg, path);
                        using (Pen pen = new Pen(accentColor, 1.4f)) g.DrawPath(pen, path);
                    }

                    int taskStartX = 420;
                    using (Pen divPen = new Pen(Color.FromArgb(40, 255, 255, 255), 1f))
                    {
                        g.DrawLine(divPen, taskStartX - 24, 18, taskStartX - 24, ctl.Height - 18);
                    }

                    using (Font roleFont = new Font("Segoe UI", 10.5f, FontStyle.Bold))
                    using (Font nameFont = new Font("Segoe UI", 15.5f, FontStyle.Bold))
                    using (Font mssvFont = new Font("Consolas", 12.5f, FontStyle.Bold))
                    using (Font classFont = new Font("Segoe UI", 10.5f, FontStyle.Regular))
                    {
                        Rectangle roleRect = new Rectangle(20, 16, 370, 26);
                        TextRenderer.DrawText(g, $"{icon}  {vaiTro}", roleFont, roleRect, accentColor, TextFormatFlags.Left | TextFormatFlags.NoPrefix);

                        Rectangle nameRect = new Rectangle(20, 48, 370, 34);
                        TextRenderer.DrawText(g, hoTen, nameFont, nameRect, Color.White, TextFormatFlags.Left | TextFormatFlags.NoPrefix);

                        Rectangle mssvRect = new Rectangle(20, 88, 370, 28);
                        TextRenderer.DrawText(g, $"MSSV: {mssv}", mssvFont, mssvRect, UITheme.GoldAccent, TextFormatFlags.Left | TextFormatFlags.NoPrefix);

                        Rectangle classRect = new Rectangle(20, 122, 370, 26);
                        TextRenderer.DrawText(g, "Lớp: 2611COMP101901  •  Nhóm 10: WINDOWN", classFont, classRect, UITheme.PrimaryCyan, TextFormatFlags.Left | TextFormatFlags.NoPrefix);
                    }

                    using (Font headerFont = new Font("Segoe UI", 11f, FontStyle.Bold))
                    using (Font taskFont = new Font("Segoe UI", 10.25f, FontStyle.Regular))
                    {
                        Rectangle headerRect = new Rectangle(taskStartX, 16, ctl.Width - taskStartX - 20, 26);
                        TextRenderer.DrawText(g, "📋  Phân công nhiệm vụ chính (WBS):", headerFont, headerRect, Color.FromArgb(203, 213, 225), TextFormatFlags.Left | TextFormatFlags.NoPrefix);

                        Rectangle taskRect = new Rectangle(taskStartX, 48, ctl.Width - taskStartX - 20, ctl.Height - 56);
                        TextRenderer.DrawText(g, nhiemVu, taskFont, taskRect, UITheme.TextPrimary, TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
                    }
                };

                pnlTeamView.Controls.Add(pnlCard);
            }

            pnlTeamView.Resize += (s, e) =>
            {
                int w = Math.Max(980, pnlTeamView.ClientSize.Width - 36);
                foreach (Control ctl in pnlTeamView.Controls)
                {
                    if (ctl.Width != w)
                    {
                        ctl.Width = w;
                        ctl.Invalidate();
                    }
                }
            };
        }

        private void pnlTeamView_Paint(object sender, PaintEventArgs e) { }
        #endregion
    }
}
