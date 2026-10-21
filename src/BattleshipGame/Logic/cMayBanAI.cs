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
        public TrangThaiAI TrangThai => _hangDoiTarget.Count > 0 ? TrangThaiAI.Target : TrangThaiAI.Hunt;

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
                _cacOTrungHienTai.Add(p);

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
                // Tau da chim: quay lai pha Hunt
                _cacOTrungHienTai.Clear();
                _hangDoiTarget.Clear();
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
            Point p1 = _cacOTrungHienTai[0];
            Point p2 = _cacOTrungHienTai[1];
            bool isNgang = p1.Y == p2.Y;

            // Xoa cac o lech truc khoi hang doi
            var dsCu = _hangDoiTarget.ToList();
            _hangDoiTarget.Clear();

            foreach (var o in dsCu)
            {
                if ((isNgang && o.Y == p1.Y) || (!isNgang && o.X == p1.X))
                {
                    _hangDoiTarget.Enqueue(o);
                }
            }

            // Mo rong ban tiep 2 dau mut
            if (isNgang)
            {
                int y = p1.Y;
                int minX = _cacOTrungHienTai.Min(pt => pt.X);
                int maxX = _cacOTrungHienTai.Max(pt => pt.X);

                ThemODauMut(new Point(minX - 1, y));
                ThemODauMut(new Point(maxX + 1, y));
            }
            else
            {
                int x = p1.X;
                int minY = _cacOTrungHienTai.Min(pt => pt.Y);
                int maxY = _cacOTrungHienTai.Max(pt => pt.Y);

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
