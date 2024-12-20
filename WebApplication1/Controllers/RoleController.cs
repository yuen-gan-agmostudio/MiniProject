using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("role")]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<IdentityRole>), 200)]
        public async Task<IActionResult> GetRoleList()
        {
            try
            {
                var roleList = await _roleManager.Roles.ToListAsync();
                return Ok(roleList);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(IdentityRole), 200)]
        public async Task<IActionResult> GetRole([FromRoute] string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) return BadRequest("Not found");
                return Ok(role);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateRole([FromBody] RoleModel model)
        {
            try
            {
                bool roleExists = await _roleManager.RoleExistsAsync(model.Name);
                if (roleExists) return BadRequest("Role exists");
                else
                {
                    var identityRole = new IdentityRole() 
                    { 
                        Name = model.Name 
                    };
                    IdentityResult result = await _roleManager.CreateAsync(identityRole);
                    if (result.Succeeded) return Ok(result);
                    else 
                    {
                        var errors = JsonConvert.SerializeObject(result.Errors);
                        return BadRequest(errors);
                    } 
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> UpdateRole([FromRoute] string id, [FromBody] RoleModel model)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) return BadRequest("Not found");

                role.Name = model.Name;
                IdentityResult result = await _roleManager.UpdateAsync(role);
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
        [Route("{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) return BadRequest("Not found");

                IdentityResult result = await _roleManager.DeleteAsync(role);
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

        [HttpGet]
        [Route("{id}/user")]
        [ProducesResponseType(typeof(List<UserModel>), 200)]
        public async Task<IActionResult> GetRoleUserList([FromRoute] string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) return BadRequest("Not found");
                string roleName = string.IsNullOrEmpty(role.Name) ? string.Empty : role.Name;

                var roleUserList = new List<UserModel>();

                foreach (var user in await _userManager.Users.ToListAsync())
                {
                    if (await _userManager.IsInRoleAsync(user, roleName))
                    {
                        roleUserList.Add(user);
                    }
                }

                return Ok(roleUserList);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("{id}/user")]
        public async Task<IActionResult> AddUserToRole([FromRoute] string id, string userId, bool isAdd)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) return BadRequest("Role not found");
                string roleName = string.IsNullOrEmpty(role.Name) ? string.Empty : role.Name;

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return BadRequest("User not found");

                IdentityResult? result;

                if (isAdd) result = await _userManager.AddToRoleAsync(user, roleName); 
                else result = await _userManager.RemoveFromRoleAsync(user, roleName);

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
    }
}
