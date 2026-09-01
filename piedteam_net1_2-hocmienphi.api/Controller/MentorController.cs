using Microsoft.AspNetCore.Mvc;
using piedteam_net1_2_hocmienphi.service.MentorService;

namespace PiedTeam_NET1_2_hocmienphi.api.Controller;

[ApiController]
[Route("api/[controller]")]
public class MentorController : ControllerBase
{
    private readonly IService _mentorService;

    public MentorController(IService mentorService)
    {
        _mentorService = mentorService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllMentor(
        string? searchTerm = null,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var result = await _mentorService.GetAllMentor(searchTerm, pageIndex, pageSize);
        return Ok(result);
    }
}