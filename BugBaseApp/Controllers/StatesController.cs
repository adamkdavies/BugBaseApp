using BugBaseApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugBaseApp.Controllers
{
    [Route("api/[controller]")]
    /// <summary>
    /// Handles the listing of state items.
    /// </summary>
    public class StatesController : ControllerBase
    {
        private readonly BugbaseContext _context;

        /// <summary>
        /// Instantiates the states controller.
        /// </summary>
        /// <param name="context">The database context required for accessing records.</param>
        public StatesController(BugbaseContext context)
        {
            _context = context;
        }

        /// <summary>
        ///  Gets a list of the existing states.
        /// </summary>
        /// <returns>The list of states.</returns>
        /// <response code="200">Successful retrieval of states.</response>
        /// <remarks>
        /// Sample response:
        /// 
        /// GET api/States
        /// 
        /// <pre>
        /// <code>
        /// [
        ///     {
        ///         "stateId": 0,
        ///         "stateName": 'New'
        ///     },
        ///     ...
        /// ]
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.States.ToListAsync());
        }

        /// <summary>
        ///  Gets an existing state.
        /// </summary>
        /// <returns>The state.</returns>
        /// <response code="200">Successful retrieval of the state.</response>
        /// <response code="404">State information was not found.</response>
        /// <remarks>
        /// Sample response:
        /// 
        /// GET api/States/0
        /// 
        /// <pre>
        /// <code>
        /// {
        ///     "stateId": 0,
        ///     "stateName": 'New'
        /// }
        /// </code>
        /// </pre>
        /// </remarks>

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var state = _context.States.FirstOrDefault(state => state.StateId == id);
            if (state == null)
            {
                return await Task.FromResult(NotFound(id));
            }
            return await Task.FromResult(Ok(state));
        }

     }
}
