namespace UserManagementApi.DTOs.User
{
    public class UserDTO
    {
        public Guid? UserId { get; set; }

        public string FullName { get; set; } = null!;

        public string? Password { get; set; } = null!;

        public Guid? LastActionUserId { get; set; }

        public Guid RoleId { get; set; }
    }
}
