using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class UserCourseModel
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public int CourseId { get; set; }
        public bool IsApproved { get; set; }
        public string? ApprovedBy { get; set; }
        public bool IsRejected { get; set; }
        public string? RejectedBy { get; set; }
        public IdentityUser? User { get; set; }
        public CourseModel? Course { get; set; }
    }
}
