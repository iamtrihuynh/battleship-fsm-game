using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BattleshipGame.Models;

namespace BattleshipGame.Logic
{
    /// <summary>
    /// Quan ly khong gian ban co ma tran 2 chieu va ham doi chien ham
    /// </summary>
    public class cBanCo
    {
        public const int KICH_THUOC_CHUAN = 10;

        public int KichThuoc { get; private set; }
        public cO[,] MaTranO { get; private set; }
        public List<cTau> DanhSachTau { get; private set; }

        /// <summary>
        /// Tong so o cua toan bo ham doi (17 o trong ban co chuan)
        /// </summary>
        public int TongSoOTau => DanhSachTau.Sum(t => t.KichThuoc);

        /// <summary>
        /// Tong so o tau da bi ban trung
        /// </summary>
        public int TongSoOBiBan => DanhSachTau.Sum(t => t.SoOBiBan);

        /// <summary>
        /// Kiem tra toan bo 5 chien ham da duoc xep hop le len ban co chua
        /// </summary>
        public bool TatCaTauDaDatXong => DanhSachTau.Count > 0 && DanhSachTau.All(t => t.DaDatXong);

        /// <summary>
        /// Kiem tra toan bo ham doi da bi ban chim hoan toan chua (Dieu kien ket thuc tran)
        /// </summary>
        public bool TatCaTauDaChim => DanhSachTau.Count > 0 && DanhSachTau.All(t => t.DaChim);

        public cBanCo(int kichThuoc = KICH_THUOC_CHUAN)
        {
            KichThuoc = Math.Max(6, kichThuoc);
            MaTranO = new cO[KichThuoc, KichThuoc];
            DanhSachTau = new List<cTau>();

            KhoiTaoBanCo();
            KhoiTaoDanhSachTauChuan();
        }

        /// <summary>
        /// Khoi tao ma tran luoi o trong
        /// </summary>
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

        /// <summary>
        /// Khoi tao 5 loai chien ham tieu chuan (tong 17 o)
        /// </summary>
        public void KhoiTaoDanhSachTauChuan()
        {
            DanhSachTau.Clear();
            DanhSachTau.Add(new cTau(LoaiTau.TauSanBay));       // 5 o
            DanhSachTau.Add(new cTau(LoaiTau.ThietGiapHam));     // 4 o
            DanhSachTau.Add(new cTau(LoaiTau.TauKhuTruc));       // 3 o
            DanhSachTau.Add(new cTau(LoaiTau.TauNgam));          // 3 o
            DanhSachTau.Add(new cTau(LoaiTau.TauTuanTra));       // 2 o
        }

        /// <summary>
        /// Kiem tra toa do co nam trong pham vi ban co hay khong
        /// </summary>
        public bool TrongPhamVi(int x, int y)
        {
            return x >= 0 && x < KichThuoc && y >= 0 && y < KichThuoc;
        }

        public bool TrongPhamVi(Point p) => TrongPhamVi(p.X, p.Y);

        /// <summary>
        /// Sinh danh sach toa do du kien khi dat tau tai vi tri goc theo huong nhat dinh
        /// </summary>
        public static List<Point> LayDanhSachToaDo(Point goc, int doDai, HuongTau huong)
        {
            List<Point> list = new List<Point>(doDai);
            for (int i = 0; i < doDai; i++)
            {
                int x = (huong == HuongTau.Ngang) ? goc.X + i : goc.X;
                int y = (huong == HuongTau.Doc) ? goc.Y + i : goc.Y;
                list.Add(new Point(x, y));
            }
            return list;
        }

        /// <summary>
        /// Thuat toan kiem tra tinh hop le khi xep tau:
        /// 1. Khong tran bien ban co (Boundary check)
        /// 2. Khong de len than tau khac (Overlap check)
        /// </summary>
        public bool KiemTraDatTauHopLe(cTau tau, Point goc, HuongTau huong)
        {
            if (tau == null || tau.KichThuoc <= 0) return false;

            List<Point> toaDoDuKien = LayDanhSachToaDo(goc, tau.KichThuoc, huong);

            foreach (Point p in toaDoDuKien)
            {
                // 1. Kiem tra tran bien
                if (!TrongPhamVi(p))
                    return false;

                // 2. Kiem tra va cham / de len tau khac
                cO o = MaTranO[p.X, p.Y];
                if (o.CoTau && o.TauChiemGiu != tau)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Dat tau len ban co tai toa do goc va huong chi dinh
        /// </summary>
        public bool DatTau(cTau tau, Point goc, HuongTau huong)
        {
            if (tau == null) return false;

            // Neu tau da duoc dat truoc do o vi tri khac, go khoi vi tri cu
            GoTau(tau);

            if (!KiemTraDatTauHopLe(tau, goc, huong))
                return false;

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

        /// <summary>
        /// Go bo mot con tau khoi ban co va thu hoi ve kho
        /// </summary>
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

        /// <summary>
        /// Xoa sach ban co va thu hoi toan bo ham doi ve kho de xep lai
        /// </summary>
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

        /// <summary>
        /// Thuat toan tu dong sinh ngau nhien hop le toan bo 5 chien ham trong 1 click.
        /// Uu tien xep tu tau dai nhat (5 o) den tau ngan nhat (2 o) de giam thieu xac suat va cham.
        /// Co co che chong tac nghen ma tran (Safety retry).
        /// </summary>
        public bool TuDongSinhBanCoNgauNhien(Random rand = null)
        {
            rand = rand ?? new Random();
            const int maxGlobalTries = 50;

            for (int attempt = 0; attempt < maxGlobalTries; attempt++)
            {
                XoaTatCaTau();

                // Sap xep tau theo thu tu giam dan ve do dai de de xep nhat
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

        /// <summary>
        /// Tiep nhan mot phat ban vao toa do p tren ban co nay.
        /// Tra ve enum KetQuaBan (Miss, Hit, Sunk, DaBanRoi, NgoaiBanCo)
        /// va out con tau bi ban trung neu co.
        /// </summary>
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
                    // Chuyen tat ca cac o cua con tau da chim sang mau/icon Sunk
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

        /// <summary>
        /// Tiep nhan phat ban khong can lay tham chieu tau
        /// </summary>
        public KetQuaBan TiepNhanPhatBan(Point p)
        {
            return TiepNhanPhatBan(p, out _);
        }
    }
}
