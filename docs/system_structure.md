**CẤU TRÚC THƯ MỤC CỦA DỰ ÁN**

BattleshipGame/                         \<--- Thư mục gốc Repo GitHub  
├── docs/                               \<--- Tài liệu báo cáo, sơ đồ UML  
│   ├── KE\_HOACH\_THIET\_KE.md  
│   └── BaoCao\_DoAn.docx  
│  
├── src/                                		\<--- Thư mục chứa toàn bộ mã nguồn  
│   ├── Battleship.sln                  \<--- File Solution mở bằng Visual Studio  
│   └── BattleshipGame/                 \<--- Project Windows Forms App chính  
│       │  
│       ├── Properties/                 \<--- Cấu hình mặc định của Visual Studio  
│       │  
│       ├── Assets/                     \<--- Tài nguyên media của game  
│       │   ├── Audio/                  \<--- Âm thanh (.wav): shot, hit, miss, sunk, bgm  
│       │   └── Images/                 \<--- Icon, logo, avatar tàu chiến (.png, .ico)  
│       │  
│       ├── Models/                     \<--- TẦNG DỮ LIỆU (Data Entities & Enums)  
│       │   ├── Enums.cs                \<--- Các enum: TrangThaiO, HuongTau, CheDoChoi  
│       │   ├── cO.cs                   \<--- Thực thể 1 ô tọa độ (x, y, đã bắn chưa, có tàu không)  
│       │   ├── cTau.cs                 \<--- Thực thể con tàu (Tên, độ dài, danh sách tọa độ)  
│       │   └── ThanhTichItem.cs        \<--- Đối tượng lưu 1 dòng thành tích lịch sử  
│       │  
│       ├── Logic/                      \<--- TẦNG NGHIỆP VỤ (Business Logic & AI)  
│       │   ├── cBanCo.cs               \<--- Xử lý ma trận 10x10, thuật toán kiểm tra va chạm  
│       │   ├── cMayBanAI.cs            \<--- Thuật toán Bot AI (Hunt Mode & Target Mode)  
│       │   ├── cQuanLyVanDau.cs        \<--- Điều phối lượt chơi, tính điểm, thắng/thua  
│       │   └── cAmThanh.cs             \<--- Quản lý phát nhạc nền và hiệu ứng âm thanh  
│       │  
│       ├── DataAccess/                 \<--- TẦNG LƯU TRỮ (Data Access Layer \- DAO)  
│       │   └── cThanhTichDAO.cs        \<--- Đọc/ghi dữ liệu file thanhtichs.txt  
│       │  
│       ├── Views/                      \<--- TẦNG GIAO DIỆN (UI Forms)  
│       │   ├── frmMenu.cs              \<--- Màn hình chính (Chọn chế độ, Bắt đầu)  
│       │   ├── frmBanTau.cs            \<--- Màn hình bàn cờ chiến đấu chính  
│       │   ├── frmThanhTich.cs         \<--- Màn hình bảng xếp hạng (DataGridView)  
│       │   └── frmAbout.cs             \<--- Màn hình giới thiệu thành viên & Luật chơi  
│       │  
│       ├── Program.cs                  \<--- Điểm bắt đầu chạy ứng dụng (Main Entry)  
│       └── App.config  
│  
├── .gitignore                          \<--- File cấu hình bỏ qua file rác (Visual Studio template)  
└── README.md                           \<--- File trang chủ giới thiệu dự án trên GitHub

### **Giải thích nhanh các thư mục chính (Vì sao lại rất dễ dùng?):**

1. **Models/ (Dữ liệu thuần túy):**

   * Chỉ chứa các biến, thuộc tính (Properties). Rất nhẹ, code xong ngay trong ngày đầu tiên.  
2. **Logic/ (Trái tim của Game):**

   * Tách riêng toàn bộ thuật toán ra khỏi giao diện. Sau này sửa AI hay sửa quy tắc xếp tàu chỉ cần vào đây sửa, không sợ làm hỏng giao diện Form.  
3. **Views/ (Giao diện Forms):**

   * Gom tất cả các Form vào một nơi. Người làm giao diện chỉ cần làm việc trong thư mục này, không bị rối mắt với các file thuật toán.  
4. **Assets/ (Âm thanh & Hình ảnh):**

   * Khi thêm file âm thanh .wav vào thư mục Assets/Audio/, trong Visual Studio chỉ cần click chuột phải vào file →\\rightarrow→ Chọn **Properties** →\\rightarrow→ đặt **Copy to Output Directory \= Copy if newer** là khi build game sẽ tự động nhận diện file.  
5. **DataAccess/ (Lưu file text):**

   * Chứa duy nhất file quản lý việc đọc/ghi thanhtichs.txt, bọc try...catch cẩn thận để chống lỗi mất file.

