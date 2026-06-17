using piedteam_net1_2_hocmienphi.repository.Abstraction;

namespace piedteam_net1_2_hocmienphi.repository.Entity;

public class Mentor : BaseEntity<Guid>
{
    public string OrganizationName { get; set; }
    public string CurrentPosition { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public ICollection<MentorCategory> MentorCategories { get; set; } = new List<MentorCategory>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<MentorFreeTime> MentorFreeTimes { get; set; } = new List<MentorFreeTime>();
}