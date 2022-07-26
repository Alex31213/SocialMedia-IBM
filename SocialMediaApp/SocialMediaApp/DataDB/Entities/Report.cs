using System;
using System.Collections.Generic;

namespace SocialMediaApp.DataDB
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; } = null!;
        public DateTime PostedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
