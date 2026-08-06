using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.Entity;
using piedteam_net1_2_hocmienphi.service.UserService;
using piedteam_net1_2_hocmienphi.service.Utils.JWTService;
using Request = piedteam_net1_2_hocmienphi.service.UserService.Request;

namespace PiedTeam_NET1_2_hocmienphi.api.Controller;

[ApiController]
[Route("api/[controller]")]
//note | annotation
public class UserController : ControllerBase
{
    private readonly IService _userService;

    public UserController(IService userService)
    {
        _userService = userService;
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
    
    [HttpGet("")]
    public async Task<List<Response.GetUserResponse>> GetAllUsers(string? searchTerm, int pageIndex, int pageSize)
    { 
        /*
        từ đó giờ, trong lập trình ở dự án hiện tại, mình chưa có đề cập gì đến
        việc lập trình bat đồng bộ - điều này có nghĩa lí gì?
        khi mà N requests gọi đến cùng 1 lúc thì nó xử lí như thế nào (chưa bàn tới)
        đó giờ mình chỉ lập trình đồng bộ thôi (Synchronous) - có nghĩa
        khi mà các request tới thì nó xu lí tuần tự
        tự đặt ra các câu hoi như sau
        - vậy thì lâp trình ất đồng bộ nghĩa là như thế nào
        - lập trình bat đồng bộ có phaải thực hiện N request cùng 1 lúc kh
        - thực hiện N request cùng 1 lúc, hình như thấy hoi giôống song song - parallel
        - vậy thì bất đồng bộ khác parallel như the nao
        
        giải thích đồng bộ và bt đồng bộ khác nhau như thế nào bằng ví dụ pha cà phê
        - đồng bộ: nhân vien nhận Order -> pha cf -> đưa khách -> rồi sau đó mới nhận khách tiếp theo
        - điểm đặc biet: trong lúc pha cf có hành động là đợi máy xay cf xong. thì lúc này nếu có khách tơới nhận order
        thawngf nhân viên bat dong bo sẽ nói rằng: m order gì, km may!!! t kh can biet, t phải đợi máy pha cf xay xong đã
        rồi t moi tính tip
        
        - bất dong bo: linh hoạt hơn, những hành động nào cần phải chờ đợi như là (đợi máy xay cf xay xong) thi chủ đông
        bỏ qua và nhận 1 hành động mới như là nhận Order mới. sau đó máy pha cf xay xong thì mình nhận kết quả
        và pha ly nước cho khách cũ
        
        thông thường mình cứ nghĩ, bất đồng bộ là người nhân viên xử li 2 order cùng 1 lúc, nhưng kh phải, 
        nó là 1 dạng làm việc thông minh
        
        giải thích bat dong bo và song song và ví dụ quản lí nhà hàng
        nhà hàng piedteam chi nhánh C#. giar sử nhaf hang của anh có 2 nhan vien la (Binh va Nam). nha hàng phuc vụ 2 mảng 
        la đồ ăn chay và đồ ăn mặn (cả 2 nhân viên deu lam dc hết)
        
        đối voi lai Parallel
        -> phân công: Bình chỉ dc làm đồ chay, Nam chỉ làm do mặn
        -> trường hợp ngon nhất: nhà hàng nhan dc các đơn hàng có khoi lượng cong viec của chay va man bang nhau
            nhan vien phuc vu hết công suất.
        -> trường hợp tệ nhất: ben đồ chay nổ 100 đơn, bên đồ mặn nổ 0 đơn. lúc này bên Bình thì lam viec xấp mặt,
        bên Nam thì chill, Bình bảo Nam qua phụ làm, Nam kh phu luon. vi sep Tan đã chia từ đầu r
        
        đối với lại bat dong bo: 
        -> cac ae làm việc hòa thuận voi nhau. ben chay nổ đơn nhieu hon, cả 2 ae góp công vao phụ
        -> nếu khi nhà hàng quá tải, luc nay chỉ can tuyển thêm nhan vien ma thoi
        
        implement vao code
        tương tu voi JS thi .NET cũng co lap trinh bat dong bo. ben JS mình có Promise thì 
        bên .NET cũn có Task. Promise = Task
        
        .NET cung co Async va Await
        -> Async thi danh gia method nay la 1 hanh dong bat dong bo
        -> Await: hay để phuong thức nay dc thực thi cho den khi hoan tat, luc nay minh tranh thu di lam cai khac
        khi ma minh await trong 1 cai ham thi minh phai khai bao cai ham do la async
        khi ma minh đánh dau 1 ham la async thi có nghĩa rang la cai ham nay sẽ hứa trả cho minh 1 kq
        hứa (promise = task). luc nay 1 cai ham async phai bat buoc tra ra task
        quy tac dinh nghia task nhu the nao:
        -> 1 cai ham thi dau ra (response) thong thuong co 2 gia tri:
            - void                              -> Task 
            - 1 list gi do ..., 1 kiểu gi do    -> Task<List<Student>> | Task<int> | Task<string>
            
        bat dong bo trong .NET thi có 2 cái ham dac biet nua la WhenAll và WhenAny
        thi WhenAll de lam cgi
        vi du: trong 1 cai logic no co 3 cai func deu la bat dong bo het
            - func 1 thi 3s
            - func 2 thi 2s
            - func 3 thi 4s
        doi voi logic binh thuong
        await func1
        await func2
        await func3
        -> tong thoi gian ham nay xu li se la 9s
        doi voi lai khi minh sai WhenAll
        -> no se lay 3 ket qua cung 1 luc dua. tren func tra ra ket qua lau nhat (func4)
        -> tong thoi gian se la 4s
        -> nhung ma neu co 1 task bi loi thi tat ca se dung lai luon
        var result = await Task.WhenAll(func1, func2, func3);
        
        doi voi lai khi minh sai WhenAny
        -> no se lay 1 ket qua tra ra ket qua nhanh nhat (func2)
        -> tong thoi gian se la 2s
        -> nhung ma neu co 1 task bi loi thi ham van se tiep tuc chay, dam bao thang nao tra ra nhanh nhat va kh bi loi
        var result = await Task.WhenAny(func1, func2, func3);
        
        vay thi cau hoi dat ra la. vay minh sai WhenAny di. tai no la nhanh nhat ma
        
        sai cach bth, khi nhung ketqua cua cac ham phu thuoc len nhau
            + vi du luong tao tai khoan
                -> truy van xuong db xem user co ton tai kh
                -> tao account va luu xuong db
                -> neu tao account thanh cong thi gui mail chuc mung
                 
        sai whenAll khi ketqua cua tung ham kh phu thuoc len nhau
            + vi du luong la tao mentor, FE dua cho minh 2 thu la UserId va CategoryId
                -> minh phai verify UserId va CategoryId co ton tai hay kh, neu kh ton tai thi bi loi ForeignKey
                -> thi o truong hop nay, 2 hanh dong kh phu thuoc lan nhau, nen sai whenAll la toi uu nhat
                
        sai whenAny khi minh muon kiem tra xem Service nao la nhanh nhat
            + thong thuong se dc sai trong Load Balancer, trong 1 he thong lon se co nhieu services.
            + 1 request se duoc call toi 3 services cung 1 luc, luon luon dam bao co se luon co 1 thang tra ra ketqua
     */
        var result = await _userService.GetAllUsers(searchTerm, pageIndex, pageSize);
        return result;
    }
    
    // PUT: /api/user/{id}
    [HttpPut("{id}")]
    public async Task<string> UpdateUserById(Guid id, [FromForm] Request.UpdateUserRequest request)
    {
        var result = await _userService.UpdateUserById(id, request);
        return result;
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
    public async Task<IActionResult> Login(string Email, string Password)
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
        var token = await _userService.Login(Email, Password);
        if (token == null)
        {
            return BadRequest();
        }
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
    
    [HttpPost("register")]
    public async Task<string> CreateNewUser(Request.CreateUserRequest request)
    {
        var result = await _userService.CreateNewUser(request);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var isDeleted = await _userService.DeleteUser(id);
        if (!isDeleted)
        {
            return NotFound();
        }
        return Ok($"Delete user id: {id}");
    }
}