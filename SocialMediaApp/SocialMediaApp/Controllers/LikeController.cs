using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.DataDB;
using SocialMediaApp.DataDB.CRUDs;
using SocialMediaApp.Model;
using System.Collections.Generic;

namespace SocialMediaApp.Controllers
{
    [Route("api/Like")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private LikeCRUD likeCrud = new LikeCRUD();

        /// <summary>
        /// Adds like to DB
        /// </summary>
        /// <response code="200">User</response>
        [HttpPost]
        [Route("AddLike")]
        public ActionResult Add([FromBody] LikeRequestModel Like) => AddLike(Like);

        /// <summary>
        /// Returns all posts by the user
        /// </summary>
        /// <param name="id">post id</param>
        /// <remarks>
        /// Returns empty enumerable object if there are no likes to that post in DB
        /// </remarks>
        [HttpGet]
        [Route("GetAllLikesFromPost/{id}")]
        public IEnumerable<Like> GetAllLikesPost([FromRoute(Name = "id")] int id) => likeCrud.GetAllLikesFromPost(id);

        /// <summary>
        /// Returns all posts by the user
        /// </summary>
        /// <param name="id">user id</param>
        /// <remarks>
        /// Returns empty enumerable object if the user didn't like any posts
        /// </remarks>
        //admin
        [HttpGet]
        [Route("GetAllLikesFromUser/{id}")]
        public IEnumerable<Like> GetAllLikesUser([FromRoute(Name = "id")] int id) => likeCrud.GetAllLikesFromUser(id);

        /// <summary>
        /// Returns all likes in DB
        /// </summary>
        /// <param name="id">like id</param>
        /// <remarks>
        /// Returns empty enumerable object if there are no likes in DB
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetLikeById/{id}")]
        public ActionResult<Like> GetLikeById([FromRoute(Name = "id")] int id) => GetById(id);

        /// <summary>
        /// Deletes specific like 
        /// </summary>
        /// <param name="id">like id</param>
        /// <returns>User</returns>
        /// <response code="200">User</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("DeleteLike/{id}")]
        public ActionResult DeleteLike([FromRoute(Name = "id")] int id) => DeleteById(id);

        /// <summary>
        /// Deletes specific like from specific user to specific post
        /// </summary>
        /// <param name="id">user1 id</param>
        /// <param name="id2">user2 id</param>
        /// <returns>User</returns>
        /// <response code="200">User</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("DeleteSpecificLike/{id}/{id2}")]
        public ActionResult DeleteLikeByUserAndPostDetails([FromRoute(Name = "id")] int userId, [FromRoute(Name = "id2")] int postId) => DeleteBySpecificIds(userId, postId);

        private ActionResult GetById(int id)
        {
            var foundLike = likeCrud.GetByID(id);
            if (foundLike == null)
            {
                return BadRequest("Like was not found!");
            }
            return Ok(foundLike);
        }

        private ActionResult DeleteBySpecificIds(int userId, int postId)
        {
            var foundUser = likeCrud.GetByID(userId);
            var foundPost = likeCrud.GetByID(postId);
            if (foundUser == null || foundPost == null)
            {
                return BadRequest("User or post not found!");
            }
            likeCrud.RemoveByUserAndPost(userId, postId);
            return Ok("LikeDeleted");
        }

        private ActionResult DeleteById(int id)
        {
            var foundLike = likeCrud.GetByID(id);
            if (foundLike == null)
            {
                return BadRequest("Like was not found!");
            }
            likeCrud.RemoveByID(id);
            return Ok(foundLike);
        }

        private ActionResult AddLike(LikeRequestModel like)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var LikeToAdd = new Like
                {
                    UserId = like.UserId,
                    PostId = like.PostId
                };

                likeCrud.Add(LikeToAdd);
                return Ok("User was added!");
            }
        }
    }
}
