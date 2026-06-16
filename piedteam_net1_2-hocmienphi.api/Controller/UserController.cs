using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.Entity;
using piedteam_net1_2_hocmienphi.service.CategoryService;
using Request = piedteam_net1_2_hocmienphi.service.UserService.Request.Request;

namespace PiedTeam_NET1_2_hocmienphi.api.Controller;

[ApiController]
[Route("api/[controller]")]
//note | annotation
public class UserController : ControllerBase
{
    private readonly AppDbContext _dbContext;
        
    public UserController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
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
    
    // PUT: /api/user/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateUser(Guid id)
    {
        return Ok($"Update user id: {id}");
    }

    // DELETE: /api/user/{id}
    // [HttpDelete("{id}")]
    // public IActionResult DeleteUser(Guid id)
    // {
    //     return Ok($"Delete user id: {id}");
    // }

    // POST: /api/user/login
    [HttpPost("login")]
    public IActionResult Login()
    {
        return Ok("Login");
    }
    
    [HttpPost("ForgotPassword")]
    public IActionResult ForgotPassword()
    {
        return Ok("ForgotPassword");
    }
    
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
        query = query.Where(x => x.IsDeleted == false);
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var sanitizerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(x =>
                x.FirstName.ToLower().Contains(sanitizerSearchTerm) ||
                x.LastName.ToLower().Contains(sanitizerSearchTerm) ||
                x.Email.ToLower().Contains(sanitizerSearchTerm)
            );
        }
        query = query.OrderBy(x => x.Email);
        var selectedUser = query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new Request.GetAllUsers()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
            })
            .ToList();
        return Ok(selectedUser);
    }

    [HttpGet("{id}")]
    public IActionResult GetUserById(Guid id)
    {
        var query = _dbContext.Users.AsQueryable();
        query = query.Where(x => x.IsDeleted == false && x.Id.Equals(id));
        //query = query.OrderBy(x => x.Id); chỉ lấy ra user có id duy nhất nên kh cần sắp xếp 
        var selectedUser2 = query.Select(x => new Request.GetAllUsers()
        {
            Id = x.Id
        }).FirstOrDefault();
        
        if (selectedUser2 == null)
        {
            return NotFound();
        }
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(Guid id)
    {
        return Ok($"Delete user id: {id}");
    }
}