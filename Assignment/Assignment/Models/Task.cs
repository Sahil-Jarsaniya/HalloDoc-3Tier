using System;
using System.Collections.Generic;

namespace Assignment.Models;

public partial class Task
{
    public int Id { get; set; }

    public string TaskName { get; set; } = null!;

    public string? Assignee { get; set; }

    public int CategoryId { get; set; }

    public string? Description { get; set; }

    public DateOnly? DueDate { get; set; }

    public string? Category { get; set; }

    public string? City { get; set; }

    public virtual Category CategoryNavigation { get; set; } = null!;
}
