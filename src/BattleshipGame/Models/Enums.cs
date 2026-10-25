using System;

namespace BattleshipGame.Models
{
    public enum TrangThaiO
    {
        Empty,
        HasShip,
        Hit,
        Miss,
        Sunk
    }

    public enum HuongTau
    {
        Ngang = 0,
        Doc = 1
    }

    public enum LoaiTau
    {
        TauTuanTra = 1,
        TauKhuTruc = 2,
        TauNgam = 3,
        ThietGiapHam = 4,
        TauSanBay = 5
    }

    public enum KetQuaBan
    {
        Miss,
        Hit,
        Sunk,
        DaBanRoi,
        NgoaiBanCo
    }

    public enum CheDoChoi
    {
        PvE_De,
        PvE_Kho,
        PvP_Hotseat,
        Custom
    }

    public static class EnumExtensions
    {
        public static int LayKichThuocChuan(this LoaiTau loai)
        {
            switch (loai)
            {
                case LoaiTau.TauTuanTra: return 2;
                case LoaiTau.TauKhuTruc: return 3;
                case LoaiTau.TauNgam: return 3;
                case LoaiTau.ThietGiapHam: return 4;
                case LoaiTau.TauSanBay: return 5;
                default: return 2;
            }
        }

        public static string LayTenMacDinh(this LoaiTau loai)
        {
            switch (loai)
            {
                case LoaiTau.TauTuanTra: return "Tàu Tuần Tra";
                case LoaiTau.TauKhuTruc: return "Tàu Khu Trục";
                case LoaiTau.TauNgam: return "Tàu Ngầm";
                case LoaiTau.ThietGiapHam: return "Thiết Giáp Hạm";
                case LoaiTau.TauSanBay: return "Tàu Sân Bay";
                default: return "Chiến Hạm";
            }
        }

        public static string ToDisplayName(this CheDoChoi cheDo)
        {
            switch (cheDo)
            {
                case CheDoChoi.PvE_De:
                    return "PvE - Dễ";
                case CheDoChoi.PvE_Kho:
                    return "PvE - Khó";
                case CheDoChoi.PvP_Hotseat:
                    return "PvP Hotseat";
                case CheDoChoi.Custom:
                    return "Custom";
                default:
                    return "PvE";
            }
        }
    }
}
