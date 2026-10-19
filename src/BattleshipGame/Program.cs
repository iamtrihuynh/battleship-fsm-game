using System;
using System.Windows.Forms;
using BattleshipGame.Logic;
using BattleshipGame.Views;

namespace BattleshipGame
{
    internal static class Program
    {
        /// <summary>
        /// Điểm khởi nhập chính cho ứng dụng Battleship MiniGame.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Tự động thu hồi tài nguyên unmanaged (âm thanh, MCI) khi thoát ứng dụng
            Application.ApplicationExit += (sender, e) =>
            {
                cAmThanh.GiaiPhong();
            };

            // Khởi chạy Bảng thành tích (hoặc đổi thành frmMenu khi Thành viên 2 tích hợp)
            Application.Run(new frmThanhTich());
        }
    }
}
