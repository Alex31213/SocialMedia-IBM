using System;
using System.Collections.Generic;

namespace SocialMediaApp.DataDB
{
    public partial class Follow
    {
        public int FollowId { get; set; }
        public int FirstUserId { get; set; }
        public int SecondUserId { get; set; }

        public virtual User FirstUser { get; set; } = null!;
        public virtual User SecondUser { get; set; } = null!;
    }
}
