using System.ComponentModel.DataAnnotations;


namespace deepu_dating.API.DTO
{
    public class AuthDTO
    {
        [Required]
        public string user { get; set; }
        
        [StringLength(8, MinimumLength=4,ErrorMessage="Password should be between 4 to 8 length")]
        public string password { get; set; }
    }
}