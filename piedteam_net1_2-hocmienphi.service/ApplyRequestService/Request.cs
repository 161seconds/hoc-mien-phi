namespace piedteam_net1_2_hocmienphi.service.ApplyRequestService;

public class Request
{
    public class CreateApplyRequestRequest
    {
        public string Description { get; set; }
        public string CvLink { get; set; }
        public Guid UserId { get; set; }
        public List<Guid> CategoryIds { get; set; }
        // khi ng duùng tạo đơn đki trở thành mentor
            // thì họ cũng phải đề cập tới
            // những lĩnh vực mà họ có thể mentor
    }
    
    public class ReviewApplyRequestRequest
    {
        public bool IsApproved { get; set; }
        public string? Reason { get; set; }
    }
}