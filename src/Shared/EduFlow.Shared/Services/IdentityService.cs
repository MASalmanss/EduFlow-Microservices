using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EduFlow.Shared.Services;

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public string GetUserId()
        => _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public string GetUserName()
        => _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;
}
