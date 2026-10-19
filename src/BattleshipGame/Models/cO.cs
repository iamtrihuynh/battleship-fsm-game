using System.Drawing;

namespace BattleshipGame.Models
{
    /// <summary>
    /// Dai dien cho 1 o toa do tren ban co (X, Y)
    /// </summary>
    public class cO
    {
        public int X { get; set; }
        public int Y { get; set; }
        public TrangThaiO TrangThai { get; set; }
        public cTau TauChiemGiu { get; set; }

        public Point ToaDo => new Point(X, Y);

        /// <summary>
        /// Kiem tra o nay co tau chiem giu hay khong
        /// </summary>
        public bool CoTau => TauChiemGiu != null || TrangThai == TrangThaiO.HasShip || TrangThai == TrangThaiO.Hit || TrangThai == TrangThaiO.Sunk;

        /// <summary>
        /// Kiem tra o nay da tung bi ban hay chua
        /// </summary>
        public bool DaBiBan => TrangThai == TrangThaiO.Hit || TrangThai == TrangThaiO.Miss || TrangThai == TrangThaiO.Sunk;

        public cO(int x, int y)
        {
            X = x;
            Y = y;
            TrangThai = TrangThaiO.Empty;
            TauChiemGiu = null;
        }

        /// <summary>
        /// Gan mot con tau vao o toa do nay
        /// </summary>
        public void DatTau(cTau tau)
        {
            TauChiemGiu = tau;
            TrangThai = TrangThaiO.HasShip;
        }

        /// <summary>
        /// Lam sach o ve trang thai nuoc trong ban dau
        /// </summary>
        public void Reset()
        {
            TrangThai = TrangThaiO.Empty;
            TauChiemGiu = null;
        }

        public override string ToString()
        {
            return $"Ô({X}, {Y}): {TrangThai}" + (TauChiemGiu != null ? $" [{TauChiemGiu.TenTau}]" : "");
        }
    }
}
