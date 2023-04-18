using DataAccess.Services;
using DataAccess.Services.Interface;
using Domain.Models;
using Domain.ViewModel;
using Domain.ViewModel.RegisterViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
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
        public readonly IEmailService emailService;


        public AuthController(IUserAuthRepository userRepository,IConfiguration configuration,RoleManager<AppRoles> roleManager, UserManager<AppUsers> userManager, IEmailService emailService)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.emailService = emailService;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUsers()
                {
                    UserName = model.Email,
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    Email = model.Email,
                    EmailConfirmed = false,
                    PhoneNumber = model.PhoneNumber,
                    CreatedOn = DateTime.Now,
                    SecurityStamp = DateTime.Now.ToString()
                };

                var userService = await userRepository.SignUp(model);
                if (userService !=null)
                {
                    //Add token to verify email
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = user.Email }, Request.Scheme);
                    var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink);
                    emailService.SendEmail(message);
                    return Ok(userService);
                }
                return BadRequest(userService);
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
            var userService = await userRepository.Login(model);
            if (userService.Success)
            {
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
            return Unauthorized(userService);
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordViewModel model)
        {
            var userService = await userRepository.ResetPassword(model);

            if (userService.Success)
            {
                return Ok(userService);
            }
            return NotFound(userService);
        }

        [HttpPost]
        [Route("forget-password")] //email confirmation to be integrated
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordViewModel model)
        {
            var user = new AppUsers()
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = false,
                CreatedOn = DateTime.Now,
                SecurityStamp = DateTime.Now.ToString()
            };

            var userService = await userRepository.ForgetPassword(model);
            if(userService.Success)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink);
                emailService.SendEmail(message);
                return Ok(userService);
            }
            return NotFound(userService);

        }

        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            var userService = await userRepository.ChangePassword(model);
            if (userService.Success)
            {
                return Ok(userService);
            }
            return NotFound(userService);

        }

        [HttpDelete]
        [Route("delete-profile")]
        public async Task<IActionResult> DeleteProfile(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel { Success = false, Message = "Id cannot be empty!" });
            }
            var userService = await userRepository.DeleteProfile(Id);
            if(userService.Success )
            {
                return Ok(userService);
            }
            return BadRequest(userService);
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                  return StatusCode(StatusCodes.Status200OK, new ResponseViewModel { Success = true, Message = "Email verification sent" });

                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel { Success = false, Message = "Enter a valid email address" });

        }

    }
}
