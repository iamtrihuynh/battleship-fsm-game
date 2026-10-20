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

            Application.Run(new frmMenu());
        }
    }
}
