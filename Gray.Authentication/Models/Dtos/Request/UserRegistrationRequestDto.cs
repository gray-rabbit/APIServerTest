using System.ComponentModel.DataAnnotations;

namespace Gray.Authentication.Models.Dtos.Request{
    public class UserRegistrationRequestDto{
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}