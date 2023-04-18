using DataAccess.Services.Interface;
using Domain.DbContext;
using Domain.Models;
using Domain.ViewModel;
using Domain.ViewModel.RegisterViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.Repository
{
    public class UserAuthRepository : PasswordValidator<AppUsers>, IUserAuthRepository 
    {
        public UserManager<AppUsers> userManager;
        public RoleManager<AppRoles> roleManager;
        public readonly IConfiguration configuration;
        public readonly IEmailService emailService;


        public UserAuthRepository(AppDbContext dbContext, UserManager<AppUsers> userManager, RoleManager<AppRoles> roleManager,  IConfiguration configuration, IEmailService emailService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.emailService = emailService;   
        }

        public async Task<ResponseViewModel> SignUp(SignUpViewModel model)
        {
            var user = new AppUsers()
            {
                UserName = model.Email,
                LastName = model.LastName,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                Email = model.Email,
                EmailConfirmed = false,
                PhoneNumber= model.PhoneNumber,
                CreatedOn = DateTime.Now,
                SecurityStamp = DateTime.Now.ToString()
                
            };


            //validates the password according to the rules in the ConfigureServices()
            var validate = await base.ValidateAsync(userManager, user, model.Password);
            List<IdentityError> errors = validate.Succeeded ? new List<IdentityError>() : validate.Errors.ToList();

            //var existUser = dbContext.Set<AppUsers>().FirstOrDefault(x => x.Email == model.Email);
            var existUser = userManager.Users.FirstOrDefault(x => x.Email == model.Email);
            if (existUser!=null)
            {
                return new ResponseViewModel() { Success = false, Message = "Email exist ,kindly use a different email" };
            }

            var role = await roleManager.FindByIdAsync(model.RoleId.ToString());
            if (model.RoleId == Guid.Empty)
            {
                return new ResponseViewModel { Success = false, Message = "Role does not exist" };

            }
            if (role == null)
            {
                return new ResponseViewModel { Success = false, Message = "Role does not exist" };

            }
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role.Name);
                return new ResponseViewModel { Success = true, Message =$"Profile successfully created and verification email has been sent to {user.Email}"};
            }
            return new ResponseViewModel { Success = false, Message = "Kindly input all fields correctly" };

        }

        public async Task<ResponseViewModel> Login(LoginViewModel model)
        {
            //checking if user email exit and password matches the provided email

            var user = await userManager.FindByNameAsync(model.Email);
            if(user != null&& !user.IsDeleted && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles =await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                };

                new Claim(ClaimTypes.Name, user.Email);
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());


                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                return new ResponseViewModel { Success = true, Message = "Login successful" };

            }
                return new ResponseViewModel { Success = false, Message = $"User with the email {user.Email} not found" };
        }

        public async Task<ResponseViewModel> ForgetPassword(ForgetPasswordViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            if (user != null && !user.IsDeleted)
            {
                var role = await userManager.GetRolesAsync(user);
                if (role != null)
                {
                    return new ResponseViewModel { Success = true, Message = $"Password reset link has been sent to {user.Email}" };

                }
            }
            return new ResponseViewModel { Success = false, Message = "User not found,kindly enter correct email address!" };
        }

        public async Task<ResponseViewModel> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            if (user != null && !user.IsDeleted)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                var role = await userManager.GetRolesAsync(user);
                if (role != null)
                {
                    await userManager.ResetPasswordAsync(user, model.Token=token, model.Password);
                    return new ResponseViewModel{ Success = true, Message = "Password reset successfully" };
                }
            }
            return new ResponseViewModel { Success = false, Message = "User not found,kindly enter correct email address!" };
        }

        public async Task<ResponseViewModel> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            if(user != null && !user.IsDeleted)
            {
                await userManager.ChangePasswordAsync(user,model.CurrentPassword,model.NewPassword);
                return new ResponseViewModel { Success = true, Message = "Password successfully changed" };
            }
            return new ResponseViewModel { Success = false, Message = "User not found,kindly enter correct email address!" };
        }

        public async Task<ResponseViewModel> DeleteProfile(Guid Id)
        {
            //var existUser = await dbContext.Set<AppUsers>().FindAsync(Id);
            var existUser = userManager.Users.FirstOrDefault(x => x.Id == Id);

            if (existUser ==null)
            {
              return new ResponseViewModel { Success = false, Message = "User not found!" };

            }
            existUser.IsDeleted = true;
            existUser.DeletedBy = existUser.UserName;
            existUser.DeletedOn = DateTime.UtcNow;

            await userManager.UpdateAsync(existUser);
            return new ResponseViewModel { Success = true, Message = "Profile deleted successfully" };

        }

        //public async Task<ResponseViewModel> ConfirmEmail(string token, string email)
        //{
        //    var user = await userManager.FindByEmailAsync(email);
        //    if(user != null && !user.IsDeleted)
        //    {
        //        var result = await userManager.ConfirmEmailAsync(user, token);
        //        if (result.Succeeded)
        //        {
        //            return new ResponseViewModel { Success = true,Message="Email verification sent"};

        //        }
        //    }
        //    return new ResponseViewModel { Success = true, Message = "Enter correct email address" };

        //}
    }
}
