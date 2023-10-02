using BugBaseApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugBaseApp.Controllers
{
    /// <summary>
    /// Handles the creation, listing, modification and delete of user records.
    /// </summary>
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly BugbaseContext _context;

        /// <summary>
        /// Instantiates the users controller.
        /// </summary>
        /// <param name="context">The database context required for managing records.</param>
        public UsersController(BugbaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a list of the users in the system.
        /// </summary>
        /// <returns>The list of users in the system.</returns>
        /// <response code="200">Successful retrieval of users information.</response>
        /// <remarks>
        /// Sample response:
        /// 
        /// GET api/Users
        /// 
        /// <pre>
        /// <code>
        /// [
        ///     {
        ///         "userId : 0,
        ///         "userName" : "johndoe",
        ///         "displayName" : "John Doe",
        ///         "email" : "john.doe@corp.com",
        ///         "phone" : "555-555-5555",
        ///         "roleId" : 0,
        ///         "role" : {...}
        ///     }, ...
        /// ]
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var bugbaseContext = _context.Users.Include(u => u.Role);
            return Ok((await bugbaseContext.ToListAsync()));
        }


        /// <summary>
        /// Gets a user by their user id.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>The user information for the specified id.</returns>
        /// <response code="200">Successful retrieval of user information.</response>
        /// <response code="404">User information was not found.</response>
        /// <remarks>
        /// Sample response:
        ///
        /// GET api/Users/0
        /// <pre>
        /// <code>
        /// {
        ///     "userId : 0,
        ///     "userName" : "johndoe",
        ///     "displayName" : "John Doe",
        ///     "email" : "john.doe@corp.com",
        ///     "phone" : "555-555-5555",
        ///     "role" : 0
        ///     "role" : {...}
        /// }
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var user = _context.Users.Include(role=>role.Role).FirstOrDefault(u => u.UserId == id);
            if(user == null)
            {
                return await Task.FromResult(NotFound(id));
            }
            return await Task.FromResult(Ok(user));
        }

        /// <summary>
        /// Adds a user to the system.
        /// </summary>
        /// <param name="user">The user entity</param>
        /// <returns>The saved entity</returns>
        /// <response code="200">Successful retrieval of user information.</response>
        /// <response code="400">Improperly formatted request.</response>
        /// <remarks>
        /// Sample request:
        /// 
        /// POST api/Users
        /// 
        /// <pre>
        /// <code>
        /// {
        ///     "userName" : "johndoe",
        ///     "displayName" : "John Doe",
        ///     "email" : "john.doe@corp.com",
        ///     "phone" : "555-555-5555",
        ///     "role" : 0
        /// }
        /// </code>
        /// </pre>
        ///
        /// Sample response:
        /// 
        /// <pre>
        /// <code>
        /// {
        ///     "userName" : "johndoe",
        ///     "displayName" : "John Doe",
        ///     "email" : "john.doe@corp.com",
        ///     "phone" : "555-555-5555",
        ///     "role" : 0
        /// }
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Post([Bind("UserId,UserName,DisplayName,Email,Phone,Role")][FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var entity = _context.Add(user);
                await _context.SaveChangesAsync();
                return await Get(entity.Entity.UserId);
            }

            return BadRequest(user);
        }

        /// <summary>
        /// Modifies a user entry.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <param name="user">The fields to modify for the user.</param>
        /// <returns>The modified user information record.</returns>
        /// <response code="200">Successful modification of user information.</response>
        /// <response code="404">User record was not found.</response>
        /// <remarks>
        /// Sample request:
        /// 
        /// POST api/Users/0
        /// 
        /// <pre>
        /// <code>
        /// {
        ///     "userName" : "janedoe",
        ///     "displayName" : "Jane Doe",
        ///     "email" : "jane.doe@corp.com",
        ///     "phone" : "444-444-4444",
        ///     "role" : 3
        /// }
        /// </code>
        /// </pre>
        ///
        /// Sample response:
        /// 
        /// <pre>
        /// <code>
        /// {
        ///     "userId: 0,
        ///     "userName" : "janedoe",
        ///     "displayName" : "Jane Doe",
        ///     "email" : "jane.doe@corp.com",
        ///     "phone" : "444-444-4444",
        ///     "role" : 3
        /// }
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [Bind("UserId,UserName,DisplayName,Email,Phone,Role")][FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = _context.Update(user);
                    await _context.SaveChangesAsync();
                    return await Get(entity.Entity.UserId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound(id);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return Ok(user);
        }

        /// <summary>
        /// Deletes a user with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Successful deletion of user information.</response>
        /// <response code="404">User record was not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var user = await _context.Users.FindAsync(id);
            
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound(id);
        }

        private bool UserExists(long id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
