namespace PiedTeam_NET1_2_hocmienphi.api.Middlewares;

public class GlobalExceptionHandlerMiddlewares : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next) //next đại diện cho cách mình xử lí mdw
    {
        try
        {
            await next(context);
            // tạo ra 1 cái try catch
            // sau đó ngồi cho kq của cái request này trả ra
            // request lúc này tại vì bị next rồi, request sẽ đi xuống controller, service, repo
            // vi await ở đây nên mình có thể nhận dc response của request
            // response của request ở đây có 2 trường hop
            // 1. bình thường - 200
            // 2. 1 cái exception error
            // catch nó
            // theo ae tại sao phải có global exception?
            // hạn chế duplicate logic try-catch
            // mình kh thể cover hết tất cả exception dc.
            // tránh bị crash app. vì nếu có 1 exception kh đc bắt thì app nổ
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}