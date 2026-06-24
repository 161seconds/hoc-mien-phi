using Microsoft.AspNetCore.Mvc;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.Entity;
using piedteam_net1_2_hocmienphi.service.CategoryService;

namespace PiedTeam_NET1_2_hocmienphi.api.Controller;
[ApiController]
[Route("api/[controller]")]

public class CategoryController : ControllerBase
{
    // dependency injection: core, kh biet = rot phong van
    // danh' 1 buoi de noi ve cai nay
    private readonly AppDbContext _dbContext;
        
    public CategoryController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet("")]
    public IActionResult GetAllCategories()
    {
        var query = _dbContext.Categories.Where(x => x.IsDeleted == false);
        query = query.Where(x => x.ParentId == null);
        query = query.OrderBy(x => x.Name);
        var selectedQuery = query
            .Select(x => new Response.GetAllParentCategoryResponse()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        // muc dich cua tao. GetAllParentCategoryResponse
        // api nay chi can 2 field ID va Name thoi, nhung ma trong db
        // luc nao cung quang ra full ( id, name ) nhung api chi can 2 field 
        // quang ra ma kh sai thi phi lam
        var result = selectedQuery.ToList();
        
        return Ok(result);
    }

    [HttpGet("{parentId}")] // dung id nay la id cua Parent
    public IActionResult GetChildrenCategoryById(Guid parentId)
    {
        var query =  _dbContext.Categories.Where(x => x.IsDeleted == false);
        query = query.Where(x => x.ParentId != null && x.ParentId == parentId);
        var selectedQueryV2 = query
            .Select(x => new piedteam_net1_2_hocmienphi.service.UserService.Request.Request.GetChildrenCategoryById()
        {
            Id = x.Id.ToString(),
            Name = x.Name
        }).ToList();
        var resultV2 = selectedQueryV2.ToList();
        return Ok(resultV2);
    }

    [HttpPost("")]
    public IActionResult CreateCategory(Request.CreateCategoryRequest requestBody)
    {
        // req no la body FE truyen cho minh, khi FE goi API nay
        // co nghia rang la, no muon tao. 1 record - 1 dong' duoi' db cua minh
        // 1 dong' duoi' db se co cac field: Id, Name, ParentId
        
        // luc nay BE phai tao ra 1 object tu class Category va' insert vao db
        // boi vi category table duoi' db, no dc tao. tu class category
        // the nen de add du lieu vao table Category thi minh phai new moi'
        // 1 object category thi moi add dc

        var newCategory = new Category()
        {
            Id = Guid.NewGuid(),
            Name = requestBody.CategoryName,
            ParentId = requestBody.ParentId
        };
        _dbContext.Categories.Add(newCategory);
        // chi moi add vao bo nho, chua add vao db
        _dbContext.SaveChanges();
        
        return Ok("create category");
    }

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
    
    [HttpDelete("{id}")]
    public  IActionResult DeleteCategoryById(Guid? id)
    {
        var query =  _dbContext.Categories.Where(x => x.IsDeleted == false);
        query = query.Where(x => x.Id == id);
        var category = query.FirstOrDefault();
        if (category != null)
        {
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
        }
        return Ok();
    }
    
    [HttpPut("{id}")] 
    public IActionResult UpdateCategory(Guid? id, Request.UpdateCategoryRequest requestBody)
    {
        var query =  _dbContext.Categories.Where(x => x.IsDeleted == false);
        query = query.Where(x => x.Id == id);
        var category = query.FirstOrDefault();
        if (category != null)
        {
            // _dbContext.Categories.Remove(category);
            // _dbContext.SaveChanges();
            category.Name = requestBody.CategoryName; // ném giá trị mới vào 
            category.ParentId = requestBody.ParentId;
            
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
        }
        return Ok("update category");
    }
}

