using Domain.ViewModel;
using Domain.ViewModel.RegisterViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.Interface
{
    public interface IUserAuthRepository
    {
        Task<ResponseViewModel> SignUp(SignUpViewModel model);
        Task<ResponseViewModel> Login(LoginViewModel model);
        Task<ResponseViewModel> ForgetPassword(ForgetPasswordViewModel model);
        Task<ResponseViewModel> ResetPassword(ResetPasswordViewModel model);
        Task<ResponseViewModel> ChangePassword(ChangePasswordViewModel model);
        Task<ResponseViewModel> DeleteProfile(Guid Id);
       // Task<ResponseViewModel> ConfirmEmail(string token, string email);



    }
}
