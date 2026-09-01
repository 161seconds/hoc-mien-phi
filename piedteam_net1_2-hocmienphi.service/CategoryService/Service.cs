using Microsoft.EntityFrameworkCore;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.Entity;

namespace piedteam_net1_2_hocmienphi.service.CategoryService;

public class Service : IService
{
    private readonly AppDbContext _dbContext;

    public Service(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Response.GetAllCategoryResponse>> GetAllCategories()
    {
        var query = _dbContext.Categories.Where(x => x.IsDeleted == false);
        query = query.Where(x => x.ParentId == null);
        query = query.OrderBy(x => x.Name);
        var selectedQuery = query
            .Select(x => new Response.GetAllCategoryResponse()
            {
                Id = x.Id,
                Name = x.Name
            });
            
        return await selectedQuery.ToListAsync();
    }

    public async Task<List<Response.GetAllCategoryResponse>> GetChildrenCategoryById(Guid parentId)
    {
        var query = _dbContext.Categories.Where(x => x.IsDeleted == false);
        query = query.Where(x => x.ParentId != null && x.ParentId == parentId);
        var selectedQueryV2 = query
            .Select(x => new Response.GetAllCategoryResponse()
        {
            Id = x.Id,
            Name = x.Name
        });
        
        return await selectedQueryV2.ToListAsync();
    }

    public async Task CreateCategory(Request.CreateCategoryRequest requestBody)
    {
        var newCategory = new Category()
        {
            Id = Guid.NewGuid(),
            Name = requestBody.CategoryName,
            ParentId = requestBody.ParentId
        };
        _dbContext.Categories.Add(newCategory);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteCategoryById(Guid id)
    {
        var query = _dbContext.Categories.Where(x => x.IsDeleted == false);
        query = query.Where(x => x.Id == id);
        var category = await query.FirstOrDefaultAsync();
        if (category != null)
        {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateCategory(Guid id, Request.UpdateCategoryRequest requestBody)
    {
        var query = _dbContext.Categories.Where(x => x.IsDeleted == false);
        query = query.Where(x => x.Id == id);
        var category = await query.FirstOrDefaultAsync();
        if (category != null)
        {
            category.Name = requestBody.CategoryName; 
            category.ParentId = requestBody.ParentId;
            
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
