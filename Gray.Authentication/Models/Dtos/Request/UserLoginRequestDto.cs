using System.ComponentModel.DataAnnotations;

namespace Gray.Authentication.Models.Dtos.Request{
    public class UserLoginRequestDto{
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}