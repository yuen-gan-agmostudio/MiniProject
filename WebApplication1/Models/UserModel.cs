using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class UserModel : IdentityUser
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? CreatedBy {  get; set; }
        public string? ModifiedBy { get; set; }
        public string? DeletedBy { get; set; }

    }
}
