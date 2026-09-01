using Microsoft.EntityFrameworkCore;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.Entity;
using piedteam_net1_2_hocmienphi.repository.Enums;

namespace piedteam_net1_2_hocmienphi.service.ApplyRequestService;

public class Service : IService
{
    private readonly AppDbContext _dbContext;

    public Service(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateApplyRequest(Request.CreateApplyRequestRequest requestBody)
    {
        var request = new ApplyRequest()
        {
            Id = Guid.NewGuid(),
            UserId = requestBody.UserId,
            Description = requestBody.Description,
            CvLink = requestBody.CvLink,
            Status = ApplyRequestStatus.Pending
        };
        _dbContext.ApplyRequests.Add(request);
        await _dbContext.SaveChangesAsync();

        var applyRequestCategory = requestBody.CategoryIds
            .Select(x => new ApplyRequestCategory()
            {
                Id = Guid.NewGuid(),
                ApplyRequestId = request.Id,
                CategoryId = x
            });
        
        _dbContext.ApplyRequestCategories.AddRange(applyRequestCategory);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Response.GetApplyRequestResponse>> GetAllApplyRequest(string? searchTerm = null, ApplyRequestStatus? status = null, int pageIndex = 1, int pageSize = 10)
    {
        var query = _dbContext.ApplyRequests
            .Where(x => x.IsDeleted == false);
            
        if(!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x => x.Description.Contains(searchTerm) 
                                     || x.User.FirstName.Contains(searchTerm)
                                     || x.User.LastName.Contains(searchTerm));
        }
        
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }

        var selectedQuery = query.Select(x => new Response.GetApplyRequestResponse()
        {
            Id = x.Id,
            Description = x.Description,
            CvLink = x.CvLink,
            Status = x.Status,
            RejectReason = x.RejectReason,
            User = new UserService.Response.GetUserResponse()
            {
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                Age = x.User.Age,
                Email = x.User.Email,
            },
            Categories = x.ApplyRequestsCategories.Select(y => new CategoryService.Response.GetAllCategoryResponse()
            {
                Id = y.Category.Id,
                Name = y.Category.Name,
            }).ToList()
        });

        selectedQuery = selectedQuery
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
            
        return await selectedQuery.ToListAsync();
    }

    public async Task<List<Response.GetApplyRequestResponse>> GetMyApplyRequest(Guid userId, ApplyRequestStatus? status = null, int pageIndex = 1, int pageSize = 10)
    {
        var query = _dbContext.ApplyRequests
            .Where(x => x.IsDeleted == false);
        query = query.Where(x => x.UserId == userId);

        if (status != null) 
        {
            query = query.Where(x => x.Status == status);
        }

        var selectedQuery = query.Select(x => new Response.GetApplyRequestResponse()
        {
            Id = x.Id,
            Description = x.Description,
            CvLink = x.CvLink,
            Status = x.Status,
            RejectReason = x.RejectReason,
            User = new UserService.Response.GetUserResponse()
            {
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                Age = x.User.Age,
                Email = x.User.Email,
            },
            Categories = x.ApplyRequestsCategories.Select(y => new CategoryService.Response.GetAllCategoryResponse()
            {
                Id = y.Category.Id,
                Name = y.Category.Name,
            }).ToList()
        });

        selectedQuery = selectedQuery
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);

        return await selectedQuery.ToListAsync();
    }

    public async Task<Response.GetApplyRequestResponse?> GetApplyRequestDetail(Guid applyRequestId)
    {
        var query = _dbContext.ApplyRequests
            .Where(x => x.IsDeleted == false);
        query = query.Where(x => x.Id == applyRequestId);

        var selectedQuery = query.Select(x => new Response.GetApplyRequestResponse()
        {
            Id = x.Id,
            Description = x.Description,
            CvLink = x.CvLink,
            Status = x.Status,
            RejectReason = x.RejectReason,
            User = new UserService.Response.GetUserResponse()
            {
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                Age = x.User.Age,
                Email = x.User.Email,
            },
            Categories = x.ApplyRequestsCategories.Select(y => new CategoryService.Response.GetAllCategoryResponse()
            {
                Id = y.Category.Id,
                Name = y.Category.Name,
            }).ToList()
        });

        return await selectedQuery.FirstOrDefaultAsync();
    }

    public async Task<bool> ReviewApplyRequest(Guid id, Request.ReviewApplyRequestRequest requestBody)
    {
        var applyRequest = await _dbContext.ApplyRequests
            .Where(x => x.IsDeleted == false)
            .Include(x => x.User)
            .Include(x => x.ApplyRequestsCategories)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (applyRequest == null)
        {
            return false;
        }

        if (requestBody.IsApproved)
        {
            applyRequest.Status = ApplyRequestStatus.Approved;
            applyRequest.User.Role = "Mentor";
            var mentor = new Mentor()
            {
                Id = Guid.NewGuid(),
                UserId = applyRequest.UserId,
            };
            _dbContext.Mentors.Add(mentor);
            var mentorCategories = applyRequest.ApplyRequestsCategories.Select(y => new MentorCategory()
            {
                Id = Guid.NewGuid(),
                CategoryId = y.CategoryId,
                MentorId = mentor.Id,
            }).ToList();
            _dbContext.MentorCategories.AddRange(mentorCategories);
        }
        else
        {
            applyRequest.Status = ApplyRequestStatus.Rejected;
            applyRequest.RejectReason = requestBody.Reason;
        }
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
