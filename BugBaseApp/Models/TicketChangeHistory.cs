using System;
using System.Collections.Generic;

namespace BugBaseApp.Models;

public partial class TicketChangeHistory
{
    public long TicketChangeHistoryId { get; set; }

    public long? TicketChangeTypeId { get; set; }

    public long? TicketId { get; set; }

    public string? TicketChangeDateTime { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public long? StateId { get; set; }

    public long? QaownerId { get; set; }

    public long? DevOwnerId { get; set; }

    public long? AssignedToId { get; set; }

    public string? NoteText { get; set; }

    public virtual User? AssignedTo { get; set; }

    public virtual User? DevOwner { get; set; }

    public virtual User? Qaowner { get; set; }

    public virtual State? State { get; set; }

    public virtual Ticket? Ticket { get; set; }

    public virtual TicketChangeType? TicketChangeType { get; set; }
}
