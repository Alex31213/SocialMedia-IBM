using System;
using System.Collections.Generic;

namespace SocialMediaApp.DataDB
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
        }

        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; } = null!;
        public DateTime PostedDate { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
    }
}
