namespace UserManagementApi.Models;

public partial class Role
{
    public Guid RoleId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public byte Priority { get; set; }

    public bool IsVisible { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid LastActionUserId { get; set; }

    public virtual User LastActionUser { get; set; } = null!;

    public virtual ICollection<RolesPermission> RolesPermissions { get; set; } = new List<RolesPermission>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
