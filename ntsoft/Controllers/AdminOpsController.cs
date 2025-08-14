using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ntsoft.Model;

namespace ntsoft.Controllers;

[ApiController]
[Route("api/adminops")]
[Authorize(Roles = "Admin")]
public class AdminOpsController(UserManager<AppUser> userMgr, RoleManager<IdentityRole> roleMgr) : ControllerBase
{
    [HttpPost("promote")]
    public async Task<IActionResult> PromoteToAdmin([FromBody] PromoteRequest req)
    {
        var user = await userMgr.FindByNameAsync(req.UserName);
        if (user is null) return NotFound("user not found");
        if (!await roleMgr.RoleExistsAsync("Admin"))
            await roleMgr.CreateAsync(new IdentityRole("Admin"));
        var res = await userMgr.AddToRoleAsync(user, "Admin");
        return res.Succeeded ? Ok("promoted") : BadRequest(res.Errors);
    }
}

public record PromoteRequest(string UserName);
