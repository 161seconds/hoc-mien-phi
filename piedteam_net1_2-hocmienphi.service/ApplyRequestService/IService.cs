using piedteam_net1_2_hocmienphi.repository.Enums;

namespace piedteam_net1_2_hocmienphi.service.ApplyRequestService;

public interface IService
{
    public Task CreateApplyRequest(Request.CreateApplyRequestRequest requestBody);
    public Task<List<Response.GetApplyRequestResponse>> GetAllApplyRequest(string? searchTerm = null, ApplyRequestStatus? status = null, int pageIndex = 1, int pageSize = 10);
    public Task<List<Response.GetApplyRequestResponse>> GetMyApplyRequest(Guid userId, ApplyRequestStatus? status = null, int pageIndex = 1, int pageSize = 10);
    public Task<Response.GetApplyRequestResponse?> GetApplyRequestDetail(Guid applyRequestId);
    public Task<bool> ReviewApplyRequest(Guid id, Request.ReviewApplyRequestRequest requestBody);
}
