using Domain.ViewModel;
using Domain.ViewModel.RegisterViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.Interface
{
    public interface IUserRepository
    {
        Task<ResponseViewModel> SignUp(SignUpViewModel model);
        Task<ResponseViewModel> Login(LoginViewModel model);
        Task<ResponseViewModel> ForgetPassword(ForgetPasswordViewModel model);
        Task<ResponseViewModel> ResetPassword(ResetPasswordViewModel model);
        Task<ResponseViewModel> ChangePassword(ResetPasswordViewModel model);
        Task<ResponseViewModel> DeleteProfile(UserProfileViewModel model);




    }
}
