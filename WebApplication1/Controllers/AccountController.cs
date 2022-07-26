using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetUser()
        {
            var claims = HttpContext.User.Claims;
            var userId = claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name))?.Value;
            return Ok(userId);
        }

        [HttpGet]
        public async Task<ActionResult> Login([FromQuery] string userId)
        {
            if (!userId.Equals("abc"))
            {
                return Unauthorized();
            }

            var claims = new List<Claim>() { new Claim(ClaimTypes.Name, userId) };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
