using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface ICourseServiceManager
    {
        List<CourseModel> GetCourseList(FilterModel filterModel);
        CourseModel? GetCourse(int id);
        void CreateCourse(CourseModel model);
        CourseModel? UpdateCourse(CourseModel model);
        void DeleteCourse(CourseModel model);
        bool EnrollCourse(UserCourseModel model);
        bool WithdrawCourse(UserCourseModel model);
        List<UserCourseModel> GetCourseUserList(FilterModel filterModel);
        bool ApproveCourseEnrollment(UserCourseModel model);
    }
}
