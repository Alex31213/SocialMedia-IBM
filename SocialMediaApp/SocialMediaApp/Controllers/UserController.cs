using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.DataDB;
using SocialMediaApp.DataDB.CRUDs;
using SocialMediaApp.Dto;
using SocialMediaApp.Model;
using SocialMediaApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApp.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserCRUD userCRUD = new UserCRUD();
        private IUserAuthorizationService _authorization;

        public UserController(IUserAuthorizationService authorization)
        {
            _authorization = authorization;
        }

        /// <summary>
        /// Verify introduced credentials in order to login User
        /// </summary>
        /// <remarks>
        /// Get a jwt token
        /// </remarks>
        /// <response code="200">User</response> 
        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] UserRequestModel user) => LoginUser(user);

        /// <summary>
        /// Add new user to DB and verifying for username to be unique
        /// </summary>
        /// <response code="200">User</response>
        [HttpPost]
        [Route("register")]
        public ActionResult Register([FromBody] UserRequestModel user) => AddDbUser(user);

        /// <summary>
        /// Get all user from DB
        /// </summary>
        /// /// <remarks>
        /// Returns empty enumerable object if there are no users in DB
        /// </remarks>
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<User> GetAll() => userCRUD.GetAllUsers();

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <remarks>
        /// Get user by id
        /// </remarks>
        /// <param name="id">user id</param>
        /// <returns>User</returns>
        /// <response code="200">User</response>  
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetUserById/{id}")]
        public ActionResult<User> GetUserById([FromRoute(Name = "id")] int id) => GetById(id);

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <remarks>
        /// Get user by username
        /// </remarks>
        /// <param name="username">user username</param>
        /// <returns>User</returns>
        /// <response code ="200">User</response>
        ///[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetUserByUsername/{username}")]
        public ActionResult<User> GetUserByUsername([FromRoute(Name = "username")] string username) => GetByUsername(username);

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <remarks>
        /// Get user by email
        /// </remarks>
        /// <param name="email">user email</param>
        /// <returns>User</returns>
        /// <response code ="200">User</response>
        ///[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetUserByEmail/{email}")]
        public ActionResult<User> GetUserByEmail([FromRoute(Name = "email")] string email) => GetByEmail(email);

        /// <summary>
        /// Update user
        /// </summary>
        /// <remarks>Update a user from database</remarks>
        /// <param name="id"></param>
        /// <returns>User</returns>
        /// <response code = "200">User</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("UpdateUser/{id}")]
        public ActionResult UpdateUser([FromRoute] int id, [FromBody] User user) => UpdateGivenUser(id, user);

        /// <summary>
        /// Update user profile picture
        /// </summary>
        /// <remarks>Update profile pic link from User database</remarks>
        /// <param name="id"></param>
        /// <param name="link"></param>
        /// <returns>User</returns>
        /// <response code = "200">User</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("ChangeProfilePic/{id}/{link}")]
        public ActionResult ChangeProfilePic([FromRoute] int id, [FromRoute] string link) => ChangePic(id, link);

        /// <summary>
        /// Delete User from DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User</returns>
        /// <response code = "200">User</response>
        [HttpDelete("DeleteUser/{id}")]
        public void DeleteUser([FromRoute] int id, bool delete) => userCRUD.ChangeDeletedState(id, true);

        private ActionResult LoginUser(UserRequestModel user)
        {
            var foundUser = userCRUD.GetUserByUsername(user.Username);
            if (foundUser == null) return BadRequest("User not found!");

            var samePassword = _authorization.VerifyHashedPassword(foundUser.Password, user.Password);
            if (!samePassword) return BadRequest("Invalid password!");

            var user_jsonWebToken = _authorization.GetToken(foundUser);

            return Ok(new ResponseLogin
            {
                Token = user_jsonWebToken
            });
        }

        private ActionResult AddDbUser(UserRequestModel user)
        {
            //TODO: validare pentru proprietatile obiectului
            //TODO: mesaj si return 400 daca nu e valid, cu mesaj
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                if (userCRUD.GetUserByUsername(user.Username) != null)
                {
                    return BadRequest("Username already exists!");
                }

                var userToAdd = new User
                {
                    Password = _authorization.HashPassword(user.Password),
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                };

                userCRUD.Add(userToAdd);
                return Ok("User was added!");
            }
        }

        private ActionResult GetById(int id)
        {
            var foundUser = userCRUD.GetByID(id);
            if (foundUser == null)
            {
                return BadRequest("User was not found!");
            }
            return Ok(foundUser);
        }

        private ActionResult GetByUsername(string username)
        {
            var foundUser = userCRUD.GetUserByUsername(username);
            if (foundUser == null)
            {
                return BadRequest("User with this username was not found!");
            }
            return Ok(foundUser);
        }

        private ActionResult GetByEmail(string email)
        {
            var foundUser = userCRUD.GetUsersByEMail(email);
            if (foundUser == null)
            {
                return BadRequest("User with this email was not found!");
            }
            return Ok(foundUser);
        }

        private ActionResult UpdateGivenUser(int id, User user)
        {
            var foundUser = userCRUD.GetByID(id);
            if (foundUser == null)
            {
                return BadRequest("User  was not found!");
            }
            userCRUD.UpdateByID(id, user);
            return Ok("User updated!");
        }

        private ActionResult ChangePic(int id, string link)
        {
            var foundUser = userCRUD.GetByID(id);
            if (foundUser == null)
            {
                return BadRequest("User  was not found!");
            }
            userCRUD.ChangeProfilePictureLink(id, link);
            return Ok("User profile pic link was updated!");
        }
    }
}