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
        private readonly ICourseServiceManager _courseService;

        public CourseController(ICourseServiceManager courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<CourseModel>), 200)]
        public IActionResult GetCourseList(int page = 1, int take = 20)
        {
            try
            {
                if (ModelState.IsValid) 
                {
                    var courseList = _courseService.GetCourseList(new FilterModel() { Page = page, Take = take });
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
                var course = _courseService.GetCourse(id);
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
                model.CreatedBy = User.Identity?.Name;
                _courseService.CreateCourse(model);
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
                model.ModifiedBy = User.Identity?.Name;
                var updatedCourse = _courseService.UpdateCourse(model);
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
                var model = new CourseModel() 
                { 
                    Id = id,
                    DeletedBy = User.Identity?.Name
                };
                _courseService.DeleteCourse(model);
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
                var users = _courseService.GetCourseUserList(new FilterModel()
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
                var result = _courseService.ApproveCourseEnrollment(new UserCourseModel()
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
