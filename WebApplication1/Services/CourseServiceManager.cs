using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using WebApplication1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Services
{
    public class CourseServiceManager : ICourseServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseServiceManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<CourseModel> GetCourseList(FilterModel filterModel)
        {
            var query = _unitOfWork.GetRepository<CourseModel>().Table.Where(x => x.DeletedDate == null).AsQueryable();

            var skip = (filterModel.Page - 1) * (filterModel.Take);
            var courses = query.Skip(skip).Take(filterModel.Take).ToList();
            return courses;
        }
        public CourseModel? GetCourse(int id)
        {
            return _unitOfWork.GetRepository<CourseModel>().Table.Where(x => x.Id == id).FirstOrDefault();
        }
        public void CreateCourse(CourseModel model)
        {
            var newCourse = new CourseModel()
            {
                Name = model.Name,
                Description = model.Description,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = model.CreatedBy
            };
            _unitOfWork.GetRepository<CourseModel>().Insert(newCourse);
        }
        public CourseModel? UpdateCourse(CourseModel model)
        {
            var course = _unitOfWork.GetRepository<CourseModel>().Table.Where(x => x.Id == model.Id).FirstOrDefault();
            if (course != null)
            {
                course.Name = string.IsNullOrEmpty(model.Name) ? course.Name : model.Name;
                course.Description = model.Description;
                course.ModifiedDate = DateTime.UtcNow;
                course.ModifiedBy = model.ModifiedBy;
                _unitOfWork.GetRepository<CourseModel>().Update(course);
            }
            return course;
        }
        public void DeleteCourse(CourseModel model)
        {
            var course = _unitOfWork.GetRepository<CourseModel>().Table.Where(x => x.Id == model.Id).FirstOrDefault();
            if (course != null)
            {
                course.DeletedDate = DateTime.UtcNow;
                course.DeletedBy = model.DeletedBy;
                _unitOfWork.GetRepository<CourseModel>().Update(course);
            }
        }
        public bool EnrollCourse(UserCourseModel model)
        {
            var course = _unitOfWork.GetRepository<CourseModel>().Table
                .Where(x => x.Id == model.CourseId
                && x.DeletedDate == null).FirstOrDefault();
            if (course == null) return false;

            var userCourse = _unitOfWork.GetRepository<UserCourseModel>().Table
                .Where(x => x.UserId == model.UserId
                && x.CourseId == model.CourseId
                && x.DeletedDate == null).FirstOrDefault();
            if (userCourse != null) return false;

            _unitOfWork.GetRepository<UserCourseModel>().Insert(new UserCourseModel()
            {
                UserId = model.UserId,
                CourseId = model.CourseId,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = model.CreatedBy
            });
            return true;
        }
        public bool WithdrawCourse(UserCourseModel model)
        {
            var course = _unitOfWork.GetRepository<CourseModel>().Table
                .Where(x => x.Id == model.CourseId
                && x.DeletedDate == null).FirstOrDefault();
            if (course == null) return false;

            var userCourse = _unitOfWork.GetRepository<UserCourseModel>().Table
                .Where(x => x.UserId == model.UserId
                && x.CourseId == model.CourseId
                && x.DeletedDate == null).FirstOrDefault();
            if (userCourse == null) return false;

            userCourse.DeletedDate = DateTime.UtcNow;
            userCourse.DeletedBy = model.DeletedBy;
            _unitOfWork.GetRepository<UserCourseModel>().Update(userCourse);
            return true;
        }
        public List<UserCourseModel> GetCourseUserList(FilterModel filterModel)
        {
            var query = _unitOfWork.GetRepository<UserCourseModel>().Table
                .Include(x => x.User)
                .Include(x => x.Course)
                .Where(x => (!filterModel.CourseId.HasValue || x.CourseId == filterModel.CourseId)
                && (string.IsNullOrEmpty(filterModel.UserId) || x.UserId == filterModel.UserId)
                && x.DeletedDate == null
                && x.Course != null && x.Course.DeletedDate == null)
                .AsQueryable();

            var skip = (filterModel.Page - 1) * (filterModel.Take);
            var courseUsers = query.Skip(skip).Take(filterModel.Take).ToList();

            return courseUsers;
        }
        public bool ApproveCourseEnrollment(UserCourseModel model)
        {
            var course = _unitOfWork.GetRepository<CourseModel>().Table
                .Where(x => x.Id == model.CourseId).FirstOrDefault();
            if (course == null) return false;

            var userCourse = _unitOfWork.GetRepository<UserCourseModel>().Table
                .Where(x => x.UserId == model.UserId && x.CourseId == model.CourseId).FirstOrDefault();
            if (userCourse == null) return false;

            userCourse.IsApproved = model.IsApproved;
            userCourse.ApprovedBy = model.IsApproved ? model.ApprovedBy : null;
            userCourse.ApprovedDate = model.IsApproved ? DateTime.UtcNow : null;
            userCourse.IsRejected = !model.IsApproved;
            userCourse.RejectedBy = !model.IsApproved ? model.ApprovedBy : null;
            userCourse.RejectedDate = !model.IsApproved ? DateTime.UtcNow : null;

            _unitOfWork.GetRepository<UserCourseModel>().Update(userCourse);
            return true;
        }
    }
}
