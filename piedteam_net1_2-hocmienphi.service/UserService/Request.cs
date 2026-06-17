namespace piedteam_net1_2_hocmienphi.service.UserService.Request;

public class Request
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Age { get; set; }
        public string? Role { get; set; }
        public string? IsDeleted { get; set; }
    }

    public class GetChildrenCategoryById
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class GetAllUsers
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class GetUserByIdResponse
    {
        public Guid Id { get; set; }
    }
}