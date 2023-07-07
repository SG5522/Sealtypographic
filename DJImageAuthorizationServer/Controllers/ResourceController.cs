using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using DJImageAuthorizationServer.Entities;

namespace DJImageAuthorizationServer.Controllers;

[Route("api")]
public class ResourceController : Controller
{
    private readonly UserManager<ApplicationUser> userManager;

    public ResourceController(UserManager<ApplicationUser> userManager)
        => this.userManager = userManager;

    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [HttpGet("message")]
    public async Task<IActionResult> GetMessage()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest();
        }

        return Content($"{user.UserName} has been successfully authenticated.");
    }
}
