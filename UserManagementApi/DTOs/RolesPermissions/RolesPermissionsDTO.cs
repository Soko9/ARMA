namespace UserManagementApi.DTOs.RolesPermission
{
    public class RolesPermissionsDTO
    {
        public required List<Guid> PermissionsIds { get; set; }

        public Guid? LastActionUserId { get; set; }
    }
}
