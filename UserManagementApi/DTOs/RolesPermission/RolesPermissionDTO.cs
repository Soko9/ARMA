namespace UserManagementApi.DTOs.RolesPermission
{
    public class RolesPermissionDTO
    {
        public Guid RoleId { get; set; }

        public Guid PermissionId { get; set; }

        public Guid LastActionUserId { get; set; }
    }
}
