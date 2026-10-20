using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
    /// <summary>
    /// Component Bàn cờ Hải quân chuẩn kích thước 10x10 do Thành viên 2 (Frontend) thiết kế.
    /// Hỗ trợ vẽ tọa độ A-J, 1-10, hiệu ứng hover, preview ô cờ và xử lý sự kiện tương tác giao diện.
    /// </summary>
    public class TacticalBoardControl : Control
    {
        public bool LaBanCoRadar { get; set; } = false;
        public string TieuDeBanCo { get; set; } = "BÀN CỜ";

        // Mảng trạng thái giao diện nội bộ (0: Nước, 1: Tàu mẫu, 2: Trúng, 3: Trượt, 4: Chìm)
        private readonly int[,] _displayMatrix = new int[10, 10];

        public event Action<int, int> KhiClickO;
        public event Action<int, int> KhiHoverO;

        private Point _hoverO = new Point(-1, -1);
        private const int LeTren = 34;
        private const int LeTrai = 34;

        public TacticalBoardControl()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true
            );
            BackColor = UITheme.HeaderBackground;
            Size = new Size(400, 430);
            Cursor = Cursors.Cross;

            // Khởi tạo một số ô mẫu tĩnh để phác thảo giao diện trực quan cho Ngày 1
            KhoiTaoHienThiMau();
        }

        private void KhoiTaoHienThiMau()
        {
            if (!LaBanCoRadar)
            {
                // Bàn cờ hạm đội: đặt sẵn vài vị trí tàu mẫu để minh họa giao diện tĩnh Ngày 1
                // Tàu sân bay (5 ô hàng C)
                for (int c = 1; c <= 5; c++) _displayMatrix[2, c] = 1;
                // Thiết giáp hạm (4 ô cột H)
                for (int r = 4; r <= 7; r++) _displayMatrix[r, 7] = 1;
                // Tàu ngầm (3 ô hàng I)
                for (int c = 2; c <= 4; c++) _displayMatrix[8, c] = 1;
            }
            else
            {
                // Bàn cờ radar: minh họa phát bắn trúng và trượt mẫu
                _displayMatrix[1, 2] = 3; // Trượt (Sóng nước)
                _displayMatrix[3, 5] = 2; // Trúng (Lửa đỏ)
                _displayMatrix[3, 6] = 2; // Trúng
                _displayMatrix[7, 8] = 3; // Trượt
            }
        }

        public void DatGiaTriO(int x, int y, int trangThai)
        {
            if (x >= 0 && x < 10 && y >= 0 && y < 10)
            {
                _displayMatrix[y, x] = trangThai;
                Invalidate();
            }
        }

        public void XoaBanCo()
        {
            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 10; c++)
                {
                    _displayMatrix[r, c] = 0;
                }
            }
            Invalidate();
        }

        private float CellSize => Math.Min((Width - LeTrai - 6) / 10f, (Height - LeTren - 6) / 10f);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            float cs = CellSize;
            int col = (int)((e.X - LeTrai) / cs);
            int row = (int)((e.Y - LeTren) / cs);

            if (col >= 0 && col < 10 && row >= 0 && row < 10)
            {
                if (_hoverO.X != col || _hoverO.Y != row)
                {
                    _hoverO = new Point(col, row);
                    KhiHoverO?.Invoke(col, row);
                    Invalidate();
                }
            }
            else
            {
                if (_hoverO.X != -1)
                {
                    _hoverO = new Point(-1, -1);
                    Invalidate();
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hoverO = new Point(-1, -1);
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left && _hoverO.X >= 0 && _hoverO.Y >= 0)
            {
                KhiClickO?.Invoke(_hoverO.X, _hoverO.Y);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            UITheme.SetHighQuality(g);

            float cs = CellSize;
            RectangleF boardRect = new RectangleF(LeTrai, LeTren, cs * 10, cs * 10);

            // 1. Khung nền bàn cờ
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(18, 28, 45)))
            {
                g.FillRectangle(bgBrush, boardRect);
            }

            using (Pen borderPen = new Pen(LaBanCoRadar ? UITheme.AccentNeon : UITheme.PrimaryCyan, 1.5f))
            {
                g.DrawRectangle(borderPen, boardRect.X, boardRect.Y, boardRect.Width, boardRect.Height);
            }

            // 2. Nhãn hàng A-J và cột 1-10
            string[] hangNhan = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            using (Font coordFont = new Font("Segoe UI", 9f, FontStyle.Bold))
            using (SolidBrush coordBrush = new SolidBrush(UITheme.TextSecondary))
            {
                for (int i = 0; i < 10; i++)
                {
                    // Cột 1-10
                    string colText = (i + 1).ToString();
                    RectangleF colRect = new RectangleF(LeTrai + i * cs, 8, cs, LeTren - 8);
                    g.DrawString(colText, coordFont, coordBrush, colRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    // Hàng A-J
                    RectangleF rowRect = new RectangleF(4, LeTren + i * cs, LeTrai - 4, cs);
                    g.DrawString(hangNhan[i], coordFont, coordBrush, rowRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
            }

            // 3. Vẽ từng ô cờ 10x10
            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 10; c++)
                {
                    RectangleF cellRect = new RectangleF(LeTrai + c * cs, LeTren + r * cs, cs, cs);
                    Color cellColor = UITheme.WaterCell;

                    int status = _displayMatrix[r, c];
                    if (status == 1 && !LaBanCoRadar)
                    {
                        cellColor = UITheme.ShipCell; // Ô có tàu
                    }
                    else if (status == 2)
                    {
                        cellColor = UITheme.DangerRed; // Ô trúng
                    }
                    else if (status == 3)
                    {
                        cellColor = Color.FromArgb(45, 58, 76); // Ô trượt
                    }
                    else if (status == 4)
                    {
                        cellColor = UITheme.SunkDarkRed; // Ô chìm
                    }

                    using (SolidBrush cellBrush = new SolidBrush(cellColor))
                    {
                        g.FillRectangle(cellBrush, cellRect.X + 1, cellRect.Y + 1, cellRect.Width - 2, cellRect.Height - 2);
                    }

                    using (Pen cellPen = new Pen(UITheme.BorderDefault, 1f))
                    {
                        g.DrawRectangle(cellPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    }

                    // Biểu tượng trúng / trượt
                    if (status == 2)
                    {
                        VeBieuTuongTrung(g, cellRect);
                    }
                    else if (status == 3)
                    {
                        VeBieuTuongTruot(g, cellRect);
                    }
                }
            }

            // 4. Hiệu ứng Hover viền phát sáng
            if (_hoverO.X >= 0 && _hoverO.Y >= 0)
            {
                RectangleF hoverRect = new RectangleF(LeTrai + _hoverO.X * cs + 1, LeTren + _hoverO.Y * cs + 1, cs - 2, cs - 2);
                using (Pen hoverPen = new Pen(LaBanCoRadar ? UITheme.AccentNeon : UITheme.PrimaryCyan, 2f))
                {
                    g.DrawRectangle(hoverPen, hoverRect.X, hoverRect.Y, hoverRect.Width, hoverRect.Height);
                }
            }
        }

        private void VeBieuTuongTrung(Graphics g, RectangleF rect)
        {
            float pad = rect.Width * 0.22f;
            using (Pen firePen = new Pen(Color.FromArgb(255, 230, 0), 2.8f))
            {
                g.DrawLine(firePen, rect.Left + pad, rect.Top + pad, rect.Right - pad, rect.Bottom - pad);
                g.DrawLine(firePen, rect.Right - pad, rect.Top + pad, rect.Left + pad, rect.Bottom - pad);
            }
        }

        private void VeBieuTuongTruot(Graphics g, RectangleF rect)
        {
            float r = rect.Width * 0.16f;
            float cx = rect.Left + rect.Width / 2f;
            float cy = rect.Top + rect.Height / 2f;

            using (SolidBrush rippleBrush = new SolidBrush(UITheme.MissGray))
            {
                g.FillEllipse(rippleBrush, cx - r, cy - r, r * 2, r * 2);
            }
        }
    }
}
