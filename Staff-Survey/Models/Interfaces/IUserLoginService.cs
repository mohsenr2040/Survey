using Microsoft.AspNetCore.Identity;
using Staff_Survey.Common;
using Staff_Survey.Models.Dtos;
using Staff_Survey.Models.Entities;

namespace Staff_Survey.Models.Interfaces
{
    public interface IUserLoginService
    {
        ResultDto<ResultUserLoginDto> SignIn(UserLoginDto request);
        void SignOut();
    }

    public class UserLoginService : IUserLoginService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public UserLoginService(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public ResultDto<ResultUserLoginDto> SignIn(UserLoginDto request)
        {
          
            //var resultUser2 = _userManager.CreateAsync(User3, "Yr1234!").Result;


            if (String.IsNullOrWhiteSpace(request.UserName) || String.IsNullOrWhiteSpace(request.Password))
            {
                return new ResultDto<ResultUserLoginDto>()
                {
                    Data = new ResultUserLoginDto()
                    {

                    },
                    Message = "نام کاربری و رمز عبور را وارد نمایید",
                    IsSuccess = false
                };
            }
            var User1 = _userManager.FindByNameAsync(request.UserName).Result;

            if (User1 == null)
            {
                return new ResultDto<ResultUserLoginDto>()
                {
                    Data = new ResultUserLoginDto()
                    {

                    },
                    IsSuccess = false,
                    Message = "کاربر وجود ندارد",
                };
            }

            _signInManager.SignOutAsync();

            var ResultSingIn = _signInManager.PasswordSignInAsync
                        (User1, request.Password, request.IsPersistent, true).Result;

            if (!ResultSingIn.Succeeded)
            {
                return new ResultDto<ResultUserLoginDto>()
                {
                    Data = new ResultUserLoginDto()
                    {

                    },
                    IsSuccess = false,
                    Message = "رمز وارد شده اشتباه است!",
                };
            }

          
            return new ResultDto<ResultUserLoginDto>()
            {
                Data = new ResultUserLoginDto()
                {
                    FullName = User1.FullName,
                    UserId = User1.Id,
                    SignInResult = ResultSingIn,
                },
                IsSuccess = true,
                Message = "ورود به سایت با موفقیت انجام شد",
            };


        }

        public void SignOut()
        {
            _signInManager.SignOutAsync();
        }
    }
}
