using System;
using System.Collections.Generic;

namespace BugBaseApp.Models;

public partial class Ticket
{
    public long TicketId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public long? StateId { get; set; }

    public long? QaownerId { get; set; }

    public long? DevOwnerId { get; set; }

    public long? AssignedToId { get; set; }

    public virtual User? AssignedTo { get; set; }

    public virtual User? DevOwner { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual User? Qaowner { get; set; }

    public virtual State? State { get; set; }

    public virtual ICollection<TicketChangeHistory> TicketChangeHistories { get; set; } = new List<TicketChangeHistory>();
}
