using piedteam_net1_2_hocmienphi.repository.Abstraction;

namespace piedteam_net1_2_hocmienphi.repository.Entity;

public class Mentor : BaseEntity<Guid>
{
    public string? OrganizationName { get; set; }
    public string? CurrentPosition { get; set; }
    
    // tại sao ở đây nên là nullable, ma kh phải là required
    // tại vì khi user trở thành mentor, sau khi họ được approved
    // thì lúc này ta tạo mới Mentor
    // nếu kh đổi thành nullable thì 2 field đó ở đâu ra bây g
    // flow đúng phải là khi mà họ login lần đầu tiên sau khi được approve
    // chúng ta sẽ yêu cầu họ bắt buộc phải điền thông tin bổ sung

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public ICollection<MentorCategory> MentorCategories { get; set; } = new List<MentorCategory>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<MentorFreeTime> MentorFreeTimes { get; set; } = new List<MentorFreeTime>();
}