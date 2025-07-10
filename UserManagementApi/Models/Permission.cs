namespace UserManagementApi.Models;

public partial class Permission
{
    public Guid PermissionId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsVisible { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid LastActionUserId { get; set; }

    public Guid PermissionCategoryId { get; set; }

    public virtual User LastActionUser { get; set; } = null!;

    public virtual PermissionCategory PermissionCategory { get; set; } = null!;

    public virtual ICollection<RolesPermission> RolesPermissions { get; set; } = new List<RolesPermission>();
}
