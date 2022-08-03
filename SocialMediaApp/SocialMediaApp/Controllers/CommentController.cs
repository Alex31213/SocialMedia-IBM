using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.DataDB;
using SocialMediaApp.DataDB.CRUDs;
using SocialMediaApp.Model;
using System.Collections.Generic;

namespace SocialMediaApp.Controllers
{
    [Route("api/Comment")]
    [ApiController,Authorize]
    public class CommentController : ControllerBase
    {
        private CommentCRUD commentCrud = new CommentCRUD();

        //[HttpGet]
        //[Route("GetAllPostComments/{id}")]
        //public IEnumerable<Comment> GetAll([FromRoute(Name = "id")] int id) => commentCrud.GetAllPostComments(id);

        /// <summary>
        /// Adds comment to DB
        /// </summary>
        /// <response code="200">User</response>
        [HttpPost]
        [Route("AddComment")]
        public ActionResult Add([FromBody] CommentRequestModel comment) => AddComment(comment);

        /// <summary>
        /// Returns all comments from DB
        /// </summary>
        /// <remarks>
        /// Returns empty enumerable object if there are no comments in DB
        [HttpGet]
        [Route("GetAllComments")]
        public IEnumerable<Comment> GetAllComments() => commentCrud.GetAllComments();

        /// <summary>
        /// Returns all comments posted by the user
        /// </summary>
        /// <param name="id">comment id</param>
        /// <remarks>
        /// Returns empty enumerable object if there are no comments in Db
        /// </remarks>
        [HttpGet]
        [Route("GetAllCommentsFromUser/{id}")]
        public IEnumerable<Comment> GetAllFromUser([FromRoute(Name = "id")] int id) => commentCrud.GetAllUserComments(id);

        /// <summary>
        /// Returns specific comment
        /// </summary>
        /// <param name="id">comment id</param>
        /// <remarks>
        /// Returns empty enumerable object if there is no comments in Db
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetCommentById/{id}")]
        public ActionResult<Comment> GetCommentById([FromRoute(Name = "id")] int id) => GetById(id);

        /// <summary>
        /// Updates specific comment text
        /// </summary>
        /// <param name="id">comment id</param>
        /// <param name="text">new comment text</param>
        /// <remarks>
        /// <response code = "200">User</response>
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("UpdateCommentText/{id}/{text}")]
        public ActionResult UpdateCommentText([FromRoute(Name = "id")] int id, [FromRoute(Name = "text")] string text) => UpdateGivenCommentText(id, text);

        /// <summary>
        /// Updates specific comment 
        /// </summary>
        /// <param name="id">comment id</param>
        /// <remarks>
        /// <response code = "200">User</response>
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("UpdateComment/{id}")]
        public ActionResult UpdateComment([FromRoute(Name = "id")] int id, [FromBody] Comment Comment) => UpdateGivenComment(id, Comment);

        /// <summary>
        /// Deletes specific comment 
        /// </summary>
        /// <param name="id">comment id</param>
        /// <remarks>
        /// <response code = "200">User</response>
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("DeleteComment/{id}")]
        public ActionResult DeleteComment([FromRoute(Name = "id")] int id) => DeleteById(id);

        private ActionResult GetById(int id)
        {
            var foundComment = commentCrud.GetByID(id);
            if (foundComment == null)
            {
                return BadRequest("Comment was not found!");
            }
            return Ok(foundComment);
        }

        private ActionResult DeleteById(int id)
        {
            var foundComment = commentCrud.GetByID(id);
            if (foundComment == null)
            {
                return BadRequest("Comment was not found!");
            }
            commentCrud.RemoveByID(id);
            return Ok(foundComment);
        }

        private ActionResult UpdateGivenCommentText(int id, string text)
        {
            var foundComment = commentCrud.GetByID(id);
            if (foundComment == null)
            {
                return BadRequest("Comment  was not found!");
            }
            commentCrud.ChangeTextComment(id, text);
            return Ok("Comment updated!");
        }

        private ActionResult UpdateGivenComment(int id, Comment comment)
        {
            var foundComment = commentCrud.GetByID(id);
            if (foundComment == null)
            {
                return BadRequest("Comment  was not found!");
            }
            commentCrud.UpdateByID(id, comment);
            return Ok("Comment updated!");
        }

        private ActionResult AddComment(CommentRequestModel comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var commentToAdd = new Comment
                {
                    PostId = comment.CommentedPost,
                    UserId = comment.CreatorUserId,
                    Text = comment.Text,
                    PostedDate = System.DateTime.Now
                };

                commentCrud.Add(commentToAdd);
                return Ok("User was added!");
            }
        }
    }
}