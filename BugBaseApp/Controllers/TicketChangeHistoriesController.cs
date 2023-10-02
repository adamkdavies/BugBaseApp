using BugBaseApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugBaseApp.Controllers
{
    /// <summary>
    /// Handles the listing of ticket change history items.
    /// </summary>
    [Route("api/[controller]")]
    public class TicketChangeHistoriesController : ControllerBase
    {
        private readonly BugbaseContext _context;

        /// <summary>
        /// Instantiates the ticket change histories controller.
        /// </summary>
        /// <param name="context">The database context required for accessing records.</param>
        public TicketChangeHistoriesController(BugbaseContext context)
        {
            _context = context;
        }

        /// <summary>
        ///  Gets a list of the existing ticket change types.
        /// </summary>
        /// <returns>The list of tickets change types.</returns>
        /// <response code="200">Successful retrieval of ticket change information.</response>
        /// <remarks>
        /// Sample response:
        /// 
        /// GET api/TicketsChangeHistories
        /// 
        /// <pre>
        /// <code>
        /// [
        ///     {
        ///         "ticketChangeHistoryId": 0,
        ///         "ticketChangeTypeId": 0,
        ///         "ticketId": 0,
        ///         "ticketChangeDateTime": "2023-...",
        ///         "title": "Bad Bug",
        ///         "description": "This really bad thing happens when...",
        ///         "product": "Bug Base",
        ///         "feature": "Tickets",
        ///         "iteration": "Iteration 0",
        ///         "stateId": 0,
        ///         "qaOwnerId" : 0,
        ///         "devOwnerId" : 1,
        ///         "asssignedToId" : 0,
        ///         "noteText" : null,
        ///         "ticket" : {0,...},
        ///         "ticketChangeType" : {0,...}
        ///     },
        ///     ...
        /// ]
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var bugbaseContext = _context.TicketChangeHistories.Include(t => t.Ticket).Include(t => t.TicketChangeType);
            return Ok(await bugbaseContext.ToListAsync());
        }

        /// <summary>
        /// Gets a ticket change history by the id.
        /// </summary>
        /// <param name="id">The ticket history id.</param>
        /// <returns>The ticket change history information for the specified id.</returns>
        /// <response code="200">Successful retrieval of ticket change type information.</response>
        /// <response code="404">Ticket change type information was not found.</response>
        /// <remarks>
        /// Sample request:
        /// 
        /// GET api/TicketsChangeHistories/0
        /// 
        /// <pre>
        /// <code>
        /// {
        ///     "ticketChangeHistoryId": 0,
        ///     "ticketChangeTypeId": 0,
        ///     "ticketId": 0,
        ///     "ticketChangeDateTime": "2023-...",
        ///     "title": "Bad Bug",
        ///     "description": "This really bad thing happens when...",
        ///     "product": "Bug Base",
        ///     "feature": "Tickets",
        ///     "iteration": "Iteration 0",
        ///     "stateId": 0,
        ///     "qaOwnerId" : 0,
        ///     "devOwnerId" : 1,
        ///     "asssignedToId" : 0,
        ///     "noteText" : null,
        ///     "ticket" : {0,...},
        ///     "ticketChangeType" : {0,...}
        /// }
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var ticket = _context.TicketChangeHistories.Where(t => t.TicketChangeHistoryId == id).FirstOrDefault();
            
            if (ticket == null)
            {
                return await Task.FromResult(NotFound(id));
            }
            return await Task.FromResult(Ok(ticket));
        }
    }
}
