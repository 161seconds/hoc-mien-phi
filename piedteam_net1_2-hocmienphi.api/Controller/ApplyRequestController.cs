using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        var applyRequestCategory = requestBody.CategoryIds
            .Select(x => new ApplyRequestCategory()
        {
            Id = Guid.NewGuid(),
            ApplyRequestId = request.Id,
            CategoryId = x
        });
        // sau khi anh xa. thi
        // add range là add nhìu dòng cùng lúc
        _dbContext.ApplyRequestCategories.AddRange(applyRequestCategory);
        _dbContext.SaveChanges();
        return Ok();
    }
    
    [HttpGet("")]
    public IActionResult GetAllApplyRequest(
        string? searchTerm = null, ApplyRequestStatus? status = null,
        int PageIndex = 1, int PageSize = 10)
    {
        var query = _dbContext.ApplyRequests
            .Where(x => x.IsDeleted == false);
        if(!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x => x.Description.Contains(searchTerm) 
                                     || x.User.FirstName.Contains(searchTerm)
                                     || x.User.LastName.Contains(searchTerm));
            /*
             // do gio chung ta chi tim kiem dieu kien o table hien tai thoi
            // dong' 96
            // con 2 thang sau thi no upd len 1 ti
            // dong' 97 98
            // luc nay thi no join voi table User de tim kiem
             */
        }
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }

        var selectedQuery = 
            query.Select(x => 
            new Response.GetApplyRequestResponse()
            {
                Id = x.Id,
                Description = x.Description,
                CvLink = x.CvLink,
                Status = x.Status,
                RejectReason =  x.RejectReason,
                User = new piedteam_net1_2_hocmienphi.service.UserService.Response.GetUserResponse()
                {
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Age = x.User.Age,
                    Email = x.User.Email,
                },
                Categories = x.ApplyRequestsCategories
                    .Select(y => new piedteam_net1_2_hocmienphi
                    .service.CategoryService.Response.GetAllCategoryResponse()
                {
                    Id = y.Category.Id,
                    Name = y.Category.Name,
                }).ToList()
            });
        selectedQuery = selectedQuery
            .Skip((PageIndex - 1) * PageSize)
            .Take(PageSize);
        var result = selectedQuery.ToList();
        return Ok(result);
    }
    
    [HttpGet("me")]
    public IActionResult GetMyApplyRequest(
        Guid UserId, 
        ApplyRequestStatus? status = null,
        DateTimeOffset? fromDate = null,
        DateTimeOffset? toDate = null,
        List<Guid> CategoryIds = null
        )
    {
        var query = _dbContext.ApplyRequests
            .Where(x => x.IsDeleted == false);
        query = query.Where(x => x.UserId == UserId);
        /*
         // cateId: la nhung cateId ma FE muon tim kiem
        // toi muon tim nhung la don co Id la nhu nay
        // .any func
        // vd: toi muon lay nhung la don co category la "kinh te"
            // mentor1: 
            // mentor2: 
         */
        if (CategoryIds != null && CategoryIds.Count > 0)
        {
            query = query.Where(x => CategoryIds.Contains(x.Id));
        }
        if (status != null) query = query.Where(x => x.Status == status);
        var selectedQuery = 
            query.Select(x => 
                new Response.GetApplyRequestResponse()
                {
                    Id = x.Id,
                    Description = x.Description,
                    CvLink = x.CvLink,
                    Status = x.Status,
                    RejectReason =  x.RejectReason,
                    User = new piedteam_net1_2_hocmienphi.service.UserService.Response.GetUserResponse()
                    {
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        Age = x.User.Age,
                        Email = x.User.Email,
                    },
                    Categories = x.ApplyRequestsCategories.Select(y => new piedteam_net1_2_hocmienphi
                        .service.CategoryService.Response.GetAllCategoryResponse()
                        {
                            Id = y.Category.Id,
                            Name = y.Category.Name, 
                        }).ToList()
                });
        var result = selectedQuery.ToList();
        return Ok(result);
    }
    
    [HttpGet("{id}")] 
    public IActionResult GetApplyRequestDetail(
        Guid ApplyRequestId)
    {
        var query = _dbContext.ApplyRequests
            .Where(x => x.IsDeleted == false);
        var selectedQuery = 
            query.Select(x => 
                new Response.GetApplyRequestResponse()
                {
                    Id = x.Id,
                    Description = x.Description,
                    CvLink = x.CvLink,
                    Status = x.Status,
                    RejectReason =  x.RejectReason,
                    User = new piedteam_net1_2_hocmienphi.service.UserService.Response.GetUserResponse()
                    {
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        Age = x.User.Age,
                        Email = x.User.Email,
                    },
                    Categories = x.ApplyRequestsCategories.Select(y => new piedteam_net1_2_hocmienphi
                        .service.CategoryService.Response.GetAllCategoryResponse()
                        {
                            Id = y.Category.Id,
                            Name = y.Category.Name,
                        }).ToList() 
                });
        var result = selectedQuery.ToList().FirstOrDefault();
        return Ok(result);
    }
    
    [HttpPost("{id}/review")]
    public IActionResult ReviewApplyRequest(Guid id, Request.ReviewApplyRequestRequest requestBody)
    {
        var query = _dbContext.ApplyRequests.Where(x => x.IsDeleted == false);
        query = query.Include(x => x.User)
                        .Include(x => x.ApplyRequestsCategories);
        query = query.Where(x => x.Id == id);  
        var applyRequest = query.FirstOrDefault();
        if (applyRequest == null){
            return NotFound();
        }

        if (requestBody.IsApproved)
        {
            applyRequest.Status = ApplyRequestStatus.Approved;
            applyRequest.User.Role = "Mentor";
            /*
             // a lua e, o trong select x.User 1 cai la ngon luon, tu join luon 
                // tai sao o day api call 1 cai la null
            // auto join no chi hoat dong khi minh select thoi
            // con` o day neu ma muon .User thi minh phai su dung
                // Include de join thu cong
             */
                var mentor = new Mentor()
                {
                    Id = Guid.NewGuid(),
                    UserId = applyRequest.UserId,
                };
                var mentorCategories = applyRequest.ApplyRequestsCategories
                    .Select(x => new MentorCategory()
                    {
                        Id = Guid.NewGuid(),
                        MentorId = mentor.Id,
                        CategoryId = x.CategoryId
                    });
                _dbContext.Mentors.Add(mentor);
                _dbContext.MentorCategories.AddRange(mentorCategories);
                _dbContext.SaveChanges();
        }
        else
        {
            applyRequest.Status = ApplyRequestStatus.Rejected;
            applyRequest.RejectReason = requestBody.Reason;
        }
        _dbContext.ApplyRequests.Update(applyRequest);
        _dbContext.SaveChanges();
        return Ok();
    }
    
}