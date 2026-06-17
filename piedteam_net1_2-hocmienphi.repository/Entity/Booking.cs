using piedteam_net1_2_hocmienphi.repository.Abstraction;

namespace piedteam_net1_2_hocmienphi.repository.Entity;

public class Booking : BaseEntity<Guid>
{
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    // chưa xu li chenh lech múi gio
    public string OrganizationName { get; set; }
    public string CurrentPosition { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid MentorId { get; set; }
    public Mentor Mentor { get; set; }
}