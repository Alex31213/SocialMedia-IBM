using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.DataDB;
using SocialMediaApp.DataDB.CRUDs;
using SocialMediaApp.Model;
using System.Collections.Generic;

namespace SocialMediaApp.Controllers
{
    [Route("api/Report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private ReportCRUD reportCrud = new ReportCRUD();

        /// <summary>
        /// Add new report to DB
        /// </summary>
        /// <response code="200">User</response>
        [HttpPost]
        [Route("AddReport")]
        public ActionResult Add([FromBody] ReportRequestModel Report) => AddReport(Report);

        /// <summary>
        /// Returns all reports
        /// </summary>
        [HttpGet]
        [Route("GetAllReports")]
        public IEnumerable<Report> GetAll() => reportCrud.GetAllReports();

        /// <summary>
        /// Returns all reports wrote by the user
        /// </summary>
        /// <param name="id">report id</param>
        /// <remarks>
        /// Returns empty enumerable object if there are no reports in DB
        /// </remarks>
        [HttpGet]
        [Route("GetAllReportsFromUser/{id}")]
        public IEnumerable<Report> GetAllFromUser([FromRoute(Name = "id")] int id) => reportCrud.GetAllUserReports(id);

        /// <summary>
        /// Returns specific report
        /// </summary>
        /// <param name="id">report id</param>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetReportById/{id}")]
        public ActionResult<Report> GetReportById([FromRoute(Name = "id")] int id) => GetById(id);

        /// <summary>
        /// Updates specific report text
        /// </summary>
        /// <param name="id">report id</param>
        /// <returns>User</returns>
        /// <response code="200">User</response>  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("UpdateReportText/{id}/{text}")]
        public ActionResult UpdateReportText([FromRoute(Name = "id")] int id, [FromRoute] string text) => UpdateGivenReportText(id, text);

        /// <summary>
        /// Updates specific report 
        /// </summary>
        /// <param name="id">report id</param>
        /// <returns>User</returns>
        /// <response code="200">User</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("UpdateReport/{id}")]
        public ActionResult UpdateReport([FromRoute(Name = "id")] int id, [FromBody] Report Report) => UpdateGivenReport(id, Report);

        /// <summary>
        /// Deletes specific report 
        /// </summary>
        /// <param name="id">report id</param>
        /// <returns>User</returns>
        /// <response code="200">User</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("DeleteReport/{id}")]
        public ActionResult DeleteReport([FromRoute(Name = "id")] int id, bool delete) => DeleteById(id);

        private ActionResult GetById(int id)
        {
            var foundReport = reportCrud.GetByID(id);
            if (foundReport == null)
            {
                return BadRequest("Report was not found!");
            }
            return Ok(foundReport);
        }

        private ActionResult DeleteById(int id)
        {
            var foundReport = reportCrud.GetByID(id);
            if (foundReport == null)
            {
                return BadRequest("Report was not found!");
            }
            reportCrud.RemoveByID(id);
            return Ok(foundReport);
        }

        private ActionResult UpdateGivenReportText(int id, string text)
        {
            var foundReport = reportCrud.GetByID(id);
            if (foundReport == null)
            {
                return BadRequest("Report  was not found!");
            }
            reportCrud.ChangeTextReport(id, text);
            return Ok("Report updated!");
        }

        private ActionResult UpdateGivenReport(int id, Report Report)
        {
            var foundReport = reportCrud.GetByID(id);
            if (foundReport == null)
            {
                return BadRequest("Report  was not found!");
            }
            reportCrud.UpdateByID(id, Report);
            return Ok("Report updated!");
        }

        private ActionResult AddReport(ReportRequestModel report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var ReportToAdd = new Report
                {
                    UserId = report.CreatorUserId,
                    Text = report.Text,
                    PostedDate = System.DateTime.Now
                };

                reportCrud.Add(ReportToAdd);
                return Ok("User was added!");
            }
        }
    }
}