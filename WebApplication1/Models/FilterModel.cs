namespace WebApplication1.Models
{
    public class FilterModel
    {
        public int Page {  get; set; }
        public int Take { get; set; }
        public string? UserId { get; set; }
        public int? CourseId { get; set; }
    }
}
