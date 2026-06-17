using piedteam_net1_2_hocmienphi.repository.Abstraction;
using piedteam_net1_2_hocmienphi.repository.Enums;

namespace piedteam_net1_2_hocmienphi.repository.Entity;

public class MentorFreeTime : BaseEntity<Guid>
{
    public MentorFreeTimeDateOfWeek DateOfWeek { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    
    public Guid MentorId { get; set; }
    public Mentor Mentor { get; set; }
}