using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace deepu_dating.API.Models
{
    [Table("Photos")]
    public class Photo
    {

        public int id { get; set; }

        public string Url { get; set; }

        public bool IsMain { get; set; }

        public string PublicId { get; set; }
        
        [ForeignKey("userId")]
        public userData userData { get; set; }

        public int userId { get; set; }
    }
}