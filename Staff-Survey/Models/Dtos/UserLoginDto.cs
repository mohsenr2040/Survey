using System.ComponentModel.DataAnnotations;

namespace Staff_Survey.Models.Dtos
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "ورود نام کاربری الزامی است")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "ورود رمزعبور الزامی است")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
    }
}
