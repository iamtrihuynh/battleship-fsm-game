using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BattleshipGame.DataAccess;
using BattleshipGame.Logic;
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
            Console.WriteLine("  BATTLESHIP MINI GAME - AUTOMATED TEST SUITE    ");
            Console.WriteLine("=================================================");

            string tempTestDir = Path.Combine(Path.GetTempPath(), "BattleshipTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempTestDir);
            string testFilePath = Path.Combine(tempTestDir, "thanhtichs.txt");
            cThanhTichDAO.DuongDanFile = testFilePath;

            try
            {
                Console.WriteLine("\n--- PHẦN 1: MEMBER 3 (DAO, Serialization, File I/O) ---");

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

                Console.WriteLine("\n--- PHẦN 2: MEMBER 1 (Models, Board Logic, Collision, Random Placement, Combat) ---");

                // Test 16: Khoi tao ban co 10x10 va 5 tau
                var banCo = new cBanCo(10);
                bool khoiTaoHopLe = (banCo.KichThuoc == 10) && (banCo.DanhSachTau.Count == 5) && (banCo.TongSoOTau == 17);
                Assert(khoiTaoHopLe, "TC-16: Khởi tạo bàn cờ 10x10 chuẩn 5 tàu (tổng 17 ô)");

                // Test 17: Dat tau theo chieu Ngang
                var tauTuanTra = banCo.DanhSachTau.First(t => t.Loai == LoaiTau.TauTuanTra); // 2 o
                bool datNgang = banCo.DatTau(tauTuanTra, new Point(0, 0), HuongTau.Ngang);
                bool oNgangHopLe = datNgang && banCo.MaTranO[0, 0].CoTau && banCo.MaTranO[1, 0].CoTau && !banCo.MaTranO[2, 0].CoTau;
                Assert(oNgangHopLe, "TC-17: Đặt tàu hợp lệ theo chiều Ngang (X tăng)");

                // Test 18: Dat tau theo chieu Doc
                var tauKhuTruc = banCo.DanhSachTau.First(t => t.Loai == LoaiTau.TauKhuTruc); // 3 o
                bool datDoc = banCo.DatTau(tauKhuTruc, new Point(4, 2), HuongTau.Doc);
                bool oDocHopLe = datDoc && banCo.MaTranO[4, 2].CoTau && banCo.MaTranO[4, 3].CoTau && banCo.MaTranO[4, 4].CoTau;
                Assert(oDocHopLe, "TC-18: Đặt tàu hợp lệ theo chiều Dọc (Y tăng)");

                // Test 19: Chặn tràn viền bàn cờ (Boundary Collision)
                var tauSanBay = banCo.DanhSachTau.First(t => t.Loai == LoaiTau.TauSanBay); // 5 o
                bool tranVienPhai = banCo.KiemTraDatTauHopLe(tauSanBay, new Point(7, 0), HuongTau.Ngang); // 7 + 5 = 12 > 10
                bool tranVienDay = banCo.KiemTraDatTauHopLe(tauSanBay, new Point(0, 8), HuongTau.Doc);    // 8 + 5 = 13 > 10
                Assert(!tranVienPhai && !tranVienDay, "TC-19: Chặn đặt tàu tràn biên mép phải và mép đáy bàn cờ");

                // Test 20: Chặn đè lên tàu khác (Overlap Collision)
                // tauTuanTra dang nam tai (0,0) va (1,0)
                bool deLenTau = banCo.KiemTraDatTauHopLe(tauSanBay, new Point(1, 0), HuongTau.Doc); // (1,0) bi trung
                Assert(!deLenTau, "TC-20: Chặn đặt tàu chồng đè (overlap) lên tàu khác");

                // Test 21: Xóa sạch bàn cờ (Reset)
                banCo.XoaTatCaTau();
                bool banCoRong = true;
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        if (banCo.MaTranO[x, y].CoTau) { banCoRong = false; break; }
                    }
                }
                bool tauDaReset = banCo.DanhSachTau.All(t => !t.DaDatXong && t.DanhSachToaDo.Count == 0);
                Assert(banCoRong && tauDaReset, "TC-21: Xóa bàn cờ (Reset) thu hồi sạch hạm đội về kho");

                // Test 22: Thuật toán tự động sinh ngẫu nhiên hợp lệ 5 chiến hạm
                bool randomThanhCong = banCo.TuDongSinhBanCoNgauNhien(new Random(12345));
                int demOTau = 0;
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        if (banCo.MaTranO[x, y].CoTau) demOTau++;
                    }
                }
                bool du17O = randomThanhCong && banCo.TatCaTauDaDatXong && (demOTau == 17);
                Assert(du17O, "TC-22: Tự động xếp ngẫu nhiên thành công 5 tàu, đúng 17 ô không va chạm");

                // Test 23: Tiếp nhận phát bắn Trượt (Miss)
                banCo.XoaTatCaTau();
                banCo.DatTau(tauTuanTra, new Point(3, 3), HuongTau.Ngang); // (3,3) va (4,3)
                var kqMiss = banCo.TiepNhanPhatBan(new Point(0, 0));
                Assert(kqMiss == KetQuaBan.Miss && banCo.MaTranO[0, 0].TrangThai == TrangThaiO.Miss,
                    "TC-23: Bắn vào ô nước trống trả về KetQuaBan.Miss");

                // Test 24: Tiếp nhận phát bắn Trúng (Hit)
                var kqHit = banCo.TiepNhanPhatBan(new Point(3, 3), out cTau tauTrung);
                bool hitHopLe = (kqHit == KetQuaBan.Hit) && (tauTrung == tauTuanTra) && (tauTuanTra.SoOBiBan == 1) && !tauTuanTra.DaChim;
                Assert(hitHopLe, "TC-24: Bắn trúng ô tàu trả về KetQuaBan.Hit và tăng SoOBiBan");

                // Test 25: Tiếp nhận phát bắn Chìm tàu (Sunk)
                var kqSunk = banCo.TiepNhanPhatBan(new Point(4, 3), out cTau tauChim);
                bool sunkHopLe = (kqSunk == KetQuaBan.Sunk) && (tauChim == tauTuanTra) && tauTuanTra.DaChim &&
                                 (banCo.MaTranO[3, 3].TrangThai == TrangThaiO.Sunk) &&
                                 (banCo.MaTranO[4, 3].TrangThai == TrangThaiO.Sunk);
                Assert(sunkHopLe, "TC-25: Bắn trúng ô cuối cùng làm chìm tàu (Sunk) và đổi các ô sang Sunk");

                // Test 26: Bắn lặp ô đã bắn hoặc ngoài biên
                var kqDaBan = banCo.TiepNhanPhatBan(new Point(3, 3));
                var kqNgoaiBien = banCo.TiepNhanPhatBan(new Point(-1, 5));
                Assert(kqDaBan == KetQuaBan.DaBanRoi && kqNgoaiBien == KetQuaBan.NgoaiBanCo,
                    "TC-26: Bẫy lỗi bắn lặp ô trả về DaBanRoi và ngoài biên trả về NgoaiBanCo");

                // Test 27: Tiêu diệt toàn bộ hạm đội (Điều kiện Thắng ván đấu)
                banCo.XoaTatCaTau();
                // Đặt cả 5 tàu vào các hàng riêng biệt
                banCo.DatTau(banCo.DanhSachTau[0], new Point(0, 0), HuongTau.Ngang); // size 5
                banCo.DatTau(banCo.DanhSachTau[1], new Point(0, 2), HuongTau.Ngang); // size 4
                banCo.DatTau(banCo.DanhSachTau[2], new Point(0, 4), HuongTau.Ngang); // size 3
                banCo.DatTau(banCo.DanhSachTau[3], new Point(0, 6), HuongTau.Ngang); // size 3
                banCo.DatTau(banCo.DanhSachTau[4], new Point(0, 8), HuongTau.Ngang); // size 2

                foreach (var tau in banCo.DanhSachTau)
                {
                    foreach (var p in tau.DanhSachToaDo.ToList())
                    {
                        banCo.TiepNhanPhatBan(p);
                    }
                }

                Assert(banCo.TatCaTauDaChim && banCo.TongSoOBiBan == 17,
                    "TC-27: Tiêu diệt toàn bộ 17 ô tàu kích hoạt điều kiện thắng cuộc (TatCaTauDaChim == true)");
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
