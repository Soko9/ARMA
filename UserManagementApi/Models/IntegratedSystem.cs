using System;
using System.Collections.Generic;

namespace UserManagementApi.Models;

public partial class IntegratedSystem
{
    public Guid SystemId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? LastActionUserId { get; set; }

    public virtual ICollection<IpwhiteList> IpwhiteLists { get; set; } = new List<IpwhiteList>();

    public virtual User? LastActionUser { get; set; }
}
