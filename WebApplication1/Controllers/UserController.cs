﻿using Microsoft.AspNetCore.Authorization;
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
        private readonly UserManager<UserModel> _userManager;
        private readonly ICourseServiceManager _courseService;

        public UserController(UserManager<UserModel> userManager, ICourseServiceManager courseService)
        {
            _userManager = userManager;
            _courseService = courseService;
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
        public async Task<IActionResult> UpdateUser([FromBody] UserInputModel model)
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

                var result = _courseService.GetCourseUserList(new FilterModel()
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
                    result = _courseService.EnrollCourse(new UserCourseModel()
                    {
                        UserId = user.Id,
                        CourseId = id,
                        CreatedBy = user.UserName
                    });
                }
                else
                {
                    result = _courseService.WithdrawCourse(new UserCourseModel()
                    {
                        UserId = user.Id,
                        CourseId = id,
                        DeletedBy = user.UserName
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
