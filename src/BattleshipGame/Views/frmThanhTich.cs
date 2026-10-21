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
        private List<ThanhTichItem> _listGoc = new List<ThanhTichItem>();
        private TieuChiSapXep _sortType = TieuChiSapXep.DiemCaoNhat;
        private bool _daLoad = false;
        private DateTime _lanTaiCuoi = DateTime.MinValue;
        private static readonly Font _fontDiem = new Font("Segoe UI", 10F, FontStyle.Bold);

        public frmThanhTich()
        {
            InitializeComponent();
            BatDoubleBuffer(dgvThanhTich);
            this.KeyPreview = true;
            this.KeyDown += FrmThanhTich_KeyDown;
        }

        // bat double buffer cho muot
        private static void BatDoubleBuffer(DataGridView dgv)
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
                this.Close();
            else if (e.KeyCode == Keys.F5)
            {
                if ((DateTime.UtcNow - _lanTaiCuoi).TotalMilliseconds >= 300)
                {
                    _lanTaiCuoi = DateTime.UtcNow;
                    TaiDuLieuTuFile();
                }
            }
        }

        private void frmThanhTich_Load(object sender, EventArgs e)
        {
            if (cboCheDo.Items.Count > 0)
                cboCheDo.SelectedIndex = 0; 

            _daLoad = true;
            TaiDuLieuTuFile();
        }

        public void TaiDuLieuTuFile()
        {
            _listGoc = cThanhTichDAO.DocDanhSach();

            if (!string.IsNullOrEmpty(cThanhTichDAO.LoiGanNhat))
            {
                lblTongSo.Text = "⚠️ " + cThanhTichDAO.LoiGanNhat;
                lblTongSo.ForeColor = Color.FromArgb(248, 113, 113);
            }

            CapNhatBang();
        }

        // Do du lieu len DataGridView
        private void CapNhatBang()
        {
            if (!_daLoad || _listGoc == null) return;

            string cheDo = cboCheDo.SelectedItem != null ? cboCheDo.SelectedItem.ToString() : "Tất cả";
            List<ThanhTichItem> listLoc = cThanhTichDAO.LocTheoCheDo(_listGoc, cheDo);
            List<ThanhTichItem> listSort = cThanhTichDAO.SapXep(listLoc, _sortType);

            const int maxRows = 100;
            List<ThanhTichItem> listShow = listSort.Count > maxRows
                ? listSort.GetRange(0, maxRows)
                : listSort;

            dgvThanhTich.SuspendLayout();
            try
            {
                dgvThanhTich.Rows.Clear();

                for (int i = 0; i < listShow.Count; i++)
                {
                    ThanhTichItem item = listShow[i];
                    if (item == null) continue;

                    string rank = (i + 1).ToString();
                    if (i == 0) rank = "🥇 " + rank;
                    else if (i == 1) rank = "🥈 " + rank;
                    else if (i == 2) rank = "🥉 " + rank;

                    int idx = dgvThanhTich.Rows.Add(
                        rank,
                        item.NgayChoi.ToString("yyyy-MM-dd HH:mm"),
                        item.TenNguoiChoi ?? "VoDanh",
                        item.DiemSo.ToString("N0"),
                        item.SoLuotBan,
                        item.ThoiGianHienThi,
                        item.CheDo ?? "PvE",
                        item.KetQua ?? "Chưa rõ"
                    );

                    DataGridViewRow row = dgvThanhTich.Rows[idx];

                    row.Cells[3].Style.ForeColor = Color.FromArgb(100, 255, 218);
                    row.Cells[3].Style.Font = _fontDiem;

                    string kq = item.KetQua ?? "";
                    if (kq.IndexOf("Thắng", StringComparison.OrdinalIgnoreCase) >= 0)
                        row.Cells[7].Style.ForeColor = Color.FromArgb(52, 211, 153);
                    else
                        row.Cells[7].Style.ForeColor = Color.FromArgb(248, 113, 113);
                }
            }
            finally
            {
                dgvThanhTich.ResumeLayout();
            }

            if (string.IsNullOrEmpty(cThanhTichDAO.LoiGanNhat))
            {
                if (listLoc.Count > maxRows)
                    lblTongSo.Text = string.Format("Hiển thị Top {0}/{1} bản ghi ({2})", maxRows, listLoc.Count, cheDo);
                else
                    lblTongSo.Text = string.Format("Tổng số bản ghi: {0} ({1})", listShow.Count, cheDo);

                lblTongSo.ForeColor = Color.FromArgb(148, 163, 184);
            }
        }

        private void cboCheDo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_daLoad) CapNhatBang();
        }

        private void btnSortDiem_Click(object sender, EventArgs e)
        {
            _sortType = TieuChiSapXep.DiemCaoNhat;
            DoiMauNut(btnSortDiem);
            CapNhatBang();
        }

        private void btnSortThoiGian_Click(object sender, EventArgs e)
        {
            _sortType = TieuChiSapXep.ThoiGianNganNhat;
            DoiMauNut(btnSortThoiGian);
            CapNhatBang();
        }

        private void btnSortLuotBan_Click(object sender, EventArgs e)
        {
            _sortType = TieuChiSapXep.SoLuotItNhat;
            DoiMauNut(btnSortLuotBan);
            CapNhatBang();
        }

        private void DoiMauNut(Button active)
        {
            Color normal = Color.FromArgb(51, 65, 85);
            Color highlight = Color.FromArgb(14, 165, 233);

            btnSortDiem.BackColor = (btnSortDiem == active) ? highlight : normal;
            btnSortThoiGian.BackColor = (btnSortThoiGian == active) ? highlight : normal;
            btnSortLuotBan.BackColor = (btnSortLuotBan == active) ? highlight : normal;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            if ((DateTime.UtcNow - _lanTaiCuoi).TotalMilliseconds >= 300)
            {
                _lanTaiCuoi = DateTime.UtcNow;
                TaiDuLieuTuFile();
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
