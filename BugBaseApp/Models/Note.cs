using System;
using System.Collections.Generic;

namespace BugBaseApp.Models;

public partial class Note
{
    public long NoteId { get; set; }

    public string? NoteText { get; set; }

    public long? TicketId { get; set; }

    public long? NoteOwnerId { get; set; }

    public virtual User? NoteOwner { get; set; }

    public virtual Ticket? Ticket { get; set; }
}
