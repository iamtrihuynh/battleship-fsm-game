using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BattleshipGame.Models;

namespace BattleshipGame.Logic
{
    public enum DoKhoAI
    {
        De,
        Kho
    }

    public enum TrangThaiAI
    {
        Hunt,
        Target
    }

    public class cMayBanAI
    {
        private readonly int _kichThuoc;
        private readonly Random _rand = new Random();
        private readonly List<Point> _danhSachChuaBan = new List<Point>();
        private readonly Queue<Point> _hangDoiTarget = new Queue<Point>();
        private readonly List<Point> _cacOTrungHienTai = new List<Point>();

        public DoKhoAI DoKho { get; set; }
        public TrangThaiAI TrangThai => (_hangDoiTarget.Count > 0 || _cacOTrungHienTai.Count > 0) ? TrangThaiAI.Target : TrangThaiAI.Hunt;

        public cMayBanAI(DoKhoAI doKho = DoKhoAI.Kho, int kichThuoc = 10)
        {
            DoKho = doKho;
            _kichThuoc = kichThuoc;
            KhoiTao();
        }

        public void KhoiTao()
        {
            _danhSachChuaBan.Clear();
            _hangDoiTarget.Clear();
            _cacOTrungHienTai.Clear();

            for (int r = 0; r < _kichThuoc; r++)
            {
                for (int c = 0; c < _kichThuoc; c++)
                {
                    _danhSachChuaBan.Add(new Point(c, r));
                }
            }
        }

        public Point ChonToaDoBan()
        {
            // Pha Target: Ban cac o trong hang doi
            while (_hangDoiTarget.Count > 0)
            {
                Point p = _hangDoiTarget.Dequeue();
                if (_danhSachChuaBan.Contains(p))
                {
                    return p;
                }
            }

            // Neu hang doi target het nhung van con cac o da ban trung chua chim (vi du lech truc hoac het dau mut theo huong cu),
            // tim cac o chua ban tiep giap voi cac o trung de tiep tuc Target, tranh bi rot ve Hunt ngau nhien
            if (_cacOTrungHienTai.Count > 0)
            {
                foreach (var hit in _cacOTrungHienTai)
                {
                    ThemCacOLanCan(hit);
                }

                while (_hangDoiTarget.Count > 0)
                {
                    Point p = _hangDoiTarget.Dequeue();
                    if (_danhSachChuaBan.Contains(p))
                    {
                        return p;
                    }
                }
            }

            if (_danhSachChuaBan.Count == 0)
            {
                return new Point(-1, -1);
            }

            // Pha Hunt
            if (DoKho == DoKhoAI.Kho)
            {
                // Ban so le theo ban co vua de tim tau nhanh hon 50%
                var oSoLe = _danhSachChuaBan.Where(p => (p.X + p.Y) % 2 == 0).ToList();
                if (oSoLe.Count > 0)
                {
                    return oSoLe[_rand.Next(oSoLe.Count)];
                }
            }

            // Ban ngau nhien trong cac o con lai
            return _danhSachChuaBan[_rand.Next(_danhSachChuaBan.Count)];
        }

        public void CapNhatKetQua(Point p, KetQuaBan kq, cTau tauBiBan = null)
        {
            _danhSachChuaBan.Remove(p);

            if (kq == KetQuaBan.Hit)
            {
                if (!_cacOTrungHienTai.Contains(p))
                {
                    _cacOTrungHienTai.Add(p);
                }

                if (_cacOTrungHienTai.Count == 1)
                {
                    // Trung phat dau: them 4 o xung quanh
                    ThemCacOLanCan(p);
                }
                else
                {
                    // Trung tu 2 phat: xac dinh duoc truc tau (ngang/doc)
                    KhoaTrucVaThemODauMut();
                }
            }
            else if (kq == KetQuaBan.Sunk)
            {
                if (tauBiBan != null && tauBiBan.DanhSachToaDo != null && tauBiBan.DanhSachToaDo.Count > 0)
                {
                    // Loai bo cac toa do cua con tau da chim khoi danh sach o dang theo doi
                    _cacOTrungHienTai.RemoveAll(pt => tauBiBan.DanhSachToaDo.Contains(pt));
                }
                else
                {
                    _cacOTrungHienTai.Clear();
                }

                _hangDoiTarget.Clear();

                // Neu van con o bi trung cua tau khac gan do, tiep tuc Target o do
                if (_cacOTrungHienTai.Count > 0)
                {
                    ThemCacOLanCan(_cacOTrungHienTai[0]);
                }
            }
        }

        private void ThemCacOLanCan(Point p)
        {
            Point[] huong = {
                new Point(p.X, p.Y - 1), // Tren
                new Point(p.X, p.Y + 1), // Duoi
                new Point(p.X - 1, p.Y), // Trai
                new Point(p.X + 1, p.Y)  // Phai
            };

            foreach (var o in huong)
            {
                if (TrongPhamVi(o) && _danhSachChuaBan.Contains(o) && !_hangDoiTarget.Contains(o))
                {
                    _hangDoiTarget.Enqueue(o);
                }
            }
        }

        private void KhoaTrucVaThemODauMut()
        {
            if (_cacOTrungHienTai.Count < 2) return;

            // Tìm nhóm ô thẳng hàng ngang hoặc dọc có >= 2 phát bắn
            var nhomY = _cacOTrungHienTai.GroupBy(pt => pt.Y).FirstOrDefault(g => g.Count() >= 2);
            var nhomX = _cacOTrungHienTai.GroupBy(pt => pt.X).FirstOrDefault(g => g.Count() >= 2);

            bool isNgang = nhomY != null;
            bool isDoc = nhomX != null;

            // Nếu chưa có nhóm nào >= 2 ô thẳng hàng, mở rộng các ô lân cận của ô mới nhất
            if (!isNgang && !isDoc)
            {
                ThemCacOLanCan(_cacOTrungHienTai.Last());
                return;
            }

            // Nếu xuất hiện cả 2 trục (2 tàu chạm nhau), ưu tiên trục chứa phát bắn mới nhất
            Point latest = _cacOTrungHienTai.Last();
            if (isNgang && isDoc)
            {
                if (nhomY != null && nhomY.Any(pt => pt == latest)) isDoc = false;
                else isNgang = false;
            }

            if (isNgang && nhomY != null)
            {
                int y = nhomY.Key;
                var cacOTrenY = nhomY.ToList();
                int minX = cacOTrenY.Min(pt => pt.X);
                int maxX = cacOTrenY.Max(pt => pt.X);

                // Lọc giữ lại các ô cùng trục Y trong hàng đợi
                var dsCu = _hangDoiTarget.Where(o => o.Y == y).ToList();
                _hangDoiTarget.Clear();
                foreach (var o in dsCu) _hangDoiTarget.Enqueue(o);

                ThemODauMut(new Point(minX - 1, y));
                ThemODauMut(new Point(maxX + 1, y));
            }
            else if (isDoc && nhomX != null)
            {
                int x = nhomX.Key;
                var cacOTrenX = nhomX.ToList();
                int minY = cacOTrenX.Min(pt => pt.Y);
                int maxY = cacOTrenX.Max(pt => pt.Y);

                // Lọc giữ lại các ô cùng trục X trong hàng đợi
                var dsCu = _hangDoiTarget.Where(o => o.X == x).ToList();
                _hangDoiTarget.Clear();
                foreach (var o in dsCu) _hangDoiTarget.Enqueue(o);

                ThemODauMut(new Point(x, minY - 1));
                ThemODauMut(new Point(x, maxY + 1));
            }
        }

        private void ThemODauMut(Point p)
        {
            if (TrongPhamVi(p) && _danhSachChuaBan.Contains(p) && !_hangDoiTarget.Contains(p))
            {
                _hangDoiTarget.Enqueue(p);
            }
        }

        private bool TrongPhamVi(Point p)
        {
            return p.X >= 0 && p.X < _kichThuoc && p.Y >= 0 && p.Y < _kichThuoc;
        }
    }
}
