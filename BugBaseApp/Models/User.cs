using System;
using System.Collections.Generic;

namespace BugBaseApp.Models;

public partial class User
{
    public long UserId { get; set; }

    public string? UserName { get; set; }

    public string? DisplayName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public long? RoleId { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Ticket> TicketAssignedTos { get; set; } = new List<Ticket>();

    public virtual ICollection<TicketChangeHistory> TicketChangeHistoryAssignedTos { get; set; } = new List<TicketChangeHistory>();

    public virtual ICollection<TicketChangeHistory> TicketChangeHistoryDevOwners { get; set; } = new List<TicketChangeHistory>();

    public virtual ICollection<TicketChangeHistory> TicketChangeHistoryQaowners { get; set; } = new List<TicketChangeHistory>();

    public virtual ICollection<Ticket> TicketDevOwners { get; set; } = new List<Ticket>();

    public virtual ICollection<Ticket> TicketQaowners { get; set; } = new List<Ticket>();
}
