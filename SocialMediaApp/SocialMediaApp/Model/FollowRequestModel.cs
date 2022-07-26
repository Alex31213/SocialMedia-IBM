using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Model
{
    public class FollowRequestModel
    {
        [Required(ErrorMessage = "Both users are required!")]
        public int FirstUserId { get; set; }

        [Required(ErrorMessage = "Both users are required!")]
        public int SecondUserId { get; set; }
    }
}
