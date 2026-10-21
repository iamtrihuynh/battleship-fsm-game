#nullable enable
using System;
using System.Drawing;
using System.Linq;
using BattleshipGame.Models;

namespace BattleshipGame.Logic
{
    public enum TrangThaiVanDau
    {
        ChuanBi,
        DangChoi,
        TamDung,
        KetThuc
    }

    public class cQuanLyVanDau
    {
        public cBanCo BanCoNguoi1 { get; private set; }
        public cBanCo BanCoNguoi2 { get; private set; }
        public cMayBanAI? MayAI { get; private set; }

        public CheDoChoi CheDo { get; set; }
        public TrangThaiVanDau TrangThai { get; private set; }
        public int LuotNguoiChoi { get; private set; } // 1: Nguoi 1, 2: Nguoi 2 / May

        public int SoPhatBanNguoi1 { get; private set; }
        public int SoPhatBanNguoi2 { get; private set; }
        public int ThoiGianGiay { get; set; }
        public int NguoiThang { get; private set; } // 0: Chua ai, 1: Nguoi 1, 2: Nguoi 2 / May

        public cQuanLyVanDau(CheDoChoi cheDo)
        {
            CheDo = cheDo;
            BanCoNguoi1 = new cBanCo();
            BanCoNguoi2 = new cBanCo();
            TrangThai = TrangThaiVanDau.ChuanBi;
            LuotNguoiChoi = 1;
            NguoiThang = 0;

            if (cheDo == CheDoChoi.PvE_De)
            {
                MayAI = new cMayBanAI(DoKhoAI.De);
            }
            else if (cheDo == CheDoChoi.PvE_Kho)
            {
                MayAI = new cMayBanAI(DoKhoAI.Kho);
            }
        }

        public void BatDauTranDau()
        {
            TrangThai = TrangThaiVanDau.DangChoi;
            LuotNguoiChoi = 1;
            SoPhatBanNguoi1 = 0;
            SoPhatBanNguoi2 = 0;
            ThoiGianGiay = 0;
            NguoiThang = 0;
        }

        public KetQuaBan Nguoi1Ban(Point p, out cTau? tauBiChim)
        {
            tauBiChim = null;
            if (TrangThai != TrangThaiVanDau.DangChoi || LuotNguoiChoi != 1)
                return KetQuaBan.DaBanRoi;

            KetQuaBan kq = BanCoNguoi2.TiepNhanPhatBan(p, out tauBiChim);
            if (kq != KetQuaBan.DaBanRoi && kq != KetQuaBan.NgoaiBanCo)
            {
                SoPhatBanNguoi1++;

                if (BanCoNguoi2.TatCaTauDaChim)
                {
                    NguoiThang = 1;
                    TrangThai = TrangThaiVanDau.KetThuc;
                }
                else
                {
                    LuotNguoiChoi = 2;
                }
            }
            return kq;
        }

        public KetQuaBan Nguoi1Ban(int x, int y, out cTau? tauBiChim) => Nguoi1Ban(new Point(x, y), out tauBiChim);

        public KetQuaBan Nguoi2Ban(Point p, out cTau? tauBiChim)
        {
            tauBiChim = null;
            if (TrangThai != TrangThaiVanDau.DangChoi || LuotNguoiChoi != 2)
                return KetQuaBan.DaBanRoi;

            KetQuaBan kq = BanCoNguoi1.TiepNhanPhatBan(p, out tauBiChim);
            if (kq != KetQuaBan.DaBanRoi && kq != KetQuaBan.NgoaiBanCo)
            {
                SoPhatBanNguoi2++;

                if (BanCoNguoi1.TatCaTauDaChim)
                {
                    NguoiThang = 2;
                    TrangThai = TrangThaiVanDau.KetThuc;
                }
                else
                {
                    LuotNguoiChoi = 1;
                }
            }
            return kq;
        }

        public KetQuaBan Nguoi2Ban(int x, int y, out cTau? tauBiChim) => Nguoi2Ban(new Point(x, y), out tauBiChim);

        public KetQuaBan MayBanNuocTiepTheo(out Point toaDoBan, out cTau? tauBiChim)
        {
            toaDoBan = new Point(-1, -1);
            tauBiChim = null;

            if (MayAI == null || TrangThai != TrangThaiVanDau.DangChoi || LuotNguoiChoi != 2)
                return KetQuaBan.DaBanRoi;

            toaDoBan = MayAI.ChonToaDoBan();
            KetQuaBan kq = BanCoNguoi1.TiepNhanPhatBan(toaDoBan, out tauBiChim);
            MayAI.CapNhatKetQua(toaDoBan, kq);
            SoPhatBanNguoi2++;

            if (BanCoNguoi1.TatCaTauDaChim)
            {
                NguoiThang = 2;
                TrangThai = TrangThaiVanDau.KetThuc;
            }
            else
            {
                LuotNguoiChoi = 1;
            }

            return kq;
        }

        public int TinhDiemNguoi1()
        {
            if (NguoiThang != 1) return 0;

            int soTauConSong = BanCoNguoi1.DanhSachTau.Count(t => !t.DaChim);
            int diem = 1000 + (soTauConSong * 200) - (SoPhatBanNguoi1 * 15) - (ThoiGianGiay * 2);
            return Math.Max(0, diem);
        }

        public void TamDungHoacTiepTuc()
        {
            if (TrangThai == TrangThaiVanDau.DangChoi)
                TrangThai = TrangThaiVanDau.TamDung;
            else if (TrangThai == TrangThaiVanDau.TamDung)
                TrangThai = TrangThaiVanDau.DangChoi;
        }

        public void DauHang(int nguoiDauHang)
        {
            if (TrangThai != TrangThaiVanDau.DangChoi && TrangThai != TrangThaiVanDau.TamDung)
                return;

            NguoiThang = (nguoiDauHang == 1) ? 2 : 1;
            TrangThai = TrangThaiVanDau.KetThuc;
        }
    }
}
