using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return BadRequest("Invalid login");
                }

                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> UpdateUser([FromBody] UserModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return BadRequest("Invalid login");
                }

                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
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

        [HttpGet]
        [Authorize]
        [Route("course")]
        [ProducesResponseType(typeof(List<UserCourseModel>), 200)]
        public async Task<IActionResult> GetEnrolledCourseList(int page = 1, int take = 20)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return BadRequest("Invalid login");

                var result = CourseServiceManager.GetCourseUserList(new FilterModel() 
                { 
                    Page = page, 
                    Take = take, 
                    UserId = user.Id 
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("course/{id}")]
        public async Task<IActionResult> EnrollCourse([FromRoute] int id, bool isEnroll)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return BadRequest("Invalid login");
                }

                bool result = false;

                if (isEnroll)
                {
                    result = CourseServiceManager.EnrollCourse(new UserCourseModel()
                    {
                        UserId = user.Id,
                        CourseId = id
                    });
                }
                else
                {
                    result = CourseServiceManager.WithdrawCourse(new UserCourseModel()
                    {
                        UserId = user.Id,
                        CourseId = id
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
