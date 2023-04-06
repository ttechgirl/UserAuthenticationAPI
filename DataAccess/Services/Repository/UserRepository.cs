﻿using DataAccess.Services.Interface;
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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext dbContext;
        public UserManager<AppUsers> userManager;
        public RoleManager<AppRoles> roleManager;
        public readonly IConfiguration configuration;

        public UserRepository(AppDbContext dbContext, UserManager<AppUsers> userManager, RoleManager<AppRoles> roleManager,  IConfiguration configuration)
        {
           this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        public Task<ResponseViewModel> DeleteProfile(UserProfileViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseViewModel> Login(LoginViewModel model)
        {

            var response = new ResponseViewModel();
            throw new NotImplementedException();
        }

        public async Task<ResponseViewModel> ResetPassword(ResetPasswordViewModel model)
        {
            var user= await userManager.FindByNameAsync(model.Email);

            if(user != null )
            {
                var role = await userManager.GetRolesAsync(user);
                if(role != null)
                {
                    await userManager.ResetPasswordAsync(user, model.token, model.Password);
                    return new ResponseViewModel() { Success = true, Message = "Password successfully changed!" };
                }
            }
            return new ResponseViewModel() {Success=false,Message="User not found,kindly enter correct email address!" };
        }

        public async Task<ResponseViewModel> SignUp(SignUpViewModel model)
        {

            var user = new AppUsers()
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                // PasswordHash=model.ConfirmPassword,
                CreatedOn = DateTime.Now,
                SecurityStamp = DateTime.Now.ToString()
            };
           
            var result = await userManager.CreateAsync(user, model.Password);

            var role = await roleManager.FindByIdAsync(model.RoleId.ToString());
            if (role == null)
                return new ResponseViewModel() { Success = false, Message = "Role does not exist" };

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role.Name);

                return new ResponseViewModel (){ Success = true, Message = "User successfully created" };
            }
            return new ResponseViewModel() { Success = false, Message = "Kindly input all fields correctly" };
        }

        public async Task<ResponseViewModel> ForgetPassword(ForgetPasswordViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            if (user != null)
            {
                var role = await userManager.GetRolesAsync(user);
                if (role != null)
                    await userManager.GeneratePasswordResetTokenAsync(user);
                    return new ResponseViewModel() { Success = true, Message = "Reset password!" };
            }
            return new ResponseViewModel() { Success = false, Message = "User not found,kindly enter correct email address!" };
        }

        public Task<ResponseViewModel> ChangePassword(ResetPasswordViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
