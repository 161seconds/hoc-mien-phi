namespace piedteam_net1_2_hocmienphi.repository.Abstraction;
public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; }
    public bool IsDeleted { get; set; }
}