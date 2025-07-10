namespace UserManagementApi.Models;

public partial class Log
{
    public Guid LogId { get; set; }

    public string ActionType { get; set; } = null!;

    public string? Description { get; set; }

    public string ActionTableName { get; set; } = null!;

    public Guid ActionRowId { get; set; }

    public Guid? ActionUserId { get; set; }

    public DateTime ActionAt { get; set; }

    public virtual User? ActionUser { get; set; }
}
