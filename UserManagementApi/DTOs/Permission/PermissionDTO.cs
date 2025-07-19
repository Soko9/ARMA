namespace UserManagementApi.DTOs.Permission
{
    public class PermissionDTO
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public Guid? LastActionUserId { get; set; }

        public Guid PermissionCategoryId { get; set; }
    }
}
