using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using BattleshipGame.DataAccess;
using BattleshipGame.Models;

namespace BattleshipGame.Views
{
    public partial class frmThanhTich : Form
    {
        private List<ThanhTichItem> _danhSachGoc = new List<ThanhTichItem>();
        private TieuChiSapXep _currentSort = TieuChiSapXep.DiemCaoNhat;
        private bool _isFormLoaded = false;
        private static readonly Font FontDiemSo = new Font("Segoe UI", 10F, FontStyle.Bold);

        public frmThanhTich()
        {
            InitializeComponent();
            BatDoubleBuffering(dgvThanhTich);
            this.KeyPreview = true;
            this.KeyDown += FrmThanhTich_KeyDown;
        }

        /// <summary>
        /// Kích hoạt Double Buffering cho DataGridView để cuộn và vẽ bảng mượt mà, chống giật hình
        /// </summary>
        private static void BatDoubleBuffering(DataGridView dgv)
        {
            try
            {
                typeof(DataGridView).InvokeMember("DoubleBuffered",
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                    null, dgv, new object[] { true });
            }
            catch { }
        }

        private void FrmThanhTich_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.F5)
            {
                TaiDuLieuTuFile();
            }
        }

        private void frmThanhTich_Load(object sender, EventArgs e)
        {
            if (cboCheDo.Items.Count > 0)
            {
                cboCheDo.SelectedIndex = 0; // "Tất cả"
            }
            _isFormLoaded = true;
            TaiDuLieuTuFile();
        }

        /// <summary>
        /// Tải toàn bộ dữ liệu mới nhất từ file thanhtichs.txt
        /// </summary>
        public void TaiDuLieuTuFile()
        {
            _danhSachGoc = cThanhTichDAO.DocDanhSach();

            if (!string.IsNullOrEmpty(cThanhTichDAO.LoiGanNhat))
            {
                lblTongSo.Text = "⚠️ " + cThanhTichDAO.LoiGanNhat;
                lblTongSo.ForeColor = Color.FromArgb(248, 113, 113);
            }

            CapNhatGiaoDienBang();
        }

        /// <summary>
        /// Lọc, sắp xếp và đổ dữ liệu vào DataGridView an toàn và tối ưu hiệu năng
        /// </summary>
        private void CapNhatGiaoDienBang()
        {
            if (!_isFormLoaded || _danhSachGoc == null) return;

            string selectedCheDo = cboCheDo.SelectedItem != null ? cboCheDo.SelectedItem.ToString() : "Tất cả";
            List<ThanhTichItem> danhSachLoc = cThanhTichDAO.LocTheoCheDo(_danhSachGoc, selectedCheDo);
            List<ThanhTichItem> danhSachSapXep = cThanhTichDAO.SapXep(danhSachLoc, _currentSort);
            
            // Giới hạn hiển thị Top 100 để đảm bảo hiệu năng mượt mà nhất
            const int maxHienThi = 100;
            List<ThanhTichItem> danhSachHienThi = danhSachSapXep.Count > maxHienThi 
                ? danhSachSapXep.GetRange(0, maxHienThi) 
                : danhSachSapXep;

            dgvThanhTich.SuspendLayout();
            try
            {
                dgvThanhTich.Rows.Clear();

                for (int i = 0; i < danhSachHienThi.Count; i++)
                {
                    ThanhTichItem item = danhSachHienThi[i];
                    if (item == null) continue;

                    // Gán icon huy chương cho Top 3
                    string hangText = (i + 1).ToString();
                    if (i == 0) hangText = "🥇 " + hangText;
                    else if (i == 1) hangText = "🥈 " + hangText;
                    else if (i == 2) hangText = "🥉 " + hangText;

                    int rowIndex = dgvThanhTich.Rows.Add(
                        hangText,
                        item.NgayChoi.ToString("yyyy-MM-dd HH:mm"),
                        item.TenNguoiChoi ?? "VoDanh",
                        item.DiemSo.ToString("N0"),
                        item.SoLuotBan,
                        item.ThoiGianHienThi,
                        item.CheDo ?? "PvE",
                        item.KetQua ?? "Chưa rõ"
                    );

                    DataGridViewRow row = dgvThanhTich.Rows[rowIndex];

                    // Đổi màu cột Điểm số sang màu Neon Cyan sáng (Dùng chung FontDiemSo tránh leak GDI)
                    row.Cells[3].Style.ForeColor = Color.FromArgb(100, 255, 218);
                    row.Cells[3].Style.Font = FontDiemSo;

                    // Đổi màu cột Kết quả (Null-safety): Xanh lá nếu Thắng, Đỏ nếu Thua
                    string ketQua = item.KetQua ?? "";
                    if (ketQua.IndexOf("Thắng", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        row.Cells[7].Style.ForeColor = Color.FromArgb(52, 211, 153); // Emerald Green
                    }
                    else
                    {
                        row.Cells[7].Style.ForeColor = Color.FromArgb(248, 113, 113); // Coral Red
                    }
                }
            }
            finally
            {
                dgvThanhTich.ResumeLayout();
            }

            if (string.IsNullOrEmpty(cThanhTichDAO.LoiGanNhat))
            {
                if (danhSachLoc.Count > maxHienThi)
                {
                    lblTongSo.Text = string.Format("Hiển thị Top {0}/{1} bản ghi ({2})", maxHienThi, danhSachLoc.Count, selectedCheDo);
                }
                else
                {
                    lblTongSo.Text = string.Format("Tổng số bản ghi: {0} ({1})", danhSachHienThi.Count, selectedCheDo);
                }
                lblTongSo.ForeColor = Color.FromArgb(148, 163, 184);
            }
        }

        private void cboCheDo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isFormLoaded)
            {
                CapNhatGiaoDienBang();
            }
        }

        private void btnSortDiem_Click(object sender, EventArgs e)
        {
            _currentSort = TieuChiSapXep.DiemCaoNhat;
            HighlightButton(btnSortDiem);
            CapNhatGiaoDienBang();
        }

        private void btnSortThoiGian_Click(object sender, EventArgs e)
        {
            _currentSort = TieuChiSapXep.ThoiGianNganNhat;
            HighlightButton(btnSortThoiGian);
            CapNhatGiaoDienBang();
        }

        private void btnSortLuotBan_Click(object sender, EventArgs e)
        {
            _currentSort = TieuChiSapXep.SoLuotItNhat;
            HighlightButton(btnSortLuotBan);
            CapNhatGiaoDienBang();
        }

        private void HighlightButton(Button activeBtn)
        {
            Color normalBg = Color.FromArgb(51, 65, 85);
            Color activeBg = Color.FromArgb(14, 165, 233);

            btnSortDiem.BackColor = (btnSortDiem == activeBtn) ? activeBg : normalBg;
            btnSortThoiGian.BackColor = (btnSortThoiGian == activeBtn) ? activeBg : normalBg;
            btnSortLuotBan.BackColor = (btnSortLuotBan == activeBtn) ? activeBg : normalBg;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            TaiDuLieuTuFile();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
