namespace deepu_dating.API.Models
{
    public class UserLike
    {
        public userData sourceUser { get; set; }

        public int sourceUserId { get; set; }

        public userData likedUser { get; set; }

        public int likedUserId { get; set; }
    }
}