using Microsoft.EntityFrameworkCore;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.Entity;
using piedteam_net1_2_hocmienphi.service.Utils.Mail;
using Quartz;

namespace piedteam_net1_2_hocmienphi.service.Utils.BackgroundJob;

[DisallowConcurrentExecution]
//tôi bắt buộc code trước đó hoàn thành xong mới cho chạy tiếp

public class SendAdvertisingJob : IJob
{
    private readonly AppDbContext _dbContext;
    private readonly IService _mailService;
    
    public SendAdvertisingJob(AppDbContext dbContext,  IService mailService)
    {
        _dbContext = dbContext;
        _mailService = mailService;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        DateOnly today = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone));
        
        // ae hiểu như thế nào về các múi giờ UTC trong lập trình
        var batchSize = 100;
        var notSendToday = await GetBatchUSersNotSendMailToday(today, batchSize);
        await SendBatchMail(notSendToday, today);
        _dbContext.Users.UpdateRange(notSendToday);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetBatchUSersNotSendMailToday(DateOnly today, int  BatchSize)
    {
        var query = _dbContext.Users
            .Where(u => u.IsDeleted == false);
        query = query.Where(u => u.SendDate <= today);
        query = query.OrderBy(u => u.Id)
            .Take(BatchSize);
        var result = await query.ToListAsync();
        return result;

    }

    public async Task SendBatchMail(List<User> users, DateOnly today)
    {
        // mảng user -> 1 mảng công việc cần gửi
        // hành động này gọi là ánh xạ Select
        // var taskList = new List<Task>();
        // foreach (var x in users)
        // {
        //     var mailContent = new MailContent();
        //     {
        //         To = x.Email,
        //         Body = "hi",
        //         Subject = "hi",
        //     }
        //     
        //     var userTask = Task.Run(async () => 
        //         _mailService.SendMail(mailContent));
        //     await _mailService.SendMail(mailContent);
        // }
        // await Task.WhenAll();
        var tasks = users.Select(async user =>
        {
            try
            {
                var mailContent = new MailContent
                {
                    To = user.Email,
                    Body = "hi",
                    Subject = "hi"
                };
                await _mailService.SendMail(mailContent);
                user.SendDate = today;
            }
            catch (Exception ex)
            {
                //log loi
            }
        });
        await Task.WhenAll(tasks);
    }

    public static class TimeZoneUtils
    {
        public static TimeZoneInfo GetVietNamTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");
            }
        }
    }
}
/*
     background job: 
        là 1 tác vụ dc thực thi ngầm tách biệt khỏi luồng xử lí chính
        tác vụ này do hệ thống thực thi
        
        vậy thì khi nào server mình sẽ chạy công việc này
            setup định kì (cronJob): cứ mỗi 7g sáng là gửi thư quảng cáo
            xóa mềm (isDeleted = true): theo thời gian, những record xóa mềm sẽ tăng lên,
                nhưng mà những dòng record này thì chúng ta kh đọc đến
                vậy nên dữ liệu bị dư thừa, db nặng lên, nên phải xóa nó đi
        la minh sẽ nhờ nó thay mình làm công việc này giúp mình (quá bận)
            anh có 1 hàm register
                xuống db kiểm tra email tồn tại (10ms)
                tính toán để tạo mk Hashing (2ms)
                lưu user này vào db (5ms)
                gửi email thông báo verify tài khoản (10s)
            logic gửi email là 1 locgic rất là nặng, và tốn nhiều thời gian để hoàn thành
                mình có thể chọn khi user bấm register khơi cho đợi email, trả ra response luôn
                cái việc mà mình gửi email thông báo sẽ do luồng background job gửi
            
            bây gio tôi muốn gửi mail cho tất cả user trong hệ thống vào 7g sáng
                đối với user ít, tầm 20 thằng thì gọi ngon luôn
                    cứ 7g sáng mình gọi 20 thằng đó ra gửi mail
                đổi với 1m user thì sao, 1tr * 20s
                xài task.whenAll để tối ưu 
                (trong vòng 20s đó, sẽ có 1000 thằng dc gửi mail)
                (trong vòng 5g, sẽ gửi xong mail cho 1tr user)
            à anh oi, sao mình kh trong vòng 20s mình gửi 1tr user luôn đi
            1 task dc tạo ra, giống như mở thêm 1 luồng bất đồng bộ để chạy
            server xử lí 1tr luồng cùng 1 lúc kh nổi
            nên là mình mới đánh đổi thời gian, dài hơn 1 xíu, 1 lần gửi 1000 user thôi
            nhưng server vẫn chịu đc và sống tốt
            
            nhưng thêm 1 vấn đề nữa, bây giờ mình chỉ gửi mail 1 lúc 1000 thằng thôi
                vậy thì chúng ta phải có 1 cái gì đó để đánh dấu
                là user nào đã gửi, user nào chưa gửi
                à tôi nghĩ ngay isSend, ngon lành luôn, siêu System design
                cái isSend này chỉ ngon lành khi dòng dữ liệu này đụng đến 1 lần r thôi
                ví dụ: trạng thái của đơn hàng này isPending, nếu như mà trong vòng 1g kh thanh toán
                thì mình chuyển thành isFailed rồi thôi, kh đụng tới nữa
                vậy thì isSend thì sao, nếu để isSend ở user, 
                mình sẽ kh biết được
                ngày nào đã gửi hay chưa, ngày 6 em để isSend là true, 
                qua ngày 7, isSend cũng là true
                vậy hệ thống biết gửi kiểu gì
     */