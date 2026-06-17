using piedteam_net1_2_hocmienphi.repository.Abstraction;

namespace piedteam_net1_2_hocmienphi.repository.Entity;

public class Booking : BaseEntity<Guid>
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string OrganizationName { get; set; }
    public string CurrentPosition { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid MentorId { get; set; }
    public Mentor Mentor { get; set; }
}