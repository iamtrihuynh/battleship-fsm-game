using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BattleshipGame.DataAccess;
using BattleshipGame.Models;

namespace BattleshipGame.Tests
{
    class Program
    {
        private static int _passCount = 0;
        private static int _failCount = 0;

        static void Assert(bool condition, string testName, string detail = "")
        {
            if (condition)
            {
                _passCount++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("[PASS] ");
                Console.ResetColor();
                Console.WriteLine(testName);
            }
            else
            {
                _failCount++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("[FAIL] ");
                Console.ResetColor();
                Console.WriteLine($"{testName} => {detail}");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=================================================");
            Console.WriteLine(" BATTLESHIP MINI GAME - MEMBER 3 AUTOMATED TESTS ");
            Console.WriteLine("=================================================");

            string tempTestDir = Path.Combine(Path.GetTempPath(), "BattleshipTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempTestDir);
            string testFilePath = Path.Combine(tempTestDir, "thanhtichs.txt");
            cThanhTichDAO.DuongDanFile = testFilePath;

            try
            {
                // Test 1: Normal serialization
                var item1 = new ThanhTichItem(new DateTime(2026, 10, 19, 14, 0, 0), "DoDocHaiQuan", 1850, 24, 115, "PvE - Nâng cao", "Thắng");
                Assert(item1.ToString() == "2026-10-19 14:00:00|DoDocHaiQuan|1850|24|115|PvE - Nâng cao|Thắng", "TC-01: ThanhTichItem.ToString() formats correctly");

                // Test 2: Delimiter Injection
                var item2 = new ThanhTichItem(DateTime.Now, "Captain|Admin|9999", 1000, 20, 100, "PvE", "Thắng");
                Assert(item2.TenNguoiChoi == "Captain-Admin-9999", "TC-02: Delimiter Injection replaced '|' with '-'");

                // Test 3: CRLF Injection
                var item3 = new ThanhTichItem(DateTime.Now, "Hacker\r\nFakePlayer|9999", 500, 10, 50, "PvE", "Thắng");
                Assert(!item3.ToString().Contains("\r") && !item3.ToString().Contains("\n"), "TC-03: CRLF Injection stripped newlines");

                // Test 4: UTF-8 BOM Handling
                string bomLine = "\uFEFF2026-10-18 10:00:00|ThuyenTruong|1500|30|120|PvE|Thắng";
                var itemBom = new ThanhTichItem(bomLine);
                Assert(itemBom.NgayChoi.Year == 2026 && itemBom.NgayChoi.Month == 10 && itemBom.NgayChoi.Day == 18, "TC-04: BOM character stripped before DateTime parse");

                // Test 5: Negative clamping
                var itemNeg = new ThanhTichItem(DateTime.Now, "Tester", -500, -10, -30, "PvE", "Thắng");
                Assert(itemNeg.DiemSo == 0 && itemNeg.SoLuotBan == 0 && itemNeg.ThoiGianGiay == 0, "TC-05: Negative values clamped to 0");

                // Test 6: ThoiGianHienThi formatting (over 60 mins)
                var itemLong = new ThanhTichItem(DateTime.Now, "Tester", 100, 10, 3665, "PvE", "Thắng");
                Assert(itemLong.ThoiGianHienThi == "61:05", "TC-06: ThoiGianHienThi handles >= 60 minutes correctly");

                // Test 7: Save and Read
                cThanhTichDAO.LuuThanhTich(item1);
                cThanhTichDAO.LuuThanhTich(new ThanhTichItem(DateTime.Now, "Loser", 500, 50, 200, "PvE", "Thua"));
                var listRead = cThanhTichDAO.DocDanhSach();
                Assert(listRead.Count == 2, "TC-07: DocDanhSach() returns saved records");

                // Test 8: Sorting by DiemCaoNhat
                var sortedDiem = cThanhTichDAO.SapXep(listRead, TieuChiSapXep.DiemCaoNhat);
                Assert(sortedDiem[0].TenNguoiChoi == "DoDocHaiQuan", "TC-08: SapXep DiemCaoNhat ranks highest score first");

                // Test 9: Sorting by ThoiGianNganNhat (Prioritize Wins)
                var winSlow = new ThanhTichItem(DateTime.Now, "WinSlow", 1000, 30, 100, "PvE", "Thắng");
                var loseFast = new ThanhTichItem(DateTime.Now, "LoseFast", 200, 10, 15, "PvE", "Thua");
                var listTime = new List<ThanhTichItem> { loseFast, winSlow };
                var sortedTime = cThanhTichDAO.SapXep(listTime, TieuChiSapXep.ThoiGianNganNhat);
                Assert(sortedTime[0].TenNguoiChoi == "WinSlow", "TC-09: SapXep ThoiGianNganNhat prioritizes Wins over fast Defeats");

                // Test 10: Filtering by Mode
                var pvpItem = new ThanhTichItem(DateTime.Now, "PvPPlayer", 1200, 25, 120, "PvP Hotseat", "Thắng");
                var customItem = new ThanhTichItem(DateTime.Now, "CustomPlayer", 1100, 25, 120, "Custom", "Thắng");
                var allItems = new List<ThanhTichItem> { item1, pvpItem, customItem };

                var pveFiltered = cThanhTichDAO.LocTheoCheDo(allItems, "PvE");
                Assert(pveFiltered.Count == 1 && pveFiltered[0].CheDo == "PvE - Nâng cao", "TC-10: LocTheoCheDo matches PvE prefix correctly");

                var pvpFiltered = cThanhTichDAO.LocTheoCheDo(allItems, "PvP Hotseat");
                Assert(pvpFiltered.Count == 1 && pvpFiltered[0].CheDo == "PvP Hotseat", "TC-11: LocTheoCheDo matches PvP Hotseat exactly");

                var allFiltered = cThanhTichDAO.LocTheoCheDo(allItems, "Tất cả");
                Assert(allFiltered.Count == 3, "TC-12: LocTheoCheDo returns all for 'Tất cả'");

                // Test 13: Null resilience
                var nullFiltered = cThanhTichDAO.LocTheoCheDo(null, "PvE");
                Assert(nullFiltered != null && nullFiltered.Count == 0, "TC-13: LocTheoCheDo handles null input list without throwing");

                // Test 14: Retry pattern on transient file lockout
                string lockFile = Path.Combine(tempTestDir, "lock_test.txt");
                cThanhTichDAO.DuongDanFile = lockFile;

                // Create and hold file lock in background for 80ms
                var lockStream = new FileStream(lockFile, FileMode.Create, FileAccess.Write, FileShare.None);
                Task.Run(() =>
                {
                    Thread.Sleep(80);
                    lockStream.Dispose();
                });

                bool saveResult = cThanhTichDAO.LuuThanhTich(new ThanhTichItem(DateTime.Now, "LockRecovered", 900, 20, 80, "PvE", "Thắng"));
                Assert(saveResult, "TC-14: Retry pattern successfully recovers from transient file lockout");

                // Test 15: File size limit (>10MB DoS protection)
                string hugeFile = Path.Combine(tempTestDir, "huge.txt");
                cThanhTichDAO.DuongDanFile = hugeFile;
                using (var fs = new FileStream(hugeFile, FileMode.Create, FileAccess.Write))
                {
                    fs.SetLength(11 * 1024 * 1024); // 11MB
                }
                var hugeList = cThanhTichDAO.DocDanhSach();
                Assert(hugeList.Count == 0 && cThanhTichDAO.LoiGanNhat != null && cThanhTichDAO.LoiGanNhat.Contains("10MB"),
                    "TC-15: Rejects file > 10MB to protect memory");
            }
            finally
            {
                try { Directory.Delete(tempTestDir, true); } catch { }
            }

            Console.WriteLine("=================================================");
            Console.WriteLine($"RESULTS: {_passCount} PASSED, {_failCount} FAILED");
            Console.WriteLine("=================================================");

            if (_failCount > 0)
            {
                Environment.Exit(1);
            }
        }
    }
}
