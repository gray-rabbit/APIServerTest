using Gray.DataService.Data;
using Gray.DataService.IConfiguration;
using Gray.Entities.DbSets;
using Gray.Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Gray.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // private readonly AppDbContext context;
        private readonly IUnitOfWork unitOfWork;

        public UserController(AppDbContext context, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await unitOfWork.Users.All());
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(RequestUserDto user)
        {
            var _user =
                new User()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Country = user.Country,
                    Phone = user.Phone,
                    Status = 1,
                    DateOfBirth = Convert.ToDateTime(user.DateOfBirth),
                    AddedDate = DateTime.UtcNow
                };

            await unitOfWork.Users.Add(_user);
            await unitOfWork.CompleteAsync();
            return CreatedAtRoute("GetUser", _user.Id, user);
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(Guid id){
            return Ok(await unitOfWork.Users.GetById(id));
        }
    }
}
