using System;
using System.Collections.Generic;
using System.Drawing;

namespace BattleshipGame.Models
{
    public class cTau
    {
        public string TenTau { get; set; }
        public int KichThuoc { get; set; }
        public HuongTau Huong { get; set; }
        public LoaiTau Loai { get; set; }
        public List<Point> DanhSachToaDo { get; private set; } = new List<Point>();

        private readonly HashSet<Point> _toaDoDaBiBan = new HashSet<Point>();

        public int SoOBiBan => _toaDoDaBiBan.Count;

        public int SoOConLai => Math.Max(0, KichThuoc - SoOBiBan);

        public bool DaChim => SoOBiBan >= KichThuoc && KichThuoc > 0;

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

        public bool ChuaToaDo(Point p)
        {
            return DanhSachToaDo.Contains(p);
        }

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

        public void GanToaDo(IEnumerable<Point> toaDo, HuongTau huong)
        {
            DanhSachToaDo.Clear();
            _toaDoDaBiBan.Clear();
            DanhSachToaDo.AddRange(toaDo);
            Huong = huong;
        }

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
