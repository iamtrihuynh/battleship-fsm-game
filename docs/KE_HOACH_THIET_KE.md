## ĐỀ TÀI: MINIGAME BẮN TÀU - NHÓM WINDOWN (BATTLESHIP TACTICAL WARFARE)

---

### 1. Phân Công Nhiệm Vụ 3 Thành Viên Trong Nhóm
* **Huỳnh Minh Trí - 51.01.104.105** (Thành viên 1 - Trưởng nhóm / Backend & AI Lead)
  * Module phụ trách: `cO.cs`, `cTau.cs`, `cBanCo.cs`, `cMayBanAI.cs`, `cQuanLyVanDau.cs`.
  * Nhiệm vụ Ngày 1: Tạo Repository GitHub, cấu hình `.gitignore`, lập trình các lớp thực thể (`cO.cs`, `cTau.cs`, các enum `TrangThaiO`, `HuongTau`), viết khung lớp `cBanCo.cs`.

* **Trần Trọng Hưng - 51.01.104.132** (Thành viên 2 - Frontend & UI/UX Lead)
  * Module phụ trách: `frmMenu.cs`, `frmBanTau.cs` (giao diện), `frmAbout.cs`, thiết kế Icon/Asset đồ họa.
  * Nhiệm vụ Ngày 1:
    1. Tạo cấu trúc Solution WinForms 3 tầng (`Models/`, `Logic/`, `DataAccess/`, `Views/`, `Assets/`).
    2. Thiết kế Form Menu chính (`frmMenu.cs`) với hỗ trợ toàn màn hình (Maximize responsive), hiệu ứng Radar xoay mượt mà, viền Neon phát sáng.
    3. Tạo giao diện tĩnh chuẩn kích thước cho Form Trận Đấu (`frmBanTau.cs`) hỗ trợ mở rộng toàn màn hình, định hình 2 bàn cờ 10x10, Dashboard thông số, kho tàu, kỹ năng và màn che sương mù.

* **Trần Vũ Hòa Phát - 51.01.104.072** (Thành viên 3 - System I/O, Audio & QA/Tester)
  * Module phụ trách: `cAmThanh.cs`, `cThanhTichDAO.cs`, `frmThanhTich.cs`, Báo cáo Word/Slide.
  * Nhiệm vụ Ngày 1: Chuẩn bị kho tài nguyên âm thanh (`.wav`), thiết kế cấu trúc lưu trữ `thanhtichs.txt` và viết lớp thực thể `ThanhTichItem.cs`.
