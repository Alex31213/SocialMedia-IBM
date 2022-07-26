using System;
using System.Collections.Generic;

namespace SocialMediaApp.DataDB
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            FollowFirstUsers = new HashSet<Follow>();
            FollowSecondUsers = new HashSet<Follow>();
            Likes = new HashSet<Like>();
            Posts = new HashSet<Post>();
            Reports = new HashSet<Report>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? ProfilePictureLink { get; set; }
        public string Role { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Follow> FollowFirstUsers { get; set; }
        public virtual ICollection<Follow> FollowSecondUsers { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
