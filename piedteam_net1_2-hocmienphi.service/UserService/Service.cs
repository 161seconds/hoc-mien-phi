using MailKit;
using Microsoft.EntityFrameworkCore;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.Entity;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using piedteam_net1_2_hocmienphi.service.Utils.JWTService;
using MailService = piedteam_net1_2_hocmienphi.service.Utils.Mail;
using MediaService = piedteam_net1_2_hocmienphi.service.Utils.MediaService;

namespace piedteam_net1_2_hocmienphi.service.UserService;

public class Service : IService
{
    private readonly AppDbContext _dbContext;
    private readonly MailService.IService _mailService;
    private readonly MediaService.IService _mediaService;
    private readonly IConfiguration _configuration;
    
    public Service(AppDbContext dbContext, MailService.IService mailService, MediaService.IService mediaService, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mailService = mailService;
        _mediaService = mediaService;
        _configuration = configuration;
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

    public async Task<string?> Login(string email, string password)
    {
        var query = _dbContext.Users.Where(x => x.IsDeleted == false);
        query = query.Where(x => x.Email == email);
        var user = await query.FirstOrDefaultAsync();
        if (user == null || user.Password != password)
        {
            return null;
        }

        var jwtOptions = new JwtOptions();
        _configuration.GetSection("JwtOptions").Bind(jwtOptions);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("UserId", user.Id.ToString()),
            new Claim("Role", user.Role),
        };
        var token = JwtService.GenerateToken(claims, jwtOptions);
        
        return token;
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> DeleteUser(Guid id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            return false;
        }
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}