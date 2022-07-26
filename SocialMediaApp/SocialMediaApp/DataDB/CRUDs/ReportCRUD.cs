using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApp.DataDB.CRUDs
{
    public class ReportCRUD
    {
        public void Add(Report addedReport)
        {
            SocialMediaContext database = new SocialMediaContext();
            database.Reports.Add(addedReport);
            database.SaveChanges();
        }

        public List<Report> GetAllReports()
        {
            SocialMediaContext database = new SocialMediaContext();
            List<User> users = database.Users.ToList();
            List<Report> reports = database.Reports.ToList();
            return reports.FindAll(x => users.SingleOrDefault(y => y.UserId == x.UserId).IsDeleted == false);
        }

        public List<Report> GetAllUserReports(int UserId)
        {
            SocialMediaContext database = new SocialMediaContext();
            List<User> users = database.Users.ToList();
            List<Report> reports = database.Reports.ToList();
            return reports.FindAll(x => x.UserId == UserId);
        }

        public Report GetByID(int Id)
        {
            SocialMediaContext database = new SocialMediaContext();
            List<Report> reports = database.Reports.ToList();
            return reports.SingleOrDefault(x => x.ReportId == Id);
        }

        public void RemoveByID(int Id)
        {
            SocialMediaContext database = new SocialMediaContext();
            var reports = database.Reports;
            reports.Remove(reports.ToList().SingleOrDefault(x => x.ReportId == Id));
            database.SaveChanges();
        }
        public void UpdateByID(int Id, Report updatedReport)
        {
            SocialMediaContext database = new SocialMediaContext();
            var reports = database.Reports;
            updatedReport.ReportId = Id;
            database.Entry(reports.SingleOrDefault(x => x.ReportId == Id)).CurrentValues.SetValues(updatedReport);
            database.SaveChanges();
        }
        public void ChangeTextReport(int Id, string NewText)
        {
            SocialMediaContext database = new SocialMediaContext();
            var reports = database.Reports;
            Report updatedReport = reports.SingleOrDefault(x => x.ReportId == Id);
            updatedReport.Text = NewText;
            database.Entry(reports.SingleOrDefault(x => x.ReportId == Id)).CurrentValues.SetValues(updatedReport);
            database.SaveChanges();
        }
        public void ChangeDeletedState(int Id, bool isDeleted)
        {
            SocialMediaContext database = new SocialMediaContext();
            var reports = database.Reports;
            Report updatedReport = reports.SingleOrDefault(x => x.ReportId == Id);
            updatedReport.IsDeleted = isDeleted;
            database.Entry(reports.SingleOrDefault(x => x.ReportId == Id)).CurrentValues.SetValues(updatedReport);
            database.SaveChanges();
        }
    }
}
