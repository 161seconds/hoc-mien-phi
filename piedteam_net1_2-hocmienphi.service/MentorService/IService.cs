namespace piedteam_net1_2_hocmienphi.service.MentorService;

public interface IService
{
    public Task<List<Response.GetMentorResponse>> GetAllMentor(string? searchTerm = null, int pageIndex = 1, int pageSize = 10);
}
