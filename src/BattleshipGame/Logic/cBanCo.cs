using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BattleshipGame.Models;

namespace BattleshipGame.Logic
{
    public class cBanCo
    {
        public const int KICH_THUOC_CHUAN = 10;

        public int KichThuoc { get; private set; }
        public cO[,] MaTranO { get; private set; }
        public List<cTau> DanhSachTau { get; private set; }
        public int TongSoOTau => DanhSachTau.Sum(t => t.KichThuoc);
        public int TongSoOBiBan => DanhSachTau.Sum(t => t.SoOBiBan);
        public bool TatCaTauDaDatXong => DanhSachTau.Count > 0 && DanhSachTau.All(t => t.DaDatXong);
        public bool TatCaTauDaChim => DanhSachTau.Count > 0 && DanhSachTau.All(t => t.DaChim);
        public cBanCo(int kichThuoc = KICH_THUOC_CHUAN)
        {
            KichThuoc = Math.Max(6, kichThuoc);
            MaTranO = new cO[KichThuoc, KichThuoc];
            DanhSachTau = new List<cTau>();

            KhoiTaoBanCo();
            KhoiTaoDanhSachTauChuan();
        }

        public void KhoiTaoBanCo()
        {
            for (int x = 0; x < KichThuoc; x++)
            {
                for (int y = 0; y < KichThuoc; y++)
                {
                    MaTranO[x, y] = new cO(x, y);
                }
            }
        }

        public void KhoiTaoDanhSachTauChuan()
        {
            DanhSachTau.Clear();
            DanhSachTau.Add(new cTau(LoaiTau.TauSanBay));
            DanhSachTau.Add(new cTau(LoaiTau.ThietGiapHam));
            DanhSachTau.Add(new cTau(LoaiTau.TauKhuTruc));
            DanhSachTau.Add(new cTau(LoaiTau.TauNgam));
            DanhSachTau.Add(new cTau(LoaiTau.TauTuanTra));
        }

        public bool TrongPhamVi(int x, int y)
        {
            return x >= 0 && x < KichThuoc && y >= 0 && y < KichThuoc;
        }

        public bool TrongPhamVi(Point p) => TrongPhamVi(p.X, p.Y);

        public static List<Point> LayDanhSachToaDo(Point goc, int doDai, HuongTau huong)
        {
            if (doDai <= 0) return new List<Point>();
            List<Point> list = new List<Point>(doDai);
            for (int i = 0; i < doDai; i++)
            {
                int x = (huong == HuongTau.Ngang) ? goc.X + i : goc.X;
                int y = (huong == HuongTau.Doc) ? goc.Y + i : goc.Y;
                list.Add(new Point(x, y));
            }
            return list;
        }

        public bool KiemTraDatTauHopLe(cTau tau, Point goc, HuongTau huong)
        {
            if (tau == null || tau.KichThuoc <= 0) return false;

            List<Point> toaDoDuKien = LayDanhSachToaDo(goc, tau.KichThuoc, huong);

            foreach (Point p in toaDoDuKien)
            {
                if (!TrongPhamVi(p))
                    return false;

                cO o = MaTranO[p.X, p.Y];
                if (o.CoTau && o.TauChiemGiu != tau)
                    return false;
            }

            return true;
        }
        public bool DatTau(cTau tau, Point goc, HuongTau huong)
        {
            if (tau == null) return false;

            List<Point> viTriCu = new List<Point>(tau.DanhSachToaDo);
            HuongTau huongCu = tau.Huong;
            bool daDatTruocDo = tau.DaDatXong;

            GoTau(tau);

            if (!KiemTraDatTauHopLe(tau, goc, huong))
            {
                if (daDatTruocDo && viTriCu.Count > 0)
                {
                    tau.GanToaDo(viTriCu, huongCu);
                    foreach (Point p in viTriCu)
                    {
                        if (TrongPhamVi(p))
                            MaTranO[p.X, p.Y].DatTau(tau);
                    }
                }
                return false;
            }

            List<Point> toaDo = LayDanhSachToaDo(goc, tau.KichThuoc, huong);
            tau.GanToaDo(toaDo, huong);

            foreach (Point p in toaDo)
            {
                MaTranO[p.X, p.Y].DatTau(tau);
            }

            if (!DanhSachTau.Contains(tau))
                DanhSachTau.Add(tau);

            return true;
        }
        public void GoTau(cTau tau)
        {
            if (tau == null) return;

            foreach (Point p in tau.DanhSachToaDo)
            {
                if (TrongPhamVi(p) && MaTranO[p.X, p.Y].TauChiemGiu == tau)
                {
                    MaTranO[p.X, p.Y].Reset();
                }
            }
            tau.Reset();
        }
        public void XoaTatCaTau()
        {
            for (int x = 0; x < KichThuoc; x++)
            {
                for (int y = 0; y < KichThuoc; y++)
                {
                    MaTranO[x, y].Reset();
                }
            }

            foreach (var tau in DanhSachTau)
            {
                tau.Reset();
            }
        }

        public bool TuDongSinhBanCoNgauNhien(Random rand = null)
        {
            rand = rand ?? new Random();
            const int maxGlobalTries = 50;

            for (int attempt = 0; attempt < maxGlobalTries; attempt++)
            {
                XoaTatCaTau();

                var danhSachSapXep = DanhSachTau.OrderByDescending(t => t.KichThuoc).ToList();
                bool datThanhCongTatCa = true;

                foreach (cTau tau in danhSachSapXep)
                {
                    bool daDatTau = false;
                    const int maxShipTries = 150;

                    for (int t = 0; t < maxShipTries; t++)
                    {
                        HuongTau huong = (rand.Next(2) == 0) ? HuongTau.Ngang : HuongTau.Doc;
                        int maxX = (huong == HuongTau.Ngang) ? KichThuoc - tau.KichThuoc : KichThuoc - 1;
                        int maxY = (huong == HuongTau.Doc) ? KichThuoc - tau.KichThuoc : KichThuoc - 1;

                        if (maxX < 0 || maxY < 0) continue;

                        int startX = rand.Next(0, maxX + 1);
                        int startY = rand.Next(0, maxY + 1);
                        Point goc = new Point(startX, startY);

                        if (KiemTraDatTauHopLe(tau, goc, huong))
                        {
                            DatTau(tau, goc, huong);
                            daDatTau = true;
                            break;
                        }
                    }

                    if (!daDatTau)
                    {
                        datThanhCongTatCa = false;
                        break;
                    }
                }

                if (datThanhCongTatCa && TatCaTauDaDatXong)
                {
                    return true;
                }
            }

            return TatCaTauDaDatXong;
        }
        public KetQuaBan TiepNhanPhatBan(Point p, out cTau tauBiBan)
        {
            tauBiBan = null;

            if (!TrongPhamVi(p))
                return KetQuaBan.NgoaiBanCo;

            cO o = MaTranO[p.X, p.Y];
            if (o.DaBiBan)
                return KetQuaBan.DaBanRoi;

            if (o.CoTau)
            {
                tauBiBan = o.TauChiemGiu;
                tauBiBan?.KiemTraBiBan(p);
                o.TrangThai = TrangThaiO.Hit;

                if (tauBiBan != null && tauBiBan.DaChim)
                {
                    foreach (Point pt in tauBiBan.DanhSachToaDo)
                    {
                        if (TrongPhamVi(pt))
                        {
                            MaTranO[pt.X, pt.Y].TrangThai = TrangThaiO.Sunk;
                        }
                    }
                    return KetQuaBan.Sunk;
                }

                return KetQuaBan.Hit;
            }
            else
            {
                o.TrangThai = TrangThaiO.Miss;
                return KetQuaBan.Miss;
            }
        }

        public KetQuaBan TiepNhanPhatBan(Point p)
        {
            return TiepNhanPhatBan(p, out _);
        }
    }
}
