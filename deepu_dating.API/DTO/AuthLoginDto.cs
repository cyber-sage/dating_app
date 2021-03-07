using System.ComponentModel.DataAnnotations;



namespace deepu_dating.API.DTO
{
    public class AuthLoginDto
    {
        [Required]
        public string username { get; set; }

        [Required]
       // [StringLength(8, MinimumLength=4,ErrorMessage="Password should be between 4 to 8 length")]
        public string password { get; set; }
    }
}