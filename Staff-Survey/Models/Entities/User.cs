using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Staff_Survey.Models.Entities
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "نام و نام خانوادگی را وارد نمایید")]
        public String FullName { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; }
    }
}
