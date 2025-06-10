using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BjjTrainer_API.Controllers
{
    public class BaseController : Controller
    {
        protected string GetCurrentUserId()
        {
            if (User?.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(userId) ? throw new UnauthorizedAccessException("User ID claim is missing.") : userId;
        }
    }
}
