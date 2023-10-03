using BugBaseApp.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugBaseApp.Controllers
{
    /// <summary>
    /// Handles the listing, creation, and deletion of notes.
    /// </summary>
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly BugbaseContext _context;

        /// <summary>
        /// Instantiates the notes controller.
        /// </summary>
        /// <param name="context">The database context required for accessing records.</param>
        public NotesController(BugbaseContext context)
        {
            _context = context;
        }

        /// <summary>
        ///  Gets a list of the existing notes.
        /// </summary>
        /// <returns>The list of notes.</returns>
        /// <response code="200">Successful retrieval of notes.</response>
        /// <remarks>
        /// Sample response:
        /// 
        /// GET api/Notes
        /// 
        /// <pre>
        /// <code>
        /// [
        ///     {
        ///         "noteId": 0,
        ///         "noteText": "I found some alternate behavior by doing...",
        ///         "ticketId": 0,
        ///         "noteOwnerId": 0,
        ///         "ticket": {0,...},
        ///         "noteOwner": {0,...}
        ///     },
        ///     ...
        /// ]
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Notes.Include(note => note.Ticket).Include(note => note.NoteOwner).ToListAsync());
        }

        /// <summary>
        ///  Gets the existing note.
        /// </summary>
        /// <param name="id">The id of the note.</param>
        /// <returns>The note.</returns>
        /// <response code="200">Successful retrieval of the note.</response>
        /// <response code="404">Note information was not found.</response>
        /// <remarks>
        /// Sample response:
        /// 
        /// GET api/Notes/0
        /// 
        /// <pre>
        /// <code>
        /// {
        ///     "noteId": 0,
        ///     "noteText": "I found some alternate behavior by doing...",
        ///     "ticketId": 0,
        ///     "noteOwnerId": 0,
        ///     "ticket": {0,...},
        ///     "noteOwner": {0,...}
        /// }
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var note = _context.Notes.Include(note=>note.Ticket).Include(note => note.NoteOwner).FirstOrDefault(note => note.NoteId == id);
            if (note == null)
            {
                return await Task.FromResult(NotFound(id));
            }
            return await Task.FromResult(Ok(note));
        }


        /// <summary>
        /// Adds a note to the system.
        /// </summary>
        /// <param name="note">The note entity.</param>
        /// <returns>The saved entity</returns>
        /// <response code="200">Successful insertion of note information.</response>
        /// <response code="400">Improperly formatted request.</response>
        /// <remarks>
        /// Sample request:
        /// 
        /// POST api/Notes
        /// 
        /// <pre>
        /// <code>
        /// {
        ///     "noteText": "I found some alternate behavior by doing...",
        ///     "ticketId": 0,
        ///     "noteOwnerId": 0
        /// }
        /// </code>
        /// </pre>
        ///
        /// Sample response:
        /// 
        /// <pre>
        /// <code>
        /// {
        ///     "noteText": "I found some alternate behavior by doing...",
        ///     "ticketId": 0,
        ///     "noteOwnerId": 0,
        ///     "ticket": {0,...},
        ///     "noteOwner": {0,...}
        /// }
        /// </code>
        /// </pre>
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Post([Bind("NoteId,NoteText,TicketId,NoteOwnerId")][FromBody] Note note)
        {
            if (ModelState.IsValid)
            {
                var entity = _context.Add(note);
                await _context.SaveChangesAsync();
                _context.Entry(entity).Reload();
                return await Get(entity.Entity.NoteId);
            }
            return BadRequest(note);
        }
    }
}
