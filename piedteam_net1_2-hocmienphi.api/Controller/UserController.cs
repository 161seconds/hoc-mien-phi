using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.Entity;
using piedteam_net1_2_hocmienphi.service.UserService;
using piedteam_net1_2_hocmienphi.service.Utils.JWTService;
using Request = piedteam_net1_2_hocmienphi.service.UserService.Request.Request;

namespace PiedTeam_NET1_2_hocmienphi.api.Controller;

[ApiController]
[Route("api/[controller]")]
//note | annotation
public class UserController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly JwtOptions _jwtOptions = new();
    
    public UserController(AppDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        configuration.GetSection("JwtOptions").Bind(_jwtOptions);
    }
    
    /*
     //endpoint gọi tên khác là các api
    // POST /api/auth/login
    // POST /api/auth/register
    // GET /api/users/{id}
    // PUT /api/users/{id}
    // DELETE /api/users/{id}
    
    // nhưng endpoint này làm ntn để xuất hiện -> khai báo trong controller
    
    //có mấy phuong thuc cua 1 request
    // get post put patch delete
    // get: lay dlieu
    // post: tao moi dlieu
    // put | patch: cap nhat dlieu
    // delete: xoa dlieu
    
    //1. tại sao lại sinh ra những method này
    // vì HTTP cần 1 cách chuẩn để nói rõ với client đang muon làm gi voi resource
    // url = đang thao tac voi cgi
    // http method = muon lam gi voi nó
    // vì sao kh chỉ dùng 1 method thôi?
    // -> rest sinh ra là de viết gọn hơn, chỉ cần nhìn vào method là hieu 
    // method giúp tách y nghia hoạt động
    //2. vd 1 method mà mình choi het cho tát cả dc kh. VD: 1 POST chơi hết, từ lấy tạo update xóa 
    
    // GET khac POST, PUT, PUT, DELETE ntn
    // GET: thường sẽ kh có request body, dlieu sẽ dc truyền qua query hoac route
    
    // theo ae có mấy cách để FE truyền dlieu sang cho BE.
        // biết là sẽ call API, nhma những cái data mà FE gửi thì nó sẽ nằm ở đâu trong request
        
    // truyền qua 3 cách thông thường:
        // query params: /api/users?name=bao&age=18
            //là những params dc nằm trên url và sao dấu ?
            // như url ở trên thì chúng ta có 2 query params là name và age
        // route: /api/users/{id} | /api/users/1234
            //là những params dc nằm trên url và sao dấu /
            // như url ở trên thì chúng ta có 1 route params là id = 1234
        // body: thường sẽ dùng cho put post patch delete
            
    // vì GET thường sẽ kh có body, nên chúng ta hạn chế sử dụng cho các API cần bảo mật
    // GET để login: GET /api/auth/login?email=bao&password=123
    // thông thường khi login ta thường sai method là POST /api/auth/login, data sẽ được dấu ở trong body
    
    // request là gì, 1 yêu cầu xuống server, mong muốn sv làm gì đó và trả ra kq
    // 1 HTTP request login, mong muốn sv xac thực thông tin đăng nhập, và trả ra kq là 1 token hoặc lỗi
    // trong 1 http request thì se có những thành phần sau:
        // url: địa chỉ api mà chúng ta muốn gọi
        // method: get post put patch delete
        // header: chứa các thông tin dữ về request, như là content-type, author, ... 
        // body: chứa dlieu mà chúng ta muốn gửi lên sv, 
        
    // restfulAPI: nó là 1 tiêu chuẩn dùng để định nghĩa thiết kế API
    // dựa trên các phương thức HTTP và các quy tắc về url,
    // để tạo ra các API dễ hiểu, dễ sử dụng và dễ bảo trì
    
    // GetAll: GET /api/user -> theo chuẩn thì phải đặt các tham số vô để truy vấn
        // GetAllUser: GET /api/user/getall
        // GetAllStaf: GET /api/user/getallstaff
        // GetAllStudent: GET /api/user/getallstudent
    // GetById: GET /api/user/{id}
    // Create: POST /api/user
        // CreateUser: POST /api/user/create-user
    // Update: PUT /api/user/{id}
    // Delete: DELETE /api/user/{id}
    //query params: pageIndex, pageSize sẽ thay đổi khi FE truyền vào
    
    // GET: /api/user/{id}
    //[HttpGet("{id}")] // route params: id sẽ thay đổi khi FE truyền vào
    // public IActionResult GetUserById(Guid id)
    // {
    //     return Ok($"Get user by id: {id}");
    // }

    // POST: /api/user
    // body: dlieu đc truyền vào body, nên kh cần phải đặt tham số trong url
     */
    
    // PUT: /api/user/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateUser(Guid id, Request.UpdateUserRequest request)
    {
        var query = _dbContext.Users.FirstOrDefault(x => x.IsDeleted == false && x.Id.Equals(id));
        if (query == null)
        {
            return NotFound();
        }
        query.FirstName = request.FirstName;
        query.LastName = request.LastName;
        _dbContext.SaveChanges();
        return Ok($"Update user id: {id}");
    }

    /*
     // DELETE: /api/user/{id}
    // [HttpDelete("{id}")]
    // public IActionResult DeleteUser(Guid id)
    // {
    //     return Ok($"Delete user id: {id}");
    // }
     */

    // POST: /api/user/login
    [HttpPost("login")]
    public IActionResult Login(string Email, string Password)
    {
        /*
        // lấy tất cả User trong db
        // theo ae tại sao phải login
        // giới hạn quyền hạn đc gọi đến các resrc
        // ví dụ: bạn phải là 1 user (đã đki hệ thống) thì bạn mới dc mua hàng
        
        // authentication và authorization
        // authen: bạn có dc quyền vào hệ thống của tôi kh
        // author: sau khi vào hệ thống của tôi rồi thì bạn có quyền gì
            // ví dụ: admin thì có quyền tạo
            // mentor thì có quyền tạo lịch rảnh
        // vậy thì thông thường, chúng ta thường dùng kĩ thuật gì để xác thực và phân quyền
        // thông thường mình hay sử dụng JWT để xác thực và phân quyền
        
        // JWT: Json Web Token: là 1 chuỗi token được mã hóa, truyền giữa client (FE) và server (BE)
            // để xác thực và phân quyền cho người dùng
        
        // thông thường JWT có 3 phần
        // header: chứa thông tin thuật toán mã hóa và loại token
        // payload: chứa thông tin và quyền hạn của người dùng
        // signature: chứa chữ ký số để xác thực token (sign(header + payload, secret)) 
         */
        /*
         // đầu tiên tìm kiếm tài khoản với email đó xem có tồn tại hay kh
        // nếu mà có thì mình mới tính tiếp đc
            // tiếp tục so sánh với password người dùng nhập vào với password có trong db
                // nếu mà trùng, ok bạn chính là chủ nhân của tài khoản, tôi sẽ trả ra JWT token cho bạn
                // nếu mà kh trùng, thì m kh phải chủ nhân của tài khoản, cútttt
        // nếu mà kh có tồn tại email thì cút
         */
        var query = _dbContext.Users.Where(x => x.IsDeleted == false);
        query = query.Where(x => x.Email == Email);
        var user = query.FirstOrDefault();
        if (user == null || user.Password != Password)
        {
            return BadRequest();
        }
        var testValue = _jwtOptions.SecretKey;
        // claims đại diện cho các thông tin nằm trong payload của jwt
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            // quan trọng nhất: claim này sẽ giúp mình new Claim(ClaimTypes.Role, user.Role)
            // sẽ giúp mình phân quyền
            new Claim("UserId", user.Id.ToString()),
            new Claim("Role", user.Role),
        };
        var token = JwtService.GenerateToken(claims, _jwtOptions);
        return Ok(token);
    }
    
    [HttpPost("ForgotPassword")]
    public IActionResult ForgotPassword()
    {
        return Ok("ForgotPassword");
    }
    
    /*
    // khai báo cho anh API sau
    // GET all category
        // có phân trang và cho phép search
    // GET category by id
    // Create category
        // yêu cầu có body là name và parentID
    // update category
    // delete category
    
    //==========
    // lam bai tap
    //bai tap ve nha
        // tao. moi user
        // GetAllUser theo phan trang
        // Search, OrderBy
        // GetUserById
     */
    
    [HttpPost]
    public IActionResult CreateNewUser(Request.CreateUserRequest request)
    {
        var createUser = new User()
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Age = request.Age,
            Email = request.Email,
            Password = request.Password,
            Role = "User",
            IsDeleted = false
        };
        _dbContext.Users.Add(createUser);
        _dbContext.SaveChanges();
        return Ok();
    }

    [HttpGet]
    public IActionResult GetAllUser(string? searchTerm, int pageIndex = 1, int pageSize = 10)
    {
        var query = _dbContext.Users.AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x => x.FirstName.Contains(searchTerm) || 
                                     x.LastName.Contains(searchTerm) || 
                                     x.Email.Contains(searchTerm));
        }
        var selectedUser = query            
            .OrderBy(x => x.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new Response.GetUserResponse()
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
            }).ToList();
        return Ok(selectedUser);
    }

    [HttpGet("{id}")]
    public IActionResult GetUserById(Guid id)
    {
        var query = _dbContext.Users.FirstOrDefault(x => x.Id == id);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(Guid id)
    {
        var query = _dbContext.Users.FirstOrDefault(x => x.Id == id);
        if (query == null)
        {
            return NotFound();
        }
        _dbContext.Users.Remove(query);
        return Ok($"Delete user id: {id}");
    }
}