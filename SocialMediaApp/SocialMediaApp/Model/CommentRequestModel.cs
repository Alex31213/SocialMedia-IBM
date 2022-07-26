using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Model
{
    public class CommentRequestModel
    {
        [Required(ErrorMessage = "Text required!")]
        [MaxLength(100, ErrorMessage = "Text too long!")]
        public string Text { get; set; }

        [Required(ErrorMessage = "User who posted the comment is required!")]
        public int CreatorUserId { get; set; }

        [Required(ErrorMessage = "Commented post is required!")]
        public int CommentedPost { get; set; }
    }
}
