namespace piedteam_net1_2_hocmienphi.service.CategoryService;

public class Request
{
    public class CreateCategoryRequest
    {
        public string CategoryName { get; set; }
        public Guid? ParentId { get; set; }
    }

    public class UpdateCategoryRequest : CreateCategoryRequest;
}