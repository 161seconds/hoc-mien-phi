using Microsoft.AspNetCore.Http;

namespace piedteam_net1_2_hocmienphi.service.UserService;

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

    public class UpdateUserRequest : CreateUserRequest
    {
        public IFormFile? Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class CreateCategoryRequest
    {
        public string CategoryName { get; set; }
        public Guid? ParentId { get; set; } 
    }
}