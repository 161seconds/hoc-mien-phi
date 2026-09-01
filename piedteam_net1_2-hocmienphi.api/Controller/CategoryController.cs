using Microsoft.AspNetCore.Mvc;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.Entity;
using piedteam_net1_2_hocmienphi.service.CategoryService;
using piedteam_net1_2_hocmienphi.service.UserService;
using IService = piedteam_net1_2_hocmienphi.service.CategoryService.IService;
using Request = piedteam_net1_2_hocmienphi.service.CategoryService.Request;

namespace PiedTeam_NET1_2_hocmienphi.api.Controller;
[ApiController]
[Route("api/[controller]")]

public class CategoryController : ControllerBase
{
    private readonly IService _categoryService;
        
    public CategoryController(IService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpGet("")]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _categoryService.GetAllCategories();
        return Ok(result);
    }

    [HttpGet("{parentId}")] // dung id nay la id cua Parent
    public async Task<IActionResult> GetChildrenCategoryById(Guid parentId)
    {
        var result = await _categoryService.GetChildrenCategoryById(parentId);
        return Ok(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateCategory(Request.CreateCategoryRequest requestBody)
    {
        await _categoryService.CreateCategory(requestBody);
        return Ok();
    }

    /*
     // [HttpDelete("{id}")]
    // public IActionResult DeleteCategory(int? id)
    // {
    //     return Ok("delete category");
    // }

    
    //bai tap ve nha
        // tao. moi user
        // GetAllUser theo phan trang
        // Search, OrderBy
        // GetUserById
     */
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoryById(Guid id)
    {
        var result = await _categoryService.DeleteCategoryById(id);
        if (result)
        {
            return Ok();
        }
        return NotFound();
    }
    
    [HttpPut("{id}")] 
    public async Task<IActionResult> UpdateCategory(Guid id, Request.UpdateCategoryRequest requestBody)
    {
        var result = await _categoryService.UpdateCategory(id, requestBody);
        if (result)
        {
            return Ok("update category");
        }
        return NotFound();
    }
}

