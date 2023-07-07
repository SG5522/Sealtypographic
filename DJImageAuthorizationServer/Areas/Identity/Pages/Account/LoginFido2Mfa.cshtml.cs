using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DJImageAuthorizationServer.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginFido2Mfa : PageModel
{
    [BindProperty(SupportsGet = true)]
    public bool RememberMe { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public void OnGet()
    {
    }

    public void OnPost()
    {
    }
}
