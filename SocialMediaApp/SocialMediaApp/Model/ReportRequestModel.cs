using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Model
{
    public class ReportRequestModel
    {
        [Required(ErrorMessage = "Text required!")]
        [MaxLength(100, ErrorMessage = "Text too long!")]
        public string Text { get; set; }

        [Required(ErrorMessage = "User who posted is required!")]
        public int CreatorUserId { get; set; }
    }
}
