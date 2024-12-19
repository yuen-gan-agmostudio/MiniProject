using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("course")]
    public class CourseController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<CourseModel>), 200)]
        public IActionResult GetCourseList(int page = 1, int take = 20)
        {
            try
            {
                if (ModelState.IsValid) 
                {
                    var courseList = CourseServiceManager.GetCourseList(new FilterModel() { Page = page, Take = take});
                    return Ok(courseList);
                }
                else
                {
                    return BadRequest(ModelState.ToString());
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CourseModel), 200)]
        public IActionResult GetCourse([FromRoute] int id)
        {
            try
            {
                var course = CourseServiceManager.GetCourse(id);
                if (course == null) return BadRequest("Not found");
                return Ok(course);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("")]
        public IActionResult CreateCourse([FromBody] CourseModel model)
        {
            try
            {
                CourseServiceManager.CreateCourse(model);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        [ProducesResponseType(typeof(CourseModel), 200)]
        public IActionResult UpdateCourse([FromRoute]int id, [FromBody] CourseModel model)
        {
            try
            {
                model.Id = id;
                var updatedCourse = CourseServiceManager.UpdateCourse(model);
                if (updatedCourse == null) return BadRequest("Not found");
                return Ok(updatedCourse);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        public IActionResult DeleteCourse([FromRoute] int id)
        {
            try
            {
                CourseServiceManager.DeleteCourse(id);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("{id}/user")]
        [ProducesResponseType(typeof(List<UserCourseModel>), 200)]
        public IActionResult GetCourseUserList([FromRoute] int id, int page = 1, int take = 20)
        {
            try
            {
                var users = CourseServiceManager.GetCourseUserList(new FilterModel()
                {
                    Page = page,
                    Take = take,
                    CourseId = id
                });
                return Ok(users);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("{id}/user/{userId}")]
        [ProducesResponseType(typeof(bool), 200)]
        public IActionResult ApproveEnrollment([FromRoute] int id, [FromRoute] string userId, bool isApprove)
        {
            try
            {
                var result = CourseServiceManager.ApproveCourseEnrollment(new UserCourseModel()
                {
                    UserId = userId,
                    CourseId = id,
                    IsApproved = isApprove,
                    ApprovedBy = User.Identity?.Name
                });
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
