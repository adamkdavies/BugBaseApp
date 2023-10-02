using System;
using System.Collections.Generic;

namespace BugBaseApp.Models;

public partial class TicketChangeType
{
    public long TicketChangeTypeId { get; set; }

    public string? TicketChangeTypeName { get; set; }

    public virtual ICollection<TicketChangeHistory> TicketChangeHistories { get; set; } = new List<TicketChangeHistory>();
}
