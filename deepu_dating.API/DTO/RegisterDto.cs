using System;
using System.ComponentModel.DataAnnotations;

namespace deepu_dating.API.DTO
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string KnownAS { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }
        
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(8, MinimumLength=4,ErrorMessage="Password should be between 4 to 8 length")]
        public string Password { get; set; }


    }
}