using BugBaseApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugBaseApp.Controllers
{
    /// <summary>
    /// Handles the listing of role items.
    /// </summary>
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly BugbaseContext _context;

        /// <summary>
        /// Instantiates the roles controller.
        /// </summary>
        /// <param name="context">The database context required for accessing records.</param>
        public RolesController(BugbaseContext context)
        {
            _context = context;
        }

        /// <summary>
        ///  Gets a list of the existing roles.
        /// </summary>
        /// <returns>The list of roles.</returns>
        /// <response code="200">Successful retrieval of roles.</response>
        /// <remarks>
        /// Sample response:
        /// 
        /// GET api/Roles
        /// 
        /// <pre>
        /// <code>
        /// [
        ///     {
        ///         "roleId": 0,
        ///         "roleName": 'Tester'
        ///     },
        ///     ...
        /// ]
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok(await _context.Roles.ToListAsync());
        }

        /// <summary>
        ///  Gets the existing role.
        /// </summary>
        /// <param name="id">The id of the role.</param>
        /// <returns>The role.</returns>
        /// <response code="200">Successful retrieval of the role.</response>
        /// <response code="404">Role information was not found.</response>
        /// <remarks>
        /// Sample response:
        /// 
        /// GET api/Roles/0
        /// 
        /// <pre>
        /// <code>
        /// {
        ///     "roleId": 0,
        ///     "roleName": 'Tester'
        /// }
        /// </code>
        /// </pre>
        /// </remarks>

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var role = _context.Roles.FirstOrDefault(role => role.RoleId == id);
            if (role == null)
            {
                return await Task.FromResult(NotFound(id));
            }
            return await Task.FromResult(Ok(role));
        }
    }
}
