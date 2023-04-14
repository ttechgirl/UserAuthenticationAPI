using DataAccess.Services.Interface;
using Domain.DbContext;
using Domain.Models;
using Domain.ViewModel;
using Domain.ViewModel.RegisterViewModel;
using Microsoft.AspNetCore.Identity;
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
    public class UserAuthRepository : IUserAuthRepository
    {
        private readonly AppDbContext dbContext;
        public UserManager<AppUsers> userManager;
        public RoleManager<AppRoles> roleManager;
        public readonly IConfiguration configuration;

        public UserAuthRepository(AppDbContext dbContext, UserManager<AppUsers> userManager, RoleManager<AppRoles> roleManager,  IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        public async Task<ResponseViewModel> SignUp(SignUpViewModel model)
        {
            var response = new ResponseViewModel();
            var user = new AppUsers()
            {
                UserName = model.Email,
                LastName = model.LastName,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                Email = model.Email,
                PhoneNumber= model.PhoneNumber,
                CreatedOn = DateTime.Now,
                SecurityStamp = DateTime.Now.ToString()
            };

            //var existUser = dbContext.Set<AppUsers>().FirstOrDefault(x => x.Email == model.Email);
            var existUser = userManager.Users.FirstOrDefault(x => x.Email == model.Email);
            if (existUser!=null)
            {
                return new ResponseViewModel() { Success = false, Message = "Email exist ,kindly use a different email" };
            }
            var result = await userManager.CreateAsync(user, model.Password);

            var role = await roleManager.FindByIdAsync(model.RoleId.ToString());

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role.Name);
                return new ResponseViewModel() { Success = true, Message = "User successfully created" };
            }
            return new ResponseViewModel() { Success = false, Message = "Kindly input all fields correctly" };

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
                return new ResponseViewModel() { Success = true, Message = "Login successful" };

            }
                return new ResponseViewModel() { Success = false, Message = "Username or password incorrect" };
        }

        public async Task<ResponseViewModel> ForgetPassword(ForgetPasswordViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            if (user != null && !user.IsDeleted)
            {
                var role = await userManager.GetRolesAsync(user);
                if (role != null)
                    return new ResponseViewModel() { Success = true, Message = "Reset password!" };
            }
            return new ResponseViewModel() { Success = false, Message = "User not found,kindly enter correct email address!" };
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
                    await userManager.ResetPasswordAsync(user, model.token=token, model.Password);
                    return new ResponseViewModel() { Success = true, Message = "Password reset successfully" };
                }
            }
            return new ResponseViewModel() { Success = false, Message = "User not found,kindly enter correct email address!" };
        }

        public async Task<ResponseViewModel> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            if(user != null && !user.IsDeleted)
            {
                await userManager.ChangePasswordAsync(user,model.CurrentPassword,model.NewPassword);
                return new ResponseViewModel() { Success = true, Message = "Password successfully changed" };
            }
            return new ResponseViewModel() { Success = false, Message = "User not found,kindly enter correct email address!" };
        }

        public async Task<ResponseViewModel> DeleteProfile(Guid Id)
        {
            //var existUser = await dbContext.Set<AppUsers>().FindAsync(Id);
            var existUser = userManager.Users.FirstOrDefault(x => x.Id == Id);

            if (existUser ==null)
            {
              return new ResponseViewModel() { Success = false, Message = "User not found!" };

            }
            existUser.IsDeleted = true;
            existUser.DeletedBy = existUser.UserName;
            existUser.DeletedOn = DateTime.UtcNow;

            await userManager.UpdateAsync(existUser);
            return new ResponseViewModel() { Success = true, Message = "Profile deleted successfully" };

        }
    }
}
