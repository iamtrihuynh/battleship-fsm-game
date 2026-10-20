using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
            NapDuLieuNoiDung();
        }

        private void NapDuLieuNoiDung()
        {
            rtbRules.Text =
@"QUY TẮC CHIẾN ĐẤU BẮN TÀU (BATTLESHIP RULES):

1. BÀN CỜ VÀ HẠM ĐỘI:
- Mỗi bên chỉ huy một vùng biển quy chuẩn 10x10 ô tọa độ (A-J, 1-10).
- Sở hữu 5 chiến hạm có chiều dài từ 2 đến 5 ô (Tổng cộng 17 ô tàu).
- Các chiến hạm được bố trí bí mật, không được chồng lấn nhau và không được vượt ra ngoài biên bàn cờ.

2. LƯỢT KHAI HỎA & KẾT QUẢ:
- Hai bên luân phiên nhau chọn tọa độ trên radar đối phương để khai hỏa.
- Trượt (Miss): Bắn vào ô nước trống. Mất lượt, quyền bắn chuyển sang đối thủ.
- Trúng (Hit): Bắn trúng một phần thân tàu địch. ĐƯỢC THƯỞNG BẮN TIẾP MỘT LƯỢT NỮA!
- Chìm (Sunk): Bắn trúng ô cuối cùng của một con tàu, chiến hạm đó bị tiêu diệt hoàn toàn. ĐƯỢC BẮN TIẾP!

3. THỜI GIAN & ĐIỀU KIỆN THẮNG:
- Mỗi lượt có tối đa 30 giây để ngắm bắn. Hết 30 giây sẽ tự động mất lượt.
- Bên nào bắn chìm toàn bộ 5 chiến hạm (17 ô) của đối phương trước sẽ giành chiến thắng chung cuộc!";

            rtbShips.Text =
@"THÔNG SỐ 5 LOẠI CHIẾN HẠM HẢI QUÂN:

1. TÀU SÂN BAY (Aircraft Carrier):
   - Kích thước: 5 ô liên tiếp.
   - Vai trò: Pháo đài nổi chủ lực của hạm đội. Chiếm diện tích lớn nhất, dễ bị radar phát hiện nhất.

2. THIẾT GIÁP HẠM (Battleship):
   - Kích thước: 4 ô liên tiếp.
   - Vai trò: Tàu bọc thép hỏa lực mạnh, là mục tiêu săn đuổi hàng đầu.

3. TÀU NGẦM (Submarine):
   - Kích thước: 3 ô liên tiếp.
   - Vai trò: Sát thủ ngầm dưới lòng đại dương, di chuyển lẩn khuất.

4. TÀU KHU TRỤC (Destroyer):
   - Kích thước: 3 ô liên tiếp.
   - Vai trò: Cơ động cao, bảo vệ sườn hạm đội.

5. TÀU TUẦN TRA (Patrol Boat):
   - Kích thước: 2 ô liên tiếp.
   - Vai trò: Tàu trinh sát nhỏ nhất, khó phát hiện nhất trên bàn cờ 100 ô.";

            rtbTactics.Text =
@"MẸO CHIẾN THUẬT TỪ CÁC ĐÔ ĐỐC:

1. CHIẾN THUẬT BẮN SO LE CHẴN LẺ (PARITY HUNTING):
   - Con tàu nhỏ nhất có kích thước là 2 ô. Do đó, bất kỳ con tàu nào cũng bắt buộc phải đè lên ít nhất một ô có tính chất (X + Y) % 2 == 0.
   - Hãy bắn theo các ô như bàn cờ vua để tiết kiệm 50% số phát bắn và nhanh chóng tìm ra tàu địch!

2. CHIẾN THUẬT BÁM ĐUỔI THẲNG HÀNG (TARGET MODE):
   - Ngay khi bắn trúng 1 ô, hãy bắn 4 hướng xung quanh (Trên, Dưới, Trái, Phải).
   - Khi trúng phát thứ 2, bạn đã xác định được tàu nằm ngang hay nằm dọc. Hãy tập trung bắn dọc theo trục đó cho đến khi nghe tiếng còi báo tàu chìm!

3. TẬN DỤNG KỸ NĂNG ĐẶC BIỆT:
   - Dùng Radar trinh sát 3x3 khi bạn nghi ngờ một góc bàn cờ có tàu ẩn nấp.
   - Dùng Pháo kích diện rộng (+) khi cần dọn sạch các vùng biển rộng lớn.";

            rtbTeam.Text =
@"BẮN TÀU - NHÓM WINDOWN (BATTLESHIP TACTICAL WARFARE)

DANH SÁCH THÀNH VIÊN NHÓM:

1. Huỳnh Minh Trí - 51.01.104.105 (Thành viên 1 - Trưởng nhóm / Backend & AI Lead)
2. Trần Trọng Hưng - 51.01.104.132 (Thành viên 2 - Frontend & UI/UX Lead)
3. Trần Vũ Hòa Phát - 51.01.104.072 (Thành viên 3 - System I/O, Audio & QA/Tester)";
        }
    }
}
