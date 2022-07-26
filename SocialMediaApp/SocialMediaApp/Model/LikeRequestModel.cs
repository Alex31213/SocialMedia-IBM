using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Model
{
    public class LikeRequestModel
    {
        [Required(ErrorMessage = "Reference post is required!")]
        public int PostId { get; set; }

        [Required(ErrorMessage = "User is required!")]
        public int UserId { get; set; }
    }
}
