using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;

        public AdminController(UserManager<UserModel> userManager)
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

        [HttpPost]
        [Route("user/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UserInputModel model)
        {
            try
            {
                var user = await _userManager.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (user == null) return BadRequest("Not found");

                if (model.IsRecover)
                {
                    user.DeletedDate = null;
                    user.DeletedBy = null;
                }
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.ModifiedDate = DateTime.UtcNow;
                user.ModifiedBy = User.Identity?.Name;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded) return Ok(result);
                else
                {
                    var errors = JsonConvert.SerializeObject(result.Errors);
                    return BadRequest(errors);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("user/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            try
            {
                var user = await _userManager.Users
                    .Where(x => x.Id == id 
                    && x.DeletedDate == null).FirstOrDefaultAsync();
                if (user == null) return BadRequest("Not found");

                user.DeletedDate = DateTime.UtcNow;
                user.DeletedBy = User.Identity?.Name;
                var result = await _userManager.UpdateAsync(user);
                if(result.Succeeded) return Ok(result);
                else
                {
                    var errors = JsonConvert.SerializeObject(result.Errors);
                    return BadRequest(errors);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("user/{id}/password")]
        public async Task<IActionResult> ChangeUserPassword([FromRoute] string id, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return BadRequest("Not found");

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (result.Succeeded)
                {
                    return Ok("Successful Password Change");
                }
                else
                {
                    var errors = JsonConvert.SerializeObject(result.Errors);
                    return BadRequest(errors);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
