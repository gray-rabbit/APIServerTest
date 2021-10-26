using Gray.DataService.Data;
using Gray.Entities.DbSets;
using Gray.Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Gray.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext context;

        public UserController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = context.Users.ToList();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(RequestUserDto user)
        {
            var _user = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Country = user.Country,
                Phone = user.Phone,
                Status = 1,
                DateOfBirth = Convert.ToDateTime(user.DateOfBirth),
                AddedDate = DateTime.UtcNow,
            };



            context.Users.Add(_user);
            await context.SaveChangesAsync();
            return Ok(_user);
        }
    }
}