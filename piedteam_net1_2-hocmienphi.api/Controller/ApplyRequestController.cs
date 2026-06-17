using Microsoft.AspNetCore.Mvc;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.Entity;
using piedteam_net1_2_hocmienphi.repository.Enums;
using piedteam_net1_2_hocmienphi.service.ApplyRequestService;

namespace PiedTeam_NET1_2_hocmienphi.api.Controller;
[ApiController]
[Route("api/[controller]")]
public class ApplyRequestController : ControllerBase
{
    private readonly AppDbContext _dbContext;
        
    public ApplyRequestController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
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
            
    
    [HttpPost("")]
    public IActionResult CreateApplyRequest(Request.CreateApplyRequestRequest requestBody)
    {
        var request = new ApplyRequest()
        {
            Id = Guid.NewGuid(),
            UserId = requestBody.UserId,
            Description = requestBody.Description,
            CvLink =  requestBody.CvLink,
            Status = ApplyRequestStatus.Pending
        };
        
        _dbContext.ApplyRequests.Add(request);
        _dbContext.SaveChanges();
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
         */
        //làm sao để biến 1 List<Guid> thành 1 List<ApplyRequestCategory)
        // dùng SELECT để ánh xạ, x lúc này tượng trưng cho 1 cateId
        var applyRequestCategory = requestBody.CategoryIds
            .Select(x => new ApplyRequestCategory()
        {
            Id = Guid.NewGuid(),
            ApplyRequestId = request.Id,
            CategoryId = x
        });
        // add range là add nhìu dòng cùng lúc
        _dbContext.ApplyRequestCategories.AddRange(applyRequestCategory);
        _dbContext.SaveChanges();
        return Ok();
    }
    
    [HttpGet("me")]
    public IActionResult GetMyApplyRequest()
    {
        return Ok();
    }
    
    [HttpGet("")]
    public IActionResult GetAllApplyRequest()
    {
        return Ok();
    }
    
    [HttpGet("{id}")] 
    public IActionResult GetApplyRequestDetail(Guid id)
    {
        return Ok();
    }
    
    [HttpPost("{id}/review")]
    public IActionResult ReviewApplyRequest(Guid id)
    {
        return Ok();
    }
}