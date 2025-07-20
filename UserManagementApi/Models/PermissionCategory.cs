using System;
using System.Collections.Generic;

namespace UserManagementApi.Models;

public partial class PermissionCategory
{
    public Guid PermissionCategoryId { get; set; }

    public string Title { get; set; } = null!;

    public byte Priority { get; set; }

    public bool IsVisible { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? LastActionUserId { get; set; }

    public virtual User? LastActionUser { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
