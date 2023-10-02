using System;
using System.Collections.Generic;

namespace BugBaseApp.Models;

public partial class State
{
    public long StateId { get; set; }

    public string? StateName { get; set; }

    public virtual ICollection<TicketChangeHistory> TicketChangeHistories { get; set; } = new List<TicketChangeHistory>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
