using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.DataDB;
using SocialMediaApp.DataDB.CRUDs;
using SocialMediaApp.Model;
using System.Collections.Generic;

namespace SocialMediaApp.Controllers
{
    [Route("api/Post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        PostCRUD postCrud = new PostCRUD();

        /// <summary>
        /// Add new post to DB
        /// </summary>
        /// <response code="200">User</response>
        [HttpPost]
        [Route("AddPost")]
        public ActionResult Add([FromBody] PostRequestModel Post) => AddPost(Post);

        /// <summary>
        /// Get all posts from DB
        /// </summary>
        [HttpGet]
        [Route("GetAllPostsFromUser")]
        public IEnumerable<Post> GetAll() => postCrud.GetAllPosts();

        /// <summary>
        /// Returns all posts by the user
        /// </summary>
        /// <param name="id">user id</param>
        /// <remarks>
        /// Returns empty enumerable object if there are no posts in DB
        /// </remarks>
        [HttpGet]
        [Route("GetAllPostsFromUser/{id}")]
        public IEnumerable<Post> GetAllFromUser([FromRoute(Name = "id")] int id) => postCrud.GetAllUserPosts(id);

        /// <summary>
        /// Returns specific post
        /// </summary>
        /// <param name="id">user id</param>
        /// <remarks>
        /// Returns empty enumerable object if there are no posts in DB
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetPostById/{id}")]
        public ActionResult<Post> GetPostById([FromRoute(Name = "id")] int id) => GetById(id);

        /// <summary>
        /// Updates specific post text
        /// </summary>
        /// <param name="id">post id</param>
        /// <param name="text">text</param>
        /// <returns>User</returns>
        /// <response code="200">User</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("UpdatePostText/{id}/{text}")]
        public ActionResult UpdatePostText([FromRoute(Name = "id")] int id, [FromRoute(Name = "text")] string text) => UpdateGivenPostText(id, text);

        /// <summary>
        /// Updates specific post 
        /// </summary>
        /// <param name="id">post id</param>
        /// <returns>User</returns>
        /// <response code="200">User</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("UpdatePost/{id}")]
        public ActionResult UpdatePost([FromRoute(Name = "id")] int id, [FromBody] Post Post) => UpdateGivenPost(id, Post);

        /// <summary>
        /// Deletes specific post 
        /// </summary>
        /// <param name="id">post id</param>
        /// <returns>User</returns>
        /// <response code="200">User</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("DeletePost/{id}")]
        public ActionResult DeletePost([FromRoute(Name = "id")] int id) => DeleteById(id);

        private ActionResult GetById(int id)
        {
            var foundPost = postCrud.GetByID(id);
            if (foundPost == null)
            {
                return BadRequest("Post was not found!");
            }
            return Ok(foundPost);
        }

        private ActionResult DeleteById(int id)
        {
            var foundPost = postCrud.GetByID(id);
            if (foundPost == null)
            {
                return BadRequest("Post was not found!");
            }
            postCrud.RemoveByID(id);
            return Ok(foundPost);
        }

        private ActionResult UpdateGivenPostText(int id, string text)
        {
            var foundPost = postCrud.GetByID(id);
            if (foundPost == null)
            {
                return BadRequest("Post  was not found!");
            }
            postCrud.ChangeTextPost(id, text);
            return Ok("Post updated!");
        }

        private ActionResult UpdateGivenPost(int id, Post Post)
        {
            var foundPost = postCrud.GetByID(id);
            if (foundPost == null)
            {
                return BadRequest("Post  was not found!");
            }
            postCrud.UpdateByID(id, Post);
            return Ok("Post updated!");
        }

        private ActionResult AddPost(PostRequestModel post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var PostToAdd = new Post
                {
                    UserId = post.CreatorUserId,
                    Text = post.Text,
                    PostedDate = System.DateTime.Now
                };

                postCrud.Add(PostToAdd);
                return Ok("User was added!");
            }
        }
    }
}
