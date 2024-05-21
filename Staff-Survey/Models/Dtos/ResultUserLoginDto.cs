using Microsoft.AspNetCore.Identity;

namespace Staff_Survey.Models.Dtos
{
    public class ResultUserLoginDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }

        public List<string> Roles { get; set; }

        public SignInResult SignInResult { get; set; }
    }
}
