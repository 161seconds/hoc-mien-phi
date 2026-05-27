var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

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

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}