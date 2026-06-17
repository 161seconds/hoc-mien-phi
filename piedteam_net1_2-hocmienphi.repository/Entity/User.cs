using piedteam_net1_2_hocmienphi.repository.Abstraction;

namespace piedteam_net1_2_hocmienphi.repository.Entity;
public class User : BaseEntity<Guid>
// khi ma ke thua thi thanwgf cha (class | abstract class) luon luon 
// nằm đầu tien sau dau : 
// nhung thang nam sau thang cha là interface
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Age { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public Mentor? Mentor { get; set; }
    
    public ICollection<ApplyRequest> ApplyRequests {get; set;}
    public ICollection<Booking> Bookings {get; set;}
}