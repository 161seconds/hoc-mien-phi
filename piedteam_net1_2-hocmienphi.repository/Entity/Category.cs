using piedteam_net1_2_hocmienphi.repository.Abstraction;

namespace piedteam_net1_2_hocmienphi.repository.Entity;

public class Category : BaseEntity<Guid>
{
    public string Name { get; set; }
    
    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }
    
    public ICollection<Category> Children { get; set; }
    public ICollection<Mentor> Mentors { get; set; }
    public ICollection<MentorCategory> MentorCategories { get; set; }
    public ICollection<ApplyRequest> ApplyRequests { get; set; }
    // nho khai bao ParentId va Parent,
    // ma EF se tu dong hieu rang
    // giua 2 truong nay co mqh voi nhau
    // va se tu dong tao ra 1 foreign key trong db de lket giua 2 bang Category voi nhau
    
    // nho vao viec khai bao ICollection<Category> Children
    // ma EF se tu dong hieu rang
    // va tao ra mqh 1 - N
}