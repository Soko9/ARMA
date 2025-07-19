namespace UserManagementApi.DTOs.PermissionCategory
{
    public class PermissionCategoryDTO
    {
        public string Title { get; set; } = null!;

        public byte Priority { get; set; }

        public Guid? LastActionUserId { get; set; }
    }
}
