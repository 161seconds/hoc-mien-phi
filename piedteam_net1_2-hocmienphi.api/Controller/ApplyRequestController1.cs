using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.Entity;
using piedteam_net1_2_hocmienphi.repository.Enums;
using piedteam_net1_2_hocmienphi.service.ApplyRequestService;

namespace PiedTeam_NET1_2_hocmienphi.api.Controller;
[ApiController]
[Route("api/[controller]")]
public class ApplyRequestController1 : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ApplyRequestController1(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("")]
    public IActionResult CreateApplyRequest(Guid id, Request.CreateApplyRequestRequest request)
    {
        var create = new ApplyRequest()
        {
            Id = Guid.NewGuid(),
            CvLink = request.CvLink,
            UserId = request.UserId,
            Description = request.Description,
        };
        _dbContext.ApplyRequests.Add(create);
        _dbContext.SaveChanges();
        var applyRequest = request.CategoryIds
            .Select(x => new ApplyRequestCategory()
            {
                Id = Guid.NewGuid(),
                ApplyRequestId = create.Id,
                CategoryId = x,
            });
        _dbContext.ApplyRequestCategories.AddRange(applyRequest);
        _dbContext.SaveChanges();
        return Ok();
    }
    
    [HttpGet("")]
    public IActionResult GetAllApplyRequest(
        string? searchTerm, 
        int pageIndex, 
        int pageSize, 
        ApplyRequestStatus? status)
    {
        var query = _dbContext.ApplyRequests.AsQueryable();
        query = query.Where(x => x.IsDeleted == false);
        if(!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x => x.Description.Contains(searchTerm) ||
                                     x.User.FirstName.Contains(searchTerm) ||
                                     x.User.LastName.Contains(searchTerm));
        }
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        var selectedQuery =
            query.Select(x => new Response.GetApplyRequestResponse()
            {
                Id = x.Id,
                Description = x.Description,
                CvLink = x.CvLink,
                Status = x.Status,
                RejectReason = x.RejectReason,
                User = new piedteam_net1_2_hocmienphi
                    .service.UserService.Response.GetUserResponse()
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
                            Id = y.CategoryId,
                            Name = y.Category.Name,
                        }).ToList()
            });
        selectedQuery = selectedQuery
            .OrderBy(x => x.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
        var result = selectedQuery.ToList();
        return Ok(result);
    }
    
    [HttpGet("me")]
    public IActionResult GetMyApplyRequest(Guid id)
    {
        var query = _dbContext.ApplyRequests.AsQueryable();
        query = query.Where(x => x.IsDeleted == false && x.Id == id);
        
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetApplyRequestDetail()
    {
        return Ok();
    }

    [HttpPost("{id}/review")]
    public IActionResult ReviewApplyRequest()
    {
        return Ok();
    }

    [HttpDelete("")]
    public IActionResult DeleteApplyRequest()
    {
        return Ok();
    }
}