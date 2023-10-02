using BugBaseApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugBaseApp.Controllers
{
    /// <summary>
    /// Handles the listing of ticket change types.
    /// </summary>
    [Route("api/[controller]")]
    public class TicketChangeTypesController : ControllerBase
    {
        private readonly BugbaseContext _context;

        /// <summary>
        /// Instantiates the ticket change type controller.
        /// </summary>
        /// <param name="context">The database context required for accessing records.</param>
        public TicketChangeTypesController(BugbaseContext context)
        {
            _context = context;
        }

        /// <summary>
        ///  Gets a list of the existing ticket change types.
        /// </summary>
        /// <returns>The list of tickets change types.</returns>
        /// <response code="200">Successful retrieval of ticket change type information.</response>
        /// <remarks>
        /// Sample response:
        /// 
        /// GET api/TicketsChangeTypes
        /// 
        /// <pre>
        /// <code>
        /// [
        ///     {
        ///         "ticketChangeTypeId": 0,
        ///         "ticketChangeTypeName": "Created"
        ///     },
        ///     ...
        /// ]
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.TicketChangeTypes.ToListAsync());
        }

        /// <summary>
        /// Gets a ticket change type by the id.
        /// </summary>
        /// <param name="id">The ticket change type id.</param>
        /// <returns>The ticket change type information for the specified id.</returns>
        /// <response code="200">Successful retrieval of ticket change type information.</response>
        /// <response code="404">Ticket change type information was not found.</response>
        /// <remarks>
        /// Sample request:
        /// 
        /// GET api/TicketsChangeTypes/0
        /// <pre>
        /// <code>
        /// {
        ///     "ticketChangeTypeId": 0,
        ///     "ticketChangeTypeName": "Created"
        /// }
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var ticket = _context.TicketChangeTypes.FirstOrDefault(ticket => ticket.TicketChangeTypeId == id);
            if (ticket == null)
            {
                return await Task.FromResult(NotFound("No such ticket change type."));
            }
            return await Task.FromResult(Ok(ticket));
        }
    }
}
