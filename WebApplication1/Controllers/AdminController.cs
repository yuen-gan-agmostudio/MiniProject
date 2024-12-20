using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Route("user")]
        public async Task<IActionResult> GetUserList()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                return Ok(users);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("user/{id}")]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            try
            {
                var user = await _userManager.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (user == null) return BadRequest("Not found");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
