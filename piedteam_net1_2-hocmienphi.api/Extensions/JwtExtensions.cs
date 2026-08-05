using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using piedteam_net1_2_hocmienphi.service.Utils.JWTService;

namespace PiedTeam_NET1_2_hocmienphi.api.Extensions;
// day se la noi config cai jwt token
public static class JwtExtensions
{
    public const string AdminPolicy = "AdminPolicy";
    public const string MentorPolicy = "MentorPolicy";
    public const string AdminAndMentorPolicy = "AdminAndMentorPolicy";
    
    public static void AddJwtServices(this IServiceCollection services, IConfiguration configuration)
    {
        JwtOptions jwtOption = new JwtOptions();
        configuration.GetSection(nameof(JwtOptions)).Bind(jwtOption);
        var key = Encoding.UTF8.GetBytes(jwtOption.SecretKey);
            // máy móc nó kh xử lí string như chúng ta, tùy thư viện, 
            // đối với thư viện này thì nó tương tác với mảng byte
        services.AddAuthentication(options =>
            { // day la config authen
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true, 
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOption.Issuer,
                    ValidAudience = jwtOption.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role
                };
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy",
                policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
            options.AddPolicy("MentorPolicy",
                policy => policy.RequireClaim(ClaimTypes.Role, "Mentor"));
            options.AddPolicy("AdminAndMentorPolicy", 
                policy =>  policy.RequireClaim(ClaimTypes.Role, "AdminAndMentor"));
        });
    }
}