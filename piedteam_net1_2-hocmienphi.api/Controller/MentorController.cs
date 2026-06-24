using Microsoft.AspNetCore.Mvc;
using piedteam_net1_2_hocmienphi.repository;
using Response = piedteam_net1_2_hocmienphi.service.MentorService.Response;
using CategoryResponse = piedteam_net1_2_hocmienphi.service.CategoryService.Response;

namespace PiedTeam_NET1_2_hocmienphi.api.Controller;

[ApiController]
[Route("api/[controller]")]
public class MentorController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public MentorController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public IActionResult GetAllMentor(
        string? searchTerm = null,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = _dbContext.Mentors
            .Where(x => x.IsDeleted == false);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x =>
                x.User.FirstName.Contains(searchTerm) ||
                x.User.LastName.Contains(searchTerm) ||
                x.User.Email.Contains(searchTerm));
        }

        var selectedQuery = query
            .OrderBy(x => x.Id)
            .Select(x => new Response.GetMentorResponse()
            {
                Id = x.Id,
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                Email = x.User.Email,
                OrganizationName = x.OrganizationName,
                CurrentPosition = x.CurrentPosition,

                Categories = x.MentorCategories
                    .Select(mc => new CategoryResponse.GetAllParentCategoryResponse()
                    {
                        Id = mc.Category.Id,
                        Name = mc.Category.Name
                    })
                    .ToList()
            });

        var result = selectedQuery
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Ok(result);
    }
}