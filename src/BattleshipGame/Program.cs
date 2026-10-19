using System;
using System.Windows.Forms;
using BattleshipGame.Logic;
using BattleshipGame.Views;

namespace BattleshipGame
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ApplicationExit += (s, e) => cAmThanh.GiaiPhong();

            // Tam thoi chay thang vao bang thanh tich de test
            Application.Run(new frmThanhTich());
        }
    }
}
