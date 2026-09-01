namespace piedteam_net1_2_hocmienphi.service.CategoryService;

public interface IService
{
    public Task<List<Response.GetAllCategoryResponse>> GetAllCategories();
    public Task<List<Response.GetAllCategoryResponse>> GetChildrenCategoryById(Guid parentId);
    public Task CreateCategory(Request.CreateCategoryRequest requestBody);
    public Task<bool> DeleteCategoryById(Guid id);
    public Task<bool> UpdateCategory(Guid id, Request.UpdateCategoryRequest requestBody);
}
