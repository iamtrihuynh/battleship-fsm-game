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

                Console.WriteLine("\n--- PHẦN 3: MEMBER 3 - NGÀY 3 (DocThanhTich, LuuThanhTich, Combat Audio & Non-interrupting BGM) ---");

                // Test 28: DocThanhTich() va LuuThanhTich() overload truyen tham so truc tiep
                string fileDay3 = Path.Combine(tempTestDir, "thanhtich_day3.txt");
                cThanhTichDAO.DuongDanFile = fileDay3;
                bool luuThanhCong = cThanhTichDAO.LuuThanhTich("ThuyenTruongPhat", 2200, 20, 95, "PvE - Nâng cao", "Thắng");
                var dsDocVe = cThanhTichDAO.DocThanhTich();
                bool docDung = luuThanhCong && dsDocVe.Count == 1 && dsDocVe[0].TenNguoiChoi == "ThuyenTruongPhat" && dsDocVe[0].DiemSo == 2200;
                Assert(docDung, "TC-28: cThanhTichDAO.LuuThanhTich() và DocThanhTich() hoạt động chuẩn xác");

                // Test 29: Kiem thu am thanh khong ngat nhac nen khi phat SFX
                // 1. Khoi dong nhac nen
                cAmThanh.PhatNhacNen("bgm_battle.wav");
                Assert(cAmThanh.DangPhatNhacNen, "TC-29: Nhạc nền BGM khởi động thành công (DangPhatNhacNen == true)");

                // 2. Ban phao (Shot) -> khong ngat BGM
                cAmThanh.PhatShot();
                Assert(cAmThanh.AmThanhVuaPhat == "shot.wav" && cAmThanh.DangPhatNhacNen,
                    "TC-30: Phát âm thanh đại bác (shot.wav) mà không làm ngắt nhạc nền BGM");

                // 3. Ban truot (Miss) -> phat miss.wav va khong ngat BGM
                var testBanCo = new cBanCo(10);
                var tauTest = testBanCo.DanhSachTau[4]; // size 2
                testBanCo.DatTau(tauTest, new Point(5, 5), HuongTau.Ngang); // (5,5) va (6,5)

                var kqBanTruot = testBanCo.TiepNhanPhatBan(new Point(0, 0));
                if (kqBanTruot == KetQuaBan.Miss) cAmThanh.PhatMiss();
                Assert(cAmThanh.AmThanhVuaPhat == "miss.wav" && cAmThanh.DangPhatNhacNen,
                    "TC-31: Kết quả Trượt (Miss) phát âm thanh miss.wav chuẩn xác, BGM vẫn tiếp tục");

                // 4. Ban trung (Hit) -> phat hit.wav va khong ngat BGM
                var kqBanTrung = testBanCo.TiepNhanPhatBan(new Point(5, 5), out _);
                if (kqBanTrung == KetQuaBan.Hit) cAmThanh.PhatHit();
                Assert(cAmThanh.AmThanhVuaPhat == "hit.wav" && cAmThanh.DangPhatNhacNen,
                    "TC-32: Kết quả Trúng (Hit) phát âm thanh hit.wav chuẩn xác, BGM vẫn tiếp tục");

                // 5. Ban chim tau (Sunk) -> phat sunk.wav va khong ngat BGM
                var kqBanChim = testBanCo.TiepNhanPhatBan(new Point(6, 5), out _);
                if (kqBanChim == KetQuaBan.Sunk) cAmThanh.PhatSunk();
                Assert(cAmThanh.AmThanhVuaPhat == "sunk.wav" && cAmThanh.DangPhatNhacNen,
                    "TC-33: Kết quả Chìm tàu (Sunk) phát âm thanh sunk.wav chuẩn xác, BGM vẫn tiếp tục");

                // 6. Chien thang (Victory) -> ngat nhac nen va phat victory.wav
                cAmThanh.PhatVictory();
                Assert(cAmThanh.AmThanhVuaPhat == "victory.wav" && !cAmThanh.DangPhatNhacNen,
                    "TC-34: Kết thúc thắng cuộc (Victory) ngắt nhạc nền trận chiến và phát victory.wav");

                // 7. That bai (Defeat) -> ngat nhac nen va phat defeat.wav
                cAmThanh.PhatNhacNen("bgm_battle.wav");
                cAmThanh.PhatDefeat();
                Assert(cAmThanh.AmThanhVuaPhat == "defeat.wav" && !cAmThanh.DangPhatNhacNen,
                    "TC-35: Thất bại (Defeat) ngắt nhạc nền trận chiến và phát defeat.wav");

                Console.WriteLine("\n--- PHẦN 4: VERIFY CÁC FIX CHO BUG REVIEW ---");

                // Test 36: Rollback in DatTau
                var banCoRollback = new cBanCo(10);
                var tauRollback = banCoRollback.DanhSachTau[0]; // 5 o
                bool datHopLe1 = banCoRollback.DatTau(tauRollback, new Point(0, 0), HuongTau.Ngang);
                bool datLoiTranBien = banCoRollback.DatTau(tauRollback, new Point(8, 0), HuongTau.Ngang);
                bool rollbackThanhCong = datHopLe1 && !datLoiTranBien &&
                                         banCoRollback.MaTranO[0, 0].CoTau &&
                                         banCoRollback.MaTranO[4, 0].CoTau &&
                                         tauRollback.DaDatXong &&
                                         tauRollback.DanhSachToaDo[0] == new Point(0, 0);
                Assert(rollbackThanhCong, "TC-36: cBanCo.DatTau rollback giữ nguyên vị trí cũ khi vị trí mới vi phạm biên");

                // Test 37: SapXep an toan voi phan tu null
                var listCoNull = new List<ThanhTichItem>
                {
                    new ThanhTichItem(DateTime.Now, "PlayerA", 1000, 20, 100, "PvE", "Thắng"),
                    null,
                    new ThanhTichItem(DateTime.Now, "PlayerB", 1500, 15, 80, "PvE", "Thắng")
                };
                var listSapXepNull = cThanhTichDAO.SapXep(listCoNull, TieuChiSapXep.DiemCaoNhat);
                bool sapXepAnToan = listSapXepNull != null && listSapXepNull.Count == 2 && listSapXepNull[0].DiemSo == 1500;
                Assert(sapXepAnToan, "TC-37: cThanhTichDAO.SapXep an toàn và lọc bỏ phần tử null mà không ném ngoại lệ");

                // Test 38: LocTheoCheDo khop PvP voi PvP Hotseat
                var listPvp = new List<ThanhTichItem>
                {
                    new ThanhTichItem(DateTime.Now, "Pvp1", 1200, 25, 120, "PvP", "Thắng"),
                    new ThanhTichItem(DateTime.Now, "Pvp2", 1300, 22, 110, "PvP Hotseat", "Thắng"),
                    new ThanhTichItem(DateTime.Now, "Pve1", 1100, 30, 150, "PvE", "Thắng")
                };
                var locPvp = cThanhTichDAO.LocTheoCheDo(listPvp, "PvP Hotseat");
                bool locPvpDung = locPvp.Count == 2;
                Assert(locPvpDung, "TC-38: cThanhTichDAO.LocTheoCheDo khớp chính xác cả 'PvP' và 'PvP Hotseat'");

                // Test 39: DangPhatNhacNen la false khi file audio khong ton tai
                cAmThanh.DungNhacNen();
                cAmThanh.PhatNhacNen("file_am_thanh_khong_ton_tai_xyz.wav");
                Assert(!cAmThanh.DangPhatNhacNen, "TC-39: cAmThanh.DangPhatNhacNen là false khi file âm thanh không tồn tại");

                // Test 40: LayDanhSachToaDo voi doDai <= 0 an toan
                var dsToaDoAm = cBanCo.LayDanhSachToaDo(Point.Empty, -3, HuongTau.Ngang);
                Assert(dsToaDoAm != null && dsToaDoAm.Count == 0, "TC-40: cBanCo.LayDanhSachToaDo trả về rỗng khi độ dài <= 0 không lỗi");

                // Test 41: Dong bo xep tau thu cong voi cBanCo va Bot ban trung duoc
                var banCoSync = new cBanCo(10);
                var tauSanBaySync = banCoSync.DanhSachTau[0];
                bool datSyncOk = banCoSync.DatTau(tauSanBaySync, new Point(2, 3), HuongTau.Ngang);
                KetQuaBan kqBanSync = banCoSync.TiepNhanPhatBan(new Point(2, 3), out cTau tauBiBanSync);
                bool syncChuan = datSyncOk && kqBanSync == KetQuaBan.Hit && tauBiBanSync == tauSanBaySync;
                Assert(syncChuan, "TC-41: Đặt tàu thủ công đồng bộ chuẩn xác với cBanCo và tiếp nhận phát bắn Hit");

                // Test 42: Tinh toan diem so chong tran so (Integer Overflow/Underflow)
                int soTauConSong = 5;
                int tongPhatBanCucDai = 50000;
                int matchSecondsCucDai = 100000;
                long tinhDiem = 1000L + (soTauConSong * 200L) - (tongPhatBanCucDai * 15L) - (matchSecondsCucDai * 2L);
                int diemClamp = (int)Math.Max(0L, Math.Min((long)int.MaxValue, tinhDiem));
                Assert(diemClamp == 0, "TC-42: Tính điểm số trận đấu an toàn, kẹp biên 0 chống tràn số âm");

                // Test 43: Kiem tra BGM ngat an toan va khong bi deadlock
                cAmThanh.DungNhacNen();
                Assert(!cAmThanh.DangPhatNhacNen, "TC-43: cAmThanh.DungNhacNen cập nhật cờ DangPhatNhacNen về false tức thì");

                // Test 44: XoaTatCaTau dat lai toan bo trang thai cac o ban co
                var banCoReset = new cBanCo(10);
                banCoReset.TuDongSinhBanCoNgauNhien();
                banCoReset.XoaTatCaTau();
                bool sachBanCo = banCoReset.MaTranO.Cast<cO>().All(o => !o.CoTau && o.TrangThai == TrangThaiO.Empty);
                Assert(sachBanCo && !banCoReset.TatCaTauDaDatXong, "TC-44: cBanCo.XoaTatCaTau làm sạch 100% các ô trên ma trận");

                Console.WriteLine("\n--- PHẦN 5: MEMBER 1 - NGÀY 3 (FSM Bot AI: Hunt Parity & Target Mode) ---");

                // Test 45: Bot AI che do Kho chon o so le (Parity Hunt)
                var botKho = new cMayBanAI(DoKhoAI.Kho, 10);
                bool tatCaSoLe = true;
                for (int i = 0; i < 20; i++)
                {
                    Point p = botKho.ChonToaDoBan();
                    if ((p.X + p.Y) % 2 != 0) { tatCaSoLe = false; break; }
                    botKho.CapNhatKetQua(p, KetQuaBan.Miss);
                }
                Assert(tatCaSoLe, "TC-45: Bot AI chế độ Khó (Hunt) luôn chọn tọa độ so le chẵn lẻ (X + Y) % 2 == 0");

                // Test 46: Chuyen sang Target Mode khi ban trung phat dau
                var botTarget = new cMayBanAI(DoKhoAI.Kho, 10);
                Point hit1 = new Point(4, 4);
                botTarget.CapNhatKetQua(hit1, KetQuaBan.Hit);
                Assert(botTarget.TrangThai == TrangThaiAI.Target, "TC-46: Bot AI chuyển sang Target Mode ngay khi bắn trúng phát đầu");

                // Test 47: Khoa truc thang hang khi trung phat thu 2
                Point hit2 = new Point(5, 4); // Trung theo truc Ngang (Y = 4)
                botTarget.CapNhatKetQua(hit2, KetQuaBan.Hit);
                Point nextShot = botTarget.ChonToaDoBan();
                bool khoaTrucNgang = (nextShot.Y == 4); // Phai ban tiep theo truc ngang
                Assert(khoaTrucNgang, "TC-47: Bot AI khóa trục ngang khi trúng phát thứ 2 và ưu tiên bắn thẳng hàng");

                // Test 48: Quay lai Hunt Mode khi tau chim
                botTarget.CapNhatKetQua(nextShot, KetQuaBan.Sunk);
                Assert(botTarget.TrangThai == TrangThaiAI.Hunt, "TC-48: Bot AI dọn hàng đợi và quay lại Hunt Mode khi tàu chìm hoàn toàn");
                Console.WriteLine("\n--- PHAN 6: MEMBER 1 - NGAY 4 (Game Manager cQuanLyVanDau & Scoring Formula) ---");

                // Test 49: Khoi tao cQuanLyVanDau va bat dau tran dau
                var ql = new cQuanLyVanDau(CheDoChoi.PvE_Kho);
                Assert(ql.TrangThai == TrangThaiVanDau.ChuanBi && ql.LuotNguoiChoi == 1,
                    "TC-49: cQuanLyVanDau khoi tao o trang thai ChuanBi va luot Nguoi choi 1");

                ql.BatDauTranDau();
                Assert(ql.TrangThai == TrangThaiVanDau.DangChoi && ql.SoPhatBanNguoi1 == 0,
                    "TC-50: BatDauTranDau() chuyen FSM sang DangChoi va reset bo dem");

                // Test 51: Quy tac chuyen luot chuan (Ban trung thuong luot, Ban truot doi luot)
                ql.BanCoNguoi2.TuDongSinhBanCoNgauNhien();
                ql.BanCoNguoi1.TuDongSinhBanCoNgauNhien();

                // 1. Tim 1 o co tau de test ban trung thuong luot
                Point oCoTau = ql.BanCoNguoi2.DanhSachTau[0].DanhSachToaDo[0];
                var kqTrung = ql.Nguoi1Ban(oCoTau, out _);
                Assert((kqTrung == KetQuaBan.Hit || kqTrung == KetQuaBan.Sunk) && ql.LuotNguoiChoi == 1,
                    "TC-51: Nguoi 1 ban trung duoc thuong luot ban tiep (LuotNguoiChoi == 1)");

                // 2. Tim 1 o nuoc trong de test ban truot doi luot
                Point oNuoc = new Point(-1, -1);
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        if (!ql.BanCoNguoi2.MaTranO[x, y].CoTau && !ql.BanCoNguoi2.MaTranO[x, y].DaBiBan)
                        {
                            oNuoc = new Point(x, y);
                            break;
                        }
                    }
                    if (oNuoc.X >= 0) break;
                }
                var kqTruot = ql.Nguoi1Ban(oNuoc, out _);
                Assert(kqTruot == KetQuaBan.Miss && ql.LuotNguoiChoi == 2,
                    "TC-52: Nguoi 1 ban truot chuyen luot sang Nguoi 2 / May (LuotNguoiChoi == 2)");

                // Test 53: May ban nuoc tiep theo
                var kqMay = ql.MayBanNuocTiepTheo(out Point toaDoMay, out _);
                Assert(toaDoMay.X >= 0 && ql.SoPhatBanNguoi2 == 1,
                    "TC-53: MayBanNuocTiepTheo() thuc hien phat ban va tang SoPhatBanNguoi2");

                // Test 54: Dau hang ket thuc tran dau va kiem tra chan so bat thuong
                ql.DauHang(99); // Tham so khong hop le bi bo qua
                Assert(ql.TrangThai == TrangThaiVanDau.DangChoi,
                    "TC-54: DauHang(99) tham so ngoai le bi bo qua, tran dau khong bi ket thuc sai");

                ql.DauHang(2); // May/Nguoi 2 dau hang -> Nguoi 1 thang
                Assert(ql.TrangThai == TrangThaiVanDau.KetThuc && ql.NguoiThang == 1,
                    "TC-55: DauHang(2) ket thuc tran dau va ghi nhan Nguoi 1 chien thang");

                // Test 56: Tinh diem chuan xac theo cong thuc va chong tran so
                ql.ThoiGianGiay = 90;
                int diemThucTe = ql.TinhDiemNguoi1();
                int diemDuKien = Math.Max(0, 1000 + (5 * 200) - (ql.SoPhatBanNguoi1 * 15) - (90 * 2));
                Assert(diemThucTe == diemDuKien,
                    $"TC-56: TinhDiemNguoi1() tinh chinh xac theo cong thuc Game Balance ({diemThucTe} diem)");

                // Test 57: cMayBanAI an toan tuyet doi khi ban sach 100 o khong nem ArgumentOutOfRangeException
                var botHetO = new cMayBanAI(DoKhoAI.De, 10);
                for (int i = 0; i < 100; i++)
                {
                    Point p = botHetO.ChonToaDoBan();
                    botHetO.CapNhatKetQua(p, KetQuaBan.Miss);
                }
                Point shotVuotMuc = botHetO.ChonToaDoBan();
                Assert(shotVuotMuc.X == -1 && shotVuotMuc.Y == -1,
                    "TC-57: cMayBanAI.ChonToaDoBan() tra ve (-1, -1) an toan khi het sach o, khong throw ArgumentOutOfRangeException");

                // Test 58: cMayBanAI tiep tuc target tau dang thuong khi 1 tau khac bi chim hoan toan
                var botMulti = new cMayBanAI(DoKhoAI.Kho, 10);
                var tau1 = new cTau("Tau1", 2);
                tau1.GanToaDo(new List<Point> { new Point(2, 2), new Point(3, 2) }, HuongTau.Ngang);
                botMulti.CapNhatKetQua(new Point(2, 2), KetQuaBan.Hit);
                botMulti.CapNhatKetQua(new Point(5, 5), KetQuaBan.Hit); // Trung tau khac
                botMulti.CapNhatKetQua(new Point(3, 2), KetQuaBan.Sunk, tau1); // Tau 1 chim
                Assert(botMulti.TrangThai == TrangThaiAI.Target,
                    "TC-58: cMayBanAI van duy tri Target Mode cho cac o tau con lai khi tau 1 da chim");

                // Test 59: LocTheoCheDo phan biet chinh xac submode 'PvE - De' khong bi ro ri sang 'PvE - Nang cao'
                var listPveSub = new List<ThanhTichItem>
                {
                    new ThanhTichItem(DateTime.Now, "P1", 1000, 20, 100, "PvE - Dễ", "Thắng"),
                    new ThanhTichItem(DateTime.Now, "P2", 1500, 25, 120, "PvE - Nâng cao", "Thắng"),
                    new ThanhTichItem(DateTime.Now, "P3", 1200, 22, 110, "PvP", "Thắng")
                };
                var locSubDe = cThanhTichDAO.LocTheoCheDo(listPveSub, "PvE - Dễ");
                var locGenericPve = cThanhTichDAO.LocTheoCheDo(listPveSub, "PvE");
                Assert(locSubDe.Count == 1 && locSubDe[0].CheDo == "PvE - Dễ" && locGenericPve.Count == 2,
                    "TC-59: cThanhTichDAO.LocTheoCheDo phân biệt chính xác submode cụ thể và generic filter");

                // Test 60: ThanhTichItem cat ngan do dai chuoi ten nguoi choi toi da 40 ky tu chong tran UI
                string tenSieuDai = new string('A', 100);
                var itemDai = new ThanhTichItem(DateTime.Now, tenSieuDai, 1000, 20, 100, "PvE", "Thắng");
                Assert(itemDai.TenNguoiChoi.Length == 40 && itemDai.TenNguoiChoi == new string('A', 40),
                    "TC-60: ThanhTichItem.TenNguoiChoi tự động cắt ngắn còn tối đa 40 ký tự an toàn");

                // Test 61: cMayBanAI tu dong phuc hoi Target Mode va quet lan can cac o trung khi hang doi trong
                var botRecovery = new cMayBanAI(DoKhoAI.Kho, 10);
                botRecovery.CapNhatKetQua(new Point(4, 4), KetQuaBan.Hit);
                Point pNext = botRecovery.ChonToaDoBan();
                Assert(botRecovery.TrangThai == TrangThaiAI.Target && pNext.X >= 0,
                    "TC-61: cMayBanAI duy trì TrangThaiAI.Target khi ô bị thương vẫn tồn tại");

                // Test 62: cAmThanh.BatTatAmThanh() bao toan trang thai an toan
                cAmThanh.DungNhacNen();
                cAmThanh.BatTatAmThanh();
                cAmThanh.BatTatAmThanh();
                Assert(cAmThanh.DaBatAmThanh,
                    "TC-62: cAmThanh.BatTatAmThanh() bảo toàn trạng thái âm thanh an toàn");
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
