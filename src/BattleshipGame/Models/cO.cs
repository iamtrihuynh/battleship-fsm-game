using System.Drawing;

namespace BattleshipGame.Models
{
    public class cO
    {
        public int X { get; set; }
        public int Y { get; set; }
        public TrangThaiO TrangThai { get; set; }
        public cTau TauChiemGiu { get; set; }

        public Point ToaDo => new Point(X, Y);
        public bool CoTau => TauChiemGiu != null || TrangThai == TrangThaiO.HasShip || TrangThai == TrangThaiO.Hit || TrangThai == TrangThaiO.Sunk;

        public bool DaBiBan => TrangThai == TrangThaiO.Hit || TrangThai == TrangThaiO.Miss || TrangThai == TrangThaiO.Sunk;

        public cO(int x, int y)
        {
            X = x;
            Y = y;
            TrangThai = TrangThaiO.Empty;
            TauChiemGiu = null;
        }

        public void DatTau(cTau tau)
        {
            TauChiemGiu = tau;
            TrangThai = TrangThaiO.HasShip;
        }

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
