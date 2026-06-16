using Microsoft.EntityFrameworkCore;
using piedteam_net1_2_hocmienphi.repository;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//kien truc 3 layer
//tầng api
    // chiu trach nhhiem khai bao cac endpoint,
        //nhan va tra request, response
    // config he thong
    // tang api nay gọi đến service

//tang service
    // chiu trach nhiem xu li nghiep vu
    // tuong tac voi repo de lay du lieu
    // gọi den repository

//tang repository
    // chiu trach nhiem tuong tac voi DB
    // cau hinh nhung thu lien quan den DB

//request là lấy đăng nhap vao he thong
    //tang API: muốn login vào he thong
        //chui vo day ne: POST /api/auth/login
            //nhận request body {email: "bao", password: "123"}
    // tang API lúc này goi xuong tang Service có cái hàm là
        // xu lí login: LoginHandler(email, password)
        // luc nay ham login trong Service se chay như sau
            // ktra email | nguoi dung co ton tai trong db kh
            // nguoi dung nay co bị Ban kh
            // neu có lỗi thì tra ve loi
            // neu kh co loi thì tra ve token dang nhap
    // tang Service luc nay goi xuong tang Repo co cái hàm là
        // GetUserByEmail(email)
        // hàm luc nay sẽ chạy câu lệnh SQL
            // lay thong tin nguoi dung ra khoi db
    
// nơi cho các học sinh đi vào nền tảng tìm mentor
     // mentor để ptr bản than, trao doi kien thuc, kinh nghiem voi nhau
    // khi mà mentor muon apply vào nền tảng thì phải điền thông tin,
        // sau đó admin sẽ duyệt
// mentor di vào nền tảng tạo lịch rảnh, 
    // và các hoc sinh sẽ book lịch rảnh đó
// để 1 user trở thành 1 mentor
    // user sẽ điền thông tin để apply trở thành mentor
        // 1 user sẽ có yêu cầu (apply req) để trở thành mentor
            // khi mà người dùng đưa cho mình CV file, chúng ta sẽ upload file đó lên cloud
        // mối quan hệ giữa user và apply req
        // mối quan hệ 1 - N: vì có thể có đơn bị từ chối hoặc được duyệt, nhưng chỉ có 1 đơn được duyệt
        // khi mà có User nộp đơn apply, thì hệ thống phải thông báo cho admin
        // khi mà admin duyệt (từ chối, chấp nhận) thì phải thông báo cho User
        
    // admin sẽ duyệt thông tin đó, nếu thông tin hợp lệ thì sẽ duyệt
    // nếu được duyệt, user đó sẽ trở thành mentor
    // chi có user nào có quyền admin thì mới được sử dụng các api như lấy đơn hệ thống/phê duyệt đơn
    
    // API:
    // 1. Tạo đơn apply
    //    - User gửi đơn apply để đăng ký trở thành Mentor.
        // POST /api/applyRequest
            // để gọi đc api này thì cần cv và mô tả bản thân
    // 2. Lấy các đơn apply của tôi
    //    - Dành cho User/Mentor.
    //    - Mentor cần API này để kiểm tra tiến độ hoặc tình trạng của đơn apply.
        // GET /api/apllyRequest/me
    
    // 3. Lấy tất cả các đơn apply
    //    - Dành cho Admin.
        // GET /api/applyRequest
    //    - Admin cần API này để xem danh sách đơn apply của người dùng.
    
    // 4. Lấy thông tin chi tiết của một đơn apply
    //    - Dành cho Admin.
        // GET /api/applyRequest/{id}
    //    - Admin xem chi tiết nội dung đơn trước khi duyệt.
    
    // 5. Duyệt đơn apply
    //    - Dành cho Admin.
        // GET /api/applyRequest/{id}/review
    //    - Khi duyệt đơn, Admin có thể chọn chấp nhận hoặc từ chối.
    //    - Nếu từ chối, Admin phải nhập lý do từ chối.
    //    - Sau khi duyệt, hệ thống phải thông báo kết quả đơn apply cho User.


// kĩ thuật snapshot
    // trong năm 2026, anh Tân bán 1 cai áo với giá là 1k 
    // sau đó Bảo thấy "áo đẹp quá", muon mua cho crush
        // Bảo mua 1 đơn hàng(Order) có ID la O1
            // trong đơn hàng có 2sp là P1 va P2 -> tổng 3k
    // thời gian thấm thoát
    // bây giờ là năm 2027, anh Tân đổi ID 1 giá thành 2k
    // sau đó Bảo ct người iu, đòi lại quà, 
        // lấy hóa đơn ra với sp P1
    
    
// trong 1 quá trình ptr phần mềm dau tien minh se trai qua: 
    // đi tìm hiểu nhu cau cua khach hang
    // hiều dc nhu cau cua khach hang thi phan tich cac requirements
    // app này gòm bnhiu nguoi sài
        // tim hieu them tren mang
    // role:
        // admin:
            // quan li user (hoc sinh va mentor)  cua nen tảng
            // quan li va phe duyệt đơn để trở thành mentor
        // mentor
            // tạo lịch cho hs book
            // quản lí lịch book (dời lich, huy lich)
            // quan li profile
        // hoc sinh
            // book lich ranh
            // quản lí lịch book (dời lich, huy lich)
            // quan li profile'

    //phuong phap 4w1h
// what why when where how

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}