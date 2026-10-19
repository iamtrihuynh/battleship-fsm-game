using System;
using System.Collections.Generic;
using System.Drawing;

namespace BattleshipGame.Models
{
    /// <summary>
    /// Dai dien cho mot con tau chien trong ham doi
    /// </summary>
    public class cTau
    {
        public string TenTau { get; set; }
        public int KichThuoc { get; set; }
        public HuongTau Huong { get; set; }
        public LoaiTau Loai { get; set; }
        public List<Point> DanhSachToaDo { get; private set; } = new List<Point>();

        private readonly HashSet<Point> _toaDoDaBiBan = new HashSet<Point>();

        /// <summary>
        /// So o cua con tau nay da bi ban trung
        /// </summary>
        public int SoOBiBan => _toaDoDaBiBan.Count;

        /// <summary>
        /// So o con lai chua bi ban
        /// </summary>
        public int SoOConLai => Math.Max(0, KichThuoc - SoOBiBan);

        /// <summary>
        /// Kiem tra con tau da bi ban chim hoan toan chua
        /// </summary>
        public bool DaChim => SoOBiBan >= KichThuoc && KichThuoc > 0;

        /// <summary>
        /// Kiem tra tau da duoc xep len ban co chua
        /// </summary>
        public bool DaDatXong => DanhSachToaDo.Count == KichThuoc;

        public cTau(string ten, int kichThuoc, LoaiTau loai = LoaiTau.TauTuanTra)
        {
            TenTau = ten;
            KichThuoc = kichThuoc;
            Loai = loai;
            Huong = HuongTau.Ngang;
        }

        public cTau(LoaiTau loai)
        {
            Loai = loai;
            KichThuoc = loai.LayKichThuocChuan();
            TenTau = loai.LayTenMacDinh();
            Huong = HuongTau.Ngang;
        }

        /// <summary>
        /// Kiem tra toa do p co nam tren than con tau hay khong
        /// </summary>
        public bool ChuaToaDo(Point p)
        {
            return DanhSachToaDo.Contains(p);
        }

        /// <summary>
        /// Tiep nhan mot phat ban vao toa do p. Tra ve true neu trung o moi cua tau nay.
        /// </summary>
        public bool KiemTraBiBan(Point p)
        {
            if (ChuaToaDo(p))
            {
                if (!_toaDoDaBiBan.Contains(p))
                {
                    _toaDoDaBiBan.Add(p);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gan danh sach toa do chiem giu khi dat tau len ban co
        /// </summary>
        public void GanToaDo(IEnumerable<Point> toaDo, HuongTau huong)
        {
            DanhSachToaDo.Clear();
            _toaDoDaBiBan.Clear();
            DanhSachToaDo.AddRange(toaDo);
            Huong = huong;
        }

        /// <summary>
        /// Thu hoi tau ve kho, xoa toa do va trang thai trung dan
        /// </summary>
        public void Reset()
        {
            DanhSachToaDo.Clear();
            _toaDoDaBiBan.Clear();
        }

        public override string ToString()
        {
            return $"{TenTau} ({KichThuoc} ô) - Đã trúng: {SoOBiBan}/{KichThuoc}" + (DaChim ? " [ĐÃ CHÌM]" : "");
        }
    }
}
