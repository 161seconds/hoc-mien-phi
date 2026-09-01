using piedteam_net1_2_hocmienphi.repository.Entity;

namespace piedteam_net1_2_hocmienphi.service.UserService;

public interface IService
{
    public Task<List<Response.GetUserResponse>> GetAllUsers(string? searchTerm, int pageIndex, int pageSize);
    public Task<string> CreateNewUser(Request.CreateUserRequest request);
    public Task<string> UpdateUserById(Guid id, Request.UpdateUserRequest request);
    public Task<string?> Login(string email, string password);
    public Task<User?> GetUserById(Guid id);
    public Task<bool> DeleteUser(Guid id);
}