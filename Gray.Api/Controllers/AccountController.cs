using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gray.Authentication.Configuration;
using Gray.Authentication.Models.Dtos.Request;
using Gray.Authentication.Models.Dtos.Response;
using Gray.DataService.IConfiguration;
using Gray.DataService.IRepository;
using Gray.Entities.DbSets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gray.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<IdentityUser> userManager;
        private readonly JwtConfig jwtConfig;
        private IUserRepository userRepository;
        public AccountController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionMonitor)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = unitOfWork.Users;
            this.userManager = userManager;
            this.jwtConfig = optionMonitor.CurrentValue;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto user)
        {
            if (ModelState.IsValid)
            {
                var userExist = await userManager.FindByEmailAsync(user.Email);
                if (userExist == null)
                {
                    return BadRequest(new UserLoginResponseDto()
                    {
                        Success = false,
                        Errors = new() { "not exist User" }
                    });
                }

                var checkedResult = await userManager.CheckPasswordAsync(userExist, user.Password);
                if (!checkedResult)
                {
                    return BadRequest(new UserLoginResponseDto
                    {
                        Success = false,
                        Errors = new() { "Password is not valid" }
                    });
                }

                var token = GenerateJwtToken(userExist);

                return Ok(new UserLoginResponseDto
                {
                    Success = true,
                    Token = token,
                });
            }
            else
            {
                return BadRequest(new UserLoginResponseDto
                {
                    Success = false,
                    Errors = new List<string>(){
                        "안맞음"
                    }
                });
            }
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto user)
        {
            if (ModelState.IsValid)
            {
                //있는지 검사하기
                var userExist = await userManager.FindByEmailAsync(user.Email);
                if (userExist != null)
                {
                    return BadRequest(new UserRegistrationResponseDto
                    {
                        Success = false,
                        Errors = new() { "Already Exist User" }
                    });
                }

                var newUser = new IdentityUser()
                {
                    Email = user.Email,
                    UserName = user.Email,
                    EmailConfirmed = true, //todo 이메일 확인 부분 만들것

                };

                var isCreated = await userManager.CreateAsync(newUser, user.Password);
                if (!isCreated.Succeeded)
                {
                    return BadRequest(new UserRegistrationResponseDto
                    {
                        Success = false,
                        Errors = isCreated.Errors.Select(x => x.Description).ToList()
                    });
                }
                var _user = new User()
                {
                    IdentityId = new Guid(newUser.Id),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    DateOfBirth = DateTime.UtcNow,
                    Phone = "",
                    Country = "",
                    Status = 1,
                    AddedDate = DateTime.UtcNow
                };
                Console.WriteLine("시작");
                await unitOfWork.Users.Add(_user);
                await unitOfWork.CompleteAsync();

                var token = GenerateJwtToken(newUser);

                return Ok(new UserRegistrationResponseDto()
                {
                    Success = true,
                    Token = token,
                });
            }
            else
            {
                return BadRequest(new UserRegistrationResponseDto()
                {
                    Success = false,
                    Errors = new() { "InValid Payload" }
                });
            }

        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(
                    new[]{
                       new Claim("Id", user.Id),
                       new Claim(JwtRegisteredClaimNames.Sub , user.Email),
                       new Claim(JwtRegisteredClaimNames.Email, user.Email),
                       new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    }
                ),
                Expires = DateTime.UtcNow.AddHours(1.0),
                Issuer = "GrayRabbiT",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            };
            var token = jwtHandler.CreateToken(tokenDescriptor);
            var tokenString = jwtHandler.WriteToken(token);
            return tokenString;
        }
    }
}