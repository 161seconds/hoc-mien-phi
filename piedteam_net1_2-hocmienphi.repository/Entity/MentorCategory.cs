using piedteam_net1_2_hocmienphi.repository.Abstraction;

namespace piedteam_net1_2_hocmienphi.repository.Entity;

public class MentorCategory : BaseEntity<Guid>
{
    public Guid MentorId { get; set; }
    public Mentor Mentor { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
}