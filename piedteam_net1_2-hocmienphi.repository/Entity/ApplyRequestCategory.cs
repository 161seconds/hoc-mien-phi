using piedteam_net1_2_hocmienphi.repository.Abstraction;

namespace piedteam_net1_2_hocmienphi.repository.Entity;

public class ApplyRequestCategory : BaseEntity<Guid>
{
    public Guid ApplyRequestId { get; set; }
    public ApplyRequest ApplyRequest { get; set; }
    
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
}