using DataAccess.Services;
using DataAccess.Services.Interface;
using Domain.Models;
using Domain.ViewModel;
using Domain.ViewModel.RegisterViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UserRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public UserManager<AppUsers> userManager;
        public RoleManager<AppRoles> roleManager;
        public readonly IConfiguration configuration;
        private readonly IUserAuthRepository userRepository;

        public AuthController(IUserAuthRepository userRepository,IConfiguration configuration,RoleManager<AppRoles> roleManager, UserManager<AppUsers> userManager)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userRepository.SignUp(model);
                if (user.Success)
                {
                    var role = await roleManager.FindByIdAsync(model.RoleId.ToString());
                    if (role == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel { Success = false, Message = "Role does not exist" });

                    }
                    return Ok(user);
                }
                return BadRequest("Kindly input all fields correctly");
            }
            return BadRequest();
           

            //if (string.Compare(model.Password, model.ConfirmPassword) != 0)
            //{
            //    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel { Success=false,Message= "Password mismatched!"});
            //}
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {

            //var user = await userManager.FindByNameAsync(model.Email);
            var user = await userRepository.Login(model);
            if (user.Success)
            {
                //var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim> { };
               
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT.Audience"],
                    expires: DateTime.Now.AddMinutes(10),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                });
            }
            return Unauthorized(user);
        }

        
        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordViewModel model)
        {
            var user = await userRepository.ResetPassword(model);

            if (user.Success)
            {
                return Ok(user);
            }
            return NotFound(user);
        }

        [HttpPost]
        [Route("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordViewModel model)
        {
            var user = await userRepository.ForgetPassword(model);
            if(user.Success)
            {
                return Ok(user);
            }
            return NotFound(user);

        }


        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            var user = await userRepository.ChangePassword(model);
            if (user.Success)
            {
                return Ok(user);
            }
            return NotFound(user);

        }

        [HttpDelete]
        [Route("delete-profile")]
        public async Task<IActionResult> DeleteProfile(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel { Success = false, Message = "Id cannot be empty!" });
            }
            var user = await userRepository.DeleteProfile(Id);
            if(user.Success )
            {
                return Ok(user);
            }
            return BadRequest(user);
        }




    }
}
