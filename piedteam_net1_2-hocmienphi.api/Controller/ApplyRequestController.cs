using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PiedTeam_NET1_2_hocmienphi.api.Extensions;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.Entity;
using piedteam_net1_2_hocmienphi.repository.Enums;
using piedteam_net1_2_hocmienphi.service.ApplyRequestService;

namespace PiedTeam_NET1_2_hocmienphi.api.Controller;
[ApiController]
[Route("api/[controller]")]
public class ApplyRequestController : ControllerBase
{
    private readonly IService _applyRequestService;
        
    public ApplyRequestController(IService applyRequestService)
    {
        _applyRequestService = applyRequestService;
    }
    
    /*
     // flow thiết kế api như thế nào
        // xác định có những api nào
            // api này sẽ làm nhiệm vụ gì
            // xác định các endpoint (method, url)
            // ai sẽ gọi api này
            // ngay lập tức chui vào controller
                // để nó ra 1 cái swagger cho an tâm
        // để làm hành động này thì cần những yêu cầu gì (input)
            // ngay lập tức list ra luon
            // đi vào model ngắm entity xem có field gi
            // sau khi xac dinh đc input rồi,
            // tức tốc đi vào service
                // chui ngay vào folder request tạo luôn
            // sau đó lắp đồ chơi này vào api mà nó cần ở controller
            // sau khi đã xac dinh dc input roi
            // quay ra hỏi thằng FE là m có cần t trả ra thêm gi kh
            // nếu cần => đi ngay vào service và tạo class response
            // nếu kh => thì thôi
        // sau khi controller đủ đồ chơi r, có đủ input, output rồi
            // implement logic xử lí bên trong
     */
    /*
         // apply Request Cate là bảng lưu trữ những lĩnh vực mà người mentor đã apply
        // FE: public List<Guid> CategoryId {get; set;}
        // trong db thì lưu trữ bằng Entity ApplyRequestCategory
        /*
         public class ApplyRequestCategory : BaseEntity<Guid>
        {
            public Guid ApplyRequestId { get; set; }
            public ApplyRequest ApplyRequest { get; set; }

            public Guid CategoryId { get; set; }
            public Category Category { get; set; }
        }
         * /
        //làm sao để biến 1 List<Guid> thành 1 List<ApplyRequestCategory)
        // dùng SELECT để ánh xạ, x lúc này tượng trưng cho 1 cateId
        // fe chi truyen cho mình 1 list requestBody.CategoryIds
        // nhung ma du lieu ma mentor mong muon duoc mentoring
        // thi no nam o bảng ApplyRequestCategory
        // vay nen minh phai anh xa tu List sang requestBody.CategoryIds
        // sang list ApplyRequestCategory
         */
    
    [HttpPost("")]
    public async Task<IActionResult> CreateApplyRequest(Request.CreateApplyRequestRequest requestBody)
    {
        await _applyRequestService.CreateApplyRequest(requestBody);
        return Ok();
    }
    
    [Authorize(Policy = JwtExtensions.AdminPolicy)] // [] dc goi la annotations
    // tôi sẽ apply authorization theo policy (nguyen tac, tieu chuan)
    [HttpGet("")]
    public async Task<IActionResult> GetAllApplyRequest(
        string? searchTerm = null, ApplyRequestStatus? status = null,
        int PageIndex = 1, int PageSize = 10)
    {
        var result = await _applyRequestService.GetAllApplyRequest(searchTerm, status, PageIndex, PageSize);
        return Ok(result);
    }
    
    //lấy ra những đơn của tui
    // khi mà đã authen với author rồi thì có nghĩa là gì ?
        // bạn chính là user trong hệ thống của chúng tôi
        // và bạn có quyền hạn truy cập các API mà chúng tôi cho phép
        // vì hệ thống đã bt chúng ta là ai rồi, thế nên chúng ta có thể lược bỏ
        // và kh cần truyền những field kh cần thiết
        // vd: Guid UserId
        // vậy thì hệ thống bt người dùng là ai, userId, email, firstname, lastname
            // bằng cách nào? 
        // hệ thống sẽ bt đc, tại ví chúng ta đã ghi những thông tin đó vào payload mà
            // xem lại ở phần Login
    [Authorize(Policy = JwtExtensions.MentorPolicy)]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyApplyRequest(
        ApplyRequestStatus? status = null,
        int pageIndex = 1,
        int pageSize = 10
        //htppContext đại diện cho cái req đc gọi tới (req co access token, origin, thong tin người gọi, 
        // vd GetMyApplyReq thì mọi cái thông tin sẽ nằm trong httpcontext
        )
    {
        var userIdString = HttpContext.User.Claims.FirstOrDefault(
            x => x.Type.Equals("UserId")
        )!.Value;
        var userId = Guid.Parse(userIdString); 
        
        var result = await _applyRequestService.GetMyApplyRequest(userId, status, pageIndex, pageSize);
        return Ok(result);
    }
    
    [HttpGet("{id}")] 
    public async Task<IActionResult> GetApplyRequestDetail(
        Guid ApplyRequestId)
    {
        var result = await _applyRequestService.GetApplyRequestDetail(ApplyRequestId);
        return Ok(result);
    }

    [HttpPost("{id}/review")]
    public async Task<IActionResult> ReviewApplyRequest(Guid id, Request.ReviewApplyRequestRequest requestBody)
    {
        /*
         flow của review apply request
         đầu tiên là lấy ra những cái applyReq chưa bị xóa
         tiếp theo la mình sẽ cần join bảng thủ công giữa user và applyReqCategories
         sau đó nếu id AR trùng với id can tim thì lụm
           lấy cái AR đầu tiên tìm thấy
           nếu null thì trả NotFound
         tiếp theo thì đến bước xem cái AR dc approved hay la bi từ chối
           nếu dc approve
             thì set lai cái status thành Approve
             và set role của user thành mentor
               tạo 1 mentor mới
               tiếp theo để lấy ra cái Category của mentor đó
               thì phai lấy cái AR vừa duyệt xong đem qua map thành cái MentorCategory
                 thì ta lấy dc id, mentorId, cateId
            nếu kh dc approve
              thi set lai cai status thanh Reject
              và đưa ra cái lí do bị từ choi RejectReason
        */
        var success = await _applyRequestService.ReviewApplyRequest(id, requestBody);
        if (!success)
        {
            return NotFound();
        }
        return Ok();
    }
}