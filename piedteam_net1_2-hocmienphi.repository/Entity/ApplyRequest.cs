using piedteam_net1_2_hocmienphi.repository.Abstraction;
using piedteam_net1_2_hocmienphi.repository.Enums;

namespace piedteam_net1_2_hocmienphi.repository.Entity;

public class ApplyRequest : BaseEntity<Guid>
{
    public string Description { get; set; }
    public string CvLink { get; set; }
    // họ sẽ đưa cho mình file CV, sau đó mình sẽ upload lên cloud
    // pending, approved, reject
    public ApplyRequestStatus Status { get; set; } = ApplyRequestStatus.Pending;
    public string? RejectReason { get; set; } // li do bi tu choi
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public ICollection<ApplyRequestCategory> ApplyRequestsCategories { get; set; }
}