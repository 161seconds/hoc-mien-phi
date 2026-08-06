using MailKit;
using Microsoft.EntityFrameworkCore;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.Entity;
using MailService = piedteam_net1_2_hocmienphi.service.Utils.Mail;
using MediaService = piedteam_net1_2_hocmienphi.service.Utils.MediaService;

namespace piedteam_net1_2_hocmienphi.service.UserService;

public class Service : IService
{
    private readonly AppDbContext _dbContext;
    private readonly MailService.IService _mailService;
    private readonly MediaService.IService _mediaService;
    
    public Service(AppDbContext dbContext, MailService.IService mailService, MediaService.IService mediaService)
    {
        _dbContext = dbContext;
        _mailService = mailService;
        _mediaService = mediaService;
    }

    public async Task<List<Response.GetUserResponse>> GetAllUsers(string? searchTerm, int pageIndex, int pageSize)
    {
        var query = _dbContext.Users.AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x => x.FirstName.Contains(searchTerm) || 
                                     x.LastName.Contains(searchTerm) || 
                                     x.Email.Contains(searchTerm));
        }

        var selectedUser = query
            .OrderBy(x => x.Id)
            .Select(x => new Response.GetUserResponse()
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
            });
        selectedUser = selectedUser
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
        var result = await selectedUser.ToListAsync();
        return result;
    }

    public async Task<string> CreateNewUser(Request.CreateUserRequest request)
    {
        var createUser = new User()
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Age = request.Age,
            Email = request.Email,
            Password = request.Password,
            Role = "User",
            IsDeleted = false
        };
        
        _dbContext.Users.Add(createUser);
        
        await _dbContext.SaveChangesAsync();

        var mailContent = new MailService.MailContent()
        {
            Body = MailService.MailTemplates.GetHelloMailTemplate(createUser.FirstName + " " + createUser.LastName),
            To = createUser.Email,
            Subject = "Welcome to Hoc Mien Phi System"
        };

        await _mailService.SendMail(mailContent);
        return "created";
    }

    public async Task<string> UpdateUserById(Guid id, Request.UpdateUserRequest request)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == id);
        
        if (user == null)
        {
            return "User not found";
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.Age = request.Age;
        user.Password = request.Password;

        if (request.Avatar != null)
        {
            var avatarUrl = await _mediaService.UploadImageAsync(request.Avatar);
            user.Avatar = avatarUrl;
        }
        
        await _dbContext.SaveChangesAsync();
        
        return "updated"; 
    }
}