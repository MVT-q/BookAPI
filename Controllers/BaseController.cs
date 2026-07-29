using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected int CurrentUserId
        {
            get
            {
                var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(claim, out var currentUserId))
                    throw new UnauthorizedAccessException("Current user identifier was not found");

                return currentUserId;
            }
        }
    }
}
