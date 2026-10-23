using System;

namespace BattleshipGame.Models
{
    /// <summary>
    /// Trang thai cua mot o toa do tren ban co
    /// </summary>
    public enum TrangThaiO
    {
        Empty,      // O nuoc trong chua bi ban
        HasShip,    // Co tau bi mat (chi hien tren ban co cua minh)
        Hit,        // Ban trung mot phan cua tau
        Miss,       // Ban truot vao o nuoc
        Sunk        // Toan bo con tau da bi ban chim hoan toan
    }

    /// <summary>
    /// Huong dat tau tren ban co
    /// </summary>
    public enum HuongTau
    {
        Ngang = 0,  // Horizontal (X tang dan, Y giu nguyen)
        Doc = 1     // Vertical (Y tang dan, X giu nguyen)
    }

    /// <summary>
    /// Phan loai 5 loai chien ham quy chuan quoc te
    /// </summary>
    public enum LoaiTau
    {
        TauTuanTra = 1,     // Patrol Boat (2 o)
        TauKhuTruc = 2,     // Destroyer (3 o)
        TauNgam = 3,        // Submarine (3 o)
        ThietGiapHam = 4,   // Battleship (4 o)
        TauSanBay = 5       // Aircraft Carrier (5 o)
    }

    /// <summary>
    /// Ket qua tra ve sau mot phat ban
    /// </summary>
    public enum KetQuaBan
    {
        Miss,           // Ban truot vao nuoc
        Hit,            // Ban trung mot o cua tau
        Sunk,           // Ban trung o cuoi cung, lam chim hoan toan tau
        DaBanRoi,       // O toa do nay da bi ban truoc do
        NgoaiBanCo      // Toa do nam ngoai pham vi ban co
    }

    /// <summary>
    /// Che do choi cua game
    /// </summary>
    public enum CheDoChoi
    {
        PvE_De,
        PvE_Kho,
        PvP_Hotseat,
        Custom
    }

    /// <summary>
    /// Tien ich mo rong cho enum
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Lay kich thuoc so o tieu chuan cua loai tau
        /// </summary>
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

        /// <summary>
        /// Lay ten tieng Viet mac dinh cua loai tau
        /// </summary>
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

        /// <summary>
        /// Chuyen enum CheDoChoi thanh chuoi hien thi khop voi cThanhTichDAO va thanhtichs.txt
        /// </summary>
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
