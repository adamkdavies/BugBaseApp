using BugBaseApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugBaseApp.Controllers
{


    /// <summary>
    ///  Handles the creation, listing, modification and delete of bug ticket records.
    /// </summary>
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly BugbaseContext _context;

        /// <summary>
        /// Instantiates the tickets controller.
        /// </summary>
        /// <param name="context">The database context required for managing records.</param>
        public TicketsController(BugbaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a list of the existing tickets.
        /// </summary>
        /// <returns>The list of tickets.</returns>
        /// <response code="200">Successful retrieval of tickets information.</response>
        /// <response code="500">Ticket information unavailable.</response>
        /// <remarks>
        /// Sample response:
        /// 
        /// GET api/Tickets
        /// 
        /// <pre>
        /// <code>
        /// [
        ///     {
        ///         "ticketId": 0,
        ///         "title": "Big Problem",
        ///         "description" : "Doing this causes a big exception ... ",
        ///         "product": "Bug Base",
        ///         "feature": "Tickets",
        ///         "iteration": "Iteration 0",
        ///         "stateId" : 1,
        ///         "qaOwnerId" : 1,
        ///         "devOwnerId : 0,
        ///         "assignedToId : 0,
        ///         "assignedTo" : {0,...},
        ///         "devOwner" : {0,...},
        ///         "qaOwner" : {1,...},
        ///         "state" : {1,...},
        ///         "ticketChangeHistories" : [{0,...},...] 
        ///     },
        ///     ...
        /// ]
        /// </code>
        /// </pre>
        /// </remarks>        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var bugbaseContext = _context.Tickets.Include(t => t.AssignedTo).Include(t => t.DevOwner).Include(t => t.Qaowner).Include(t => t.State).Include(t => t.TicketChangeHistories);
            return Ok(await bugbaseContext.ToListAsync());
        }

        /// <summary>
        /// Gets a ticket by their ticket id.
        /// </summary>
        /// <param name="id">The ticket specified by the id.</param>
        /// <returns>The ticket information for the specified id.</returns>
        /// <response code="200">Successful retrieval of ticket information.</response>
        /// <response code="404">Ticket record was not found.</response>
        /// <remarks>
        /// Sample request:
        /// 
        /// GET api/Tickets/0
        /// 
        /// <pre>
        /// <code>
        /// {
        ///     "ticketId": 0,
        ///     "title": "Big Problem",
        ///     "description" : "Doing this causes a big exception ... ",
        ///     "product": "Bug Base",
        ///     "feature": "Tickets",
        ///     "iteration": "Iteration 0",
        ///     "stateId" : 1,
        ///     "qaOwnerId" : 1,
        ///     "devOwnerId" : 0,
        ///     "assignedToId" : 0,
        ///     "assignedTo" : {0,...},
        ///     "devOwner" : {0,...},
        ///     "qaOwner" : {1,...},
        ///     "state" : {1,...},
        ///     "ticketChangeHistories" : [{0,...},...] 
        /// }
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var bugbaseContext = _context.Tickets.Include(t => t.AssignedTo).Include(t => t.DevOwner).Include(t => t.Qaowner).Include(t => t.State).Include(t => t.TicketChangeHistories);
            var ticket = bugbaseContext.FirstOrDefault(ticket => ticket.TicketId == id);
            if(ticket == null)
            {
                return await Task.FromResult(NotFound("No such ticket."));
            }
            return await Task.FromResult(Ok(ticket));
        }

        /// <summary>
        /// Adds a ticket to the system.
        /// </summary>
        /// <param name="ticket">The ticket entity.</param>
        /// <returns>The saved entity</returns>
        /// <response code="200">Successful insertion of ticket information.</response>
        /// <response code="400">Improperly formatted request.</response>
        /// <remarks>
        /// Sample request:
        /// 
        /// POST api/Tickets
        /// 
        /// <pre>
        /// <code>
        /// {
        ///     "title": "Big Problem",
        ///     "description" : "Doing this causes a big exception ... ",
        ///     "qaOwnerId" : 1,
        ///     "devOwnerId" : 0
        /// }
        /// </code>
        /// </pre>
        ///
        /// Sample response:
        /// 
        /// <pre>
        /// <code>
        /// {
        ///     "ticketId": 0,
        ///     "title": "Big Problem",
        ///     "description" : "Doing this causes a big exception ... ",
        ///     "product": "Bug Base",
        ///     "feature": "Tickets",
        ///     "iteration": "Iteration 0",
        ///     "stateId" : 1,
        ///     "qaOwnerId" : 1,
        ///     "devOwnerId" : 0,
        ///     "assignedToId" : 0,
        ///     "assignedTo" : null,
        ///     "devOwner" : {0,...},
        ///     "qaOwner" : {1,...},
        ///     "state" : {1,...},
        ///     "ticketChangeHistories" : [{0,...}] 
        /// }
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Post([Bind("TicketId,Title,Description,State,Qaowner,DevOwner,AssignedTo")][FromBody] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var entity = _context.Add(ticket);
                await _context.SaveChangesAsync();
                return await Get(entity.Entity.TicketId);
            }

            return BadRequest(ticket);
        }

        /// <summary>
        /// Modifies a ticket entry.
        /// </summary>
        /// <param name="id">The id of the ticket.</param>
        /// <param name="ticket">The fields to modify for the ticket.</param>
        /// <returns>The modified ticket information record.</returns>
        /// <response code="200">Successful modification of ticket information.</response>
        /// <response code="404">Ticket record was not found.</response>
        /// <remarks>
        /// Sample request:
        /// 
        /// POST api/Tickets
        /// 
        /// <pre>
        /// <code>
        /// {
        ///     "ticketId": 0,
        ///     "stateId" 5
        /// }
        /// </code>
        /// </pre>
        ///
        /// Sample response:
        /// 
        /// <pre>
        /// <code>
        /// {
        ///     "ticketId": 0,
        ///     "title": "Big Problem",
        ///     "description" : "Doing this causes a big exception ... ",
        ///     "product": "Bug Base",
        ///     "feature": "Tickets",
        ///     "iteration": "Iteration 0",
        ///     "stateId" : 5,
        ///     "qaOwnerId" : 1,
        ///     "devOwnerId" : 0,
        ///     "assignedToId" : 1,
        ///     "assignedTo" : {1,...},
        ///     "devOwner" : {0,...},
        ///     "qaOwner" : {1,...},
        ///     "state" : {5,...},
        ///     "ticketChangeHistories" : [{0,...}] 
        /// }
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(long id, [Bind("TicketId,Title,Description,State,Qaowner,DevOwner,AssignedTo")][FromBody] Ticket ticket)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var entity = _context.Update(ticket);
                    await _context.SaveChangesAsync();
                    return await Get(entity.Entity.TicketId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketId))
                    {
                        return NotFound(id);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return Ok(ticket);
        }

        /// <summary>
        /// Deletes a ticket with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Successful deletion of ticket information.</response>
        /// <response code="404">Ticket record was not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {

            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                return Ok();
            }

            await _context.SaveChangesAsync();
            return NotFound(id);
        }

        private bool TicketExists(long id)
        {
            return (_context.Tickets?.Any(e => e.TicketId == id)).GetValueOrDefault();
        }
    }
}
