namespace piedteam_net1_2_hocmienphi.service.UserService.Request;

public class Request
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}