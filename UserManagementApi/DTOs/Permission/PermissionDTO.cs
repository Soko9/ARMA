namespace UserManagementApi.DTOs.Permission
{
    public class PermissionDTO
    {
        public Guid? PermissionId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public Guid? LastActionUserId { get; set; }

        public Guid PermissionCategoryId { get; set; }
    }
}
