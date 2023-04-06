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
        private readonly ILogger<AuthController> logger;
        //makes it easy to access appsettings
        public readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;


        public AuthController(IUserRepository userRepository,UserManager<AppUsers> userManager, RoleManager<AppRoles> roleManager, ILogger<AuthController> logger, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
            this.configuration = configuration;
            this.userRepository = userRepository;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpViewModel model)
        {
            var user = await userRepository.SignUp(model);
            if (user != null)
            {
                return Ok("User successfully created" );
            }
            return BadRequest ("Kindly input all fields correctly");
           

            //if (string.Compare(model.Password, model.ConfirmPassword) != 0)
            //{
            //    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel { Success=false,Message= "Password mismatched!"});
            //}
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {

            //checking if user email exit and password matches the provided email
            var user = await userManager.FindByNameAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                };

                new Claim(ClaimTypes.Name, user.Email);
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());


                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));


                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT.Audience"],
                    expires: DateTime.Now.AddMinutes(20),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    Message = "Login successful"

                });
            }
            return Unauthorized("Username or password incorrect");
        }

        [HttpPost]
        [Route("reset-password")]

        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await userRepository.ResetPassword(model);
            if (user != null)
            {

            }
            return Unauthorized("User ");
        }

        [HttpPost]
        [Route("forget-password")]

        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            var user = await userRepository.ForgetPassword(model);
            if(user != null)
            {
                return Ok(user);
            }
            return NotFound("User not found,kindly enter correct email address!");

        }


        [HttpPost]
        [Route("change-password")]

        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await userRepository.ChangePassword(model);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound("User not found,kindly enter correct email address!");

        }
    }
}
