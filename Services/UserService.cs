using System.Security.Claims;

namespace MagikarpMayhem.Services;

public class UserService
{
    private readonly Data.MagikarpMayhemContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(Data.MagikarpMayhemContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public Models.User? GetUserById(int id)
    {
        return _context.User.FirstOrDefault(user => user.Id == id);
    }
    
    public Models.User? GetCurrentUser()
    {
        int? currentUserId = GetCurrentUserId();
        if (currentUserId == null)
        {
            return null;
        }
        return GetUserById(currentUserId.Value);
    }

    private int? GetCurrentUserId()
    {
        // Assuming you have access to HttpContext
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? int.Parse(userIdClaim.Value) : null;
    }
}