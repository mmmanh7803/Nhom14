using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace QLcaulacbosinhvien.Models{
    public class EventMember
{
    public int EventAccountID { get; set; }
    public int EventID { get; set; } // Khóa ngoại đến bảng Event
    public Event Event { get; set; }
    public int MemberID { get; set; }
    public Member Member {get; set;}
    public int AccountID { get; set; } // Khóa ngoại đến bảng Member
    public Account Account { get; set; }
    public DateTime? CheckInTime { get; set; }

    public bool IsAttended { get; set; } // Trạng thái điểm danh (true nếu thành viên tham gia)
}

}
