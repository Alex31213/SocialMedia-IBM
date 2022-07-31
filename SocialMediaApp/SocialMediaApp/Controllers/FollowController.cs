using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.DataDB;
using SocialMediaApp.DataDB.CRUDs;
using SocialMediaApp.Model;
using System.Collections.Generic;

namespace SocialMediaApp.Controllers
{
    [Route("api/Follow")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private FollowCRUD followCrud = new FollowCRUD();

        /// <summary>
        /// Adds like to DB
        /// </summary>
        /// <response code="200">User</response>
        [HttpPost]
        [Route("AddFollow")]
        public ActionResult Add([FromBody] FollowRequestModel Follow) => AddFollow(Follow);

        /// <summary>
        /// Returns all follows by the user
        /// </summary>
        /// <remarks>
        /// Returns empty enumerable object if there are no follows in DB
        //admin
        [HttpGet]
        [Route("GetAllFollowsFromPost")]
        public IEnumerable<Follow> GetAllFollowsPossible() => followCrud.GetAllFollows();

        /// <summary>
        /// Returns all follows of the user
        /// </summary>
        /// <param name="id">user id</param>
        /// <remarks>
        /// Returns empty enumerable object if the user doesn't follow anybody
        /// </remarks>
        [HttpGet]
        [Route("GetAllFollowsFromUser/{id}")]
        public IEnumerable<Follow> GetAllFollowsUser([FromRoute(Name = "id")] int id) => followCrud.GetUserFollows(id);

        /// <summary>
        /// Returns specific follow in DB
        /// </summary>
        /// <param name="id">like id</param>
        /// <remarks>
        /// Returns empty enumerable object if there is no follow in DB
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetFollowById/{id}")]
        public ActionResult<Follow> GetFollowById([FromRoute(Name = "id")] int id) => GetById(id);

        /// <summary>
        /// Deletes specific follow from DB
        /// </summary>
        /// <param name="id">like id</param>
        /// <response code="200">User</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("DeleteFollow/{id}")]
        public ActionResult DeleteFollow([FromRoute(Name = "id")] int id) => DeleteById(id);

        /// <summary>
        /// Deletes specific follow from specific user to specific post
        /// </summary>
        /// <param name="id">user1 id</param>
        /// <param name="id2">user2 id</param>
        /// <returns>User</returns>
        /// <response code="200">User</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("DeleteSpecificFollow/{id}/{id2}")]
        public ActionResult DeleteFollowByUserAndPostDetails([FromRoute(Name = "id")] int userId, [FromRoute(Name = "id2")] int secondUserId) => DeleteBySpecificIds(userId, secondUserId);

        private ActionResult GetById(int id)
        {
            var foundFollow = followCrud.GetByID(id);
            if (foundFollow == null)
            {
                return BadRequest("Follow was not found!");
            }
            return Ok(foundFollow);
        }

        private ActionResult DeleteBySpecificIds(int userId, int secondUserId)
        {
            var foundUser = followCrud.GetByID(userId);
            var foundSecondUser = followCrud.GetByID(secondUserId);
            if (foundUser == null || foundSecondUser == null)
            {
                return BadRequest("Users not found!");
            }
            followCrud.RemoveByUserIDs(userId, secondUserId);
            return Ok("FollowDeleted");
        }

        private ActionResult DeleteById(int id)
        {
            var foundFollow = followCrud.GetByID(id);
            if (foundFollow == null)
            {
                return BadRequest("Follow was not found!");
            }
            followCrud.RemoveByID(id);
            return Ok(foundFollow);
        }

        private ActionResult AddFollow(FollowRequestModel follow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var FollowToAdd = new Follow
                {
                    FirstUserId = follow.FirstUserId,
                    SecondUserId = follow.SecondUserId
                };

                followCrud.Add(FollowToAdd);
                return Ok("User was added!");
            }
        }
    }
}
