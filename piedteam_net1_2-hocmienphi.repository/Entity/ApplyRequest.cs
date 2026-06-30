using piedteam_net1_2_hocmienphi.repository.Abstraction;
using piedteam_net1_2_hocmienphi.repository.Enums;

namespace piedteam_net1_2_hocmienphi.repository.Entity;

public class ApplyRequest : BaseEntity<Guid>
{
    public string Description { get; set; }
    public string CvLink { get; set; }
    public ApplyRequestStatus Status { get; set; } = ApplyRequestStatus.Pending;
    public string? RejectReason { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ICollection<ApplyRequestCategory> ApplyRequestsCategories { get; set; }
}